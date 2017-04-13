using System;
using System.Collections.Generic;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using static CpCommon.ExceptionHandler;
using System.Threading;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpMngLib.Manager
{
	/// <summary>
	/// DaqControlManager 
	/// EXE1: Continuous sample acquisition
	/// EXE2: Provides Data to Virtual Channel
	/// </summary>
	public class CpMngDAQControl : CpDeviceManagerBase, IDAQManager
	{
		public CpMngDAQControl(bool activeHw) : base(activeHw)
		{
		}

		private int ServerPort { set; get; } = 0;

		public bool IsCreated { private set; get; } = false;
		public bool IsOpened { private set; get; } = false;
		public bool IsClosed { private set; get; } = false;
		public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;

		public List<string> ReceivedMessageList { set; get; } = new List<string>();
		public bool RunningState { get; set; } = false;
        public double SampleRateLimit { get; set; }

        List<IDAQIO> IDAQManager.ReadAIChannels
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		List<IDAQIO> IDAQManager.ReadCIChannels
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		List<IDAQIO> IDAQManager.ReadTrgChannels
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		CpDAQDataContainer IDAQManager.DataContainer
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool CloseDevice()
		{
            if (IsClosed) return true;
            IDAQController devDAQCtrl = DeviceInstance as IDAQController;
			IsClosed = devDAQCtrl?.DevClose() ?? false;
			return IsClosed;
		}

		public bool CreateDevice(ClsDeviceInfoBase info)
		{
			var oResult = TryFunc(() =>
			{
				IsCreated = CreateInstanceFromDll(info.DllName);

				DeviceInfo = info;

				return IsCreated;
			});

            if (oResult.Result == false)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create CpMngDAQControl.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);

                if (oResult.HasException)
                    UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}

		public bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
		{
			throw new NotImplementedException();
		}

		public bool InitManager()
		{
			IDAQController devDAQCtrl = DeviceInstance as IDAQController;

			//return devDAQCtrl?.DevInit() ?? false;

			return true;
		}

		public bool OpenDevice()
		{
			IDAQController devDAQCtrl = DeviceInstance as IDAQController;
			ClsDAQControllerInfo infoDAQCtrl = DeviceInfo as ClsDAQControllerInfo;

			if (devDAQCtrl == null || infoDAQCtrl == null)
			{
				return false;
			}

			FuncEvtHndl = new CpFunctionEventHandler();
            devDAQCtrl.DeviceID = infoDAQCtrl.HwName;
            devDAQCtrl.SAMPLE_PER_BUFFER = infoDAQCtrl.SAMPLE_PER_BUFFER;
            devDAQCtrl.SAMPLING_PER_SEC = infoDAQCtrl.SAMPLING_PER_SEC;
            devDAQCtrl.FuncEvtHndl = FuncEvtHndl;

            IsOpened = devDAQCtrl.DevOpen();

            SampleRateLimit = devDAQCtrl.GetCurrentSampleRate();

            return IsOpened;
		}

		public bool ResetDevice()
		{
            if (!IsOpened) return false;

            IDAQController devDAQCtrl = DeviceInstance as IDAQController;

            return devDAQCtrl.DevReset(); 
		}

        public void StartContAIAcqOp()
        {
            ((IDAQController)DeviceInstance).StartContAIAcqOp();
        }

        public void StopContAIAcqOp()
		{
			((IDAQController)DeviceInstance).StopContAIAcqOp();
		}

        public void ResetBuffer(int Channel)
        {
            ((IDAQController)DeviceInstance).DeviceResetDaqTask(Channel);
        }

        public bool ReturnBuffer(double[] vresult)
        {
            return ((IDAQController)DeviceInstance).ReturnBuffer(vresult);
        }

        public bool RegisterAIChannel(string strChannelID, string strAICoupling, double dLowpassCutoffFrequency, bool bLowpassEnable, double dMax, double dMin, string strTerminalConfiguration)
		{
            return ((IDAQController)DeviceInstance).RegisterAIChannel(strChannelID, strAICoupling, dLowpassCutoffFrequency, bLowpassEnable, dMax, dMin, strTerminalConfiguration);
		}

		public async Task<double[]> CollectData(string strChannelID, int nDataCount, double dSamplingRate, CancellationToken cancelToken)
		{
			return await ((IDAQController)DeviceInstance).CollectData(strChannelID, nDataCount, dSamplingRate, cancelToken);            
		}

        public void StartAsyncCollect(string strChannelID, int nDataCount, double dSamplingRate, CancellationToken cancelToken)
        {
            ((IDAQController)DeviceInstance).StartAsyncCollect(strChannelID, nDataCount, dSamplingRate, cancelToken);
        }

        public double[] EndAsyncCollect(string strChannelID, CancellationToken cancelToken)
        {
            return ((IDAQController)DeviceInstance).EndAsyncCollect(strChannelID, cancelToken);
        }

        void IDAQManager.ReadContinuousDAQValues(bool bState)
		{
			throw new NotImplementedException();
		}

		double IDAQManager.GetInstantV(string strChannelID)
		{
			throw new NotImplementedException();
		}

		List<List<double>> IDAQManager.GetPeriodicV(string strChannelID, double dTimeStart, double dTimeEnd)
		{
			throw new NotImplementedException();
		}

		double[] IDAQManager.UpdateFrequencyData(ICounterInput devCI)
		{
			throw new NotImplementedException();
		}

		double[] IDAQManager.UpdatePulseWidth(ICounterInput devCI)
		{
			throw new NotImplementedException();
		}  
	}
}
