using System;
using System.Collections.Generic;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpMngLib.Manager
{
    /// <summary>
    /// Example to represent how to build a device manager.
    /// A Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
    /// The named dll for a device is bound in run-time environment as well.    
    /// Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
    /// </summary>
    public class CpMngLVDT : CpDeviceManagerBase, ILVDTManager
    {
        public DeviceControlState ControlState { get; } = DeviceControlState.Normal;

        public bool IsCreated { get; }
        public bool IsOpened { get; set; }
        public bool IsClosed { get; set; }
        public string COM_PORT { set; get; }

        public ILVDT LVDT { private set; get; }
        public ClsLVDTInfo infoMotion { private set; get; }

        public CpMngLVDT(bool activeHw) : base(activeHw)
        {
        }

        public bool InitManager()
        {
            return true; //test ahn
            //throw new NotImplementedException();
        }

        public bool CreateDevice(ClsDeviceInfoBase info)
        {
            var oResult = TryFunc(() =>
            {
                if (!CreateInstanceFromDll(info.DllName))
                    return false;

                DeviceInfo = info;
                LVDT = (ILVDT)DeviceInstance;
                infoMotion = DeviceInfo as ClsLVDTInfo;
                FuncEvtHndl = new CpFunctionEventHandler();
                LVDT.COM_PORT = DeviceInfo.HwName;

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

        public bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
        {
            throw new NotImplementedException();
        }

        public bool OpenDevice()
        {
            var oResult = TryFunc(() =>
            {

                LVDT = (ILVDT)DeviceInstance;
                ClsLVDTInfo infoLVDT = DeviceInfo as ClsLVDTInfo;

                if (LVDT == null || infoLVDT == null)
                    return false;

                IsOpened = LVDT.DevOpen();
                IsClosed = !IsOpened;
                return IsOpened;

            });

            if (oResult.HasException || oResult.Result == false)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Open a LVDT Control", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return false;
            }

            return oResult.Result;
        }

        public double GetFuntionDimension()
        {
            return LVDT.GetFuntionDimension();
        }

        public void SetMasterDimension()
        {
            LVDT.SetMasterDimension();
        }

        public bool CloseDevice()
        {
            if (IsClosed) return true;
            IsClosed = LVDT.DevClose();
            IsOpened = !IsClosed;
            return IsClosed;
        }

        public bool ResetDevice()
        {
            if (!IsOpened) return false;
            return LVDT.DevReset();
        }
    }
}
