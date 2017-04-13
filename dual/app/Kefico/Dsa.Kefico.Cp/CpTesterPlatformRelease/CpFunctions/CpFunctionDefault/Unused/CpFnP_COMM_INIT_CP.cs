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

namespace CpTesterPlatform.Functions
{
	public class CpFnP_COMM_INIT_CP : CpTsShell, IP_COMM_INIT_CP
    {
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            var tResult = TryFunc(() =>
            {
                PsCCSStdFnModuleP_COMM_INIT_CP psModuleP_COMM_INIT_CP = this.Core as PsCCSStdFnModuleP_COMM_INIT_CP;
                Debug.Assert(psModuleP_COMM_INIT_CP != null);
				
				string strDeviceID = psModuleP_COMM_INIT_CP.DeviceID;
//                 string strDataTx = psModuleP_COMM_INIT_CP.DataTx;
//                 string strDataRx = psModuleP_COMM_INIT_CP.DataRx;
//                 string Timeout = psModuleP_COMM_INIT_CP.TimeOut;
//                 string strEOIonW = psModuleP_COMM_INIT_CP.EOIonW; // C = Continue, L = Last (get response message)
//                 string strEOSonR = psModuleP_COMM_INIT_CP.EOSonR;
// 				string strError_Flags = psModuleP_COMM_INIT_CP.Error_Flags;
// 				
				if(cpTsParent is ICOM_INIT_CP)
				{
					CpDeviceManagerBase baseDevMgr = cpMngSystem.MngHardware.GetDeviceManager(strDeviceID);

					if(baseDevMgr == null)
						return TsResult.NONE;

                    //if (strProtocol.ToUpper() == "GPIB")
                    //{
                        if (cpMngSystem.CnfSystem.HardwareActivation.ActiveHwFncGPIB == false) //Hardware Activation = False -> Simulation Mode						
                            return TsResult.NONE;

                        if (!(baseDevMgr is IGpibManager) || baseDevMgr.DeviceInstance == null)
                            return TsResult.NG;

                        /*
						((IGpibManager) baseDevMgr).SetCommConfig(strBoardAddress, strPortID_Pri, strPortID_Sec, strEOSonR, strEOIonW, strTimeout);

						if(((IGpibManager) baseDevMgr).CreateDevice(baseDevMgr.DeviceInfo) == false)
							return TsResult.NG;
						if(((IGpibManager) baseDevMgr).OpenDevice() == false)
							return TsResult.NG;
						*/

                    //}
					//수정 필요 -> 글로벌 변수에서 가져오도록.
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
