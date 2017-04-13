module Dsu.Driver.Plc.Melsec.MxPlcConfig

open System
open System.Configuration
open System.Net
open AppConfig
open Log4NetWrapper


let mxPlcActorSystemName = "mxplc-system"


/// <summary>
/// Mx PLC  server 가 구동되는 server.  "192.168.0.2" 등의 형태로 써야 함. "localhost" 형태로 쓰면 안됨.
/// 특별히 설정하지 않으면, localhost 에 해당하는 IP address 를 사용함.
/// </summary>
let mutable mxPlcServer = getIpAddress()


/// <summary>
/// Mx PLC server 에 접속하려는 client 제한.  0.0.0.0 이면 무제한
/// </summary>
let mutable mxPlcPeers = "0.0.0.0"

/// <summary>
/// Mx PLC  actor service port
/// </summary>
let mutable mxPlcPort = 50011

let mxPlcActorName = "mxplc-guardian-actor"

/// <summary>
/// AKKA actor 에서 제한하는 통신 byte 길이
/// </summary>
let mxPlcActorMaxPayloadBytes = 30000000

/// <summary>
/// Mx PLC server actor 가 request 에 대해서 반응하는 시간에 대한 timeout 설정.
/// PLC client Actor 가 mxPlc server actor 에게 Ask message 전송 후, 
/// mxPlcServerTaskCompletionTimeoutSecond 시간 안에 응답이 없으면 오류로 처리한다.
/// </summary>
let mutable mxPlcServerTaskCompletionTimeoutSecond = 120


/// <summary>
/// Client 와 서버 간 시간 동기화 오류 한계
/// </summary>
let mutable mxPlcServerClientTimeDifferenceLimitSecond = 30


/// <summary>
/// Mx PLC  server actor 가 ping request 에 대해서 반응하는 시간에 대한 timeout 설정.
/// </summary>
let mutable mxPlcServerPingTimeoutSecond = 5

/// <summary>
/// Debug enable 여부.
/// </summary>
let mutable mxPlcEnableDebug = false



let mutable log4netConfigFile = "MxPlcLog4net.xml"


let printMxPlcConfiguration() =
    use changer = consoleColorChanger ConsoleColor.Yellow
    logInfo "MxPlc Configuration"
    use changer = consoleColorChanger ConsoleColor.Cyan
    logInfo "\tMxPlc server = %s" mxPlcServer
    logInfo "\tTask timeout = %d" mxPlcServerTaskCompletionTimeoutSecond
    logInfo "\tEnable debug = %s" (mxPlcEnableDebug.ToString())
    logInfo "\tlog4netConfigFile = %s" log4netConfigFile
    



/// <summary>
/// App.Config (runtime 에는 xxx.exe.config) 파일로부터 overriding configuration 정보 읽어서 mutable 값에 할당
/// </summary>
let loadFromAppConfig() =
    let configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile

    match readStringKey "mxPlcServer" with
    | Some(v) -> mxPlcServer <- v
    | _ -> ()

    match readStringKey "log4netConfigFile" with
    | Some(v) -> log4netConfigFile <- v
    | _ -> ()    


    let convertIp hostName =
        if hostName = "localhost" then
            let ipAddress =                 
                Array.Find (Dns.GetHostAddresses(Dns.GetHostName()), (fun ip -> (ip.ToString()).Contains(".")))
            ipAddress.ToString()
        else
            hostName

    mxPlcServer <- (convertIp mxPlcServer)


    let peers = ConfigurationManager.AppSettings.["mxPlcPeers"];
    if peers <> null then mxPlcPeers <- peers

    match readIntKey "mxPlcPort" with
    | Some(v) -> mxPlcPort <- v
    | _ -> ()


    match readIntKey "mxPlcServerTaskCompletionTimeoutSecond" with
    | Some(v) -> mxPlcServerTaskCompletionTimeoutSecond <- v
    | _ -> ()

    match readBoolKey "mxPlcEnableDebug" with
    | Some(v) -> mxPlcEnableDebug <- v
    | _ -> ()


(*
 *  최초 App.Config file 강제 loading.  여기서 수행하지 않으면 모든 appliation 에서 최초 시작시 수행해 주어야 함.
 *)
loadFromAppConfig()

