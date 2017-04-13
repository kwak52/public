using System;
using System.Diagnostics;
using System.Reflection;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.TextString;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;


namespace CpTesterPlatform.Functions
{
	public class CpFnM_DMM : CpTsShell, IM_DMM
    {
        private ClsDMMInfo.NIDMMPropertiesInfo MeasuringDevInfo = new ClsDMMInfo.NIDMMPropertiesInfo();

        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent)
        {
            TryAction(() =>
            {
                base.ExecuteProlog(cpMngSystem, iMngStation, cpTsParent);

                IGvManager iMngGv = iMngStation.MngTStep.GetMngGv();

                string sRetrunValue = string.Empty;

                CpTsMacroShell parentMacro = cpTsParent as CpTsMacroShell;
                PsCCSStdFnModuleM_DMM psModuleM_DMM = this.Core as PsCCSStdFnModuleM_DMM;
                Debug.Assert(psModuleM_DMM != null);

                // trigger delay was not set at the trigger information.
                string sTrgDelay = psModuleM_DMM.DMM_TRG_DELAY;
                double dTrgDelay = 0.0;
                if (iMngGv.IsKindOfVariable(sTrgDelay))
                {
                    CpGlbVarBase gvTrgDelay = iMngGv.GetValueWithGlobalLocalAll(iMngGv, parentMacro.MacroGlobalVar, sTrgDelay);
                    dTrgDelay = Convert.ToDouble(gvTrgDelay.RawValue);
                }
                else
                    dTrgDelay = Convert.ToDouble(sTrgDelay);

                string sReturnValueName = psModuleM_DMM.R_MESSWERT;
                int nChannel = Convert.ToInt32(psModuleM_DMM.VXI_GERAET);
                

                CpSpecDMMMesureMode dmmMode = (CpSpecDMMMesureMode)Enum.Parse(typeof(CpSpecDMMMesureMode), psModuleM_DMM.MESS_MODUS.ToString());
                UtilTextMessageEdits.UtilTextMsgToConsole("Current step # = " + parentMacro.Core.StepNum.ToString(), ConsoleColor.Yellow, CpDefineEnumDebugPrintLogLevel.DEBUG);
              
            });

            return base.ExecuteEpilog(cpMngSystem, iMngStation, cpTsParent);
        }

        public void createDMMProperies(ICbManager cbManager, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleM_DMM psModuleM_DMM = this.Core as PsCCSStdFnModuleM_DMM;
            Debug.Assert(psModuleM_DMM != null);

            var tResult = TryAction(() =>
            {
                

                // log
                this.ResultLog.TsActionResult = TsResult.OK;
            });

            if ( tResult.HasException )
                this.ResultLog.TsActionResult = TsResult.ERROR;
        }
    }
}
