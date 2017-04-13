// 시뮬레이션 모드로 DAQ 데이터를 생성.  테스트를 위한 routine 이므로 release 에서는 반드시 제거되어야 함.
// 필요시 NUMBER_SAMPLES_IN_DAQ_CHART 수치 변경 
// #define SIMULATE_DAQ_GENERATION

using CpTesterPlatform.CpApplication;
using CpTesterPlatform.CpApplication.Configure;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpResultViewTable;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTester;
using CpTesterPlatform.CpTesterSs;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.Functions;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.Base;
using Dsu.Driver.Paix;
using Dsu.Driver.UI.NiDaq;
using PsKGaudi.Parser.PsCCSSTDFn.ControlFn;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CpCommon.ExceptionHandler;
using static PsKGaudi.Parser.PsCCS;

namespace CpTesterSs.UserControl
{
    /// <summary>
    /// Main User-Interface for a Station    
    /// </summary>
    public partial class UcMainViewSs : DevExpress.XtraEditors.XtraUserControl
	{
        private const int NUMBER_SAMPLES_IN_DAQ_CHART = 1500;

        public DevExpress.XtraBars.Docking.DockManager DockManager => dockManagerSs;

        /// <summary>
        /// Station User-Interface Control
        /// </summary>
        public class ClsSubUiControl 
		{
			public bool UseBreak { get; set; } = false;
			public bool JumpToNextBreak { get; set; } = false;
			public bool JumpToNextStep { get; set; } = false;
			public bool SaveMeasuringLog { get; set; } = false;
			public bool SaveConsoleLogToFile { get; set; } = false;
			public bool SaveMeasuringData { get; set; } = false;
					
			public ClsSubUiControl(bool bUseBreak, bool bJumpToNextBreak, bool bJumpToNextStep)
			{
				UseBreak = bUseBreak;
				JumpToNextBreak = bJumpToNextBreak;
				JumpToNextStep = bJumpToNextStep;
			}
		}

		/// <summary>
		/// Managers : Station, Application, System, Process
		/// 1) The application manager has several station managers.
		/// 2) The system manager has a hardware manager that has several device managers.
		/// 3) The station manager has a Test step manager.
		/// 4) The test step manager has control variable/global variable managers. 
		/// </summary>
		public CpStnManager MngStation { get; } 
		public CpApplicationManager TheApplication => FormAppSs.TheApplication;
		public CpSystemManager TheSystemManager => FormAppSs.TheSystemManager;
		public CpProcessManager TheProcessManager => FormAppSs.TheProcessManager;
        public readonly FormAppSs TheMainForm = null;
        public bool ManualStop { get; set; } = false;
        public CpPlcIF PlcIF { get; set; }
        public List<string> DaqChartItem { get; set; } = new List<string>();


        /// <summary>
        /// Cancelation token will stops a test, when it is activated.
        /// </summary>
        public int StationID => MngStation.StationId;
        public List<ClsTsCtrBlockBase> LstContorolBlockDevice = new List<ClsTsCtrBlockBase>();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CancellationTokenSource _linkedCts; // = CancellationTokenSource.CreateLinkedTokenSource(FormAppSs.CancellationToken, _cts.Token);
        public void ResetCancellationTokenSources()
        {
            _cts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(FormAppSs.CancellationToken, _cts.Token);
        }

        #region child forms (user-control).
        /// <summary>
        /// Child forms for the user-interface of a station
        /// </summary>
        public UcTStepStatus frmSgTStepStatus { get; private set; }
		public UcTStepInformation frmSgTStepInformation { get; private set; }
		public UcTStepList frmSgTStepList { get; private set; }
        public CpResultViewTable resultView { get { return cpResultViewTable1; } }
		public UcDaqChartCtrl DaqChart { get; private set; }
		#endregion

		public ClsSubUiControl SgStnUiCtr { get; } = new ClsSubUiControl(false, false, false);
		public CpStationStatus SgStnStatus = CpStationStatus.IDLE;

        public DevExpress.XtraBars.BarButtonItem StartButton => barButtonItemStartSgTest;
		public DevExpress.XtraBars.BarButtonItem StopButton => barButtonItemStopSgTest;
		public DevExpress.XtraBars.BarButtonItem PauseButton => barButtonItemPauseSgTest;
		public DevExpress.XtraBars.BarButtonItem JumpButton => barButtonItemSgJump;
		public DevExpress.XtraBars.BarButtonItem NextButton => barButtonItemSgNext;

