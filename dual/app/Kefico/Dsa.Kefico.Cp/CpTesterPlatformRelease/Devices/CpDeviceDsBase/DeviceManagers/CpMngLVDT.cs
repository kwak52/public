using System;
using System.Collections.Generic;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using static CpBase.CpLog4netLogging;
using CpTesterPlatform.CpDevices;

namespace CpTesterPlatform.CpMngLib.Manager
{
    /// <summary>
    /// Example to represent how to build a device manager.
    /// A Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
    /// The named dll for a device is bound in run-time environment as well.    
    /// Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
    /// </summary>
    public class CpMngLVDT : CpDeviceManagerDsBase, ILVDTManager
    {
        public string COM_PORT { set; get; }

        public ILVDT LVDT => DeviceInstance as ILVDT;
        public ClsLVDTInfo infoMotion { private set; get; }

        public CpMngLVDT(bool activeHw) : base(activeHw)
        {
        }


        protected override IDevice CreateDeviceInstance(string dllHint)
        {
            LogInfo($"+ Creating device instance from {dllHint}.");
            if (dllHint == "CpDevLVDT_Sony")
                return new CpDevLVDT_Sony();

            throw new NotImplementedException($"No DeviceInstance creator for {dllHint}");
        }

        public override bool CreateDevice(ClsDeviceInfoBase info)
        {
            if (base.CreateDevice(info))
            {
                infoMotion = DeviceInfo as ClsLVDTInfo;
                FuncEvtHndl = new CpFunctionEventHandler();
                LVDT.COM_PORT = DeviceInfo.HwName;

                return true;
            }

            return false;
        }

        public override bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
        {
            throw new NotImplementedException();
        }


        public double GetFuntionDimension()
        {
            return LVDT.GetFuntionDimension();
        }

        public void SetMasterDimension()
        {
            LVDT.SetMasterDimension();
        }
    }
}
