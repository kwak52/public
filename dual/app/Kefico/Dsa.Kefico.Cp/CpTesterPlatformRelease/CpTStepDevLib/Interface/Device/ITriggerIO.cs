using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface ITriggerIO : IDAQIO
    {
		string DISourceChs { set; get; }
		string TrgOutputCh { set; get; } 
		string TrgRisingChs  { set; get; } 
		string TrgFallingChs  { set; get; }
    }
}
