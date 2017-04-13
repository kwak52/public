using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface ISwitchBlock : IDevice
    {
        bool DevOpen(string Device_ID, string topologyName);

        bool ConnectSwitch(string rowName, string colName, int blockNum = 0);

        bool DisconnectSwitch(string rowName, string colName, int blockNum = 0);
    }
}
