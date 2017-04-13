using System;
using System.Reflection;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.ControlFn;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// COM = communication
	/// Block = message block    
	/// [CCS Doc]
	/// COMBLOCK = Initializes the CAN interface for the KWP2000 protocol.
	/// Sends an interface protocol block from the tester via the serial interface to the control unit.
	/// Depending on the mode, a response (measured data block) may be expected from the control unit;
	/// the response is written to a received data memory. 
	/// The function ComBlock can be used in conjunction with the functions; K2000Ini, CAN2000Ini, as well as the general function ComInit.
	/// That received data memory can be accessed by means of the functions  BlockInt  IDH_BlockInt and  BlockStr  IDH_BlockStr .
	/// Evaluation of tolerances within the ComBlock function is not permissible.
	/// </summary>
	/// <param name="Interface_Protocol_Block"> string.</param>
	/// <param name="Bytes_Expected_Back"> int.</param>
	/// <param name="Timeout"> sec.</param>
	/// <param name="Mode"> c/init/l/s.</param>  

	public class CpFnCOMBLOCK : CpTsMacroShell, ICOMBLOCK
    {
		public CpFnCOMBLOCK()
		{
			 
		}

        public bool isLastMessage()
        {
            var oResult = TryFunc(() =>
            {
                /* 8th parameter is "C/L"*/
                string[] arstrSummary = this.Core.GetTestStepSummary();
                string arstrParms = arstrSummary[ClsDefineCANConstantcs.FLAG_COMBLOCK_LOC_LC_DATA];
                string strParmsLCFlag = arstrParms.Split(',')[Convert.ToInt32(PsCCSStdFnCtrCOMBLOCK.FuncParms.FLAG_LC)];

                CpCANComblockParm eComblockType = (CpCANComblockParm)Enum.Parse(typeof(CpCANComblockParm), strParmsLCFlag);
                return eComblockType == CpCANComblockParm.L ? true : false;
            });
            if (oResult.HasException)
            {

                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to get information of this test step in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + oResult.Exception.ToString(), ConsoleColor.White);

                return false;
            }

            return oResult.Result;
        }

        public string getMessageData()
        {
            var oResult = TryFunc(() =>
            {
                /* 8th parameter is "C/L"*/
                string[] arstrSummary = this.Core.GetTestStepSummary();
                string arstrParms = arstrSummary[ClsDefineCANConstantcs.FLAG_COMBLOCK_LOC_LC_DATA];
                string strParmsData = arstrParms.Split(',')[Convert.ToInt32(PsCCSStdFnCtrCOMBLOCK.FuncParms.DATA)];

                return strParmsData;
            });
            if (oResult.HasException)
            {

                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to get information of this test step in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + oResult.Exception.ToString(), ConsoleColor.White);

                return string.Empty ;
            }

            return oResult.Result;
        }
    }
}
