open System

try
    failwith "this is exception!!"
    raise (Exception("This is raised exception"))
    failwithf "this is formatted %s" "exception"
with exn ->
    printf "I got Exception: %O" exn


try
    raise (InvalidOperationException("This is raised exception"))
    raise (ArgumentNullException("Argument Null"))
    failwith "this is exception!!"
with
    | :? InvalidOperationException -> printfn "[[ Invalid operation ]]"
    | :? ArgumentNullException -> printfn "[[ Argument Null ]]"



try
    raise (ArgumentNullException("Argument Null"))
    raise (InvalidOperationException("This is raised exception"))
    failwith "this is exception!!"
with exn ->
    match exn with
    | :? InvalidOperationException as exIOE -> printfn "[[ Invalid operation: %O ]]" exIOE
    | :? ArgumentNullException -> printfn "[[ Argument Null: %O ]]" exn
    | _ -> printfn "Unknown error!"

try
    raise (ArgumentNullException("Argument Null"))
    raise (InvalidOperationException("This is raised exception"))
    failwith "this is exception!!"
with exn ->
    match exn with
    | :? InvalidOperationException -> printfn "[[ Invalid operation: %O ]]" exn
    | :? ArgumentNullException -> printfn "[[ Argument Null: %O ]]" exn
    | _ -> printfn "Unknown error!"



try
    failwith "this is exception!!"
with
    | exn when false -> printfn "[[ This should not be printed. ]]"
    | exn when true -> printfn "[[ Got excpetion ]]"


try
    failwith "exception2"
    failwith "exception1"
    failwith "this is exception!!"
with
    | Failure("exception1") -> printfn "[[ exception1 ]]"
    | Failure("exception2") -> printfn "[[ exception2 ]]"
    | _ -> printfn "[[ Unknow error ]]"





// https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/exception-handling/the-try-with-expression
exception Error1 of string
exception Error2 of string * int
try
    raise (Error1("x"))
    raise (Error2("x", 10))
   with
      | Error1(str) -> printfn "Error1 %s" str
      | Error2(str, i) -> printfn "Error2 %s %d" str i




