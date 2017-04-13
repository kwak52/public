
#load "LayeredMonad.fs"
open LayeredMonad


/// Lazily constructed random generator
let lazyRandom = lazy System.Random()

/// Generate a lazy list with random numbers
let nums = 
  llist { for x in 1 .. 5 do 
            let! rnd = lazyRandom
            yield rnd.Next(10) }

/// Generate a lazy list by using 'nunms' twice
let twiceNums = 
  llist { yield! nums
          for n in nums do
            yield n * 10 }

// Run this line to see the result
twiceNums |> toList





/// Law that specifies lifting w.r.t. unit
let liftUnit v = 
  let m1 = ll { let! x = l { return v } in yield x } 
  let m2 = ll { yield v }
  m1 |> shouldEqual m2

/// Law that specifies lifting w.r.t. bind
let liftBind m g f = 
  let m1 = ll { let! y = l { let! x = m in return! f x } 
                yield! g y }    
  let m2 = ll { let! x = m in let! y = l { return! f x }
                yield! g y }
  m1 |> shouldEqual m2