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
    public class CpMngAIControl : CpDeviceManagerDsBase, IAnalogInputManager
    {
        public CpMngAIControl(bool activeHw) : base(activeHw)
        {
        }


        public override bool OpenDevice()
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