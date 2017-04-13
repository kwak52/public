using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsFGNInfo : ClsDeviceInfoBase
    {
        private string m_strDevice_IDRef = string.Empty;
        public string Device_IDRef
        {
            get { return m_strDevice_IDRef; }
            set { m_strDevice_IDRef = value; }
        }

        private double m_dSignalLevel = 0.0;
        public double SignalLevel
        {
            get { return m_dSignalLevel; }
            set { m_dSignalLevel = value; }
        }
        public ClsFGNInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            m_strDevice_IDRef = attNode.Attributes["REFDevice_ID"].Value;
            m_dSignalLevel = Convert.ToDouble(attNode.Attributes["SIGNALLEVEL"].Value);
        }
    }
}
