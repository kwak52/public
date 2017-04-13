using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using PsCommon.Enum;

namespace CpTesterPlatform.Functions
{
	public class CpFnM_COUNTER_CP : CpTsShell, IM_COUNTER_CP
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleM_COUNTER_CP psModuleM_Cnt_CP = this.Core as PsCCSStdFnModuleM_COUNTER_CP;
			Debug.Assert(psModuleM_Cnt_CP != null);

			string strDataDeviceID = psModuleM_Cnt_CP.DeviceID;
            eCOUNT_MODE COUNT_MODE = psModuleM_Cnt_CP.COUNT_MODE;
            eMESS_MODUS MESS_MODUS = psModuleM_Cnt_CP.MESS_MODUS;
			string strDataTrg = psModuleM_Cnt_CP.SCANN_UNIT_TRG;			
			string strDataMTime = psModuleM_Cnt_CP.DURATION;
			string strDataITime = psModuleM_Cnt_CP.INTERVAL;
			string strDataResult = psModuleM_Cnt_CP.DEVICE_OUTPUT;
			string strDataTimeout = psModuleM_Cnt_CP.TIMEOUT;			
			string strDataDim = psModuleM_Cnt_CP.DIMENSION;
			string strEvalData = psModuleM_Cnt_CP.R_MESSWERT;
											            
			return TsResult.NONE;
        }
		
    }
}
