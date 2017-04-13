using Dsu.PLC.Common;

namespace Dsu.PLC.AB
{
    public class AbConnectionParameters : ConnectionParametersEthernet
    {
        public byte[] Path { get; set; } = new byte[] { 0 };
        public AbConnectionParameters(string ip, ushort port = 5000, TransportProtocol protocol = TransportProtocol.Udp)
            : base(ip, 0xAF12/*=44818*/) { }
    }
}
