using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IDIOControl : IDevice
    {
        string CONTROLLER_ADDRESS { set; get; }
        int DIGITAL_INPUT_POINT { set; get; }
        int DIGITAL_OUTPUT_POINT { set; get; }
        List<bool> CURRENT_DI_STATE { set; get; }
        List<bool> CURRENT_DO_STATE { set; get; }

        void SetDOutState(int nPointIdx, bool bState);
        bool GetDOutState(int nPointIdx);
        bool GetDInState(int nPointIdx);
    }
}
