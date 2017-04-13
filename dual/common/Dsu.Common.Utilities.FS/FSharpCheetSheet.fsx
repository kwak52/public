// https://dungpa.github.io/fsharp-cheatsheet/



let xs = [ 1..2..9 ]
let ys = [| for i in 0..4 -> 2 * i + 1 |]
let zs = List.init 5 (fun i -> 2 * i + 1)

let xs' = Array.fold (fun str n -> 
            sprintf "%s,%i" str n) "" [| 0..9 |]

let last xs = List.reduce (fun acc x -> x) xs
let lxs = last xs


module List =

    // The apply function for lists
    // [f;g] apply [x;y] becomes [f x; f y; g x; g y]
    let apply (fList: ('a->'b) list) (xList: 'a list)  = 
        [ for f in fList do
          for x in xList do
              yield f x ]