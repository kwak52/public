
// definition of Lucas numbers using pattern matching
let rec luc x =
    match x with
    | x when x <= 0 -> failwith "value must be greater than 0"
    | 1 -> 1
    | 2 -> 3
    | x -> luc (x - 1) + luc (x - 2)


// function for converting a Boolean to a string
let booleanToString x =
    match x with false -> "False" | _ -> "True"
// function for converting a string to a Boolean
let stringToBoolean x =
    match x with
    | "True" | "true" -> true
    | "False" | "false" -> false
    | _ -> failwith "unexpected input"




// list to be concatenated
let listOfList = [[2; 3; 5]; [7; 11; 13]; [17; 19; 23; 29]]
// definition of a concatenation function
let rec concatList l =
    match l with
    | head :: tail -> head @ (concatList tail)
    | [] -> []
// call the function
let primes = concatList listOfList

// print the results
printfn "%A" primes





// function that attempts to find various sequences
let rec findSequence l =
    match l with
    // match a list containing exactly 3 numbers
    | [x; y; z] ->
        printfn "Last 3 numbers in the list were %i %i %i" x y z

    // match a list of 1, 2, 3 in a row
    | 1 :: 2 :: 3 :: tail ->
        printfn "Found sequence 1, 2, 3 within the list"
        findSequence tail
    // if neither case matches and items remain
    // recursively call the function
    | head :: tail -> findSequence tail
    // if no items remain terminate
    | [] -> ()
// some test data
let testSequence = [1; 2; 3; 4; 5; 6; 7; 8; 9; 8; 7; 6; 5; 4; 3; 2; 1]
// call the function
findSequence testSequence








// type representing a couple
type Couple = { him : string ; her : string }
// list of couples
let couples =
    [ { him = "Brad" ; her = "Angelina" };
        { him = "Becks" ; her = "Posh" };
        { him = "Chris" ; her = "Gwyneth" };
        { him = "Michael" ; her = "Catherine" } ]
// function to find "David" from a list of couples
let rec findDavid l =
    match l with
    | { him = x ; her = "Posh" } :: tail -> x
    | _ :: tail -> findDavid tail
    | [] -> failwith "Couldn't find David"
// print the results
printfn "%A" (findDavid couples)


let rec findPartner soughtHer l =
    match l with
    | { him = x ; her = her } :: tail when her = soughtHer -> x
    | _ :: tail -> findPartner soughtHer tail
    | [] -> failwith "Couldn't find him"

findPartner "Catherine" couples |> printfn "%A"






// list of objects
let anotherList = [ box "one"; box 2; box 3.0 ]
// pattern match and print value
let recognizeAndPrintType (item : obj) =
    match item with
    | :? System.Int32 as x -> printfn "An integer: %i" x
    | :? System.Double as x -> printfn "A double: %f" x
    | :? System.String as x -> printfn "A string: %s" x
    | x -> printfn "An object: %A" x
// interate over the list pattern matching each item
List.iter recognizeAndPrintType anotherList



try
    // look at current time and raise an exception
    // based on whether the second is a multiple of 3
    if System.DateTime.Now.Second % 3 = 0 then
        raise (new System.Exception("Normal Exception"))
    else
        raise (new System.ApplicationException("Application Exception"))
with
    | :? System.ApplicationException as ex ->
        // this will handle "ApplicationException" case
        printfn "A second that was not a multiple of 3\nException=%O" ex
    | ex ->
        // this will handle all other exceptions
        printfn "A second that was a multiple of 3\nException=%O" ex



