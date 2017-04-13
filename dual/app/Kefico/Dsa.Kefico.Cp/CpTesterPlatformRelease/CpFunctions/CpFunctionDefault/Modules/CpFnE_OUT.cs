using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using PsCommon;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Manager;
using System.Threading.Tasks;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System.Windows.Forms;
using CpBase;

namespace CpTesterPlatform.Functions
{
    public class CpFnE_OUT : CpTsShellWithCotrolBlock, IE_OUT
    {
        private CpTsShell cpStep = null;
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            ResultLog.TsActionResult = TsResult.NONE;
            cpStep = cpTsParent;

            TakeDelay();

            foreach (var wire in Core.GetWiringForControlBlock())
            {
                CpDeviceManagerBase devManager = CpUtil.GetManagerControlDevice(cpMngSystem, iMngStation, wire.PinAlias);
                if (devManager == null) continue;

                if (!CpUtil.CheckEnableDevice(cpMngSystem, devManager.DeviceInfo.DeviceType)) continue;

                switch (devManager.DeviceInfo.DeviceType)
                {
                    #region Unused device
                    //case CpDeviceType.RELAYMATRIX: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.SWITCHBLOCK: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.SCOPE: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.DMM: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.RESISTOR: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.SMU: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.COUNTER: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.COUNTER_INPUT: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.FGN: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.RELAY: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.LOAD: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.KAM: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.LVDT: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.LASERMARKER: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    //case CpDeviceType.TRIGGER_IO: ResultLog.TsActionResult = ControlExcute(devManager as CpDeviceManagerBase, wire); break;
                    #endregion
                    case CpDeviceType.POWER_SUPPLY: ResultLog.TsActionResult = ControlPowerSupply(devManager as CpMngPowerSupply, wire); break;
                    case CpDeviceType.LCRMETER: ResultLog.TsActionResult = ControlLcrMeter(devManager as CpMngLCRMeter, wire); break;
                    case CpDeviceType.ANALOG_INPUT: ResultLog.TsActionResult = ControlAnalogInput(devManager as CpMngAIControl, wire); break;
                    case CpDeviceType.MOTION: ResultLog.TsActionResult = ControlMotion(devManager as CpMngMotion, wire).Result; break;
                    case CpDeviceType.DIGITAL_IO: ResultLog.TsActionResult = ControlDigitalIO(devManager as CpMngDIOControl, iMngStation, wire); break;
                    //case CpDeviceType.PLC: ResultLog.TsActionResult = ControlPLC(devManager as IPLCManager, wire); break;
                    default:
                        continue;
                }
            }

            //kwak, ahn : todo check => 실제 다음과 같은 Assert fail 경우가 발생한다.
            //Debug.Assert(ResultLog.TsActionResult != TsResult.ERROR);
            return TsResult.OK;
        }

        private void TakeDelay()
        {
            PsCCSStdFnModuleE_OUT modEOut = this.Core as PsCCSStdFnModuleE_OUT;
            List<PsCCSStdWiring> vWiringInRange = modEOut.GetWiringForControlBlockInRange();

            if (!vWiringInRange.Exists(x => x.PinAlias == "WZ"))
                return;

            string strWZValue = vWiringInRange.Find(x => x.PinAlias == "WZ").PinPositive;

            if (strWZValue == "-")
                return;

            int nWZValue = Convert.ToInt32(strWZValue);

            if (nWZValue > 0)
                Thread.Sleep(nWZValue);
        }
        private TsResult ControlPLC(IPLCManager devManager, PsCCSStdWiring wiring)
        {
            throw new NotImplementedException();
        }

        private TsResult ControlDigitalIO(CpMngDIOControl devManager, IStnManager iMngStation, PsCCSStdWiring wire)
        {
            var tResult = TryFunc(() =>
            {
                Debug.Assert(devManager != null);
                ClsTsCtrBlockWithAdapter CtrBlockWithAdapter = iMngStation.MngTStep.MngControlBlock.DicAnsteuerWithCtrBlock[wire.PinAlias] as ClsTsCtrBlockWithAdapter;
                if (CtrBlockWithAdapter == null) return TsResult.NONE;

                CpAdtCnf AdtCnf = iMngStation.MngTStep.MngControlBlock.LstLoadedAdapterCnf.Where(pin => pin.AdtBpAddress.RawString == CtrBlockWithAdapter.SgPinNum).FirstOrDefault();
                if (AdtCnf == null) return TsResult.NONE;
                if(devManager.DeviceInfo.Device_ID != AdtCnf.AdtMslUnit.RawString)
                    return TsResult.NONE;

                if (wire.PinPositive == "-" || wire.PinPositive.ToUpper() == "OFF")
                    devManager.SetDOutState(Convert.ToInt32(AdtCnf.AdtBpAddress.RawString.Replace("DI", "").Replace("DO", "")), false);
                else if (wire.PinPositive.ToUpper() == "ON")
                    devManager.SetDOutState(Convert.ToInt32(AdtCnf.AdtBpAddress.RawString.Replace("DI", "").Replace("DO", "")), true);
                else
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to execute test step for wiring in step "
                        + cpStep?.StepNum + " " + wire.PinPositive, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                    return TsResult.ERROR;
                }

                return TsResult.OK;
            });

