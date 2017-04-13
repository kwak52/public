using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using Dsu.Driver.Math;
using System.IO;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using PsKGaudi.Parser.PsCCSSTDFn;

namespace CpTesterPlatform.Functions
{

    public static class CpUtilDaq
    {
        public static bool UsingLimeDAQ = true;
        public static int DaqError = 999;
        public static Dictionary<string, DaqSquareWave> DicDaqWave = new Dictionary<string, DaqSquareWave>();

        public static void CreateDaqChartEvent(int IndexStation, CpTsShell cpTsParent, double[] vdDataArray)
        {
            var PairedParms = cpTsParent.Core.GetModuleList().Where(w => w.STDKeficoName == "M_AUSWERTUNG").FirstOrDefault();
            double Max = PairedParms.PairedParmsMinMax.Max != "" ? Convert.ToDouble(PairedParms.PairedParmsMinMax.Max) : 0;
            double Min = PairedParms.PairedParmsMinMax.Min != "" ? Convert.ToDouble(PairedParms.PairedParmsMinMax.Min) : 0;
            DaqChartEvent.DaqSubject.OnNext(new DAQResult(IndexStation, cpTsParent.Core.GetMO(), vdDataArray, Min, Max));
        }

        public static double GetWidthTime(double[] vdData, double secInterval, double trigger, bool bLowWidth, string aiName, bool NewDaqSqWave)
        {
            DaqSquareWave daqSqWave;
            if (NewDaqSqWave)
            {
                daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
                SaveDaqWave(aiName, daqSqWave);
            }
            else
            {
                daqSqWave = LoadDaqSuareWave(aiName);
                if (daqSqWave == null || daqSqWave.IsSucceeded)
                    daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
            }

            if(daqSqWave == null || !daqSqWave.IsSucceeded)
                return DaqError / 100000;

            double duty = 0;
            if (bLowWidth)
                duty = 1 - daqSqWave.Duty;
            else
                duty = daqSqWave.Duty;

            return (duty * ((double)(daqSqWave.NumHighSamples + daqSqWave.NumLowSamples) / daqSqWave.NumRisingEdges) * daqSqWave.IntervalTime); 
        }

        public static double GetHighV(double[] vdData, double secInterval, double trigger, string aiName, bool NewDaqSqWave)
        {
            DaqSquareWave daqSqWave;
            if (NewDaqSqWave)
            {
                daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
                SaveDaqWave(aiName, daqSqWave);
            }
            else
            {
                daqSqWave = LoadDaqSuareWave(aiName);
                if (daqSqWave == null || !daqSqWave.IsSucceeded)
                    daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
            }

            if (daqSqWave == null|| !daqSqWave.IsSucceeded)
                return DaqError;

            return daqSqWave.HighAverage;
        }

        public static double GetLowV(double[] vdData, double secInterval, double trigger, string aiName, bool NewDaqSqWave)
        { 
            DaqSquareWave daqSqWave;
            if (NewDaqSqWave)
            {
                daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
                SaveDaqWave(aiName, daqSqWave);
            }
            else
            {
                daqSqWave = LoadDaqSuareWave(aiName);
                if (daqSqWave == null ||!daqSqWave.IsSucceeded)
                    daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
            }

            if (daqSqWave == null|| !daqSqWave.IsSucceeded)
                return DaqError;

            return daqSqWave.LowAverage;
        }

        public static double GetDuty(double[] vdData, double secInterval, double trigger, string aiName, bool NewDaqSqWave)
        {
            DaqSquareWave daqSqWave;
            if (NewDaqSqWave)
            {
                daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
                SaveDaqWave(aiName, daqSqWave);
            }
            else
            {
                daqSqWave = LoadDaqSuareWave(aiName);
                if (daqSqWave == null || !daqSqWave.IsSucceeded)
                    daqSqWave = GetDaqSquareWave(vdData, secInterval, trigger);
            }

            if (daqSqWave == null|| !daqSqWave.IsSucceeded)
                return DaqError;

            return daqSqWave.Duty;
        }

        private static DaqSquareWave LoadDaqSuareWave(string aiName)
        {
            if (DicDaqWave.ContainsKey(aiName))
                return DicDaqWave[aiName];
            else
                return null;
        }

        private static void SaveDaqWave(string aiName, DaqSquareWave daqSqWave)
        {
            if (DicDaqWave.ContainsKey(aiName))
                DicDaqWave[aiName] = daqSqWave;
            else
                DicDaqWave.Add(aiName, daqSqWave);
        }

        public static void SaveDAQData(double[] vdData, string comment = "")
        {
            Task.Run(() =>
            {
                string strCrtTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                FileStream filestreamOutput = new FileStream("Trace_" + comment + strCrtTime + "_DaqSquareWave.txt", FileMode.OpenOrCreate);
                StreamWriter streamwriterOutput = new StreamWriter(filestreamOutput);
                for (int i = 0; i < vdData.Length; i++)
                    streamwriterOutput.WriteLine(vdData[i]);
                streamwriterOutput.AutoFlush = true;
                filestreamOutput.Close();
            });
        }

        private static DaqSquareWaveDecisionParameters _defaultDaqDecisionParameters = new DaqSquareWaveDecisionParameters()
        {
            TrimRatioFront = 0.1,
            TrimRatioRear = 0.1
        };

        public static DaqSquareWave GetDaqSquareWave(double[] vdData, double secInterval, double trigger)
        {
            var oResult = TryFunc(() =>
            {
                var parameters = _defaultDaqDecisionParameters.Clone();
                return new DaqSquareWave(parameters, vdData, 1 / secInterval * 1000, trigger);
            });

            if (oResult.HasException)
            {
                return null;
            }
            return oResult.Result;
        }


        //Simulation data
        public static List<double> GetDataArraySample()
        {
            List<double> vdData = new List<double>();
            for (int i = 0; i < 10000; i++)
            {
                if (i / 10 % 2 == 0)
                    vdData.Add(1.5);
                else
                    vdData.Add(0.7);
            }

            return vdData;
        }



    }
}
