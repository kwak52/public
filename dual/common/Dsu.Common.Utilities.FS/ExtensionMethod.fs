namespace Dsu.Common.Utilities.FS
open System.Collections.Generic

// Extension method of F#.  0 Real World Functional Programming pp.239
// F# 에서는 type extension 이라고 부름
module Extension =
    //[<System.Runtime.CompilerServices.Extension>]
    type System.String with        
        member x.MyLengthFunc() = x.Length;
        member x.MyLengthProp = x.Length;

    // instead Seq<'T>
    type IEnumerable<'T> with
        member x.bind f = Seq.collect f x
        member x.map f = Seq.map f x

        member x.select f = Seq.map f x
        member x.selectMany f = Seq.collect f x
        member x.where f = Seq.filter f x
        member x.realize = Array.ofSeq x |> ignore

module Seq =
    let realize x = Array.ofSeq x |> ignore


[<AutoOpen>]
module MacroExtension =
    let clip n s e = min e (max n s)

type Class1() = 
    member this.X = "F#"
