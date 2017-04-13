(*
 * Actor message 는 serialize 되기 위해서 property 는 get(), set() 을 지원해야 한다.
 *)

[<AutoOpen>]
module Dsu.Common.Utilities.FS.ActorMessagesBase

open System
open Dsu.Common.Utilities.Core



let private macAddress = HardwareId.GetMacAddress()
let private ip = getIpAddress()

type IActorMessage =
    abstract Id: int with get, set
    abstract Message: string with get, set
    abstract MacAddress: string with get
    abstract Ip: string with get

type IActorMessageRequest =
    inherit IActorMessage

type IActorMessageReply =
    inherit IActorMessage

    
type ActorMessageBase(msg:string) =
    static let mutable counter = 0
    let mutable messageId = 0
    do
        messageId <- System.Threading.Interlocked.Increment(&counter)

    interface IActorMessage with
        member val Message = msg with get, set
        member val Id = messageId with get, set
        member val MacAddress = macAddress with get
        member val Ip = ip with get

    member __.Message with get() = (__ :> IActorMessage).Message and set(v) = (__ :> IActorMessage).Message <- v
    member __.Id with get() = (__ :> IActorMessage).Id and set(v) = (__ :> IActorMessage).Id <- v
    member __.MacAddress with get() = (__ :> IActorMessage).MacAddress
    member __.Ip with get() = (__ :> IActorMessage).Ip

    new () =
        ActorMessageBase("")

    override x.ToString() = sprintf "Id=%d, Message=%s" x.Id x.Message

type AmRequestCrash(reason) =
    inherit ActorMessageBase(reason)

type AmPing() =
    inherit ActorMessageBase()

type AmPong(id, msg) as g =
    inherit ActorMessageBase()
    do
        g.Id <- id
        g.Message <- msg


type AmWithString(msg) =
    inherit ActorMessageBase()
    let mutable msg = msg
    member __.String with get() = msg and set(v) = msg <- v

/// <summary>
/// Request 보내고 응답을 받기 전까지 보낸 message 들을 관리하기 위한 map
/// id:int -> request message * time stamp * response message 의 map
/// response message 는 최초 null 로 할당되고, response 도착 시 해당 response message 로 채워진다.
///     popOnFinished 호출을 통해서 pop 된다.
/// </summary>
let mutable internal pendingMessageQueue : Map<int, IActorMessage * DateTime * IActorMessage option> = 
    Map.empty

/// <summary>
/// Request 보내는 시점에 호출되어 해당 message 를 관리한다.
/// </summary>
let addPendingMessage (msg:IActorMessage) (stamp:DateTime) =
    let tuple = msg, stamp, None
    let id = msg.Id
    pendingMessageQueue <- pendingMessageQueue.Add(id, tuple)


/// <summary>
/// Reply 보내는 시점에 호출되어 해당 message 가 reply 되었음을 표시한다.
/// </summary>
let markRepliedMessage (replyMessage:IActorMessage) =
    let id = replyMessage.Id
    let tuple = pendingMessageQueue.TryFind(id)
    match tuple with
        | Some(e1, e2, _) ->
            let newTuple = e1, e2, Some(replyMessage)
            pendingMessageQueue <- pendingMessageQueue.Add(id, newTuple)
        | None ->
            sprintf "Failed to find message with id %d" id |> failwithlog 

/// <summary>
/// CPT Guardian actor 에서 최종 결과물을 확인하고, reply message 가 존재하면 이를 pop 해서 반환한다.
/// </summary>
/// <param name="id">최초 요청 message id</param>
let popOnFinished (id:int) =
    match pendingMessageQueue.TryFind(id) with
        | Some(v) ->
            let response = Functions.thd v
            if response.IsSome then response else None
        | _ ->
            None


