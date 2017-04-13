using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IResistor : IDevice
    {
        bool DevOpen(string BoardName, int SlotNumber, int OffsetValue = 0);

        bool SetResistor(int Channel, double Resistance);
        int TotalChannel();
    }
}
