using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpSystem.Manager;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.TextString;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using PsCommon.Enum;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Call parameters according to Mode global.
	/// ZUWEIS = Assign
	/// Task of the module is to write a constant to a variable .
	/// However, it is also possible a variable by another variable to occupy. This is for example a variable of a ntig 
	/// To maintain macro in the checklist as Prueflisten variable to this to store for further processing .
	/// </summary>    
	/// <param name="VARIABLE"> ex.&G_GETDATA_ALIASFILE.</param>
	/// <param name="VARIABLEN_WERT"> ex.$P_0.</param>
	/// <param name="VAR_FUNKTION">.</param>
	/// <param name="MAKRO_NAME">.</param>
	/// <param name="VARIABLEN_TYP">ex.STRING.</param>

	public class CpFnM_ZUWEIS : CpTsShell, IM_ZUWEIS
    {
        public void setShuntValue(ICbManager cbManager, CpTsShell cpTsParent)
        {
            IDCITRG cpTsDCITrg = cpTsParent as IDCITRG;
            // Set default shunt value for KAM.
            if (cpTsDCITrg != null)
            {
                PsCCSStdFnModuleM_ZUWEIS psModuleM_ZUWEIS = this.Core as PsCCSStdFnModuleM_ZUWEIS;
                Debug.Assert(psModuleM_ZUWEIS != null);

                string strAssignedValue = psModuleM_ZUWEIS.VARIBLEN_WERT;

                double dShunt = 0.0;
                if (double.TryParse(strAssignedValue, out dShunt))
                {
                    if (dShunt == 0.1) cpTsDCITrg.setShuntValue(CpInstLoadValue.REG_01);
                    else if (dShunt == 1) cpTsDCITrg.setShuntValue(CpInstLoadValue.REG_1);
                    else if (dShunt == 10) cpTsDCITrg.setShuntValue(CpInstLoadValue.REG_10);
                    else if (dShunt == 100) cpTsDCITrg.setShuntValue(CpInstLoadValue.REG_100);
                    else if (dShunt == 1000) cpTsDCITrg.setShuntValue(CpInstLoadValue.REG_1000);
                    else cpTsDCITrg.setShuntValue(CpInstLoadValue.NONE);
                }
            }
        }

        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleM_ZUWEIS psModuleM_ZUWEIS = this.Core as PsCCSStdFnModuleM_ZUWEIS;
            Debug.Assert(psModuleM_ZUWEIS != null);

            string strVariable = psModuleM_ZUWEIS.VARIABLE;
            string strAssignedValue = psModuleM_ZUWEIS.VARIBLEN_WERT;
            eVARIABLEN_TYP VARIABLEN_TYP = psModuleM_ZUWEIS.VARIABLEN_TYP;

            if (strVariable == string.Empty)
                Debug.Assert(false);

            IBERECHNEN cpTsBerechnen = cpTsParent as IBERECHNEN;
            if (cpTsBerechnen != null)
            {
                iMngStation.MngTStep.GetMngGv().SetValue(strVariable, cpTsBerechnen.getEquationResult().ToString(), CpStringFormat.ASCII);
                this.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }

            IDCIS cpTsDcis = cpTsParent as IDCIS;
            if (cpTsDcis != null)
            {
                cpTsDcis.setEquationResult(Double.Parse(strAssignedValue));
            }

            //CpTsMacroMsDCIShell cpTsDCI /*or DCIS */ = cpTsParent as CpTsMacroMsDCIShell;
            IDCI cpTsDCI = cpTsParent as IDCI;
            IDCIS cpTsDCIS = cpTsParent as IDCIS;
            IDCITRG cpTsDCITrg = cpTsParent as IDCITRG;

            // DCV function should not be entered.
            if (cpTsDCI != null || cpTsDCITrg != null)
            {
                IMacro cpTsDCI_Trg = null;
                if (cpTsDCI != null)
                    cpTsDCI_Trg = cpTsDCI;
                else if (cpTsDCITrg != null)
                    cpTsDCI_Trg = cpTsDCITrg;
                else
                    Debug.Assert(false);

                CpSpecDimension eDimension = (CpSpecDimension)Enum.Parse(typeof(CpSpecDimension), VARIABLEN_TYP.ToString());
                CpStringFormat eFormat = UtilTextConvertOrParse.convertDimmensionToStringFormat(eDimension);

                // If it is kinds of a local variable, the list contains global variable should be added it immediately.
                // ex. Although DCITRG - RDCITRG are separated, the common variable used in the DCITRG used in the DCITRG only.

                CpGlbVarBase gvGetVariable = iMngStation.MngTStep.GetMngGv().GetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsDCI_Trg.MacroGlobalVar, strVariable);
                iMngStation.MngTStep.GetMngGv().SetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsDCI_Trg.MacroGlobalVar, strVariable, strAssignedValue, eFormat);

                if (cpTsDCIS != null)
                {
                    //cpMngSystem.MngHardware.MngLoad.setKamShuntResister(strAssignedValue, eFormat);
                }
				

                //cpMngSystem.MngTStep.MngGlobalVariable.setGvValue(strVariable, strAssignedValue, eFormat);
                this.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }

            this.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.NONE;
        }
    }
}
