using System.Collections.Generic;
using CpTesterPlatform.CpCommon;
using System.Threading;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public enum DAQ_AI_MODE
    {
        CONTINUOUS_TIME = 0,
        SINGLE_TIME,
        TRIGGER_SAMPLING
    }

    public enum GROUNT_TYPE
    {
        Nrse = 0,
        Rse,
        Differential,
        Pseudodifferential
    }

    public interface IDAQController : IDevice
    {
        void StartContAIAcqOp();
        void StopContAIAcqOp();
        void DeviceResetDaqTask(int Channel);
        bool ReturnBuffer(double[] vresult);


        bool RegisterAIChannel(string strChannelID, string strAICoupling, double dLowpassCutoffFrequency, bool bLowpassEnable, double dMax, double dMin, string strTerminalConfiguration);
        Task<double[]> CollectData(string strChannelID, int nDataCount, double dSamplingRate, CancellationToken cancelToken);
        void StartAsyncCollect(string strChannelID, int nDataCount, double dSamplingRate, CancellationToken cancelToken);
        double[] EndAsyncCollect(string strChannelID, CancellationToken cancelToken);


        double MinimumAIValue { set; get; }
        double MaximumAIValue { set; get; }
        string TerminalCfg { set; get; }
        DAQ_AI_MODE AIAcqMode { set; get; }
        string AITriggerSrc { set; get; }
        string AITriggerStartEdge { set; get; }
        string AITimingSrc { set; get; }
        int SAMPLING_PER_SEC { set; get; }
        int SAMPLE_PER_BUFFER { set; get; }
        double GetCurrentSampleRate();

        // 
        //         string CIChannelID { set; get; }
        //         double CIMinimumValue { set; get; }
        //         double CIMaximumValue { set; get; }
        //         double CIMinimumFrequency { set; get; }
        //         double CIMaximumFrequency { set; get; }
        //         string CICounterChannel { set; get; }
        //         int CISampleCount { set; get; }
        // 
        //         bool IsValidAIChannel(string strChannelID);
        //         bool IsValidCIChannel(string strChannelID);
        // 
        //         void InitAIChannel(string strChannelID, DAQ_AI_MODE eAiMode);
        //         void InitCIChannel(string strChannelID, CpCounterAttribute eCType);
        //         void InitTrgIOControl(CpFunctionEventHandler evtHndl, string strSrcCh, string strTrgOut, string strRCh, string strFCh);
        // 
        //         void StartReadWaveValues();
        //         void EndReadWaveValues();
        // 
        //         void StartTriggerControl();
        //         void EndTriggerControl();
        //         //double [] ReadValueV();
        //         //List<List<double>> ReadValueWaveV(int nUnitSampleCount);
        // 
        //         void UpdateFrequency(string strChID);
        //         void UpdatePulseWidth(string strChID);
        //         double[] GetCounterResult(string strChID);

    }
}
