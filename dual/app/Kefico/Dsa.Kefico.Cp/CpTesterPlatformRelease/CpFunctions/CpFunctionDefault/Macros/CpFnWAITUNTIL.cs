using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// This function is used to pause the testing sequence until a specified time.
	/// It controls a timer and can access that timer.
	/// The result returned is the timer reading at which it is detected that the waiting period has elapsed.
	/// With the aid of tolerance limits this can be used to generate an error message if the specified time has already passed when the function is invoked.    
	/// </summary>

	public class CpFnWAITUNTIL : CpTsMacroMsShell, IWAITUNTIL
    {
		public CpFnWAITUNTIL()
		{
			 
		}
    }
}
