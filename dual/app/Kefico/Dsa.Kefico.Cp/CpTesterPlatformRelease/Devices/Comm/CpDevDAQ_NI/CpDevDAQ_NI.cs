using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using static CpCommon.ExceptionHandler;
using Dsu.Driver;
using NationalInstruments.DAQmx;
using static Dsu.Driver.NiDaqParams;
using static Dsu.Driver.NiDaqScAi;
using System.Threading;
using CpTesterPlatform.Functions;
using Dsu.Driver.Base;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevDAQ_NI : CpDevNormalBase, IDAQController
    {
        public int SAMPLING_PER_SEC { set; get; }
        public int SAMPLE_PER_BUFFER { set; get; } = 10000;

        public List<DaqScAiParams> Multi_Channel_Parms { set; get; } = new List<DaqScAiParams>();

        public double MinimumAIValue { set; get; }
        public double MaximumAIValue { set; get; }
        public string TerminalCfg { set; get; }
        public DAQ_AI_MODE AIAcqMode { set; get; }
        public string AITriggerSrc { set; get; }
        public string AITriggerStartEdge { set; get; }
        public string AITimingSrc { set; get; }
        public double AISamplingRate { set; get; }
        public int AISampleCount { set; get; }

        public string CIChannelID { set; get; }
        public double CIMinimumValue { set; get; }
        public double CIMaximumValue { set; get; }
        public double CIMinimumFrequency { set; get; }
        public double CIMaximumFrequency { set; get; }
        public string CICounterChannel { set; get; }
        public int CISampleCount { set; get; }

        private bool FinishChannel0 = false;
        private bool FinishChannel1 = false;
        private bool FinishChannel2 = false;
        private bool FinishChannel3 = true; //Unuse 8FF  //-1 is all
        private NiDaqMcAi.DaqMcAiManager manager;

        private DaqMcAiParams parameters;
        public override bool DevClose()
        {
            return true;
        }

        public override bool DevOpen()
        {
            List<DaqScAiParams> daqParms = new List<DaqScAiParams>();

            for (int i = 0; i < 4; i++)  //4채널 기본 사용 ai0 ~ ai3  
            {
                var ChannelParams = new NiDaqParams.DaqScAiParams(DeviceID + "/ai" + i.ToString());
                if (DriverBaseGlobals.IsLine())
                {
                    ChannelParams.LowpassEnable = true;
                    ChannelParams.LowpassCutoffFrequency = 500 * 1000;
                }
                //ChannelParams.Max = 2;
                //ChannelParams.Min = -2;
                daqParms.Add(ChannelParams);
            }

            parameters = new DaqMcAiParams(daqParms);

            var hwMax = NiDaqHwProbe.NiDaqHwLocal.GetAIMaximumMultiChannelRate(DeviceID);
            // todo : 강제로 낮게 sampling rate 를 잡은 것이므로, hw config 에 정의된 값으로 설정되도록 할 것.
            //Background DAQ Default sampleRate is 1M (1,000,000)
            if (hwMax < SAMPLING_PER_SEC)
            {
                UtilTextMessageBox.UIMessageBoxForWarning(
                  "DAQ Configuration Error"
                  , string.Format("DAQ Configuration SampleRate Limit Error\r\n[CPTesterSystemConfigue.xml >> SAMPLING_PER_SEC >> {0} HW Limit {1}].", SAMPLING_PER_SEC, hwMax));
                parameters.SamplingRate = hwMax;
            }
            else
                parameters.SamplingRate = SAMPLING_PER_SEC;

            NiDaq.CreateMcManager(parameters);
            manager = NiDaq.McManager();
            manager.ResetDaqTask();

            return true;
        }

        public double GetCurrentSampleRate()
        {
            return parameters.SamplingRate;
        }


        public void DeviceResetDaqTask(int clearChannel)
        {
            bool clear = false;
            if (clearChannel == -1)
                clear = true;

            if (clearChannel == 0) FinishChannel0 = true;
            else if (clearChannel == 1) FinishChannel1 = true;
            else if (clearChannel == 2) FinishChannel2 = true;
            else if (clearChannel == 3) FinishChannel3 = true;

            if (FinishChannel0 && FinishChannel1 && FinishChannel2 && FinishChannel3)
                clear = true;

            if(clear)
            {
                FinishChannel0 = false;
                FinishChannel1 = false;
                FinishChannel2 = false;
                FinishChannel3 = false;

                if (DriverBaseGlobals.IsLine7DCT())
                {
                    FinishChannel2 = true;
                    FinishChannel3 = true;
                }
                else if (DriverBaseGlobals.IsLine8FF())
                    FinishChannel3 = true; //Unuse 8FF

                System.Threading.Tasks.Task.Run(() => NiDaq.McManager().ResetDaqTask());

                CpUtilDaq.DicDaqWave.Clear();
            }
        }

        public bool ReturnBuffer(double[] vresult)
        {
            manager.ReturnBuffer(vresult);
            return true;
        }


        public void StopContAIAcqOp()
        {
            NiDaq.McManager().Cancel();
        }

        public bool RegisterAIChannel(string strChannelID, string strAICoupling, double dLowpassCutoffFrequency, bool bLowpassEnable, double dMax, double dMin, string strTerminalConfiguration)
        {
            {
                TryResult oResult = TryAction(() =>
                {
                    var sgAIChParms = new DaqScAiParams(strChannelID);

                    AICoupling eAICpl = AICoupling.DC;
                    AITerminalConfiguration eTCType = AITerminalConfiguration.Differential;

                    if (AICoupling.TryParse(strAICoupling, out eAICpl))
                        sgAIChParms.AICoupling = eAICpl;

                    if (AITerminalConfiguration.TryParse(strTerminalConfiguration, out eTCType))
                        sgAIChParms.TerminalConfiguration = eTCType;

                    sgAIChParms.LowpassCutoffFrequency = dLowpassCutoffFrequency;
                    sgAIChParms.LowpassEnable = bLowpassEnable;
                    sgAIChParms.Max = dMax;
                    sgAIChParms.Min = dMin;
                    sgAIChParms.VoltageUnits = AIVoltageUnits.Volts;

                    Multi_Channel_Parms.Add(sgAIChParms);
                });

                if (oResult.HasException)
                    return false;

                return true;
            }
        }


        public async Task<double[]> CollectData(string strChannelID, int nDataCount, double dSamplingRate, CancellationToken cancelToken)
        {
            var daqch = new DaqScAiManager(strChannelID);

            // 해당 channel 로부터 비동기적으로 주어진 샘플 갯수만큼 수집하는 task를 await 를 통해 데이터 수집
            var data = await daqch.CollectAsync(dSamplingRate, nDataCount, cancelToken);

            return data;
        }

        public void StartAsyncCollect(string strChannelID, int nDataCount, double dSamplingRate, CancellationToken cancelToken)
        {
            var daqch = new DaqScAiManager(strChannelID);

            // 해당 channel 로부터 비동기적으로 주어진 샘플 갯수만큼 수집하는 task를 await 를 통해 데이터 수집
            daqch.StartCollect(dSamplingRate, nDataCount, cancelToken);            
        }
            
        public double[] EndAsyncCollect(string strChannelID, CancellationToken cancelToken)
        {
            var daqch = new DaqScAiManager(strChannelID);

            // 해당 channel 로부터 비동기적으로 주어진 샘플 갯수만큼 수집하는 task를 await 를 통해 데이터 수집
            var data = daqch.EndCollect(cancelToken);

            return data;
        }

        public void StartContAIAcqOp()
        {
            throw new NotImplementedException();
        }

     

        //static void CreateDaqManager(List<DaqScAiParams> daqParms)
        //{
        //    //// Type1 : channel 별 parameter 조정없이, 사전 정의된 parameter 로 모든 channel 을 사용할 경우.
        //    //NiDaqMc.createManagerSimple(4);     // manager 생성

        //    // Type2: channel 별 parameter 를 조정하여 각 channel 별로 다르게 사용할 경우.
        //    //var parameters = MultiChannelDef.createDefaultMcParameter(4);

        //    var mcparms = new DaqMcAiParams(daqParms); 
        //    mcparms.SamplingRate = 100000;
        //    mcparms.NumberOfSamples = 10000;

        //    mcparms.SamplingRate = 1000;
        //    mcparms.NumberOfSamples = 100;
        //    NiDaq.CreateMcManager(mcparms);  // channel 별 parameter 를 다르게 적용한 manager 생성
        //}
    }
}
