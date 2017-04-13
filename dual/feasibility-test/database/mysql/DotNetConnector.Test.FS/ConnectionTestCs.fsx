// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

// Define your library scripting code here

#r "System.Configuration.dll"
#r "bin/Debug/MySQL.data.dll"
#r "bin/Debug/Dsu.DB.MySQL.dll"



open MySql.Data.MySqlClient
open Dsu.DB.MySQL

let connStr = "server=dualsoft.co.kr;user=securekwak;database=kefico;port=3306;password=kwak;Allow User Variables=True"


let conn = new MySqlConnection(connStr)
conn.Open() |> ignore
conn.ExecuteNonQuery("SET @user='kim';")
let kwak = conn.ExecuteScalar("SELECT @user;")
