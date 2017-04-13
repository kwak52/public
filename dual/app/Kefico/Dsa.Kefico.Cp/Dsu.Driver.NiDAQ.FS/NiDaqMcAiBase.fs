module Dsu.Driver.NiDaqMcAiBase

open System
open System.Threading
open System.Reactive.Subjects
open NationalInstruments
open NationalInstruments.DAQmx
open Dsu.Driver.Base
open Dsu.Driver.NiDaqMcBase
open System.Collections.Concurrent

/// <summary>
/// 다채널 DAQ 관리자.
/// 생성자에서 thread 를 생성해서 DAQ board 에서 다채널에 대해서 계속 읽어 들이면서 event 를 공지한다.
/// 특정 채널의 데이터를 획득하려면 AsyncCollect 메서드를 이용한다.
/// </summary>
[<AbstractClass>]
type DaqMcAiManagerBase (parameters:DaqMcAiParams) as this =
    inherit DaqMcManagerBase(parameters)
    let mutable analogInReader:AnalogMultiChannelReader = null
    let mutable counter = 0L     // debugging purpose, only
    let mutable pause = false
    let numberOfSamples = parameters.NumberOfSamples
    let daqSubject = new Subject<DaqMessageType>()   // thread 에서 보드를 통해 DAQ 를 읽었음을 공지하기 위한 subject

    let bufferCache = new ConcurrentDictionary<string, ConcurrentBag<double array array>>()
    let allocBuffer(numChannel:int, numSamples:int) =
        let key = sprintf "%d:%d" numChannel numSamples
        //let ok, bag = bufferCache.TryGetValue(key)
        let allocate() =
            // 다채널 샘플링 저장을 위한 2D double array.  { 채널 당 x 샘플 수 } 의 2d array
            let mcSamples: double array array = Array.zeroCreate numChannel

            // 각 채널별로 sampling 갯수 만큼의 공간 확보
            for ch in 0..numChannel-1 do
                mcSamples.[ch] <- Array.zeroCreate numSamples
            
            mcSamples

        let dictOk, bag = bufferCache.TryGetValue(key)
        if dictOk then
            let ok, buffer = bag.TryTake()
            if ok then buffer else allocate()
        else
            bufferCache.TryAdd(key, new ConcurrentBag<double array array>()) |> ignore
            allocate()

    let returnBuffer(mcSamples:double array array) =
        let key = sprintf "%d:%d" mcSamples.Length mcSamples.[0].Length
        if not (bufferCache.ContainsKey(key)) then
            bufferCache.TryAdd(key, new ConcurrentBag<double array array>()) |> ignore

        bufferCache.[key].Add(mcSamples)



    let rec startBackgroundDaqService() = 
        parameters.LogInfo()
        if not (NiDaqBase.Check()) then
            raise (new Exception("No Daq board found!"))

        logInfo "(Re)starting multichannel DAQ service."

        // background task 로 board 에서 DAQ 값을 계속 읽어 들임
        let bgTask = 
            async {
                try
                    this.Init()
                with exn ->
                    this.OnException exn
                    try
                        this.StopTask()
                    with ex' ->
                        logError "DAQ: Failed to stop task. Exception:%O" ex'

                    daqSubject.OnNext(Error(exn))
                    startBackgroundDaqService()     // starts again
            }

        this.ResetCancellationTokenSource()         // !!! MUST HAVE: if not, will not restart DAQ service
        Async.Start(bgTask, this.CancellationTokenSource.Token)

    // channel 당 voltage offset 값: 일반적으로 0.0
    static let voltageOffsets:float array = Array.zeroCreate 4

    do
        // driver 초기화 시 모든 offset 값을 reset
        DriverBaseGlobals.DriverResetSubject.Subscribe(fun obj ->
            {0..3} |> Seq.iter(fun i -> voltageOffsets.[i] <- 0.0)) |> ignore

        logInfo "Creating Multichannel DAQ service manager."
        startBackgroundDaqService()

    /// Channel 의 voltage offset 값 설정
    static member SetChannelVoltageOffset(ch, offset) = voltageOffsets.[ch] <- offset

    /// background 다채널 DAQ 수집을 위한 작업 수행.  see ContAcqVoltageSamples_IntClk.2010 sample
    override x.Init() =
        base.Init()
        let task = x.McTask

        let channelAndParameters =
            parameters.AiChannelParameters
            |> Seq.map(fun p -> 
                let aiChannel = task.AIChannels.CreateVoltageChannel(p.Channel, "", p.TerminalConfiguration , p.Min, p.Max, p.VoltageUnits)                
                aiChannel, p)
            |> Array.ofSeq      // fix it

        // test runtime change : Can't change parameter while task is running 
