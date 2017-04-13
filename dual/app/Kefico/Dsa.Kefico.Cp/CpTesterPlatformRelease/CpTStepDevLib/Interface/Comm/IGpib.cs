using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IGpib : IDevice, IComm
    {		
		int BoardNumber { set; get; }
		byte PrimaryNumber { set; get; } 
		byte SecondaryNumber { set; get; }
		char EndOfStringOnReading { set; get; }
		char EndOfIdentityOnWriting { set; get; } 
		int Timeout { set; get; } 
		
        bool DevOpen(int boardNumber, uint primaryNum, uint secondaryNum);
		void SetCommConfig(string strEndOfStringOnReading, string strEndOfIdentityOnWriting, string strTimeout);
    }
}
