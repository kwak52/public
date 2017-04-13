(*
 * Command set : see pp.35, XEL_60V_and_below_P_M370093-01-RvF.pdf

*)

module Dsu.Driver.Sorensen


open System
open System.Collections.Concurrent
open System.Threading
open AppConfig
open DriverExcpetionModule
open Dsu.Driver.NiVISA



/// <summary>
/// Sorensen Power Supply IP.
/// </summary>
let mutable powerSupplyIp = "192.168.0.110"

/// <summary>
/// Sorensen Power Supply Port.
/// </summary>
let mutable powerSupplyPort = 9221

let mutable powerSupplyChannel = 1

let loadFromAppConfig() =
    match readStringKey "powerSupplyIp" with
    | Some(v) -> powerSupplyIp <- v
    | _ -> ()

    match readIntKey "powerSupplyPort" with
    | Some(v) -> powerSupplyPort <- v
    | _ -> ()

    match readIntKey "powerSupplyChannel" with
    | Some(v) -> powerSupplyChannel <- v
    | _ -> ()

    logInfo "-- Sorensen Power Supply Configuration --"
    logInfo "\tIP/Port = %s %d" powerSupplyIp powerSupplyPort
    logInfo "\tChannel = %d" powerSupplyChannel


/// 내부적으로 power supplier 의 implementation 을 복수개 지원하기 위한 interface.
type internal IPowerSupplierImpl =
    inherit IDisposable
    abstract GetId : unit -> string
    abstract GetIsActive: int -> bool
    abstract SetIsActive: int * bool -> unit
    abstract GetVoltage: int -> float
    abstract GetVoltageObserve: int -> float
    abstract SetVoltage: int * float -> unit
    abstract GetCurrent: int -> float
    abstract GetCurrentObserve: int -> float
    abstract SetCurrent: int * float -> unit
    abstract IncV: int -> unit
    abstract GetDeltaV: int -> float
    abstract SetDeltaV: int * float -> unit
    abstract SendCommand: string -> unit
    abstract SendCommandGetResponse: string -> SocketResponse

let internal b2n(b) = if b then 1 else 0
let internal castTo<'T> o = (box o) :?> 'T
let internal toInterface (o:obj) =
    castTo<IPowerSupplierImpl>(o)


/// "ip:port" or "resourceName" -> (counter, PowerSupplierManagerImpl) 의 map
/// 주어진 ip, port 를 사용하고 있는 PowerSupplierManagerImpl 객체의 수를 관리
let internal powerSupplyMap = new ConcurrentDictionary<string, int * IPowerSupplierImpl>()
    
