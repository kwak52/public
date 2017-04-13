
open System.Drawing

type Shape =
    | Rectangle of Point * Point
    | Ellipse of Point * Point
    | Composed of Shape * Shape



type MyOption<'T> =
    | MySome of 'T
    | MyNone


let opt = MySome(10)
let v = match opt with
    | MySome(v) -> v
    | MyNone -> -1
printfn "%d" v


type 'T MySecondOption =
    | MySecondSome of 'T
    | MySecondNone




type 'T list =
    | ([])
    | (::) of 'T * 'T list


type Tree<'T> =
    | Tree of 'T * Tree<'T> * Tree<'T>
    | Tip of 'T

let rec sizeOfTree tree =
    match tree with
    | Tree(_, l, r) -> 1 + sizeOfTree l + sizeOfTree r
    | Tip _ -> 1
let smallTree = Tree ("1", Tree ("2", Tip "a", Tip "b"), Tip "c");;
sizeOfTree smallTree;;




type Node =
    {   Name : string
        Links : Link list }
and Link =
    | Dangling
    | Link of Node









let (n1: option<System.Random>) = None       // val n : System.Random option = None
let n2 = (None: option<System.Random>)       // val n : System.Random option = None
let n3 = None                                // val n : 'a option
let o2 = Some(1)
printfn "%d" (Option.get o2)
Option.get (Some(100)) |> printfn "%d"

let a = Some(10)

let printCity(cityInfo) =
    printfn "Population of %s is %d."
        (fst cityInfo) (snd cityInfo)

let prague = ("Prague", 1188126)
printCity(prague)

let (_, population) = prague





#load "Functions.fs"
open Functions

let add3 x = x+3
let nn1 = twice add3 3
let nn2 = twice ((+) 2) 3


let x33 = ntimes 10 add3 3
let n32 = successor(31)


let squareIt = fun n -> n * n
let doubleIt = fun n -> n * 2
let doubleAndSquare = compose squareIt doubleIt
let x18 = doubleAndSquare 3





type DiscreteEventCounter =
    {
        mutable Total : int;
        mutable Positive : int;
        Name : string
    }

let recordEvent (s : DiscreteEventCounter) isPositive =
    s.Total <- s.Total + 1
    if isPositive then s.Positive <- s.Positive + 1

let reportStatus (s : DiscreteEventCounter) =
    printfn "We have %d %s out of %d" s.Positive s.Name s.Total

let newCounter nm =
    { Total = 0;
        Positive = 0;
        Name = nm }






// https://bradcollins.com/2015/10/16/f-friday-seq-choose/
//
// Filtering over a sequence of values omits values that do not meet certain criteria. 
// Mapping over a sequence of values transforms each value into another value. 
// What if you could do both at the same time—filter out unwanted values, 
// but transform the ones that are left? You can with Seq.choose.
type Order =
| Fulfilled of id:string * total:decimal
| Cancelled of id:string * total:decimal


let orders = [
    Fulfilled ("fef3356074b4", 28.50m)
    Fulfilled ("2605c9988f1d", 88.25m)
    Cancelled ("94edac47971f", 22.01m)
    Fulfilled ("2a1ff57b8f46", 39.30m)
    Fulfilled ("9ee0a3e3da3a", 27.97m)
    Cancelled ("db5dc439ad93", 99.49m)
    Fulfilled ("08d58811ed36", 53.72m)
    Cancelled ("63ebd07475ca", 93.66m)
    Cancelled ("12d16ae9c112",  7.79m)
    Fulfilled ("c5ecedaedb0e", 87.21m)
]
 
let cancelledDollars = 
    orders 
    |> Seq.choose (function     // "function" only allows for one argument but allows for pattern matching, while "fun" is the more general and flexible way to define a function.
                   | Cancelled (_, dollars) -> 
                        Some dollars 
                   | _ -> None)
    |> Seq.sum
// val cancelledDollars : decimal = 222.95M

let cancelledIds = 
    orders
    |> Seq.choose ( function
                    | Cancelled (id, dollars) -> Some (id, dollars)
                    | _ -> None)
    |> Array.ofSeq






