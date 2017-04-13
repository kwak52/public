using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;
using PsCommon.Enum;

namespace CpTesterPlatform.Functions
{
	public class CpFnP_TRANSMIT_CP : CpTsShell, IP_TRANSMIT_CP
    {        
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            var tResult = TryFunc(() =>
            {
                PsCCSStdFnModuleP_TRANSMIT_CP psModuleP_TRANSMIT_CP = this.Core as PsCCSStdFnModuleP_TRANSMIT_CP;
                Debug.Assert(psModuleP_TRANSMIT_CP != null);

				string strDeviceID = psModuleP_TRANSMIT_CP.DeviceID;
                string strDataTx = psModuleP_TRANSMIT_CP.DataTx;
                string strDataRx = psModuleP_TRANSMIT_CP.DataRx;
                string Timeout = psModuleP_TRANSMIT_CP.TIMEOUT;
                eEOIonW EOIonW = psModuleP_TRANSMIT_CP.EOIonW; // C = Continue, L = Last (get response message)
                eEOSonR EOSonR = psModuleP_TRANSMIT_CP.EOSonR;
				string strError_Flags = psModuleP_TRANSMIT_CP.ERRORFLAG;

                if (cpTsParent is ICOMBLOCK_CP)
                {	
 					CpDeviceManagerBase baseDevMgr = cpMngSystem.MngHardware.GetCommDeviceManager(strDeviceID);
 
 					if(baseDevMgr == null)
 						return TsResult.OK;
 
 					baseDevMgr.TransmitData(strDataTx);
 					
 					if(((IDevManager) baseDevMgr).ControlState == DeviceControlState.Error)
 						return TsResult.NG;
 
 					if(iMngStation.MngTStep.GetMngGv().IsKindOfVariable(strDataRx))
 						iMngStation.MngTStep.GetMngGv().ModifyData(strDataRx, new CpGlbVarBase((string) cpMngSystem.MngHardware.GetCommDeviceManager(strDeviceID).Result()));					
				}
                
                return TsResult.OK;
            });

            if (tResult.HasException)
            {
                this.ResultLog.TsActionResult = TsResult.ERROR;
                return TsResult.ERROR;
            }

            return tResult.Result;
        }
    }
}
