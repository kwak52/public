using System.Xml;
using CpTesterPlatform.CpCommon;
using System;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsDAQControllerInfo : ClsDeviceInfoBase
    {
        public string DEVICE_NAME { get; set; }
        public int SAMPLING_PER_SEC { get; set; }
        public int SAMPLE_PER_BUFFER { get;set;}

        public ClsDAQControllerInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");
            SAMPLING_PER_SEC = Convert.ToInt32(attNode.Attributes["SAMPLING_PER_SEC"].Value);
            SAMPLE_PER_BUFFER = Convert.ToInt32(attNode.Attributes["SAMPLE_PER_BUFFER"].Value);
        }
    }
}