//        async {
//            do! Async.Sleep(3000)
//            channelAndParameters |> Seq.iter (fun (channel, _) ->
//                channel.Maximum <- +2.0
//                channel.Minimum <- +2.0)
//        } |> Async.Start

        // Configure the timing parameters
        task.Timing.ConfigureSampleClock("", parameters.SamplingRate,
            parameters.SampleClockActiveEdge,
            parameters.SampleQuantityMode,
            numberOfSamples)

        // Verify the Task
        task.Control(TaskAction.Verify)

        channelAndParameters
        |> Seq.iter(fun (channel, p) ->
            try
                channel.Coupling <- p.AICoupling               // AICoupling.AC
                let model = 
                    let parsed = NiDaqHwLocal.ParseChannelName(p.Channel)
                    if parsed.IsSome then
                        let deviceName = fst parsed.Value
                        let device = NiDaqHwLocal.GetDevice(deviceName)
                        device.ProductType.ToString()
                    else
                        ""
                if model.Contains("6115") then
                    channel.LowpassEnable <- p.LowpassEnable       // true
                    if(p.LowpassEnable) then
                        logInfo "DAQ: Enabling lowpass filter: cutoff frequency=%.2f" channel.LowpassCutoffFrequency
                        channel.LowpassCutoffFrequency <- p.LowpassCutoffFrequency    // 500.0 * 1000.0
            with exn ->
                logError "DAQ: Failed to initialize channel %s: %O" p.Channel exn
                System.Diagnostics.Debug.Assert(false);
        )

        // Specifies whether to overwrite samples in the buffer that you have not yet read.
        task.Stream.ReadOverwriteMode <- parameters.ReadOverwriteMode
        task.Stream.ReadWaitMode <- parameters.ReadWaitMode;
        //task.Stream.ReadRelativeTo <- ReadRelativeTo.FirstPretriggerSample


        analogInReader <- new AnalogMultiChannelReader(task.Stream)

        // Use SynchronizeCallbacks to specify that the object 
        // marshals callbacks across threads appropriately.
        analogInReader.SynchronizeCallbacks <- true

        base.TaskStarted <- true
        logInfo "DAQ: Sampling rate=%.0f, number of samples=%d" parameters.SamplingRate numberOfSamples

        while not x.CancellationTokenSource.IsCancellationRequested do
            if not pause then
                // ----------------------------------------------------
                // Thread 상에서 동기적으로 DAQ 를 읽는다.
                // ----------------------------------------------------
                let data = analogInReader.ReadWaveform(numberOfSamples)
                //
                //

                counter <- counter + 1L
                if counter % 1000L = 0L then
                    printfn "%d-th" counter

                //
                // 동기적으로 읽은 결과를 비동기적으로 event notify 한다.
                //
                async {
                    //printfn "Reading buffer %d-th" counter
                    if counter % 10000L = 0L then
                        let interval = data.[0].PrecisionTiming.SampleInterval
                        logInfo "Iterating %d-th loop on %A.  Sample interval=%f(ms)" counter PrecisionDateTime.Now interval.TotalMilliseconds


    //                let now = PrecisionDateTime.Now
    //                let hwScanTime = data.[0].PrecisionTiming.TimeStamp     // same for all channel
    //                let hwSampleInterval = data.[0].PrecisionTiming.SampleInterval

                    // DAQ board 에서 data 를 읽었음을 공지
                    daqSubject.OnNext(MultiChannelReadResult(data, voltageOffsets))

                    data |> Seq.iteri (fun i d -> x.PerChannelSubjectArray.[i].OnNext(d.GetRawData(), voltageOffsets.[i]))
                

                } |> Async.Start

                //analogInReader.MemoryOptimizedReadWaveform(numSamples, data) |> ignore
        logInfo "DAQ Task cancelled."

    /// Force reset daq task
    member x.ResetDaqTask() =
        logInfo "Request DAQ Task cancellation."
