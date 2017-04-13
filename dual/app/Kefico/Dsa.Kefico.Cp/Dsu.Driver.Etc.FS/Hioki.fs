(*
 * Command set : see Hioki/English/index.html page on installation CD
 *)


module Dsu.Driver.Hioki


open System
open AppConfig
open Log4NetWrapper
open DriverExcpetionModule



/// <summary>
/// Hioki Power Supply IP.
/// </summary>
let mutable lcrIp = "192.168.0.50"

/// <summary>
/// Hioki Power Supply Port.
/// </summary>
let mutable lcrPort = 3500


let loadFromAppConfig() =
    match readStringKey "lcrIp" with
    | Some(v) -> lcrIp <- v
    | _ -> ()

    match readIntKey "lcrPort" with
    | Some(v) -> lcrPort <- v
    | _ -> ()

    logInfo "-- Hioki Configuration --"
    logInfo "\tIP/Port = %s %d" lcrIp lcrPort


/// <summary>
/// HIOKI LCR Meter 관리자
/// </summary>
type LCRMeterManager(ip:string, port:int) as this =
    inherit TcpClientManager(ip, port)
    let procException:(Exception -> unit) = fun exn -> ()
    let newline = "\r\n"
    let addNewLine command = command + newline
    let stripNewLine msg:string =
        Text.RegularExpressions.Regex.Replace(msg, "[\\r\\n]*$", "")
    let bool2OnOff b = if b then "ON" else "OFF"

    let query command = 
        match (this.SendCommandGetResponse (addNewLine command)) with
        | Response(v) -> v
        | Exception(exn) ->
            let msg = sprintf "Failed executing command [%s] on hioki id on %s:%d. Exception=\n%O" command ip port exn
            failwithlog msg
        |_ -> 
            let msg = sprintf "Failed executing command [%s] on hioki id on %s:%d" command ip port
            failwithlog msg

    let write command = this.SendCommand (addNewLine command)
    let mutable mode = ""

    let stringPropertyGetter command = query (sprintf ":%s?" command)
    let stringPropertySet command value = write (sprintf ":%s %s" command value)

    let stringPropertySetter command value (enums: string seq) =
        let enumLookup = buildSimpleLookup enums
        if enumLookup.Contains value then
            write (sprintf ":%s %s" command value)
            logDebug "HIOKI: %s <- %s" command value
        else
            let msg = sprintf "Failed to set property %s with value %s.  Possible values = {%s}" command value (String.Join(",", enums))
            failwithlog msg

    // command 로 query 를 날려서 "ON" "OFF" 값에 따라 boolean 값으로 반환한다.
    let onOffPropertyGetter command =
        let v = query (sprintf ":%s?" command) |> stripNewLine
        match v with
        | "ON" -> true
        | "OFF" -> false
        | _ -> failwithlog (sprintf "Failed to get property for command %s" command)

    // boolean value 값에 따라서 command 의 "ON" / "OFF" 값으로 설정한다.
    let onOffPropertySetter command value =
        let onoff = bool2OnOff value
        logDebug "HIOKI: %s <- %s" command onoff
        write (sprintf ":%s %s" command onoff)

    let doublePropertyGetter command =
        match Double.TryParse (query (sprintf ":%s?" command)) with
        | (true, v) -> v
        | _ -> failwithlog (sprintf "Failed to get property for command %s" command)
    let doublePropertySetter command value =
        logDebug "HIOKI: %s <- %f" command value
        write (sprintf ":%s %f" command value) |> ignore

    let intPropertyGetter command =
        match Int32.TryParse (query (sprintf ":%s?" command)) with
        | (true, v) -> v
        | _ -> failwithlog (sprintf "Failed to get property for command %s" command)
    let intPropertySetter command value =
        logDebug "HIOKI: %s <- %d" command value
        write (sprintf ":%s %d" command value) |> ignore
    
    let id = query ("*IDN?") |> stripNewLine


    /// <summary>
    /// Hioki Manager 에서 exception 이 발생했을 때 수행할 사용자의 action 등록
    /// </summary>
    member val ProcException:(Exception -> unit) = procException with get, set

    /// Get HIOKI instrument ID
      member val Id = id with get
    /// Default LCR 사용, CONTINUOUS 일 경우 Remote 접근 금지(Local에서 LCR 모드로 변경해야함)
    member __.Mode 
        with get() = stringPropertyGetter "Mode" 
        and set(v) = stringPropertySetter "Mode" v [|"LCR"; "CONTINUOUS";|]
    ///600,000 ~ 660,000 Hz 시험 예상치
    member __.Frequency 
        with get() = doublePropertyGetter "Frequency"
        and set(v) = doublePropertySetter "Frequency" v 
    /// Default V 사용
    member __.Level 
        with get() = stringPropertyGetter "Level" 
        and set(v) = stringPropertySetter "Level" v [|"V"; "CV"; "DV"|]
    /// Default MEDIUM 사용, Fast 일때 샘플링은 빠르지만 정확도 떨어짐
    member __.Speed 
        with get() = stringPropertyGetter "Speed" 
        and set(v) = stringPropertySetter "Speed" v [|"FAST"; "MEDIUM"; "SLOW"; "SLOW2"|]
    /// Default EXTERNAL 사용, EXTERNAL은 이벤트 시점시 데이터 취득 (Trigger:Delay 설정 무의미)
    member __.Trigger 
        with get() = stringPropertyGetter "Trigger" 
        and set(v) = stringPropertySetter "Trigger" v [|"INTERNAL"; "EXTERNAL"|]
    /// Default 0.1 (100ms), INTERNAL 일 경우 사용
    member __.TriggerDelay 
        with get() = doublePropertyGetter "Trigger:Delay"
        and set(v) = doublePropertySetter "Trigger:Delay" v 
    /// Default false [|"FAST"; "MEDIUM"; "SLOW"; "SLOW2"|] 사용, true 일 경우 Speed 값을 직접 입력해야함
    member __.Wave 
        with get() = onOffPropertyGetter "Wave"
        and set(v) = onOffPropertySetter "Wave" v
    ///기준전압 조절기능 Bias 사용안함
    member __.DCBias 
        with get() = onOffPropertyGetter "DCBias"
        and set(v) = onOffPropertySetter "DCBias" v
    /// MEASure 대상 설정 Default "Cs, Rdc" = "8,64,0"  
    ///[128(Lp)     + 64(Ls)     + 32(D)      + 16(Cp)     + 8(Cs)     + 4(Θ)      + 2(Y)    + 1(Z)],
    ///[128(unused) + 64(RDC)    + 32(B)      + 16(X)      + 8(Rp)     + 4(G)      + 2(Rs)   + 1(Q)],
    ///[128(unused) + 64(unused) + 32(unused) + 16(unused) + 8(unused) + 4(unused) + 2(E)    + 1(S)]
    member __.MeasureItem 
        with get() = stringPropertyGetter "MEAS:ITEM"
        and set(v) = stringPropertySet "MEAS:ITEM" v
    /// MEASure Remote Event
    member __.MEASure 
        with get() = stringPropertyGetter "MEASure"
    /// Default ASCii
    member __.Format 
        with get() = stringPropertyGetter "FORMat:DATA"
        and set(v) = stringPropertySetter "FORMat:DATA" v [|"ASCii"; "REAL"|]
    /// Display# 대상 설정 Default "Cs, Rdc"  기타 : "Z", "Y", "PHASE", "CS", "CP", "D", "LS", "LP", "Q", "RS", "G", "RP", "X", "B", "RDC", "S", "E", "OFF"
    member __.Display1 
        with get() = stringPropertyGetter "PARameter1"
        and set(v) = stringPropertySetter "PARameter1" v [|"Z"; "Y"; "PHASE"; "CS"; "CP"; "D"; "LS"; "LP"; "Q"; "RS"; "G"; "RP"; "X"; "B"; "RDC"; "S"; "E"; "OFF"|]

    member __.Display2
        with get() = stringPropertyGetter "PARameter2"
        and set(v) = stringPropertySetter "PARameter2" v [|"Z"; "Y"; "PHASE"; "CS"; "CP"; "D"; "LS"; "LP"; "Q"; "RS"; "G"; "RP"; "X"; "B"; "RDC"; "S"; "E"; "OFF"|]

    member __.Display3
        with get() = stringPropertyGetter "PARameter3"
        and set(v) = stringPropertySetter "PARameter3" v [|"Z"; "Y"; "PHASE"; "CS"; "CP"; "D"; "LS"; "LP"; "Q"; "RS"; "G"; "RP"; "X"; "B"; "RDC"; "S"; "E"; "OFF"|]

    member __.Display4
        with get() = stringPropertyGetter "PARameter4"
        and set(v) = stringPropertySetter "PARameter4" v [|"Z"; "Y"; "PHASE"; "CS"; "CP"; "D"; "LS"; "LP"; "Q"; "RS"; "G"; "RP"; "X"; "B"; "RDC"; "S"; "E"; "OFF"|]
    /// Initializing the instrument
    member __.Reset 
        with set(v) = stringPropertySetter "RST" v [|"RST"|]
    /// Default Load 1, 최대 128 사용
    member __.Load  
        with set(v) = intPropertySetter "LOAD" v 


    //member x.Id = x.SendCommandGetResponse "*IDN?\r\n"
