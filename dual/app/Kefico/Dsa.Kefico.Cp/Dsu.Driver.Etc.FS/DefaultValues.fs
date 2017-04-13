[<AutoOpen>]
module Dsu.Driver.DefaultValues

open System
open Polly

let private makeRetryPolicy sleepDurations =
    let onRetry exn span counter context =
        logWarn "Exception on %d-th trial: span=%A, exn=%O" counter span exn
    Policy.Handle<Exception>().WaitAndRetry(sleepDurations, onRetry)

/// operation 실패시, retry 수행 parameter.  실패시 {1, 2, 3, 4, 5}초의 시간간격으로 총 5회 재시도
let defaultRetryPolicy = 
    let sleepDurations = {1..5} |> Seq.map (float >> TimeSpan.FromSeconds)
    makeRetryPolicy sleepDurations

let moderateRetryPolicy = 
    makeRetryPolicy [| TimeSpan.FromSeconds(0.5); TimeSpan.FromSeconds(1.0); |]

let urgentRetryPolicy = 
    let sleepDurations = {1..3} |> Seq.map (float >> TimeSpan.FromMilliseconds)
    makeRetryPolicy sleepDurations