//        x.CancellationTokenSource.Cancel()
//        pause <- true
//        x.ResetCancellationTokenSource()
//        System.Threading.Thread.Sleep(1)
//        startBackgroundDaqService()
//        pause <- false

        if base.TaskStarted then
            let task = x.McTask
            pause <- true
            task.Stream.ReadAllAvailableSamples <- true
            analogInReader.ReadWaveform(-1) |> ignore
            task.Stream.ReadAllAvailableSamples <- false
            pause <- false
            true
        else
            false


    /// 사용이 끝난 sample buffer 반환
    member x.ReturnBuffer(mcSamples) = returnBuffer(mcSamples)
    member x.ReturnBuffer(scSamples:double array) =
        let mcSamples: double array array = Array.zeroCreate 1
        mcSamples.[0] <- scSamples
        returnBuffer(mcSamples)

    /// <summary>
    /// 주어진 복수개의 channel 에 대해 numSamples 갯수만큼 읽어 들인 후에 결과를 반환
    /// </summary>
    /// <param name="channels">읽어 들일 channel 이름의 seqeunce</param>
    /// <param name="numSamples">채널 당 읽어 들일 샘플 수</param>
    member x.AsyncCollect(channels:string seq, numSamples) = x.AsyncCollect(channels, numSamples, dummyCancellationToken)

    /// <summary>
    /// 주어진 복수개의 channel 에 대해 numSamples 갯수만큼 읽어 들인 후에 결과를 반환
    /// </summary>
    /// <param name="channels">읽어 들일 channel 이름의 seqeunce</param>
    /// <param name="numSamples">채널 당 읽어 들일 샘플 수</param>
    /// <param name="externalCTS">Cancellation token</param>
    member x.AsyncCollect(channels:string seq, numSamples, cancellationToken:CancellationToken) =
        async {
            // channel 이름 별로 data.[] 에서 몇 번째 index 인지를 map 으로 구성
            let channelMap = channels |> Seq.mapi (fun i ch -> i, x.ChannelDict.[ch]) |> Map.ofSeq
            let numChannel = Seq.length channels

            // 다채널 샘플링 저장을 위한 2D double array.  { 채널 당 x 샘플 수 } 의 2d array
            let mcSamples = allocBuffer(numChannel, numSamples)

            // 수집된 sample 갯수
            let mutable collected = 0
            let mutable subscription:IDisposable  = null

            // 수집 완료를 기다리기 위한 tcs
            let tcs = new Tasks.TaskCompletionSource<bool>(false)


            // 다채널 DAQ 에서 data 를 읽었다는 event 를 받기 위한 subscription 등록
            subscription <- daqSubject.Subscribe(fun daqMsg -> 
                match daqMsg with
                | MultiChannelReadResult(data, voltageOffsets) ->
                    let rawData = data |> Array.map (fun d -> d.GetRawData())
                    let numReadSamples = rawData.[0].Length

                    let indexBase = collected
                    // 읽은 sample 들에 대해서, 아직 수집 완료 목표치가 달성되지 않은 동안 계속 수집
                    for i in 0..numReadSamples-1 do
                        if indexBase + i < numSamples then
                            for n in 0..numChannel-1 do
                                let dataCh = channelMap.[n]
                                let offset = voltageOffsets.[dataCh]    // channel 의 voltage offset 값
                                mcSamples.[n].[indexBase + i] <- offset + rawData.[dataCh].[i]
                            collected <- collected + 1

                    // 수집이 완료된 경우
                    if (collected >= numSamples || cancellationToken.IsCancellationRequested) then
                        // self unsubscription.
                        subscription.Dispose()

                        // 수집 완료를 공지
                        tcs.TrySetResult(true) |> ignore
                | Error(ex) ->
                    collected <- 0      // 에러 발생시, 재 수집
            )

            // 수집 완료까지 기다렸다가 결과를 return
            let! result = Async.AwaitTask tcs.Task

            return mcSamples
        }

    /// <summary>
    /// 주어진 하나의 channel 에 대해 numSamples 갯수만큼 읽어 들인 후에 결과를 반환
    /// </summary>
    /// <param name="channel">읽어 들일 channel 이름</param>
    /// <param name="numSamples">읽어 들일 샘플 수</param>
    member x.AsyncCollect(channel:string, numSamples, externalCTS:CancellationToken) =
        async {
            let channels = [| channel |]
            let! mcSamples = x.AsyncCollect( channels, numSamples, externalCTS)
            return mcSamples.[0]
        }
    member x.AsyncCollect(channel:string, numSamples) = x.AsyncCollect(channel, numSamples, dummyCancellationToken)

    /// DAQ Single channel manager 에서 해당 channel 에서 DAQ acquisition event 를 받고자 할 때에 사용한다.
    member internal x.PerChannelSubject(channel) = x.PerChannelSubjectArray.[x.IndexOfChannel(channel)]
    member internal x.IndexOfChannel(channel) = x.ChannelDict.[channel]



    /// McDaqManager 에서 exception 이 발생했음을 simulate.  Debugging purpose only
    member x.CrashMe() =
        //onException(new Exception("Intended exception, for debugging"))
        ()


    new (physicalChannels) =
        let parameters = new DaqMcAiParams(physicalChannels)
        new DaqMcAiManagerBase(parameters)


