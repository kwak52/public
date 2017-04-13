using System;
using System.Collections.Generic;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.Mathmatics;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;
using PsCommon.Enum;
using System.Reflection;

namespace CpTesterPlatform.Functions
{
    public class CpFnP_CAL_PULSEEDGE : CpTsShell, IP_CAL_PULSEEDGE
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleP_CAL_PULSEEDGE psModuleP_CAL_PULSEW = this.Core as PsCCSStdFnModuleP_CAL_PULSEEDGE;
            Debug.Assert(psModuleP_CAL_PULSEW != null);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.ANALOG_INPUT)) return TsResult.SKIP;

            string strDevID = psModuleP_CAL_PULSEW.DeviceID;
            string strDataInput = psModuleP_CAL_PULSEW.INPUTDATA;
            string strEdgeIndex = psModuleP_CAL_PULSEW.EDGE_INDEX;
            string strDataTrigger = psModuleP_CAL_PULSEW.TRIGGER;
            eEDGE_COUNT_MODE EDGE_COUNT_MODE = psModuleP_CAL_PULSEW.EDGE_COUNT_MODE;
            string strDataDim = psModuleP_CAL_PULSEW.DIMENSION;
            string strDataResult = psModuleP_CAL_PULSEW.R_MESSWERT;

            TryAction(() =>
            {
                int nTargetIdx = Convert.ToInt32(strEdgeIndex);
                List<double> vdData = (List<double>)iMngStation.MngTStep.GetMngGv().GetValue(strDataInput).RawValue;
                double dMean = CpMathLib.GetAverage(vdData);
                bool bIsHigh = false;
                bool bIsLow = false;

                if (EDGE_COUNT_MODE == eEDGE_COUNT_MODE.High)
                    bIsHigh = true;
                if (EDGE_COUNT_MODE == eEDGE_COUNT_MODE.Low)
                    bIsLow = true;

                List<KeyValuePair<int, int>> vEdgeIndexResult = CpMathLib.GetEdgeList(vdData, dMean);

                iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = CpMathLib.GetEdgeDetectedTime(vEdgeIndexResult, nTargetIdx, bIsHigh, bIsLow);
            });
            return TsResult.OK;
        }
    }
}
