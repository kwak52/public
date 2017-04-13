module Dsu.Driver.Plc.Melsec.MxPlcParameters

open System
open FSharp.Interop.Dynamic     // http://tomasp.net/blog/dynamic-sql.aspx/
open Dsu.Common.Utilities.FsReflection
open Dsu.Driver.Plc.Melsec.Type

(*
    ACTETHERLib.XXX.{TCP, UDP}   XXX = 
        ActAJ71E71, ActAJ71QE71, ActLCPU, ActMLAJ71E71TCP, ActMLAJ71QE71TCP, ActMLLCPUTCP, ActMLQJ71E71TCP, ...
    ACTETHERLib.YYY.  YYY = 
        ActFXENETTCP, ActMLFXENETTCP
*)

/// <summary>
/// QJ71E71{TCP, UDP}, QNUDECPU{TCP, UPD} 공통 사항
/// </summary>
[<AbstractClass>]
type PlcParametersBase (cpuType:CpuType, ip:string) =
    member val ActCpuType                   = cpuType with get, set
    member val ActDestinationIONumber       = 0 with get, set
    member val ActDidPropertyBit            = 1 with get, set
    member val ActDsidPropertyBit           = 1 with get, set
    member val ActHostAddress               = ip with get, set
    member val ActIONumber                  = 1023 with get, set
    member val ActMultiDropChannelNumber    = 0 with get, set
    member val ActNetworkNumber             = 1 with get, set
    member val ActPassword                  = "" with get, set
    member val ActStationNumber             = 1 with get, set
    member val ActThroughNetworkType        = 0 with get, set
    member val ActTimeOut                   = 10000 with get, set
    member val ActUnitNumber                = 0 with get, set


    abstract NetworkCardInterfaceType: NetworkCardInterface with get

    abstract ActInterface : obj with get
    abstract ActInterfaceType : ActType with get
    abstract GetFinalObject : unit -> obj
//    override x.ToString() =
//        GetPropertyInfo(x.GetFinalObject()) |> Seq.map(fun (name, value) -> sprintf "%s\t%O\n" name value) |> String.Concat

    abstract ApplyToActObject: unit -> unit
    default x.ApplyToActObject () =
        let act = x.ActInterface
        act?ActPassword                 <- x.ActPassword
        act?ActCpuType                  <- x.ActCpuType.GetInteger()
        act?ActDestinationIONumber      <- x.ActDestinationIONumber
        act?ActDidPropertyBit           <- x.ActDidPropertyBit
        act?ActDsidPropertyBit          <- x.ActDsidPropertyBit
        act?ActHostAddress              <- x.ActHostAddress
        act?ActIONumber                 <- x.ActIONumber
        act?ActMultiDropChannelNumber   <- x.ActMultiDropChannelNumber
        act?ActNetworkNumber            <- x.ActNetworkNumber
        act?ActStationNumber            <- x.ActStationNumber
        act?ActThroughNetworkType       <- x.ActThroughNetworkType
        act?ActTimeOut                  <- x.ActTimeOut
        act?ActUnitNumber               <- x.ActUnitNumber

/// <summary>
/// Melsec PLC parameter (TCP type)
/// </summary>
type PlcParametersQJ71E71TCP(cpuType, ip) =
    inherit PlcParametersBase(cpuType, ip)

    let act = new ACTETHERLib.ActQJ71E71TCPClass()
    let networkCardInterface = NetworkCardInterface.QJ71E71TCP(act)
    override val ActInterface = act :> obj with get
    override val ActInterfaceType = ActType.QJ71E71TCP with get
    override val NetworkCardInterfaceType = networkCardInterface with get
    override x.GetFinalObject() = box(x)

    member val ActConnectUnitNumber = 0 with get, set
    member val ActSourceNetworkNumber = 1 with get, set
    member val ActSourceStationNumber = 2 with get, set


    member val ActDestinationPortNumber = 0 with get, set       // only valid on TCP connection.

    override x.ApplyToActObject () =
        base.ApplyToActObject()
        act.ActConnectUnitNumber        <- x.ActConnectUnitNumber
        act.ActSourceNetworkNumber      <- x.ActSourceNetworkNumber
        act.ActSourceStationNumber      <- x.ActSourceStationNumber
        act.ActDestinationPortNumber    <- x.ActDestinationPortNumber


/// <summary>
/// Melsec QJ71E71UDP PLC parameter (UDP type)
/// </summary>
type PlcParametersQJ71E71UDP(cpuType, ip) =
    inherit PlcParametersBase(cpuType, ip)

    let act = new ACTETHERLib.ActQJ71E71UDPClass()
    let networkCardInterface = NetworkCardInterface.QJ71E71UDP(act)
    override val NetworkCardInterfaceType = networkCardInterface with get
    override val ActInterface = act :> obj with get
    override val ActInterfaceType = ActType.QJ71E71UDP with get
    override x.GetFinalObject() = box(x)

    member val ActConnectUnitNumber = 0 with get, set
    member val ActSourceNetworkNumber = 1 with get, set
    member val ActSourceStationNumber = 2 with get, set

    member val ActPortNumber = 5001 with get, set       // only valid on UDP connection.
    
    override x.ApplyToActObject() =
        base.ApplyToActObject()
        act.ActConnectUnitNumber        <- x.ActConnectUnitNumber
        act.ActSourceNetworkNumber      <- x.ActSourceNetworkNumber
        act.ActSourceStationNumber      <- x.ActSourceStationNumber
        act.ActPortNumber               <- x.ActPortNumber

  
            
















/// <summary>
/// Melsec PLC parameter (TCP type)
/// </summary>
type PlcParametersQNUDECPUTCP(cpuType, ip) =
    inherit PlcParametersBase(cpuType, ip)

    let act = new ACTETHERLib.ActQNUDECPUTCPClass()
    let networkCardInterface = NetworkCardInterface.QNUDECPUTCP(act)
    override val ActInterface = act :> obj with get
    override val ActInterfaceType = ActType.QNUDECPUTCP with get
    override val NetworkCardInterfaceType = networkCardInterface with get
    override x.GetFinalObject() = box(x)

    member val ActIntelligentPreferenceBit = 0 with get, set

    override x.ApplyToActObject () =
        base.ApplyToActObject()
        //act.ActIntelligentPreferenceBit <- x.ActIntelligentPreferenceBit


/// <summary>
/// Melsec QJ71E71UDP PLC parameter (UDP type)
/// </summary>
type PlcParametersQNUDECPUUDP(cpuType, ip) =
    inherit PlcParametersBase(cpuType, ip)

    let act = new ACTETHERLib.ActQNUDECPUUDPClass()
    let networkCardInterface = NetworkCardInterface.QNUDECPUUDP(act)
    override val NetworkCardInterfaceType = networkCardInterface with get
    override val ActInterface = act :> obj with get
    override val ActInterfaceType = ActType.QNUDECPUUDP with get
    override x.GetFinalObject() = box(x)

    member val ActIntelligentPreferenceBit = 0 with get, set
    member val ActDirectConnectionBit = 0 with get, set

    override x.ApplyToActObject () =
        base.ApplyToActObject()
        act.ActIntelligentPreferenceBit <- x.ActIntelligentPreferenceBit
        act.ActDirectConnectionBit      <- x.ActDirectConnectionBit
