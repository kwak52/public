using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
	public class CpFnE_TRIGGER : CpTsShell, IE_TRIGGER
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpTsMacroMsDCTrgShell cpTsMacroMs = cpTsParent as CpTsMacroMsDCTrgShell;
            PsCCSStdFnModuleE_TRIGGER psModuleE_TRIGGER = this.Core as PsCCSStdFnModuleE_TRIGGER;

            Debug.Assert(psModuleE_TRIGGER != null && cpTsMacroMs != null);

            string strTrgLine = psModuleE_TRIGGER.TRG_LINE;
            string strTrgDelay = psModuleE_TRIGGER.TRG_DELAY;
            string strReturnVariable = psModuleE_TRIGGER.RESTZEIT;

            if (iMngStation.MngTStep.GetMngGv().IsKindOfVariable(strReturnVariable) && !string.IsNullOrEmpty(strReturnVariable))
            {
                iMngStation.MngTStep.GetMngGv().SetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(),
                    cpTsMacroMs.MacroGlobalVar, strReturnVariable, strTrgDelay, CpStringFormat.ASCII);
            }

            if (!cpTsMacroMs.DicTriggerInfo.ContainsKey(strTrgLine))
                return TsResult.NG;//Debug.Assert(false);

            return TsResult.OK;
        }

        public virtual TsResult createTriggerInfo(ICbManager cbManager, CpTsShell cpTsParent = null)
        {
            CpTsMacroMsDCTrgShell cpParent = cpTsParent as CpTsMacroMsDCTrgShell;
            PsCCSStdFnModuleE_TRIGGER psModuleE_TRIGGER = this.Core as PsCCSStdFnModuleE_TRIGGER;

            Debug.Assert(psModuleE_TRIGGER != null && cpParent != null);

            string strTrgLine = psModuleE_TRIGGER.TRG_LINE;
            string strScanUnit = psModuleE_TRIGGER.SCAN_UNIT;

            if (!cpParent.DicTriggerInfo.ContainsKey(strTrgLine))
            {
                cpParent.DicTriggerInfo.Add(
                        strTrgLine,
                        new NIDevTriggerInfo()
                        {
                            MaxLevel = Convert.ToDouble(psModuleE_TRIGGER.MAX_PEGEL),
                            Level = Convert.ToDouble(psModuleE_TRIGGER.PEGEL),
                            Slope = psModuleE_TRIGGER.FLANKE,
                            TrgDelay = Convert.ToDouble(psModuleE_TRIGGER.TRG_DELAY),
                            TrgName = psModuleE_TRIGGER.TRG_NAME,
                            ScanUnit = psModuleE_TRIGGER.SCAN_UNIT,
                            VxiDevice = psModuleE_TRIGGER.VXI_GERAET,
                        }
                    );
            }
            else
            {
                // Already added info
                cpParent.DicTriggerInfo[strTrgLine].MaxLevel = Convert.ToDouble(psModuleE_TRIGGER.MAX_PEGEL);
                cpParent.DicTriggerInfo[strTrgLine].Level = Convert.ToDouble(psModuleE_TRIGGER.PEGEL);
                cpParent.DicTriggerInfo[strTrgLine].Slope = psModuleE_TRIGGER.FLANKE;
                cpParent.DicTriggerInfo[strTrgLine].TrgDelay = Convert.ToDouble(psModuleE_TRIGGER.TRG_DELAY);
                cpParent.DicTriggerInfo[strTrgLine].TrgName = psModuleE_TRIGGER.TRG_NAME;
                cpParent.DicTriggerInfo[strTrgLine].ScanUnit = psModuleE_TRIGGER.SCAN_UNIT;
                cpParent.DicTriggerInfo[strTrgLine].VxiDevice = psModuleE_TRIGGER.VXI_GERAET;
            }

            // Print Added trigger info
            //UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("[CREATED TRIGGER INFO] {0} MaxLevel({1}) Level({2}) Slope({3}) Delay({4}) TrgName({5}) ScanUnit({6}) VxiDevice({7})",
            //    strTrgLine,
            //    cpParent.DicTriggerInfo[strTrgLine].MaxLevel,
            //    cpParent.DicTriggerInfo[strTrgLine].Level,
            //    cpParent.DicTriggerInfo[strTrgLine].Slope.ToString(),
            //    cpParent.DicTriggerInfo[strTrgLine].TrgDelay,
            //    cpParent.DicTriggerInfo[strTrgLine].TrgName,
            //    cpParent.DicTriggerInfo[strTrgLine].ScanUnit,
            //    cpParent.DicTriggerInfo[strTrgLine].VxiDevice
            //    ), ConsoleColor.White);

            return TsResult.NONE;
        }
    }
}
