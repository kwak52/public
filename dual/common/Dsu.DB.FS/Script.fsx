// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

// trunk\app\Kefico\Dsa.Kefico.Cp\Cpt.FS\MySql.fsx 참고..


#I "bin"
#r "System.Configuration.dll"
#r "MySQL.data.dll"
#r "Dsu.DB.FS.dll"


#load "MySQL.fs"
open MySql.Data.MySqlClient
open Dsu.DB.FS
open MwsConfig

// Define your library scripting code here

let connStr = "server=dualsoft.co.kr;user=securekwak;database=kefico;port=3306;password=kwak;Allow User Variables=True"

let conn = new MySqlConnection(connStr)
conn.Open() |> ignore
conn.ExecuteNonQuery("SET @user='kim';")
let kwak = conn.ExecuteScalar("SELECT @user;")
printfn kwak