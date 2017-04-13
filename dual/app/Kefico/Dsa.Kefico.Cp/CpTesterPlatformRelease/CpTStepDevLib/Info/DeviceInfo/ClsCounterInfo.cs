using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsCounterInfo : ClsDeviceInfoBase
    {
        public ClsCounterInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {

        }
    }
}
