using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
	public interface ILVDT : IDevice 
	{
        string COM_PORT { set; get; }
        double GetFuntionDimension();
        void  SetMasterDimension();
    }
}
