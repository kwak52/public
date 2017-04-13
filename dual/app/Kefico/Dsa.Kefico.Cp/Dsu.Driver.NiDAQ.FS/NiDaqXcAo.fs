/// NI DAQ Single/Multi Channel AO(Analog Output) module
module Dsu.Driver.NiDaqXcAo

open System
open NationalInstruments
open NationalInstruments.DAQmx
open DriverExcpetionModule

type DaqMcAoManager (parameters:DaqMcAoParams) as this =
    inherit DaqMcManagerBase(parameters)
    let mutable analogOutWriter:AnalogMultiChannelWriter = null
    let numberOfSamples = parameters.NumberOfSamples

    do
        parameters.LogInfo()
        if not (NiDaqBase.Check()) then
            raise (new Exception("No Daq board found!"))

        // background task 로 board 에서 DAQ 값을 계속 읽어 들임
        let bgTask = 
            async {
                try
                    this.Init()
                with exn ->
                    this.OnException exn
            }
        Async.Start(bgTask, this.CancellationTokenSource.Token)

    /// background 다채널 DAQ 수집을 위한 작업 수행.  see ContAcqVoltageSamples_IntClk.2010 sample
    override x.Init() =
        let task = x.McTask
        parameters.AoChannelParameters
        |> Seq.iter(fun p -> 
            task.AOChannels.CreateVoltageChannel(p.Channel, "", p.Min, p.Max, p.VoltageUnits) |> ignore )


        // Configure the timing parameters
        task.Timing.ConfigureSampleClock("", parameters.SamplingRate,
            parameters.SampleClockActiveEdge,
            parameters.SampleQuantityMode,
            numberOfSamples)

        task.Timing.SampleTimingType <- SampleTimingType.SampleClock;
        // Specifies the sampling rate in samples per channel per second.
        // task.Timing.SampleClockRate <- 50000.0       // == parameters.SamplingRate
         

        // Verify the Task
        task.Control(TaskAction.Verify)

        analogOutWriter <- new AnalogMultiChannelWriter(task.Stream)
        analogOutWriter.WriteMultiSample(false, parameters.Patterns)
        task.Start()


/// <summary>
/// NI DAQ AO manager 객체에 대한 interface 를 생성한다.
/// </summary>
let rec createManager parameters =
    try
        let daq = new DaqMcAoManager(parameters)

        // daq 다채널 manager 에서 쓰기 스레드에서 exception 이 발생하는 경우의 처리
        daq.ProcException <- fun exn -> daq.Dispose()
        Some(daq)
    with exn ->
        DriverExceptionSubject.OnNext(DaqException(exn))
        None

let createManagerSimple(channelNames, pattern) = createManager (createDefaultMcAoParameter(channelNames, pattern))
