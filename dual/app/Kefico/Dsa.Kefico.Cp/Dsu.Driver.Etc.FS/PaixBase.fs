// T:\PDF-Books\시험기\PAIX\Sample\C#\IO\IO\Form1.cs
// T:\PDF-Books\시험기\PAIX\Doc
//  1. NMC2-Library-Manual(V2.0.3).pdf
//      - Error code : pp.18.  0 = NMC_OK
// NMC2.h (C/C++ header for doxygen comment)
// T:\PDF-Books\시험기\PAIX\Sample\TotalDemo(VC2010)\NMC2_TotalDemo.sln 참고 할 것.
namespace Dsu.Driver.Paix

open System
open Dsu.Driver.PaixMotionControler
open Dsu.Driver


/// 구현용 PAIX motion controller 관리자. 직접 PaixManagerBase 객체 생성하지 말고 createManager 호출을 통해 PaixManager 객체를 생성할 것.
type PaixManagerBase(ip:string, autoOpen) =
    // PAIX device number : 192.168.0.XXX 에서 XXX 에 해당함.
    let mutable deviceNumber = 0s

    // Gantry 등에서 사용되는 group.  뭘 의미하는지 불명확...
    let maxGroup = 2

    let timeout = tcpSocketConnectionTimeoutMilli

    let allAxes = [|0s..7s|]

    // string ip address 를 short 의 array 로 변환한 것.  array length = 4
    let ipComponents =
        let (succ, ipaddr) = Net.IPAddress.TryParse ip
        ipaddr.GetAddressBytes()
            |> Array.map (fun b -> (int16)b )

    let ping deviceNumber waitTimeMilli =
        NMC2.PingCheck(deviceNumber, defaultArg waitTimeMilli timeout) = NMC2.NMC_OK


    let openPaix(retryPolicy:Polly.Retry.RetryPolicy) =
        let iptuples = (deviceNumber, ipComponents.[0], ipComponents.[1], ipComponents.[2])

        NMC2.SetIPAddress iptuples |> ignore

        try
            retryPolicy.Execute(fun () ->
                if not (ping deviceNumber None) then
                    failwithlog (sprintf "Paix(%s) not responding.  Check connection." ip)
                    
//                if NMC2.OpenDevice(deviceNumber) <> 0s then         // NMC2.nmc_OpenDeviceEx 를 사용할 경우, timeout 적용 가능
//                    failwithlog "Paix failed to open."

                // OpenDeviceEx() 호출시 다음 exception 오류 발생 : AnyCPU paltform 으로 선택한 후, prefer win32 를 선택한 경우에 발생함.
                // Managed Debugging Assistant 'PInvokeStackImbalance' has detected a problem in 'W:\solutions\trunk\app\Kefico\Dsa.Kefico.Cp\\bin\DriverTestConsoleApp.FS.exe'.
                // Additional information: PInvoke 함수 'Dsu.Driver.PaixWrapper!Paix.MotionControler.NMC2::OpenDeviceEx'에 대한 호출 결과 스택이 불안정하게 되었습니다. 
                //      관리되는 PInvoke 시그니처와 관리되지 않는 대상 시그니처가 일치하지 않기 때문인 것 같습니다. 
                //      호출 규칙 및 PInvoke 시그니처의 매개 변수와 관리되지 않는 대상 시그니처가 일치하는지 확인하십시오.
                let code = NMC2.OpenDeviceEx(deviceNumber, int64(timeout))
                if code <> 0s then         // NMC2.OpenDeviceEx 를 사용할 경우, timeout 적용 가능
                    // soft fail : Retry 수행
                    failwithlog (sprintf "Paix(%s) failed to open.  code=%d" ip code))

            NMC2.AllAxisStop(deviceNumber, 0s) |> ignore      // 0s for decelerated stop
        with exn ->
            // hard fail : 최종 retry 에서도 fail 한 경우
            failwithlog (sprintf "PAIX(%s) failed to open with excetion:%O." ip exn)

    /// paix 연결 불안정성으로 인해서, 연결이 끊긴 경우, 재시도 후 해당 기능을 다시 수행한다.
    let retryWhenDisconnected f : int16 =
        // to simulated disconnected status, use NMC2.CloseDevice(deviceNumber)
        let code = f()
        if code = -1s then
            logError "Paix retrying connection.."
            openPaix(urgentRetryPolicy)     // try upmost 3 times.  interally, paix tries 3 times, so total 9 times retrial
            f()
        else
            code
    
    do
        deviceNumber <- ipComponents.[3]
        Dsu.Driver.PaixWrapper.PlatformSelector.SelectPlatform()
        logInfo "Paix manager created on IP %s" ip
        if autoOpen then
            openPaix(moderateRetryPolicy)

    /// Tells whether origin position is set after program launch.  원점 설정 여부.  IsOriginCalibratable 일 때에만 True 로 설정가능
    static member val IsOriginCalibrated = false with get, set
    /// End effector 가 원점 근처에 왔는지의 여부.  원점 설정 여부와는 다름
    static member val IsOriginCalibratable = false with get, set

    /// Stops all axes [0..7]
    member x.EmergencyStop() = allAxes |> Seq.iter (fun ax -> x.SuddenStop(ax) |> ignore)

    member val DeviceType = DeviceType.DeviceType_None with get, set
    /// Number of axes
    member val MotionType = -1s with get, set
    member val DioType = DioType.Dio_None with get, set
    member val ExDioType = ExDioType.ExDio_None with get, set
    member val MDioType = MDioType.MDio_None with get, set
    member x.OpenPaix() =
        openPaix(moderateRetryPolicy)
        logInfo "\tPaix(%s) opened." ip

        match x.GetDeviceInfo() with
        | Some(motionType, dioType:int16, exDio:int16, mDio:int16) ->
            x.MotionType <- motionType
            x.DioType <- LanguagePrimitives.EnumOfValue (int dioType)
            x.ExDioType <- LanguagePrimitives.EnumOfValue (int exDio)
            x.MDioType <- LanguagePrimitives.EnumOfValue (int mDio)
        | None -> failwithlog "Paix failed to GetDeviceInfo()"

        match x.GetDeviceType() with
        | Some(v) -> x.DeviceType <- LanguagePrimitives.EnumOfValue (int v)
        | None -> failwithlog "Paix failed to GetDeviceType()"

    member __.Ping ?waitTimeMilli =
        NMC2.PingCheck(deviceNumber, defaultArg waitTimeMilli timeout) = NMC2.NMC_OK

    member __.Close() = NMC2.CloseDevice(deviceNumber)








    member __.OpenDevice() = NMC2.OpenDevice(deviceNumber)
    member __.OpenDeviceEx lWaitTime = NMC2.OpenDeviceEx(deviceNumber, lWaitTime)
    /// 단축 로직 정보 읽기
    member __.GetParaLogic axisNo =
        match NMC2.GetParaLogic(deviceNumber, axisNo) with
        | (NMC2.NMC_OK, logic) -> Some(logic)
        | _ -> None
    /// 단축 확장 로직 정보 읽기
    member __.GetParaLogicEx axisNo =
        match NMC2.GetParaLogicEx(deviceNumber, axisNo) with
        | (NMC2.NMC_OK, logic) -> Some(logic)
        | _ -> None

    /// axisNo 축의 현재 속도 정보를 읽어 온다.  0 이면 성공, 다른 경우 실패
    member __.GetParaSpeed axisNo =
        match NMC2.GetParaSpeed(deviceNumber, axisNo) with
        | (NMC2.NMC_OK, speed) -> Some(speed)
        | _ -> None
    member __.GetRingCountMode axisNo = 
        match NMC2.GetRingCountMode(deviceNumber, axisNo) with
        | (NMC2.NMC_OK, maxPulse, maxEncoder, ringMode) -> Some(maxPulse, maxEncoder, ringMode)
        | _ -> None
    member __.GetParaTargetPos axisNo =
        match NMC2.GetParaTargetPos(deviceNumber, axisNo) with
        | (NMC2.NMC_OK, targetPos) -> Some(targetPos)
        | _ -> None
    /// 현재 출력 속도
    member __.GetDriveAxesSpeed() =
        let drvSpeed: float array = Array.create 8 0.0
        match NMC2.GetDriveAxesSpeed(deviceNumber, drvSpeed) with
        | NMC2.NMC_OK -> Some(drvSpeed)
        | _ -> None

    /// 모션 전용 신호 정보
    member __.GetAxesMotionOut() = 
        match NMC2.GetAxesMotionOut(deviceNumber) with
        | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
        | _ -> None

    /// 단축 Emergency 로직 설정
    member __.SetEmgLogic groupNo logic  = NMC2.SetEmgLogic(deviceNumber, groupNo, logic)
    /// 단축 하드웨어 +Limit 로직 설정
    member __.SetPlusLimitLogic axisNo logic = NMC2.SetPlusLimitLogic(deviceNumber, axisNo, logic)
    /// 단축 하드웨어 -Limit 로직 설정
    member __.SetMinusLimitLogic axisNo logic = NMC2.SetMinusLimitLogic(deviceNumber, axisNo, logic)
    /// 단축 소프트웨어 Limit 설정
    member __.SetSWLimitLogic axisNo nUse swMinusPos dwPlusPos  = NMC2.SetSWLimitLogic(deviceNumber, axisNo, nUse, swMinusPos, dwPlusPos)
    /// 단축 확장 소프트웨어 Limit 설정 
    member __.SetSWLimitLogicEx axisNo nUse swMinusPos swPlusPos opt  = NMC2.SetSWLimitLogicEx(deviceNumber, axisNo, nUse, swMinusPos, swPlusPos, opt)
    /// 단축 알람 로직 설정
    member __.SetAlarmLogic axisNo logic = NMC2.SetAlarmLogic(deviceNumber, axisNo, logic)
    /// 단축 Near 입력 로직 설정
    member __.SetNearLogic axisNo logic = NMC2.SetNearLogic(deviceNumber, axisNo, logic)
    /// 단축 위치 결정 완료 입력 로직 설정
    member __.SetInPoLogic axisNo logic = NMC2.SetInPoLogic(deviceNumber, axisNo, logic)
    /// 단축 서보 Ready 입력 로직 설정
    member __.SetSReadyLogic axisNo logic = NMC2.SetSReadyLogic(deviceNumber, axisNo, logic)
    /// 단축 엔코더 체배 설정
    member __.SetEncoderCount axisNo countMode = NMC2.SetEncoderCount(deviceNumber, axisNo, countMode)
    /// 단축 엔코더 카운트 방향 설정
    member __.SetEncoderDir axisNo countMode = NMC2.SetEncoderDir(deviceNumber, axisNo, countMode)
    member __.SetEncoderMode axisNo mode = NMC2.SetEncoderMode(deviceNumber, axisNo, mode)
    /// 단축 엔코더 Z상 로직 설정
    member __.SetEncoderZLogic axisNo logic = NMC2.SetEncoderZLogic(deviceNumber, axisNo, logic)
    /// 원점 완료상태 해제 사용 여부 확인
    member __.GetHomeDoneAutoCancel axisNo alarm servoOff currentOff servoReady =
        match NMC2.GetHomeDoneAutoCancel(deviceNumber, axisNo) with
        | (NMC2.NMC_OK, alarm, servoOff, currentOff, servoReady) -> Some(alarm, servoOff, currentOff, servoReady)
        | _ -> None

    /// 원점 완료상태 해제 사용 여부 설정
    member __.SetHomeDoneAutoCancel axisNo alarm servoOff currentOff servoReady = NMC2.SetHomeDoneAutoCancel(deviceNumber, axisNo, alarm, servoOff, currentOff, servoReady)

    //????? GetParaLogic 과 SetParaLogic 의 parameter 가 동일하다???? Set 에서 out paramenter 의 의미는????
