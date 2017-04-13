// T:\PDF-Books\시험기\PAIX\Sample\C#\IO\IO\Form1.cs
// T:\PDF-Books\시험기\PAIX\Doc
//  1. NMC2-Library-Manual(V2.0.3).pdf
//      - Error code : pp.18.  0 = NMC_OK
// NMC2.h (C/C++ header for doxygen comment)
// T:\PDF-Books\시험기\PAIX\Sample\TotalDemo(VC2010)\NMC2_TotalDemo.sln 참고 할 것.
namespace Dsu.Driver.Paix

open System
open AppConfig
open Dsu.Driver.Base
open Dsu.Driver.DriverExcpetionModule
open Dsu.Driver.PaixMotionControler

module ModuleBase =
    let NMC_OK = NMC2.NMC_OK
    let mutable paixIp = "192.168.0.11"     // 11, 12

open ModuleBase

/// <summary>
/// PAIX motion controller 관리자. 직접 PaixManager 객체 생성하지 말고 createManager 호출을 통해 PaixManager 객체를 생성할 것.
/// - PAIX 에 동시 접속 가능한 client 는 최대 6개
/// </summary>
type Manager(ip:string, autoOpen) = 
    inherit PaixManagerBase(ip, autoOpen)

    let procException:(Exception -> unit) = fun exn -> ()

    new (ip:string) = new Manager(ip, true)
    new () = new Manager(paixIp, true)

    member x.GetAllParaLogics() =
        {0s..32s} |> Seq.map x.GetParaLogic |> Seq.takeWhile (fun l -> l.IsSome) |> Array.ofSeq
    member x.GetNumberOfAxes() =        //  x.GetAllParaLogics().Length
        match x.GetAxesExpress() with
        | Some(v) -> v.dCmd.Length
        | _ -> 0
          
    /// 지정된 축이 모두 멈추었는지를 반환
    member x.IsAxesStop(axes:int16 seq) =
        axes
            |> Seq.forall (fun ax -> 
                match x.GetBusyStatus(ax) with
                | Some(v) -> v = 0s
                | _ -> false)

    /// 모든 축이 모두 멈추었는지를 반환
    member x.IsAllAxesStop() = x.IsAxesStop([|0s..7s|])

    /// 다축 절대값 이동.  (축, 값) pair 의 seq
    member x.MultiAxesMoveAbs(spec:seq<int16*int64>) =
        let numAxes = Seq.length spec |> int16
        let axes = spec |> Seq.map fst |> Array.ofSeq
        let positions = spec |> Seq.map (snd >> float) |> Array.ofSeq
        x.VarAbsMove numAxes axes positions = NMC_OK

    /// Alarm reset 시 오류 발생한 axis 가 있다면 해당 axis 목록을 반환한다.  성공시 empty list
    /// 주의 사항: alarm clear 수행 시(On Off toggle), 서보 자동으로 꺼졌다 켜지므로, break 가 풀린 상황에서는 흘러 내리게 된다.
    /// 프로그램 code 내에서 alarm clear 하는 일은 없도록 한다.
    /// alarm 상황이 발생하면 사용자에게 공지하고, 전원을 껐다 키도록 한다.
    [<Obsolete("Do not use. ClearAlarm is dangerous.")>]
    member x.ClearAlarm(axes:seq<int16>) =
        axes 
            |> Seq.choose (fun ax -> 
                let r1 = x.SetAlarmResetOn ax 1s
                System.Threading.Thread.Sleep(200)
                let r2 = x.SetAlarmResetOn ax 0s
                match r1, r2 with
                | 0s, 0s -> None
                | _ -> Some(ax))
            |> Array.ofSeq
            
             
    /// <summary>
    /// PAIX Manager 에서 exception 이 발생했을 때 수행할 사용자의 action 등록
    /// </summary>
    member val ProcException:(Exception -> unit) = procException with get, set


    member x.Check() =
        logInfo "-- Paix checking..."
        match x.Ping 10 with
        | true -> "succeeded."
        | _ -> "failed."

        |> logInfo "\tPing test result: %s"

        let home = x.GetHomeStatus()
        logInfo "\tPaix HomeStatus=%A" home

        let enumV = x.GetEnumList()
        logInfo "\tPaix EnumLists=%A" enumV

        match x.GetAxesExpress() with
        | Some(v) ->
            PrintFields(v)
            PrintFields(x.GetAxesMotionOut())
            logInfo "\tAxes Speed=%A" (x.GetDriveAxesSpeed().Value)
            logInfo "\tStop Info=%A" (x.GetStopInfo().Value)
            let cmdSpeed, encSpeed = x.GetAxesCmdEncSpeed().Value
            logInfo "\tCmd Speed=%A" cmdSpeed
            logInfo "\tEnc Speed=%A" encSpeed
        | None -> logError "\tNo Axes found!"



//        let en = x.GetEnumList()
//
//        let n = x.GetAllParaLogics().Length
//
//        for i in {7s..10s} do
//            let logic = x.GetParaLogic(i)
//            printfn "%A" logic


//        [0s..10s] 
//            |> Seq.map (fun i -> 
//                let logic = paix.GetParaLogic(i)
//                printfn "%d-th %A" i logic
//                logic )

    interface IDeviceDriver with
        member val ProcException = procException with get, set


module Module =

    /// <summary>
    /// Exception safe paix manager singleton
    /// If exception occurs on paix manager, new instance is automatically created.
    /// </summary>
    let mutable private paixManagerSingleton: Manager option = None

    /// <summary>
    /// Paix manager 객체에 대한 interface 를 생성한다.
    /// </summary>
    /// <param name="ip">기기의 ip</param>
    /// <param name="autoOpen">생성자에서 자동 paix 연결 여부</param>
    let rec createManager ip autoOpen =
        try
            let paix = new Manager(ip, false)
            paix.ProcException <- fun exn -> createManager ip true |> ignore
            if autoOpen then
                paix.OpenPaix()

            paixManagerSingleton <- Some(paix)
        with exn ->
            DriverExceptionSubject.OnNext(PaixException(exn))
            raise exn

    /// Paix manager 객체에 대한 interface 를 생성한다.
    let createManagerSimple() = createManager paixIp true

    //let loadFromAppConfig() = ModuleBase.loadFromAppConfig()
    let loadFromAppConfig() =
        match readStringKey "paixIp" with
        | Some(v) -> paixIp <- v
        | _ -> ()

        logInfo "-- Paix Configuration --"
        logInfo "\tIP = %s" paixIp

    /// <summary>
    /// Returns exception safe singleton instance
    /// paix manager 에서 exception 이 발생하는 경우, 새로운 객체를 생성하도록 하여 반환한다.
    /// </summary>
    let manager() =
        paixManagerSingleton.Value


