using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using CpTesterPlatform.CpMngLib.Manager;
using PsCommon.Enum;
using PsCommon;
using System.Reflection;
using System.Threading;

namespace CpTesterPlatform.Functions
{
	public class CpFnM_DLCRM : CpTsShell, IM_DLCRM
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleM_DLCRM psModuleM_DLCRM = this.Core as PsCCSStdFnModuleM_DLCRM;
			Debug.Assert(psModuleM_DLCRM != null);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.LCRMETER)) return TsResult.SKIP;


            string strDataDeviceID = psModuleM_DLCRM.DeviceID;
            eLCR_MODE LCR_MODE = psModuleM_DLCRM.LCR_MODE;
            eMESS_MODUS MESS_MODUS = psModuleM_DLCRM.MESS_MODUS;
            string strDataMRange = psModuleM_DLCRM.RANGE;
            string strDataITime = psModuleM_DLCRM.INTERVAL;
            string strDataMTime = psModuleM_DLCRM.DURATION;
            string strDataDeviceOutput = psModuleM_DLCRM.DEVICE_OUTPUT;
            string strDataTimeout = psModuleM_DLCRM.TIMEOUT;
            string strDataDim = psModuleM_DLCRM.DIMENSION;
            string strDataResult = psModuleM_DLCRM.R_MESSWERT;

            ILCRMeterManager LCRMng = ((ILCRMeterManager)cpMngSystem.MngHardware.GetDeviceManager(strDataDeviceID.Split(';')[0]));

            if (strDataDeviceID.Contains(";"))
            {
                ((CpMngLCRMeter)LCRMng).SetSettingFile(Convert.ToInt16(strDataDeviceID.Split(';')[1]));
                Thread.Sleep(800);
            }

            if (LCRMng.IsOpened == false)
                return TsResult.NONE;

            double dResult = 0.0;
            if (LCR_MODE == eLCR_MODE.CAP)
            {
                dResult = LCRMng.GetCapicatance();
            }
            else if (LCR_MODE == eLCR_MODE.RES)
            {
                dResult = LCRMng.GetResistance();

                double dim = 1;

                if (strDataDim == "GOHM")
                    dim = dim * 0.000001;
                if (strDataDim == "MEGAOHM")
                    dim = dim * 0.001;
                else if (strDataDim == "OHM")
                    dim = dim * 1;

                dResult = dResult * dim;

                if(dResult == 0)
                    return TsResult.NONE;

            }

            iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = Math.Round(dResult, 11);

            return TsResult.NONE;
        }
    }
}








