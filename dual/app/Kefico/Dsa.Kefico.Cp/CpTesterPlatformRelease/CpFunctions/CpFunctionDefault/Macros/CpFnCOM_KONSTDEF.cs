using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.ControlFn;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
	public class CpFnCOM_KONSTDEF : CpTsMacroShell, ICOM_KONSTDEF
    {
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager cpMngStation, CpTsShell cpTsParent = null)
        {
            var oResult = TryFunc(() =>
            {
                PsCCSStdFnCtrCOM_KONSTDEF psacroComdef = this.Core as PsCCSStdFnCtrCOM_KONSTDEF;
                Debug.Assert(psacroComdef != null);

                string strPort = psacroComdef.ArstdSerialParmsP.GetValueByIndex(4);

                base.ExecuteMain(cpMngSystem, cpMngStation, cpTsParent);

                return TsResult.OK;
            });
                        
            if (oResult.HasException)
            {
                this.ResultLog.TsActionResult = TsResult.ERROR;
                return TsResult.ERROR;
            }

            return oResult.Result;
        }
    }
}
