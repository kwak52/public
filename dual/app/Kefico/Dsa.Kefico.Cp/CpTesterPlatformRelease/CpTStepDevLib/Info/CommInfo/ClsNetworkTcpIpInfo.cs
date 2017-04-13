using System;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsNetworkTcpIpInfo : ClsDeviceInfoBase    
	{
		public string IP_ADDRESS { set; get; } = string.Empty;
		public int PORT_NO { set; get; } = 8080;
		public bool CR { set; get; } = true;
		public bool LF { set; get; } = true;

		public ClsNetworkTcpIpInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

			IP_ADDRESS = attNode.Attributes["IP_ADDRESS"].Value;
			PORT_NO = Convert.ToInt32(attNode.Attributes["PORT_NO"].Value);
			CR = Convert.ToBoolean(attNode.Attributes["CR"].Value);
			LF = Convert.ToBoolean(attNode.Attributes["LF"].Value);
        }
	}
}
