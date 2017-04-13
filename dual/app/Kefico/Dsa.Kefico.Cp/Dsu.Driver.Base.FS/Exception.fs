[<AutoOpen>]
module Dsu.Driver.DriverExcpetionModule

open System
open System.Reactive.Subjects
open Dsu.Common.Utilities
open Dsu.Common.Utilities.ExceptionUtils

/// <summary>
/// 오류 발생시, 더 이상 retry 해 볼 가능성이 없는 exception
/// </summary>
type UnrecovervableException(msg, exn) =
    inherit ExceptionWrapper(msg, exn)
    new(exn:Exception) = new UnrecovervableException(exn.Message, exn)
        

type DriverException =
    | SorensenException of Exception
    | UpsException of Exception
    | DaqException of Exception     // NationalInstruments.DAQmx.DAQException
    | PaixException of Exception
    | HiokiException of Exception
    //| FatalException of DriverException
    override x.ToString() =
        match x with
        | SorensenException(ex) -> sprintf "Sorensen Exception:\r\n%s" (ex.ToString())
        | UpsException(ex) -> sprintf "UPS Exception:\r\n%s" (ex.ToString())
        | DaqException(ex) -> sprintf "DAQ Exception:\r\n%s" (ex.ToString())
        | PaixException(ex) -> sprintf "Paix Exception:\r\n%s" (ex.ToString())
        | HiokiException(ex) -> sprintf "Hioki Exception:\r\n%s" (ex.ToString())
        //| FatalException(ex) -> sprintf "FatalException Exception:\r\n%s" (ex.ToString())
    member x.OriginalException =
        match x with 
        | SorensenException(ex) -> ex
        | UpsException(ex) -> ex
        | DaqException(ex) -> ex
        | PaixException(ex) -> ex
        | HiokiException(ex) -> ex
        //| FatalException(ex) -> ex.OriginalException


type IDeviceDriver =
    abstract ProcException: (Exception -> unit) with get, set

let DriverExceptionSubject = new Subject<DriverException>()


let mutable internal driverExceptionSubscription:IDisposable = null
do
    driverExceptionSubscription <- DriverExceptionSubject.Subscribe(fun drvexn ->
        logError "%s" (drvexn.ToString()))

