
let data = [1..100]
let elems = data.[1..3]


let mydata = 
    seq {
        yield 1
        yield 2
    }

let ourdata =
    seq {
        yield 5
        yield 6
        yield! mydata
    }

let mine = mydata |> List.ofSeq
let ours = ourdata |> List.ofSeq












#load "ExtensionMethod.fs"       
open Dsu.Common.Utilities.FS
clip 5 10 100





let duration f =
    let timer = new System.Diagnostics.Stopwatch()
    timer.Start()
    let result = f()
    let nano = (timer.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency) * 1000000000L
    (result, nano)

let mcSamples: double array array = Array.zeroCreate 4

let alloc() = 
    // 각 채널별로 sampling 갯수 만큼의 공간 확보
    for ch in 0..3 do
        mcSamples.[ch] <- Array.zeroCreate 1000000
let t = duration alloc
t

let x = duration (fun () -> printfn "%s" "hello")






// do nothing : _noop = ()
match [1..10] with
  | [] -> printfn "Empty!"
  | _ -> ()



open System
let (succ, num) = Int32.TryParse("3")

let (succ2, num2) = Int32.TryParse("3.1")



for i in [1..10] do
    printfn "%d" i








let o1 = Some(1)
let v1 = o1 |> Option.get

let s1 = Some("AAA")
let vs1 = s1 |> Option.toObj





let generateCircularSeq (lst:'a list) = 
    let rec next () = 
        seq {
            for element in lst do
                yield element
            yield! next()
        }
    next()

for i in [1;2;3;4;5;6;7;8;9;10] |> generateCircularSeq |> Seq.take 12 do
    i |> System.Console.WriteLine 



let unsort xs =
        let rand = System.Random(Seed=0)
        let rand = System.Random()
        xs
        |> Seq.map (fun x -> rand.Next(),x)
        |> Seq.cache
        |> Seq.sortBy fst
        |> Seq.map snd

unsort ['1' .. '9'] |> Seq.toList
unsort [1..9] |> Seq.toList
unsort ['a' .. 'z'] |> Seq.toList





type MyMaybeBuilder() =
    member __.Bind(x, f) = 
        match x with
        | Some(v) -> f v
        | None -> None
    member __.Return(x) = Some(x)

let maybe = new MyMaybeBuilder()

let divideBy y x =
    if y = 0 then None else Some(x/y)

let divideByWorkflow init x y z = 
    maybe 
        {
        let! a = init |> divideBy x
        let! b = a |> divideBy y
//        let! c = b |> divideBy z
        return b |> divideBy z
        //return c
        }    
let good = divideByWorkflow 12 3 2 1
let bad = divideByWorkflow 12 3 0 1












// Programming F#, pp. 249

type RoundingWorkflow(sigDigs : int) =
    let round x = System.Math.Round(double x, sigDigs)
    // Due to result being constrained to type float, you can only use
    // let! against float values. (Otherwise will get a compiler error.)
    member this.Bind(result, rest) =
        let result' = round result
        rest result'
    member this.Return x = round x
let withPrecision sigDigs = new RoundingWorkflow(sigDigs)



let test = withPrecision 12 {
    let! x = 2.0 / 12.0
    let! y = 3.5
    return x / y
}





// Beginning F# 4.0 pdf, pp.37
let result =
    if System.DateTime.Now.Second % 2 = 0 then
        box "heads"
    else
        box false
printfn "%A" result






//http://www.fssnip.net/nQ/title/Agent-demo
// http://stackoverflow.com/questions/3443875/mailboxprocessor-usage-guidelines
MailboxProcessor.Start(fun inbox ->
    async {
      while true do
        let! message = inbox.Receive()
        processMessage(message)
    })




open System
open System.Threading
let withTimeLimit f (limitMilli:int) =
    let cts = new CancellationTokenSource();
    cts.CancelAfter(limitMilli);
    let task = Async.StartAsTask(async{return f()}, cancellationToken=cts.Token)
    if not (task.Wait(limitMilli)) then
        raise (new Exception("Timeout expired."))
        //failwith "I am failing"
    
    task.Result

let mutable succeeded = false
let f() =
    printfn "Started."
    Thread.Sleep 2000
    printfn "Finished"
    succeeded = true

try
    printfn "Starting try block"
    printfn "TASK RESULT=%A" (withTimeLimit f 100)
with exn ->
    printfn "CAUGHT EXCEPTION: %O" exn
assert succeeded
