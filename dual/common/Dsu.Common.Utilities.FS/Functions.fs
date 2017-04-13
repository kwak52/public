[<AutoOpen>]
module Functions

open System
open System.Threading
open System.Net
open System.Diagnostics
open Dsu.Common.Utilities.Core



/// <summary>
/// Returns tuple of function execution result and duration
/// </summary>
/// <param name="f"></param>
let duration f =
    //let timer = new System.Diagnostics.Stopwatch()
    let timer = new HighResolutionTimer()
    timer.Start()

    let result = f()
    timer.Stop()
    (result, timer.ElapsedMilliseconds)

/// Executes f twice
let twice f = f >> f

/// Function composition
let compose f g = f >> g

/// repeat f n times
let rec ntimes (n: int) f = 
    if n = 0 then (fun x -> x)
    else
        let g = ntimes (n-1) f
        compose f g


/// Returns function which get successor
let successor = fun x -> x + 1

/// Returns function which get predecessor
let predecessor = fun x -> x - 1


/// Y-combinator, or Sage bird
let rec Y f x = f (Y f) x



/// Extracts value from F# option type.  if None, it fails.
let opt2val(o : option<'T>) =
    match o with 
    | Some(v) -> v
    | None -> failwith "Failed to extract value from None option."


let isSome(o : option<'T>) =
    match o with 
    | Some(v) -> true
    | None -> false

let isNone(o : option<'T>) = 
    isSome o |> not


let convertToString x =
    try
        x |> string |> Some
    with ex -> None

let dos2unix (str:string) =
    str.Replace("\r\n", "\n")

let third (_, _, c) = c
let thd (_, _, c) = c
let fth (_, _, _, c) = c



/// <summary>
/// 주어진 sequence 를 unsort/random shuffle/scramble 한다.
/// http://www.fssnip.net/16/title/Sequence-Random-Permutation
/// </summary>
/// <param name="sqn"></param>
let scramble (sqn : seq<'T>) = 
    let rnd = new System.Random()
    let rec scramble2 (sqn : seq<'T>) = 
        /// Removes an element from a sequence.
        let remove n sqn = sqn |> Seq.filter (fun x -> x <> n)
 
        seq {
            let x = sqn |> Seq.item (rnd.Next(0, sqn |> Seq.length))
            yield x
            let sqn' = remove x sqn
            if not (sqn' |> Seq.isEmpty) then
                yield! scramble2 sqn'
        }
    scramble2 sqn

// Example:
//scramble ['1' .. '9'] |> Seq.toList
//scramble [1..9] |> Seq.toList
//scramble ['a' .. 'z'] |> Seq.toList


/// <summary>
/// http://www.fssnip.net/2n/title/Sequnsort
/// </summary>
/// <param name="xs"></param>
let unsort xs =
        //let rand = System.Random(Seed=0)
        let rand = System.Random()
        xs
        |> Seq.map (fun x -> rand.Next(),x)
        |> Seq.cache
        |> Seq.sortBy fst
        |> Seq.map snd



/// <summary>
/// 주어진 sequence 를 계속 circular population 한다.
/// http://www.fssnip.net/1N/title/Function-to-generate-circular-infinite-sequence-from-a-list
/// </summary>
/// <param name="lst"></param>
let generateCircularSeq (lst:'a list) = 
    let rec next () = 
        seq {
            for element in lst do
                yield element
            yield! next()
        }
    next()

//for i in [1;2;3;4;5;6;7;8;9;10] |> generateCircularSeq |> Seq.take 12 do
//    i |> System.Console.WriteLine 



/// <summary>
/// http://www.fssnip.net/18/title/Haskell-function-iterate
/// </summary>
/// <param name="f"></param>
/// <param name="value"></param>
let rec iterate f value = seq { 
  yield value
  yield! iterate f (f value) }

// Returns: seq [1; 2; 4; 8; ...]
//Seq.take 10 (iterate ((*)2) 1)


/// <summary>
/// http://www.fssnip.net/1R/title/Take-every-Nth-element-of-sequence
/// </summary>
/// <param name="n"></param>
/// <param name="seq"></param>
let everyNth n seq = 
    seq |> Seq.mapi (fun i el -> el, i)              // Add index to element
        |> Seq.filter (fun (el, i) -> i % n = n - 1) // Take every nth element
        |> Seq.map fst                               // Drop index from the result



let tee f x = 
    f x
    x

let escapeQuote(s:string) = s.Replace("'", @"\'")
let singleQuote(s:string) = sprintf "'%s'" s
let doubleQuote(s:string) = sprintf "\"%s\"" s




/// <summary>
/// Awaits dotnet Task, asynchronosely
/// </summary>
/// <param name="taskf">Task<'a> generating function.  Not Task<'a> itself.</param>
let awaitDotNetTask taskf =
    async {
        return! Async.AwaitTask (taskf())
    }

/// <summary>
/// Waits dotnet Task, synchornosly
/// </summary>
/// <param name="taskf"></param>
let waitDotNetTask taskf =
    awaitDotNetTask taskf |> Async.RunSynchronously



/// <summary>
/// 주어진 action f 를 주어진 시간 limitMilli 내에 수행.  실패하면 exception raise
/// C# 구현은 ToolsDateTime.ExecuteWithTimeLimit 참고
/// </summary>
/// <param name="f"></param>
/// <param name="limitMilli"></param>
let withTimeLimit f (limitMilli:int) (description:string) =
    let cts = new CancellationTokenSource();
    cts.CancelAfter(limitMilli);
    let task = Async.StartAsTask(async{return f()}, cancellationToken=cts.Token)
    if not (task.Wait(limitMilli)) then
        failwithlog (sprintf "Timeout(%d ms) expired on %s." limitMilli description)
    
    task.Result




/// <summary>
/// General type casting : http://stackoverflow.com/questions/18928268/f-numeric-type-casting
/// e.g
///     cast<int> (box 1.1) -- converts object(->float) to int
///     cast<int> 1.23 -- converts float to int
///     cast<int> "123" -- converts string to int
///     cast<int> "1.23" -- crash!!!
///     cast<float> "1.23" |> cast<int> -- converts string -> float -> int
/// </summary>
/// <param name="typecast"></param>
/// <param name="x"></param>
let cast<'a> input = System.Convert.ChangeType(input, typeof<'a>) :?> 'a

/// <summary>
/// type casting : http://stackoverflow.com/questions/18928268/f-numeric-type-casting
/// e.g
///     tryCast<int> (box 1.1) -- Some(1)
///     tryCast<int> 1.23 -- Some(1)
///     tryCast<int> "123" -- Some(123)
///     tryCast<int> "1.23" -- None
/// </summary>
/// <param name="typecast"></param>
/// <param name="x"></param>
let tryCast<'a> input =
  try Some(cast<'a> input)
  with _ -> None






let private cprintfWith endl c fmt = 
    // http://stackoverflow.com/questions/27004355/wrapping-printf-and-still-take-parameters
    // https://blogs.msdn.microsoft.com/chrsmith/2008/10/01/f-zen-colored-printf/
    Printf.kprintf 
        (fun s -> 
            let old = System.Console.ForegroundColor 
            try 
                System.Console.ForegroundColor <- c;
                System.Console.Write (s + endl)
            finally
                System.Console.ForegroundColor <- old) 
        fmt

let cprintf c fmt = cprintfWith "" c fmt
let cprintfn c fmt = cprintfWith "\n" c fmt


let printfBlack fmt = cprintf ConsoleColor.Black fmt
let printfnBlack fmt = cprintfn ConsoleColor.Black fmt
let printfDarkBlue fmt = cprintf ConsoleColor.DarkBlue fmt
let printfnDarkBlue fmt = cprintfn ConsoleColor.DarkBlue fmt
let printfDarkGreen fmt = cprintf ConsoleColor.DarkGreen fmt
let printfnDarkGreen fmt = cprintfn ConsoleColor.DarkGreen fmt
let printfDarkCyan fmt = cprintf ConsoleColor.DarkCyan fmt
let printfnDarkCyan fmt = cprintfn ConsoleColor.DarkCyan fmt
let printfDarkRed fmt = cprintf ConsoleColor.DarkRed fmt
let printfnDarkRed fmt = cprintfn ConsoleColor.DarkRed fmt
let printfDarkMagenta fmt = cprintf ConsoleColor.DarkMagenta fmt
let printfnDarkMagenta fmt = cprintfn ConsoleColor.DarkMagenta fmt
let printfDarkYellow fmt = cprintf ConsoleColor.DarkYellow fmt
let printfnDarkYellow fmt = cprintfn ConsoleColor.DarkYellow fmt
let printfGray fmt = cprintf ConsoleColor.Gray fmt
let printfnGray fmt = cprintfn ConsoleColor.Gray fmt
let printfDarkGray fmt = cprintf ConsoleColor.DarkGray fmt
let printfnDarkGray fmt = cprintfn ConsoleColor.DarkGray fmt
let printfBlue fmt = cprintf ConsoleColor.Blue fmt
let printfnBlue fmt = cprintfn ConsoleColor.Blue fmt
let printfGreen fmt = cprintf ConsoleColor.Green fmt
let printfnGreen fmt = cprintfn ConsoleColor.Green fmt
let printfCyan fmt = cprintf ConsoleColor.Cyan fmt
let printfnCyan fmt = cprintfn ConsoleColor.Cyan fmt
let printfRed fmt = cprintf ConsoleColor.Red fmt
let printfnRed fmt = cprintfn ConsoleColor.Red fmt
let printfMagenta fmt = cprintf ConsoleColor.Magenta fmt
let printfnMagenta fmt = cprintfn ConsoleColor.Magenta fmt
let printfYellow fmt = cprintf ConsoleColor.Yellow fmt
let printfnYellow fmt = cprintfn ConsoleColor.Yellow fmt
let printfWhite fmt = cprintf ConsoleColor.White fmt
let printfnWhite fmt = cprintfn ConsoleColor.White fmt




let consoleColorChanger(color) =
    let crBackup = System.Console.ForegroundColor
    System.Console.ForegroundColor <- color
    let disposable = 
        { new IDisposable with
            member x.Dispose() = System.Console.ForegroundColor <- crBackup }
    disposable




//let printfnd fmt = printfn fmt

(*
 * http://stackoverflow.com/questions/11559440/how-to-manage-debug-printing-in-f
 * http://www.fssnip.net/M
 * Akka actor 와 함께 사용하면, release version 에서 메시지가 제대로 동작하지 않음.
 *)

// this has the same type as printf, but it doesn't print anything
let private fakePrintf fmt =
    fprintf System.IO.StreamWriter.Null fmt


#if DEBUG
let printfnd fmt =
    printfn fmt
let printfd fmt =
    printf fmt
#else
let printfnd fmt =
    fakePrintf fmt
let printfd fmt =
    fakePrintf fmt
#endif


let getIpAddress() : string =
    let ip = Array.Find (Dns.GetHostAddresses(Dns.GetHostName()), (fun ip -> (ip.ToString()).Contains(".")))
    ip.ToString()



let inClosedRange (value:'a) ((min:'a), (max:'a)) = 
    min <= value && value <= max


let min a b = if a < b then a else b
let max a b = if a > b then a else b


let removeNewline msg:string =
    Text.RegularExpressions.Regex.Replace(msg, "[\\r\\n]*$", "")
