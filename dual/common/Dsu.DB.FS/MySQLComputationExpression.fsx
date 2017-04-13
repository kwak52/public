

#I "../bin"
#r "System.Configuration.dll"
#r "MySQL.data.dll"
#r "Dsu.DB.FS.dll"
#r "Dsu.Common.Utilities.FS.dll"
#r "Dsu.Kefico.Cpt.FS"

open MySql.Data.MySqlClient
open Dsu.DB.FS
//open RetryMonad 
open MySQLComputationExpression
open MwsConfig

printfn __SOURCE_DIRECTORY__


#load "MySQLComputationExpression.fs"

// Define your library scripting code here

let connStr = DatabaseConfig.connectionString
let args : sqlParams = 
    [|
        ("@username", "kwak" :> obj)
        ("@password", "pass" :> obj)
        ("@email", "kwak@dualsoft.co.kr" :> obj)
    |]



let sql = "INSERT INTO user(username, password, email) VALUES(@username, @password, @email);"
let stringArg = Spec(connStr)
let connArg = 
    let conn = new MySqlConnection(connStr)
    conn.Open() |> ignore
    Connection(conn)

mysql(connArg, sql) {
     do! setParameters args
     return! execNonQuery()
 } //:?> string