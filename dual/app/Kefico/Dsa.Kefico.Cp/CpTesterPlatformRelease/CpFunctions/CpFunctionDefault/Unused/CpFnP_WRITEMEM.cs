using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
	public class CpFnP_WRITEMEM : CpTsShell, IP_WRITEMEM
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
			PsCCSStdFnModuleP_WRITEMEM psModuleP_WRITEMEM = this.Core as PsCCSStdFnModuleP_WRITEMEM;
			Debug.Assert(psModuleP_WRITEMEM != null);

			string strDataDevID = psModuleP_WRITEMEM.DeviceID;
			string strDataInput = psModuleP_WRITEMEM.INPUTDATA;
			string strDataTimeout = psModuleP_WRITEMEM.TIMEOUT;
											
			if(cpTsParent is ILASER_MARK)
			{
				
			}
                
			return TsResult.OK;
        }
    }
}
