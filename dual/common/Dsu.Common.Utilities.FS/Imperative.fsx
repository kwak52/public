open System

let repeatPrint n =
    for i = 1 to n do
        printfn "%d-th loop" i
    printfn "Done!"

repeatPrint 5

let loopUntilSaturday() =
    let mutable now = DateTime.Now
    while ( now.DayOfWeek <> DayOfWeek.Saturday) do
        printfn "Still working!"
        now <- now + TimeSpan.FromDays(1.0)
    printfn "Saturday at last!"

do
    loopUntilSaturday()




for (b, pj) in [("Banana 1", false); ("Banana 2", true)] do
    if pj then
        printfn "%s is in pyjamas today!" b


