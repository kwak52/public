
[<AutoOpen>]
module Dsu.Driver.NiDaqParams

open System
open NationalInstruments
open NationalInstruments.DAQmx

let mega = 1000000.0


let private selectUniqueDevice(channelNames) =
    let distinctDevices = channelNames |> Seq.choose NiDaqHwLocal.ParseChannelName |> Seq.map fst |> Seq.distinct
    if Seq.length distinctDevices <> 1 then
        failwithlog (sprintf "No unique device specified on channel description: %A" channelNames)
    let distinctDevice = Seq.item 0 distinctDevices
    NiDaqHwLocal.GetDevice(distinctDevice)

let private verifyChannels(channelNames) =
    if Seq.isEmpty channelNames then
        failwithlog "No channel specified."

    let invalidChannels = NiDaqHwLocal.SelectInvalidChannels(channelNames)
    if not (Seq.isEmpty invalidChannels) then
        failwithlog (sprintf "Invalid channel specified: %A" invalidChannels)

    for channelName in channelNames do
        match NiDaqHwLocal.ParseChannelName(channelName) with
        | Some(dev, _) ->
            if not (Array.contains dev NiDaqHwLocal.DeviceNames) then
                failwithlog (sprintf "Device %s not exists." dev)
        | _ -> failwithlog (sprintf "Invalid channel name %s" channelName)

/// <summary>
/// Single/Per Channel Parameters
/// </summary>
[<AbstractClass>]
type DaqScParams(channel:string, min:double, max:double) =
    do
        verifyChannels([channel])
    /// channel name : e.g "Dev5/ai0"
    member val Channel = channel with get

    /// Min value for CreateVoltageChannel.  default = -10.0
    member val Min = min with get, set
    /// Max value.  default = +10.0
    member val Max = max with get, set

    member x.LogInfo() =
        logInfo "\t-- Channel %s DAQ Parameters.." x.Channel
        logInfo "\t\tMin=%f, Max=%f" min max


        
/// <summary>
/// Single/Per Channel AI Parameters
/// </summary>
type DaqScAiParams(channel, min, max, terminalConfiguration:AITerminalConfiguration, voltageUnits:AIVoltageUnits) =
    inherit DaqScParams(channel, min, max)
    /// Voltage unit.  default = AIVoltageUnits.Volts
    member val VoltageUnits = voltageUnits with get, set
    /// Terminal configuration.  default = -1
    member val TerminalConfiguration = terminalConfiguration with get, set      // enum -1
    /// AI coupling.  default = AICoupling.DC
    member val AICoupling = AICoupling.DC with get, set
    /// Low pass filter enable.  default = false
    member val LowpassEnable = false with get, set
    /// Low pass cut-off frequency.  default = 500K Hz
    member val LowpassCutoffFrequency = 500.0 * 1000.0 with get, set
    new(channel) = new DaqScAiParams(channel, -10.0, +10.0, (enum -1), AIVoltageUnits.Volts)

/// <summary>
/// Single/Per Channel AO parameters
/// </summary>
type DaqScAoParams(channel, min, max, pattern:double seq, voltageUnits:AOVoltageUnits) =
    inherit DaqScParams(channel, min, max)
    let pattern = pattern |> Array.ofSeq
    /// Voltage unit.  default = AOVoltageUnits.Volts
    member val VoltageUnits = voltageUnits with get, set
    member val Pattern = pattern with get
    new(channel, pattern) = new DaqScAoParams(channel, -10.0, +10.0, pattern, AOVoltageUnits.Volts)


/// <summary>
/// Multi-Channel parameters
/// </summary>
[<AbstractClass>]
type DaqMcParams private (perChannelParameters:seq<DaqScParams>) = 
    member val internal ChannelParameters = Array.ofSeq perChannelParameters with get
    member x.GetChannelParameter(channel) = x.ChannelParameters |> Array.find(fun ch -> ch.Channel = channel)
    /// Physical channel 이름.  e.g seq {"Dev5/ai0", "Dev5/ai1", "Dev5/ai2"}
    member val ChannelNames = (perChannelParameters |> Seq.map (fun chParam -> chParam.Channel) |> Array.ofSeq) with get, set
    /// 초당 수집할 샘플의 수.  NI DAQ board 지원 최대치는 사양에 따라 다르다.
    abstract SamplingRate:double with get, set
    /// 1회 scan에서 수집할 샘플의 수
    member val NumberOfSamples = 500000 with get, set

    member val SampleClockActiveEdge = SampleClockActiveEdge.Rising with get, set
    member val SampleQuantityMode = SampleQuantityMode.ContinuousSamples with get, set

    member x.LogInfo() =
        logInfo "\tSampling Rate=%f, Number of samples = %d" x.SamplingRate x.NumberOfSamples
        perChannelParameters |> Seq.iter (fun p -> p.LogInfo())

    internal new (perChannelParameters:seq<DaqScAiParams>) = 
        let casted : seq<DaqScParams> = perChannelParameters |> Seq.cast
        new DaqMcParams(casted)
    internal new (perChannelParameters:seq<DaqScAoParams>) = 
        let casted : seq<DaqScParams> = perChannelParameters |> Seq.cast
        new DaqMcParams(casted)



