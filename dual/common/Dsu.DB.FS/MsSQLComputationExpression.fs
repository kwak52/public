// https://viralfsharp.com/category/computation-expression/
//
// Microsoft SQL Server 용 computation expression
//
module MsSQLComputationExpression

open System
open System.Data.SqlClient
open System.Data

 
type sqlParams = (string * obj) []
 
type CmdSqlMonad<'a> = SqlCommand -> 'a
 
let sqlMonad<'a> (f : SqlCommand -> 'a) : CmdSqlMonad<'a> = f
     
let setParameters (sqlParameters : sqlParams) =
    sqlMonad(fun (cmd : SqlCommand) -> sqlParameters |> Seq.iter(fun (name, value) -> cmd.Parameters.AddWithValue(name, value) |> ignore))
 
let setType (tp : CommandType) = sqlMonad (fun cmd -> cmd.CommandType <- tp)
 
let execReader () = 
    sqlMonad(fun cmd -> cmd.ExecuteReader())
 
let execNonQuery() =
    sqlMonad(fun cmd ->  cmd.ExecuteNonQuery())
 
let execScalar() =
    sqlMonad (fun cmd -> cmd.ExecuteScalar())
 
let command(text) = sqlMonad(fun cmd -> cmd.CommandText <- text)
 
let setTimeout(sec) = sqlMonad(fun cmd -> cmd.CommandTimeout <- sec)
 
type CmdSqlBuilder (connectionString, name) =
    do
        if String.IsNullOrWhiteSpace(connectionString) then invalidArg "connectionString" "connection string must be supplied"
         
    let connection = new SqlConnection(connectionString)
    let cmd = new SqlCommand(name, connection)
 
    do
        cmd.CommandTimeout <- 60 * 20
        (retry {
            return connection.Open()
        }) defaultRetryParams
 
    let dispose () = 
        cmd.Dispose()
 
    interface IDisposable with
        member this.Dispose () =
            dispose()
            GC.SuppressFinalize(this)
 
    override this.Finalize() = dispose()
 
    member this.Command = cmd
    member this.Return ( x : 'a) : CmdSqlMonad<'a> = fun cmd -> x
    member this.Run( m : CmdSqlMonad<'a>) = m cmd
    member this.Delay(f : unit -> CmdSqlMonad<'a>) = f()
    member this.ReturnFrom(m : CmdSqlMonad<'a>) = m
                      
    member this.Bind(c : CmdSqlMonad<'a>, f : 'a -> CmdSqlMonad<'b>) =
        sqlMonad(fun cmd -> 
            let value = c cmd
            f value cmd)
 
let sqlCommand(connection, name)  = new CmdSqlBuilder(connection, name)