/// <summary>
/// 내부 구현용 Power Supply manager.  TCP/IP Socket 을 이용한 통신 기능 갖추고 있으나, property get/set 기능 지원 하지 않음.
/// </summary>
type internal SorensenTcpImpl(ip:string, port:int) as this =
    inherit TcpClientManager(ip, port)

    // locking object
    let locker = new obj()
    // locked execution
    let L f = lock locker f
    // locked execution for base method call 
    let L' f = 
        Monitor.Enter locker
        try
            f
        finally
            Monitor.Exit locker

    let key = sprintf "%s:%d" ip port
    let sendCommandGetResponse command = writeToSocketAndGetResponse this.Socket command
    let sendCommand(query) = this.SendCommand(query)
    let getValue(queryKey:string) =
        let query = sprintf "%s?\n" queryKey
        match sendCommandGetResponse query with
        | Response(v) ->
            let trimmed = Text.RegularExpressions.Regex.Replace(v, "[AV\r\n]+$", "")
            match Double.TryParse(trimmed.Replace(queryKey, "")) with
            | (true, volt) -> volt
            | _ -> failwithlog (sprintf "Failed to get value for %s. result = %s" query v)
        | _ -> failwithlog (sprintf "Failed to get value for %s. No response." query)

    let getVoltage(ch) = getValue(sprintf "V%d" ch)
    let getVoltageObserve(ch) = getValue(sprintf "V%dO" ch)
    let getCurrent(ch) = getValue(sprintf "I%d" ch)
    let getCurrentObserve(ch) = getValue(sprintf "I%dO" ch)
    let getIsActive(ch) = Math.Abs(getValue(sprintf "OP%d" ch) - 1.0) <= 0.0001
    let setVoltage(ch, v) = sendCommand (sprintf "V%d %f\n" ch v)
    let setCurrent(ch, v) = sendCommand (sprintf "I%d %f\n" ch v)
    let setIsActive(ch, b) = sendCommand (sprintf "OP%d %d\n" ch (b2n(b)))
    
    let dispose() =
        match powerSupplyMap.TryGetValue(key) with
        | (true, (count, itf)) ->
            if count = 1 then
                powerSupplyMap.TryRemove(key) |> ignore
                itf.Dispose()
            else
                powerSupplyMap.[key] <- (count - 1, itf)
        | _ ->
            failwithlog "Internal sorensen implementation error."

    let getId() = 
        L(fun () -> 
            match sendCommandGetResponse("*IDN?\n") with
            | Response(id) -> id
            | _ -> failwithlog "Failed to get id of powersupply.")

    do
        if not (powerSupplyMap.TryAdd(key, (1, toInterface(this)))) then
            failwithlog "Failed to add dictionary key for sorensen."

        logInfo "Sorensen manager created on %s" key

        sendCommand("*RST\n")
        sendCommand("*CLS\n")

    member x.GetId() = getId()
    member x.GetIsActive(channel) = L(fun () -> getIsActive(channel))
    member x.SetIsActive(channel, b) = L(fun () -> setIsActive(channel, b))

    member x.GetVoltage(channel) = L(fun () -> getVoltage(channel))
    member x.GetVoltageObserve(channel) = L(fun () -> getVoltageObserve(channel))
    member x.SetVoltage(channel, v) = L(fun () -> setVoltage(channel, v))

    member x.GetCurrent(channel) = L(fun () -> getCurrent(channel))
    member x.GetCurrentObserve(channel) = L(fun () -> getCurrentObserve(channel))
    member x.SetCurrent(channel, i) = L(fun () -> setCurrent(channel, i))
    
    member x.IncV(channel) = L(fun () -> sendCommand(sprintf "INCV%d\n" channel))

    member x.GetDeltaV(channel) = L(fun () -> getValue(sprintf "DELTAV%d" channel))
    member x.SetDeltaV(channel, v) = L(fun () -> sendCommand(sprintf "DELTAV%d %f" channel v))

    override __.SendCommand(command) =
        // lambda 함수로 base method 호출시 다음과 같은 오류 발생하므로, 직접 Monitor.Enter/Exit 을 이용
        // error FS0405: A protected member is called or 'base' is being used. This is only allowed in the direct implementation of members since they could escape their object scope.
//        let f() = base.SendCommand(command)
//        L(f)

        Monitor.Enter locker
        try
            base.SendCommand(command)
        finally
            Monitor.Exit locker

    override __.SendCommandGetResponse(command) =
        let mutable response:SocketResponse = Empty
        Monitor.Enter locker
        try
            response <- base.SendCommandGetResponse(command)
        finally
            Monitor.Exit locker

        response


    new () = new SorensenTcpImpl(powerSupplyIp, powerSupplyPort)

    member x.Dispose() = dispose()

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface IPowerSupplierImpl with
        member x.GetCurrent(channel) = x.GetCurrent(channel)
        member x.GetCurrentObserve(channel) = x.GetCurrentObserve(channel)
        member x.GetDeltaV(channel) = x.GetDeltaV(channel)
        member x.GetId() = x.GetId()
        member x.GetIsActive(channel) = x.GetIsActive(channel)
        member x.GetVoltage(channel) = x.GetVoltage(channel) 
        member x.GetVoltageObserve(channel) = x.GetVoltageObserve(channel) 
        member x.IncV(channel) = x.IncV(channel)
        member x.SendCommand(command) = x.SendCommand(command) 
        member x.SendCommandGetResponse(command) = x.SendCommandGetResponse(command)
        member x.SetCurrent(channel, i) = x.SetCurrent(channel, i)
        member x.SetDeltaV(channel, v) = x.SetDeltaV(channel, v)
        member x.SetIsActive(channel, b) = x.SetIsActive(channel, b)
        member x.SetVoltage(channel, v) = x.SetVoltage(channel, v)
        

