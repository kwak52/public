// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r @"..\bin\PcapDotNet.Core.dll"

#load "Devices.fs"

open Dsu.PacketAnalysis
open PcapDotNet.Core

// Define your library scripting code here

// Network card device 를 listing
let devices = Devices.NetworkDevices |> List.ofSeq
let deviceNames = devices |> List.map (fun d -> (d.Name, d.Description))

let deviceDetails = devices |> List.map (fun d -> Devices.DeviceInfo d)


let a = new MonitoringTarget("192.168.0.100")
printfn "%A" a.Ip