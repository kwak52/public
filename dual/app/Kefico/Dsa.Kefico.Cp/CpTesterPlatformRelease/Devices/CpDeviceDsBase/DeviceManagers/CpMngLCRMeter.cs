using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using System;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using static CpBase.CpLog4netLogging;
using CpTesterPlatform.CpTStepDev.Interface;
using CpTesterPlatform.CpDevices;

namespace CpTesterPlatform.CpMngLib.Manager
{
    public class CpMngLCRMeter : CpDeviceManagerDsBase, ILCRMeterManager
    {
        public ILCRMeter LCRMeter { private set; get; }
        public ClsLVDTInfo infoMotion { private set; get; }

        public CpMngLCRMeter(bool activeHw) : base(activeHw)
        {
        }

        protected override IDevice CreateDeviceInstance(string dllHint)
        {
            LogInfo($"+ Creating device instance from {dllHint}.");
            if (dllHint == "CpDevLCR_Hioki")
                return new CpDevLCR_Hioki();

            throw new NotImplementedException($"No DeviceInstance creator for {dllHint}");
        }


        public override bool CreateDevice(ClsDeviceInfoBase info)
        {
            if (base.CreateDevice(info))
            {
                LCRMeter = (ILCRMeter)DeviceInstance;
                infoMotion = DeviceInfo as ClsLVDTInfo;
                FuncEvtHndl = new CpFunctionEventHandler();
                LCRMeter.CONTROLLER_ADDRESS = DeviceInfo.HwName;

                return true;
            }

            return false;
        }




        public override bool InitManager()
        {
            if (LCRMeter == null || infoMotion == null)
                return false;
            else
                return true;
        }

        public override bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
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
