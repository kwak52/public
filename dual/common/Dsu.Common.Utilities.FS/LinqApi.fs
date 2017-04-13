[<AutoOpen>]
module LINQApi

open System.Linq
open System.Collections.Generic

type IEnumerable<'T> with
    member x.nonNullAny() = x <> null && x.Any()
    member x.isNullOrEmpty() = x = null || Seq.isEmpty x

