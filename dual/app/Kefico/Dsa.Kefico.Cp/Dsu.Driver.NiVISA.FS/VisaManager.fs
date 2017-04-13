[<AutoOpen>]
module NiVISA


open System
open NationalInstruments.VisaNS
open System.Threading.Tasks

let findResourcesWithPattern pattern = ResourceManager.GetLocalManager().FindResources(pattern)
let findResources() = findResourcesWithPattern "?*"

type Rs232cManager(resourceName:string) =

    let resourceManager = ResourceManager.GetLocalManager()
    let session = resourceManager.Open(resourceName) :?> MessageBasedSession
    let doQuery(q:string) = session.Clear(); session.Query(q).Replace("\r", "").Replace("\n", "")

    do
        session.SynchronizeCallbacks <- true

    static member FindPorts(pattern) : string array =
        ResourceManager.GetLocalManager().FindResources(pattern)
    
    static member val AllPorts = Rs232cManager.FindPorts("?*")

    member val Session = session with get
    member x.Query(query:string): string = doQuery(query)

    /// returns id of sorensen power supply : "SORENSEN, XEL 30-3P, J00466702, 3.02-4.05"
    member val Id = doQuery("*IDN?\n") with get

    (*
     * C# implementation:
            Func<IAsyncResult, string> finishWrite = result => { mbSession.EndWrite(result); return mbSession.LastStatus.ToString(); };
            Task<string>.Factory.FromAsync(
                mbSession.BeginWrite,
                finishWrite, //mbSession.EndWrite,
                textToWrite,
                (object)null
            );
    *)
    // https://blog.justjuzzy.com/2012/10/turn-iasyncresult-code-into-await-keyword/
    // https://msdn.microsoft.com/en-us/library/dd997423(v=vs.110).aspx
    member x.WriteAsync(msg:string) =
        let state = null  // new obj()
        let beginWrite = Func<string, AsyncCallback, obj, IAsyncResult>(fun msg callback o -> session.BeginWrite(msg, callback, o))
        let endWrite = Func<IAsyncResult, string>(fun a -> 
            session.EndWrite(a)
            session.LastStatus.ToString())
        Task<string>.Factory.FromAsync(
            beginWrite,
            endWrite,
            msg, state)

    member x.Write(msg) =
        async {
            return! x.WriteAsync(msg) |> Async.AwaitTask 
        } |> Async.RunSynchronously
        

    member x.ReadAsync() =
        let state:obj = null  // new obj()
        let bufferSize = 1024
        let beginRead = Func<AsyncCallback,obj,IAsyncResult>(fun callback state' -> session.BeginRead(bufferSize, callback, state'))
        let endRead = Func<IAsyncResult, string>(fun a -> session.EndReadString(a))

        Task<string>.Factory.FromAsync(beginRead, endRead, state)

    member x.Read(msg) =
        async {
            return! x.ReadAsync() |> Async.AwaitTask 
        } |> Async.RunSynchronously
        
