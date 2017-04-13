[<AutoOpen>]
module Dsu.Driver.NiDaqMcBase

open System
open System.Threading
open System.Reactive.Subjects
open NationalInstruments
open NationalInstruments.DAQmx
open DriverExcpetionModule

[<AbstractClass>]
type DaqMcManagerBase (parameters:DaqMcParams) as this =
    let channelDict =   // string -> int map (channel name -> index)
        parameters.ChannelNames
        |> Seq.mapi(fun i c -> c, i)
        |> Map.ofSeq

    let numberOfChannels = parameters.ChannelNames.Length

    let mutable task = new DAQmx.Task("")
    let mutable taskStarted = false
    let mutable cts:CancellationTokenSource = new CancellationTokenSource()     // Thread 를 멈추기위한 task completion source

    let stopTask() =
        if task <> null && taskStarted then
            logInfo "DAQ: Stopping running task."
            cts.Cancel()
            try
                try
                    task.Stop()
                    task.Dispose()
                with ex' ->
                    logWarn "DAQ: Task dispose exception:%O" ex'
            with exn ->
                logWarn "DAQ: Exception while stopping task: %O" exn
            task <- null
            taskStarted <- false
            logInfo "DAQ: Task stopped."

    let init() =
        stopTask()
        logInfo "DAQ: Creating new task."
        task <- new DAQmx.Task("")
        this.ResetCancellationTokenSource()

    let dispose() =
        cts.Cancel()
        stopTask()

    let perChannelSubject = [| for i in 0..numberOfChannels-1 do yield new Subject<float array * float>() |]        // 각 채널의 {data array * voltage offset} tuple 

    let procException:(Exception -> unit) = fun exn -> ()

    member internal __.ResetCancellationTokenSource() = cts <- new CancellationTokenSource()
    abstract member Init: unit -> unit
    default __.Init() = init()
    member internal __.StopTask() = stopTask()

    member internal __.TaskStarted with get() = taskStarted and set(v) = taskStarted <- v
    member val internal ChannelDict = channelDict with get
    member val internal PerChannelSubjectArray = perChannelSubject with get
    member internal __.CancellationTokenSource = cts
    // Exception 발생 시, 호출되는 routine
    member internal x.OnException(exn:Exception) =
        logError "------ Exception: %O" exn
        this.ProcException(exn)     // DaqMcAiManager 에 등록된 exception 발생시의 routine 을 먼저 수행
        match exn with
        | :? DaqException as ex ->
            match ex.Error with
            | -200222 -> logError "DAQ: Acquisition has been stopped to prevent an input buffer overwrite."
            | -200279 -> logError "DAQ: The application is not able to keep up with the hardware acquisition."
            | _ -> logError "DAQ: Unknown DaqException with code %d" ex.Error
            printfn "%O" ex
        | _ ->
            logError "Unknown Exception type %s\r\n%O" (exn.GetType().ToString()) exn


    /// <summary>
    /// McDaqManager 에서 exception 이 발생했을 때 수행할 사용자의 action 등록
    /// </summary>
    member val ProcException:(Exception -> unit) = procException with get, set

    member x.Dispose() = dispose()

    member val Parameters = parameters with get

    /// DAQ data 수집을 취소
    member x.Cancel() =
        logInfo "DAQ: Canceled."
        x.CancellationTokenSource.Cancel()

    member __.McTask with get() = task

    interface IDisposable with
        member __.Dispose() = dispose()

    interface IDeviceDriver with
        member val ProcException = procException with get, set

