using CpTesterPlatform.CpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PsCommon;
using PsCommon.Enum;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IDmm : IDevice
    {
        //bool DevOpen(string Device_ID);
        double GetCurrentVoltage();
        double GetDmmSgValue(string sChannel, ClsDMMInfo.NIDMMPropertiesInfo clsDMMInfo);
        double GetDmmSgTrgValue(string sChannel, ClsDMMInfo.NIDMMPropertiesInfo clsDMMInfo, NIDevTriggerInfo clsTriggerInfo);
        bool SetDmmSgTrgConfigure(string sChannel, ClsDMMInfo.NIDMMPropertiesInfo clsDMMInfo, NIDevTriggerInfo clsTriggerInfo, CpDefineTriggerType eTriggerType);
        double GetCurrentSampleRate();
        void ResetLastTrigger();
        double[] ReadDmmSgTrgArray(ClsDMMInfo.NIDMMPropertiesInfo clsDMMInfo);
        int findTriggeredIndexFrArray(double[] ardReadData, eFLANKE eSlope, double dTrgLevel, double dWindow);
        double[] ReadDmmSgExtTrgArray(ClsDMMInfo.NIDMMPropertiesInfo clsDMMInfo);
    }
}
