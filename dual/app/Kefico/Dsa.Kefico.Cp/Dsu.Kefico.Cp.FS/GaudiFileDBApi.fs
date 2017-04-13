module GaudiFileDBApi



open System
open System.IO 
open System.Text.RegularExpressions
open MySql.Data.MySqlClient
open Functions
open GaudiFileParserApi
open MySQLComputationExpression
open MwsConfig


/// <summary>
/// kefico.dimension table 에 대한 lookup
/// </summary>
let mutable internal dimenisionDictionary = Map.empty
let mutable internal functionDictionary = Map.empty
let internal getDimensionDictionary (conn: MySqlConnection) =
    if dimenisionDictionary.IsEmpty then
        printfd "\tExtracts dimension information.."
        let dimTable = mysqlExecReaderIntoDataTable conn "SELECT id, name FROM dimension;"
        dimenisionDictionary <-
            [ for row in dimTable.Rows ->
                row.["name"] :?> string, row.["id"] :?> uint16 ]
            |> Map.ofSeq

    dimenisionDictionary

let internal getFunctionDictionary (conn: MySqlConnection) =
    if functionDictionary.IsEmpty then
        printfd "\tExtracts function information.."
        let dimTable = mysqlExecReaderIntoDataTable conn "SELECT id, name FROM function;"
        functionDictionary <-
            [ for row in dimTable.Rows ->
                row.["name"] :?> string, row.["id"] :?> uint32 ]
            |> Map.ofSeq

    functionDictionary


let private getDbString(str) =
    match str with
    | "" -> "NULL"
    | a -> escapeQuote a |> singleQuote

/// DB server 내의 PamType 을 parser 가 인지하는 type 으로 변환
let internal pamTypeToGate(pamType) =
    match pamType with
    | "CT" -> "T"
    | "HT" -> "H"
    | "FT" -> "R"
    | "RW" | _  -> failwithlog (sprintf "Unknown gate type for pamType %s" pamType)

let internal fillStepTable conn (pdvId:uint32) values =
    // creates tt_step_parsed temporary table
    let createTemporaryStepTable() = 
        try
            printfn "CALL createTemporaryStepTable()..."
            mysqlExecNonQuery conn "CALL createTemporaryStepTable()" |> ignore

            (*
             * 대량의 data insert 시, 일반적으로 Prepared command 을 추천하지만, 실제 써 보니 더 느려서 text 기반 insert 방식으로 변경함.
             * see svn version @344 for this file.
             *)
            let sql = "INSERT INTO tt_step_parsed(pdvId, position, step, min, max, dim, fncId, modName, parameter, comment)\r\nVALUES\r\n\t" + values + "\r\n;"                
            mysqlExecNonQuery conn sql |> ignore

            printfn "   Finished prepared command execution."
        with exn ->
            raise exn

    let mergeStepFromTemporaryTable() =
        printfn "CALL mergeStepFromTemporaryTable()..."
        let sql = System.String.Format("CALL mergeStepFromTemporaryTable({0})", pdvId);
        mysqlExecNonQuery conn sql |> ignore

    createTemporaryStepTable()
    mergeStepFromTemporaryTable()

//    if MwsConfig.mwsEnableDebug then
//        // for debugging purpose.
//        printf "Fixing tt_step_parsed Table.."
//        let fixTemporaryTable temp fixedTable =
//            MySQLApi.executeNonQuery conn (sprintf "DROP TABLE IF EXISTS %s;" fixedTable) |> ignore
//            MySQLApi.executeNonQuery conn (sprintf "CREATE TABLE %s SELECT * FROM %s;" fixedTable temp) |> ignore
//
//        fixTemporaryTable "tt_step_parsed" "ttstep_parsed"
//        fixTemporaryTable "tt_step_natural_join" "ttstep_natural_join"
//        fixTemporaryTable "tt_step_report" "ttstep_report"
//        printfn "Done!"

    mysqlExecReaderIntoDataTable conn "SELECT * FROM tt_step_report;"

