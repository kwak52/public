using CpTesterPlatform.CpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsICControllerInfo : ClsDeviceInfoBase
    {

        public ClsICControllerInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {

        }
    }
}