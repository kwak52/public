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
	/// F_UGEBR_ZUSATZ = additional board setting for the V-BOARD which giving the voltage to the product.
	/// [CCS Doc]
	/// This module - as well as the ' F_ANSTEUER' module - the Interface from Gaudí to subordinate hardware driver represents .
	/// The one of the two DC sources can by calling a 'F_ANSTEUER'- and - if necessary - 'F_6073' module be programmed .
	/// The in 'F_6073 ' transferred data are not part of PAV and therefore this special module to the predetermined location in the 'shared memory' copies.
	/// - turn on or off of the V-board. (my opinion)
	/// - set the maximum ampere and voltage of the V-Board. (my opinion)
	/// </summary>    

	public class CpFnF_UGEBR_ZUSATZ : CpTsShell, IF_UGEBR_ZUSATZ
    {
        const string DEFAULT_MAX_VOLTAGE = "20";        // 20 V
        const string DEFAULT_LIMIT_CURRENT = "2.0";       // 2 A

        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager cpMngStation, CpTsShell cpTsParent = null)
        {
            // main function.                        
            PsCCSStdFnModuleF_UGEBR_ZUSATZ psModuleF_UGEBR_ZUSATZ = this.Core as PsCCSStdFnModuleF_UGEBR_ZUSATZ;
            Debug.Assert(psModuleF_UGEBR_ZUSATZ != null);


            // log
            this.ResultLog.TsActionResult = TsResult.OK;
            return TsResult.OK;
        }
    }
}
