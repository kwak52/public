using System.Xml;
using CpTesterPlatform.CpCommon;
using System;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsLCRMeterInfo : ClsDeviceInfoBase 
	{
        public int CHANNEL { set; get; } = 1;
        public string COMMENT { set; get; }

        public ClsLCRMeterInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            COMMENT = attNode.Attributes["COMMENT"].Value;
            CHANNEL = Convert.ToInt32(attNode.Attributes["CHANNEL"].Value);
        }
	}
}
