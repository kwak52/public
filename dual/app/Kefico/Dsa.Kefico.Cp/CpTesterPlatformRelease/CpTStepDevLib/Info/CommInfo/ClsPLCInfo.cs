using System;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsPLCInfo : ClsDeviceInfoBase 
	{
        public string CPU_TYPE_STR { set; get; } = string.Empty; //Example: "Q03UDECPU";        
        public string PLC_IP_ADDR { set; get; } = string.Empty;
        public int PLC_PORT_NUMBER { set; get; } = 8080;
        public string CONNECTION_TYPE_STR { set; get; } = string.Empty;
        public int TIMEOUT { set; get; } = 1000;
        public bool READPORT { set; get; } = false;

        public ClsPLCInfo(CpDeviceType insttype, XmlNode node)
			: base(insttype, node)
		{
			XmlNode attNode = node.SelectSingleNode("Attributes");

            CPU_TYPE_STR = attNode.Attributes["CPU_TYPE_STR"].Value; //Example: "Q03UDECPU";        
            PLC_IP_ADDR = attNode.Attributes["PLC_IP_ADDR"].Value;
            PLC_PORT_NUMBER = Convert.ToInt32(attNode.Attributes["PLC_PORT_NUMBER"].Value);
            CONNECTION_TYPE_STR = attNode.Attributes["CONNECTION_TYPE_STR"].Value;
            TIMEOUT = Convert.ToInt32(attNode.Attributes["TIMEOUT"].Value);
            READPORT = Convert.ToBoolean(attNode.Attributes["READPORT"].Value);

        }
	}
}
