/// MITSUBISH Melsec PLC driver
module Dsu.Driver.Plc.Melsec.Driver

open System
open FSharp.Interop.Dynamic     // http://tomasp.net/blog/dynamic-sql.aspx/
open Dsu.Common.Utilities.FsReflection
open Dsu.Driver.Plc.Melsec.Type
open MxPlcParameters



/// <summary>
/// Melsec PLC driver
/// </summary>
type PlcDriver(parameters:PlcParametersBase) =
    let actType = parameters.ActInterfaceType
    let neworkCardInterface = parameters.NetworkCardInterfaceType
    let act = parameters.ActInterface   // createActObject(actType)

    do
        if Environment.Is64BitProcess then
            failwith "Melsec PlcDriver instance should have been created on x86 platform."

        logInfo "Network interface : %A" neworkCardInterface
        parameters.ApplyToActObject()

        let result = act?Open()
        if ( result <> 0 ) then
            failwithf "Failed to Open() MELSEC PLC.  result code = %d" result

    member internal __.GetServerInfo() =
        parameters.ToString() // + (GetPropertyInfo(act) |> Seq.map(fun (name, value) -> sprintf "%s\t%O\n" name value) |> String.Concat)

    member internal __.ReadDeviceRandomHelper(device, dwSize) =
        let values: int32 array = Array.create dwSize 0

        let code =
            match neworkCardInterface with
            | QJ71E71TCP(ai) -> ai.ReadDeviceRandom(device, dwSize, &values.[0])
            | QJ71E71UDP(ai) -> ai.ReadDeviceRandom(device, dwSize, &values.[0])
            | AJ71E71TCP(ai) -> ai.ReadDeviceRandom(device, dwSize, &values.[0])
            | AJ71E71UDP(ai) -> ai.ReadDeviceRandom(device, dwSize, &values.[0])
            | QNUDECPUTCP(ai) -> ai.ReadDeviceRandom(device, dwSize, &values.[0])
            | QNUDECPUUDP(ai) -> ai.ReadDeviceRandom(device, dwSize, &values.[0])

        code, values

    // FSharp.Interop.Dynamic 구현의 dynamic 으로는 ref/out parameter 호출이 불가능하다.
    // 맞는 type 으로 casting 해서 직접 값을 가져와야 한다.
    member x.ReadDeviceRandom(device, dwSize) : int array = 
        let code, values = x.ReadDeviceRandomHelper(device, dwSize)
        if code <> 0 then failwithf "Failed to read (ReadDeviceRandom) for device:%s" device
        values

    member internal __.ReadDeviceRandom2Helper(device, dwSize) =
        let values: int16 array = Array.create dwSize 0s
        let code =
            match neworkCardInterface with
            | QJ71E71TCP(ai) -> ai.ReadDeviceRandom2(device, dwSize, &values.[0])
            | QJ71E71UDP(ai) -> ai.ReadDeviceRandom2(device, dwSize, &values.[0])
            | AJ71E71TCP(ai) -> ai.ReadDeviceRandom2(device, dwSize, &values.[0])
            | AJ71E71UDP(ai) -> ai.ReadDeviceRandom2(device, dwSize, &values.[0])
            | QNUDECPUTCP(ai) -> ai.ReadDeviceRandom2(device, dwSize, &values.[0])
            | QNUDECPUUDP(ai) -> ai.ReadDeviceRandom2(device, dwSize, &values.[0])
        code, values


    member x.ReadDeviceRandom2(device, dwSize) : int16 array = 
        let code, values = x.ReadDeviceRandom2Helper(device, dwSize)
        if code <> 0 then failwithf "Failed to read (ReadDeviceRandom2) for device:%s" device
        values

    member __.WriteDeviceRandom(device, values:int seq) : int = 
        let code =
            let data = values |> Array.ofSeq
            let dwSize = data.Length
            match neworkCardInterface with
            | QJ71E71TCP(ai) -> ai.WriteDeviceRandom(device, dwSize, &data.[0])
            | QJ71E71UDP(ai) -> ai.WriteDeviceRandom(device, dwSize, &data.[0])
            | AJ71E71TCP(ai) -> ai.WriteDeviceRandom(device, dwSize, &data.[0])
            | AJ71E71UDP(ai) -> ai.WriteDeviceRandom(device, dwSize, &data.[0])
            | QNUDECPUTCP(ai) -> ai.WriteDeviceRandom(device, dwSize, &data.[0])
            | QNUDECPUUDP(ai) -> ai.WriteDeviceRandom(device, dwSize, &data.[0])

        if code <> 0 then failwithf "Failed to write (WriteDeviceRandom) for device:%s" device
        code

    member __.WriteDeviceRandom2(device, values:int16 seq) : int = 
        let code =
            let data = values |> Array.ofSeq
            let dwSize = data.Length
            match neworkCardInterface with
            | QJ71E71TCP(ai) -> ai.WriteDeviceRandom2(device, dwSize, &data.[0])
            | QJ71E71UDP(ai) -> ai.WriteDeviceRandom2(device, dwSize, &data.[0])
            | AJ71E71TCP(ai) -> ai.WriteDeviceRandom2(device, dwSize, &data.[0])
            | AJ71E71UDP(ai) -> ai.WriteDeviceRandom2(device, dwSize, &data.[0])
            | QNUDECPUTCP(ai) -> ai.WriteDeviceRandom2(device, dwSize, &data.[0])
            | QNUDECPUUDP(ai) -> ai.WriteDeviceRandom2(device, dwSize, &data.[0])

        if code <> 0 then failwithf "Failed to write (WriteDeviceRandom2) for device:%s" device
        code

    member __.SetDevice(device, value) =
        match neworkCardInterface with
        | QJ71E71TCP(ai) -> ai.SetDevice(device, value)
        | QJ71E71UDP(ai) -> ai.SetDevice(device, value)
        | AJ71E71TCP(ai) -> ai.SetDevice(device, value)
        | AJ71E71UDP(ai) -> ai.SetDevice(device, value)
        | QNUDECPUTCP(ai) -> ai.SetDevice(device, value)
        | QNUDECPUUDP(ai) -> ai.SetDevice(device, value)

    member __.SetDevice2(device, value) =
        match neworkCardInterface with
        | QJ71E71TCP(ai) -> ai.SetDevice2(device, value)
        | QJ71E71UDP(ai) -> ai.SetDevice2(device, value)
        | AJ71E71TCP(ai) -> ai.SetDevice2(device, value)
        | AJ71E71UDP(ai) -> ai.SetDevice2(device, value)
        | QNUDECPUTCP(ai) -> ai.SetDevice2(device, value)
        | QNUDECPUUDP(ai) -> ai.SetDevice2(device, value)


    member internal __.GetDeviceHelper(device) =
        match neworkCardInterface with
        | QJ71E71TCP(ai) -> ai.GetDevice(device)
        | QJ71E71UDP(ai) -> ai.GetDevice(device)
        | AJ71E71TCP(ai) -> ai.GetDevice(device)
        | AJ71E71UDP(ai) -> ai.GetDevice(device)
        | QNUDECPUTCP(ai) -> ai.GetDevice(device)
        | QNUDECPUUDP(ai) -> ai.GetDevice(device)

    member x.GetDevice(device) =
        let code, value = x.GetDeviceHelper(device)
        if code <> 0 then failwithf "Failed to read (GetDevice) for device: %s" device
        value


    member internal __.GetDevice2Helper(device) =
        match neworkCardInterface with
        | QJ71E71TCP(ai) -> ai.GetDevice2(device)
        | QJ71E71UDP(ai) -> ai.GetDevice2(device)
        | AJ71E71TCP(ai) -> ai.GetDevice2(device)
        | AJ71E71UDP(ai) -> ai.GetDevice2(device)
        | QNUDECPUTCP(ai) -> ai.GetDevice2(device)
        | QNUDECPUUDP(ai) -> ai.GetDevice2(device)

    member x.GetDevice2(device) =
        let code, value = x.GetDevice2Helper(device)
        if code <> 0 then failwithf "Failed to read (GetDevice2) for device: %s" device
        value

    interface IDisposable with
        member __.Dispose() = act?Close()

