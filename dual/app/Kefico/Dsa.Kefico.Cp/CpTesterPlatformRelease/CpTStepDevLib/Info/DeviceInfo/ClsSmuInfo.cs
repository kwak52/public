using CpTesterPlatform.CpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsSmuInfo : ClsDeviceInfoBase
    {
        public ClsSmuInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            SmuType = (CpInstSmuType)Enum.Parse(typeof(CpInstSmuType), node["ID"].InnerText);
            ChannelNum = Convert.ToInt16(attNode.Attributes["CHNUM"].Value);
            LimitVolt = Convert.ToDouble(attNode.Attributes["LIMITVOLT"].Value);
            LimitCurr = Convert.ToDouble(attNode.Attributes["LIMITCURRENT"].Value);
        }

        public int ChannelNum { get; private set; } = 0;
        public double LimitVolt { get; private set; } = 0.0;
        public double LimitCurr { get; private set; } = 0.0;
        public CpInstSmuType SmuType { get; private set; } = CpInstSmuType.POWER_1;

    }
}
