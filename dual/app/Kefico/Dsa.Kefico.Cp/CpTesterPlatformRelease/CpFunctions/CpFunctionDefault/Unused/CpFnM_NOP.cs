using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Calling the process VXI_DMM.
	/// </summary>  

	public class CpFnM_NOP : CpTsShell, IM_NOP
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager cpMngStation, CpTsShell cpTsParent = null)
        {
            ResultLog.TsActionResult = TsResult.NONE;

            // CAN2000INI is not kind of wiring function.
            ICAN2000INI cpTsCAN2000INI = cpTsParent as ICAN2000INI;
            if (cpTsCAN2000INI != null)
                ResultLog.TsActionResult = TsResult.OK;

            // BERECHNEN is not kind of wiring function.
            IBERECHNEN cpTsBERECHNEN = cpTsParent as IBERECHNEN;
            if (cpTsBERECHNEN != null)
                ResultLog.TsActionResult = TsResult.OK;

            // STRCAT is not kind of wiring function.
            ISTRCAT cpTsSTRCAT = cpTsParent as ISTRCAT;
            if (cpTsSTRCAT != null)
                ResultLog.TsActionResult = TsResult.OK;

            // STRCMP is not kind of wiring function.
            ISTRCMP cpTsSTRCMP = cpTsParent as ISTRCMP;
            if (cpTsSTRCMP != null)
                ResultLog.TsActionResult = TsResult.OK;

            // WAITUNTIL is not kind of wiring function.
            IWAITUNTIL cpTsWAITUNTIL = cpTsParent as IWAITUNTIL;
            if (cpTsWAITUNTIL != null)
                ResultLog.TsActionResult = TsResult.OK;

            return TsResult.OK;
        }
    }
}
