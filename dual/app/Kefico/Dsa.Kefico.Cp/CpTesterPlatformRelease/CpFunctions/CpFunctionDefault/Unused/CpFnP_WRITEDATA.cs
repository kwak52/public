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
	public class CpFnP_WRITEDATA : CpTsShell, IP_WRITEDATA
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_WRITEDATA psModuleP_READMEM = this.Core as PsCCSStdFnModuleP_WRITEDATA;
			Debug.Assert(psModuleP_READMEM != null);
			
			string strDataDevID = psModuleP_READMEM.DeviceID;
			string strDataWriteType = psModuleP_READMEM.WriteType;
			string strDataWriteMode = psModuleP_READMEM.WriteMode;
			string strDataInputData = psModuleP_READMEM.INPUTDATA;			
			string strDataTimeout = psModuleP_READMEM.TIMEOUT;	
        
			if(cpTsParent is IWRITE_DATA)
			{
		
			}

			return TsResult.OK;
        }
    }
}
