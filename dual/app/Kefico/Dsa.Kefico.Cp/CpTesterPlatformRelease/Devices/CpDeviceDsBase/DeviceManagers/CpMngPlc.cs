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
using static CpBase.CpLog4netLogging;
using System.Collections.Concurrent;
using CpTesterPlatform.CpLogUtil;
using System.Globalization;
using CpTesterPlatform.CpDevices;

namespace CpTesterPlatform.CpMngLib.Manager
{
    public delegate void EventPLCHandler(string address, int value);

    /// <summary>
    /// Example to represent how to build a device manager.
    /// A Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
    /// The named dll for a device is bound in run-time environment as well.    
    /// Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
    /// </summary>
    public class CpMngPlc : CpDeviceManagerDsBase, IPLCManager
    {
        public CpMngPlc(bool activeHw) : base(activeHw)
        {
        }

        public IPLC PLC => DeviceInstance as IPLC;
        private CpDevPLC_Mitsubishi MitsubishiPLC => DeviceInstance as CpDevPLC_Mitsubishi;
        public string DeviceID { set; get; }
        public Dictionary<string, int> DicMonitor { set; get; } = new Dictionary<string, int>();
        public event EventPLCHandler EventPLC;


        protected override IDevice CreateDeviceInstance(string dllHint)
        {
            LogInfo($"+ Creating device instance from {dllHint}.");
            switch (dllHint)
            {
                case "CpDevPLC_Mitsubishi":
                    return new CpDevPLC_Mitsubishi();
                default:
                    throw new NotImplementedException($"Unknown device type{dllHint}.");
            }
        }
        public override bool CreateDevice(ClsDeviceInfoBase info)
        {
            if (base.CreateDevice(info))
            {
                FuncEvtHndl = new CpFunctionEventHandler();
                return true;
            }

            return false;
        }

        public override bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
        {
            return true;
        }

        public override bool OpenDevice()
        {
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



        public bool AddDevices(string deviceNames)
        {
            if (!IsOpened) return false;
            if (DicMonitor.ContainsKey(deviceNames))
                return false;

            DicMonitor.Add(deviceNames, -1);
            return PLC.AddDevices(deviceNames);
        }

        public async Task SingleScanStartAsync()
        {
            if (!IsOpened) return;

            await MitsubishiPLC.SingleScanStartAsync();
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