type internal SorensenRs232Impl(resourceName:string, channel:int) as this =
    inherit Rs232cManager(resourceName)     // SorensenRs232cManager(resourceName)

    // locking object
    let locker = new obj()
    // locked execution
    let L f = lock locker f

    let getValue(queryKey:string) = 
        let query = sprintf "%s?\n" queryKey
        let response = this.Query(query)
        match Double.TryParse(response.Replace(queryKey, "")) with
        | (true, v) -> v
        | _ -> failwithlog (sprintf "Failed to get value for %s. result = %s" query response)


    let sendCommandGetResponse command = 
        try            
            SocketResponse.Response(this.Write(command))
        with exn ->
            SocketResponse.Exception(exn)

    let sendCommand(command) = 
        this.Write(command) |> ignore       // todo: check 필요.. write 후 read 값이 없는 경우, 바로 반환되나???

    let getVoltage(ch) = getValue(sprintf "V%d" ch)
    let getCurrent(ch) = getValue(sprintf "I%d" ch)
    let getVoltageObserve(ch) = getValue(sprintf "V%dO" ch)
    let getCurrentObserve(ch) = getValue(sprintf "I%dO" ch)
    let getIsActive(ch) = Math.Abs(getValue(sprintf "OP%d" ch) - 1.0) <= 0.0001
    let setVoltage(ch, v) = sendCommand (sprintf "V%d %f\n" ch v)
    let setCurrent(ch, v) = sendCommand (sprintf "I%d %f\n" ch v)
    let setIsActive(ch, b) = sendCommand (sprintf "OP%d %d\n" ch (b2n(b)))

    let key = resourceName 
    let dispose() =
        match powerSupplyMap.TryGetValue(key) with
        | (true, (count, itf)) ->
            if count = 1 then
                powerSupplyMap.TryRemove(key) |> ignore
                itf.Dispose()
            else
                powerSupplyMap.[key] <- (count - 1, itf)
        | _ ->
            failwithlog "Internal sorensen implementation error."

    do
        if not (powerSupplyMap.TryAdd(key, (1, toInterface(this)))) then
            failwithlog "Failed to add dictionary key for sorensen."

        logInfo "Sorensen manager created on %s" key

        sendCommand("*RST\n")
        sendCommand("*CLS\n")

    member x.GetId() = L(fun() -> x.Query("*IDN?\n"))
    member x.GetIsActive(channel) = L(fun () -> getIsActive(channel))
    member x.SetIsActive(channel, b) = L(fun () -> setIsActive(channel, b))

    member x.GetVoltage(channel) = L(fun () -> getVoltage(channel))
    member x.GetVoltageObserve(channel) = L(fun () -> getVoltageObserve(channel))
    member x.SetVoltage(channel, v) = L(fun () -> setVoltage(channel, v))

    member x.GetCurrent(channel) = L(fun () -> getCurrent(channel))
    member x.GetCurrentObserve(channel) = L(fun () -> getCurrentObserve(channel))
    member x.SetCurrent(channel, i) = L(fun () -> setCurrent(channel, i))
    
    member x.IncV(channel) = L(fun () -> sendCommand(sprintf "INCV%d\n" channel))

    member x.GetDeltaV(channel) = L(fun () -> getValue(sprintf "DELTAV%d" channel))
    member x.SetDeltaV(channel, v) = L(fun () -> sendCommand(sprintf "DELTAV%d %f" channel v))
    member x.SendCommand(command) = L(fun () -> sendCommand(command))
    member x.SendCommandGetResponse(command) = L(fun() -> Response(x.Query(command)))
    member x.Dispose() = dispose()


    interface IPowerSupplierImpl with
        member x.Dispose() = x.Dispose()
        member x.GetCurrent(channel) = x.GetCurrent(channel)
        member x.GetCurrentObserve(channel) = x.GetCurrentObserve(channel)
        member x.GetDeltaV(channel) = x.GetDeltaV(channel)
        member x.GetId() = x.GetId()
        member x.GetIsActive(channel) = x.GetIsActive(channel)
        member x.GetVoltage(channel) = x.GetVoltage(channel) 
        member x.GetVoltageObserve(channel) = x.GetVoltageObserve(channel) 
        member x.IncV(channel) = x.IncV(channel)
        member x.SendCommand(command) = x.SendCommand(command) 
        member x.SendCommandGetResponse(command) = x.SendCommandGetResponse(command)
        member x.SetCurrent(channel, i) = x.SetCurrent(channel, i)
        member x.SetDeltaV(channel, v) = x.SetDeltaV(channel, v)
        member x.SetIsActive(channel, b) = x.SetIsActive(channel, b)
        member x.SetVoltage(channel, v) = x.SetVoltage(channel, v)
        


type internal createParameter =
    /// ip, port, channel
    | Tcp of string * int * int
    /// resource-name, channel
    | Rs232 of string * int
    

