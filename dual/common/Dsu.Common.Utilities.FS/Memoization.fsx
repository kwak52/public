#load "Memoization.fs"
open Memoization
open System.Collections.Generic

// Calculate 2! for the first time
factorialMem(2)
// Use the cached value
factorialMem(2)
// The value of 2! is being recalculated!!
factorialMem(3)




let nums = Seq.unfold (fun num ->
    if (num <= 10) then Some(string(num), num + 1) else None) 0
nums |> List.ofSeq





// pp.77, Expert F# 4.0, 4th
let isWord (words : string list) =
    printfn "Calculating words, JUST ONE TIME. 반복적호출해도 한번만 수행된다."
    let wordTable = Set.ofList words
    fun w -> wordTable.Contains(w)      // function 을 반환

let isCapital = isWord ["London"; "Paris"; "Warsaw"; "Tokyo"];;
isCapital "Paris";;
isCapital "Manchester";;

// 잘못된 구현예제.
let isCapitalSlow word = isWord ["London"; "Paris"; "Warsaw"; "Tokyo"] word
isCapitalSlow "Paris";;
isCapitalSlow "Manchester";;


// general lookup service
let capitalLookup = buildSimpleLookup ["London"; "Paris"; "Warsaw"; "Tokyo"];;
capitalLookup.Contains "Paris";;


let rec fib =
    fun n -> if n <= 2 then 1 else fib (n - 1) + fib (n - 2)

let fibFast =
    memoize (fib)
fibFast 40      // <- 처음엔 느리나, 반복적으로 호출할 때, cache 결과를 사용함.

let rec fibNotFast n =
    memoize fib n
fibNotFast 40   // <- 계속 느림.

let fibUsingUnfold n =
    let fibs =
        (1I,1I) |> Seq.unfold
            (fun (n0, n1) ->
                Some(n0, (n1, n0 + n1)))
    Seq.item n fibs






let sixty = lazy (30 + 30);;
// val sixty : Lazy<int> = Value is not created

sixty.Force();;
// val it : int = 60