//    member __.SetParaLogic axisNo out NMCPARALOGIC pLogic);           // 단축 로직 일괄 설정
//    member __.SetParaLogicEx axisNo out NMCPARALOGICEX pLogicEx);     // 단축 확장 로직 일괄 설정

    /// 로직 설정파일로 로직설정
    member __.SetParaLogicFile strs = NMC2.SetParaLogicFile(deviceNumber, strs)
    member __.SetPulseMode axisNo mode = NMC2.SetPulseMode(deviceNumber, axisNo, mode)
    /// 단축 펄스 출력 모드 설정 
    member __.SetPulseLogic axisNo logic = NMC2.SetPulseLogic(deviceNumber, axisNo, logic)
    member __.Set2PulseDir axisNo dir = NMC2.Set2PulseDir(deviceNumber, axisNo, dir)
    member __.Set1PulseDir axisNo dir = NMC2.Set1PulseDir(deviceNumber, axisNo, dir)
    member __.SetPulseActive axisNo pulseActive = NMC2.SetPulseActive(deviceNumber, axisNo, pulseActive)
    member __.SetSCurveSpeed axisNo startSpeed acc dec driveSpeed = NMC2.SetSCurveSpeed(deviceNumber, axisNo, startSpeed, acc, dec, driveSpeed)
    /// <summary>
    /// 이동을 수행하기 전 속도를 반드시 지정하여야 합니다. 속도 단위: 이동단위/초
    /// 사다리꼴 속도 프로파일의 형태로 이동 속도를 설정합니다.
    /// </summary>
    /// <param name="axisNo">축번호</param>
    /// <param name="startSpeed">시작속도, 출하값=10</param>
    /// <param name="acc">가속속도, 출하값=1000</param>
    /// <param name="dec">감속속도, 출하값=1000</param>
    /// <param name="driveSpeed">구동속도, 출하값=1000</param>
    member __.SetSpeed axisNo startSpeed acc dec driveSpeed = NMC2.SetSpeed(deviceNumber, axisNo, startSpeed, acc, dec, driveSpeed)
    member __.SetOverrideRunSpeed axisNo acc dec driveSpeed  = NMC2.SetOverrideRunSpeed(deviceNumber, axisNo, acc, dec, driveSpeed)
    member __.SetOverrideDriveSpeed axisNo driveSpeed  = NMC2.SetOverrideDriveSpeed(deviceNumber, axisNo, driveSpeed)
    member __.SetAccSpeed axisNo acc = NMC2.SetAccSpeed(deviceNumber, axisNo, acc)
    member __.SetDecSpeed axisNo dec = NMC2.SetDecSpeed(deviceNumber, axisNo, dec)
    member __.AbsMove axisNo pos = NMC2.AbsMove(deviceNumber, axisNo, pos)
    member __.RelMove axisNo amount  = NMC2.RelMove(deviceNumber, axisNo, amount)
    member __.VelMove axisNo dpos drive mode = NMC2.VelMove(deviceNumber, axisNo, dpos, drive, mode)
    member __.AbsOver axisNo pos = NMC2.AbsOver(deviceNumber, axisNo, pos)
    member __.VarRelMove axisCount axisNos amounts = NMC2.VarRelMove(deviceNumber, axisCount, axisNos, amounts)
    member __.VarAbsMove axisCount axisNos posList = NMC2.VarAbsMove(deviceNumber, axisCount, axisNos, posList)
    member __.VarAbsOver axisCount axisNos posList = NMC2.VarAbsOver(deviceNumber, axisCount, axisNos, posList)
    member __.JogMove axis dnir  = NMC2.JogMove(deviceNumber, axis, dnir)
    member __.SuddenStop axisNo  = NMC2.SuddenStop(deviceNumber, axisNo)
    member __.DecStop axisNo = NMC2.DecStop(deviceNumber, axisNo)
    /// mode: 0=decelearted stop, 1=emergency stop
    member __.AllAxisStop mode  = NMC2.AllAxisStop(deviceNumber, mode)
    member __.MultiAxisStop count axisSelects mode = NMC2.MultiAxisStop(deviceNumber, count, axisSelects, mode) 
    /// 축 표시 읽기
    member __.GetAxesExpress() =
        match NMC2.GetAxesExpress(deviceNumber) with
        | (NMC2.NMC_OK, nmcData) -> Some(nmcData)
        | _ -> None

    /// 정지 원인 확인. pp. 189.  {0=normal, 1=emergency, 2=MinusLimit, 3=PlusLimit, 4=AlarmOn, 5=NearOn, 6=EncoderZ}
    member __.GetStopInfo() =
        let stopModes: int16 array = Array.create 8 0s
        match NMC2.GetStopInfo(deviceNumber, stopModes) with
        | NMC2.NMC_OK -> Some(stopModes)
        | _ -> None

    member private x.checkAllAxesStopInfo(axes:seq<int16>, f:int16->bool) =
        match x.GetStopInfo() with
        | Some(stopModes) ->
            axes |> Seq.forall f    //(fun ax -> stopModes.[int ax] = reason )
        | None -> false

    member private x.collectAxesWithStopInfo(axes:seq<int16>, f:int16->bool) =
        match x.GetStopInfo() with
        | Some(stopModes) ->
            axes |> Seq.filter f
        | None -> Seq.empty
        

    member x.IsNormalStopped(axes:seq<int16>) = x.checkAllAxesStopInfo(axes, (fun n -> n = 0s))
    member x.IsEncorderZStopped(axes:seq<int16>) = x.checkAllAxesStopInfo(axes, (fun n -> n = 6s))    // 6 : Encoder Z stop
    member x.IsNoAlaram(axes:seq<int16>) = x.checkAllAxesStopInfo(axes, (fun n -> n <> 4s))           // 4 : AlarmOn
    member x.AlaramedAxes(axes:seq<int16>) = x.collectAxesWithStopInfo(axes, (fun n -> n = 4s))       // 4 : AlarmOn

    /// 현재의 지령값(command position)을 0으로 재설정한다.  실제 device 가 움직이지는 않는다.
    member __.SetCmdPos axisNo pos = NMC2.SetCmdPos(deviceNumber, axisNo, pos)
    /// 현재의 실제값(encoder position)을 0으로 재설정한다.  실제 device 가 움직이지는 않는다.
    member __.SetEncPos axisNo pos = NMC2.SetEncPos(deviceNumber, axisNo, pos)
    member __.SetCmdEncPos axisNo pos = NMC2.SetCmdEncPos(deviceNumber, axisNo, pos)

    (*
     * Homing
     *)

    member __.HomeMove axisNo homeMode homeEndMode offset reserve = NMC2.HomeMove(deviceNumber, axisNo, homeMode, homeEndMode, offset, reserve)
    member __.GetHomeStatus() =
        match NMC2.GetHomeStatus(deviceNumber) with
        | (NMC2.NMC_OK, homeFlag) -> Some(homeFlag)
        | _ -> None
    member __.SetHomeSpeed axisNo homeSpeed0 homeSpeed1 homeSpeed2 = NMC2.SetHomeSpeed(deviceNumber, axisNo, homeSpeed0, homeSpeed1, homeSpeed2)
    member __.SetHomeSpeedEx axisNo homeSpeed0 homeSpeed1 homeSpeed2 offsetSpeed = NMC2.SetHomeSpeedEx(deviceNumber, axisNo, homeSpeed0, homeSpeed1, homeSpeed2, offsetSpeed)
    member __.SetHomeSpeedAccDec(axisNo, homeSpeed0, start0, acc0, dec0, homeSpeed1, start1, acc1, dec1, homeSpeed2, start2, acc2, dec2, offsetSpeed, offsetStart, offsetAcc, offsetDec) =
        NMC2.SetHomeSpeedAccDec(deviceNumber, axisNo, homeSpeed0, start0, acc0, dec0, homeSpeed1, start1, acc1, dec1, homeSpeed2, start2, acc2, dec2, offsetSpeed, offsetStart, offsetAcc, offsetDec)

    member __.HomeDoneCancel axisNo = NMC2.HomeDoneCancel(deviceNumber, axisNo)
    member __.SetHomeDelay homeDelay = NMC2.SetHomeDelay(deviceNumber, homeDelay)


    member __.Interpolation2Axis axisNo0 pos0 axisNo1 pos1 opt = NMC2.Interpolation2Axis(deviceNumber, axisNo0, pos0, axisNo1, pos1, opt)
    member __.Interpolation3Axis axisNo0 pos0 axisNo1 pos1 axisNo2 pos2 opt = NMC2.Interpolation3Axis(deviceNumber, axisNo0, pos0, axisNo1, pos1, axisNo2, pos2, opt)
    member __.InterpolationArc axisNo0 axisNo1 center0 center1 angle opt = NMC2.InterpolationArc(deviceNumber, axisNo0, axisNo1, center0, center1, angle, opt)
    member __.InterpolationRelCircle axisNo0 centerPulse0 endPulse0 axisNo1 centerPulse1 endPulse1 dir = NMC2.InterpolationRelCircle(deviceNumber, axisNo0, centerPulse0, endPulse0, axisNo1, centerPulse1, endPulse1, dir)
    member __.InterpolationAbsCircle axisNo0 centerPulse0 endPulse0 axisNo1 centerPulse1 endPulse1 dir = NMC2.InterpolationAbsCircle(deviceNumber, axisNo0, centerPulse0, endPulse0, axisNo1, centerPulse1, endPulse1, dir)
    /// 단축 모터 전류 신호 출력 
    member __.SetCurrentOn axisNo outValue = NMC2.SetCurrentOn(deviceNumber, axisNo, outValue)
    member __.SetServoOn axisNo outValue = NMC2.SetServoOn(deviceNumber, axisNo, outValue)
    member __.SetAlarmResetOn axisNo outValue = NMC2.SetAlarmResetOn(deviceNumber, axisNo, outValue)
    member __.SetDccOn axisNo outValue = NMC2.SetDccOn(deviceNumber, axisNo, outValue)
    /// 다축 모터 전류 신호 출력
    member __.SetMultiCurrentOn count axisSelects out = NMC2.SetMultiCurrentOn(deviceNumber, count, axisSelects, out)
    /// 다축 서보 온 신호 출력
    member __.SetMultiServoOn count axisSelects out = NMC2.SetMultiServoOn(deviceNumber, count, axisSelects, out)
    member __.SetMultiAlarmOn count axisSelects out = NMC2.SetMultiAlarmOn(deviceNumber, count, axisSelects, out)
    /// 다축 편차 카운터 클리어 신호 출력
    member __.SetMultiDccOn count axisSelects out = NMC2.SetMultiDccOn(deviceNumber, count, axisSelects, out)

    member __.SetEnableNear axisNo mode = NMC2.SetEnableNear(deviceNumber, axisNo, mode)
    member __.SetEnableEncZ axisNo mode = NMC2.SetEnableEncZ(deviceNumber, axisNo, mode)
    /// 단축 Limit On 시 정지 모드 설정
    member __.SetLimitStopMode axisNo stopMode  = NMC2.SetLimitStopMode(deviceNumber, axisNo, stopMode)
    /// 단축 Busy Off 시점 설정
    member __.SetBusyOffMode axisNo mode = NMC2.SetBusyOffMode(deviceNumber, axisNo, mode) 
    member __.SetRingCountMode axisNo maxPulse maxEncoder ringMode  = NMC2.SetRingCountMode(deviceNumber, axisNo, maxPulse, maxEncoder, ringMode)
    member __.MoveRing axisNo pos moveMode = NMC2.MoveRing(deviceNumber, axisNo, pos, moveMode) 
    member __.SetSyncSetup mainAxisNo subAxisNoEnable0 subAxisNoEnable1 subAxisNoEnable2 = NMC2.SetSyncSetup(deviceNumber, mainAxisNo, subAxisNoEnable0, subAxisNoEnable1, subAxisNoEnable2)  
    member __.SetSync groupNo syncGrpList0 syncGrpList1 = NMC2.SetSync(deviceNumber, groupNo, syncGrpList0, syncGrpList1)
    member __.SyncFree groupNo = NMC2.SyncFree(deviceNumber, groupNo)
    member __.SetMDIOOutPin pinNo outStatus = NMC2.SetMDIOOutPin(deviceNumber, pinNo, outStatus)
    member __.SetMDIOOutTogPin pinNo = NMC2.SetMDIOOutTogPin(deviceNumber, pinNo)
    member __.SetMDIOOutPins count pinNos statuses = NMC2.SetMDIOOutPins(deviceNumber, count, pinNos, statuses)
    member __.SetMDIOOutTogPins count pinNos = NMC2.SetMDIOOutTogPins(deviceNumber, count, pinNos)
    member __.GetMDIOInPin pinNo inStatus =
        match NMC2.GetMDIOInPin(deviceNumber, pinNo) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None
    member __.GetMDIOInput inStatuses = NMC2.GetMDIOInput(deviceNumber, inStatuses)
    member __.GetMDIOInputBit bitNo =
        match NMC2.GetMDIOInputBit(deviceNumber, bitNo) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None
    member __.GetMDIOOutput outStatus = NMC2.GetMDIOOutput(deviceNumber, outStatus)
    member __.SetMDIOOutput outStatus = NMC2.SetMDIOOutput(deviceNumber, outStatus)
    member __.SetMDIOOutputBit bitNo outStatus = NMC2.SetMDIOOutputBit(deviceNumber, bitNo, outStatus)
    member __.SetMDIOOutputTog bitNo = NMC2.SetMDIOOutputTog(deviceNumber, bitNo)
    member __.SetMDIOOutputAll onBitNo offBitNo = NMC2.SetMDIOOutputAll(deviceNumber, onBitNo, offBitNo) 
    member __.SetMDIOOutputTogAll bitNo = NMC2.SetMDIOOutputTogAll(deviceNumber, bitNo)
    member __.GetMDIOInput32() =
        match NMC2.GetMDIOInput32(deviceNumber) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None
    member __.GetMDIOOutput32() =
        match NMC2.GetMDIOInput32(deviceNumber) with
        | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
        | _ -> None
    member __.SetMDIOOutput32 outStatus = NMC2.SetMDIOOutput32(deviceNumber, outStatus)
    member __.SetDIOOutPin pinNo outStatus =
        retryWhenDisconnected ( fun () -> NMC2.SetDIOOutPin(deviceNumber, pinNo, outStatus))

    member __.SetDIOOutTogPin pinNo = NMC2.SetDIOOutTogPin(deviceNumber, pinNo)
    member __.SetDIOOutPins count pinNos statuses = NMC2.SetDIOOutPins(deviceNumber, count, pinNos, statuses)
    member __.SetDIOOutTogPins count pinNos = NMC2.SetDIOOutTogPins(deviceNumber, count, pinNos)
    member __.GetDIOInPin pinNo =
        match NMC2.GetDIOInPin(deviceNumber, pinNo) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None

    member __.GetDIOInput inStatus =
        let f() = NMC2.GetDIOInput(deviceNumber, inStatus)
        retryWhenDisconnected f

    member __.GetDIOInput128 inStatus =
        retryWhenDisconnected (fun () -> NMC2.GetDIOInput128(deviceNumber, inStatus))

    member __.GetDIOInputBit bitNo =
        match NMC2.GetDIOInputBit(deviceNumber, bitNo) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None

    member __.GetDIOOutput outStatus =
        let f() = NMC2.GetDIOOutput(deviceNumber, outStatus)
        retryWhenDisconnected f

    member __.GetDIOOutput128 outStatus =
        let f() = NMC2.GetDIOOutput128(deviceNumber, outStatus)
        retryWhenDisconnected f

    member __.SetDIOOutput outStatus = 
        let f() = NMC2.SetDIOOutput(deviceNumber, outStatus)
        retryWhenDisconnected f

    member __.SetDIOOutput128 outStatus =
        let f() = NMC2.SetDIOOutput128(deviceNumber, outStatus)
        retryWhenDisconnected f

    member __.SetDIOOutputBit bitno outStatus = NMC2.SetDIOOutputBit(deviceNumber, bitno, outStatus)
    member __.SetDIOOutputTog bitNo = NMC2.SetDIOOutputTog(deviceNumber, bitNo)
    member __.SetDIOOutputAll onBitNo offBitNos = NMC2.SetDIOOutputAll(deviceNumber, onBitNo, offBitNos)
    member __.SetDIOOutputTogAll bitNo = NMC2.SetDIOOutputTogAll(deviceNumber, bitNo)
    member __.GetDIOInput64() =
        match NMC2.GetDIOInput64(deviceNumber) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None

    member __.GetDIOOutput64() =
        match NMC2.GetDIOInput64(deviceNumber) with
        | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
        | _ -> None
    member __.SetDIOOutput64 outStatus = NMC2.SetDIOOutput64(deviceNumber, outStatus)
    member __.GetDIOInput32 index =
        match NMC2.GetDIOInput32(deviceNumber, index) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None
    member __.GetDIOOutput32 index =
        match NMC2.GetDIOOutput32(deviceNumber, index) with
        | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
        | _ -> None
    member __.SetDIOOutput32 index outStatus = NMC2.SetDIOOutput32(deviceNumber, index, outStatus)
    member __.SetEXDIOOutPin pinNo = 
        match NMC2.SetEXDIOOutPin(deviceNumber, pinNo) with
        | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
        | _ -> 
            match NMC2.SetEXDIOOutPin(deviceNumber, pinNo) with
            | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
            | _ -> 
                match NMC2.SetEXDIOOutPin(deviceNumber, pinNo) with
                | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
                | _ -> 
                    match NMC2.SetEXDIOOutPin(deviceNumber, pinNo) with
                    | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
                    | _ -> 
                        match NMC2.SetEXDIOOutPin(deviceNumber, pinNo) with
                        | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
                        | _ -> None
        

    member __.SetEXDIOOutTogPin pinNo = NMC2.SetEXDIOOutTogPin(deviceNumber, pinNo)
    member __.SetEXDIOOutPins count pinNos statuses = NMC2.SetEXDIOOutPins(deviceNumber, count, pinNos, statuses)
    member __.SetEXDIOOutTogPins count pinNos = NMC2.SetEXDIOOutTogPins(deviceNumber, count, pinNos)
    member __.GetEXDIOInPin pinNo = 
        match NMC2.GetEXDIOInPin(deviceNumber, pinNo) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None

    member __.GetEXDIOInput inStatus = NMC2.GetEXDIOInput(deviceNumber, inStatus)
    member __.GetEXDIOInputBit bitNo =
        match NMC2.GetEXDIOInputBit(deviceNumber, bitNo) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None
    member __.GetEXDIOOutput outStatus = NMC2.GetEXDIOOutput(deviceNumber, outStatus)
    member __.SetEXDIOOutput outStatus = NMC2.SetEXDIOOutput(deviceNumber, outStatus)
    member __.SetEXDIOOutputBit bitNo outStatus = NMC2.SetEXDIOOutputBit(deviceNumber, bitNo, outStatus)
    member __.SetEXDIOOutputTog bitNo = NMC2.SetEXDIOOutputTog(deviceNumber, bitNo)
    member __.SetEXDIOOutputAll onBitNos offBitNos = NMC2.SetEXDIOOutputAll(deviceNumber, onBitNos, offBitNos)
    member __.SetEXDIOOutputTogAll bitNos = NMC2.SetEXDIOOutputTogAll(deviceNumber, bitNos) 
    member __.GetEXDIOInput32() =
        match NMC2.GetEXDIOInput32(deviceNumber) with
        | (NMC2.NMC_OK, inStatus) -> Some(inStatus)
        | _ -> None
    member __.GetEXDIOOutput32() =
        match NMC2.GetEXDIOOutput32(deviceNumber) with
        | (NMC2.NMC_OK, outStatus) -> Some(outStatus)
        | _ -> None
    member __.SetEXDIOOutput32 outStatus = NMC2.SetEXDIOOutput32(deviceNumber, outStatus)
    member __.SetOutLimitTimePin ioType pinNo on time  = NMC2.SetOutLimitTimePin(deviceNumber, ioType, pinNo, on, time)
    member __.GetOutLimitTimePin ioType pinNo =
        match NMC2.GetOutLimitTimePin(deviceNumber, ioType, pinNo) with
        | (NMC2.NMC_OK, set, status, outStatus, remainTime) -> Some(set, status, outStatus, remainTime)
        | _ -> None

    [<Obsolete("Do not use. F# implemetation of GetFirmVersion() is dangerous.")>]
    member __.GetFirmVersion() =
        match NMC2.GetFirmVersion(deviceNumber) with
        | (NMC2.NMC_OK, v) -> Some(v)
        | _ -> None
    member __.SetUnitPerPulse axisNo ratio  = NMC2.SetUnitPerPulse(deviceNumber, axisNo, ratio)
    member __.GetUnitPerPulse axisNo = NMC2.GetUnitPerPulse(deviceNumber, axisNo)
    member __.SetProtocolMethod nMethod  = NMC2.SetProtocolMethod(deviceNumber, nMethod)
    member __.GetProtocolMethod() = NMC2.GetProtocolMethod(deviceNumber)
    member __.GetIPAddress() =
        match NMC2.GetIPAddress() with
        | (NMC2.NMC_OK, field0, field1, field2, field3) -> Some(field0, field1, field2, field3)
        | _ -> None

    member __.SetIPAddress field0 field1 field2 = NMC2.SetIPAddress(deviceNumber, field0, field1, field2)
    member __.WriteIPAddress ips subNet gateway = NMC2.WriteIPAddress(deviceNumber, ips, subNet, gateway)
    member __.SetDefaultIPAddress()  = NMC2.SetDefaultIPAddress(deviceNumber)
    member __.GetDeviceType() =
        match NMC2.GetDeviceType(deviceNumber) with
        | (NMC2.NMC_OK, deviceType) -> Some(deviceType)
        | _ -> None
    member __.GetDeviceInfo() =
        match NMC2.GetDeviceInfo(deviceNumber) with
        | (NMC2.NMC_OK, motionType, dioType, eXDio, mDio) -> Some(motionType, dioType, eXDio, mDio)
        | _ -> None

    member __.GetEnumList() =
       match NMC2.GetEnumList(ipComponents) with
        | (0, nmcList) -> Some(nmcList)
        | _ -> None
    member __.GetDIOInfo() =
        match NMC2.GetDIOInfo(deviceNumber) with
        | (0, inCount, outCount) -> Some(inCount, outCount)
        | _ ->
            match NMC2.GetDIOInfo(deviceNumber) with
            | (0, inCount, outCount) -> Some(inCount, outCount)
            | _ ->
                match NMC2.GetDIOInfo(deviceNumber) with
                | (0, inCount, outCount) -> Some(inCount, outCount)
                | _ ->
                    match NMC2.GetDIOInfo(deviceNumber) with
                    | (0, inCount, outCount) -> Some(inCount, outCount)
                    | _ ->
                        match NMC2.GetDIOInfo(deviceNumber) with
                        | (0, inCount, outCount) -> Some(inCount, outCount)
                        | _ -> None

    member __.DIOTest mode delay = NMC2.DIOTest(deviceNumber, mode, delay)
    /// ROM에 저장
    member __.MotCfgSaveToROM mode = NMC2.MotCfgSaveToROM(deviceNumber, mode)
    /// ROM 초기화
    member __.MotCfgSetDefaultROM mode = NMC2.MotCfgSetDefaultROM(deviceNumber, mode)
    /// ROM 읽기
    member __.MotCfgLoadFromROM mode = NMC2.MotCfgLoadFromROM(deviceNumber, mode)
    [<Obsolete("사용 안함")>]
    member __.AccOffsetCount axisNo pulse = NMC2.AccOffsetCount(deviceNumber, axisNo, pulse)
    member __.PingCheck waitTime = NMC2.PingCheck(deviceNumber, waitTime)
    member __.GetBusyStatus axisNo =
        match NMC2.GetBusyStatus(deviceNumber, axisNo) with
        | (NMC2.NMC_OK, busyStatus) -> Some(busyStatus)
        | _ -> None

    member __.GetBusyStatusAll busyStatus = NMC2.GetBusyStatusAll(deviceNumber, busyStatus)
    member __.SetTriggerCfg axis cmpMode cmpAmount dioOnTime pinNo dioType reserve = NMC2.SetTriggerCfg(deviceNumber, axis, cmpMode, cmpAmount, dioOnTime, pinNo, dioType, reserve)
    member __.SetTriggerEnable axis enable= NMC2.SetTriggerEnable(deviceNumber, axis, enable)
    member __.GetMapIO mapInStatus = NMC2.GetMapIO(deviceNumber, mapInStatus)
    member __.MapMove axis pos mapIndex opt = NMC2.MapMove(deviceNumber, axis, pos, mapIndex, opt)
    member __.MapMoveEx axis pos mapIndex opt posType = NMC2.MapMoveEx(deviceNumber, axis, pos, mapIndex, opt, posType)
    member __.GetMapData mapIndex =
        match NMC2.GetMapData(deviceNumber, mapIndex) with
        | (NMC2.NMC_OK, nmcMapData) -> Some(nmcMapData)
        | _ -> None

    member __.GetMapDataEx mapIndex dataIndex =
        match NMC2.GetMapDataEx(deviceNumber, mapIndex, dataIndex) with
        | (NMC2.NMC_OK, nmcMapData) -> Some(nmcMapData)
        | _ -> None

    member x.GetAxesCmdSpeed() =
        match x.GetAxesCmdEncSpeed() with
        | Some(cmdSpeed, _) -> Some(cmdSpeed)
        | _ -> None
    member x.GetAxesEncSpeed () = 
        match x.GetAxesCmdEncSpeed() with
        | Some(_, encSpeed) -> Some(encSpeed)
        | _ -> None
    /// 위치 지령 펄스 속도 및 엔코더 입력 속도
    member __.GetAxesCmdEncSpeed() =
        let cmdSpeed: float array = Array.create 8 0.0
        let encSpeed: float array = Array.create 8 0.0
        match NMC2.GetAxesCmdEncSpeed(deviceNumber, cmdSpeed, encSpeed) with
        | NMC2.NMC_OK -> Some(cmdSpeed, encSpeed)
        | _ -> None

    member __.SetGantryAxis groupNo main sub = NMC2.SetGantryAxis(deviceNumber, groupNo, main, sub)
    member __.SetGantryEnable groupNo gantryEnable = NMC2.SetGantryEnable(deviceNumber, groupNo, gantryEnable)
    member __.GetGantryInfo() =
        let enable: int16 array = Array.create maxGroup 0s
        let mainAxes: int16 array = Array.create maxGroup 0s
        let subAxes: int16 array = Array.create maxGroup 0s
        match NMC2.GetGantryInfo(deviceNumber, enable, mainAxes, subAxes) with
        | NMC2.NMC_OK -> Some(enable, mainAxes, subAxes)
        | _ -> None
    member __.ContRun groupNo runMode = NMC2.ContRun(deviceNumber, groupNo, runMode)
    member __.GetContStatus() =
        match NMC2.GetContStatus(deviceNumber) with
        | (NMC2.NMC_OK, contStatus) -> Some(contStatus)
        | _ -> None

    member __.SetContNodeLine groupNo nodeNo axisNo0 axisNo1 pos0 pos1 start acc dec driveSpeed =
        NMC2.SetContNodeLine(deviceNumber, groupNo, nodeNo, axisNo0, axisNo1, pos0, pos1, start, acc, dec, driveSpeed)
    member __.SetContNodeLineIO groupNo nodeNo axisNo0 axisNo1 pos0 pos1 start acc dec driveSpeed onOff =
        NMC2.SetContNodeLineIO(deviceNumber, groupNo, nodeNo, axisNo0, axisNo1, pos0, pos1, start, acc, dec, driveSpeed, onOff)
    member __.SetContNode3Line groupNo nodeNo axisNo0 axisNo1 axisNo2 pos0 pos1 pos2 start acc dec driveSpeed =
        NMC2.SetContNode3Line(deviceNumber, groupNo, nodeNo, axisNo0, axisNo1, axisNo2, pos0, pos1, pos2, start, acc, dec, driveSpeed)
    member __.SetContNode3LineIO  groupNo nodeNo axisNo0 axisNo1 axisNo2 pos0 pos1 pos2 start acc dec driveSpeed onOff =
        NMC2.SetContNode3LineIO(deviceNumber, groupNo, nodeNo, axisNo0, axisNo1, axisNo2, pos0, pos1, pos2, start, acc, dec, driveSpeed, onOff)
    member __.SetContNodeArc groupNo nodeNo axisNo0 axisNo1 center0 center1 center2 start acc dec driveSpeed =
        NMC2.SetContNodeArc(deviceNumber, groupNo, nodeNo, axisNo0, axisNo1, center0, center1, center2, start, acc, dec, driveSpeed)
    member __.SetContNodeArcIO groupNo nodeNo axisNo0 axisNo1 center0 center1 center2 start acc dec driveSpeed onOff =
        NMC2.SetContNodeArcIO(deviceNumber, groupNo, nodeNo, axisNo0, axisNo1, center0, center1, center2, start, acc, dec, driveSpeed, onOff)
    member __.ContNodeClear groupNo = NMC2.ContNodeClear(deviceNumber, groupNo)
    member __.ContSetIO groupNo ioType ioPinNo endNodeOnOff = NMC2.ContSetIO(deviceNumber, groupNo, ioType, ioPinNo, endNodeOnOff)
    member __.GetCmdPos axis = 
        match NMC2.GetCmdPos(deviceNumber, axis) with
        | (NMC2.NMC_OK, cmdPos) -> Some(cmdPos)
        | _ -> None

    member __.GetEncPos axis =
        match NMC2.GetEncPos(deviceNumber, axis) with
        | (NMC2.NMC_OK, encPos) -> Some(encPos)
        | _ -> None
    member __.SetDisconectedStopMode timeInterval stopMode = NMC2.SetDisconectedStopMode(deviceNumber, timeInterval, stopMode)
    member __.SetDisconnectedStopMode timeInterval stopMode = NMC2.SetDisconnectedStopMode(deviceNumber, timeInterval, stopMode)
    /// Emergency 사용 여부 설정
    member __.SetEmgEnable enable = NMC2.SetEmgEnable(deviceNumber, enable)
    member __.SetSerialConfig baud data stop parity = NMC2.SetSerialConfig(deviceNumber, baud, data, stop, parity)
    member __.SetSerialMode mode = NMC2.SetSerialMode(deviceNumber, mode)
    member __.SerialWrite len str = NMC2.SerialWrite(deviceNumber, len, str)

    // ????
    member __.SerialRead() =
        let readLen = ref 0s
        let readStr : char array = Array.zeroCreate 1024
        match NMC2.SerialRead(deviceNumber, readLen, readStr) with
        | NMC2.NMC_OK -> Some(readLen, readStr)
        | _ -> None

    member __.SetMpgMode axisNo mode pulse = NMC2.SetMpgMode(deviceNumber, axisNo, mode, pulse)
    member __.ContiSetNodeClear groupNo = NMC2.ContiSetNodeClear(deviceNumber, groupNo)
    member __.ContiSetMode groupNo nAVTRIMode emptyMode n1stAxis n2ndAxis n3rdAxis dMaxDrvSpeed nIoType nIoCtrlPinMask nIoCtrlEndVal = 
            NMC2.ContiSetMode(deviceNumber, groupNo, nAVTRIMode, emptyMode, n1stAxis, n2ndAxis, n3rdAxis, dMaxDrvSpeed, nIoType, nIoCtrlPinMask, nIoCtrlEndVal)
    member __.ContiGetStatus() =
        match NMC2.ContiGetStatus(deviceNumber) with
        | (NMC2.NMC_OK, contiStatus) -> Some(contiStatus)
        | _ -> None
    member __.ContiAddNodeLine2Axis groupNo pos0 pos1 start acc dec drvSpeed ioCtrlVal =
            NMC2.ContiAddNodeLine2Axis(deviceNumber, groupNo, pos0, pos1, start, acc, dec, drvSpeed, ioCtrlVal)
    member __.ContiAddNodeLine3Axis groupNo pos0 pos1 pos2 start acc dec drvSpeed ioCtrlVal =
            NMC2.ContiAddNodeLine3Axis(deviceNumber, groupNo, pos0, pos1, pos2, start, acc, dec, drvSpeed, ioCtrlVal)
    member __.ContiAddNodeArc groupNo center0 center1 angle start acc dec drvSpeed ioCtrlVal =
            NMC2.ContiAddNodeArc(deviceNumber, groupNo, center0, center1, angle, start, acc, dec, drvSpeed, ioCtrlVal)
    member __.ContiSetCloseNode groupNo = NMC2.ContiSetCloseNode(deviceNumber, groupNo)
    member __.ContiRunStop groupNo runMode = NMC2.ContiRunStop(deviceNumber, groupNo, runMode)
    member __.AVTRISetMode axis nAVTRIMode = NMC2.AVTRISetMode(deviceNumber, axis, nAVTRIMode)
    member __.AVTRIGetMode axis =
        match NMC2.AVTRIGetMode(deviceNumber, axis) with
        | (NMC2.NMC_OK, nAVTRIMode) -> Some(nAVTRIMode)
        | _ -> None

    member __.SetWaitTime lWaitTime = NMC2.SetWaitTime(deviceNumber, lWaitTime)



    interface IDisposable with
        member x.Dispose() = x.Close()



