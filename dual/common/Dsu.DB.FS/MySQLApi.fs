[<AutoOpen>]
module MySQLApi

open System
open MySql.Data.MySqlClient

/// <summary>
/// MySQL transaction 으로 wrapping
/// </summary>
/// <param name="conn"></param>
/// <param name="f"></param>
let withTransaction (conn: MySqlConnection) f onFailure =
    let tr = conn.BeginTransaction()
    try
        let result = f()
        tr.Commit()
        Some(result)
    with ex -> 
        tr.Rollback()
        onFailure(ex)
        printfn "%A" ex
        None

let private rethrowWithLog exn sql =
    logError "MySql exception: %O" exn
    logError "FAILED QUERY=\r\n%s" sql
    raise exn

let executeNonQuery (conn: MySqlConnection) sql =
    let cmd = new MySqlCommand(sql, conn)
    cmd.ExecuteNonQuery()


let mysqlExecNonQuery conn sql =
    try
        mysql(Connection(conn), sql) {
            return! execNonQuery()
        }
    with exn -> rethrowWithLog exn sql

/// Query 실행 결과 data table 을 반환
let mysqlExecReaderIntoDataTable conn sql =
    try
        mysql(Connection(conn), sql) {
            return! execReaderIntoDataTable()
        }
    with exn -> rethrowWithLog exn sql

/// Query 실행 결과가 unique row 가 나와야 하는 경우에 실행. unique 한 DataRow 를 반환
let mysqlExecReaderIntoDataUniqueRow conn sql =
    try
        let rows = 
            let table = mysqlExecReaderIntoDataTable conn sql
            table.Rows
        if rows.Count <> 1 then
            failwithlog (sprintf "Failed to get unique row with query: %s" sql)
        rows |> Seq.cast<System.Data.DataRow> |> Seq.head
    with exn -> rethrowWithLog exn sql


/// Query 실행 결과가 unique 한 scalar 값이 나와야 하는 경우에 실행.  unqiue 한 object 를 반환
let mysqlExecScalar conn sql =
    try
        mysql(Connection(conn), sql) {
            return! execScalar()
        }
    with exn -> rethrowWithLog exn sql


let toMysqlDateTime (dt:DateTime) = dt.ToString("yyyy-MM-dd HH:mm:ss")


let mysqlCreateConnection(connStr) =
    let conn = new MySqlConnection(connStr)
    conn.Open()

    // 서버에서 설정한 max_allowed_packet 값이 불특정 시점에 제멋대로 바뀌는 현상에 대한 보정.  강제로 새로 설정함.
    let max_allowed_packet = mysqlExecScalar conn "SELECT @@max_allowed_packet;" :?> uint64
    if max_allowed_packet < uint64(64 * 1024 * 1024) then
        logWarn "Adjusting max_allowed_packet: %d -> %d" max_allowed_packet (256*1024*1024)
        mysqlExecNonQuery conn "SET GLOBAL max_allowed_packet=256*1024*1024;" |> ignore
        let conn' = new MySqlConnection(connStr)
        conn'.Open()
        conn'
    else
        conn
