module Dsu.Driver.Plc.Melsec.MxPlcManagerModule

open Akka.FSharp
open Akka.Actor
open Log4NetWrapper
open Dsu.Common.Utilities.FS
open Dsu.Driver.Plc.Melsec


let internal maxPayloadBytes = MxPlcConfig.mxPlcActorMaxPayloadBytes.ToString()

/// <summary>
/// HOCON configuration
/// </summary>
let internal config = """
akka {  
    suppress-json-serializer-warning = on  
    actor {
        provider = "Akka.Remote.RemoteActorRefProvider, Akka.Remote"
    }    
    remote {
        maximum-payload-bytes = """ + maxPayloadBytes + """ bytes
        helios.tcp {
            # tcp-reuse-addr = off
            transport-protocol = tcp
            port = 0                    # get first available port
            hostname = 0.0.0.0          
            send-buffer-size = """ + maxPayloadBytes + """b
            receive-buffer-size = """ + maxPayloadBytes + """b
            message-frame-size = """ + maxPayloadBytes + """b
            maximum-frame-size = """ + maxPayloadBytes + """b
            # http://stackoverflow.com/questions/31753052/akka-net-remote-disconnection
            tcp-keepalive = on
        }
    }
}
"""


/// <summary>
/// MxPlcClient Actor system
/// </summary>
let system = System.create "mxplc-client-actor-system" (Configuration.parse config)

/// <summary>
/// MxPlc server actor 를 가져오기 위한 함수
/// </summary>
let getServerActor() : IActorRef = 
    let actorPath = sprintf "akka.tcp://%s@%s:%d/user/%s" MxPlcConfig.mxPlcActorSystemName MxPlcConfig.mxPlcServer MxPlcConfig.mxPlcPort MxPlcConfig.mxPlcActorName
    let actorSelection = system.ActorSelection actorPath

    // convert ActorSelection into IActorRef.
    // http://stackoverflow.com/questions/16105536/how-to-ask-an-actorselection
    let taskf() = actorSelection.ResolveOne(System.TimeSpan.FromSeconds(float MxPlcConfig.mxPlcServerPingTimeoutSecond))      // Task<IActorRef>
    Functions.waitDotNetTask(taskf)


type MxPlcManager() =
    let server = getServerActor()

    member __.GetServerInfo() = 
        let raw = server.Ask(new AmRequestServerInfo()).Result
        match raw with
        | :? AmWithString as m -> m.String
        | _ -> failwithf "Failed to get Server info: Reply type=%A" raw

    member __.ReadDeviceRandom(device, dwSize) : int array = 
        let raw = server.Ask(new AmReqeustReadDeviceRandom(device, dwSize)).Result
        match raw with
        | :? AmReplyValues as m -> m.Values
        | :? AmError as m -> failwithf "Failed to read (ReadDeviceRandom) for device:%s: result code=%d" device m.Code
        | _ -> failwithf "Failed to read (ReadDeviceRandom) for device:%s: Reply type=%A" device raw


    member __.ReadDeviceRandom2(devices, dwSize) : int16 array = 
        let raw = server.Ask(new AmReqeustReadDeviceRandom2(devices, dwSize)).Result
        match raw with
        | :? AmReplyValues2 as m -> m.Values
        | :? AmError as m -> failwithf "Failed to read (ReadDeviceRandom) for devices:%s: result code=%d" devices m.Code
        | _ -> failwithf "Failed to read (ReadDeviceRandom) for devices:%s: Reply type=%A" devices raw

    member __.WriteDeviceRandom(devices, values:int array) : int = 
        let raw = server.Ask(new AmReqeustWriteDeviceRandom(devices, values)).Result
        match raw with
        | :? AmSuccess as m -> m.Code
        | :? AmError as m -> failwithf "Failed to read (ReadDeviceRandom) for devices:%s: result code=%d" devices m.Code
        | _ -> failwithf "Failed to read (ReadDeviceRandom) for devices:%s: Reply type=%A" devices raw

    member __.WriteDeviceRandom2(devices, values:int16 array) : int = 
        let raw = server.Ask(new AmReqeustWriteDeviceRandom2(devices, values)).Result
        match raw with
        | :? AmSuccess as m -> m.Code
        | :? AmError as m -> failwithf "Failed to read (ReadDeviceRandom2) for devices:%s: result code=%d" devices m.Code
        | _ -> failwithf "Failed to read (ReadDeviceRandom2) for devices:%s: Reply type=%A" devices raw


    member x.GetDevice(device) : int =
        let raw = server.Ask(new AmReqeustGetDevice(device)).Result
        match raw with
        | :? AmReplyValue as m -> m.Value
        | :? AmError as m -> failwithf "Failed to read (GetDevice) for devices:%s: result code=%d" device m.Code
        | _ -> failwithf "Failed to read (GetDevice) for devices:%s: Reply type=%A" device raw

    member x.GetDevice2(device) : int16 =
        let raw = server.Ask(new AmReqeustGetDevice2(device)).Result
        match raw with
        | :? AmReplyValue2 as m -> m.Value
        | :? AmError as m -> failwithf "Failed to read (GetDevice2) for devices:%s: result code=%d" device m.Code
        | _ -> failwithf "Failed to read (GetDevice2) for devices:%s: Reply type=%A" device raw

    member __.SetDevice(device, value) =
        let raw = server.Ask(new AmReqeustSetDevice(device, value)).Result
        match raw with
        | :? AmSuccess as m -> m.Code
        | :? AmError as m -> failwithf "Failed to write (SetDevice) for devices:%s: result code=%d" device m.Code
        | _ -> failwithf "Failed to write (SetDevice) for devices:%s: Reply type=%A" device raw

    member __.SetDevice2(device, value) =
        let raw = server.Ask(new AmReqeustSetDevice2(device, value)).Result
        match raw with
        | :? AmSuccess as m -> m.Code
        | :? AmError as m -> failwithf "Failed to write (SetDevice) for devices:%s: result code=%d" device m.Code
        | _ -> failwithf "Failed to write (SetDevice) for devices:%s: Reply type=%A" device raw
