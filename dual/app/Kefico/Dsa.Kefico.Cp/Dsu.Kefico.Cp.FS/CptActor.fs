module CptActor

open Akka.FSharp
open Akka.Actor
open Log4NetWrapper
open Dsu.Common.Utilities.FS

let internal maxPayloadBytes = MwsConfig.mwsActorMaxPayloadBytes.ToString()

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
/// CPT Actor system
/// </summary>
let system = System.create "cpt-actor-system" (Configuration.parse config)


let mutable receivingActor : IActorRef = null

/// <summary>
/// CPT (guardian) actor 가 response 를 받았음을 알려주는 event subject
/// </summary>
let CptActorSubject = new System.Reactive.Subjects.Subject<obj>() 


/// <summary>
/// Server actor 에게 request 전송 후, 완료까지 기다릴 시간.  time out 경과 후에는 exception raise
/// </summary>
let internal getTimeOut() = System.TimeSpan.FromSeconds(float MwsConfig.mwsServerTaskCompletionTimeoutSecond)


/// <summary>
/// CPT child actor.  CP tester 관련된 실제 (dangerous 한) 작업을 수행할 actor.
/// http://www.fssnip.net/pV
/// https://github.com/petabridge/akkadotnet-code-samples/blob/master/RemoteDeploy/RemoteDeploy.Deployer/Program.cs
/// </summary>
type internal CptChildActor(serverActor: IActorRef, receivingActor: IActorRef) =
    inherit Actor()

    override x.OnReceive message =
        let sender = x.Sender
        try
            match message with
            | :? IActorMessage as m ->
                serverActor.Tell(m, receivingActor)

            | _ as m ->
                printfn "Client got unknown message %A" m
        with exn ->
            let msg = exn.ToString() |> sprintf "Exception occurred: %s"
            printfn "%s" msg
            failwith msg



/// <summary>
/// MWS server actor 를 가져오기 위한 함수
/// </summary>
let getServerActor() : IActorRef = 
    let actorPath = sprintf "akka.tcp://%s@%s:%d/user/%s" MwsConfig.mwsActorSystemName MwsConfig.mwsServer MwsConfig.mwsPort MwsConfig.mwsActorName
    let actorSelection = system.ActorSelection actorPath

    // convert ActorSelection into IActorRef.
    // http://stackoverflow.com/questions/16105536/how-to-ask-an-actorselection
    let taskf() = actorSelection.ResolveOne(System.TimeSpan.FromSeconds(float MwsConfig.mwsServerPingTimeoutSecond))      // Task<IActorRef>
    Functions.waitDotNetTask(taskf)


/// <summary>
/// MWS service actor.  CPT Actor 의 요청을 받아서 (remote) MWS server 상에서 구동되는 actor
/// </summary>
let mutable internal server : IActorRef = getServerActor()


/// <summary>
/// 주어진 task 를 수행하기 위해서 1회용 child actor instance 를 생성.
/// - context 는 guadian actor 의 context 이다.
/// - guadian actor 의 child actor 로 생성된다.
/// </summary>
type internal CptChildActorAllocator(system:ActorSystem, receivingActor: IActorRef, context:IUntypedActorContext) =
    let args : obj array = [| server; receivingActor |]
    static let mutable counter = 0
    let child = 
        let childName = 
            let number = System.Threading.Interlocked.Increment(&counter)
            sprintf "cpt-child-actor-%d" number
        context.ActorOf(Props(typedefof<CptChildActor>, args), childName)
    do 
        context.Watch(child) |> ignore

    // dispose 시에 actor 정리
    interface System.IDisposable with
        member this.Dispose() = 
            async {
                (* Unwatch 수행 이전에 약간의 delay 가 !!!! 반드시 !!!! 필요하다. *)
                printfnd "%s disposed" child.Path.Name
                do! Async.Sleep(200)
                context.Unwatch(child) |> ignore
                system.Stop(child)
            } |> Async.Start
    member x.Actor = child


type internal CptReceivingActor() =
    inherit Actor()
    override x.OnReceive message =
        match message with
            | :? IActorMessage as m ->
                if m :? AmReplySteps then
                    printfn "Client(Receiving actor) got step reply with %d steps" (m :?> AmReplySteps).Steps.Length
                markRepliedMessage m
                //m.Table.ConvertToString() |> printfn "%s" 
                CptActorSubject.OnNext(m)
            | :? string as m ->
                printfn "Client(Receiving actor) got message %s" m
            | _ as m ->
                printfn "Client(Receiving actor) got unknown message %A" m
                



/// <summary>
/// CPT Guardian actor.  CP tester 작업 요청에 따라서 child actor 를 fork 해서 작업을 수행한다.
/// Child actor 의 exception 에 대해서 안전성 보장.
/// </summary>
type internal CptGuardianActor (hostId: int, sec: string) =
    inherit Actor()
    do
        MwsConfig.loadFromAppConfig()
        receivingActor <- Actor.Context.ActorOf(Props(typedefof<CptReceivingActor>), "cpt-receiving-actor")
        Actor.Context.Watch(server) |> ignore

    member __.ReceivingActor = receivingActor

    override x.OnReceive message =
        let this = x
        let sender = x.Sender
        let context = Actor.Context

        let forkAndRequest m =
            use resource = new CptChildActorAllocator(system, receivingActor, context)
            let child = resource.Actor
            child.Tell(m, receivingActor)

        match message with
            | :? Terminated as m ->
                if m.ActorRef = server then
                    logError "Got server terminated message."
                    tryMessageBox("Got server terminated message.")

                    CptActorSubject.OnNext(new AmError(ErrorCodeEnums.MwsServerTerminatedError, "Server terminated"))
                    // became unavailable state
                    System.Windows.Forms.Application.Exit()
                    failwith "Got server terminated message."
                else
                    printfn "GuardianActor got actor %s terminated message." m.ActorRef.Path.Name                    

            | :? IActorMessage as m ->
                forkAndRequest(m)

            | _ as m ->
                printfn "Client got unknown message %A" m



let CreateCptGuardianActor(hostId: int, sec: string) =
    let args : obj array = [| hostId; sec |]
    system.ActorOf(Props(typedefof<CptGuardianActor>, args), "cpt-guardian-actor")        // https://gist.github.com/akimboyko/e58e4bfbba3e9a551f05




