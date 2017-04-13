using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsCANInfo : ClsDeviceInfoBase
    {
        public uint Baudrate { set; get; } = 0;
        public string Identifier { set; get; } = string.Empty;

        public ClsCANInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");
            Baudrate = Convert.ToUInt32(attNode.Attributes["BAUDRATE"].InnerText);
            Identifier = attNode.Attributes["IDENTIFIER"].InnerText;
        }
    }

    public class CanPacket
    {
        public byte[] Data { set; get; } = null;
        public int Length { set; get; } = 0;
        public int Identifier { set; get; } = 0;
    }
}
