
#load "MaybeBuilder.fs"

let divideByWorkflow init x y z = 
    let divideBy bottom top =
        if bottom = 0
        then None
        else Some(top/bottom)


    maybe 
        {
        let! a = init |> divideBy x
        let! b = a |> divideBy y
        let! c = b |> divideBy z
        return c
        }    

let good = divideByWorkflow 12 3 2 1
let bad = divideByWorkflow 12 3 0 1

