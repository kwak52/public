using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
	public interface IUPS : IDevice 
	{
        string IP_ADDRESS { set; get; }
        double GetTemperature();
        double GetHumidity();
    }
}
