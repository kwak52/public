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
    public class CpMngTriggerIO : CpDeviceManagerBase, INetworkTcpIpManager
    {
        public CpMngTriggerIO(bool activeHw) : base(activeHw)
        {
        }

        public bool IsCreated { private set; get; } = false;
        public bool IsOpened { private set; get; } = false;
        public bool IsClosed { private set; get; } = false;

        public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;
        public IUPS ups { private set; get; }
        public ClsTriggerIOInfo infoUPS { private set; get; }

        public List<string> ReceivedMessageList { get; set; }

        public bool CloseDevice()
        {
            return ups.DevClose();
        }

        public bool CreateDevice(ClsDeviceInfoBase info)
        {
            var oResult = TryFunc(() =>
            {
                if (!CreateInstanceFromDll(info.DllName))
                    return false;

                DeviceInfo = info;
                ups = (IUPS)DeviceInstance;
                infoUPS = DeviceInfo as ClsTriggerIOInfo;
                FuncEvtHndl = new CpFunctionEventHandler();
                ups.IP_ADDRESS = infoUPS.IP_ADDRESS;

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

        public bool InitManager()
        {
            return true;
        }

        public bool OpenDevice()
        {
            var oResult = TryFunc(() =>
            {
                IsOpened = ups.DevOpen();
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

        public bool ResetDevice()
        {
            return ups.DevReset();
        }

        public void SendData(string writeData)
        {
            throw new NotImplementedException();
        }

        public string SendQueryData(string writeData)
        {
            throw new NotImplementedException();
        }

        public double GetTemperature()
        {
            if (!IsOpened)
                return 0.0;


            return ups.GetTemperature();
        }

        public double GetHumidity()
        {
            if (!IsOpened)
                return 0.0;


            return ups.GetHumidity();
        }
    }
}
