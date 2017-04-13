[<AutoOpen>]
module Socket

open System
open System.Net
open System.Net.Sockets
open System.Reactive.Subjects
open System.Configuration
open AppConfig
open Polly


type SocketResponse =
    | Response of string
    | Empty
    | Exception of System.Exception
    member x.GetException() =
        match x with
        | Exception(exn) -> exn
        | _ -> failwith "Not an exception."
    member x.Value
        with get() =
            match x with
            | Response(v) -> v
            | _ -> failwith "Not a some value."


let mutable pauseBetweenSendAndReceiveMilli = 20
let mutable tcpSocketConnectionTimeoutMilli = 1000

let loadFromAppConfig() =
    match readIntKey "pauseBetweenSendAndReceiveMilli" with
    | Some(v) -> pauseBetweenSendAndReceiveMilli <- v
    | _ -> ()

    match readIntKey "tcpSocketConnectionTimeoutMilli" with
    | Some(v) -> tcpSocketConnectionTimeoutMilli <- v
    | _ -> ()

    logInfo "-- TCP/IP Socket Configuration --"
    logInfo "\tpauseBetweenSendAndReceiveMilli = %d" pauseBetweenSendAndReceiveMilli
    logInfo "\tcpSocketConnectionTimeoutMilli = %d" tcpSocketConnectionTimeoutMilli



let readFromSocketAsync (socket:TcpClient) =
    async {
        let stream = socket.GetStream()
        while not stream.DataAvailable do
            Async.Sleep(pauseBetweenSendAndReceiveMilli) |> ignore

        let mutable b = stream.ReadByte()
        let bytes =
            seq {
                while b <> -1 && (char)b <> '\n' do
                    yield (byte)b;
                    b <- stream.ReadByte()
            } |> Array.ofSeq
        return Response(Text.Encoding.ASCII.GetString(bytes))
    }

let readFromSocket (socket:TcpClient) =
    readFromSocketAsync socket |> Async.RunSynchronously


let writeToSocketAsync (socket:TcpClient) (command:string) =
    async {
        try
            let stream = socket.GetStream()
            let bytes = Text.Encoding.ASCII.GetBytes(command)
            stream.Write(bytes, 0, bytes.Length)
            stream.Flush()
        with exn ->
            failwithlog (sprintf "Failed to write socket. with command %s" command)
    }

let writeToSocket (socket:TcpClient) (command:string) =
    writeToSocketAsync socket command |> Async.RunSynchronously


let writeToSocketAndGetResponse (socket:TcpClient) (command:string) =
    async {
        do! writeToSocketAsync socket command
        return! readFromSocketAsync socket
    } |> Async.RunSynchronously


let joinStrings (strings: string seq) crlf =
    String.Join(crlf, strings)

type TcpClientManager(ip:string, port:int, policy:Policy) =
    let mutable tcpSocket: System.Net.Sockets.TcpClient = null
    do
        try
            policy.Execute(fun () ->
                tcpSocket <- new System.Net.Sockets.TcpClient()
                if not (tcpSocket.ConnectAsync(ip, port).Wait(tcpSocketConnectionTimeoutMilli)) then
                    // soft fail : Retry 수행
                    failwithlog (sprintf "Connection timed out (%d ms): ip=%s, port=%d." tcpSocketConnectionTimeoutMilli ip port))
        with exn ->
            // hard fail : 최종 retry 에서도 fail 한 경우
            failwithlog (sprintf "Failed to connect: ip=%s, port=%d with excetion:%O." ip port  exn)

    member val Ip = ip with get
    member val Port = port with get

    member val Socket = tcpSocket with get
    member val Stream = tcpSocket.GetStream() with get
    member __.PostCommand(command) = writeToSocketAsync tcpSocket command
    
    abstract SendCommand: string -> unit
    default __.SendCommand(command) = writeToSocket tcpSocket command

    abstract SendCommandGetResponse: string -> SocketResponse
    default __.SendCommandGetResponse(command) = writeToSocketAndGetResponse tcpSocket command

    member x.SendCommandWithTimeout(command, timeoutMilli) =
        let f() = x.SendCommand(command)
        withTimeLimit f timeoutMilli (sprintf "Socket: %s" command)
    member x.SendCommandGetResponseWithTimeout(command, timeoutMilli) =
        let f() = x.SendCommandGetResponse(command)
        withTimeLimit f timeoutMilli (sprintf "Socket: %s" command)
    member __.Read() = readFromSocket tcpSocket
    abstract Close : unit -> unit
    default x.Close() = tcpSocket.Close()
    interface IDisposable with
        member x.Dispose() = x.Close()

    new (ip, port) = 
        let defaultPolicy = 
            let sleepDurations = {1..5} |> Seq.map (float >> TimeSpan.FromSeconds)
            let onRetry exn span counter context =
                logWarn "Exception on %d-th trial: span=%A, exn=%O" counter span exn
            Policy.Handle<Exception>().WaitAndRetry(sleepDurations, onRetry)
        new TcpClientManager(ip, port, defaultPolicy)


type TcpTelnetClientManager(serverIp:string, serverPort:int) as this = 
    inherit TcpClientManager(serverIp, serverPort)
    let subject = new Subject<string>()
    let mutable goOn = true

    member val Subject = subject with get

    new (serverIp) = new TcpTelnetClientManager(serverIp, 23)

    member __.StartMonitor() =
        async {
            while goOn do
                let s = this.Socket
                let available = this.Socket.Available
                if available > 0 then
                    let! bytes = this.Stream.AsyncRead available
                    let readString = 
                        System.Text.Encoding.ASCII.GetString(bytes)
                    subject.OnNext readString

                do! Async.Sleep 50
        } |> Async.Start


    interface IDisposable with
        member __.Dispose() = 
            goOn <- false
            base.Close()