		///Constructor - Receiving a station ID and importing a corresponding station configuration with the station ID from the parent form.
		public UcMainViewSs(int nStationId, FormAppSs ParentMainForm, CpPlcIF plc)
		{
            PlcIF = plc;

            TheMainForm = ParentMainForm;
			MngStation = TheApplication.GetStation(nStationId);
			InitializeComponent();
            LoadingSubView();

#if SIMULATE_DAQ_GENERATION
            //kwak,
            DaqChartItem.AddRange(Enumerable.Range(0, 10).Select(n => $"TestFunction{n}"));
            DaqChart = new UcDaqChartCtrl(this, false)
            {
                UseFixedYRange = true,
                // todo : parameter 를 수정할 것.
                //Channel = AiDAQ.HwName,
                //SamplingRate = Convert.ToDouble(commDAQ.SAMPLING_PER_SEC),// Background 에서 돌고 있는 sampling rate 와 동일해야 함.
                SamplingRate = 1000000,
                CollectNumberMultiplier = 15,
                NumSamplesPerBuffer = NUMBER_SAMPLES_IN_DAQ_CHART,      // 크게 잡지 말것.
                RedrawPause = 1000,             // 작게 잡지 말것.
                IsCrosshairEnabled = false,     // DAQ chart view 에서 cross hair 는 비활성화한다.
                Dock = DockStyle.Fill,
            };


            DaqChart.Prepare();

            panelControlSgRstView.Controls.Add(DaqChart);
            panelControlSgRstView.Dock = DockStyle.Fill;
            //kwak
#endif

            CfgLogState(ParentMainForm.MainUiCtr.SaveMeasuringLog, ParentMainForm.MainUiCtr.SaveMeasuringData, ParentMainForm.MainUiCtr.SaveConsoleLogToFile);
			
			barButtonItemSgBreak.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
        }

        public void CfgLogState(bool bSaveMeasuringLog, bool bSaveMeasuringData, bool bSaveConsoleLogToFile)
		{
			SgStnUiCtr.SaveMeasuringLog = bSaveMeasuringLog;
			SgStnUiCtr.SaveMeasuringData = bSaveMeasuringData;
			SgStnUiCtr.SaveConsoleLogToFile = bSaveConsoleLogToFile;
		}

        void LoadingSubView()
        {
            #region child forms (user-control).
            /// <summary> 
            /// Loading sub forms.
            /// </summary>

            // load status view
            frmSgTStepStatus = new UcTStepStatus(this);
            panelControlSgTestStatus.Controls.Add(frmSgTStepStatus);
            panelControlSgTestStatus.Dock = DockStyle.Fill;

            // load information view                
            frmSgTStepInformation = new UcTStepInformation(this);
            panelControlSgTestInformation.Controls.Add(frmSgTStepInformation);
            panelControlSgTestInformation.Dock = DockStyle.Fill;

            // load test-list
            frmSgTStepList = new UcTStepList(this);
            panelControlSgTest.Controls.Add(frmSgTStepList);
            panelControlSgTest.Dock = DockStyle.Fill;

       

            cpTrendViewChart.StationIndex = MngStation.StationId;
            cpTrendViewChart.SetLogPath(TheApplication.CnfApp.AppConfigure.SaveLogPath);
            cpTrendViewChart.SetDisplayLimit(10);


#if SIMULATE_DAQ_GENERATION
            // simulates DAQ data generation
            {
                var random = new Random();

                Task.Run(async () =>
                {
                    int i = 0;
                    await Task.Delay(2000);
                    while (true)
                    {
                        i++;
                        await Task.Delay(100);
                        var data = Enumerable.Range(0, NUMBER_SAMPLES_IN_DAQ_CHART).Select(n => random.NextDouble() * 2.0).ToArray();
                        //UcDaqChartCtrl.TestDaqDataSubject.OnNext(data);

                        var min = 0.1 + (i % 10) / 10.0;
                        var max = +1.0 - (i % 10) / 10.0;
                        //Trace.WriteLine($"Set chart Min={min}, Max={max}.");
                        DaqChartEvent.DaqSubject.OnNext(new DAQResult(MngStation.StationId, $"TestFunction{i % 10}", data, min, max));
                    }
                });
            }
#endif

#endregion
        }

        private void action1_Update(object sender, EventArgs e)
        {
            //var starting = TheMainForm.Starting;
            //barButtonItemStartSgTest.Enabled = !starting;
            //barButtonItemOpenFile.Enabled = !starting;
            //barButtonItemStopSgTest.Enabled = !starting;
            //barButtonItemPauseSgTest.Enabled = !starting;
            //barButtonItemSgBreak.Enabled = !starting;
            //barButtonItemSgJump.Enabled = !starting;
            //barButtonItemSgNext.Enabled = !starting;
        }