//    member x.Id =
//        let command = 
//            let details = [| "*RST";
//                             ":TRIGger EXTernal";
//                             ":FREQuency 200E+03";
//                             ":*TRG";
//                             ":MEASure?";
//                             "" |]
//            String.Join(newline, details)
//
//        x.SendCommandGetResponse command

    new () = new LCRMeterManager(lcrIp, lcrPort)




/// <summary>
/// Exception safe Hioki manager singleton
/// If exception occurs on paix manager, new instance is automatically created.
/// </summary>
let mutable private hiokiManangerSingleton: LCRMeterManager option = None

/// <summary>
/// Hioki LCR meter manager 객체에 대한 interface 를 생성한다.
/// </summary>
/// <param name="ip">기기의 ip</param>
/// <param name="port">기기의 port</param>
let rec createManager ip port =
    try
        let hioki = new LCRMeterManager(ip, port)
        hioki.ProcException <- fun exn -> createManager ip port |> ignore
        hiokiManangerSingleton <- Some(hioki)

        logInfo "-- HIOKI with ID=%s connected." hioki.Id
        logInfo "\tMode = %s" hioki.Mode
        logInfo "\tFrequency = %g" hioki.Frequency
        logInfo "\tLevel = %s" hioki.Level
        logInfo "\tSpeed = %s" hioki.Speed
        logInfo "\tTrigger = %s" hioki.Trigger
        logInfo "\tTriggerDelay = %A" hioki.TriggerDelay
        logInfo "\tWave = %A" hioki.Wave
        logInfo "\tDCBias = %A" hioki.DCBias
        logInfo "\tFormat = %A" hioki.Format
        logInfo "\tMeasureItem = %A" hioki.MeasureItem
        logInfo "\tDisplay1 = %A" hioki.Display1
        logInfo "\tDisplay2 = %A" hioki.Display2
        logInfo "\tDisplay3 = %A" hioki.Display3
        logInfo "\tDisplay4 = %A" hioki.Display4
        logInfo "\tMEASure = %A" hioki.MEASure
    with exn ->
        DriverExceptionSubject.OnNext(HiokiException(exn))
        raise exn

/// Hioki LCR meter manager 객체에 대한 interface 를 생성한다.
let createManagerSimple() = createManager lcrIp lcrPort

/// <summary>
/// Returns exception safe singleton instance
/// paix manager 에서 exception 이 발생하는 경우, 새로운 객체를 생성하도록 하여 반환한다.
/// </summary>
let manager() =
    hiokiManangerSingleton.Value
