module MwsServer

open System
open Akka.FSharp
open Akka.Actor
open PsCommon
open GaudiFileDBApi
open log4net
open MwsConfig
open Log4NetWrapper
open PsKGaudi.Parser
open Dsu.Common.Utilities.FS
open Dsu.Common.Utilities.Core.ExtensionMethods


/// MWS server 에서 발생한 exception 을 전달
/// Actor system 에 묻혀 있으므로, exception 전달을 원할히 받기 위한 공지 구조
let MwsServerFailedMessageSubject = new System.Reactive.Subjects.Subject<obj>() 

let mutable internal counter = 0

let internal checkDatabase() =
    use conn = mysqlCreateConnection(MwsConfig.getConnectionString())
    let dictionaryToString(dict:Map<string,uint16>) =
        dict |> Map.toSeq |> Seq.sortBy snd
        |> Seq.map (fun tpl -> sprintf "\t%s=%d\r\n" (fst tpl) (snd tpl))
        |> Seq.fold (fun acc elem -> acc + elem) ""
    let enumTupleToString(enums: seq<string*int>) =
        enums |> Seq.sortBy snd
            |> Seq.map (fun tpl -> sprintf "\t%s=%d\r\n" (fst tpl) (snd tpl))
            |> Seq.fold (fun acc elem -> acc + elem) ""

    printfYellow "Checking database sanity...\r\n"
    let checkDimensions() =
        printfCyan "\tChecking dimension table..."
        let dimServerDict = GaudiFileDBApi.getDimensionDictionary(conn)

        let t = typeof<CpSpecDimension>

        let dllEnums = 
            Enum.GetNames(t)                // string array
            |> Seq.map (fun eName -> eName, Functions.cast<int>(Enum.Parse(t, eName)))

        dllEnums
            |> Seq.iter (fun (eName, d) -> 
                match dimServerDict.TryFind(eName) with
                | Some(d') ->                                               // prime version : remote(from server)
                    if d <> int(d') then
                        let msg = sprintf "ERROR on dimension info %s : %d(local) != %d(server)" eName d d'
                        logError "%s" msg
                        logError "Server contents\n%s" (dictionaryToString dimServerDict)
                        failwith msg
                | None ->
                    failwithf "ERROR on enumeration info. %s not found on server." eName)

        let numDllEnums = Seq.length dllEnums
                                    
        if numDllEnums <> dimServerDict.Count then
            logError "Server enumerations =\r\n%s" (dictionaryToString dimServerDict)
            logError "Dll enumeration = \r\n%s" (enumTupleToString dllEnums)
            failwithf "Number of %A type count mismatch: %d(local) != %d(server)" t numDllEnums dimServerDict.Count

        printfnCyan " OK!"

    let checkFunctions() =
        printfCyan "\tChecking function table..."
        let funcDict = GaudiFileDBApi.getFunctionDictionary(conn)
        let t = typeof<PsCCSDefineEnumSTDFunction>

        let funcNames = Enum.GetNames(t)                // string array

        funcNames
        |> Seq.map (fun eName -> eName, Functions.cast<int>(Enum.Parse(t, eName)))
        |> Seq.iter (fun (eName, d) -> 
            match funcDict.TryFind(eName) with
            | Some(d') ->                                               // prime version : remote(from server)
                if d <> int(d') then
                    failwith (sprintf "ERROR on function info %s : %d(local) != %d(server)" eName d d')
            | None ->
                failwithf "ERROR on function info. %s not found." eName)

        let funcLookup = buildSimpleLookup funcNames
        funcDict |> Map.toSeq |> Seq.map fst 
        |> Seq.iter (fun f ->
            if not (funcLookup.Contains f) then 
                printfnRed "Function %s not found." f)

        if funcNames.Length <> funcDict.Count then
            failwithf "Number of %A type count mismatch: %d(local) != %d(server)" t funcNames.Length funcDict.Count

        printfnCyan " OK!"

    checkDimensions()
    checkFunctions()

    printfnGreen "Done!"

type MwsServerActor(logger:ILog) =
    inherit Actor()
    
    do
        try
            checkDatabase()
        with exn ->
            MwsServerFailedMessageSubject.OnNext exn

    member val Logger = logger

    /// CPT 로부터 받은 request message 를 처리한다.
    /// WARNING: 본 routine 에서 exception 을 일으키면 안된다.
    ///
    override x.OnReceive message =
        if message.Equals("kill") then
            printfn "Got kill message"
        else if message :? Terminated then
            printfn "Got Terminated message"


        let sender = x.Sender
        let computation = async {
            use changer = consoleColorChanger ConsoleColor.Yellow
            try
                match message with
                | :? Terminated as m ->
                    logInfo "Got peer terminated message."

                | :? AmRequestCrash as m ->
                    logInfo "Got poison pill."
                    async {
                        new AmPong(m.Id, "Server is dying....") |> sender.Tell 
                        do! Async.Sleep(1000)
                        failwith m.Message
                    } |> Async.Start

                | :? AmRequestTestDetails as m ->
                    logInfo "Got test details request from %s(%s).  Processing..." m.Ip m.MacAddress

                    let (partNumber, gate, isProduction) = m.PartNumber, m.Gate, m.IsProduction
                    use mySqlConnection = mysqlCreateConnection(MwsConfig.getConnectionString())
                    let query = sprintf "CALL getTestDetails('%s', '%s', %s);" partNumber gate (isProduction.ToString())
                    logDebug "SQL: %s" query
                    let row = mysqlExecReaderIntoDataUniqueRow mySqlConnection query
                    let id = row.["id"] :?> uint32;
                    let productNumber = row.["productNumber"] :?> string
                    let pamGroup = row.["pamGroup"] :?> string
                    let product = row.["product"] :?> string
                    let productType = row.["productType"] :?> string
                    let fileStem = row.["fileStem"] :?> string
                    let version = Dsu.Common.Utilities.Tools.ForceToInt(row.["version"])
                    let pathHint = row.["pathHint"] :?> string
                    let path=
                        let productionCode = if isProduction then "f" else "e"
                        let prefix = mwsTestListPathPrefix
                        let path = sprintf @"%s\%s\%s.CpXv%02d%s" prefix pathHint fileStem version productionCode
                        //let path = sprintf @"%s\%s\p%s.v%02d%s" prefix pathHint productNumber version productionCode
                        path.Replace(@"\\", @"\")

                    let gzipped = Dsu.Common.Utilities.Core.ExtensionMethods.EmZip.ZippedBytesFromFile(path)
                    let reply = new AmReplyTestDetails(Message="reply details",
                                    Id=m.Id, PdvId=id, TestListFilePath=path, ProductNumber=productNumber, Version=version,
                                    Product=product, ProductType=productType, PamGroup=pamGroup, FileStem=fileStem, ZippedFileBytes=gzipped)
                    reply |> sender.Tell 
                    logInfo "Done!"






                | :? AmRequestStepsOnGaudiFile as m ->
                    logInfo "Got step request message from %s(%s).  Processing..." m.Ip m.MacAddress
                    let pdvId = m.PdvId
                    let (path, productNumber, gate) = getTestDetailsFromPdvId(pdvId)
                    counter <- counter + 1
                    logDebug "[%d-th] Server: AmRequestStepsOnGaudiFile (Path=%s, Gate=%s)" counter path gate
                    if sender = null then
                        logError "No sender defined for AmRequestStepsOnGaudiFile message."
                    else
                        let now = System.DateTime.Now
                        logDebug "Found sender: Client should echo back %A" now

                        // MWS 서버이므로, CPT client 가 여럿이 동시에 접근할 수 있으므로, 이를 위해서 개별 connection 을 따로 만들어서 사용해야 한다.
                        // 그러지 않으면, MySql.Data.MySqlClient.MySqlException 발생할 수 있음.
                        // There is already an open DataReader associated with this Connection which must be closed first
                        use conn = mysqlCreateConnection(MwsConfig.getConnectionString())
                        let (steps, pdvId) = UploadStepFromGaudiFile conn pdvId
                        let msg = sprintf "Echoing this message: %A %A" now path
                        let reply = AmReplySteps(steps, pdvId, Message=msg, Id=m.Id)
                        logDebug "Sending reply... "
                        reply |> sender.Tell 
                        logInfo "Done!"


                | :? AmRequestUploadCpXml as m ->
                    logInfo "Got upload CpXml [%s] request for pdvId=%d" m.Path m.PdvId
                    let serverCpXmlPath = 
                        let row =
                            use conn = mysqlCreateConnection(MwsConfig.getConnectionString())
                            let query = sprintf "SELECT pathHint, fileStem, version, isProduction from pdvj_vw where pdvId=%d;" m.PdvId
                            mysqlExecReaderIntoDataUniqueRow conn query
                        let fileStem = row.["fileStem"] :?> string
                        let version = Dsu.Common.Utilities.Tools.ForceToInt(row.["version"])
                        let productionCode = if (row.["isProduction"] :?> bool) then "f" else "e";
                        let pathHint = row.["pathHint"] :?> string

                        sprintf @"%s\%s\%s.CpXv%02d%s" mwsTestListPathPrefix pathHint fileStem version productionCode
                    try                        
                        EmZip.ZippedBytesToFile(serverCpXmlPath, m.ZippedFileBytes)
                        new AmReplyOK(Message=serverCpXmlPath, Id=m.Id)|> sender.Tell 
                    with exn ->
                        new AmError(ErrorCodeEnums.FailedToCreateCpXml, exn.Message, Id=m.Id) |> sender.Tell

                | :? AmRequestAssembliesVersion as m ->
                    logInfo "Got version request message from %s(%s).  Processing..." m.Ip m.MacAddress
                    let versionInfos = 
                        System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                        |> Seq.map (fun a -> sprintf "%s\t%A\r\n" a.Name a.Version)
                        |> Seq.fold (fun acc elem -> acc + elem) ""

                    printfnd "Replying with versions\r\n%s" versionInfos
                    new AmReplyAssembliesVersion(versionInfos, Message=versionInfos, Id=m.Id)
                        |> sender.Tell 
                    logInfo "Done!"

                | :? AmRequestCreateFolder as m ->
                    logInfo "Got folder creation request from PDV %s(%s): %s..." m.Ip m.MacAddress m.Message
                    try
                        let createFolder dir =
                            if IO.Directory.Exists(dir) then
                                logWarn "Directory %s already exists." dir
                            else
                                IO.Directory.CreateDirectory dir |> ignore
                                logInfo "Created folder %s" dir
                        
                        let flashFolder = sprintf @"%s\%s" MwsConfig.mwsFlashPathPrefix m.ProductNumber
                        let testlistFolder = sprintf @"%s\%s%s" MwsConfig.mwsTestListPathPrefix m.Product m.ProductType
                        createFolder flashFolder
                        createFolder testlistFolder

                        new AmPong(m.Id, (sprintf "created %s, %s" flashFolder testlistFolder)) |> sender.Tell 
                    with exn ->
                        new AmError(ErrorCodeEnums.FailedToCreateFolder, exn.Message, Id=m.Id) |> sender.Tell 
                    logInfo "Done!"

                    

                | :? AmPing as m ->
                    logInfo "Got ping message from %s(%s): %s..." m.Ip m.MacAddress m.Message
                    new AmPong(m.Id, "Pong") |> sender.Tell 
                    logInfo "Done!"

                | :? string as m ->
                    logError "Got unexpected string message %s" m
                    "[" + m + "]" |> sender.Tell 
                    logInfo "Done!"

                | :? ActorMessageBase as m -> 
                    let msg = sprintf "Got unknown actor message type %A!!!" (m.GetType())
                    logError "%s" msg
                    new AmError(ErrorCodeEnums.UnknownError, msg, Id=m.Id) |> sender.Tell 
                | _ -> 
                    logError "Got unknown message type!!!"
            with exn ->
                exn.ToString() |> logError "Exception occurred on mwsServer:\r\n%s" 
                MwsServerFailedMessageSubject.OnNext message

                try
                    match message with
                    | :? ActorMessageBase as m ->
                        new AmError(ErrorCodeEnums.UnknownError, exn.Message, Id=m.Id) |> sender.Tell 
                    | _ as m ->
                        logError "Reporting error on Unknown request."
                with exn2 ->
                    logError "Failed to generate exception detail info." 
                //raise exn
        }
        computation |> Async.StartAsTask |> ignore



let internal maxPayloadBytes = MwsConfig.mwsActorMaxPayloadBytes.ToString()
// the most basic configuration of remote actor system
// http://stackoverflow.com/questions/36685326/max-allowed-size-128000-bytes-actual-size-of-encoded-class-scala-error-in-akk
// http://kataribe.naist.jp/akkadotnet/akka.net/blame/f4bef5a991dcb27d8835c254370b34692803d017/src/Akka.Remote/Configuration/Remote.conf
let internal config = """
        akka {
            suppress-json-serializer-warning = on  
            actor {
                provider = "Akka.Remote.RemoteActorRefProvider, Akka.Remote"
                debug {
                    receive = on
                    autoreceive = on
                    lifecycle = on
                    event-stream = on
                    unhandled = on
                }
            }    
            remote {
                # enabled-transports = ["akka.remote.helios.tcp", "akka.remote.helios.udp"]  # http://getakka.net/docs/remoting/transports
                enabled-transports = ["akka.remote.helios.tcp"]
                maximum-payload-bytes = """ + maxPayloadBytes + """ bytes
                # http://stackoverflow.com/questions/17360303/akka-remote-system-shutdown-leads-to-endpointdisassociatedexception
                # http://getakka.net/docs/concepts/configuration
                # log-remote-lifecycle-events = off
                log-remote-lifecycle-events = INFO
                helios.tcp {
                    # tcp-reuse-addr = off
                    transport-protocol = tcp
                    port = """ + MwsConfig.mwsPort.ToString() + """
                    hostname = """ + MwsConfig.mwsServer + """
                    public-hostname = """ + MwsConfig.mwsServer + """
                    send-buffer-size = """ + maxPayloadBytes + """b
                    receive-buffer-size = """ + maxPayloadBytes + """b
                    message-frame-size = """ + maxPayloadBytes + """b
                    maximum-frame-size = """ + maxPayloadBytes + """b
                    # http://stackoverflow.com/questions/31753052/akka-net-remote-disconnection
                    tcp-keepalive = on
                }
            }
        }
        """



let CreateMwsServerActor(logger: ILog) =
    Log4NetWrapper.SetLogger(logger)
    printMwsConfiguration()

    // remote system only listens for incoming connections
    // it will receive actor creation request from local-system (see: FSharp.Deploy.Local)
    logInfo "Creating actor system.."
    let system = System.create MwsConfig.mwsActorSystemName (Configuration.parse config)
    logInfo "Actor system created."

    let args : obj array = [|logger|]
    let mwsServerActor = system.ActorOf(Props(typedefof<MwsServerActor>, args), MwsConfig.mwsActorName)
    system, mwsServerActor




