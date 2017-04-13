using Dsu.PLC.Common;

namespace Dsu.PLC.Melsec
{
    public class MxConnectionParameters : ConnectionParametersEthernet
    {
        public MxConnectionParameters(string ip, ushort port=5000, TransportProtocol protocol = TransportProtocol.Udp)
            : base(ip, port, protocol)
        {
        }
    }
}
