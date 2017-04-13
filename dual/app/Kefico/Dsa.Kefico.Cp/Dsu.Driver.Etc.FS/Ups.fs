(*
// UPS 온도/습도 확인 방법
// 1. Browser 를 통해 해당 IP 에 접속, user/pwd = apc/apc 입력.
//      Configuration > Universal I/O > Temp & Humidity
// 1. Telnet session 을 통해 해당 IP 에 접속, user/pwd = apc/apc 입력.
//      uio -st 입력하면 온/습도계가 장착되어 있으면 다음과 같이 출력됨
//      U1:22.5C:OK:42%RH:OK\n
//      U2:NA
//
//      온/습도계가 장착되어 있지 않으면
//      U1:Lost Comm\n
//      U2:NA
*)
module Dsu.Driver.Ups

open System
open System.Threading
open System.Reactive.Subjects
open System.Text.RegularExpressions
open AppConfig
open DriverExcpetionModule


let mutable (upsIp, upsPort) = ("192.168.0.33", 23)
let mutable (upsUserId, upsPassword) = ("apc", "apc")

/// <summary>
/// 온/습도 측정 interval.  30초
/// </summary>
let mutable internal queryIntervalMilli = 30 * 100


/// <summary>
/// Telnet prompt 를 읽고, request 를 보내기 전의 sleep interval
/// </summary>
let mutable internal telnetCommunicationPauseMilli = 200


let logUpsConfig() =
    logInfo "-- UPS Configuration --"
    logInfo "\tIP/Port = %s %d" upsIp upsPort
    logInfo "\tUser Id/Password = %s/%s" upsUserId upsPassword
    logInfo "\tqueryIntervalMilli = %d" queryIntervalMilli
    logInfo "\ttelnetCommunicationPauseMilli = %d" telnetCommunicationPauseMilli

let loadFromAppConfig() =
    Socket.loadFromAppConfig()

    match readStringKey "upsIp" with
    | Some(v) -> upsIp <- v
    | _ -> ()

    match readStringKey "upsUserId" with
    | Some(v) -> upsUserId <- v
    | _ -> ()

    match readStringKey "upsPassword" with
    | Some(v) -> upsPassword <- v
    | _ -> ()

    match readIntKey "queryIntervalMilli" with
    | Some(v) -> queryIntervalMilli <- v
    | _ -> ()

    match readIntKey "telnetCommunicationPauseMilli" with
    | Some(v) -> telnetCommunicationPauseMilli <- v
    | _ -> ()

    match readIntKey "upsPort" with
    | Some(v) -> upsPort <- v
    | _ -> ()

    logUpsConfig()


type UpsData (timeStamp:System.DateTime, temperature:double, humidity:double) =
    member val TimeStamp = timeStamp with get, set
    member val Temperature = temperature with get
    member val Humidity = humidity with get

let upsDataChangedSubject = new Subject<UpsData>()
let mutable private lastUpsData: UpsData option = None



// https://www.pcreview.co.uk/threads/simple-telnet-commands-using-c-and-tcpclient.1353104/
// TODO 
//  telnet session 이 다섯번 연속 수행되면 timeout 오류가 발생한다.  해결책이 필요하다.
//  하나의 세션을 구동하고, 계속 읽은 값을 어딘가에 써 놓는 방식???
/// <summary>
/// APC UPS manager
/// </summary>
type UpsManager(ip:string) =
    inherit TcpTelnetClientManager(ip)

    // locking object
    let locker = new obj()
    // locked execution
    let L f = lock locker f
    // locked execution for base method call 
    let L' f = 
        Monitor.Enter locker
        try
            f
        finally
            Monitor.Exit locker

    let procException:(Exception -> unit) = fun exn -> ()
    let cts = new CancellationTokenSource()     // Thread 를 멈추기위한 task completion source

