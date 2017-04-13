using System;
using CpTesterPlatform.CpTStepDev.Interface;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface INetworkTcpIp : IDevice
    {	
        bool DevInit();
        bool InitTcpClient(string deviceId, string deviceIp, int portNumber, bool bCR = false, bool bLF = false);
        bool SendData(byte[] writeBuffer);
		bool SendData(string writeData);
    }
}
