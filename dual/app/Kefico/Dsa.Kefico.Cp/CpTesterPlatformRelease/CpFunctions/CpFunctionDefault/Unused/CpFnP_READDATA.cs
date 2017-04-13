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
	public class CpFnP_READDATA : CpTsShell, IP_READDATA
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_READDATA psModuleP_READMEM = this.Core as PsCCSStdFnModuleP_READDATA;
			Debug.Assert(psModuleP_READMEM != null);

			string strDataDevID = psModuleP_READMEM.DeviceID;
			string strDataReadType = psModuleP_READMEM.ReadType;
			string strDataOutput = psModuleP_READMEM.DEVICE_OUTPUT;
			string strDataStartAddr = psModuleP_READMEM.StartAddr;			
			string strDataEndAddr = psModuleP_READMEM.EndAddr;			
			string strDataTimeout = psModuleP_READMEM.TIMEOUT;
															
															
			if(cpTsParent is IREAD_DATA)
			{
				
			}
            
			return TsResult.OK;
        }
    }
}
