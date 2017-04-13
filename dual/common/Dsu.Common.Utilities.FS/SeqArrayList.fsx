// a sequence of squares
let squares = seq { for x in 1 .. 10 -> x * x }
let squaresList = [ for i in 1..9 -> i*i ]
// a sequence of even numbers
let evens n =
    seq { for x in 1 .. n do
            if x % 2 = 0 then yield x }

let emptyList = []
let oneItem = "one " :: []
let twoItem = "one " :: "two " :: []
let shortHand = ["apples "; "pears"]

let twoLists = ["one, "; "two, "] @ ["buckle "; "my "; "shoe "]




// create some list comprehensions
let numericList = [ 0 .. 9 ]
let alpherSeq = seq { 'A' .. 'Z' }
// create some list comprehensions
let multiplesOfThree = [ 0 .. 3 .. 30 ]
let revNumericSeq = [ 9 .. -1 .. 0 ]
let squareTuples = [ for i in 0..9 -> i, i*i ]

let sliced = squareTuples.[3..6]

