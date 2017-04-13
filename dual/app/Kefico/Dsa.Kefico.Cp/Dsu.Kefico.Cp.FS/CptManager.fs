module CptManagerModule

open System
open Akka.Actor
open Dsu.Common.Utilities
open Dsu.Common.Utilities.FS
open MySql.Data.MySqlClient
open PsCommon
open PsKGaudi.Parser


let mutable log4netConfigFile = "CptLog4net.xml"


//Threading.Thread.CurrentThread.CurrentCulture <- new Globalization.CultureInfo("en-US");
//let xxx = DateTime.Now
//printfn "%s" (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

let mutable internal mySqlConnection: MySqlConnection = null
let private openMySqlConnection() =
    if mySqlConnection = null then
        mySqlConnection <- new MySqlConnection(MwsConfig.getConnectionString())
        mySqlConnection.Open() |> ignore
        mysqlExecNonQuery mySqlConnection "SET GLOBAL max_allowed_packet=256*1024*1024;" |> ignore

/// <summary>
/// Cpt(Common Platform Tester) 관리자.
/// 시험기가 필요로 하는 기능들을 수행
/// </summary>
type CptManager private (configHost:CptHostConfig, configProduct:ProductConfig, configTest:TestConfig) =
    let hostId = configHost.Host.Value
    let sec = configHost.Section.Value
    let actorSystem = CptActor.system
    let mutable serverActor : IActorRef = CptActor.server
    let guardianActor = CptActor.CreateCptGuardianActor(hostId, sec)
    let receivingActor = CptActor.receivingActor
    let mutable pdvId : uint32 option = None
    let getSqlServerTime() = mysqlExecScalar mySqlConnection "SELECT NOW();" :?> DateTime


    let ask2(message: IActorMessage, timeOut:System.TimeSpan ) =
        let startTime = System.DateTime.Now
        addPendingMessage message startTime
        guardianActor.Tell(message)

        // waits until answer receiving
        let waitResponse(): IActorMessage =
            let rec waitHelper (waitIntervalMilli:int) : IActorMessage =
                if System.DateTime.Now - startTime < timeOut then
                    let response = popOnFinished message.Id
                    match response with
                        | Some(v) -> v
                        | None ->
                            Async.Sleep(waitIntervalMilli) |> ignore
                            waitHelper (2 * waitIntervalMilli) 
                else
                    let msg = sprintf "Timeout for message %s" (message.ToString())
                    new AmError(ErrorCodeEnums.Timeout, msg) :> IActorMessage

            waitHelper 10

        waitResponse()

    let ask1(message: IActorMessage) =
        ask2(message, TimeSpan.FromSeconds(float MwsConfig.mwsServerTaskCompletionTimeoutSecond))

    /// <summary>
    /// 시험기 구동 전에 필요한 점검 수행
    /// 1. Database server 와 필요한 정보 matching
    /// 1. MWS server actor 와 assembly version 호환성 체크
    /// </summary>
    let sanityCheck() =
        let checkAssemblyVersions() =
            let localAssembliesMap =
                System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                |> Seq.map (fun a -> a.Name, a.Version)
                |> Map.ofSeq

#if DEBUG
            localAssembliesMap
            |> Map.iter (fun k v -> printfn "%s = %A" k v)
