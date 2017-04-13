namespace Dsu.PLC.Common
{
    public abstract class ConnectionParametersEthernet : IConnectionParametersEthernet
    {
        public string Ip { get; }
        public ushort Port { get; }

        public TransportProtocol TransportProtocol { get; }

        public ConnectionParametersEthernet(string ip, ushort port, TransportProtocol protocol=TransportProtocol.Tcp)
        {
            Ip = ip;
            Port = port;
            TransportProtocol = protocol;
        }
    }
}
