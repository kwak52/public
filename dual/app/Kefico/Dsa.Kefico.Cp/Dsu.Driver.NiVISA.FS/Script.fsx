//F#에 대해 http://fsharp.org에서 자세히 알아봅니다. F# 프로그래밍에 대한 자세한 지침은 
// 'F# 자습서' 프로젝트를 참조하세요.

#I "../../../../open-sources/bin"
#r "NationalInstruments.VisaNS.dll"
#load "VisaManager.fs"

open NationalInstruments.VisaNS

printfn "%A" (findResources())

let manager = new Rs232cManager("ASRL3::INSTR")
printfn "ID = [%s]" manager.Id

manager.Query("*IDN?\n") |> printfn "[%A]"
// Returns : "SORENSEN, XEL 30-3P, J00466702, 3.02-4.05\n"


let ports = Rs232cManager.AllPorts
// manager.WriteAsync("v1 3.3\n")


manager.Write("v1 3.1\n") |> printfn "Final result is [%A]"
manager.Query("v1?\n") |> printfn "V1 value is [%A]"

manager.Write("v1 3.1\n") |> printfn "Final result is [%A]"
manager.Read() |> printfn "Read V1 value is [%A]"
