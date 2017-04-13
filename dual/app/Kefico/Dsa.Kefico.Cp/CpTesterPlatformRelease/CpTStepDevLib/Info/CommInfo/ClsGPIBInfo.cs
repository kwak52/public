using CpTesterPlatform.CpCommon;
using System;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsGPIBInfo : ClsDeviceInfoBase
    {
        public ClsGPIBInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            m_nBoardNum = Convert.ToInt16(attNode.Attributes["BOARDNUM"].Value);
            m_bPrimaryAddr = Convert.ToByte(attNode.Attributes["PRIMARY_ADDR"].Value);
            m_bSecondaryAddr = Convert.ToByte(attNode.Attributes["SECONDARY_ADDR"].Value);
        }

        private byte m_bPrimaryAddr = 0x0;
        public byte PrimaryAddr
        {
            get { return m_bPrimaryAddr; }
            set { m_bPrimaryAddr = value; }
        }

        private byte m_bSecondaryAddr = 0x0;
        public byte SecondaryAddr
        {
            get { return m_bSecondaryAddr; }
            set { m_bSecondaryAddr = value; }
        }

        private int m_nBoardNum = 0;
        public int BoardNum
        {
            get { return m_nBoardNum; }
            set { m_nBoardNum = value; }
        }
    }
}
