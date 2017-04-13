namespace Dsu.Common.Utilities.FS

open System.Runtime.CompilerServices

[<assembly:Extension>]
do ()


//module Async = 

[<AutoOpen>]
[<Extension>]
type AsyncExt () =
    /// F# Async<'T> (Microsoft.FSharp.Control.FSharpAsync) 를 C# Task<T> 로 변환
    [<Extension>]
    static member ToTask (x: Async<'T>) cancellationToken =         // System.Threading.CancellationTokenSource
        Async.StartAsTask(x, ?cancellationToken=cancellationToken)
