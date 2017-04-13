using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// GEBER = GIVER
	/// ZUSATZ = ADDITIONAL
	/// F_IGEBR_ZUSATZ = additional board setting for the I-BOARD which giving the ampere to the product.
	/// [CCS Doc]
	/// GAUDI interface in Vector interpreter for 6051 Map
	/// - turn on or off of the I-board. (my opinion)
	/// - set the maximum ampere and voltage of the I-Board. (my opinion)
	/// 
	/// 1. ANSTEUER_UNIT : I_GEBER_
	/// 2. SPANNUNG : Selection of the open circuit voltage to the current source map;
	///             the positive value should not overstep 105 volts;
	///             the unit of the voltage [V];
	///             
	/// 3. REFERENZ : Selecting a reference account
	///                 EXT : in this case must be <= 20.0 volts are given a corresponding positive reference voltage.
	///                 INT : in this case, the internal reference is taken into account.
	///                 -   : this is the default setting
	/// 
	/// 4. MAX_U_REFERENZ : Selection of the reference voltage is switched on external reference parameters
	///                     20 >= X > 0
	/// 
	/// 5. OUTPUT_MODE : Selection of the output mode of the encoder. 
	///                 ON : switched output
	///                 UT : switched output and untimed (The same as "ON")
	///                 T  : clocked output
	///                 OFF / - : these inputs turn off the output
	///                           the state of the output is independent of the other settings.
	///                           in the default setting of the output is switched off
	/// </summary>    

	public class CpFnF_IGEBR_ZUSATZ : CpTsShell, IF_IGEBR_ZUSATZ
    {
        const string DEFAULT_VOLTAGE = "104";           // 104 V
        const string DEFAULT_EXT_MAX_VOLTAGE = "20";        //  EXT : 20 V
        const string DEFAULT_CURRENT_RANGE = "0.1";    // 450 mA
        const string CONTROL_UNIT = "ANSTEUER_PAR";

        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager cpMngStation,  CpTsShell cpTsParent = null)
        {
            // main function.            
            PsCCSStdFnModuleF_IGEBR_ZUSATZ psModuleF_IGEBR_ZUSATZ = this.Core as PsCCSStdFnModuleF_IGEBR_ZUSATZ;
            Debug.Assert(psModuleF_IGEBR_ZUSATZ != null);

            //Check Version New PsCommon.Enum
            string strCtrUnit = psModuleF_IGEBR_ZUSATZ.ANSTEUER_UNIT;
            string strCtrLimitUnit = "I_QUMAX" + strCtrUnit.Substring(strCtrUnit.LastIndexOf("_"));
            string strSource = psModuleF_IGEBR_ZUSATZ.REFERENZ;

            


            string strLimitCurr = psModuleF_IGEBR_ZUSATZ.STROMBEREICH;
            double dLimitCurr = Convert.ToDouble((strLimitCurr == ClsGlobalStringForTStep.CP_TS_PARM_EMPTY ? DEFAULT_CURRENT_RANGE : strLimitCurr));

            // log
            this.ResultLog.TsActionResult = TsResult.OK;
            return TsResult.OK;
        }
    }
}
