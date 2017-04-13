// https://viralfsharp.com/category/computation-expression/

[<AutoOpen>]
module MySQLComputationExpression

open System
open System.Data
open MySql.Data.MySqlClient
open RetryBuilder 

// mysql command 를 수행 종료시까지 허용할 최대 시간 (초)
let mutable defaultSqlCommandTimeoutSecond = 300    // 5분


 
type sqlParams = (string * obj) []
 
type CmdSqlMonad<'a> = MySqlCommand -> 'a
 
let sqlMonad<'a> (f : MySqlCommand -> 'a) : CmdSqlMonad<'a> = f
     
let setParameters (sqlParameters : sqlParams) =
    sqlMonad(fun (cmd : MySqlCommand) -> sqlParameters |> Seq.iter(fun (name, value) -> cmd.Parameters.AddWithValue(name, value) |> ignore))
 
let setType (tp : CommandType) = sqlMonad (fun cmd -> cmd.CommandType <- tp)
 
let execReader () = 
    sqlMonad(fun cmd -> cmd.ExecuteReader())
 
let execNonQuery() =
    sqlMonad(fun cmd ->  cmd.ExecuteNonQuery())
 
let execScalar() =
    sqlMonad (fun cmd -> cmd.ExecuteScalar())

let execReaderIntoDataTable () =
    sqlMonad (fun cmd -> 
        let adaptor = new MySqlDataAdapter(cmd)
        let dt = new DataTable()
        adaptor.Fill(dt) |> ignore
        dt)
 
let command(text) = sqlMonad(fun cmd -> cmd.CommandText <- text)
 
let setTimeout(sec) = sqlMonad(fun cmd -> cmd.CommandTimeout <- sec)


type MySqlConnectionArg =
    | Connection of MySqlConnection
    | Spec of string
 
type CmdSqlBuilder (connectionArg: MySqlConnectionArg, sql) =
    let mutable connection: MySqlConnection = null
    let mutable cmd : MySqlCommand = null
    let connected = 
        match connectionArg with
        | Connection(v) -> true
        | _ -> false

    do
        match connectionArg with
        | Connection(v) -> 
            if v = null then invalidArg "connectionArg" "non-null connection must be supplied"
            connection <- v
        | Spec(s) ->
            if String.IsNullOrWhiteSpace(s) then invalidArg "connectionArg" "connection string must be supplied"
            connection <- new MySqlConnection(s)
         
    let tryConnect =
        do if connection.State <> ConnectionState.Open then
            (retry {
                return connection.Open()
            }) defaultRetryParams

    do
        tryConnect
        cmd <- new MySqlCommand(sql, connection)
        // mysql command 를 수행 종료시까지 허용할 최대 시간 (초)
        cmd.CommandTimeout <- defaultSqlCommandTimeoutSecond    // 5 분 허용
 
 
    let dispose () = 
        cmd.Dispose()
        do if not connected then
            connection.Dispose()

 
    interface IDisposable with
        member this.Dispose () =
            dispose()
            GC.SuppressFinalize(this)
 
    override __.Finalize() = dispose()
 
    member __.Command = cmd
    member __.Return ( x : 'a) : CmdSqlMonad<'a> = fun cmd -> x
    member __.Run( m : CmdSqlMonad<'a>) = m cmd
    member __.Delay(f : unit -> CmdSqlMonad<'a>) = f()
    member __.ReturnFrom(m : CmdSqlMonad<'a>) = m
                      
    member __.Bind(c : CmdSqlMonad<'a>, f : 'a -> CmdSqlMonad<'b>) =
        sqlMonad(fun cmd -> 
            let value = c cmd
            f value cmd)
 
let mysql(connection: MySqlConnectionArg, sql)  = new CmdSqlBuilder(connection, sql)

