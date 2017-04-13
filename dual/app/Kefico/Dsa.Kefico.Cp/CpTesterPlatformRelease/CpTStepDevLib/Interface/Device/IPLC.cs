using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpTesterPlatform.CpMngLib.Interface;

namespace CpTesterPlatform.CpTStepDev.Interface
{

    public interface IPLC : IDevice
    {
        string CPU_TYPE_STR { set; get; } //Example: "Q03UDECPU";        
        string PLC_IP_ADDR { set; get; }
        int PLC_PORT_NUMBER { set; get; }
        string CONNECTION_TYPE_STR { set; get; }
        bool READPORT { set; get; }
        int TIMEOUT { set; get; }

        bool AddDevices(string deviceNames);
        bool WriteDevice(string strDeviceWrite, int value);
        bool WriteBitDevice(string strDeviceWrite, int[] value);
        int ReadDevice(string strDeviceRead);
    }
}
