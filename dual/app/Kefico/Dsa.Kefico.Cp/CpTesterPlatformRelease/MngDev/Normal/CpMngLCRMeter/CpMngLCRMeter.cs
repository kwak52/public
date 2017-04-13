using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using System;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using CpTesterPlatform.CpTStepDev.Interface;

namespace CpTesterPlatform.CpMngLib.Manager
{
    public class CpMngLCRMeter : CpDeviceManagerDsBase, ILCRMeterManager
    {
        public ILCRMeter LCRMeter { private set; get; }
        public ClsLVDTInfo infoMotion { private set; get; }

        public CpMngLCRMeter(bool activeHw) : base(activeHw)
        {
        }

        public override bool CreateDevice(ClsDeviceInfoBase info)
        {
            var oResult = TryFunc(() =>
            {
                if (!CreateInstanceFromDll(info.DllName))
                    return false;

                DeviceInfo = info;
                LCRMeter = (ILCRMeter)DeviceInstance;
                infoMotion = DeviceInfo as ClsLVDTInfo;
                FuncEvtHndl = new CpFunctionEventHandler();
                LCRMeter.CONTROLLER_ADDRESS = DeviceInfo.HwName;

                return true;
            });

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create CpMngLCRMeter.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            return oResult.Result;
        }

        public override bool InitManager()
        {
            if (LCRMeter == null || infoMotion == null)
                return false;
            else
                return true;
        }

        public bool OpenDevice()
        {
            IsOpened = LCRMeter.DevOpen();
            IsClosed = !IsOpened;
            return IsOpened;
        }

        public bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
        {
            throw new NotImplementedException();
        }

        public bool SetSettingFile(int loadID)
        {
            return LCRMeter.SetSettingFile(loadID);
        }

        public int GetSettingFile()
        {
            return LCRMeter.GetSettingFile();
        }

        public double GetInductance() { return LCRMeter.GetInductance(); }
        public double GetCapicatance() { return LCRMeter.GetCapicatance(); }
        public double GetResistance() { return LCRMeter.GetResistance(); }
    }
}
