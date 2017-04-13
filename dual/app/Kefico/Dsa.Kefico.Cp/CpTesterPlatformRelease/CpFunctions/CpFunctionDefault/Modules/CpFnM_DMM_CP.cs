using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System.Threading;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpMngLib.BaseClass;
using PsCommon.Enum;
using CpTesterPlatform.CpTStepDev.Interface;
using System.Reflection;
using CpTesterPlatform.CpCommon.ResultLog;

namespace CpTesterPlatform.Functions
{
    public class CpFnM_DMM_CP : CpTsShell, IM_DMM_CP
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleM_DMM_CP psModuleM_DMM_CP = this.Core as PsCCSStdFnModuleM_DMM_CP;
            Debug.Assert(psModuleM_DMM_CP != null);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.ANALOG_INPUT)) return TsResult.SKIP;
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.POWER_SUPPLY)) return TsResult.SKIP;

            string strDataDeviceID = psModuleM_DMM_CP.DeviceID;
            string strDataMRange = psModuleM_DMM_CP.DMM_RANGE;
            string strDataMTime = psModuleM_DMM_CP.DMM_DURATION;
            string strInterval = psModuleM_DMM_CP.INTERVAL;
            string strDataTrgSrc = psModuleM_DMM_CP.TRG_SRC;
            string strDataTrgCnt = psModuleM_DMM_CP.DMM_TRG_COUNT;
            string strDataTrgDelay = psModuleM_DMM_CP.DMM_TRG_DELAY;
            eDMM_AUTOZERO DMM_AUTOZERO = psModuleM_DMM_CP.DMM_AUTOZERO;
            string strDataSmpSrc = psModuleM_DMM_CP.DMM_SMP_SOURCE;
            string strDataSmpCnt = psModuleM_DMM_CP.DMM_SMP_COUNT;
            string strDataSmpPeriod = psModuleM_DMM_CP.DMM_SMP_PERIOD;
            eDMM_OFFSET_COM DMM_OFFSET_COM = psModuleM_DMM_CP.DMM_OFFSET_COM;
            string strDataMask = psModuleM_DMM_CP.DMM_MASKE;
            string strDataTimeout = psModuleM_DMM_CP.DMM_TIMEOUT;
            eMESS_MODUS MESS_MODUS = psModuleM_DMM_CP.MESS_MODUS;
            string strDataResult = psModuleM_DMM_CP.R_MESSWERT;
            string strDataDim = psModuleM_DMM_CP.DIMENSION;


            IDevManager MngDev = (IDevManager)cpMngSystem.MngHardware.GetDeviceManager(strDataDeviceID);
            if (MngDev is IAnalogInputManager)
            {
                CpMngAIControl mngAIControl = (CpMngAIControl)MngDev;
                IAnalogInput analogInput = mngAIControl.DeviceInstance as IAnalogInput;

                double[] vresult = null;
                if (MESS_MODUS == eMESS_MODUS.M)
                {
                    vresult = analogInput.GetPeriodicV(Convert.ToDouble(strDataMTime) / 1000.0, 1 / Convert.ToDouble(strInterval) * 1000).Result;
                    if (vresult.Where(d => d == 0).Count() / vresult.Count() > 10)
                    {
                        analogInput.ReturnBuffer(vresult);
                        vresult = analogInput.GetPeriodicV(Convert.ToDouble(strDataMTime) / 1000.0, 1 / Convert.ToDouble(strInterval) * 1000).Result;
                    }

                    iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = vresult.ToList();
                }
                else if (MESS_MODUS == eMESS_MODUS.NORM)
                {
                    vresult = analogInput.GetPeriodicV(1, 1 / Convert.ToDouble(strInterval) * 1000).Result;
                    iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = vresult.Average();
                }

                analogInput.ReturnBuffer(vresult);
            }
            else if (MngDev is IPowerSupplyManager)
            {
                CpMngPowerSupply mngPowerSupply = (CpMngPowerSupply)MngDev;

                if (MESS_MODUS == eMESS_MODUS.NORM)
                {
                    double vresult = mngPowerSupply.GetObservedVoltage();
                    iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = vresult;
                }
            }

            return TsResult.NONE;
        }
    }
}
