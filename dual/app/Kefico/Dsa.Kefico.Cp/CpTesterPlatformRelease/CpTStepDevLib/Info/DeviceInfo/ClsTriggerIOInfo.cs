using System;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsTriggerIOInfo : ClsDeviceInfoBase
    {
        public string IP_ADDRESS { get; set; }
        public string COMMENT { set; get; }

        public ClsTriggerIOInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");
            IP_ADDRESS = attNode.Attributes["IP_ADDRESS"].Value;
            COMMENT = attNode.Attributes["COMMENT"].Value;

        }
    }
}
