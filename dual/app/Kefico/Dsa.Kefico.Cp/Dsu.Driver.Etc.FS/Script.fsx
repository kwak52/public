// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

//#load "Library1.fs"
//open Dsu.Driver.Etc.FS

// Define your library scripting code here



open System
open System.Net
open System.Net.Sockets





let clientSocket1 = new System.Net.Sockets.TcpClient("192.168.0.100", 9221)
let writer = new IO.BinaryWriter(clientSocket1.GetStream())
let command = @"v1 5\n"
writer.Write(command)




open System
open System.Net
open System.Net.Sockets



let clientSocket2 = new System.Net.Sockets.TcpClient()
clientSocket2.Connect("192.168.0.100", 9221)
let serverStream = clientSocket2.GetStream()
let bytes = Text.Encoding.ASCII.GetBytes(@"v1 7\n")


serverStream.Write(bytes, 0, bytes.Length)
serverStream.Flush()




let permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts)
