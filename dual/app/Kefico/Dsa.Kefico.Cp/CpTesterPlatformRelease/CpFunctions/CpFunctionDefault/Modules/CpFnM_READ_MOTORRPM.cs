using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using CpTesterPlatform.CpMngLib.Manager;
using static CpCommon.ExceptionHandler;
using System.Reflection;

namespace CpTesterPlatform.Functions
{
    public class CpFnM_READ_MOTORRPM : CpTsShell, IM_READ_MOTORRPM
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.MOTION)) return TsResult.SKIP;
            PsCCSStdFnModuleM_READ_MOTORRPM psModuleM_READ_MOTORRPM = this.Core as PsCCSStdFnModuleM_READ_MOTORRPM;
            Debug.Assert(psModuleM_READ_MOTORRPM != null);

            CpMngMotion mngMotion = cpMngSystem.MngHardware.GetDeviceManager(psModuleM_READ_MOTORRPM.DeviceID) as CpMngMotion;

            if (mngMotion == null)
                return TsResult.ERROR;

            TryResultT<TsResult> oResult = TryFunc(() =>
            {
                var curRPM = mngMotion.GetCurrentRpm();

                if (string.IsNullOrEmpty(psModuleM_READ_MOTORRPM.DEVICE_OUTPUT) || psModuleM_READ_MOTORRPM.AXIS_ID != "-")
                    iMngStation.MngTStep.GetMngGv().SetValue(psModuleM_READ_MOTORRPM.DEVICE_OUTPUT, Math.Round(curRPM, 4), PsCommon.CpStringFormat.FLOAT);

                iMngStation.MngTStep.GetMngGv().SetValue(psModuleM_READ_MOTORRPM.DEVICE_OUTPUT, curRPM, PsCommon.CpStringFormat.FLOAT);

                return TsResult.OK;
            });

            if (oResult.HasException)
                return TsResult.ERROR;
            return oResult.Result;
        }
    }
}
