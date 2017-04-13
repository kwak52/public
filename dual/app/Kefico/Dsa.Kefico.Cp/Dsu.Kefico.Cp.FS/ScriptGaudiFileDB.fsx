// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r "System.Configuration.dll"

#I "../bin"
#r "MySQL.data.dll"
#r "Dsu.DB.FS.dll"
#r "Dsu.Kefico.Cp.FS.dll"


printfn __SOURCE_DIRECTORY__

System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__ + @"\..\bin")

open MySql.Data.MySqlClient
open GaudiFileParserApi
open MwsConfig



    
let connStr = MwsConfig.getConnectionString()
let conn = new MySqlConnection(connStr)
conn.Open() |> ignore

filePathFromPdvId conn 5
    |> Option.toObj


UploadStepFromGaudiFile conn 5
// UploadGaudiFile conn "../p9000120000.CpXml"



let wrap f =
    printfn "Begin"
    f()
    printfn "End"

let wrapTest a =
    printfn "Main Begin"
    let f() = printfn "Hello"
    wrap f
    printfn "Main End"

wrapTest 1




let tupler() =
    (1, 2)

let (a, b) = tupler()



let enclose b e string:string =
    b + string + e

enclose "'" "'" "aaa"