            return tResult.Succeeded ? tResult.Result : TsResult.ERROR;
        }

        private async Task<TsResult> ControlMotion(CpMngMotion devManager, PsCCSStdWiring wire)
        {
            try
            {
                if (wire.PinAlias == "MOTOR_DIR")
                {
                    devManager.SetDirection(wire.PinPositive != "CCW");
                }
                else if (wire.PinAlias == "MOTOR_RPM")
                {
                    if (wire.PinPositive == "-" || wire.PinPositive.ToUpper() == "0")
                    {
                        devManager.StopMotion();
                    }
                    else
                    {
                        devManager.SetParametor(Convert.ToDouble(wire.PinPositive), Convert.ToDouble(wire.PinPositive), Convert.ToDouble(wire.PinPositive));
                        devManager.SetRpmSpeed(Convert.ToDouble(wire.PinPositive), true);
                        devManager.SetRelMove(10000);
                    }
                }
                else if (wire.PinAlias == "ROBOT" && wire.PinPositive != "-")
                {
                    if (!await devManager.MovePath(wire.PinPositive, CpDsFormAppCommon.CancellationToken))
                        return TsResult.ERROR;
                    else
                    {
                        var pin = wire.PinPositive.ToUpper();
                        if (pin.Contains("READYTOOUTPUT")|| pin == "7DCT_READYTOODDEVEN")
                            devManager.SetRelMove(2, CpUtilRobot.GetAirGapPulseUsingSingleAxis(CpUtil.AirGapOffSet));
                        else if (pin == "READYTOINPUT" || pin == "READYTOMIDDLE")
                        {
                            devManager.SetRelMove(2, CpUtilRobot.GetAirGapPulse(CpUtil.AirGapOffSet, true));
                            devManager.SetRelMove(1, CpUtilRobot.GetAirGapPulse(CpUtil.AirGapOffSet, false));
                        }
                        else if (pin == "PRIMARUPOSSET" || pin == "OUTPUTPOSSET" || pin == "TURBINEPOSSET")
                        {
                            devManager.SetRelMove(2, CpUtilRobot.GetAirGapPulseUsingIAI(CpUtil.AirGapOffSet));
                        }
                    }
                }

                if (CpDsFormAppCommon.CancellationToken.IsCancellationRequested)
                    return TsResult.ERROR;

                return TsResult.OK;
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                    return TsResult.ERROR;

                throw;
            }
        }

        private TsResult ControlAnalogInput(CpMngAIControl devManager, PsCCSStdWiring wire)
        {
            throw new NotImplementedException();
        }

        private TsResult ControlLcrMeter(CpMngLCRMeter devManager, PsCCSStdWiring wire)
        {
            throw new NotImplementedException();
        }

        private TsResult ControlPowerSupply(CpMngPowerSupply devManager, PsCCSStdWiring wire)
        {
            var tResult = TryFunc(() =>
            {
                if (wire.PinPositive == "-")
                {
                    devManager.SetCurrent(0);
                    devManager.SetVoltage(0);
                    devManager.SetOutput(false);
                    return TsResult.OK;
                }

                double dVolt = Convert.ToDouble(wire.PinPositive);
                switch (wire.ValueType)
                {
                    case CpSpecDimension.V:
                        devManager.SetOutput(true);
                        devManager.SetCurrent(3);
                        devManager.SetVoltage(dVolt);
                        break;
                    case CpSpecDimension.MV:
                        dVolt = dVolt / ClsDefineGlobalConstantcs.CONV_MILLI_VALUE;
                        devManager.SetVoltage(dVolt);
                        break;
                    default:
                        UtilTextMessageEdits.UtilTextMsgToConsole("Failed to execute test step for wiring in step " + cpStep?.StepNum + " Value type is none", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                        return TsResult.ERROR;
                }

                return TsResult.OK;
            });

            return tResult.Succeeded ? tResult.Result : TsResult.ERROR;
        }
    }
}
