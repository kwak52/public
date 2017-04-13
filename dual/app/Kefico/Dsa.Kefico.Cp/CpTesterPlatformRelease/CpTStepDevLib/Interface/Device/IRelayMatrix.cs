using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IRelayMatrix : IDevice
    {
        bool DevOpen(string Device_ID, string topologyName);

        bool ConnectSwitch(string rowName, string colName);

        bool DisconnectSwitch(string rowName, string colName);
    }
}
