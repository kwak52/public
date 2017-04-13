using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpMngLib.Manager
{
    public class CpMngAIControl : CpDeviceManagerBase, IAnalogInputManager
    {
        public CpMngAIControl(bool activeHw) : base(activeHw)
        {
        }

        public bool IsCreated { private set; get; } = false;
        public bool IsOpened { private set; get; } = false;
        public bool IsClosed { private set; get; } = false;
        public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;

        public bool CloseDevice()
        {
            if (IsClosed) return true;
            IAnalogInput devAIControl = (IAnalogInput) DeviceInstance;

            if (devAIControl == null)
                return true;

            return devAIControl.DevClose();
        }

        public bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
        {
            var oResult = TryFunc(() =>
            {
                if (info.VirtualDevice)
                {
                    IsCreated = CreateInstanceFromDll(info.DllName);

                    if (!IsCreated)
                        return IsCreated;

                    DeviceInfo = info;
                    IsCreated = true;
                    IsOpened = true;

                    ((CpVitualDeviceManagerBase)DeviceInstance).SetCommDeviceMgr(devComm);
              
                    return true;
                }

                return false;
            });

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create CpMngPowerSupply.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            return oResult.Result;
        }

        public bool CreateDevice(ClsDeviceInfoBase info)
        {
            var oResult = TryFunc(() =>
            {
                if (!CreateInstanceFromDll(info.DllName))
                    return false;

                DeviceInfo = info;

                return true;
            });

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create CpMngPowerSupply.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            return oResult.Result;
        }

        public bool InitManager()
        {
            return true;
        }

        public bool OpenDevice()
        {
            IAnalogInput devAIControl = (IAnalogInput)DeviceInstance;
            ClsAnalogInputInfo infoAI = DeviceInfo as ClsAnalogInputInfo;

            if (devAIControl == null || infoAI == null)
                return false;

            FuncEvtHndl = new CpFunctionEventHandler();

            devAIControl.FuncEvtHndl = FuncEvtHndl;
            devAIControl.DeviceID = DeviceInfo.Device_ID;

            devAIControl.ChannelID = infoAI.HwName;
            devAIControl.AICoupling = infoAI.AICoupling;
            devAIControl.LowpassCutoffFrequency = infoAI.LowpassCutoffFrequency;
            devAIControl.LowpassEnable = infoAI.LowpassEnable;
            devAIControl.Max = infoAI.Max;
            devAIControl.Min = infoAI.Min;
            devAIControl.TerminalConfiguration = infoAI.TerminalConfiguration;

            IsOpened = devAIControl.DevOpen();

            return IsOpened;
        }

        public bool ResetDevice()
        {
            if (!IsOpened) return false;
            IAnalogInput devAIControl = (IAnalogInput)DeviceInstance;

            return devAIControl.DevReset();
        }
        
        public double GetInstantV()
        {
            throw new NotImplementedException();
        }

        public async Task<double []> GetPeriodicV(double dPeriodSec, double dSamplingRate)
        {
            return await ((IAnalogInput)DeviceInstance).GetPeriodicV(dPeriodSec, dSamplingRate);
        }

        public bool ReturnBuffer(double[] vresult)
        {
            return ((IAnalogInput)DeviceInstance).ReturnBuffer(vresult);
        }

        public void StartDataCollecting(int nPeriodMs, double dSamplingRate)
        {
            ((IAnalogInput)DeviceInstance).StartDataCollecting(nPeriodMs, dSamplingRate);
        }

        public List<double> EndDataCollecting()
        {
            return ((IAnalogInput)DeviceInstance).EndDataCollecting();
        }

        public double GetFrequency()
        {
            throw new NotImplementedException();
        }

        public List<double> GetPeriodicV(int nPeriodMs)
        {
            throw new NotImplementedException();
        }
    }
}