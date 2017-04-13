namespace Dsu.Driver.Math

open Dsu.Driver.Math.Statistics
open FSharp.Collections.ParallelSeq
open MathNet.Numerics.Statistics

/// <summary>
/// 측정 data 해석을 위한 class
/// </summary>
type DaqWidthAnalyzer(data: float array, samplingRate:float, min:float, trigger:float, max:float) =
    // rising / falling edge 갯수 counting
    let numRising, numFalling, widths, duties =
        let mutable numRising = 0
        let mutable numFalling = 0
        let mutable risingIndex = None
        let mutable fallingIndex = None
        let mutable widths = []
        let mutable duties = []
        let mutable duty = None
        let isHigh(v) =
            if v < min || v > max then None
            else Some(min <= v && v <= trigger)

        data |> Array.iteri(fun i d ->
            if i > 0 then
                match isHigh(data.[i-1]), isHigh(data.[i]) with
                | Some(true), Some(false) ->
                    numFalling <- numFalling + 1    // FALLING
                    
                    match fallingIndex with
                    | Some(v) -> 
                        widths <- (i-v)::widths
                    | _ -> ()

                    match risingIndex with
                    | Some(v) -> 
                        duties <- (i-v)::duties
                        risingIndex <- None
                    | _ -> ()

                    fallingIndex <- Some(i)
                    
                    
                    
                | Some(false), Some(true) ->
                    numRising <- numRising + 1      // RISING
                    risingIndex <- Some(i)
                | Some(h1), Some(h2) -> ()
                | _ ->
                    printfn "Unexpected case 2."
                    ()







//                match decisions.[i-1].IsHigh, decisions.[i].IsHigh with
//                | true, false ->
//                    numFalling <- numFalling + 1    // FALLING
//                    match fallingIndex with
//                    | Some(v) -> 
//                        widths |> List.add (i-v)
//                    | _ -> ()
//                    match risingIndex with
//                    | Some(v) -> 
//                        duties |> List.add (i-v)
//                    | _ -> ()
//                    fallingIndex = Some(i)
//                    
//                    
//                    
//                | false, true ->
//                    numRising <- numRising + 1      // RISING
//                    risingIndex = Some(i)
//                | _ -> ()
        )
        numRising, numFalling, widths |> List.rev |> Array.ofList, duties |> List.rev |> Array.ofList

    member __.Widths with get() = widths
    member __.Duties with get() = duties