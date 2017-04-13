/// NI DAQ Multi-channel AI reader
module Dsu.Driver.NiDaqMcAi

open System
open DriverExcpetionModule
open Dsu.Driver.NiDaqMcAiBase
open Dsu.Driver.Base

type DaqMcAiManager (parameters:DaqMcAiParams) =
    inherit DaqMcAiManagerBase(parameters)

/// <summary>
/// DAQ multichannel manager.(Singleton)
/// </summary>
let mutable private daqMcAiManagerSingleton: DaqMcAiManager option = None

/// <summary>
/// NI DAQ manager 객체에 대한 interface 를 생성한다.
/// </summary>
let rec createManager parameters =
    try
        if daqMcAiManagerSingleton.IsNone then

            let daq = new DaqMcAiManager(parameters)

            daq.ProcException <- fun exn ->
                logError "DAQ exception occurred: %O" exn
            daqMcAiManagerSingleton <- Some(daq)
    with exn ->
        DriverExceptionSubject.OnNext(DaqException(exn))

let createManagerSimple(channelNames) = createManager (createDefaultMcAiParameter(channelNames))

/// <summary>
/// Returns DAQ exception safe singleton instance
/// daq 다채널 manager 에서 읽기 스레드에서 exception 이 발생하는 경우, 새로운 객체를 생성하도록 하여 반환한다.
/// </summary>
let manager() = daqMcAiManagerSingleton.Value
let managerSingleton() = daqMcAiManagerSingleton



//do
//    NiDaqMcBase.daqManageSubject.Subscribe(fun msg -> 
//        match msg with
//        | "Disposed" -> daqMcAiManagerSingleton <- None
//        | _ -> ()
//    ) |> ignore
////    logInfo "Daq MultiChannel manager created."
////    createManagerSimple()
//
