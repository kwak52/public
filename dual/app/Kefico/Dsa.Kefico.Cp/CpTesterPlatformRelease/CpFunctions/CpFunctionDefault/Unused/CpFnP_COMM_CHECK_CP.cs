using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
	public class CpFnP_COMM_CHECK_CP : CpTsShell, IP_COMM_CHECK_CP
    {
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            var tResult = TryFunc(() =>
            {
                PsCCSStdFnModuleP_COMM_CHECK_CP psModuleP_COMM_CHECK_CP = this.Core as PsCCSStdFnModuleP_COMM_CHECK_CP;
                Debug.Assert(psModuleP_COMM_CHECK_CP != null);

				string strDeviceID = psModuleP_COMM_CHECK_CP.DeviceID;
                string strProtocol = psModuleP_COMM_CHECK_CP.PROTOKOLL;
                string strResultGv = psModuleP_COMM_CHECK_CP.ERGEBNIS;
                string strErrorFlagsGv = psModuleP_COMM_CHECK_CP.ERRORFLAG;

				if(cpTsParent is ICOM_INIT)
				{
					if(cpMngSystem.CnfSystem.HardwareActivation.ActiveHwFncGPIB == false) //Hardware Activation = False -> Simulation Mode						
					{
						iMngStation.MngTStep.GetMngGv().SetValue(strResultGv, ((int) BIN_BOOL_TYPE.TRUE).ToString(), CpStringFormat.BIN);	
						return TsResult.NONE;
					}

					if(cpMngSystem.MngHardware.DicCommDeviceManager.ContainsKey(strDeviceID) == false) //Hardware Activation = False -> Simulation Mode
					{
						iMngStation.MngTStep.GetMngGv().SetValue(strResultGv, ((int) BIN_BOOL_TYPE.FALSE).ToString(), CpStringFormat.BIN);	
						return TsResult.NG;
					}

					if(cpMngSystem.MngHardware.DicCommDeviceManager[strDeviceID] is IGpibManager && strProtocol.ToUpper() == "GPIB")
					{
						iMngStation.MngTStep.GetMngGv().SetValue(strResultGv, ((int) BIN_BOOL_TYPE.TRUE).ToString(), CpStringFormat.BIN);

						return TsResult.OK;
					}
					else
					{
						iMngStation.MngTStep.GetMngGv().SetValue(strErrorFlagsGv, "Cannot find a corresponding device with the ID.", CpStringFormat.ASCII);
						iMngStation.MngTStep.GetMngGv().SetValue(strResultGv, ((int) BIN_BOOL_TYPE.FALSE).ToString(), CpStringFormat.BIN);
						return TsResult.NG;
					}					
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