        public async Task ShowStateDisplay()
        {
            await this.DoAsync(() =>
            {
                ucPLCMonitor PLCMonitor = new ucPLCMonitor();
                PLCMonitor.SetDefaultText();
                PLCMonitor.SetPlcIF(PlcIF);
                PLCMonitor.Dock = DockStyle.Fill;
                panelControlState.Controls.Add(PLCMonitor);
                panelControlState.Dock = DockStyle.Fill;
            });
        }

     

        private void ShowDAQChart(bool dynamic=false)
        {
            var CommMng = TheSystemManager.MngHardware.DicCommDeviceManager;
            var DeviceMng = TheSystemManager.MngHardware.DicDeviceManager;
            var StationDeviceMng = MngStation.CnfStation.DevList;

            ClsDAQControllerInfo commDAQ = CommMng.Where(w => w.Value.DeviceInfo.DeviceType == CpDeviceType.DAQ_CONTROLLER).FirstOrDefault().Value.DeviceInfo as ClsDAQControllerInfo;
            if (commDAQ == null)
                return;

            foreach (CpStnDevConfigure StnDevConfigure in StationDeviceMng)
            {
                if (StnDevConfigure.ID == string.Empty)
                    continue;
                ClsAnalogInputInfo AiDAQ = DeviceMng.Where(w => w.Value.DeviceInfo.Device_ID == StnDevConfigure.ID).FirstOrDefault().Value.DeviceInfo as ClsAnalogInputInfo;
                if (AiDAQ == null)
                    continue;


                // load swift-chart
                // todo : chart drawing 은 resource 를 많이 소모하므로, parent tab 에서 swift chart tab 이 활성화되는 시점에만 chart 를 그려야 한다.
                DaqChart = new UcDaqChartCtrl(this, dynamic)
                {
                    UseFixedYRange = true,
                    // todo : parameter 를 수정할 것.
                    Channel = AiDAQ.HwName,
                    SamplingRate = Convert.ToDouble(commDAQ.SAMPLING_PER_SEC),// Background 에서 돌고 있는 sampling rate 와 동일해야 함.
                    CollectNumberMultiplier = 15,
                    NumSamplesPerBuffer = NUMBER_SAMPLES_IN_DAQ_CHART,      // 크게 잡지 말것.
                    RedrawPause = 1000,             // 작게 잡지 말것.
                    IsCrosshairEnabled = false,     // DAQ chart view 에서 cross hair 는 비활성화한다.
                    Dock = DockStyle.Fill,
                };

                if (dynamic)
                {
                    panelControlSgRstViewRun.Controls.Add(DaqChart);
                    panelControlSgRstViewRun.Dock = DockStyle.Fill;
                    DaqChart.Run();                   // 그리기 시작.  멈추려면 DaqChart.Stop() 호출\
                }
                else
                {
                    panelControlSgRstView.Controls.Add(DaqChart);
                    panelControlSgRstView.Dock = DockStyle.Fill;
                    DaqChart.Prepare();
                }






            }
        }

