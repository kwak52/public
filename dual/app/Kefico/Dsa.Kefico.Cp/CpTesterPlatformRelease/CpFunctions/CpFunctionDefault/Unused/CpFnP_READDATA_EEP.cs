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
	public class CpFnP_READDATA_EEP : CpTsShell, IP_READDATA_EEP
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_READDATA_EEP psModuleP_READMEM = this.Core as PsCCSStdFnModuleP_READDATA_EEP;
			Debug.Assert(psModuleP_READMEM != null);

			string strDataDevID = psModuleP_READMEM.DeviceID;
			string strDataReadType = psModuleP_READMEM.ReadType;
			string strDataDieCh = psModuleP_READMEM.DieChannel;
			string strDataParmCode = psModuleP_READMEM.EEPROM_ParmeterCode;			
			string strDataOutput = psModuleP_READMEM.DEVICE_OUTPUT;			
			string strDataTimeout = psModuleP_READMEM.TIMEOUT;
															
			if(cpTsParent is IREAD_EEP_DATA)
			{
		
			}
            
			return TsResult.OK;
        }
    }
}
