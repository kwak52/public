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
	public class CpFnP_READMEM : CpTsShell, IP_READMEM
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_READMEM psModuleP_READMEM = this.Core as PsCCSStdFnModuleP_READMEM;
			Debug.Assert(psModuleP_READMEM != null);

			string strDataDevID = psModuleP_READMEM.DeviceID;
			string strDataOutput = psModuleP_READMEM.DEVICE_OUTPUT;
			string strDataTimeout = psModuleP_READMEM.TIMEOUT;
															
			if(cpTsParent is IPLC_READ)
			{
				
			}
            
			return TsResult.OK;
        }
    }
}
