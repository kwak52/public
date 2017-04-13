[<AutoOpen>]
module Dsu.Driver.NiVISA


open System
open System.Threading
open System.Threading.Tasks
open System.Text.RegularExpressions
open System.Reactive.Subjects
open NationalInstruments.VisaNS


// - VISA dll 위치
//  C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 15.5\NationalInstruments.Visa.dll
//  C:\Program Files (x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 15.5\NationalInstruments.Visa.dll
// - VISA sample file 위치
//C:\Users\Public\Documents\National Instruments\NI-VISA\Examples\.NET\15.5


/// <summary>
/// NI VISA manager
/// - Resource name : see pp.4-3, NI-VISA-user-manual-370423a.pdf
///     * Serial INSTR : "ASRL[board][::INSTR]
///     * TCPIP INSTR : "TCPIP[board]::host address[::LAN device name][::INSTR]"
///     * TCPIP SOCKET : "TCPIP[board]::host address::port::SOCKET"
/// </summary>
type VisaManager(resourceName:string) =
    // locking object
    let locker = new obj()
    // locked execution
    let L f = lock locker f
    let lock() = Monitor.Enter locker
    let unlock() = Monitor.Exit locker

    let resourceManager = ResourceManager.GetLocalManager()
    let session = resourceManager.Open(resourceName) :?> MessageBasedSession
    let doQuery(q:string, removeNewline) =
        //L(fun () -> session.Clear())                             // 필요시 session.Clear 수행
        let result = L(fun () -> session.Query(q))
        if removeNewline
            then result.Replace("\r", "").Replace("\n", "")        // Query 결과에서 \r\n 제거
            else result

    do
        logInfo "Visa manager created on resource: %s" resourceName
        // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
        session.SynchronizeCallbacks <- true

    static member FindPorts(pattern) : string array =
        ResourceManager.GetLocalManager().FindResources(pattern)
    
    member val Session = session with get
    member x.QuerySimple(query:string) = doQuery(query, true)
    member x.Query(query:string) = doQuery(query, false)
    member x.Clear() = L(fun () -> session.Clear())

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
    /// <summary>
    /// NI VISA 를 통해, session 에 async 로 write 하고, response string 의 task 를 반환한다.
    /// </summary>
    /// <param name="msg"></param>
    // https://blog.justjuzzy.com/2012/10/turn-iasyncresult-code-into-await-keyword/
    // https://msdn.microsoft.com/en-us/library/dd997423(v=vs.110).aspx
    member x.WriteAsync(msg:string) : Task<string> =
        let state = null  // new obj()
        let beginWrite = Func<string, AsyncCallback, obj, IAsyncResult>(fun msg callback o ->
            lock()
            session.BeginWrite(msg, callback, o))
        let endWrite = Func<IAsyncResult, string>(fun a -> 
            session.EndWrite(a)
            unlock()
            session.LastStatus.ToString())
        Task<string>.Factory.FromAsync(
            beginWrite,
            endWrite,
            msg, state)

    /// <summary>
    /// NI VISA 를 통해, session 에 write 하고, response string 을 반환한다.
    /// </summary>
    /// <param name="msg"></param>
    member x.Write(msg) =
        async {
            return! x.WriteAsync(msg) |> Async.AwaitTask 
        } |> Async.RunSynchronously
        

    member x.ReadAsync() =
        let state:obj = null  // new obj()
        let bufferSize = 1024
        let beginRead = Func<AsyncCallback,obj,IAsyncResult>(fun callback state' ->
            lock()
            session.BeginRead(bufferSize, callback, state'))
        let endRead = Func<IAsyncResult, string>(fun a ->
            unlock()
            session.EndReadString(a))

        Task<string>.Factory.FromAsync(beginRead, endRead, state)

    member x.Read() =
        async {
            return! x.ReadAsync() |> Async.AwaitTask 
        } |> Async.RunSynchronously

    interface IDisposable with
        member x.Dispose() =
            session.Dispose()        



/// <summary>
/// NI VISA 를 이용한 RS232C 구현
/// </summary>
type Rs232cManager(resourceName:string) =
    inherit VisaManager(resourceName)

    let serial = base.Session :?> SerialSession
    static member val AllCOMPorts = VisaManager.FindPorts("?*")
    new (comPort:int) = new Rs232cManager(sprintf "ASRL%d::INSTR" comPort)

    // https://forums.ni.com/t5/Instrument-Control-GPIB-Serial/How-do-I-read-from-a-serial-485-card-in-a-C-program/td-p/309486
    member val Serial = serial with get
    member __.BaudRate with get() = serial.BaudRate and set(v) = serial.BaudRate <- v
    member __.DataBits with get() = serial.DataBits and set(v) = serial.DataBits <- v
    member __.StopBits with get() = serial.StopBits and set(v) = serial.StopBits <- v
    member __.Parity with get() = serial.Parity and set(v) = serial.Parity <- v
    member __.FlowControl with get() = serial.FlowControl and set(v) = serial.FlowControl <- v
    member __.TerminationCharacterEnabled with get() = serial.TerminationCharacterEnabled and set(v) = serial.TerminationCharacterEnabled <- v

/// <summary>
/// Read-only monitoring manager.   
/// 생성자에서 serial port 에 event 가 발생하면 수행할 action 을 등록한다.  (단 action 을 F# fun 으로 변환해서 등록해야 한다.)
/// serial port 에서의 event 는 정상적으로 읽은 문자열 값과 오류 상황에서의 exception 둘 다 가능하고, object type 으로 action 에 전달된다.
/// </summary>
// C# 에서 사용 예
// <code>
// var f = FuncConvert.ToFSharpFunc( (object evt) =>
//      if (evt is string)
//          Trace.WriteLine($"Got string: {(string)evt}"));
//      else if (evt is Exception)
//          ....
// var rs232 = new Rs232cMonitor("ASRL3::INSTR", f) { BaudRate = 115200 };
// </code>
type Rs232cMonitor(resourceName:string, action:obj->unit) as this =
    inherit Rs232cManager(resourceName)

    let serial = this.Serial
    
    let handler = new SerialSessionEventHandler(fun sndr e ->
        try
            let msg = serial.ReadString() |> removeNewline
            action(msg)
        with exn ->
            logWarn "Exception on Rs232cMonitor: %O" exn
            action(exn))
    do
        logInfo "Starting Rs232cMonitor service on %s." resourceName

        // AnyCharacterReceived 이벤트 등록
        // http://www.hivmr.com/db/dcx1cffxcjcackf71ccmamd8xz7pm1zk
        serial.add_AnyCharacterReceived handler
        serial.EnableEvent(SerialSessionEventType.AnyCharacterReceived, EventMechanism.Handler)

    /// Pause monitoring (pause 상태에서의 serial port 수신 내용은 discard 된다.)
    member __.Pause() = serial.remove_AnyCharacterReceived handler
    /// Resume monitoring
    member __.Resume() = serial.add_AnyCharacterReceived handler




/// <summary>
/// Sorensen Power Supply 의 기능 구현 class.
/// internal implementation
/// </summary>
type internal VisaSorensenImpl(manager:VisaManager) =
    let (|AnswerPattern|_|) pattern input =
        let m = Regex.Match(input,pattern) 
        if (m.Success) then Some m.Groups.[1].Value else None  

    let testVoltage str =
        match str with
        | AnswerPattern "[VI]?\d+ (.*)" v -> 
            Some(System.Double.Parse(v))
        | _ ->
            printfn "The value '%s' is something else" str
            None

    /// returns id of sorensen power supply : "SORENSEN, XEL 30-3P, J00466702, 3.02-4.05"
    member x.Id with get() = manager.QuerySimple("*IDN?\n")
    member x.SetVoltage(volt) = manager.Write (sprintf "v1 %f\n" volt) = "Success"
    member x.SetCurrent(channel, mA) = manager.Write (sprintf "I%d %f\n" channel mA) = "Success"
    member x.GetVoltage(channel:string) = 
        // https://fsharpforfunandprofit.com/posts/convenience-active-patterns/
        manager.QuerySimple channel     // "V1 3.100"
            |> testVoltage                  // Some(3.1)
    member x.GetVoltage(channel:int) = x.GetVoltage (sprintf "v%d?\n" channel)

    member x.GetCurrent(channel:int) = 
        manager.QuerySimple (sprintf "I%d?\n" channel) |> testVoltage




/// <summary>
/// 예제 : NI VISA RS232C 를 이용한 Sorensen Power supply 구현
/// 실제 구현은 Ethernet 으로 연결되며, PowerSupplierManager 를 이용한다. (Sorensen.fs)
/// </summary>
type SorensenRs232cManager(resourceName:string) as this =
    inherit Rs232cManager(resourceName)

    let sorensenImpl = new VisaSorensenImpl(this)
    member x.Id with get() = sorensenImpl.Id
    member x.SetVoltage(volt) = sorensenImpl.SetVoltage(volt)
    member x.GetVoltage(channel:string) = sorensenImpl.GetVoltage(channel)
    member x.GetVoltage(channel:int) = sorensenImpl.GetVoltage(channel)
    member x.SetCurrent(channel, mA) = sorensenImpl.SetCurrent(channel, mA)
    member x.GetCurrent(channel:int) = sorensenImpl.GetCurrent(channel)

//    interface IPowerSupply with
//        member x.Id with get() = this.Id
//        member x.SetVoltage(volt) = this.SetVoltage(volt)
//        member x.GetVoltage(channel:string) = this.GetVoltage(channel)
//        member x.GetVoltage(channel:int) = this.GetVoltage(channel)

    new (comPort:int) = new SorensenRs232cManager(sprintf "ASRL%d::INSTR" comPort)



/// <summary>
/// NI VISA 를 이용한 TCP/IP ethernet manager.
/// </summary>
type EthernetManager(resourceName:string) =
    inherit VisaManager(resourceName)

    static member internal createResourceName ip port =
        let board = 0
        // http://zone.ni.com/reference/en-XX/help/370131S-01/ni-visa/visaresourcesyntaxandexamples/
        sprintf "TCPIP%d::%s::%d::SOCKET" board ip port
    new (ip, port) = new EthernetManager(EthernetManager.createResourceName ip port)





/// <summary>
/// NI VISA 를 이용한 Sorensen power supply 구현
/// </summary>
type SorensenEthernetManager(resourceName:string) as this =
    inherit EthernetManager(resourceName)

    let sorensenImpl = new VisaSorensenImpl(this)
    member x.Id with get() = sorensenImpl.Id
    member x.SetVoltage(volt) = sorensenImpl.SetVoltage(volt)
    /// channel : e.g "V1?\n"
    member x.GetVoltage(channel:string) = sorensenImpl.GetVoltage(channel)
    member x.GetVoltage(channel:int) = sorensenImpl.GetVoltage(channel)
    member x.SetCurrent(channel, mA) = sorensenImpl.SetCurrent(channel, mA)
    member x.GetCurrent(channel:int) = sorensenImpl.GetCurrent(channel)

//    interface IPowerSupply with
//        member x.Id with get() = this.Id
//        member x.SetVoltage(volt) = this.SetVoltage(volt)
//        member x.GetVoltage(channel:string) = this.GetVoltage(channel)
//        member x.GetVoltage(channel:int) = this.GetVoltage(channel)

    new (ip, port) = 
        // sorensen ethernet 관련 visa name 예제 : "TCPIP0::192.168.1.100::9221::SOCKET".  XEL_60V_and_below_P_M370093-01-RvF.pdf
        let resourceName = EthernetManager.createResourceName ip port
        new SorensenEthernetManager(resourceName)


/// <summary>
/// NI VISA 에서 Ethernet 은 sorensen power 는 잘 되나, hioki 는 좀 문제가 있는 듯.
/// Resource 검색시에 sorensen 은 자동으로 올라오나, hioki 는 안올라옴.
/// Query 수행시 sorensen 은 잘되나, hioki 는 timeout 오류 발생함.
/// </summary>
//[<Obsolete("Use Hioki.LCRMeterManager instead.")>]
//type HiokiEthernetManager(resourceName:string) as this =
//    inherit EthernetManager(resourceName)
//
//    member __.GetId() = this.Query("*IDN?\r\n")
//    //member x.Id = x.SendCommandGetResponse "*RST\n:TRIGger EXTernal\n:FREQuency 200E+03\n:*TRG\n:MEASure?\n"
//
//    //member x.Id with get() = x.Query("*IDN?")
//
//
//    /// <summary>
//    /// Default
//    /// </summary>
//    /// <param name="ip"></param>
//    /// <param name="port">default = 3500</param>
//    new (ip, port) = 
//        // sorensen ethernet 관련 visa name 예제 : "TCPIP0::192.168.1.100::9221::SOCKET".  XEL_60V_and_below_P_M370093-01-RvF.pdf
//        let resourceName = EthernetManager.createResourceName ip port
//        new HiokiEthernetManager(resourceName)
