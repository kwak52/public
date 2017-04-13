using CpTesterPlatform.CpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsDeviceList : Dictionary<string,ClsDeviceInfoBase>
    {
        public ClsDeviceInfoBase GetDeviceInfo(string devId)
        {
            return this[devId];
        }
    }

    public class ClsMitechLoadUnitList : List<ClsLoadUnitInfo>
    {

    }
}
