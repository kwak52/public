using CpTesterPlatform.CpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsNetworkUdpInfo : ClsDeviceInfoBase
    {
        public int ServerPort { set; get; } = 0;

        public ClsNetworkUdpInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");
            ServerPort = Convert.ToInt32(attNode.Attributes["SERVERPORT"].InnerText);
        }
    }
}
