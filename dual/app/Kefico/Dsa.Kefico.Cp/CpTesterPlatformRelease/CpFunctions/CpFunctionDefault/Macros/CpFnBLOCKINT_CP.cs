using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// CAN = CAN
	/// 2000 = KWP2000
	/// INI = INITIALIZATION
	/// [CCS Doc]
	/// BLOCKINT = Initializes the CAN interface for the KWP2000 protocol.
	/// This function can only be carried out after a read operation by the function  ComBlock or CANBlock  IDH_ComBlock.
	/// As its result, it returns the bytes specified in the column Testing Parameters 
	/// from low byte to high byte of a loaded measured data block and interprets them as integers.
	/// If LowByte = HighByte, the result returned is only one byte.       
	/// </summary>
	/// <param name="LowByte"> int.</param>
	/// <param name="HighByte"> int.</param>    

	public class CpFnBLOCKINT_CP : CpTsMacroMsShell, IBLOCKINT_CP
    {
		public CpFnBLOCKINT_CP()
		{
		}
    }
}