let internal getTestDetailsFromPdvId(pdvId) =
    use mySqlConnection = mysqlCreateConnection(MwsConfig.getConnectionString())
    let query = sprintf "CALL getTestDetailsFromPdvId(%d);" pdvId
    logDebug "SQL: %s" query
    let table = mysqlExecReaderIntoDataTable mySqlConnection query

    if table.Rows.Count <> 1 then
        failwithf "Failed to get test information details from server with pdvId=%d" pdvId

    let row = table.Rows |> Seq.cast<System.Data.DataRow> |> Seq.head
    let id = row.["id"] :?> uint32;
    let productionCode = if (row.["isProduction"] :?> bool) then "f" else "e";
    let partNumber = row.["partNumber"] :?> string
    let productNumber = row.["productNumber"] :?> string
    let pamType = row.["pamType"] :?> string            // gate
    let product = row.["product"] :?> string
    let productType = row.["productType"] :?> string
    let fileStem = row.["fileStem"] :?> string
    let version = row.["version"] :?> uint32;
    let pathHint = row.["testListPathHint_gc"] :?> string
    let filePath = sprintf @"%s\%s\%s.CpXv%02d%s" MwsConfig.mwsTestListPathPrefix pathHint fileStem version productionCode

    if id <> pdvId then 
        failwithlog (sprintf "Internal error.  pdvid(%d) != id(%d)" pdvId id)

    logInfo "Testlist info : pdvId=%d, partNumber=%s, productNumber=%s, pamType=%s, version=%d productionCode=%s"
            id partNumber productNumber pamType version productionCode

    filePath, productNumber, pamTypeToGate(pamType)

/// <summary>
/// 1. 주어진 pdvId 로 gaudi file path 및 최종 수정 시간 정보를 획득
/// 1. gaudi file parsing 해서 steps 정보 획득
/// 1. conn 을 이용하여 server 로부터 gaudi file 정보 및 현존 step 정보 획득
/// </summary>
/// <param name="conn"></param>
/// <param name="pdvId"></param>
/// <returns>Parsing 된 step 정보 : stored procedure 호출을 통해서 conn 에 temporary table tt_step_report 를 만들고 그 값을 읽어서 반환</returns>
let UploadStepFromGaudiFile (conn: MySqlConnection) (pdvId:uint32) = //productNumberHint (gateHint:string) =    
    logInfo "Uploading test list for pdvId: %d" pdvId
    let upload() =


        let (filePath, productNumber, gate)  = getTestDetailsFromPdvId(pdvId)


        Directory.GetCurrentDirectory() |> printfn "%A" 
        let date = File.GetLastWriteTime filePath
        let steps = 
            ParseGaudiFile filePath productNumber gate     // todo : which one?? partNumber or productNumber??  -- maybe productNumber




        // tt_step_parsed 에 삽입할 values () 라인들을 생성함
        let values =
            let stringFromOption (opt: 'a option) = match opt with | Some(v) -> v.ToString() | None -> "NULL"
            let dimDict = getDimensionDictionary conn

            steps
            |> Seq.map(fun s -> 
                    let dimString = s.dim.ToString()
                    let dimension = Map.find dimString dimDict
                    let modName = getDbString(s.modName)
                    let fncId = (uint32)s.func
                    let parameter = getDbString(s.parameter)
                    let comment = getDbString(s.comment)
                    sprintf "(%d, %d, %d, %s, %s, %d, %d, %s, %s, %s)"
                        pdvId s.positionNumber s.stepNumber (stringFromOption s.min) (stringFromOption s.max) dimension fncId modName parameter comment
                )
            |> Seq.fold(fun acc elem -> (if acc = "" then "" else acc + "\r\n\t, ") + elem) ""

        let result = 
            let fillStep() = 
                let f() = fillStepTable conn pdvId values
                let onFailure exn = logError "%s" (exn.ToString())
                MySQLApi.withTransaction conn f onFailure

            let timeLimit = (1000 * MwsConfig.mwsServerTaskCompletionTimeoutSecond)
            Functions.withTimeLimit fillStep timeLimit "fillStep"

        match result with
            | Some(v) -> 
                logInfo "Finished upload step with success: %d steps" v.Rows.Count
                //v.ConvertToString() |> printfn "%s" 
                (v |> Step.ofDataTable), pdvId
            | None ->
                let msg = "Failure on UploadStepFromGaudiFile."
                logError "%s" msg
                failwith msg


    let (result, time) = Functions.duration (fun () -> upload())
    logInfo "Parsing + Uploading to database server took %f seconds." ( (float time) / 1000.0)
    result
