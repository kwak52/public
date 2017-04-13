// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.


open System
open Akka.FSharp
open Akka.Actor
open MwsConfig
open MySql.Data.MySqlClient
            


[<EntryPoint>]
let main argv = 
    Console.Title <- "GaudiFileManager(Remote): " + Diagnostics.Process.GetCurrentProcess().Id.ToString()
    Console.SetBufferSize(Console.BufferWidth, 3000);

    cprintfn ConsoleColor.Green  """
    ======================================
                MWS server
    ======================================
    """

    MwsConfig.loadFromAppConfig()


    let mutable restartSystem = false
    MwsServer.MwsServerFailedMessageSubject
        .Subscribe(fun msg -> 
            if msg :? IActorMessage then
                restartSystem <- true)
        |> ignore

            

    let serverLoop() =
        let (system, mwsServerActor) = MwsServer.CreateMwsServerActor(null)

        let checkEof() =
            printfd "Type Q for quit:"
            let line = Console.ReadLine().ToLower().Trim()
            if restartSystem then
                restartSystem <- false
                true
            else
                line = "q"

        while true do
            if checkEof() then
                Environment.Exit(0)


    while true do
        try
            serverLoop()
        with exn -> 
            Windows.Forms.MessageBox.Show(exn.ToString()) |> ignore
        

    //Console.ReadLine() |> ignore
    0
