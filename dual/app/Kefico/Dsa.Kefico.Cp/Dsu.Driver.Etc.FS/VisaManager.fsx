#I "../../../../open-sources/bin"
#r "NationalInstruments.VisaNS.dll"
#load "DeviceDriverInterfaces.fs"
#load "VisaManager.fs"

open NationalInstruments.VisaNS
open Dsu.Driver

let rs232c = new Rs232cManager("ASRL5::INSTR")
//rs232c.BaudRate <- 19200
printfn "RESPONSE:%s" (rs232c.Query("*r\r\n"))
//rs232c.Clear()

//let manager = new SorensenRs232cManager("ASRL3::INSTR")
let manager = new SorensenRs232cManager(3)
printfn "ID = [%s]" manager.Id
manager.SetVoltage(2.5)

manager.GetVoltage(1).Value |> printfn "[%A]"


manager.SetCurrent(1, 1.1)
manager.GetCurrent(1).Value |> printfn "Current is [%A]"


manager.GetVoltage("v1?\n").Value |> printfn "[%A]"

manager.Query("*IDN?\n") |> printfn "[%A]"
// Returns : "SORENSEN, XEL 30-3P, J00466702, 3.02-4.05\n"


let ports = Rs232cManager.AllCOMPorts
// manager.WriteAsync("v1 3.3\n")


manager.Write("v1 3.1\n") |> printfn "Final result is [%A]"
manager.Query("v1?\n") |> printfn "V1 value is [%A]"

manager.Write("v1 3.1\n") |> printfn "Final result is [%A]"
manager.Read() |> printfn "Read V1 value is [%A]"



open System.Text.RegularExpressions
// https://fsharpforfunandprofit.com/posts/convenience-active-patterns/
let (|VoltageAnswerPattern|_|) pattern input =
    let m = Regex.Match(input,pattern) 
    if (m.Success) then Some m.Groups.[1].Value else None  

let testVoltage str =
    match str with
    | VoltageAnswerPattern "V\d+ (.*)" v -> 
        Some(System.Double.Parse(v))
    | _ ->  printfn "The value '%s' is something else" str
            None
testVoltage  "V1 3.100"



let sorensenPowerManager = new SorensenEthernetManager("192.168.0.100", 9221)
sorensenPowerManager.SetVoltage(3.3)
