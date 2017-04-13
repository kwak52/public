using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsRelayInfo : ClsDeviceInfoBase
    {
        public ClsRelayInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
        }
    }
}
