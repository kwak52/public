using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// CAN = CAN
	/// 2000 = KWP2000
	/// INI = INITIALIZATION
	/// [CCS Doc]
	/// CAN2000INI = Initializes the CAN interface for the KWP2000 protocol.
	/// Following successful initialization, the testing station can communicate with the control unit using the function ComBlock.
	/// The specified timeout periods are:
	/// Timeout flow-control: 20 ms;
	/// timeout after flow-control: 50 ms;
	/// timeout after consecutive: 50 ms;
	/// timeout first frame: 50 ms;
	/// timeout after reject: 5000 ms        
	/// </summary>
	/// <param name="Interface"> CAN1.</param>
	/// <param name="ID"> H.</param>
	/// <param name="BaudRate"> 500000.</param>
	/// <param name="ScanningPoint"> 75.</param>
	/// <param name="TerminalResistance"> 120.</param>
	/// <param name="MessageArchitecture"> 1.</param>
	/// <param name="ID_Send"> 7E0.</param>
	/// <param name="ID_Send_Length"> 11.</param>
	/// <param name="ID_Read"> 7E8.</param>
	/// <param name="ID_Read_Length"> 11.</param>

	public class CpFnCAN2000INI : CpTsMacroShell, ICAN2000INI
    {
		public CpFnCAN2000INI()
		{
			 
		}
    }
}
