using System;
using System.Collections.Generic;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using CpTesterPlatform.CpCommon;
using System.Threading.Tasks;
using System.Threading;
using static CpCommon.ExceptionHandler;
using System.Collections.Concurrent;
using CpTesterPlatform.CpLogUtil;
using System.Globalization;

namespace CpTesterPlatform.CpMngLib.Manager
{
    public delegate void EventPLCHandler(string address, int value);

    /// <summary>
    /// Example to represent how to build a device manager.
    /// A Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
    /// The named dll for a device is bound in run-time environment as well.    
    /// Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
    /// </summary>
    public class CpMngPlc : CpDeviceManagerBase, IPLCManager
    {
        public CpMngPlc(bool activeHw) : base(activeHw)
        {
        }

        public bool IsCreated { private set; get; } = false;
        public bool IsOpened { private set; get; } = false;
        public bool IsClosed { private set; get; } = false;

        public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;
        public IPLC PLC { private set; get; }
        public string DeviceID { set; get; }
        public Dictionary<string, int> DicMonitor { set; get; } = new Dictionary<string, int>();
        public event EventPLCHandler EventPLC;
        public bool InitManager()
        {
            return true;
        }

        public bool CreateDevice(ClsDeviceInfoBase info)
        {
            var oResult = TryFunc(() =>
            {
                if (!CreateInstanceFromDll(info.DllName))
                    return false;

                DeviceInfo = info;
                FuncEvtHndl = new CpFunctionEventHandler();

              
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
            return true;
        }

        public bool OpenDevice()
        {
            PLC = (IPLC)DeviceInstance;
            ClsPLCInfo PLCInfo = DeviceInfo as ClsPLCInfo;

            if (PLC == null || PLCInfo == null)
                return false;

            FuncEvtHndl = new CpFunctionEventHandler();
            FuncEvtHndl.OnPLCReceive += FuncEvtHndl_OnPLCReceive;

            PLC.FuncEvtHndl = FuncEvtHndl;
            PLC.DeviceID = DeviceInfo.Device_ID;
            PLC.PLC_IP_ADDR = DeviceInfo.HwName;
            PLC.CPU_TYPE_STR = PLCInfo.CPU_TYPE_STR;
            PLC.PLC_IP_ADDR = PLCInfo.PLC_IP_ADDR;
            PLC.PLC_PORT_NUMBER = PLCInfo.PLC_PORT_NUMBER;
            PLC.CONNECTION_TYPE_STR = PLCInfo.CONNECTION_TYPE_STR;
            PLC.READPORT = PLCInfo.READPORT;
            PLC.TIMEOUT = PLCInfo.TIMEOUT;

            IsOpened = PLC.DevOpen();
            IsClosed = !IsOpened;

            if (IsOpened == false)
                DeviceInstance = null;

            return IsOpened;
        }



        private void FuncEvtHndl_OnPLCReceive(string data)
        {
            if(data.Contains(";"))
            {
                string address = data.Split(';')[0];
                int value = Convert.ToInt32(data.Split(';')[1]);
                if (DicMonitor.ContainsKey(address))
                    DicMonitor[address] = value;

                EventPLC?.Invoke(address, value);
            }
        }

        public bool CloseDevice()
        {
            if (IsClosed) return true;
            if (PLC == null) return true;
          
            IsClosed = PLC.DevClose();
            IsOpened = !IsClosed;
            return IsClosed;
        }

        public bool AddDevices(string deviceNames)
        {
            if (!IsOpened) return false;
            if (DicMonitor.ContainsKey(deviceNames))
                return false;

            DicMonitor.Add(deviceNames, -1);
            return PLC.AddDevices(deviceNames);
        }

        public bool SingleScanStart()
        {
            if (!IsOpened) return false;

            return PLC.SingleScanStart();
        }

        public int ReadDevice(string deviceName)
        {
            if (!IsOpened) return -1;
            return PLC.ReadDevice(deviceName);
        }

        public bool WriteDevice(string deviceName, int value)
        {
            if (!IsOpened) return false;
            return PLC.WriteDevice(deviceName, value);
        }

        public bool WriteBitDevice(string deviceName, int[] value)
        {
            if (!IsOpened) return false;
            return PLC.WriteBitDevice(deviceName, value);
        }

        public int ReadMonitor(string deviceName)
        {
            if (DicMonitor.ContainsKey(deviceName))
                return DicMonitor[deviceName];

            return -1;
        }

        public bool ResetDevice()
        {
            if (!IsOpened) return false;
            return PLC.DevReset();
        }

        public int[] ReadPLC(string strDevice, int nStartAddr, int nEndAddr)
        {
            throw new NotImplementedException();
        }

        public bool WritePLC(string strDevice, int nStartAddr, int nEndAddr, int[] anData)
        {
            throw new NotImplementedException();
        }
    }
}
