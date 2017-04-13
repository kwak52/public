// https://viralfsharp.com/category/computation-expression/
// http://www.fssnip.net/8o/title/Monadic-Retry

[<AutoOpen>]
module RetryBuilder 

open System

type RetryParams = {
    maxRetries : int
    waitBetweenRetries : int
}

let defaultRetryParams = {maxRetries = 3; waitBetweenRetries = 1000}

type RetryMonad<'a> = RetryParams -> 'a
let rm<'a> (f : RetryParams -> 'a) : RetryMonad<'a> = f

let internal retryFunc<'a> (f : RetryMonad<'a>) =
    rm (fun retryParams -> 
        let rec execWithRetry f i e =
            match i with
            | n when n = retryParams.maxRetries -> raise e
            | _ -> 
                try
                    f retryParams
//                    printfn "Retrying %A for %d-th" f i
//                    let result = f retryParams
//                    printfn "Result: %A" result
//                    result
                with e ->
                    System.Threading.Thread.Sleep(retryParams.waitBetweenRetries)
                    execWithRetry f (i + 1) e

        execWithRetry f 0 (Exception()) ) 

    
type RetryBuilder() =
        
    member __.Bind (p : RetryMonad<'a>, f : 'a -> RetryMonad<'b>)  =
        rm (fun retryParams -> 
            let value = retryFunc p retryParams //extract the value
            f value retryParams                //... and pass it on
        )

    member __.Return (x : 'a) = fun defaultRetryParams -> x
    member __.Run(m : RetryMonad<'a>) = m
    member __.Delay(f : unit -> RetryMonad<'a>) = f ()


    member __.Zero() = failwith "Zero"

let retry = RetryBuilder()

