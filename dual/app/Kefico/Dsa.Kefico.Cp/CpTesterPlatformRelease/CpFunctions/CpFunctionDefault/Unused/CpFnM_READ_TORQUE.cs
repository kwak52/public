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
	public class CpFnM_READ_TORQUE : CpTsShell, IM_READ_TORQUE
    {
		public class CpFnP_SET_TORQUE : CpTsShell, IP_SET_TORQUE
		{
			protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
			{		
				PsCCSStdFnModuleM_READ_TORQUE psModuleReadTorque = this.Core as PsCCSStdFnModuleM_READ_TORQUE;
				Debug.Assert(psModuleReadTorque != null);
			
				string strDevID = psModuleReadTorque.DeviceID;
                eMESS_MODUS MESS_MODUS = psModuleReadTorque.MESS_MODUS;
				string strInterval = psModuleReadTorque.INTERVAL;
				string strDuration = psModuleReadTorque.DURATION;
				string strDevOutput = psModuleReadTorque.DEVICE_OUTPUT;
				string strTimeout = psModuleReadTorque.TIMEOUT;
				string strDataDim = psModuleReadTorque.DIMENSION;
				string strEvalResult = psModuleReadTorque.R_MESSWERT;
								            
				return TsResult.NONE;
			}		
		}
    }
}
