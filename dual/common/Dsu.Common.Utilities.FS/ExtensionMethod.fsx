// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "ExtensionMethod.fs"
open Dsu.Common.Utilities.FS
open Dsu.Common.Utilities.FS.Extension


let a = "Hello"
printfn "%A" a.Length;;
let f = a.MyLengthFunc()
let p = a.MyLengthProp

// Define your library scripting code here

let mapper = fun a -> a + 2
[1;2;3] |> List.map mapper

[1;2;3].map mapper
[1;2;3] |> List.map mapper


let filter = fun a -> a % 2 = 0
[1;2;3].where filter
[1;2;3] |> List.filter filter

