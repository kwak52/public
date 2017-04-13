using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Performs a bit operation on two integers. The result is also an integer.
	/// </summary>
	/// <param name="Value1 [INT]"> First integer value (or variable which contains the value) for the bit operation.</param>
	/// <param name="Value2 [INT]"> Second integer value (or variable which contains the value) for the bit operation.</param>
	/// <param name="Operation [Str]"> Logical bit operator: AND, OR, XOR.</param>

	public class CpFnBITOP : CpTsMacroMsShell, IBITOP
    {
		public CpFnBITOP()
		{
		}
    }
}
