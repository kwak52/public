using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Initializes and establishes communication via the diagnosis interface for the specified protocol. This function does not return a value.
	/// The function ComInit may only be used if there is no special initialization function (such as K2000Ini) for the required protocol.
	/// </summary>    

	public class CpFnCOM_INIT : CpTsMacroShell, ICOM_INIT
    {
		public string PROTOCOL { set; get; }
		public string BAUD_RATE { set; get; }

		public CpFnCOM_INIT()
		{
			
		}		 
    }
}