        #region TASKS
        private void StopTask(CpProcess cpProc, CpStationStatus stationStatus)
        {
            CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(cpProc, CpSystemStatus.STOP, stationStatus));
        }
        /// <summary>
        /// Pre-Main-Post Task 관계 : Start Test 시작 전 Initialize 및 결과를 기다리기 위함. 
        /// Test 종료 메시지에 의해 FinalizeTestResult호출 -> Post Task 구현
        /// </summary>
        public async Task StartPreTask()
		{
            CpProcess cpProc = null;
            try
            {
                _cts = new CancellationTokenSource();
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(FormAppSs.CancellationToken, _cts.Token);

                //Task<CpProcessEndStatus> process = Task.Run(() => TaskSgPreTask.taskSgPreTest(this, CancelToken), CancelToken.Token);
                Task<CpProcessEndStatus> process = Task.Factory.StartNew(async () => await TaskSgPreTask.taskSgPreTest(this, _linkedCts)).Result;
                cpProc = new CpProcess(StationID, process, _linkedCts);
                TheProcessManager.CurrentTasks.Add(cpProc);

                CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(cpProc, CpSystemStatus.RUN, CpStationStatus.PRE_RUN));

                Task finTask = process.ContinueWith(antecendent => CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(cpProc, antecendent.Result, CpStationStatus.PRE_FINISHED)));
                await process;
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException || (ex is AggregateException && ((AggregateException)ex).InnerExceptions.OfType<TaskCanceledException>().Any()))
                    StopTask(cpProc, CpStationStatus.PRE_RUN);
                else
                    UtilTextMessageEdits.UtilTextMsgToConsole($"Unhandled exception: {ex}.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }

        public async Task StartMainTask()
        {
            CpProcess cpProc = null;
            try
            {
                int nStartStep = Convert.ToInt32(MngStation.CnfStation.TestListCondition.StartStep);
                ((CpTsManager)MngStation.MngTStep).setTsRange(nStartStep);

                //Task<CpProcessEndStatus> process = Task.Run(() => TaskSgTest.taskSgTest(this, CancelToken), CancelToken.Token);
                Task<CpProcessEndStatus> process = Task.Factory.StartNew(async () => await TaskSgTest.taskSgTest(this, _linkedCts)).Result;
                cpProc = new CpProcess(StationID, process, _linkedCts);
                TheProcessManager.CurrentTasks.Add(cpProc);

                CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(cpProc, CpSystemStatus.RUN, CpStationStatus.MAIN_RUN));

                Task finTask = process.ContinueWith(antecendent => CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(cpProc, antecendent.Result, CpStationStatus.MAIN_FINISHED)));
                await process;
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException || (ex is AggregateException && ((AggregateException)ex).InnerExceptions.OfType<TaskCanceledException>().Any()))
                    StopTask(cpProc, CpStationStatus.MAIN_RUN);
                else
                    UtilTextMessageEdits.UtilTextMsgToConsole($"Unhandled exception: {ex}.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
            }

        }

        public async Task StartPostTask()
		{
            CpProcess cpProc = null;
            try
            {
                var This = this;
                //Task<CpProcessEndStatus> process = Task.Run(() => TaskSgPostTask.taskSgPostTest(This, CancelToken), CancelToken.Token);
                Task<CpProcessEndStatus> process = Task.Factory.StartNew(async() => await TaskSgPostTask.taskSgPostTest(this, _linkedCts)).Result;
                cpProc = new CpProcess(StationID, process, _linkedCts);
                TheProcessManager.CurrentTasks.Add(cpProc);

                CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(cpProc, CpSystemStatus.RUN, CpStationStatus.POST_RUN));

                Task finTask = process.ContinueWith(antecendent => CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(cpProc, antecendent.Result, CpStationStatus.POST_FINISHED)));
                await process;
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException || (ex is AggregateException && ((AggregateException)ex).InnerExceptions.OfType<TaskCanceledException>().Any()))
                    StopTask(cpProc, CpStationStatus.POST_FINISHED);
                else
                    UtilTextMessageEdits.UtilTextMsgToConsole($"Unhandled exception: {ex}.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
		}
		#endregion

		/// <summary>
		/// Operation 중임을 표시하기 위한 disposable class.
		/// </summary>
		private class OnOperationMarker : IDisposable
		{
			private UcMainViewSs _mainView;

			public OnOperationMarker(UcMainViewSs mv)
			{
				_mainView = mv;
				_mainView.DoAsync(() =>
				{
					_mainView.barButtonItemStartSgTest.Enabled = false;
					_mainView.barButtonItemStopSgTest.Enabled = true;
					_mainView.barButtonItemPauseSgTest.Enabled = true;					
				});
			}

			public void Dispose()
			{
				_mainView.DoAsync(() =>
				{
					_mainView.barButtonItemStartSgTest.Enabled = true;
					_mainView.barButtonItemStopSgTest.Enabled = false;					
					_mainView.barButtonItemPauseSgTest.Enabled = false;
				});
			}
		}

		public IDisposable GetOnOperationMarker() { return new OnOperationMarker(this);}

        public async Task UpdateResultViewTable()
        {
            await this.DoAsync(() => cpResultViewTable1.threadSafeResultViewUpdate());
        }

        public async Task UpdateForPrintOutFn(int nCrtStepIndex, CpTsShell cpTStep, CpSystemStatus eCpStatus, string strMsgFst = "", string strMsgScd = "")
		{
			await this.DoAsync(async () =>

			{	            
				// Select Current Step & Focus on the Current Step (Scroll).
				frmSgTStepList.GridViewTestSteps.SelectRow(nCrtStepIndex);
				frmSgTStepList.GridViewTestSteps.FocusedRowHandle = nCrtStepIndex;

				// status bar test number
				frmSgTStepStatus.labelTsHeadNumber.Text = cpTStep.Core.StepNum.ToString();

				// print out
				if ((cpTStep.Core as PsCCSStdFnPrintOut) != null)
				{
					string strPrintOutParm = (cpTStep.Core as PsCCSStdFnPrintOut).ArstdSerialParmsP.GetValueByIndex(CPTsIndex.INDEX_PRINT_OUT_DISPLAY_TEXT);
					await frmSgTStepInformation.ChangeStepFunction( ClsGlobalStringForUIDisplay.CP_MODULE_PRINT + strPrintOutParm);
				}
				else
				   await frmSgTStepInformation.ChangeStepFunction(cpTStep.Core.STDBoschName);
			});
		}

		/// - Test-List Loading
		private async Task<bool> LoadSgTestList(string strSelFileName)
		{
			MngStation.MngTStep = null;
			return await WaitLoadingTestList(MngStation, strSelFileName);
		}

        public static Task<bool> WaitLoadingTestList(CpStnManager MngStn, string strSelFileName)
        {

            ((CpTsManager)MngStn.MngTStep)?.GaudiReadData.Clear();
            PsCCSGaudiFile psGaudiReadData = null;
            TryResult exeRst = TryAction(() =>
            {
                psGaudiReadData = PsCCSGaudiFile.PsCCSPaseDataSimple(strSelFileName);
            });

            if (exeRst.HasException == true || psGaudiReadData == null)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to load a Testlist.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole($" \t- Reason: {exeRst.Exception}", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
            }

            MngStn.MngTStep = new CpTsManager(psGaudiReadData);

            return Task.FromResult(true);
        }

		/// - Event-List
		private async void barButtonItemOpenFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			/// open file dialog.
			using (OpenFileDialog opFileDlg = new OpenFileDialog())
			{
				if (opFileDlg.ShowDialog() == DialogResult.OK)
				{
					await OpenTestListFile(opFileDlg.FileName);
					await ProceedControlBlock();
                }
					
			}
		}

		/// <summary>
		/// advanced preparation for the test list.
		/// loading & building a alias structure from the '.cnf' file (Marked at the 'E_DOKU / ADAPTERFILE').
		/// linking between the TestSteps(0-1000) & the alias structure.
		/// preparations.
		/// </summary>
		/// <returns></returns>
		private async Task<bool> ProceedControlBlock()
		{

			TryResult exeRst = TryAction(() =>
			{				
				((CpTsManager)MngStation.MngTStep).setTsRange(ClsDefineGlobalConstStepNumber.STEP_CONTROL_BLOCK_START, ClsDefineGlobalConstStepNumber.STEP_CONTROL_BLOCK_END);

				_cts = new CancellationTokenSource();
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(FormAppSs.CancellationToken, _cts.Token);
            });
			if(exeRst.HasException == true)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to load control block from the file.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + exeRst.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return await TaskCpCtrBlock.WaitCpConstructComblockSteps(TheSystemManager, TheApplication, MngStation, _linkedCts);            
		}

		/// <summary>
		/// A Test List File Opens
		/// </summary>
		/// <param name="strPath"></param>
		/// <returns></returns>
		private async Task OpenTestListFile(string strPath)
		{
			await Task.Run(async () =>
			{
				using (GetOnOperationMarker())
				{
					/// 1.show current status.
					MngStation.StationStatus = CpSystemStatus.LOADING;

					frmSgTStepStatus.ChangeStatus(MngStation.StationStatus);
					
					/// 2.load a file.
					bool result = await LoadSgTestList(strPath);
					if (!result)
					{
						string strMsg = TheSystemManager.MngResx.GetString("StringFailedToLoadFile") + strPath;
						throw new FileLoadException(strMsg);
					}				

					/// 3.display test-list information.
					barStaticItemLoadedTestList.Caption = strPath;

					/// 4.show test list in the grid. fill up with the given data.
					await UcTStepList.AsyncUpdateInitialGridViewAll(((CpTsManager) MngStation.MngTStep), frmSgTStepList);

		
				}
			});

        }

        private void InitializePlcInterface()
        {
            TheSystemManager.MngHardware.DicDeviceManager.Values.Where(w => w.DeviceInfo.DeviceType == CpDeviceType.PLC).ForEach(e =>
            {
                if (((ClsPLCInfo)e.DeviceInfo).READPORT)
                {
                    PlcIF.MngPlcRead = e as CpMngPlc;
                    PlcIF.CreatEventPLC();
                    TheMainForm.AddMonitiorUnloadingStation((CpMngPlc)e);
                }
                else
                    PlcIF.MngPlcWrite = e as CpMngPlc;
            });

            foreach (CpAdtCnf cpAdtCnf in MngStation.MngTStep.MngControlBlock.LstLoadedAdapterCnf)
            {
                if (cpAdtCnf.CtrBlockProperty != ControBlockProperty.CP_FU)
                    continue;
                CpDeviceManagerBase DeviceManagerBase = CpUtil.GetManagerDevice(TheSystemManager, cpAdtCnf.AdtMslUnit.RawString);
                if (DeviceManagerBase == null)
                    continue;

                PlcIF.AddItem(cpAdtCnf.AdtPinName.RawString, cpAdtCnf.AdtBpAddress.RawString);
                PlcIF.MngPlcRead.AddDevices(cpAdtCnf.AdtBpAddress.RawString);
            }

            PlcIF.StopHandler += PlcIF_StopHandler;
        }

        public void PlcIF_FdMasterHandler(bool CheckOn)
        {
            if (!CheckOn) return;
            if (CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.LVDT))
            {
                var ps = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.LVDT);
                foreach (var mng in ps)
                    ((CpMngLVDT)mng).SetMasterDimension();
            }
        }

        private void PlcIF_StopHandler(bool bStop)
        {
            if (bStop)
                _cts.Cancel();
            else
            {
                _cts = new CancellationTokenSource();
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(FormAppSs.CancellationToken, _cts.Token);
            }
        }

        /// <summary>
        ///Close 
        /// </summary>
        public void CloseTest()
        {
            MngStation.StationStatus = CpSystemStatus.FINISH;
            SgStnStatus = CpStationStatus.IDLE;
            _cts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(FormAppSs.CancellationToken, _cts.Token);

            foreach (var dev in LstContorolBlockDevice)
            {
                ClsTsCtrBlockWithAdapter adt = dev as ClsTsCtrBlockWithAdapter;
                CpDeviceManagerBase devManager = CpUtil.GetManagerControlDevice(TheSystemManager, MngStation, dev.CtrBlockPinName);
                if (devManager == null || !devManager.ActiveHw)
                    continue;


                switch (devManager.DeviceInfo.DeviceType)
                {
                    case CpDeviceType.POWER_SUPPLY: ((CpMngPowerSupply)devManager).ResetDevice(); break;
                    case CpDeviceType.MOTION: ((CpMngMotion)devManager).ResetDevice(); break;
                    case CpDeviceType.DIGITAL_IO: ((CpMngDIOControl)devManager).ResetDeviceBit(Convert.ToInt32(adt.SgPinNum.Replace("DI", "").Replace("DO", ""))); break;//ahn
                    default:
                        continue;
                }
            }
        }

        /// <summary>
        ///Initailize
        /// </summary>
        public void InitiallizeTest()
		{
            CloseTest();
        }

        /// <summary>
        /// Repeat -Task
        /// - 자동 운전이 아닐 경우 반복 테스트 수행
        /// - If an event is fired from the user, it has to be stopped immediately.
        /// </summary>
        public async void RepeatRun()
        {
            if (TheMainForm.AutoStartPLC)
            {
                StartSgPreTest();
            }
            else
            {
                if (ManualStop)
                    return;

                if (MngStation.CnfTestCondition.RepeatTest)
                {
                    var nRepeat = Convert.ToInt32(MngStation.CnfTestCondition.RepeatNum);
                    var nCurrent = MngStation.TsCount + MngStation.NgCount;
                    if (nRepeat > nCurrent)
                    {
                        //display repeat counter 
                        FormAppSs.TheMainForm.ShowDisplayMessage($"Repeat: {nCurrent}/{nRepeat}");

                        var resetTime = Convert.ToInt32(MngStation.CnfTestCondition.ResetTime);
                        await Task.Delay(resetTime);
                        StartSgPreTest();
                    }
                }
            }
        }

        /// <summary>
        /// Main-Task
        /// - a test consists of a set of test steps. When all of steps executed sequentially, the tester memorize test step logs.
        /// - If an event is fired from the user, it has to be stopped immediately.
        /// </summary>
        public async void StartSgMainTest()
		{
            await TryActionAsync(async () =>
            {
                SgStnStatus = CpStationStatus.MAIN_RUN;						
			await StartMainTask();
            });
        }

        /// <summary>
        /// Pre-Task (await task when all) 
        /// - When a station need to start a new test, it has to check all of conditions satisfied. 
        /// = If all of stations ready to start, the process manager triggers all stations. 
        /// - ex) a PLC ready signal means all mechanical & control conditions are set.
        /// </summary>
        public async void StartSgPreTest()
        {
            await TryActionAsync(async () =>
            {
                if (await TaskSgBackground.taskSgBackgroundSkippable(this, _linkedCts)) // "return true" is Skip Test
                {
                    if (FormAppSs.TheMainForm.AutoStartPLC)
                    {
                        SgStnStatus = CpStationStatus.POST_RUN;
                        await StartPostTask();
                    }
                }
                else
                {
                    SgStnStatus = CpStationStatus.PRE_RUN;
                    await StartPreTask();
                }
            });
        }

        /// <summary>
        /// Post-Task (await task when all)
        /// - When a station finished a test completely, it has to proceed a post sequence task (save log, report)
        /// - The result of test should be displayed in the main view.
        /// </summary>
        public async void StartSgPostTest()
        {
            await TryActionAsync(async () =>
            {
                SgStnStatus = CpStationStatus.POST_RUN;
                await StartPostTask();
                await this.DoAsync(() => cpTrendViewChart.UpdateTrendChart());
            });
        }
    
		public void PauseSgMainTest()
		{			
			CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(new CpProcess(StationID, null, null), new CpProcessEndStatus(CpSystemStatus.PAUSE, 0, 0), CpStationStatus.MAIN_RUN));
		}

        /// <summary>
        /// Change the Status Color of a Station
        /// </summary>
        /// <param name="sysStat"></param>
		public void ChangeSystemStatus(CpSystemStatus sysStat)
		{
			frmSgTStepStatus.ChangeStatus(sysStat);
		}
        
        /// <summary>
        /// DAQ Measuring Start for Async Data Requests from Multiple-Stations
        /// </summary>
        /// <param name="bState"></param>
        public void InitDAQMeasurement(bool bState)
        {
            List<string> vstrIDs = TheSystemManager.MngHardware.GetCommDeviceIdList(CpDeviceType.DAQ_CONTROLLER);

            foreach (string strId in vstrIDs)
            {
                if (TheSystemManager.MngHardware.GetCommDeviceManager(strId).ActiveHw == true)
                {
                    IDAQManager daqMng = (IDAQManager)TheSystemManager.MngHardware.GetCommDeviceManager(strId);

                    if (daqMng.RunningState != bState)
                        daqMng.ReadContinuousDAQValues(bState);
                }
            }
        }


        /// <summary>
        /// To handle messages about operation states. 
        /// </summary>		
        private void barButtonItemStartSgTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            // create a sequencer.
            var vPreTask = new List<Task> { new Task(StartSgPreTest) };
			Parallel.ForEach(vPreTask, task => task.Start());			
		}

		private void barButtonItemStopSgTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			TheProcessManager.GetStationProcess(StationID)?.CancelToken.Cancel();

            CloseTest();
        }

        private async void UcMainViewSs_Load(object sender, EventArgs e)
        {
            var strTlPath = Directory.GetCurrentDirectory() + TheApplication.GetStnTestListPath(StationID);
            if (!File.Exists(strTlPath))
            {
                UtilTextMessageBox.UIMessageBoxForWarning("Failed to load a Testlist.", string.Format("[Station {0}]\r\nFailed to load TestList file {1} ", MngStation.StationId, strTlPath));
                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("[Station {0}]Failed to load TestList file {1} ", MngStation.StationId, strTlPath), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return;
            }

            await OpenTestListFile(strTlPath);

            if (await ProceedControlBlock())
            {
                LstContorolBlockDevice.Clear();
                LstContorolBlockDevice.AddRange(MngStation.MngTStep.MngControlBlock.DicAnsteuerWithCtrBlock.Values);

                cpTrendViewChart.SetTestStepMng(MngStation.MngTStep as CpTsManager);
                cpTrendViewChart.InitialDisplayLimit = MngStation.TsCount + MngStation.NgCount;
                cpTrendViewChart.InitTrendChart();
                cpResultViewTable1.SetTeststepManager(MngStation.MngTStep as CpTsManager);

                barButtonItemStartSgTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItemOpenFile.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItemStopSgTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItemPauseSgTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItemSgBreak.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItemSgJump.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItemSgNext.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                if (FormAppSs.TheMainForm.AutoStartPLC)
                {
                    InitializePlcInterface();

                    dockPanelState.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                }
                else
                {
                    dockPanelState.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }

                MngStation.MngTStep.GaudiReadData.ListTestStep.Where(w =>
                w.STDKeficoName == "PULSE_D"
               || w.STDKeficoName == "PULSE_V"
               || w.STDKeficoName == "PULSE_W").ForEach(f => DaqChartItem.Add(f.GetMO()));

                CheckTestListforDeviceID();
            }

            if (CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.ANALOG_INPUT))
            {
                ShowDAQChart(false);

                if (DriverBaseGlobals.IsAudit())
                    ShowDAQChart(dynamic: true);
                else
                    panelControlSgRstViewRun.Visible = false;
            }

            MngStation.StationStatus = CpSystemStatus.READY;
            frmSgTStepStatus.ChangeStatus(MngStation.StationStatus);
        }

        private void CheckTestListforDeviceID()
        {
            MngStation.MngTStep.GaudiReadData.ListTestStep.ForEach(f => f.GetModuleList().Where(w => w.STDKeficoName == "M_LVDT").ForEach(m =>
            {
                PsCCSStdFnModuleM_LVDT psModuleM_LVDT = m as PsCCSStdFnModuleM_LVDT;
                CheckDeviceID(f, psModuleM_LVDT.DeviceID);
            }));

            MngStation.MngTStep.GaudiReadData.ListTestStep.ForEach(f => f.GetModuleList().Where(w => w.STDKeficoName == "M_DMM_CP").ForEach(m =>
            {
                PsCCSStdFnModuleM_DMM_CP psModuleM_DMM_CP = m as PsCCSStdFnModuleM_DMM_CP;
                CheckDeviceID(f, psModuleM_DMM_CP.DeviceID);
            }));

            MngStation.MngTStep.GaudiReadData.ListTestStep.ForEach(f => f.GetModuleList().Where(w => w.STDKeficoName == "M_DLCRM").ForEach(m =>
            {
                PsCCSStdFnModuleM_DLCRM psModuleM_DLCRM = m as PsCCSStdFnModuleM_DLCRM;
                CheckDeviceID(f, psModuleM_DLCRM.DeviceID.Split(';')[0]);
            }));
        }

        private void CheckDeviceID(PsKGaudi.Parser.PsCCSSTDFn.PsCCSStdFnBase f, string unitName)
        {
            if (!TheSystemManager.MngHardware.DicDeviceManager.ContainsKey(unitName))
                UtilTextMessageBox.UIMessageBoxForWarning("Failed to load a Testlist.", string.Format("[Station {0}]\r\nFailed to loading TestList file. \r\n[{1} step] can't find device : {2}", MngStation.StationId, f.StepNum, unitName));
        }

        private void barButtonItemPauseSgTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			PauseSgMainTest();
		}

		private void barButtonItemSgJump_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			SgStnUiCtr.JumpToNextBreak = true;

			PauseSgMainTest();
		}

		private void barButtonItemSgNext_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			SgStnUiCtr.JumpToNextStep = true;		

			if(MngStation.StationStatus != CpSystemStatus.PAUSE)
				PauseSgMainTest();				
		}

		private void barButtonItemSgBreak_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			SgStnUiCtr.UseBreak = SgStnUiCtr.UseBreak != true;
						
			barButtonItemSgBreak.Down = SgStnUiCtr.UseBreak;
		}	
		
		public void SetMoveNextStep(int nStepNum, long xTime)
		{
			CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(new CpProcess(StationID, null, null), new CpProcessEndStatus(CpSystemStatus.NEXT_STEP, xTime, nStepNum), CpStationStatus.MAIN_RUN));
		}	
		
		public void SetBreakStep(int nStepNum, long xTime)
		{
			CptApplication.TheApplication.ApplicationSubject.OnNext(new CpProcessEvent(new CpProcess(StationID, null, null), new CpProcessEndStatus(CpSystemStatus.PAUSE, xTime, nStepNum), CpStationStatus.MAIN_RUN));
		}

		public bool IsThisBreakStep(int nStepNum)
		{
			var arRowData = new ArrayList();
			var bBreakPoint = false;

			TryResult exeRst = TryAction(() =>
			{
				arRowData = this.frmSgTStepList.DicUbBreakPointColumns[nStepNum];
				bBreakPoint = (bool)(arRowData[(int)UiCpDefineEnumTsUnboundColumns.BRK]);                
			});

			if(exeRst.HasException == true)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to check the state of the break point in the main-frame.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + exeRst.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
				
				return false;
			}

			return bBreakPoint;
		}

        /// <summary>
        /// Increment OK Count
        /// </summary>
		public void IncreaseSuceededResult()
		{	
			MngStation.TsCount++;
			frmSgTStepStatus.UpdateSuceededResult();
		}

        /// <summary>
        /// Increment NG Count
        /// </summary>
        public void IncreaseFailedResult()
		{
			MngStation.NgCount++;
			frmSgTStepStatus.UpdateFailedResult();
		}

        private void barButtonItemClearCount_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FormAdmin.DoModal())
            {
                MngStation.TsCount = -1;
                MngStation.NgCount = -1;


                IncreaseSuceededResult();
                IncreaseFailedResult();
            }
        }
    }
}

