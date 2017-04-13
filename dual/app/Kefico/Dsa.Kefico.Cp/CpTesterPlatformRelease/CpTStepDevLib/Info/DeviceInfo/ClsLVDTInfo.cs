using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsLVDTInfo : ClsDeviceInfoBase
    {
        public string COMMENT { set; get; } = "0";

        public ClsLVDTInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            COMMENT = attNode.Attributes["COMMENT"].Value;
        }

    }
}
