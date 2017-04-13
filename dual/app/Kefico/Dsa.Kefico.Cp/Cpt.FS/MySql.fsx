
// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#I "../bin"
#r "System.Configuration.dll"
#r "MySQL.data.dll"
#r "Dsu.DB.FS.dll"
#r "Dsu.Kefico.Cpt.FS"

open MySql.Data.MySqlClient
open Dsu.DB.FS
open MwsConfig

// Define your library scripting code here

let connStr = DatabaseConfig.connectionString

let conn = new MySqlConnection(connStr)
conn.Open() |> ignore
conn.ExecuteNonQuery("SET @user='kim';")
let kwak = conn.ExecuteScalar("SELECT @user;")
printfn "%A" kwak