using System;
using System.Linq;
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
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpMngLib.Manager;
using System.Reflection;
using CpTesterPlatform.CpCommon.ResultLog;

namespace CpTesterPlatform.Functions
{
    public class CpFnP_CAL_PULSEV : CpTsShell, IP_CAL_PULSEV
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleP_CAL_PULSEV psModuleP_CAL_PULSEV = this.Core as PsCCSStdFnModuleP_CAL_PULSEV;

            Debug.Assert(psModuleP_CAL_PULSEV != null);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.ANALOG_INPUT)) return TsResult.SKIP;

            string strDevID = psModuleP_CAL_PULSEV.DeviceID;
            string strDataInput = psModuleP_CAL_PULSEV.INPUTDATA;
            ePEAK PEAK = psModuleP_CAL_PULSEV.PEAK;
            string strDataTrigger = psModuleP_CAL_PULSEV.TRIGGER;
            string strTimeMInterval = psModuleP_CAL_PULSEV.DMM_APER_TIME;
            string strDataDim = psModuleP_CAL_PULSEV.DIMENSION;
            string strDataResult = psModuleP_CAL_PULSEV.R_MESSWERT;

            return TryFunc(() =>
            {
                if (iMngStation.MngTStep.GetMngGv().GetValue(strDataInput).RawValue.ToString() == "0")
                    return TsResult.ERROR;

                double dVotage;
                List<double> vdData = (List<double>)iMngStation.MngTStep.GetMngGv().GetValue(strDataInput).RawValue;
                if (vdData == null)
                    return TsResult.ERROR;
                double[] vdDataArray = vdData.ToArray();

                (cpTsParent.ResultLog as ClsRlMeasuring).MeasuredDataArray = vdData;
                (cpTsParent.ResultLog as ClsRlMeasuring).SamplingRate = Convert.ToDouble(strTimeMInterval);
                if (CpUtil.UsingLimeDAQ)
                {
                    if (PEAK == ePEAK.HIGH)
                        dVotage = CpUtilDaq.GetHighV(vdDataArray, Convert.ToDouble(strTimeMInterval), Convert.ToDouble(strDataTrigger), strDataInput, true);
                    else if (PEAK == ePEAK.LOW)      
                        dVotage = CpUtilDaq.GetLowV(vdDataArray, Convert.ToDouble(strTimeMInterval), Convert.ToDouble(strDataTrigger), strDataInput, false);
                    else
                        return TsResult.ERROR;
                }
                else
                {
                    double dMean = CpMathLib.GetAverage(vdData);
                    bool bIsHighV = false;

                    if (PEAK == ePEAK.HIGH)
                        bIsHighV = true;
                    else if (PEAK == ePEAK.LOW)
                        bIsHighV = false;
                    else
                        return TsResult.ERROR;

                    dVotage = CpMathLib.FilterData(vdData.ToList(), dMean, bIsHighV);
                }
                CpUtilDaq.CreateDaqChartEvent(iMngStation.StationId, cpTsParent,vdDataArray);
                iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = Math.Round(dVotage, 4);

                return TsResult.OK;
            }).Result;
        }

        public double GetResult()
        {
            return 0.0;
        }
    }
}
