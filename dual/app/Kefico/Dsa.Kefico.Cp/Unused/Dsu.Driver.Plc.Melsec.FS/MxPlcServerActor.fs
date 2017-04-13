module Dsu.Driver.Plc.Melsec.MxPlcServer

open System
open Akka.FSharp
open Akka.Actor
open log4net
open Log4NetWrapper
open MxPlcConfig
open MxPlcParameters
open Dsu.Driver.Plc.Melsec.Driver
open Dsu.Common.Utilities.FS

let MwsServerFailedMessageSubject = new System.Reactive.Subjects.Subject<obj>() 

let mutable internal counter = 0



type MxPlcServerActor(parameters, logger:ILog) =
    inherit Actor()
    
    let mutable plcDriver = new PlcDriver(parameters)

    member val Logger = logger
    override x.OnReceive message =
        if message.Equals("kill") then
            printfn "Got kill message"
        else if message :? Terminated then
            printfn "Got Terminated message"


        let sender = x.Sender
        let computation = async {
            use changer = consoleColorChanger ConsoleColor.Yellow
            try
                match message with
                | :? Terminated as m ->
                    logInfo "Got peer terminated message."

                | :? AmRequestServerInfo as m ->
                    let info = plcDriver.GetServerInfo()
                    sender.Tell(new AmWithString(info))

                | :? AmReqeustReadDeviceRandom2 as m ->
                    let code, values = plcDriver.ReadDeviceRandom2Helper(m.Device, m.Length)
                    match code with
                    | 0 -> sender.Tell(new AmReplyValues2(values))
                    | _ -> sender.Tell(new AmError(code))
                | :? AmReqeustReadDeviceRandom as m ->
                    let code, values = plcDriver.ReadDeviceRandomHelper(m.Device, m.Length)
                    match code with
                    | 0 -> sender.Tell(new AmReplyValues(values))
                    | _ -> sender.Tell(new AmError(code))

                | :? AmReqeustWriteDeviceRandom as m ->
                    let code = plcDriver.WriteDeviceRandom(m.Device, m.Values)
                    match code with
                    | 0 -> sender.Tell(new AmSuccess())
                    | _ -> sender.Tell(new AmError(code))

                | :? AmReqeustWriteDeviceRandom2 as m ->
                    let code = plcDriver.WriteDeviceRandom2(m.Device, m.Values)
                    match code with
                    | 0 -> sender.Tell(new AmSuccess())
                    | _ -> sender.Tell(new AmError(code))


                | :? AmReqeustGetDevice as m ->
                    let code, value = plcDriver.GetDeviceHelper(m.Device)
                    match code with
                    | 0 -> sender.Tell(new AmReplyValue(value))
                    | _ -> sender.Tell(new AmError(code))

                | :? AmReqeustGetDevice2 as m ->
                    let code, value = plcDriver.GetDevice2Helper(m.Device)
                    match code with
                    | 0 -> sender.Tell(new AmReplyValue2(value))
                    | _ -> sender.Tell(new AmError(code))

                | :? AmReqeustSetDevice as m ->
                    let code = plcDriver.SetDevice(m.Device, m.Value)
                    match code with
                    | 0 -> sender.Tell(new AmSuccess())
                    | _ -> sender.Tell(new AmError(code))

                | :? AmReqeustSetDevice2 as m ->
                    let code = plcDriver.SetDevice2(m.Device, m.Value)
                    match code with
                    | 0 -> sender.Tell(new AmSuccess())
                    | _ -> sender.Tell(new AmError(code))

//                | :? AmPing as m ->
//                    logInfo "Got ping message from %s(%s): %s..." m.Ip m.MacAddress m.Message
//                    new AmPong(m.Id, "Pong") |> sender.Tell 
//                    logInfo "Done!"

                | :? string as m ->
                    logError "Got unexpected string message %s" m
                    "[" + m + "]" |> sender.Tell 
                    logInfo "Done!"

                | _ -> 
                    logError "Got unknown message type!!!"
                    failwith "unknown message"
            with exn ->
                exn.ToString() |> logError "Exception occurred on mxPlcServer:\n%s" 
                MwsServerFailedMessageSubject.OnNext message

                try
                    match message with
//                    | :? ActorMessageBase as m ->
//                        new AmError(ErrorCodeEnums.UnknownError, exn.Message, Id=m.Id) |> sender.Tell 
                    | _ as m ->
                        logError "Reporting error on Unknown request."
                with exn2 ->
                    logError "Failed to generate exception detail info." 
                //raise exn
        }
        computation |> Async.StartAsTask |> ignore



let internal maxPayloadBytes = MxPlcConfig.mxPlcActorMaxPayloadBytes.ToString()
// the most basic configuration of remote actor system
// http://stackoverflow.com/questions/36685326/max-allowed-size-128000-bytes-actual-size-of-encoded-class-scala-error-in-akk
// http://kataribe.naist.jp/akkadotnet/akka.net/blame/f4bef5a991dcb27d8835c254370b34692803d017/src/Akka.Remote/Configuration/Remote.conf
let internal config = """
        akka {
            suppress-json-serializer-warning = on  
            actor {
                provider = "Akka.Remote.RemoteActorRefProvider, Akka.Remote"
                debug {
                    receive = on
                    autoreceive = on
                    lifecycle = on
                    event-stream = on
                    unhandled = on
                }
            }    
            remote {
                # enabled-transports = ["akka.remote.helios.tcp", "akka.remote.helios.udp"]  # http://getakka.net/docs/remoting/transports
                enabled-transports = ["akka.remote.helios.tcp"]
                maximum-payload-bytes = """ + maxPayloadBytes + """ bytes
                # http://stackoverflow.com/questions/17360303/akka-remote-system-shutdown-leads-to-endpointdisassociatedexception
                # http://getakka.net/docs/concepts/configuration
                # log-remote-lifecycle-events = off
                log-remote-lifecycle-events = INFO
                helios.tcp {
                    # tcp-reuse-addr = off
                    transport-protocol = tcp
                    port = """ + MxPlcConfig.mxPlcPort.ToString() + """
                    hostname = """ + MxPlcConfig.mxPlcServer + """
                    public-hostname = """ + MxPlcConfig.mxPlcServer + """
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



let CreateMxPlcServerActor(parameters:PlcParametersBase, logger: ILog) =
    Log4NetWrapper.SetLogger(logger)
    printMxPlcConfiguration()

    // remote system only listens for incoming connections
    // it will receive actor creation request from local-system (see: FSharp.Deploy.Local)
    logInfo "Creating actor system.."
    let system = System.create MxPlcConfig.mxPlcActorSystemName (Configuration.parse config)
    logInfo "Actor system created."

    let args : obj array = [|parameters; logger|]
    let mxPlcServerActor = system.ActorOf(Props(typedefof<MxPlcServerActor>, args), MxPlcConfig.mxPlcActorName)
    system, mxPlcServerActor




