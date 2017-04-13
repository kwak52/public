using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CpCommon;
using CpTesterPlatform.CpApplication;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncLib.Manager;
using CpTesterPlatform.DxUtility.frmLog;
using CpTesterSs;
using CpTesterSs.UserControl;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.DX;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Drawing;
using System.Linq;
using DevExpress.XtraEditors;
using CpTesterPlatform.DxUtility;
using static CpCommon.ExceptionHandler;
using static CpBase.CpLog4netLogging;
using static Dsu.Driver.Util.DriverLog4netLogging;
using CpTesterPlatform.Functions;
using log4net;
using CpTesterPlatform.CpTesterSs;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTStepDev;
using System.Reflection;
using Dsu.Driver.Util.Emergency;
using System.Threading.Tasks;
using Dsu.Driver;
using CpTesterSs.UIManual;
using Dsu.Driver.Paix;
using Dsu.Common.Utilities.Exceptions;
using System.Xml.Linq;
using Dsu.Driver.Base;
using System.Diagnostics;
using CpBase;
using CpTesterSs.Event;

namespace CpTesterPlatform.CpTester
{
    /// <summary>
    /// Main application form.
    /// 
    /// Designer 에서 다음과 같은 오류가 나는 경우는, 
    /// 1. 임시로 CpDxRibbonFormAppCommon 상속을 DevExpress.XtraBars.Ribbon.RibbonForm 상속으로 변경한 후,
    /// 1. design 작업을 수행한 후, 
    /// 1. 원래의 CpDxRibbonFormAppCommon 상속으로 변경해 주세요.
    /// </summary>
    public partial class FormAppSs
        //: DevExpress.XtraBars.Ribbon.RibbonForm		// <-- for design time
        : CpDsFormAppCommon
    {
        private Timer _mTimerRepeatTest = new Timer();

        /// Hardware configuration folder.  e.g "Configure\HwConfig\Audit78"
        public static string CPTesterHardwareConfigurationFolder { get; private set; }
        /// Signal definition fot this tester hardware.  e.g "SignalDefinitionFileAudit78.xml"
        public static string SignalDefinitionFile { get; private set; }
        public static string ToOriginPoseName { get; private set; }


        // managers.

        /// <summary>
        /// application manager controls components above the base system component (CU+SS).
        /// </summary>
        public static CpApplicationManager TheApplication { get; private set; }
        public CpApplicationManager MngApplication => TheApplication;

        public static FormAppSs TheMainForm { get; private set; }

        /// <summary>
        /// system manager controls components in the base system component (CU+SS).
        /// </summary>
        public static CpSystemManager TheSystemManager { get; private set; }
        public CpSystemManager MngSystem => TheSystemManager;

        /// <summary>
        /// process manager controls a set of stations (SS).
        /// </summary>
        public static CpProcessManager TheProcessManager { get; private set; }
        public CpProcessManager MngProcess => TheProcessManager;

        public static List<CpStnManager> Stations => TheApplication.Station;

        public static IEnumerable<T> GetDeviceManagers<T>() => CpSystemManager.GetDeviceManagers<T>();
        public static T GetDeviceManager<T>() => CpSystemManager.GetDeviceManager<T>();
        public static CpMngMotion RobotManager => GetDeviceManagers<CpMngMotion>().Where(d => d.PaixRobot != null).FirstOrDefault();


        public bool ProcessIdle { set; get; } = true;
        public ILog Logger { get; set; }
        public bool AutoMode { get; set; } = true;
        public bool AutoStartPLC { get; set; } = false;
        public bool BackgroundRunning { get; set; } = false;
        private List<string> LstStnCountInfo = new List<string>();

        private Dictionary<int, CpPlcIF> DicPlcIF = new Dictionary<int, CpPlcIF>();
        private FormTotalResult TotalResultDialog;
        private FormTotalResult7DCT TotalResultDialog7DCT;
        private frmSgDocFrame AuditfrmSgDocFrame;
        // user interface
        public ClsMainUIControl MainUiCtr { get; } = new ClsMainUIControl(true, false, false);

        public bool Starting
        {
            get
            {
                if (TheApplication == null)
                    return false;
                if (FormAppSs.Stations.Where(w => w.CnfStation.Enable).Any(s => s.StationStatus.IsOneOf(CpSystemStatus.RUN, CpSystemStatus.PRE_TASK_RUN, CpSystemStatus.POST_TASK_RUN))
               || FormAppSs.TheMainForm.GetViews().Any(v => v.SgStnStatus.IsOneOf(CpStationStatus.POST_RUN, CpStationStatus.MAIN_RUN, CpStationStatus.PRE_RUN))
                    || BackgroundRunning)
                    return true;
                else
                    return false;
            }
        }

        public FormAppSs()
        {
            InitializeComponent();
            TheMainForm = this;
            timerGlobalTask.Start();
            tabbedView.DocumentAdded += tabbedView_DocumentAdded;
            UtilTextMessageEdits.UEventOutput += WriteOutputText;
            tabbedView.DocumentProperties.AllowClose = false;
        }


        private void tabbedView_DocumentAdded(object sender, DocumentEventArgs e)
        {
            if (!e.Document.IsVisible)
                tabbedView.Controller.CreateNewDocumentGroup(e.Document as Document, Orientation.Vertical);
        }

        private CpSignalManager _signalManager;
        private void MapIOSignals()
        {
            var plcEnabled = CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.PLC);
            var dioEnabled = CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.DIGITAL_IO);
            List<CpMngPlc> lstPLCMng = new List<CpMngPlc>();
            List<CpMngDIOControl> lstUDIOMng = new List<CpMngDIOControl>();
            if (plcEnabled || dioEnabled)
            {
                if (plcEnabled)
                {
                    lstPLCMng = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.PLC).Cast<CpMngPlc>().ToList();
                }

