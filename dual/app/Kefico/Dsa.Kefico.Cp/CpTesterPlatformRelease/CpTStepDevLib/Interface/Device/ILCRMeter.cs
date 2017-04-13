using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
	public interface ILCRMeter : IDevice 
	{
        string CONTROLLER_ADDRESS { set; get; }

        bool SetSettingFile(int LoadID);
        int GetSettingFile();
        double GetInductance();
		double GetCapicatance();
		double GetResistance();
	}
}
