[<AutoOpen>]
module Dsu.Driver.Base.Utilities
open Dsu.Common.Utilities
open System.Threading

/// <summary>
/// Print structure of C/C++ using reflection.
/// 주로 PAIX 호출에서 return 받은 결과를 debugging 용으로 print 할 때에 사용된다.
/// </summary>
/// <param name="obj">(주로) C/C++ 구조체</param>
let PrintFields(obj) =
    logInfo "- structure %s" (obj.GetType().Name)
    let fis = FsReflection.GetFieldInfo(obj)
    fis |> Seq.iter (fun fi -> logInfo "\t%s=%A" (fst fi) (snd fi))

let PrintProperties(obj) =
    logInfo "- type %s" (obj.GetType().Name)
    let pis = FsReflection.GetPropertyInfo(obj)
    pis |> Seq.iter (fun pi -> logInfo "\t%s=%A" (fst pi) (snd pi))

/// <summary>
/// Cancel 호출을 하지않는, dummy cancellation token source.  다른 함수 호출시의 filler 로 사용
/// </summary>
let dummyCancellationToken =
    let noRequestCts = new CancellationTokenSource()     // CTS without any Cancellation Request 
    noRequestCts.Token