                if (dioEnabled)
                {
                    // see taskWatchSensor() code
                    lstUDIOMng = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.DIGITAL_IO).Cast<CpMngDIOControl>().ToList();
                }

                _signalManager = new CpSignalManager($@"{CPTesterHardwareConfigurationFolder}\{SignalDefinitionFile}", lstPLCMng.Cast<CpMngPlc>(), lstUDIOMng.Cast<CpMngDIOControl>());

                if (plcEnabled)
                {
                    foreach (var mngPLC in lstPLCMng)
                        if (mngPLC.PLC.READPORT)
                            SignalManager.Dictionary.Values
                                .Where(o => o.Type == "PLC")
                                .ForEach(o => mngPLC.AddDevices(o.Address));
                }
            }
        }

        /// Application 시작 이후, alarm clear 한번이라도 한적 있다면 true.  Home move 성공 이후에는 false 로 설정됨
        public static bool HasAlarmClearHistory { get; set; }
        private void AlarmClearCarefully()
        {
            if (_signalManager == null)
                return;

            _signalManager.BreakDevicesOn();

            TheSystemManager.MngHardware.DicDeviceManager.Values.Where(w => w.DeviceInfo.DeviceType == CpDeviceType.MOTION).ForEach(e =>
            {
                if (((ClsMotionInfo)e.DeviceInfo).AXIS_ROBOT)
                {
                    var mngMotion = e as CpMngMotion;
                    if (mngMotion.ClearAlarmOnDemand())
                        HasAlarmClearHistory = true;
                }
            });
            _signalManager.BreakDevicesOff();

            _signalManager.PowerOnDevices();
        }

        private void LoadTesterHWIdentification(string xmlConfigFileName)
        {
            var configFile = @"Configure\HwConfig\CPTesterHWIdentification.xml";

            /* double check with CpTesterConfiguration.xml. */
            //var hwSpecs = XDocument.Load(xmlConfigFileName).Descendants("HaConfigure").Elements("CPTesterHWIdentificationFile").First().Value;
            //if (hwSpecs != configFile)
            //    throw ExceptionWithCode.Create(ErrorCodes.APP_ConfigurationError, "HwConfigCPTesterHWIdentification.xml configuration error.");

            var specXml = XDocument.Load(configFile).Descendants("CPTesterHardware").First();
            var atts = specXml.Attributes().ToDictionary(at => at.Name.ToString(), at => at.Value);
            var testerName = atts["Name"];      // e.g "A_78KGP"
            SignalDefinitionFile = atts["SignalDefinitionFile"];    // e.g "SignalDefinitionAudit78.xml"
            CPTesterHardwareConfigurationFolder = atts["CPTesterHardwareConfigurationFolder"];
            ToOriginPoseName = atts.ContainsKey("ToOriginPoseName") ? atts["ToOriginPoseName"] : "";

            DriverBaseGlobals.TesterType = (CpTesterEnum)Enum.Parse(typeof(CpTesterEnum), testerName);
        }

        // form events.
        private async void FormAppSs_Load(object sender, EventArgs args)
        {
            try
            {
                this.Enabled = false;

                if (ModifierKeys == Keys.Shift)
                    FormDeveloper.DoModal();

                OnLoadInitialize();

                #region configurations.
                var config = $@"{Directory.GetCurrentDirectory()}\Configure";
                var sFilePath = $@"{config}\AppConfig\{ClsGlobalStringForGeneral.FILE_CP_CONFIGUE_NAME}";
                if (!File.Exists(sFilePath))
                {
                    ShowMessageBox("System Configuration file loading Error.");
                    Close();
                }

                LoadTesterHWIdentification(sFilePath);
                #endregion

                /// 개발자 모드 : progress bar 의 동작 형식이 pingpong 에서 cycle 로 변경됨
                /// InitCpTester 이전에 수행 되어야 함.
                List<IDisposable> subscriptions = new List<IDisposable>();
                subscriptions.Add(Globals.DeveloperModeChangedSubject.Subscribe(developerMode =>
                {
                    repositoryItemMarqueeProgressBar1.MarqueeWidth = developerMode ? 30 : 100;
                    repositoryItemMarqueeProgressBar1.ProgressAnimationMode = developerMode ? DevExpress.Utils.Drawing.ProgressAnimationMode.Cycle : DevExpress.Utils.Drawing.ProgressAnimationMode.PingPong;
                }));

                // unhandled exception 발생시 처리
                subscriptions.Add(UnhandledExceptionHandler.UnhandledExceptionSubject.Subscribe(ex =>
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\FatalError.txt";
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine($"{DateTime.Now}\t{ex}\r\n");
                    }

                    // unhandled exception 발생시, progress bar 에 총 exception 갯수가 표시되며, 표시 방식이 변경됩니다.

                    //barEditItem1.EditValue = $"Check Exception({UnhandledExceptionHandler.TotalNumberOfUnhandledException}).";
                    //repositoryItemMarqueeProgressBar1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
                    //repositoryItemMarqueeProgressBar1.LookAndFeel.UseDefaultLookAndFeel = false;
                    //repositoryItemMarqueeProgressBar1.StartColor = repositoryItemMarqueeProgressBar1.EndColor = Color.Orchid;
                    //repositoryItemMarqueeProgressBar1.ShowTitle = true;
                    //ribbonStatusBarCPMainForm.LayoutChanged();
                }));


                // CpTester 시작
                await InitCPTester(sFilePath);


                // subscribe to ups data change event.
                subscriptions.Add(Ups.upsDataChangedSubject.Subscribe(async upsData =>
                {
                    await this.DoAsync(() =>
                    {
                        CurrentHumidity = upsData.Humidity;
                        CurrentTemerature = upsData.Temperature;
                    });
                }));

                subscriptions.Add(MessageSubject.Subscribe(async evt =>
                {
                    var logLevel = CpDefineEnumDebugPrintLogLevel.NONE;
                    var color = ConsoleColor.White;
                    switch (evt.Item1)
                    {
                        case DriverLogLevel.Debug: logLevel = CpDefineEnumDebugPrintLogLevel.DEBUG; break;
                        case DriverLogLevel.Info: logLevel = CpDefineEnumDebugPrintLogLevel.INFO; break;
                        case DriverLogLevel.Error: logLevel = CpDefineEnumDebugPrintLogLevel.FATAL; color = ConsoleColor.Red; break;
                        default: logLevel = CpDefineEnumDebugPrintLogLevel.FATAL; break;
                    }
                    var message = evt.Item2;
                    await this.DoAsync(() =>
                    {
                        UtilTextMessageEdits.UtilTextMsgToConsole(message, color, logLevel);
                    });
                }));

                subscriptions.Add(RobotMessageSubject.Subscribe(async evt =>
                {
                     await this.DoAsync(() =>
                     {
                          labelControl_Message.Text = evt.Item2.ToString();
                     });
                }));

                subscriptions.Add(ExceptionWithCode.ExceptionWithCodeSubject.Subscribe(evt =>
                {
                    var msg = $"{evt.Message}({evt.ErrorCode})";
                    CpSignalManager.ShowErrorFormShortly(SignalEnum.XExceptionWithCode, msg);
                }));


                Application.Idle += OnApplicationIdle;

                // 프로그램 종료시 수행해야 할 작업 지정
                Globals.ApplicationExitSubject.Subscribe(s =>
                {
                    subscriptions.ForEach(d => d.Dispose());
                    Application.Idle -= OnApplicationIdle;
                });

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Failed to initialize CP Tester!\r\n{ex.Message}");
                LogError($"Failed to initialize CP Tester!\r\n{ex}");
            }
            finally
            {
                this.Enabled = true;
            }
        }



        private void ClearCurrentTestCfg()
        {
            foreach (var frmOn in this.MdiChildren)
            {
                var frame = frmOn as frmSgDocFrame;
                if (frame == null) continue;
                frame.Close();
            }
        }

        private async Task InitCPTester(string sFilePath)
        {
            ClearCurrentTestCfg();
            // including station manager.
            TheApplication = new CpApplicationManager(sFilePath);
            TheSystemManager = new CpSystemManager(sFilePath);
            TheProcessManager = new CpProcessManager();
            CpTStepFnc.FunctionDLLName = TheApplication.CnfApp.AppConfigure.FuncExeDllFile;

            OpenCommDevices();
            OpenDevices();

            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

            MapIOSignals();

            if ( DriverBaseGlobals.IsAudit() )
                AlarmClearCarefully();

            LoadStation();
            LoadLayout();

            await StartInitialProcess();
        }

        private void LoadStation()
        {
            if ( DriverBaseGlobals.IsLine())
            {
                AutoStartPLC = true;
                // ready status            

                if (! CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.PLC))
                {
                    if ( DialogResult.No == MessageBox.Show("PLC not enabled in Line tester. Do you want to continue?", "NO PLC", MessageBoxButtons.YesNo) )
                        System.Diagnostics.Process.GetCurrentProcess().Kill();

                    AutoStartPLC = false;
                }

                #region open stations.

                var oResult = TryAction(() =>
                {
                    foreach (CpStnManager mngStn in TheApplication.Station)
                    {
                        CpPlcIF PlcIF = new CpPlcIF(mngStn.StationId);
                        DicPlcIF.Add(mngStn.StationId, PlcIF);
                        frmSgDocFrame sgDocFrame = new frmSgDocFrame(mngStn.StationId, this, PlcIF);

                        sgDocFrame.Text = TheApplication.GetStation(mngStn.StationId).Name;
                        sgDocFrame.Show();

                        repositoryItemComboBox3.Items.Add(mngStn.StationId.ToString());
                    }

                });

                if (oResult.HasException)
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Device Open StationConfigure.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                }

                #endregion 
                barButtonItem_LoadTestList.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                OpenAuditMode();
                barButtonItemMnCtrStart.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        private void OpenAuditMode()
        {
            var UcMain = AuditfrmSgDocFrame?.GetMyViewFrm();
            AuditfrmSgDocFrame?.Close();
            CpStnManager mngStn = SelectStnManager();
            if (mngStn == null)
                return;

            TheApplication.Station.ForEach(stn =>
            {
                if (mngStn != stn)
                    stn.CnfStation.Enable = false;
                else
                    stn.CnfStation.Enable = true;
            });

            AuditfrmSgDocFrame = new frmSgDocFrame(mngStn.StationId, this, PlcIF: null);

            AuditfrmSgDocFrame.Text = TheApplication.GetStation(mngStn.StationId).Name;
            AuditfrmSgDocFrame.Show();

            repositoryItemComboBox3.Items.Add(mngStn.StationId.ToString());

        }

        private static CpStnManager SelectStnManager()
        {
            CpStnManager mngStn = null;
            if (DriverBaseGlobals.IsAudit78())
            {
                var form = new FormTestlistSelectorA78(TheApplication.Station);
                form.ShowDialog();
                mngStn = FormTestlistSelectorA78.SelectedStation;
            }
            else if (DriverBaseGlobals.IsAuditGCVT())
            {
                var form = new FormTestlistSelectorAGCVT(TheApplication.Station);
                form.ShowDialog();
                mngStn = FormTestlistSelectorAGCVT.SelectedStation;
            }

            return mngStn;
        }

        private async Task UpdateUIMain()
        {
            await this.DoAsync(() =>
            {
                string sTTNR = TheApplication.Station.Where(s => s.CnfStation.Enable).FirstOrDefault().MngTStep.GaudiReadData.TestListInfo.PartNum; 
                labelControlTitle.Text = string.Format("{0} [{1}]", TheApplication.CnfApp.SystemInfoConfigure.Title, sTTNR);
                
                barCheckItemShowColComment.Checked = !barCheckItemShowColComment.Checked; barCheckItemShowColComment.PerformClick();
                barCheckItemShowColGate.Checked = !barCheckItemShowColGate.Checked; barCheckItemShowColGate.PerformClick();
                barCheckItemShowColParameter.Checked = !barCheckItemShowColParameter.Checked; barCheckItemShowColParameter.PerformClick();
                barCheckItemShowColPosition.Checked = !barCheckItemShowColPosition.Checked; barCheckItemShowColPosition.PerformClick();
                barCheckItemShowColReturn.Checked = !barCheckItemShowColReturn.Checked; barCheckItemShowColReturn.PerformClick();
                barCheckItemShowColVariant.Checked = !barCheckItemShowColVariant.Checked; barCheckItemShowColVariant.PerformClick();
                barCheckItemShowColInf.Checked = !barCheckItemShowColInf.Checked; barCheckItemShowColInf.PerformClick();
                barCheckItemShowColMP.Checked = !barCheckItemShowColMP.Checked; barCheckItemShowColMP.PerformClick();
            });
        }

        private async Task ManualUIVisible()
        {
            const DevExpress.XtraBars.BarItemVisibility never = DevExpress.XtraBars.BarItemVisibility.Never;

            if (DriverBaseGlobals.IsLine())
            {
                ribbonPageGroupDoor.Visible = false;
                ucTestMonitor1.SetStationName();
            }
            else
                dockPanel_TestResult.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;

            if (!DriverBaseGlobals.IsAudit78())
                barButtonItem_RobotReadyMove.Visibility = never;

            if (!CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.PLC))
                barButtonItem_PLC.Visibility = never;

            if (!CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.LVDT))
                barButtonItem_LVDT.Visibility = never;

            if (!CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.POWER_SUPPLY))
            {
                barButtonItem_PowerSupply1.Visibility = never;
                barButtonItem_PowerSupply2.Visibility = never;
                barButtonItem_PowerSupply3.Visibility = never;
            }
            else
            {
                List<IDevManager> lstMng = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.POWER_SUPPLY);
                if (lstMng.Count < 2)
                {
                    barButtonItem_PowerSupply2.Visibility = never;
                    barButtonItem_PowerSupply3.Visibility = never;
                }
                else if (lstMng.Count < 3)
                    barButtonItem_PowerSupply3.Visibility = never;
            }

            if (!CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.LCRMETER))
                barButtonItem_LCR.Visibility = never;

            if (!CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.DIGITAL_IO))
                barButtonItem_DigtalIO.Visibility = never;

            if (!CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.ANALOG_INPUT))
                barButtonItem_AnalogIO.Visibility = never;

            if (!CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.MOTION))
            {
                barButtonItem_Motor1.Visibility = never;
                barButtonItem_Motor2.Visibility = never;
                barButtonItem_Motor3.Visibility = never;
            }
            else
            {
                List<IDevManager> lstMng = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), false);
                if (lstMng.Count < 2)
                {
                    barButtonItem_Motor2.Visibility = never;
                    barButtonItem_Motor3.Visibility = never;
                }
                else if (lstMng.Count < 3)
                    barButtonItem_Motor3.Visibility = never;
            }

            if (CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), true).Count == 0)
            {
                barButtonItem_Robot.Visibility = never;
                barButtonItem_manualAirGap.Visibility = never;
                barEditItem_RPM.Visibility = never;
            }
            else
            {
                var psMotor = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), false).FirstOrDefault();
                if (psMotor != null)
                {
                    CpMngMotion mngMotion = psMotor as CpMngMotion;

                    if (mngMotion.FuncEvtHndl == null) return;
                    mngMotion.FuncEvtHndl.OnTcpIpReceive += async str => 
                    {
                        var dataArr = str.Split(';');
                        if (dataArr.Length != 9)
                            return;
                        await this.DoAsync(() =>
                        {
                            barEditItem_RPM.EditValue = Convert.ToSingle(Math.Round(mngMotion.GetCurrentRpm(), 4));
                        });
                    };
                }
            }


            await UpdateUIMain();
        }

        private async Task StartInitialProcess()
        {
            await Task.Run(async () =>
            {
                await CheckAllStationsReady();
                UtilTextMessageEdits.UtilTextMsgToConsole("All stations are ready.", ConsoleColor.Yellow, CpDefineEnumDebugPrintLogLevel.INFO);

                await InitializeProcess();


                if (DriverBaseGlobals.IsLine())
                {
                    System.Diagnostics.Debug.Assert(AutoStartPLC);
                    UpdateTestCountInfo();
                    TheSystemManager.MngHardware.DicDeviceManager.Values.ForEach(async f =>
                    {
                        if (f is CpMngPlc && ((CpMngPlc)f).PLC.READPORT)
                            await ((CpMngPlc)f).SingleScanStartAsync();
                    });

                    DicPlcIF.Values.ForEach(f => f.AutoStartInterface());
                    GetViews().ForEach(async v => await v.ShowStateDisplay());
                    await StartTest();
                }
                else
                {
                    Debug.Assert(DriverBaseGlobals.IsAudit());

                    var robots = GetDeviceManagers<CpMngMotion>().Where(d => d.PaixRobot != null);
                    var mngDio = GetDeviceManagers<CpMngDIOControl>();
                    FormManualRobotAudit.SetDeviceManagers(robots, mngDio);
                    if (robots != null && mngDio != null)
                    {
                        await FormAppSs.TheMainForm.DoAsync(() =>
                        {
                            if (!PaixManagerBase.IsOriginCalibrated)
                            {
                                FormAppSs.TheMainForm.Visible = false;
                                Form form = DriverBaseGlobals.IsAudit78() ? (Form)new FormManualRobotAudit78() : new FormManualRobotAuditGCVT();
                                if ( DialogResult.OK !=  form.ShowDialog())
                                {
                                    ShowMessageBox("Failed to configure robot.");
                                }

                                FormAppSs.TheMainForm.Visible = true;
                            }
                        });
                    }
                }
            });
        }

        private void InitializeRobotPath()
        {
            string directory = $@"{Directory.GetCurrentDirectory()}\{CPTesterHardwareConfigurationFolder}";
            string search = "*.poses";
            List<string> lstPath = Directory.EnumerateFiles(directory, search, SearchOption.TopDirectoryOnly).ToList();
            CpMngMotion mngMotion;

            TheSystemManager.MngHardware.DicDeviceManager.Values.Where(w => w.DeviceInfo.DeviceType == CpDeviceType.MOTION).ForEach(e =>
            {
                if (((ClsMotionInfo)e.DeviceInfo).AXIS_ROBOT)
                {
                    mngMotion = e as CpMngMotion;
                    mngMotion.SetModePath(lstPath);
                }
            });
        }

        private void OpenCommDevices()
        {
            foreach (CpDeviceType eType in Enum.GetValues(typeof(CpDeviceType)))
            {
                foreach (string strCommDevID in TheSystemManager.MngHardware.GetCommDeviceIdList(eType))
                {
                    if (TheSystemManager.MngHardware.DicCommDeviceManager[strCommDevID].ActiveHw == false)
                        continue;

                    IDevManager idevMgr = (IDevManager)TheSystemManager.MngHardware.DicCommDeviceManager[strCommDevID];

                    if (!idevMgr.OpenDevice())
                        UtilTextMessageBox.UIMessageBoxForWarning(
                          "Hardware Configuration Error", string.Format("Failed to open device [CPTesterSystemConfigue.xml >> CommDevices] \r\n{0}].", strCommDevID));
                }
            }
        }

        private void OpenDevices()
        {
            List<string> failedMessages = new List<string>();
            foreach (CpDeviceType eType in Enum.GetValues(typeof(CpDeviceType)))
            {
                foreach (string strDevID in TheSystemManager.MngHardware.GetDeviceIdList(eType))
                {
                    if (TheSystemManager.MngHardware.DicDeviceManager[strDevID].ActiveHw == false)
                        continue;

                    IDevManager idevMgr = (IDevManager)TheSystemManager.MngHardware.DicDeviceManager[strDevID];

                    if (!idevMgr.OpenDevice())
                    {
                        failedMessages.Add($"[CPTesterSystemConfigue.xml >> Devices >> {strDevID}].");
                    }
                }
            }

            if (failedMessages.Any())
            {
                LogErrorWithMessageBox("Failed to open devices.\r\nExiting the application\r\n\r\n" + String.Join("\r\n", failedMessages.ToArray()));
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void CloseDevices()
        {
            TheSystemManager?.MngHardware?.CloseDeviceManager();
            TheSystemManager?.MngHardware?.CloseCommDeviceManager();
        }

        // menu events.        

        public string GetVersion()
        {
            string strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return strVersion;
        }

  
        private void barCheckItemShowLogWindow_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var form = new DxFormLog();
            form.Initialize(this);
            form.MdiParent = this;
            form.Show();
        }

        public UcMainViewSs GetStationForm(int nStationId)
        {
            foreach (var frmOn in this.MdiChildren)
            {
                var frame = frmOn as frmSgDocFrame;
                if (frame == null) continue;
                var viewss = ((frmSgDocFrame)frmOn).GetMyViewFrm();
                if (viewss.MngStation.StationId == nStationId)
                    return viewss;
            }
            return null;
        }

        public int GetStationCount()
        {
            int nCnt = 0;
            foreach (var frmOn in this.MdiChildren)
            {
                var frame = frmOn as frmSgDocFrame;
                if (frame == null) continue;
                var viewss = ((frmSgDocFrame)frmOn).GetMyViewFrm();
                nCnt++;
            }
            return nCnt;
        }

        public List<CpStnManager> GetAllStnMng()
        {
            List<CpStnManager> vStnMng = new List<CpStnManager>();

            foreach (var frmOn in this.MdiChildren)
            {
                var frame = frmOn as frmSgDocFrame;
                if (frame == null) continue;
                var viewss = ((frmSgDocFrame)frmOn).GetMyViewFrm();
                vStnMng.Add(viewss.MngStation);
            }

            return vStnMng;
        }

        public override void HandleStationEvent(IObservableEvent evt)
        {
            if (evt is CpProcessEvent)
            {
                CpProcessEvent procEvt = (CpProcessEvent)evt;
                UcMainViewSs viewSs = GetStationForm(procEvt.Process.StationId);
                CpTaskEndStatus endStat = TheProcessManager.HandleProcessEvent(procEvt);

                UtilTextMessageEdits.UtilTextMsgToConsole("Current Station State, Station ID: " + viewSs.StationID.ToString() + ", System: " +
                endStat.SystemStatus + ", Station: " + endStat.StationStatus, ConsoleColor.Green, CpDefineEnumDebugPrintLogLevel.INFO);

                if (endStat.SystemStatus == CpSystemStatus.NG || endStat.SystemStatus == CpSystemStatus.SKIP)
                {
                    viewSs.CloseTest();
                    viewSs.SgStnStatus = CpStationStatus.IDLE;
                    viewSs.ChangeSystemStatus(endStat.SystemStatus);
                    viewSs.frmSgTStepStatus.ChangeStatus(procEvt.SystemStatus, "Not Passed", "Spend Time = " + procEvt.TestEndStatus.SpentTime + " ms");
                    if (endStat.SystemStatus == CpSystemStatus.NG) viewSs.IncreaseFailedResult();
                    if (endStat.SystemStatus == CpSystemStatus.SKIP) viewSs.ManualStop = true;
                    viewSs.StartSgPostTest();
                    TheProcessManager.CurrentTasks.Remove(procEvt.Process);
                }
                else if (endStat.SystemStatus == CpSystemStatus.STOP)
                {
                    viewSs.CloseTest();
                    viewSs.SgStnStatus = CpStationStatus.IDLE;
                    viewSs.ChangeSystemStatus(endStat.SystemStatus);
                    viewSs.frmSgTStepStatus.ChangeStatus(procEvt.SystemStatus, "Test Stopped", "Spend Time = " + procEvt.TestEndStatus.SpentTime + " ms");
                    TheProcessManager.CurrentTasks.Remove(procEvt.Process);
                }
                else if (endStat.SystemStatus == CpSystemStatus.OK || endStat.SystemStatus == CpSystemStatus.FINISH)
                {
                    switch (endStat.StationStatus)
                    {
                        case CpStationStatus.PRE_FINISHED:
                            TheProcessManager.CurrentTasks.Remove(procEvt.Process);
                            viewSs.StartSgMainTest();
                            break;
                        case CpStationStatus.MAIN_FINISHED:
                            TheProcessManager.CurrentTasks.Remove(procEvt.Process);
                            viewSs.frmSgTStepStatus.ChangeStatus(procEvt.SystemStatus, "Pass", "Spend Time = " + procEvt.TestEndStatus.SpentTime + " ms");
                            viewSs.IncreaseSuceededResult();
                            viewSs.StartSgPostTest();
                            break;
                        case CpStationStatus.POST_FINISHED:
                            TheProcessManager.CurrentTasks.Remove(procEvt.Process);
                            viewSs.CloseTest();
                            viewSs.SgStnStatus = CpStationStatus.IDLE;
                            viewSs.RepeatRun();
                            break;
                    }
                }

                if (endStat.StationStatus == CpStationStatus.PRE_RUN)
                {
                    viewSs.MngStation.StationStatus = CpSystemStatus.PRE_TASK_RUN;
                    viewSs.frmSgTStepStatus.ChangeStatus(CpSystemStatus.PRE_TASK_RUN, "");
                }

                if (procEvt.SystemStatus == CpSystemStatus.PAUSE && endStat.StationStatus == CpStationStatus.MAIN_RUN)
                {
                    if (viewSs.MngStation.StationStatus == CpSystemStatus.PAUSE)
                    {
                        viewSs.MngStation.StationStatus = CpSystemStatus.RUN;
                        viewSs.frmSgTStepStatus.ChangeStatus(CpSystemStatus.RUN, "");
                    }
                    else
                    {
                        viewSs.MngStation.StationStatus = CpSystemStatus.PAUSE;

                        if (procEvt.TestEndStatus.EndStepNum != 0)
                            viewSs.frmSgTStepStatus.ChangeStatus(CpSystemStatus.PAUSE, "Paused: Break Point Detected at a Test Step", "Step: " + procEvt.TestEndStatus.EndStepNum);
                        else
                            viewSs.frmSgTStepStatus.ChangeStatus(CpSystemStatus.PAUSE, "Test Paused");
                    }
                }

                if (procEvt.SystemStatus == CpSystemStatus.NEXT_STEP && endStat.StationStatus == CpStationStatus.MAIN_RUN)
                {
                    if (viewSs.MngStation.StationStatus == CpSystemStatus.RUN)
                        return;

                    if (procEvt.TestEndStatus.EndStepNum != 0)
                        viewSs.frmSgTStepStatus.ChangeStatus(CpSystemStatus.PAUSE, "Paused: at a Current Test Step", "Step: " + procEvt.TestEndStatus.EndStepNum);
                }
            }
        }



        private async void barButtonItemLoadCfgFrFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormAdmin.DoModal()) return;
            using (OpenFileDialog opFileDlg = new OpenFileDialog())
            {
                if (opFileDlg.ShowDialog() == DialogResult.OK)
                {
                    await InitCPTester(opFileDlg.FileName);
                }
            }
        }

        private void barButtonItemExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barCheckItemDebugConsoleOut_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItemDebugConsoleOut.Checked)
            {
                Controls.Remove(ribbonStatusBarCPMainForm);
                Controls.Add(dockPanel_Output);
                Controls.Add(ribbonStatusBarCPMainForm);
            }
            else
            {
                Controls.Remove(dockPanel_Output);
            }
        }

        private void WriteOutputText(string strMessage, ConsoleColor csColor = ConsoleColor.White, CpDefineEnumDebugPrintLogLevel logLevel = CpDefineEnumDebugPrintLogLevel.DEBUG)
        {
            if (!InvokeRequired && !IsHandleCreated)
                UpdateLog(strMessage, logLevel);
            else
                /* !!!!! do not await */
                this.DoAsync(() =>
                {
                    UpdateLog(strMessage, logLevel);
                });
        }

        private void UpdateLog(string strMessage, CpDefineEnumDebugPrintLogLevel logLevel)
        {
            if (listBoxControl_Output.Items.Count > 10000)
                FileWriteLog();

            Color color;
            switch (logLevel)
            {
                case (CpDefineEnumDebugPrintLogLevel.NONE): color = Color.Black; Logger.Info(strMessage); break;
                case (CpDefineEnumDebugPrintLogLevel.FATAL): color = Color.Red; Logger.Error(strMessage); break;
                case (CpDefineEnumDebugPrintLogLevel.DEBUG): color = Color.Orange; Logger.Debug(strMessage); break;
                case (CpDefineEnumDebugPrintLogLevel.INFO): color = Color.Green; Logger.Info(strMessage); break;
                case (CpDefineEnumDebugPrintLogLevel.LAST): color = Color.Blue; Logger.Info(strMessage); break;
                default: color = Color.Black; break;
            }

            string[] arrMessage = strMessage.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //foreach (string message in arrMessage)
            listBoxControl_Output.Items.Add(string.Format("<color={3}>{0} [{1}]: {2}</color>", DateTime.Now, logLevel, arrMessage[0], color.Name));

            listBoxControl_Output.SelectedIndex = listBoxControl_Output.Items.Count - 1;
        }

        private void FileWriteLog()
        {
            string strCrtTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            FileStream filestreamOutput = new FileStream(strCrtTime + "_WindowOutput.txt", FileMode.Create);
            StreamWriter streamwriterOutput = new StreamWriter(filestreamOutput);
            for (int i = 0; i < listBoxControl_Output.Items.Count; i++)
            {
                string log = listBoxControl_Output.GetItemText(i);
                if (log.Contains(">"))
                    streamwriterOutput.WriteLine(log.Split(new char[] { '>' }, 2)[1].Replace(@"</color>", string.Empty));
            }
            streamwriterOutput.AutoFlush = true;
            listBoxControl_Output.Items.Clear();
            filestreamOutput.Close();
        }

        private void FormAppSs_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Keyboard.IsShiftKeyPressed)
            {
                FormDeveloper.DoModal();
                e.Cancel = true;
                return;
            }
            if (Keyboard.IsControlKeyPressed)
            {
                Globals.IsDeveloperMode = false;
                e.Cancel = true;
                return;
            }

            if (XtraMessageBox.Show(this, "Exit Application?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Mainform 이 닫힘을 공지
                Globals.ApplicationExitSubject.OnNext(true);

                SaveTestCountInfo();

                CloseDevices();
                SaveLayout();
            }
            else
                e.Cancel = true;
        }

        private void SaveTestCountInfo()
        {
            if (TheApplication == null)
                return;
            string strFileName = Directory.GetCurrentDirectory() + TheApplication.CnfApp.AppConfigure.CnfPath + "TEST_INFO.stn";
         

            List<string> lstStationTestCount = new List<string>();
            foreach (CpStnManager stnMng in TheApplication.Station)
                lstStationTestCount.Add(string.Format("{0};{1};{2}", stnMng.StationId, stnMng.TsCount, stnMng.NgCount));

            FileStream filestream = new FileStream(strFileName, FileMode.Create);
            StreamWriter streamwriterOutput = new StreamWriter(filestream);
            for (int i = 0; i < lstStationTestCount.Count; i++)
                streamwriterOutput.WriteLine(lstStationTestCount[i]);
            streamwriterOutput.AutoFlush = true;
            filestream.Close();
        }


        private void  UpdateTestCountInfo()
        {
            string strFileName = Directory.GetCurrentDirectory() + TheApplication.CnfApp.AppConfigure.CnfPath + "TEST_INFO.stn";
            List<string> testdata = new List<string>();
            if (!File.Exists(strFileName))
                return;

            using (StreamReader reader = new StreamReader(strFileName))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    testdata.Add(line);
                }
            }


            foreach (var info in testdata)
            {
                CpStnManager stn = TheApplication.Station.Where(w => w.StationId.ToString() == info.Split(';')[0]).FirstOrDefault();
                if (stn == null || !stn.CnfStation.Enable)
                    continue;

                stn.TsCount = Convert.ToInt32(info.Split(';')[1]) - 1;
                stn.NgCount = Convert.ToInt32(info.Split(';')[2]) - 1;

                UcMainViewSs viewSs = GetStationForm(stn.StationId);
                viewSs.IncreaseSuceededResult();
                viewSs.IncreaseFailedResult();


            }


        }


        private void barButtonItemMnCtrLogTStepAction_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLogTStepAction frmLogViewer = new frmLogTStepAction(GetAllStnMng());

            frmLogViewer.Show();
        }

        private void barButtonItemMnCtrLogTStepMeasuring_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLogTStepMeasuring frmLogViewer = new frmLogTStepMeasuring(GetAllStnMng());

            frmLogViewer.Show();
        }

        private void barButtonItemMnCtrPS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Define code to control a power supply manually.
            UpdateLog("Define code to control a power supply manually.", CpDefineEnumDebugPrintLogLevel.FATAL);
        }

        private void barButtonItemMnCtrTCPIP_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Define code to control TCP/IP communication manually.
            UpdateLog("Define code to control TCP/IP communication manually.", CpDefineEnumDebugPrintLogLevel.FATAL);
        }

        private void barButtonItemMnCtrManualSignalAnalysis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new Dsu.Driver.UI.NiDaq.FormNiDaqExplorer().Show();
            //Define code to control Manual Signal Analysis Function using DAQ Devices.
            //UpdateLog("Define code to control Manual Signal Analysis Function using DAQ Devices.", CpDefineEnumDebugPrintLogLevel.FATAL);
        }

        private void barButtonItemMnCtrTsConditon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // read xml file.          
            var sFilePath = Directory.GetCurrentDirectory() + "\\Configure\\AppConfig\\" + ClsGlobalStringForGeneral.FILE_CP_CONFIGUE_NAME;
            if (!File.Exists(sFilePath))
            {
                ShowMessageBox("System Configuration file loading Error.");
                return;
            }

            UiCpConfigure frmConfig = new UiCpConfigure(TheApplication.xDoc, ClsGlobalStringForGeneral.FILE_CP_CONFIGUE_NAME);
            frmConfig.Show();
        }

        private void barButtonItemMnMain_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // new frmManual(MngSystem, MngApplication).Show();
        }

        private void barEditItem_PartID_EditValueChanged(object sender, EventArgs e)
        {
            CpUtil.PartID = barEditItem_PartID.EditValue.ToString();
        }
        //int cou = 0;
        private void barButtonItem_SelectAuto_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          //  ucTestMonitor1.UdpateNgStation(cou++ % 6, "TEST");
         //   if (!FormAdmin.DoModal()) return;
            AutoMode = true;
            simpleButton_AutoManu.Text = "  AUTO  ";
        }

        private void barButtonItem_SelectManual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
         //   if (!FormAdmin.DoModal()) return;
            AutoMode = false;
            simpleButton_AutoManu.Text = "MANUAL";
        }

        private async Task InitializeProcess()
        {
            await this.DoAsync(() =>
            {
                UpdateMainLabelText();
                barHeaderItemVersion.Caption = "CP-Tester Version: " + GetVersion();

                foreach (string strLogLevel in Enum.GetNames(typeof(CpDefineEnumDebugPrintLogLevel)))
                    repositoryItemComboBox2.Items.Add(strLogLevel);

                barEditItemShowLogLevel.EditValue = UtilTextMessageEdits.PrintLogLevel.ToString();
                barEditItem_PartID.EditValue = "PartID";

                if (CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.MOTION))
                    InitializeRobotPath();
                if (CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.TRIGGER_IO))
                {
                    var mngTempHumi = CpUtil.GetManagerDevice(TheSystemManager, CpDeviceType.TRIGGER_IO) as CpMngTriggerIO;
                    CurrentHumidity = mngTempHumi.GetHumidity();
                    CurrentTemerature = mngTempHumi.GetTemperature();
                }

                if (AutoStartPLC)
                    labelControl_Message.Visible = false;
                else
                    labelControl_Message.Visible = true;

                 ManualUIVisible();
            });

        }

        public void UpdateMainLabelText()
        {
            this.DoAsync(() =>
            {
                string sTTNR = TheApplication.Station.Where(s => s.CnfStation.Enable).FirstOrDefault().MngTStep.GaudiReadData.TestListInfo.PartNum;
                labelControlTitle.Text = string.Format("{0} [{1}]", TheApplication.CnfApp.SystemInfoConfigure.Title, sTTNR);
            });
        }

        private void action1_Update(object sender, EventArgs e)
        {
            barEditItem_AirGapMonitor.EditValue = CpUtil.AirGapOffSet + CpUtil.AirGapDefault;

            barButtonItemMnCtrStart.Enabled = !Starting;
            barButtonItemMnCtrStop.Enabled = Starting || _robotReadyMoving;
            barButtonItem_SelectAuto.Enabled = !AutoMode && !Starting;
            barButtonItem_SelectManual.Enabled = AutoMode && !Starting;
            barButtonItem_RobotReadyMove.Enabled = !Starting && !_robotReadyMoving;

            barButtonItem_PLC.Enabled = !AutoMode;
            barButtonItem_LVDT.Enabled = !AutoMode;
            barButtonItem_PowerSupply1.Enabled = !AutoMode;
            barButtonItem_PowerSupply2.Enabled = !AutoMode;
            barButtonItem_PowerSupply3.Enabled = !AutoMode;
            barButtonItem_LCR.Enabled = !AutoMode;
            barButtonItem_DigtalIO.Enabled = !AutoMode;
            barButtonItem_AnalogIO.Enabled = !AutoMode;
            barButtonItem_Motor1.Enabled = !AutoMode;
            barButtonItem_Motor2.Enabled = !AutoMode;
            barButtonItem_Motor3.Enabled = !AutoMode;
            barButtonItem_Robot.Enabled = !AutoMode;
            barButtonItem_TempHumi.Enabled = !AutoMode;

            barEditItem_TemperatureLower.Enabled = !AutoMode;
            barEditItem_TemperatureUpper.Enabled = !AutoMode;
            barEditItem_HumidityLower.Enabled = !AutoMode;
            barEditItem_HumidityUpper.Enabled = !AutoMode;

            ribbonPageGroupDoor.Visible = Globals.IsDeveloperMode;
        }

        public async void ShowErrorFormShortly(string description, int expire = 5000)
        {
            await TheMainForm.DoAsync(() =>
            {
                var errorForm = FormError.TheForm;
                LogError(description);
                if (errorForm == null)
                {
                    errorForm = new FormError(description);
                    errorForm.DoModal();
                }
                else
                {
                    errorForm.Visible = true;
                    errorForm.AddReason(description);
                }
            });
        }

        private async void barButtonItem_LoadTestList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
                FormDeveloper.DoModal();

            FormAppSs.ResetCancellationTokenSource();

            OpenAuditMode();
            LoadLayout();

            await StartInitialProcess();
        }

    }
}
