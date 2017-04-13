using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpUtility.TextString;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsDIOControlInfo : ClsDeviceInfoBase
    {
        public int DIGITAL_INPUT_POINT { set; get; }
        public int DIGITAL_OUTPUT_POINT { set; get; }
        public string COMMENT { set; get; }

        public ClsDIOControlInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");
            COMMENT = attNode.Attributes["COMMENT"].Value;
            DIGITAL_INPUT_POINT = Convert.ToInt32(attNode.Attributes["DIGITAL_INPUT_POINT"].Value);
            DIGITAL_OUTPUT_POINT = Convert.ToInt32(attNode.Attributes["DIGITAL_OUTPUT_POINT"].Value);
        }
    }
}
