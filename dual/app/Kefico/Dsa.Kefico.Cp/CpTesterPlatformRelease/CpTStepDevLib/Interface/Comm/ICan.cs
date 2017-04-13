using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface ICan : IDevice, IComm
    {
        bool DevOpen(string DeviceName, uint devBaudrate);
    }
}
