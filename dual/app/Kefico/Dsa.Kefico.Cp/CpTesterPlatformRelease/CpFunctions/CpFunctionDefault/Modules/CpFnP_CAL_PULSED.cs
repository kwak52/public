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
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpMngLib.Manager;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using PsKGaudi.Parser.PsCCSSTDFn;

namespace CpTesterPlatform.Functions
{
    public class CpFnP_CAL_PULSED : CpTsShell, IP_CAL_PULSED
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleP_CAL_PULSED psModuleP_CAL_PULSED = this.Core as PsCCSStdFnModuleP_CAL_PULSED;
            Debug.Assert(psModuleP_CAL_PULSED != null);
            if (!CpUtil.CheckEnableDevice(cpMngSystem, CpDeviceType.ANALOG_INPUT)) return TsResult.SKIP;

            string strDevID = psModuleP_CAL_PULSED.DeviceID;
            string strDataInput = psModuleP_CAL_PULSED.INPUTDATA;
            ePEAK PEAK = psModuleP_CAL_PULSED.PEAK;
            string strDataTrigger = psModuleP_CAL_PULSED.TRIGGER;
            string strTimeMInterval = psModuleP_CAL_PULSED.DMM_APER_TIME;
            string strDataDim = psModuleP_CAL_PULSED.DIMENSION;
            string strDataResult = psModuleP_CAL_PULSED.R_MESSWERT;

            var result = TsResult.ERROR;
            TryAction(() =>
            {
                List<double> vdData = iMngStation.MngTStep.GetMngGv().GetValue(strDataInput).RawValue as List<double>;
                if (vdData == null)
                    return;
                double[] vdDataArray = vdData.ToArray();


                double duty;
                (cpTsParent.ResultLog as ClsRlMeasuring).MeasuredDataArray = vdData;
                (cpTsParent.ResultLog as ClsRlMeasuring).SamplingRate = Convert.ToDouble(strTimeMInterval);
                if (CpUtil.UsingLimeDAQ)
                {
                    if (PEAK == ePEAK.LOW)
                        duty = (1 - CpUtilDaq.GetDuty(vdDataArray, Convert.ToDouble(strTimeMInterval), Convert.ToDouble(strDataTrigger), strDataInput, false)) * 100;
                    else
                        duty = CpUtilDaq.GetDuty(vdDataArray, Convert.ToDouble(strTimeMInterval), Convert.ToDouble(strDataTrigger), strDataInput, false) * 100;
                }
                else
                {
                    double dMean = CpMathLib.GetAverage(vdData);
                    List<KeyValuePair<int, int>> vEdgeIndexResult = CpMathLib.GetEdgeList(vdData, dMean);

                    if (PEAK == ePEAK.LOW)
                        duty = 100 - CpMathLib.GetAverageDuty(vEdgeIndexResult);
                    else
                        duty = CpMathLib.GetAverageDuty(vEdgeIndexResult);
                }

                CpUtilDaq.CreateDaqChartEvent(iMngStation.StationId, cpTsParent, vdDataArray);
                iMngStation.MngTStep.GetMngGv().GetValue(strDataResult).RawValue = Math.Round(duty, 4);
                result = TsResult.OK;
            });
            return result;
        }

    

        public double GetResult()
        {
            return 0.0;
        }
    }
}
