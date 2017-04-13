module Dsu.Driver.NiDaq

open System
open System.Threading
open System.Reactive.Subjects
open NationalInstruments
open NationalInstruments.DAQmx
open MultiChannelDef
open DriverExcpetionModule

/// <summary>
/// 다채널 DAQ 관리자.
/// 생성자에서 thread 를 생성해서 DAQ board 에서 다채널에 대해서 계속 읽어 들이면서 event 를 공지한다.
/// 특정 채널의 데이터를 획득하려면 AsyncCollectFinite 메서드를 이용한다.
/// </summary>
type DaqMcMananger (parameters:DaqMcParams) as this =
    let mutable task = new Task("")//"DAQMultiChannelManagerTask")
    let mutable analogInReader:AnalogMultiChannelReader = null
    let mutable counter = 0L     // debugging purpose, only
    let numSamples = parameters.NumberOfSamples
    let cts = new CancellationTokenSource()     // Thread 를 멈추기위한 task completion source
    let subject = new Subject<AnalogWaveform<float> []>()   // thread 에서 보드를 통해 DAQ 를 읽었음을 공지하기 위한 subject
    let procException:(Exception -> unit) = fun exn -> ()

    let channelDict =   // string -> int map (channel name -> index)
        parameters.PhysicalChannels
        |> Seq.mapi(fun i c -> c, i)
        |> Map.ofSeq

    let dispose() =
        cts.Cancel()
        if task <> null then
            task.Dispose()
            task <- null

    // Exception 발생 시, 호출되는 routine
    let onException(exn:Exception) =
        this.ProcException(exn)     // DaqMcManager 에 등록된 exception 발생시의 routine 을 먼저 수행
        logError "------ Exception: %O" exn
        dispose()
        match exn with
        | :? DaqException as ex ->
            match ex.Error with
            | -200279 -> logError "The application is not able to keep up with the hardware acquisition."
            | _ -> logError "Unknown DaqException with code %d" ex.Error
            printfn "%O" ex
        | _ ->
            logError "Unknown Exception type %s\r\n%O" (exn.GetType().ToString()) exn

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
                    onException exn
            }
        Async.Start(bgTask, cts.Token)

    member val Parameters = parameters with get

    /// DAQ data 수집을 취소
    member __.Cancel() = cts.Cancel()

    /// background 다채널 DAQ 수집을 위한 작업 수행.  see ContAcqVoltageSamples_IntClk.2010 sample
    member private x.Init() =
        parameters.PhysicalChannels
        |> Seq.iter(fun c -> 
            let mutable tc:AITerminalConfiguration = enum -1
            task.AIChannels.CreateVoltageChannel(c, "", tc , parameters.Min, parameters.Max, parameters.VoltageUnits) |> ignore)

        // Configure the timing parameters
        task.Timing.ConfigureSampleClock("", parameters.SamplingRate,
            SampleClockActiveEdge.Rising,
            SampleQuantityMode.ContinuousSamples,
            numSamples)

        // Verify the Task
        task.Control(TaskAction.Verify)

        // Specifies whether to overwrite samples in the buffer that you have not yet read.
        task.Stream.ReadOverwriteMode <- ReadOverwriteMode.DoNotOverwriteUnreadSamples
        //task.Stream.ReadRelativeTo <- ReadRelativeTo.FirstPretriggerSample
        //task.Stream.ReadWaitMode <- ReadWaitMode.Poll;

        analogInReader <- new AnalogMultiChannelReader(task.Stream)

        // Use SynchronizeCallbacks to specify that the object 
        // marshals callbacks across threads appropriately.
        analogInReader.SynchronizeCallbacks <- true

        while not cts.IsCancellationRequested do
            let data = analogInReader.ReadWaveform(numSamples)
            counter <- counter + 1L
            if counter = 100L then
                printfn "100-th"

            async {
                //printfn "Reading buffer %d-th" counter
                if counter = 2L then
                    printfn "2-th"
                if counter % 10000L = 0L then
                    let interval = data.[0].PrecisionTiming.SampleInterval
                    logInfo "Iterating %d-th loop on %A.  Sample interval=%f(ms)" counter PrecisionDateTime.Now interval.TotalMilliseconds


                let now = PrecisionDateTime.Now
                let hwScanTime = data.[0].PrecisionTiming.TimeStamp     // same for all channel
                let hwSampleInterval = data.[0].PrecisionTiming.SampleInterval

                // DAQ board 에서 data 를 읽었음을 공지
                subject.OnNext(data)

            } |> Async.Start

            //analogInReader.MemoryOptimizedReadWaveform(numSamples, data) |> ignore




    /// <summary>
    /// 주어진 복수개의 channel 에 대해 numSamples 갯수만큼 읽어 들인 후에 결과를 반환
    /// </summary>
    /// <param name="channels">읽어 들일 channel 이름의 seqeunce</param>
    /// <param name="numSamples">채널 당 읽어 들일 샘플 수</param>
    member __.AsyncCollectFinite(channels:string seq, numSamples) =
        async {
            // channel 이름 별로 data.[] 에서 몇 번째 index 인지를 map 으로 구성
            let channelMap = channels |> Seq.mapi (fun i ch -> i, channelDict.[ch]) |> Map.ofSeq
            let numChannel = Seq.length channels

            // 다채널 샘플링 저장을 위한 2D double array.  { 채널 당 x 샘플 수 } 의 2d array
            let mcSamples: double array array = Array.zeroCreate numChannel

            // 각 채널별로 sampling 갯수 만큼의 공간 확보
            for ch in 0..numChannel-1 do
                mcSamples.[ch] <- Array.zeroCreate numSamples

            // 수집된 sample 갯수
            let mutable collected = 0
            let mutable subscription:IDisposable  = null

            // 수집 완료를 기다리기 위한 tcs
            let tcs = new Tasks.TaskCompletionSource<bool>(false)


            // 다채널 DAQ 에서 data 를 읽었다는 event 를 받기 위한 subscription 등록
            subscription <- subject.Subscribe(fun data -> 
                let rawData = data |> Array.map (fun d -> d.GetRawData())
                let numReadSamples = rawData.[0].Length

                let indexBase = collected
                // 읽은 sample 들에 대해서, 아직 수집 완료 목표치가 달성되지 않은 동안 계속 수집
                for i in 0..numReadSamples-1 do
                    if indexBase + i < numSamples then
                        for n in 0..numChannel-1 do
                            let dataCh = channelMap.[n]
                            mcSamples.[n].[indexBase + i] <- rawData.[dataCh].[i]
                        collected <- collected + 1

                // 수집이 완료된 경우
                if collected >= numSamples then
                    // self unsubscription.
                    subscription.Dispose()

                    // 수집 완료를 공지
                    tcs.TrySetResult(true) |> ignore
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
    member x.AsyncCollectFinite(channel, numSamples) =
        async {
            let! mcSamples = x.AsyncCollectFinite([|channel|], numSamples)
            return mcSamples.[0]
        }

    member x.CollectInfinitely(channel:string) : seq<double> =
        // channel 이름 별로 data.[] 에서 몇 번째 index 인지를 map 으로 구성
        let dataCh = channelDict.[channel]
        let queue = new System.Collections.Generic.Queue<double>()
        use subscription = subject.Subscribe(fun data -> 
            let rawData = data |> Array.map (fun d -> d.GetRawData())
            let numReadSamples = rawData.[0].Length
            for i in 0..numReadSamples-1 do
                queue.Enqueue(rawData.[dataCh].[i])
        )

        seq {
            while true do
                if queue.Count > 0 then
                    yield queue.Dequeue()
        }

//        seq {
//            yield! (x.AsyncCollectFinite(channel, x.Parameters.NumberOfSamples) |> Async.RunSynchronously) 
//        }

    /// <summary>
    /// McDaqManager 에서 exception 이 발생했을 때 수행할 사용자의 action 등록
    /// </summary>
    member val ProcException:(Exception -> unit) = procException with get, set

    /// McDaqManager 에서 exception 이 발생했음을 simulate.  Debugging purpose only
    member x.CrashMe() =
        onException(new Exception("Intended exception, for debugging"))

    new (physicalChannels, min, max) =
        let parameters = new DaqMcParams(physicalChannels, min, max)
        new DaqMcMananger(parameters)


    interface IDisposable with
        member __.Dispose() = dispose()

    interface IDeviceDriver with
        member val ProcException = procException with get, set



/// <summary>
/// DAQ multichannel manager.(Singleton)
/// If exception occurs on daq manager, new instance is automatically created.
/// </summary>
let mutable private daqMcManangerSingleton: DaqMcMananger option = None

/// <summary>
/// NI DAQ manager 객체에 대한 interface 를 생성한다.
/// </summary>
let rec createMananger() =
    try
        if daqMcManangerSingleton.IsNone then
            let parameters = 
                let channels = seq { for i in 0..3 do yield sprintf "Dev5/ai%d" i }
                let min, max = -10.0, +10.0
                new DaqMcParams(channels, min, max)            

            let daq = new DaqMcMananger(parameters)

            // daq 다채널 manager 에서 읽기 스레드에서 exception 이 발생하는 경우, 새로운 객체를 생성하도록 하여 반환한다.
            daq.ProcException <- fun exn ->
                daqMcManangerSingleton <- None
                createMananger() |> ignore
            daqMcManangerSingleton <- Some(daq)
    with exn ->
        DriverExceptionSubject.OnNext(DaqException(exn))



/// <summary>
/// Returns DAQ exception safe singleton instance
/// daq 다채널 manager 에서 읽기 스레드에서 exception 이 발생하는 경우, 새로운 객체를 생성하도록 하여 반환한다.
/// </summary>
let manager() =
    daqMcManangerSingleton.Value





type DaqScMananger(channel:string) =
    let getSamplingRatio(swSamplingRate) = 
        let hwSamplingRate = manager().Parameters.SamplingRate      // 10MS/s 로 고정

        if hwSamplingRate < swSamplingRate then
            failwithf "Can't sample with sampling rate larger than %f" hwSamplingRate
        if swSamplingRate = 0. then
            failwithf "Can't sample with zero sampling rate."

        let ratio = hwSamplingRate / swSamplingRate
        swSamplingRate, hwSamplingRate, ratio

    member __.AsyncCollect(samplingRate, numSamples:int) =
        let swSamplingRate, hwSamplingRate, ratio = getSamplingRatio(samplingRate)

        if hwSamplingRate = swSamplingRate then
            manager().AsyncCollectFinite(channel, numSamples)
        else
            async {
                let actualRequiredNumSample = int (ratio * double(numSamples))
                let! data = manager().AsyncCollectFinite(channel, actualRequiredNumSample)
                return data |> Seq.mapi(fun i el -> i, el)
                    |> Seq.filter(fun (i, el) -> i % (int)ratio = 0)
                    |> Seq.map snd
                    |> Array.ofSeq
            }

    member x.CollectInfinitely(samplingRate) =
        let swSamplingRate, hwSamplingRate, ratio = getSamplingRatio(samplingRate)
        manager().CollectInfinitely(channel)
//        if hwSamplingRate = swSamplingRate then
//            manager().CollectInfinitely(channel)
//        else



do
    createMananger()