#endif

            let remoteAssembliesMap =
                let response: IActorMessage = ask1(new AmRequestAssembliesVersion())
                let serverVersions = response :?> AmReplyAssembliesVersion
                serverVersions.Versions.Replace("\r\n", "\n").Split('\n')
                |> Seq.filter (fun line -> line <> "")
                |> Seq.map (fun line -> 
                    let tokens = line.Split('\t')
                    Seq.item 0 tokens, Seq.item 1 tokens
                )
                |> Map.ofSeq

            
            printfYellow "Checking server assembly versions... "

            localAssembliesMap
            |> Map.iter (fun n v -> 
                match remoteAssembliesMap.TryFind(n) with
                | Some(rv) -> if v.ToString() <> rv then failwith (sprintf "Version mismatch on %s: %A != %A" n v rv)
                | None -> ()
            )
            printfnGreen "Done!"


        MwsServer.checkDatabase()
        checkAssemblyVersions()


    /// <summary>
    /// CPT system 시작 시, db server 와 필요한 정보를 sync 한다.
    /// 1. CPT id 확보
    /// 1. server 시간으로 Time 동기화
    /// 1. Power on notify
    /// 1. Batch change notify
    /// </summary>
    let prologue() =
        let getCptId() =
            let query = sprintf "SELECT id FROM ccs WHERE host=%d AND sec='%s' LIMIT 1;" configHost.Host.Value configHost.Section.Value
            let cptId = query |> mysqlExecScalar mySqlConnection
            configHost.CptId <- 
                match cptId with
                | :? System.DBNull -> None
                | _ as n -> 
                    let id = Functions.tryCast<uint32>(n)
                    if id.IsNone then
                        failwith "Failed to extract CP tester id from server."
                    id
        let syncServerTime() =
            printfYellow "Checking server/client clocks... "

            let serverTime = getSqlServerTime()
            let localTime = DateTime.Now

            let diff = serverTime - localTime
            let limit = float MwsConfig.mwsServerClientTimeDifferenceLimitSecond
            if diff > TimeSpan.FromSeconds(limit) then
                failwith (sprintf "Server time and local system time have difference larger than %f seconds." limit)

            printfnd "Server=%A, Local=%s" serverTime (localTime.ToString("yyyy-MM-dd HH:mm:ss"))
            printfnGreen "Done!"



        getCptId()
        syncServerTime()


    do
        MwsConfig.loadFromAppConfig()
        MwsConfig.printMwsConfiguration()
        openMySqlConnection()

        sanityCheck()
        prologue()

        configHost.CptId <- 
            let query = sprintf "SELECT id FROM ccs WHERE host=%d AND sec='%s';" configHost.Host.Value configHost.Section.Value
            let ccsId = mysqlExecScalar mySqlConnection query :?> uint32
            Some(ccsId)

    member val ConfigHost = configHost with get
    member val ConfigProduct = configProduct with get
    member val ConfigTest = configTest with get

    member __.GetSqlServerTime() = getSqlServerTime()
    member x.Dispose() = (x :> IDisposable).Dispose()
    interface IDisposable with
        member x.Dispose () =
            async {
                // actor system terminate 끝을 확인하는 win32 task 를 반환하므로, 이를 async 로 기다린다.
                // Async.RunSynchronously 로 기다리면 lock 걸려서 무한정 기다리게 됨.
                do! actorSystem.WhenTerminated |> Async.AwaitTask

                mySqlConnection.Dispose()
                GC.SuppressFinalize(x)
            } |> Async.Start    // <- Do NOT use Async.RunSynchronously


    // TODO: CptManager 생성 없이 CptHostConfig 정보 만으로 PowerOn 상태 전송 가능하도록 API 변경할 것.
    static member internal notifyPowerOnOffStatusChange(configHost:CptHostConfig, powerOn:bool) =
        logInfo "Notifying client power %s... " (if powerOn then "ON" else "OFF")

        let query = 
            let now = toMysqlDateTime DateTime.Now
            let powerOn' = powerOn.ToString()
            let (host, fixture, batch) = configHost.GetValuesUnsafe()
            let sec = configHost.Section.Value
            sprintf "CALL notifyPowerOnOffStatusChange('%s', %d, '%s', %s, '%s', '%s');"
                    now host sec powerOn' fixture batch

        openMySqlConnection()                    
        mysqlExecNonQuery mySqlConnection query |> ignore
        logInfo "Done!"

    static member SetLogger(logger) =
        Log4NetWrapper.SetLogger(logger)


    static member NotifyPowerOn(configHost:CptHostConfig) = CptManager.notifyPowerOnOffStatusChange(configHost, true)
    member x.NotifyPowerOff() = CptManager.notifyPowerOnOffStatusChange(configHost, false)

    member x.NotifyBatchChange() =
        logInfo "Notifying batch change... "
        let now = toMysqlDateTime(DateTime.Now)
        let (_, fixture, batch) = configHost.GetValuesUnsafe()
        let cptId = configHost.CptId.Value
        let (ecuId, eprom) = (configProduct.EcuId, configProduct.Eprom)
        let sec = configHost.Section.Value
        let query = sprintf "CALL notifyBatchChange('%s', %d, %d, '%s', '%s', '%s', '%s');"
                                now cptId pdvId.Value ecuId eprom fixture batch
        mysqlExecNonQuery mySqlConnection query |> ignore
        logInfo "Done!"

    member x.PdvId = pdvId
    member x.MySqlConnection = mySqlConnection

    member __.Ask(message: IActorMessage, timeOut:System.TimeSpan ) = ask2(message, timeOut)

    member private x.AskRaisable(message: IActorMessage, timeOut:System.TimeSpan ) : IActorMessage =
        match x.Ask(message, timeOut) with
            | :? AmError as m ->
                logError "error to get response: %s" (m.ToString())
                failwithf "error to get response: %s" (m.ToString())

            | _ as m ->
                m

    member x.Ask(message: IActorMessage) =
        x.Ask(message, TimeSpan.FromSeconds(float MwsConfig.mwsServerTaskCompletionTimeoutSecond))


    /// <summary>
    /// Guadi file parsing 을 서버에 request 해서 시험에 필요한 step 정보들을 추출한다.
    /// </summary>
    /// <param name="gaudiFilePath"></param>
    /// <param name="variant">Variant 번호.  양산은 "01", 나머지는 02..99 까지 허용가능함</param>
    /// <param name="gates">{H, R, T} for Hot, Room, Total test</param>
    member x.LoadTestData(pdvId) =
        logInfo "Got test list load request for pdvId=%d" pdvId
        let loadTestData() =
            let response = x.Ask(new AmRequestStepsOnGaudiFile(pdvId))
            match response with
            | :? AmReplySteps as am ->
                //pdvId <- Some(am.PdvId)
                x.NotifyBatchChange()
                am.Steps
            | :? AmError as am ->
                let msg = "Got failure from server: " + am.ToString()
                logError "%s" msg
                failwith msg
            | _ ->
                let msg = "Failed to get response from server. Response: = " + response.ToString()
                logError "%s" msg
                failwith msg

        let (result, time) = Functions.duration (fun () -> loadTestData())
        logInfo "Test list loading took %f seconds." ( (float time) / 1000.0)
        result



    member x.ApiRequestTestStep(details:CptTestInformationDetails) =
        x.LoadTestData(details.PdvId)

    member x.ApiUploadCpXml(pdvId, path) =
        logInfo "Got CpXml [%s] upload request for pdvId=%d" path pdvId
        let response = x.Ask(new AmRequestUploadCpXml(pdvId, path))
        match response with
        | :? AmReplyOK as am ->
            ()
        | :? AmError as am ->
            let msg = "Got failure from server: " + am.ToString()
            logError "%s" msg
            failwith msg
        | _ ->
            let msg = "Failed to get response from server. Response: = " + response.ToString()
            logError "%s" msg
            failwith msg


    /// <summary>
    /// CPT system 종료 시 필요한 작업
    /// </summary>
    member internal x.Epilogue() =
        x.NotifyPowerOff()


    /// <summary>
    /// 1회 test 결과를 서버에 송부한다.
    /// NG 가 나더라도, upload step 에 포함되어 있으면 모든 step 을 전송한다.
    ///  서버도 NG 이후의 step 에 대해서, upload step 에 포함되어 있으면 이를 저장하여야 한다. (2016.12.20 회의 결과)
    ///  즉, 서버에서 받은 step 에 대해서 시험기가 올려주는 모든 step 을 서버는 저장하고 있어야 한다.
    /// </summary>
    member x.UploadTestResult(summary:CptModule.TestSummary, steps:seq<Step.UploadStep>) =
        let uploadTestResultHelper() = 
            let now = summary.StartTime
            let duration = summary.Duration
            // 전체 시험에 대한 NG/OK 판정
            let ok = steps |> Seq.forall(fun s -> s.IsOK)

            let sql =
                // SQL 문 insert into 에 넣을 복수개의 values 값을 문자열로 생성
                let values =
                    let length = Seq.length steps
                    steps
                    |> Seq.sortBy (fun s -> s.StepNumber)
                    |> Seq.map (fun s -> 
                        let message = 
                            match s.Message with
                            | Some(m) -> Functions.singleQuote m
                            | None -> "NULL"
                        if s.HasMinMaxValue then
                            sprintf "(%d, %M, %s, %s)" s.Id s.Value.Value message (s.IsOK.ToString())
                        else
                            sprintf "(%d, NULL, %s, TRUE)" s.Id message)

                    |> Seq.fold(fun acc elem -> (if acc = "" then "" else acc + "\r\n\t, ") + elem) ""

                printfnd "Uploading steps =\r\n%s" values
                System.Diagnostics.Trace.WriteLine(values)


                let date = now.ToString("yyyy-MM-dd")
                let time = now.ToString("HH:mm:ss")
                let (_, fixture, batch) = configHost.GetValuesUnsafe()
                let cptId = configHost.CptId.Value

                let (ecuId, eprom) = (configProduct.EcuId, configProduct.Eprom)

                """
                CALL generateTemporaryBundleTable();
                INSERT INTO tt_bundle(stepId, value, message, ok)
                VALUES
                """ + values + """
                ;

                """ + sprintf "CALL insertMeasure('%s', '%s', '%M', %d, %d, '%s', '%s', '%s', '%s', %s);\n\n"
                                date time duration cptId pdvId.Value ecuId eprom fixture batch (ok.ToString())

            System.Diagnostics.Trace.WriteLine(sql)

            let onFailure exn = 
                logError "%s" (exn.ToString())
                tryMessageBox(exn.ToString())

            let f() = mysqlExecNonQuery mySqlConnection sql
            MySQLApi.withTransaction mySqlConnection f onFailure

        let (result, time) = Functions.duration (fun () -> uploadTestResultHelper())
        logInfo "Uploading test list took %f seconds." ( (float time) / 1000.0)
        result


    member x.GetCpTestInformationDetails() = x.ApiGetCpTestInformationDetails(configTest.PartNumber, configTest.Gate, configTest.IsProduction)

    /// <summary>
    /// 시험기의 여러 조건을 조합하여, test 관련 정보를 추출한다.
    /// Return 값은 CptTestInformationDetails type record
    /// </summary>
    member x.ApiGetCpTestInformationDetails(partNumber, gate, isProduction) =
        let request = new AmRequestTestDetails(partNumber, gate, isProduction)
        let response = x.Ask(request)
        match response with
        | :? AmReplyTestDetails as m ->
            pdvId <- Some(m.PdvId)
            new CptTestInformationDetails(request, m)
        | _ ->
            let msg = "Failed to get response from server. Response: = " + response.ToString()
            logError "%s" msg
            failwith msg
    
    member x.UploadModifiedTestListDebugging(values) =
        GaudiFileDBApi.fillStepTable mySqlConnection pdvId.Value values

    /// Cpt Manager 를 생성해서 반환.  (Excpetion 에 대한 1차 처리 수행하기 위해서 직접 Cpt Manager 생성을 막음)
    static member Create(configHost, configProduct, configTest) =
        try
            new CptManager(configHost, configProduct, configTest)
        with exn ->
            match exn with
            | :? System.TypeInitializationException ->
                failwith "Failed to initialize actor system.  check MWS Window Service is running"
            | _ ->
                raise exn



