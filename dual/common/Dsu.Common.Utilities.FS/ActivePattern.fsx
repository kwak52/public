open System.Text.RegularExpressions

// https://fsharpforfunandprofit.com/posts/convenience-active-patterns/

do
    printfn __SOURCE_DIRECTORY__

// create an *PARTIAL* active pattern
let (|FirstRegexGroup|_|) pattern input =
   let m = Regex.Match(input,pattern) 
   if (m.Success) then Some m.Groups.[1].Value else None  

// create a function to call the pattern
let testRegex str = 
    match str with
    | FirstRegexGroup "http://([^/]*).*" host -> 
           printfn "The value is a url and the host is %s" host
    | FirstRegexGroup ".*?@(.*)" host -> 
           printfn "The value is an email and the host is %s" host
    | _ -> printfn "The value '%s' is something else" str
   
// test
testRegex "http://google.com/test"
testRegex "http://google.com"
testRegex "alice@hotmail.com"





open System
// definition of the *COMPLETE* active pattern
let (|Bool|Int|Float|String|) input =
    // attempt to parse a bool
    let success, res = Boolean.TryParse input
    if success then Bool(res)
    else
        // attempt to parse an int
        let success, res = Int32.TryParse input
        if success then Int(res)
        else
            // attempt to parse a float (Double)
            let success, res = Double.TryParse input
            if success then Float(res)
            else String(input)
// function to print the results by pattern
// matching over the active pattern
let printInputWithType input =
    match input with
    | Bool b -> printfn "Boolean: %b" b
    | Int i -> printfn "Integer: %i" i
    | Float f -> printfn "Floating point: %f" f
    | String s -> printfn "String: %s" s

// print the results
printInputWithType "true"
printInputWithType "12"
printInputWithType "-12.1"
printInputWithType "Something else"







