[<AutoOpen>]
module Memoization

open System.Collections.Generic

// Listing 10.5 Generic memoization function
let memoize(f) =    
    // Initialize cache captured by the closure
    let cache = new Dictionary<_, _>()
    (fun x ->
        let succ, v = cache.TryGetValue(x)
        if succ then v else 
        let v = f(x)
        cache.Add(x, v)
        v)

// Standard recursive factorial
let rec factorial(x) =
    printf "factorial(%d); " x
    if (x <= 0) then 1 else x * factorial(x - 1) // Recursive call

// Memoize it using 'memoize' function
let factorialMem = memoize(factorial)


/// <summary>
/// Hash 를 지원하지 않는 container 에서 임시로 빠르게 hash 를 구현하기 위한 방법.
/// </summary>
type LookupService<'a> =
    abstract Contains : 'a -> bool

/// <summary>
/// Hash 를 지원하지 않는 container 에서 임시로 빠르게 hash 를 구현하기 위한 구조를 생성.
/// e.g
///     let capitalLookup = buildSimpleLookup ["London"; "Paris"; "Warsaw"; "Tokyo"]
///     capitalLookup.Contains "Paris"
/// pp.78, Expert F# 4.0
/// </summary>
let buildSimpleLookup(data : 'a seq) =
    let wordTable = HashSet<'a>(data)
    { new LookupService<'a> with
        member t.Contains w = wordTable.Contains w }

