module PdvManagerModule

open Akka.Actor
open System
open Dsu.Common.Utilities
open Dsu.Common.Utilities.FS
open System.Net

type PdvClientConfig() =
    let ipAddress =                 
        Array.Find (Dns.GetHostAddresses(Dns.GetHostName()), (fun ip -> (ip.ToString()).Contains(".")))
    let ip = ipAddress.ToString()
    member val Ip = ip with get

/// <summary>
/// PDV 관리자.
/// </summary>
type PdvManager(config:PdvClientConfig) =
    let ip = config.Ip
    let actorSystem = CptActor.system
    let mutable serverActor : IActorRef = CptActor.server
    let guardianActor = CptActor.CreateCptGuardianActor(0, "")
    let receivingActor = CptActor.receivingActor


    do
        MwsConfig.loadFromAppConfig()
        MwsConfig.printMwsConfiguration()


    interface IDisposable with
        member this.Dispose () =
            actorSystem.Terminate()
                |> Async.AwaitTask
                |> Async.RunSynchronously

            GC.SuppressFinalize(this)

    member __.Ask(message: IActorMessage, timeOut:System.TimeSpan ) =
        let startTime = System.DateTime.Now
        addPendingMessage message startTime
        guardianActor.Tell(message)

        // waits until answer receiving
        let waitResponse(): IActorMessage =
            let rec waitHelper (waitIntervalMilli:int) : IActorMessage =
                if System.DateTime.Now - startTime < timeOut then
                    let response = popOnFinished message.Id
                    match response with
                        | Some(v) -> v
                        | None ->
                            Async.Sleep(waitIntervalMilli) |> ignore
                            waitHelper (2 * waitIntervalMilli) 
                else
                    let msg = sprintf "Timeout for message %s" (message.ToString())
                    new AmError(ErrorCodeEnums.Timeout, msg) :> IActorMessage

            waitHelper 10

        waitResponse()

    member x.AskRaisable(message: IActorMessage, timeOut:System.TimeSpan ) : IActorMessage =
        match x.Ask(message, timeOut) with
            | :? AmError as m ->
                failwithf "error to get response: %s" (m.ToString())
            | _ as m ->
                m

    member x.Ask(message: IActorMessage) =
        x.Ask(message, TimeSpan.FromSeconds(float MwsConfig.mwsServerTaskCompletionTimeoutSecond))







