[<AutoOpen>]
module CSharpInterop

open System


let valueFromOption (opt: 'a option) =
    match opt with
    | Some(v) -> v
    | None -> Unchecked.defaultof<'a>       // C# default('a) 에 해당.  http://stackoverflow.com/questions/2246206/what-is-the-equivalent-in-f-of-the-c-sharp-default-keyword

//let s = Some(1)
//let n = option<int>.None
//
//valueFromOption(s) -> 1
//valueFromOption(n) -> 0


let createSome (value: 'a) = Some(value)


let colorPrint (cr:ConsoleColor) (msg:string) = 
    cprintf cr "%s" msg


let PrintRed msg = colorPrint ConsoleColor.Red msg
let PrintGreen msg = colorPrint ConsoleColor.Green msg
let PrintBlue msg = colorPrint ConsoleColor.Blue msg
let PrintYellow msg = colorPrint ConsoleColor.Yellow msg
let PrintCyan msg = colorPrint ConsoleColor.Cyan msg


let PrintLineRed msg = cprintfn ConsoleColor.Red msg
let PrintLineGreen msg = cprintfn ConsoleColor.Green msg
let PrintLineBlue msg = cprintfn ConsoleColor.Blue msg
let PrintLineYellow msg = cprintfn ConsoleColor.Yellow msg
let PrintLineCyan msg = cprintfn ConsoleColor.Cyan msg


//let PrintRed msg = cprintf ConsoleColor.Red msg
//let PrintGreen msg = cprintf ConsoleColor.Green msg
//let PrintBlue msg = cprintf ConsoleColor.Blue msg
//let PrintYellow msg = cprintf ConsoleColor.Yellow msg
//let PrintCyan msg = cprintf ConsoleColor.Cyan msg
//
//
//let PrintLineRed msg = cprintfn ConsoleColor.Red msg
//let PrintLineGreen msg = cprintfn ConsoleColor.Green msg
//let PrintLineBlue msg = cprintfn ConsoleColor.Blue msg
//let PrintLineYellow msg = cprintfn ConsoleColor.Yellow msg
//let PrintLineCyan msg = cprintfn ConsoleColor.Cyan msg



(*
// F# - C# function interop
- http://blog.leifbattermann.de/2015/05/28/convert-action-to-fsharp-function/
- http://stackoverflow.com/questions/1952114/call-a-higher-order-f-function-from-c-sharp



To get an FSharpFunc from the equivalent C# function use:

Func<int,int> cs_func = (i) => ++i;
var fsharp_func = Microsoft.FSharp.Core.FSharpFunc<int,int>.FromConverter(
    new Converter<int,int>(cs_func));

To get a C# function from the equivalent FSharpFunc, use

var cs_func = Microsoft.FSharp.Core.FSharpFunc<int,int>.ToConverter(fsharp_func);
int i = cs_func(2);

So, this particular case, your code might look like:

Func<int, int> cs_func = (int i) => ++i;
int result = ApplyOn22(Microsoft.FSharp.Core.FSharpFunc<int, int>.FromConverter(
            new Converter<int, int>(cs_func)));
 *)
