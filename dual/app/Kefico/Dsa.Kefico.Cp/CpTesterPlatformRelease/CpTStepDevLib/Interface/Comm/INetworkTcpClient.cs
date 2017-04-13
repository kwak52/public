using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public class TcpData : EventArgs
    {
        public byte[] lstByteData { get; set; }
    }
    public interface INetworkTcpClient : IDevice, IComm
    {
        event EventHandler<TcpData> ReceivedTCPData;
        bool DevOpen(string serverIp, int serverPort);
    }
}
