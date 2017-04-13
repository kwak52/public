open System

// Abbreviation type : It’s common to use lowercase names for type abbreviations
type index = int
type flags = int64
type results = string * System.TimeSpan * int * int
type StringMap<'T> = Map<string, 'T>
type Projections<'T, 'U> = ('T -> 'U) * ('U -> 'T)
type PingPong = Ping | Pong


type Person =
    {   Name : string
        DateOfBirth : DateTime }

type Company =
    {   Name: string
        Address : string }

let person = {Name = "kwak"; DateOfBirth = DateTime.Now}        // <- field 구분자가 명확하면 OK

type Dot = { X : int; Y : int }
type Point = { X : float; Y : float }

let dotError = {X = 1.0; Y = 1.0}       // <-- ambiguity를 가지면, 마지막으로 정의된 record 를 따름.  즉 Point
let dot = ({X = 1; Y = 1} : Dot)


// boxing & unboxing
let boxed = box 1
let unboxed = unbox<int> boxed
// let x = (boxed :?> string);;            // System.InvalidCastException: Unable to cast object of type 'System.Int32' to type 'System.String'.

match boxed with
| :? string -> printfn "The object is a string"
| :? int as d -> printfn "The object is an integer %d" d
| _ -> printfn "The input is something else"

let addresses =
    Map.ofList [    "Jeff", "123 Main Street, Redmond, WA 98052"
                    "Fred", "987 Pine Road, Phila., PA 19116"
                    "Mary", "PO Box 112233, Palo Alto, CA 94301"]


type Point3D = {X : float; Y : float; Z : float}
let p1 = {X = 3.0; Y = 4.0; Z = 5.0}
let p2 = {p1 with Y = 0.0; Z = 0.0}


let round x =
    if x >= 100 then 100
    elif x < 0 then 0
    else x



let round2 x =
    match x with
    | _ when x >= 100 -> 100
    | _ when x < 0 -> 0
    | _ -> x


let rec length l =
    match l with
    | [] -> 0
    | h :: t -> 1 + length t




let mapFirst inp = List.map fst inp