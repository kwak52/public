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
    public class CpMngTriggerIO : CpDeviceManagerDsBase, INetworkTcpIpManager
    {
        public CpMngTriggerIO(bool activeHw) : base(activeHw)
        {
        }

        public IUPS ups => DeviceInstance as IUPS;
        public ClsTriggerIOInfo infoUPS { private set; get; }

        public List<string> ReceivedMessageList { get; set; }

        public override bool CloseDevice()
        {
            return ups.DevClose();
        }

        protected override IDevice CreateDeviceInstance(string dllHint)
        {
            LogInfo($"+ Creating device instance from {dllHint}.");
            if (dllHint == "CpDevUPS_APC")
                return new CpDevUPS_APC();

            throw new NotImplementedException($"No DeviceInstance creator for {dllHint}");
        }

        


        public override bool CreateDevice(ClsDeviceInfoBase info)
        {
            if ( base.CreateDevice(info) )
            {
                infoUPS = DeviceInfo as ClsTriggerIOInfo;
                FuncEvtHndl = new CpFunctionEventHandler();
                ups.IP_ADDRESS = infoUPS.IP_ADDRESS;

                return true;
            }

            return false;
        }


        public override bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
        {
            throw new NotImplementedException();
        }


        public override bool OpenDevice()
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
