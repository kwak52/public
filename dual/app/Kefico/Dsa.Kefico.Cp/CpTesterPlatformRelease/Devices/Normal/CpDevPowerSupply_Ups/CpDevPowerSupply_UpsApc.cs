using CpTesterPlatform.CpTStepDev.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevPowerSupply_UpsApc : IDevice
    {
        public string DeviceID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public CpTesterPlatform.CpCommon.CpFunctionEventHandler FuncEvtHndl
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DevClose()
        {
            throw new NotImplementedException();
        }

        public bool DevOpen()
        {
            var srv = Dsu.Driver.Paix.createManager("192.168.0.11", true)?.Value;
            return true;
        }

        public bool DevReset()
        {
            throw new NotImplementedException();
        }

        public string GetModuleName()
        {
            throw new NotImplementedException();
        }
    }
}
