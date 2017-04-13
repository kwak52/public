using System;
using System.Diagnostics;
using System.Reflection;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.TextString;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
	public class CpFnR_CURRENT_DATE : CpTsShell, IR_CURRENT_DATE
    {
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);

            return TsResult.NONE;
        }
    }
}
