module MwsConfig

open System
open System.Configuration
open System.Net
open AppConfig
open Log4NetWrapper


/// MWS service 가 windows service 로 동작하면 true, debugging 용 console 로 동작하면 false 값을 가짐.
/// MWS Service App 실행 파일에서 상황에 맞게 설정한다.
let mutable mwsAsWindowsService = false

let mwsActorSystemName = "server-system"


/// <summary>
/// MWS server 가 구동되는 server.  "192.168.0.2" 등의 형태로 써야 함. "localhost" 형태로 쓰면 안됨.
/// 특별히 설정하지 않으면, localhost 에 해당하는 IP address 를 사용함.
/// </summary>
let mutable mwsServer = getIpAddress()


/// <summary>
/// MWS server 에 접속하려는 client 제한.  0.0.0.0 이면 무제한
/// </summary>
let mutable mwsPeers = "0.0.0.0"

/// <summary>
/// MWS actor service port
/// </summary>
let mutable mwsPort = 50001

let mwsActorName = "mws-guardian-actor"

/// <summary>
/// AKKA actor 에서 제한하는 통신 byte 길이
/// </summary>
let mwsActorMaxPayloadBytes = 30000000

/// <summary>
/// MWS server actor 가 request 에 대해서 반응하는 시간에 대한 timeout 설정.
/// CPT Actor 가 mws server actor 에게 Ask message 전송 후, 
/// mwsServerTaskCompletionTimeoutSecond 시간 안에 응답이 없으면 오류로 처리한다.
/// - Gaudi File parsing 시간 + DB processing 시간을 고려하여 넉넉히 잡아야 한다.
///
///     Linux MWS Server machine 의 경우, step merging 시에 약 30초 소요됨.
///     개발 서버(with SSD) machine 의 경우, step merging 시에 약 20초 소요됨.
///
/// - see MySQLComputationExpression.defaultSqlCommandTimeoutSecond
/// </summary>
let mutable mwsServerTaskCompletionTimeoutSecond = 120


/// <summary>
/// Client 와 서버 간 시간 동기화 오류 한계
/// </summary>
let mutable mwsServerClientTimeDifferenceLimitSecond = 30


/// <summary>
/// MWS server actor 가 ping request 에 대해서 반응하는 시간에 대한 timeout 설정.
/// </summary>
let mutable mwsServerPingTimeoutSecond = 5

/// <summary>
/// Debug enable 여부.
/// </summary>
let mutable mwsEnableDebug = false



/// Test list(Gaudi) file path
let mutable mwsTestListPathPrefix = @"C:\"

/// Flash Rom file path
let mutable mwsFlashPathPrefix = @"C:\"

let mutable log4netConfigFile = "MwsLog4net.xml"
//let mutable internal gLogger : log4net.ILog = null
//
//
//let logInfo fmt = logWithLogger gLogger gLogger.Info fmt
//let logDebug fmt = logWithLogger gLogger gLogger.Debug fmt
//let logWarn fmt = logWithLogger gLogger gLogger.Warn fmt
//let logError fmt = logWithLogger gLogger gLogger.Error fmt
//
//let failwithlog fmt = failwithlogger gLogger fmt

/// <summary>
/// Database service host name
/// </summary>
let mutable dbServer = "dualsoft.co.kr"
let mutable dbUser = "securekwak"
let mutable dbPassword = "kwak"
let mutable dbDatabase = "kefico"
let dbPort = 3306

/// MWS windows service 및 CPT 둘다 사용하는 connection string.
let getConnectionString() = 
    sprintf "server=%s;user=%s;password=%s;compress=true;database=%s;port=%d;Allow User Variables=True" dbServer dbUser dbPassword dbDatabase dbPort


let printMwsConfiguration() =
    use changer = consoleColorChanger ConsoleColor.Yellow
    logInfo "MWS Configuration"
    use changer = consoleColorChanger ConsoleColor.Cyan
    logInfo "\tMWS server = %s" mwsServer
    logInfo "\tTask timeout = %d" mwsServerTaskCompletionTimeoutSecond
    logInfo "\tDatabase Server/Port = %s/%d" dbServer dbPort
    logInfo "\tDatabase = %s" dbDatabase
    logInfo "\tUser = %s" dbUser
    logInfo "\tEnable debug = %s" (mwsEnableDebug.ToString())
    logInfo "\tlog4netConfigFile = %s" log4netConfigFile
    logInfo "\tmwsTestListPathPrefix = %s" mwsTestListPathPrefix
    logInfo "\tmwsFlashPathPrefix = %s" mwsFlashPathPrefix
    



/// <summary>
/// App.Config (runtime 에는 xxx.exe.config) 파일로부터 overriding configuration 정보 읽어서 mutable 값에 할당
/// </summary>
let loadFromAppConfig() =
    let configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile

    match readStringKey "mwsServer" with
    | Some(v) -> mwsServer <- v
    | _ -> ()

    match readStringKey "log4netConfigFile" with
    | Some(v) -> log4netConfigFile <- v
    | _ -> ()

    match readStringKey "mwsTestListPathPrefix" with
    | Some(v) -> mwsTestListPathPrefix <- v
    | _ -> ()

    match readStringKey "mwsFlashPathPrefix" with
    | Some(v) -> mwsFlashPathPrefix <- v
    | _ -> ()
    


    let convertIp hostName =
        if hostName = "localhost" then
            let ipAddress =                 
                Array.Find (Dns.GetHostAddresses(Dns.GetHostName()), (fun ip -> (ip.ToString()).Contains(".")))
            ipAddress.ToString()
        else
            hostName

    mwsServer <- (convertIp mwsServer)


    let peers = ConfigurationManager.AppSettings.["mwsPeers"];
    if peers <> null then mwsPeers <- peers

    match readIntKey "mwsPort" with
    | Some(v) -> mwsPort <- v
    | _ -> ()


    match readIntKey "mwsServerTaskCompletionTimeoutSecond" with
    | Some(v) -> mwsServerTaskCompletionTimeoutSecond <- v
    | _ -> ()

    match readBoolKey "mwsEnableDebug" with
    | Some(v) -> mwsEnableDebug <- v
    | _ -> ()



    let dbServer' = ConfigurationManager.AppSettings.["dbServer"];
    if dbServer' <> null then dbServer <- dbServer'

    let dbUser' = ConfigurationManager.AppSettings.["dbUser"];
    if dbUser' <> null then dbUser <- dbUser'
    
    let dbPassword' = ConfigurationManager.AppSettings.["dbPassword"];
    if dbPassword' <> null then dbPassword <- dbPassword'
    
    let dbDatabase' = ConfigurationManager.AppSettings.["dbDatabase"];
    if dbDatabase' <> null then dbDatabase <- dbDatabase'


(*
 *  최초 App.Config file 강제 loading.  여기서 수행하지 않으면 모든 appliation 에서 최초 시작시 수행해 주어야 함.
 *)
loadFromAppConfig()

