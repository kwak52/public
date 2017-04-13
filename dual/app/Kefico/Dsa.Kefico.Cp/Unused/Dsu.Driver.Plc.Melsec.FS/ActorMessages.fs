(*
 * Actor message 는 serialize 되기 위해서 property 는 get(), set() 을 지원해야 한다.
 * Actor message type 들은 AKKA actor system 에 의해서 serialize 가 필요하므로, internal 이나 protected 로 선언되면 안된다.
 *)

[<AutoOpen>]
module Dsu.Driver.Plc.Melsec.ActorMessages

open Dsu.Common.Utilities.FS

type AmRequestServerInfo() =
    inherit ActorMessageBase()

type AmRequestBaseWithDeviceAndLength(device, length) =
    inherit ActorMessageBase()
    member __.Device:string = device
    member __.Length:int = length

type AmReqeustReadDeviceRandom(device, length) =
    inherit AmRequestBaseWithDeviceAndLength(device, length)

type AmReqeustReadDeviceRandom2(device, length) =
    inherit AmRequestBaseWithDeviceAndLength(device, length)


type AmReqeustGetDevice(device) =
    inherit AmRequestBaseWithDeviceAndLength(device, 1)

type AmReqeustGetDevice2(device) =
    inherit AmRequestBaseWithDeviceAndLength(device, 1)

type AmReqeustSetDevice(device, value:int) =
    inherit AmRequestBaseWithDeviceAndLength(device, 1)
    member __.Value = value

type AmReqeustSetDevice2(device, value:int16) =
    inherit AmRequestBaseWithDeviceAndLength(device, 1)
    member __.Value = value


type AmReqeustWriteDeviceRandom(device, values:int array) =
    inherit AmRequestBaseWithDeviceAndLength(device, values.Length)
    member __.Values = values

type AmReqeustWriteDeviceRandom2(device, values:int16 array) =
    inherit AmRequestBaseWithDeviceAndLength(device, values.Length)
    member __.Values = values
    //new (devices, value) = new AmReqeustWriteDeviceRandom2(devices, [|value|])


type AmReplyBase(code) =
    member __.Code = code
    new () = new AmReplyBase(0)

type AmReplyValue(value:int) =
    inherit AmReplyBase()
    member __.Value = value

type AmReplyValue2(value:int16) =
    inherit AmReplyBase()
    member __.Value = value

type AmReplyValues(values:int array) =
    inherit AmReplyBase()
    member __.Values = values

type AmReplyValues2(values:int16 array) =
    inherit AmReplyBase()
    member __.Values = values

type AmError(code) =
    inherit AmReplyBase(code)

type AmSuccess() =
    inherit AmReplyBase(0)
