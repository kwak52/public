using Dsu.PLC.Common;

namespace Dsu.PLC.Fuji
{
    public class FjConnectionParameters : ConnectionParametersEthernet
    {
		internal FjHwConfig _hwConfig;
	    internal Config Config { get { return _hwConfig.Config; } }
		public FjConnectionParameters(string ioAreaInitFilePath, string ip, ushort port=507)
            : base(ip, port, TransportProtocol.Tcp)
        {
			_hwConfig = new FjHwConfig(ioAreaInitFilePath);
        }
	}
}
