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
using System.Linq;
using System.Reflection;
using CpTesterPlatform.CpCommon.ResultLog;


namespace CpTesterPlatform.Functions
{
    public class CpFnP_CAL_PULSEW : CpTsShell, IP_CAL_PULSEW
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {

            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleP_CAL_PULSEW psModuleP_CAL_PULSEW = this.Core as PsCCSStdFnModuleP_CAL_PULSEW;
            Debug.Assert(psModuleP_CAL_PULSEW != null);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.ANALOG_INPUT)) return TsResult.SKIP;

            string strDevID = psModuleP_CAL_PULSEW.DeviceID;
            string strDataInput = psModuleP_CAL_PULSEW.INPUTDATA;
            ePEAK PEAK = psModuleP_CAL_PULSEW.PEAK;
            string strDataTrigger = psModuleP_CAL_PULSEW.TRIGGER;
            string strTimeMInterval = psModuleP_CAL_PULSEW.DMM_APER_TIME;
            string strDataDim = psModuleP_CAL_PULSEW.DIMENSION;
            string strDataResult = psModuleP_CAL_PULSEW.R_MESSWERT;
            string strTrigger = psModuleP_CAL_PULSEW.TRIGGER;


            return TryFunc(() =>
            {
                double dWidth;
                List<double> vdData = (List<double>)iMngStation.MngTStep.GetMngGv().GetValue(strDataInput).RawValue;
                if (vdData == null)
                    return TsResult.ERROR;
                double[] vdDataArray = vdData.ToArray();

                (cpTsParent.ResultLog as ClsRlMeasuring).MeasuredDataArray = vdData.ToList();
                (cpTsParent.ResultLog as ClsRlMeasuring).SamplingRate = Convert.ToDouble(strTimeMInterval);
                if (CpUtil.UsingLimeDAQ)
                {
                    if (PEAK == ePEAK.HIGH)
                        dWidth = CpUtilDaq.GetWidthTime(vdDataArray, Convert.ToDouble(strTimeMInterval), Convert.ToDouble(strDataTrigger), false, strDataInput, false);
                    else if (PEAK == ePEAK.LOW)         
                        dWidth = CpUtilDaq.GetWidthTime(vdDataArray, Convert.ToDouble(strTimeMInterval), Convert.ToDouble(strDataTrigger), true, strDataInput, false);
                    else
                        return TsResult.ERROR;
                }
                else
                {
                    double dMean = CpMathLib.GetAverage(vdData);
                    bool bIsHigh = false;

                    if (PEAK == ePEAK.HIGH)
                        bIsHigh = true;
                    else if (PEAK == ePEAK.LOW)
                        bIsHigh = false;
                    else
                        return TsResult.ERROR;

                    List<KeyValuePair<int, int>> vEdgeIndexResult = CpMathLib.GetEdgeList(vdData, dMean);
                    dWidth = CpMathLib.GetAverageWidth(vEdgeIndexResult, bIsHigh, Convert.ToDouble(strTimeMInterval));
                }

                CpUtilDaq.CreateDaqChartEvent(iMngStation.StationId, cpTsParent, vdDataArray);
                iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = Math.Round(dWidth, 10);

                return TsResult.OK;

            }).Result;
        }

        public double GetResult()
        {
            return 0.0;
        }
    }
}
