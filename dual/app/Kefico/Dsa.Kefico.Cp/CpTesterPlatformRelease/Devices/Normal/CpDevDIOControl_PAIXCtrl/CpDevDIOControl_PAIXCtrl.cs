using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using Dsu.Driver;
using Dsu.Driver.Util.Emergency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static CpCommon.ExceptionHandler;
using Paix = Dsu.Driver.Paix;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevDIOControl_PAIXCtrl : IDIOControl
    {

        public CpFunctionEventHandler FuncEvtHndl { get; set; }
        public string DeviceID { get; set; }

        public string CONTROLLER_ADDRESS { set; get; } = "0.0.0.0";
        public int DIGITAL_INPUT_POINT { set; get; } = 48;
        public int DIGITAL_OUTPUT_POINT { set; get; } = 48;

        public List<bool> CURRENT_DI_STATE { set; get; }
        public List<bool> CURRENT_DO_STATE { set; get; }



        private Paix.Manager paix_ReadPort;
        private Paix.Manager paix_WritePort;
        private CancellationTokenSource cancelToken;

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }

        public bool DevClose()
        {
            if (cancelToken != null)
                cancelToken.Cancel();

            paix_ReadPort.Close();
            return true;
        }

        /// <summary>
        /// Device Open Process:
        /// Get Controller Version
        /// </summary>
        /// <returns></returns>
        public bool DevOpen()
        {

            var oResult = TryFunc(() =>
            {
                paix_ReadPort = new Paix.Manager(CONTROLLER_ADDRESS, false);
                paix_ReadPort.SetProtocolMethod(0); //  1 is UDP, 0 is TCP
                paix_ReadPort.OpenPaix();
                Thread.Sleep(500);

                paix_WritePort = new Paix.Manager(CONTROLLER_ADDRESS, false);
                paix_WritePort.SetProtocolMethod(1); //  1 is UDP, 0 is TCP
                paix_WritePort.OpenPaix();

                var version = paix_ReadPort.GetFirmVersion();

                if (paix_ReadPort.PingCheck(100) != 0)
                    return false;
                if (paix_WritePort.PingCheck(100) != 0)
                    return false;

                InitDIODevType();

                Task taskWatch = new Task(() => taskWatchSensor());

                taskWatch.Start();

                return true;
            });

            if (oResult.HasException || oResult.Result == false)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Open a DIO Control", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return false;
            }

            return oResult.Result;
        }

        public bool DevReset()
        {
            return true;
        }
        
        private short[] anInStatus = new short[128];
        public List<int> UpdateCurrentDIState()
        {
            if ( 0 == paix_ReadPort.GetDIOInput128(anInStatus) )
                return UpdateSignalState(anInStatus, CURRENT_DI_STATE);

            return null;
        }

        private short[] anOutStatus = new short[128];
        public List<int> UpdateCurrentDOState()
        {
            if ( 0 == paix_ReadPort.GetDIOOutput128(anOutStatus) )
                return UpdateSignalState(anOutStatus, CURRENT_DO_STATE);

            return null;
        }

        /// <summary>
        /// Update Digital Input States
        /// The Current State Stored in a Variable: CURRENT_DI_STATE
        /// </summary>
        /// <returns>List<int> Type Variable for Notifying Changed Pins</returns>
        static List<int> UpdateSignalState(short[] anSignalStatus, List<bool> vbCurrentState)
        {
            List<int> vnChangedPins = new List<int>();

            for (int i = 0; i < anSignalStatus.Length; i++)
            {
                if (i == vbCurrentState.Count)
                    break;

                bool bCurState = Convert.ToBoolean(anSignalStatus[i]);
                bool bExState = vbCurrentState[i];

                if (bCurState != bExState)
                {
                    vbCurrentState[i] = bCurState;
                    vnChangedPins.Add(i);
                }
            }

            return vnChangedPins;
        }

        public void SetDOutState(int nPointIdx, bool bState)
        {
            paix_WritePort.SetDIOOutPin(Convert.ToInt16(nPointIdx), Convert.ToInt16(bState));
        }

        public bool GetDOutState(int nPointIdx)
        {
            return CURRENT_DO_STATE[nPointIdx];
        }

        public bool GetDInState(int nPointIdx)
        {
            return CURRENT_DI_STATE[nPointIdx];
        }

        /// <summary>
        /// Get Pin Count
        /// </summary>
        private void InitDIODevType()
        {
            CURRENT_DI_STATE = new List<bool>();
            CURRENT_DO_STATE = new List<bool>();
            ///Get Device Info            
            Tuple<short, short> tResult = paix_ReadPort.GetDIOInfo().Value;
            var DICount = tResult.Item1;
            var DOCount = tResult.Item2;

            for (int i = 0; i < DICount; i++)
                CURRENT_DI_STATE.Add(false);
            for (int i = 0; i < DOCount; i++)
                CURRENT_DO_STATE.Add(false);
        }


        private bool _udioConnected = false;
        private async void taskWatchSensor()
        {
            cancelToken = new CancellationTokenSource();

            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                    return;

                try
                {
                    //Thread.Sleep(50);
                    await Task.Delay(50);///Equal sleep time with a PAIX example

                    List<int> vnChangedDIPins = UpdateCurrentDIState();
                    List<int> vnChangedDOPins = UpdateCurrentDOState();

                    if (vnChangedDIPins == null || vnChangedDOPins == null)
                        throw new Exception("Failed to get DIO from paix.");

                    if ( ! _udioConnected )
                        SignalManager.RawSignalSubject.OnNext(new ExtraSignal(SignalEnum.XUDIOConnected, ""));
                    _udioConnected = true;

                    //kwak  //test ahn
                    foreach (var IndexDI in vnChangedDIPins)
                    {
                        var input = $"DI;{IndexDI};{CURRENT_DI_STATE[IndexDI]};{DeviceID}";

                        // kwak
                        // First, signal to SignalManager before each device manager.
                        // For precedence reason.
                        SignalManager.RawSignalSubject.OnNext(new UDIOSignal(input));

                        FuncEvtHndl.DoTcpIpReceive(input);
                    }

                    foreach (var IndexDO in vnChangedDOPins)
                    {
                        var output = $"DO;{IndexDO};{CURRENT_DO_STATE[IndexDO]};{DeviceID}";
                        FuncEvtHndl.DoTcpIpReceive(output);
                    }
                }
                catch (Exception ex)
                {
                    //kwak, todo 
                    // connection broken case with udio
                    if ( _udioConnected)
                        SignalManager.RawSignalSubject.OnNext(new ExtraSignal(SignalEnum.XUDIODisconnected, ex.Message));
                    _udioConnected = false;
                }
            }
        }
    }
}