/// <summary>
/// 다채널 DAQ 획득을 위한 parameter 설정
/// </summary>
type DaqMcAiParams(perChannelParameters:seq<DaqScAiParams>) as this = 
    inherit DaqMcParams(perChannelParameters)
    let device = perChannelParameters |> Seq.map (fun p -> p.Channel) |> selectUniqueDevice
    let hwMax = device.AIMaximumMultiChannelRate
    let mutable _samplingRate = hwMax / 10.0        // hwMax 의 10% 만 사용

    override __.SamplingRate
        with get() = _samplingRate
        and set(v) = 
            if v > hwMax then
                failwithlog (sprintf "Sampling rate specification error. Allow H/W maximum is %f, but you specified %f." hwMax v)
            _samplingRate <- v
    member val AiChannelParameters = this.ChannelParameters |> Seq.cast<DaqScAiParams> |> Array.ofSeq

    /// 1회 scan 에 걸리는 시간
    member x.ScanTimeMilli with get() = 1000.0 * double(x.NumberOfSamples) / _samplingRate 

    member val ReadOverwriteMode = ReadOverwriteMode.DoNotOverwriteUnreadSamples with get, set
    member val ReadWaitMode = ReadWaitMode.Poll with get, set

    member x.LogInfo() =
        logInfo "-- Multichannel DAQ Parameters.."
        logInfo "\tScan time=%f(ms)" x.ScanTimeMilli
        base.LogInfo()


/// <summary>
/// AO 출력 parameters
/// </summary>
type DaqMcAoParams(perChannelParameters:seq<DaqScAoParams>) as this = 
    inherit DaqMcParams(perChannelParameters)
    let device = perChannelParameters |> Seq.map (fun p -> p.Channel) |> selectUniqueDevice
    let hwMax = device.AOMaximumRate
    let mutable _samplingRate = hwMax
    do
        let lengthArray = perChannelParameters |> Seq.map( fun p -> p.Pattern.Length) |> Array.ofSeq
        let allEqualLength = lengthArray |> Array.forall(fun l -> l = lengthArray.[0])
        if not allEqualLength then
            failwithlog "Wave pattern length mismatch.  They should have the same length across all channel."

    // 출력 pattern
    let patterns = 
        let pats = perChannelParameters |> Seq.map (fun p -> p.Pattern) |> Array.ofSeq
        Array2D.init pats.Length pats.[0].Length (fun x y -> pats.[x].[y])

    override __.SamplingRate
        with get() = _samplingRate
        and set(v) =
            if v > hwMax then
                failwithlog (sprintf "Sampling rate specification error. Allow H/W maximum is %f, but you specified %f." hwMax v)
            _samplingRate <- v

    member val Patterns = patterns with get
    member val AoChannelParameters = this.ChannelParameters |> Seq.cast<DaqScAoParams> |> Array.ofSeq
    new (perChannelParameters) = new DaqMcAoParams(perChannelParameters)       // NI-DAQ-6115 의 경우, AO 의 최대치 4MS/s



/// <summary>
/// NI DAQ Multi-channel AI 에 대한 기본 parameter 를 생성한다.
/// </summary>
/// <param name="channelPrefix">e.g "Dev5/ai"</param>
/// <param name="numChannel"></param>
let createDefaultMcAiParameter(channelNames:string seq) =
    verifyChannels(channelNames)
    let perChannelParameters = channelNames |> Seq.map(fun ch -> new DaqScAiParams(ch)) |> Array.ofSeq
    new DaqMcAiParams(perChannelParameters)


/// <summary>
/// NI DAQ Multi-channel AO 에 대한 기본 parameter 를 생성한다.
/// </summary>
/// <param name="channelPrefix">e.g "Dev5/ai"</param>
/// <param name="numChannel"></param>
let internal createDefaultMcAoParameter(channelNames:string seq, pattern) =
    verifyChannels(channelNames)
    let perChannelParameters = 
        channelNames |> Seq.map (fun ch -> new DaqScAoParams(ch, pattern)) 
    new DaqMcAoParams(perChannelParameters)

