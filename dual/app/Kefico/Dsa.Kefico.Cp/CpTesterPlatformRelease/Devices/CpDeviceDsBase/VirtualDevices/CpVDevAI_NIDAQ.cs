using CpTesterPlatform.CpTStepDev.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using System.Threading;
using static CpCommon.ExceptionHandler;
using System.Reflection;

namespace CpTesterPlatform.CpDevices
{
    public class CpVDevAI_NIDAQ : CpVitualDeviceManagerBase, IAnalogInput
    {
        public CpFunctionEventHandler FuncEvtHndl { get; set; }
        public string DeviceID { get; set; }

        public string ChannelID { set; get; }

        public string AICoupling { get; set; }
        public double LowpassCutoffFrequency { get; set; }
        public bool LowpassEnable { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public string TerminalConfiguration { get; set; }
        public List<double> CollectedData { get; set; }
        public CancellationToken OperationCanceller { get; set; }
        public double SampleRateLimit { get; set; }

        public bool DevClose()
        {
            return true;
        }

        public bool DevOpen()
        {
            TryResult oResult = TryAction(() =>
            ((CpMngDAQControl)CommDeviceManager).RegisterAIChannel(ChannelID, AICoupling, LowpassCutoffFrequency, LowpassEnable, Max, Min, TerminalConfiguration));
            SampleRateLimit = ((CpMngDAQControl)CommDeviceManager).SampleRateLimit;

            if (oResult.HasException)
                return false;

            return true;
        }

        public bool DevReset()
        {
            return true;
        }

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }

        public bool IsValidChannel(string strChannel)
        {
            return true;
        }

        public double GetFrequency()
        {
            throw new NotImplementedException();
        }

        public double GetInstantV()
        {
            throw new NotImplementedException();
        }


        async public Task<double[]> GetPeriodicV(double nPeriodSec, double dSamplingRate)
        {
            double sampleRate = dSamplingRate;
            if (SampleRateLimit < dSamplingRate)
            {
                //UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("DAQ Configuration SampleRate Limit Error\r\n[CpEditor >> {0} HW Limit {1}]."
                //    , sampleRate, dSamplingRate));
                sampleRate = SampleRateLimit;
            }

            return await GetCollectedData(nPeriodSec, sampleRate);
        }

        async Task<double[]> GetCollectedData(double nPeriodSec, double dSamplingRate)
        {
            int nDataCount = (int)(dSamplingRate * nPeriodSec);

            return await ((CpMngDAQControl)CommDeviceManager).CollectData(ChannelID, nDataCount, dSamplingRate, OperationCanceller);
        }

        public void StartDataCollecting(int nPeriodMs, double dSamplingRate)
        {
            int nDataCount = Convert.ToInt32(dSamplingRate) * nPeriodMs;

            CollectedData = new List<double>();
            OperationCanceller = new CancellationToken();

            ((CpMngDAQControl)CommDeviceManager).StartAsyncCollect(ChannelID, nDataCount, dSamplingRate, OperationCanceller);            
        }

        public List<double> EndDataCollecting()
        {
            CollectedData = new List<double>();
            OperationCanceller = new CancellationToken();

            var resultData = ((CpMngDAQControl)CommDeviceManager).EndAsyncCollect(ChannelID, OperationCanceller);

            CollectedData.AddRange(resultData);

            return CollectedData;
        }

        public bool ReturnBuffer(double[] vresult)
        {
            ((CpMngDAQControl)CommDeviceManager).ReturnBuffer(vresult);
            return true;
        }
    }
}