//    // Exception 발생 시, 호출되는 routine
//    let onException(exn:DriverException) =
//        this.ProcException(exn)     // Manager 에 등록된 exception 발생시의 routine 을 먼저 수행
//        logError "------ Exception: %O" exn
//
    let readIO() = 
        let appendCrLf str = str + "\r\n"          // WARNING: "\n" 이 아니라 "\r\n" 임.
        async {
            try
                let mutable readBuffer = ""
                let t = telnetCommunicationPauseMilli
                use tcp = new TcpTelnetClientManager(ip)
                tcp.Subject.Subscribe(fun str -> 
                    //printfn "<%s>" str
                    readBuffer <- readBuffer + str) |> ignore
                tcp.StartMonitor()

                let mutable goOn = true
                while goOn do 
                    if readBuffer.Contains("User Name") then
                        readBuffer <- ""
                        do! tcp.PostCommand(appendCrLf(upsUserId))
                        readBuffer <- ""
                        goOn <- false
                    else
                        do! Async.Sleep t
 
                goOn <- true
                while goOn do 
                    if readBuffer.Contains("Password") then
                        readBuffer <- ""
                        do! tcp.PostCommand(appendCrLf(upsPassword))
                        readBuffer <- ""
                        goOn <- false
                    else
                        do! Async.Sleep t


                while true do
                    goOn <- true
                    while goOn do 
                        if readBuffer.Contains("apc>") then
                            readBuffer <- ""
                            do! tcp.PostCommand(appendCrLf("uio -st"))
                            readBuffer <- ""
                            goOn <- false
                        else
                            do! Async.Sleep t


                    goOn <- true
                    while goOn do 
                        if readBuffer.EndsWith("apc>") then        // e.g "uio -st\r\nE000: Success\r\nU1:22.5C:OK:42%RH:OK\nU2:NA\n\napc>"
                            try
                                let result = 
                                    let lines = 
                                        readBuffer.Replace("\r\n", "\n").Split '\n'
                                        |> Array.filter (fun s -> not (s.StartsWith("uio -st")) && not (Regex.IsMatch(s, "^U\d+:NA")))

                                    if lines.[0] <> "E000: Success" then
                                        failwithlog ("Invalid result: " + readBuffer)
                                    lines |> Array.filter (fun s -> Regex.IsMatch(s, "^U\d+:.*OK")) |> Array.head

                                //logDebug "[[[[[[[[[%s]]]]]]]]]]]" result
                                let regex = Regex.Match(result, @"^U1:([^C]*)C:OK:([^%]*)%RH:OK$")
                                if regex.Groups.Count <> 3 then
                                    failwithlog ("Invalid result: " + readBuffer)

                                let temp = System.Double.Parse(regex.Groups.[1].ToString())
                                let humidity = System.Double.Parse(regex.Groups.[2].ToString())
                                //logDebug "Sending result temperature=%f, humidity=%f" temp humidity
                                let now = DateTime.Now
                                match lastUpsData, temp with
                                | Some(v), temp when v.Temperature = temp ->
                                    logDebug "\tUpdating last checked time to %A" now
                                    lastUpsData.Value.TimeStamp <- now
                                | _ ->
                                    let data = new UpsData(DateTime.Now, temp, humidity)
                                    L(fun () -> lastUpsData <- Some(data))
                                    upsDataChangedSubject.OnNext(data)

                                goOn <- false
                            with exn ->
                                let exn' =
                                    let msg = sprintf "Failed to parse UPS data: %s\r\n" readBuffer
                                    let msg = msg + (sprintf "Check: UPS via browser (user/pass=%s/%s) http://%s\r\n" upsUserId upsPassword ip)
                                    let msg = msg + "Check: Configuration > Universal I/O > Temp & Humidity\r\n"
                                    new UnrecovervableException(msg, exn)
                                DriverExceptionSubject.OnNext(UpsException(exn'))
                                raise exn'
                        else
                            do! Async.Sleep t

                    do! Async.Sleep queryIntervalMilli
                with exn ->
                    DriverExceptionSubject.OnNext(UpsException(exn))
                    logError "%O" exn
                    //onException exn
        }

    do
        logInfo "Ups manager created."
        logUpsConfig()

    /// Background threading 취소
    member __.Cancel() =
        logInfo "Ups manager service canceled."
        cts.Cancel()

    member x.ReadIO() = 
        // background task 로 온/습도 값을 계속 읽어 들임
        Async.Start(readIO(), cts.Token)

    /// UPS Manager 에서 exception 이 발생했을 때 수행할 사용자의 action 등록
    member val ProcException:(Exception -> unit) = procException with get, set
    member __.GetTemparature() = L(fun() -> lastUpsData.Value.Temperature)
    member __.GetHumidity() = L(fun() -> lastUpsData.Value.Humidity)
    member __.GetTimeStamp() = L(fun() -> lastUpsData.Value.TimeStamp)
    member __.GetLastUpsData() = L(fun() -> lastUpsData.Value)

    new () = new UpsManager(upsIp)



/// <summary>
/// Exception safe paix manager singleton
/// If exception occurs on paix manager, new instance is automatically created.
/// </summary>
let mutable private upsManagerSingleton: UpsManager option = None

let rec createManager(ip:string) =
    try
        let upsManager = new UpsManager(ip)
        upsManagerSingleton <- Some(upsManager)
        upsManager.ProcException <- fun exn ->
            match exn with
            | :? UnrecovervableException as ex ->
                DriverExceptionSubject.OnNext(UpsException(exn))
            | _ ->
                logError "Retrying to create UPS manager on exeption %O" exn
                createManager ip |> ignore
        upsManager.ReadIO()
    with exn ->
        DriverExceptionSubject.OnNext(UpsException(exn))
        raise exn


/// <summary>
/// Returns exception safe singleton instance
/// UPS manager 에서 exception 이 발생하는 경우, 새로운 객체를 생성하도록 하여 반환한다.
/// </summary>
let manager() =
    upsManagerSingleton.Value



