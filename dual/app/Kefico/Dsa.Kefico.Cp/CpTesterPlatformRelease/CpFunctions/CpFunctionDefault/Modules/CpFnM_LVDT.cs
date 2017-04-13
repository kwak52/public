using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System.Reflection;
using CpTesterPlatform.CpMngLib.Manager;

namespace CpTesterPlatform.Functions
{
    public class CpFnM_LVDT : CpTsShell, IM_LVDT
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleM_LVDT psModuleM_LVDT = this.Core as PsCCSStdFnModuleM_LVDT;
            Debug.Assert(psModuleM_LVDT != null);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.LVDT)) return TsResult.SKIP;
            string returnValue = psModuleM_LVDT.R_MESSWERT;
            string offsetValue = psModuleM_LVDT.OFFSET;
            double plcOffset = 0;
            CpMngLVDT lvdtmng = (CpMngLVDT)cpMngSystem.MngHardware.GetDeviceManager(psModuleM_LVDT.DeviceID);
            if (psModuleM_LVDT.DeviceID == "FD0")
                plcOffset = CpUtil.FD_INPUT_OFFSET;
            else if (psModuleM_LVDT.DeviceID == "FD1")
                plcOffset = CpUtil.FD_OUTPUT_OFFSET;
            else if (psModuleM_LVDT.DeviceID == "FD2")
                plcOffset = CpUtil.FD_MIDDLE_OFFSET;

            if (offsetValue == "") offsetValue = "0";

             iMngStation.MngTStep.GetMngGv().GetValue(returnValue).RawValue = Math.Round(lvdtmng.GetFuntionDimension(), 4) + Convert.ToDouble(offsetValue) + plcOffset;
            //iMngStation.MngTStep.GetMngGv().GetValue(returnValue).RawValue = CpUtil.GetStationindex(0);

            return TsResult.NONE;
        }
    }
}
