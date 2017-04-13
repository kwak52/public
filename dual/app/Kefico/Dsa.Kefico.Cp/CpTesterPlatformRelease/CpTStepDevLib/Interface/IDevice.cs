using CpTesterPlatform.CpCommon;
using System.Reflection;
using CpUtility;

namespace CpTesterPlatform.CpTStepDev.Interface
{
	[FixedSignature("Device interface")]
    public interface IDevice
    {
		CpFunctionEventHandler FuncEvtHndl { get; set; }
		string DeviceID { get; set; }

        string GetModuleName();
        bool DevOpen();
        bool DevClose();
        bool DevReset();
    }
}
