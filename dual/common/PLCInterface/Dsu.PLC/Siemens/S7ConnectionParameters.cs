using Dsu.PLC.Common;

namespace Dsu.PLC.Siemens
{
    public class S7ConnectionParameters : ConnectionParametersEthernet
    {
        public S7ConnectionParameters(string ip, ushort port=102, TransportProtocol protocol = TransportProtocol.Tcp)
            : base(ip, port, protocol)
        {
        }
    }
}
