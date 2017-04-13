using System;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public class UdpData : EventArgs
    {
        public byte[] lstByteData { get; set; }
    }
    public interface INetworkUdp : IDevice, IComm
    {
        event EventHandler<UdpData> ReceivedUDPData;
        bool DevOpen(int serverPort);
        bool DevInit();
        bool AddUDPClient(int deviceId, string deviceIp, int portNumber);
        bool WriteData(int senderId, byte[] writeBuffer);
		bool WriteData(int senderId, string writeData);
    }
}
