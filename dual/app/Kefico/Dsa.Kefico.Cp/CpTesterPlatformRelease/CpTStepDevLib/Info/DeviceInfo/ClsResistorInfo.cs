using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsResistorInfo : ClsDeviceInfoBase
    {
        public ClsResistorInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            BusNumber = Convert.ToInt16(attNode.Attributes["HWNAME"].InnerText);
            SlotNumber = Convert.ToInt16(attNode.Attributes["SLOTNUM"].Value);
            ChannelNum = Convert.ToInt16(attNode.Attributes["CHNUM"].Value);
            Offset = Convert.ToInt16(attNode.Attributes["OFFSET"].Value);
        }

        public int BusNumber { set; get; } = 0;
        public int SlotNumber { set; get; } = 0;
        public int ChannelNum { set; get; } = 0;
        public int Offset { set; get; } = 0;
    }
}
