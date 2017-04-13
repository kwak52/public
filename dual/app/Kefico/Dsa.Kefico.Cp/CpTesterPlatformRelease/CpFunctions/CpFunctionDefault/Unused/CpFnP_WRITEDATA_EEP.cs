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
	public class CpFnP_WRITEDATA_EEP : CpTsShell, IP_WRITEDATA_EEP 
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_WRITEDATA_EEP psModuleP_READMEM = this.Core as PsCCSStdFnModuleP_WRITEDATA_EEP;
			Debug.Assert(psModuleP_READMEM != null);
			
			string strDataDevID = psModuleP_READMEM.DeviceID;
			string strDataWriteType = psModuleP_READMEM.WriteType;
			string strDataDieCh = psModuleP_READMEM.DieChannel;
			string strDataCalParm = psModuleP_READMEM.Calculation_Parm;			
			string strDataDatParmFirst = psModuleP_READMEM.Data_Parm_First;	
			string strDataDataParmLevel = psModuleP_READMEM.Data_Parm_Level;
			string strDataParmReadFreq = psModuleP_READMEM.PWM_Parm_Read_Freq;
			string strDataParmReadCode = psModuleP_READMEM.PWM_Parm_Read_Code;
			string strDataParmChangeFreq = psModuleP_READMEM.PWM_Parm_Change_Freq;			
			string strDataMemoryLoc = psModuleP_READMEM.UserID_or_Memory_Loc_EEPROM_Code;	
			string strDataTimeout = psModuleP_READMEM.TIMEOUT;	
				        
			if(cpTsParent is IWRITE_EEP_DATA)
			{
		
			}
            
			return TsResult.OK;
        }
    }
}