/// <summary>
/// Sorensen Power Supply manager.  composition 을 통해서 대리로 기능 수행.  property get/set 기능 갖춤.
/// 직접 생성하지 말고, createManager 를 통해서 생성할 것
/// </summary>
type PowerSupplierManager private (createParameter) =
    let channel = 
        match createParameter with
        | Tcp(_, _, ch) -> ch
        | Rs232(_, ch) -> ch

    let impl: IPowerSupplierImpl = 
        let implObj: obj = 
            match createParameter with
            | Tcp(ip, port, channel) ->
                let key = sprintf "%s:%d" ip port
                match powerSupplyMap.TryGetValue(key) with
                | (true, tpl) ->
                    let existing = (snd tpl) :?> SorensenTcpImpl
                    powerSupplyMap.[key] <- ((fst tpl) + 1, snd tpl)
                    box(existing)
                | _ ->
                    box(new SorensenTcpImpl(ip, port))
            | Rs232(resourceName, channel) ->
                box(new SorensenRs232Impl(resourceName, channel))
        toInterface(implObj)

    let rs232Impl() = castTo<SorensenRs232Impl>(impl)

    let procException:(Exception -> unit) = fun exn -> ()

    new () = new PowerSupplierManager(Tcp(powerSupplyIp, powerSupplyPort, 1))
    new (ip, port, channel) = new PowerSupplierManager(Tcp(ip, port, channel))
    new (resourceName, channel) = new PowerSupplierManager(Rs232(resourceName, channel))

    /// <summary>
    /// Sorensen Power Supply Manager 에서 exception 이 발생했을 때 수행할 사용자의 action 등록
    /// </summary>
    member val ProcException:(Exception -> unit) = procException with get, set

    //interface IPowerSupplierManager with
    member x.Id with get() = impl.GetId()
    member val ChannelId = channel with get, set
    member x.Voltage
        with get() = impl.GetVoltage(channel)
        and set(v) = impl.SetVoltage(channel, v)
    member x.Current
        with get() = impl.GetCurrent(channel)
        and set(v) = impl.SetCurrent(channel, v)

    member x.VoltageObserve with get() = impl.GetVoltageObserve(channel)
    member x.CurrentObserve with get() = impl.GetCurrentObserve(channel)

    member x.IsActive
        with get() = impl.GetIsActive(channel)
        and set(b) = impl.SetIsActive(channel, b)

    member x.DeltaV
        with get() = impl.GetDeltaV(channel)
        and set(v) = impl.SetDeltaV(channel, v)

    member x.IncV() = impl.IncV(channel)
    member x.IncChV(channel) = impl.IncV(channel)

    member x.SetAllActive(b) = impl.SendCommand(sprintf "OPALL %d\n" (b2n(b)))
    member x.Clear(v) = impl.SendCommand(sprintf "*CLS %d\n" v)
    member x.Reset(v) = impl.SendCommand(sprintf "*RST %d\n" v)
    member x.Config
        with get() =
            match impl.SendCommandGetResponse("CONFIG?\n") with
                | Response(v) -> v
                | _ -> failwithlog "Failed to get value for config query."
                    



    member __.Rs232cManager with get() = castTo<Rs232cManager>(impl)
    member __.Serial with get() = rs232Impl().Serial
    member __.BaudRate with get() = rs232Impl().BaudRate and set(v) = rs232Impl().BaudRate <- v
    member __.DataBits with get() = rs232Impl().DataBits and set(v) = rs232Impl().DataBits <- v
    member __.StopBits with get() = rs232Impl().StopBits and set(v) = rs232Impl().StopBits <- v
    member __.Parity with get() = rs232Impl().Parity and set(v) = rs232Impl().Parity <- v
    member __.FlowControl with get() = rs232Impl().FlowControl and set(v) = rs232Impl().FlowControl <- v
    member __.TerminationCharacterEnabled with get() = rs232Impl().TerminationCharacterEnabled and set(v) = rs232Impl().TerminationCharacterEnabled <- v

    interface IDisposable with
        member __.Dispose() = 
            impl.Dispose()


/// <summary>
/// Exception safe Sorensen manager singleton
/// If exception occurs on paix manager, new instance is automatically created.
/// </summary>
let mutable private sorensenManagerSingleton: PowerSupplierManager option = None


/// <summary>
/// Power supply manager(Sorensen) 객체에 대한 interface 를 생성한다.
/// </summary>
/// <param name="ip">기기의 ip</param>
/// <param name="port">기기의 port</param>
/// <param name="channel">활성 channel.  추후 ChannelId property 를 통해 바꿀 수 있다.</param>
let rec createManager ip port channel =
    try
        let sorensen = new PowerSupplierManager(ip, port, channel)
        sorensen.ProcException <- fun exn -> createManager ip port channel |> ignore
        sorensenManagerSingleton <- Some(sorensen) 
    with exn ->
        DriverExceptionSubject.OnNext(SorensenException(exn))
        raise exn

/// Power supply manager(Sorensen) 객체에 대한 interface 를 생성한다.
let createManagerSimple() = createManager powerSupplyIp powerSupplyPort powerSupplyChannel


/// <summary>
/// Returns exception safe singleton instance
/// sorensen manager 에서 exception 이 발생하는 경우, 새로운 객체를 생성하도록 하여 반환한다.
/// </summary>
let manager() =
    sorensenManagerSingleton.Value

