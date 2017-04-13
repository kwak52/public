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
	public class CpFnP_SET_TORQUE : CpTsShell, IP_SET_TORQUE
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {		
            PsCCSStdFnModuleP_SET_TORQUE psModuleP_SET_TORQUE = this.Core as PsCCSStdFnModuleP_SET_TORQUE;
			Debug.Assert(psModuleP_SET_TORQUE != null);
			
			string strDevID = psModuleP_SET_TORQUE.DeviceID;
			string strInputTorque = psModuleP_SET_TORQUE.INPUT_TORQUE;
            eHOLD_MODE HOLD_MODE = psModuleP_SET_TORQUE.HOLD_MODE;
			string strTimeout = psModuleP_SET_TORQUE.TIMEOUT;
			string strDataDim = psModuleP_SET_TORQUE.DIMENSION;
			string strDataResult = psModuleP_SET_TORQUE.R_MESSWERT;
								            
			return TsResult.OK;
        }		
    }
}
