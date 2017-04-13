[<AutoOpen>]
module Dsu.Driver.NiDaqBase

open System.Text.RegularExpressions
open NationalInstruments.DAQmx

/// <summary>
/// NI DAQ measure result type.
/// </summary>
type Measure =
    | Measure of double
    | OutOfRange
    | Exception of System.Exception
    member x.GetException() =
        match x with
        | Exception(exn) -> exn
        | _ -> failwithlog "Not an exception."
    member x.Value
        with get() =
            match x with
            | Measure(v) -> v
            | _ -> failwithlog "Not a some value."

/// Active pattern for DAQ channel matching
let (|ChannelPattern|_|) input =
    let m = Regex.Match(input, "([^/].*)/(.*)") 
    if (m.Success) then Some (m.Groups.[1].Value, m.Groups.[2].Value) else None  


/// returns map of {string(=device name) * Device}
let Devices = 
    DaqSystem.Local.Devices
        |> Seq.map(fun d -> d, DaqSystem.Local.LoadDevice(d))
        |> Map.ofSeq


let Check() =
    logInfo "-- Scanning local DAQ Devices... Found %d devices" Devices.Count

    Devices
        |> Map.iter (fun name d -> 
            logInfo "\t%s/%s, Serial=%d, Simulated=%b" name d.ProductType d.SerialNumber d.IsSimulated)

    Devices.Count > 0


type DaqMessageType =
    //| SingleChannelReadResult of float array
    /// 다채널 읽기 event
    | MultiChannelReadResult of NationalInstruments.AnalogWaveform<float> array * float array
    /// DAQ exception event
    | Error of System.Exception

/// for task start/stop status 
type DaqTask(taskName) =
    let daqTask = new NationalInstruments.DAQmx.Task(taskName)
    let mutable started = false
    member __.Task with get() = daqTask
    member __.Stop() =
        started <- false
        daqTask.Stop()
