using System;
using System.Reflection;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// COM = communication
	/// Block = message block    
	/// [CCS Doc]
	/// COMBLOCK_CP = Initializes the Serial (RS-232) or Parrarel (GPIB) interface.
	/// Sends an interface protocol block from the tester via the serial interface to the control unit.
	/// Depending on the mode, a response (measured data block) may be expected from the control unit;
	/// the response is written to a received data memory. 
	/// The function ComBlock_CP can be used in conjunction with the functions; COM_INIT_CP.
	/// That received data memory can be accessed by means of the functions  BLOCKINT_CP
	/// Evaluation of tolerances within the ComBlock_CP function is not permissible.
	/// </summary>
	/// <param name="Interface_Protocol_Block"> string.</param>
	/// <param name="Bytes_Expected_Back"> int.</param>
	/// <param name="Timeout"> sec.</param>
	/// <param name="Mode"> c/init/l/s.</param>  

	public class CpFnWRITE_EEP_DATA : CpTsMacroShell, IWRITE_EEP_DATA
    {
		public CpFnWRITE_EEP_DATA()
		{
			 
		}		
    }
}
