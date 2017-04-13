using System.Xml;
using CpTesterPlatform.CpCommon;
using System;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsPowerSupplyInfo : ClsDeviceInfoBase
    {
        public double VOLTAGE_LIMIT { set; get; } = 10;
        public double CURRENT_LIMIT { set; get; } = 10;
        public int  CHANNEL { set; get; } = 1;
        public string COMMENT { set; get; }

        public ClsPowerSupplyInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {          
            XmlNode attNode = node.SelectSingleNode("Attributes");

            VOLTAGE_LIMIT = Convert.ToDouble(attNode.Attributes["VOLTAGE_LIMIT"].Value);
            CURRENT_LIMIT = Convert.ToDouble(attNode.Attributes["CURRENT_LIMIT"].Value);
            CHANNEL = Convert.ToInt32(attNode.Attributes["CHANNEL"].Value);
            COMMENT = attNode.Attributes["COMMENT"].Value;
        }
    }
}
