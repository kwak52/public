using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using Dsu.Common.Utilities.ExtensionMethods;
using CpTesterPlatform.CpTStepDev;
using System.IO;
using CpTesterPlatform.CpCommon;
using System.Threading;
using Dsu.Driver.Util.Emergency;
using Dsu.Driver.Paix;
using static CpFunctionDefault.Util.CpLog4netLogging;
using System.Reactive.Linq;
using System.Diagnostics;

namespace CpTesterPlatform.CpTester
{
    public partial class frmManuRobot : Form
    {
        private List<CpMngMotion> DevMgrSet = new List<CpMngMotion>();
        private List<CpMngDIOControl> lstDioDevMgr = new List<CpMngDIOControl>();
        private CpMngMotion SelectedMng;
        private double LEAD { get { return Convert.ToDouble(((ClsMotionInfo)SelectedMng.DeviceInfo).CFG_DISTANCE_PER_REVOLUTION); } }
        private System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private List<string> lstPath = new List<string>();
        private Image imgOn;
        private Image imgOff;

        public frmManuRobot(List<IDevManager> lstDevMgr, List<IDevManager> lstDevMgrDio)
        {
            InitializeComponent();

            foreach (var mng in lstDevMgrDio)
                lstDioDevMgr.Add(mng as CpMngDIOControl);

            foreach (var mng in lstDevMgr)
                DevMgrSet.Add(mng as CpMngMotion);



            imgOn = Image.FromFile(Directory.GetCurrentDirectory() + "\\IMG\\LedOn.ico");
            imgOff = Image.FromFile(Directory.GetCurrentDirectory() + "\\IMG\\LedOff.ico");
        }

        private void UpdateLabelIndicator(ToolStripStatusLabel label, string value)
        {
            if (label.Text == value)
                label.BackColor = SystemColors.Control;
            else
            {
                label.BackColor = Color.Pink;
                label.Text = value;
            }
        }


        private void UpdateToolstripCmdEnc()
        {
            var allLabels = new[] { toolStripStatusLabelX, toolStripStatusLabelY, toolStripStatusLabelZ, toolStripStatusLabelTilt,
                                    toolStripStatusLabelCmdX, toolStripStatusLabelCmdY, toolStripStatusLabelCmdZ, toolStripStatusLabelCmdTilt};

            // 정해진 주기로 status strip 에 X, Y, Z, tilt 의 좌표를 갱신함.
            var subscription = Observable.Interval(TimeSpan.FromMilliseconds(250))
                .Subscribe(async (tick) =>
                {
                    if (SelectedMng == null)
                    {
                        allLabels.ForEach(l => { l.Text = "?"; l.BackColor = SystemColors.Control; });
                    }
                    else
                    {
                        try
                        {
                            await this.DoAsync(() =>
                            {
                                var allAxes = SelectedMng.GetAxesList();
                                // Encoder position
                                var encPositions = allAxes.Select(ax => String.Format("{0:n0}", SelectedMng.GetEncPos(ax))).ToArray();
                                UpdateLabelIndicator(toolStripStatusLabelX, encPositions[0]);
                                UpdateLabelIndicator(toolStripStatusLabelY, encPositions[1]);
                                UpdateLabelIndicator(toolStripStatusLabelZ, encPositions[2]);
                                UpdateLabelIndicator(toolStripStatusLabelTilt, encPositions[3]);

                                // command position
                                var cmdPositions = allAxes.Select(ax => String.Format("{0:n0}", SelectedMng.GetCmdPos(ax))).ToArray();
                                UpdateLabelIndicator(toolStripStatusLabelCmdX, cmdPositions[0]);
                                UpdateLabelIndicator(toolStripStatusLabelCmdY, cmdPositions[1]);
                                UpdateLabelIndicator(toolStripStatusLabelCmdZ, cmdPositions[2]);
                                UpdateLabelIndicator(toolStripStatusLabelCmdTilt, cmdPositions[3]);

                                // current speed
                                //var speeds = _manager.GetAxesEncSpeed().Value;
                                //Trace.WriteLine($"Speed = {speeds[0]}, {speeds[1]}, {speeds[2]}, {speeds[3]}");
                            });
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"{ex}");
                        }
                    }
                });

            FormClosed += (s, e) => subscription.Dispose();
        }

        private void ucManuMotion_Load(object sender, EventArgs arg)
        {
            if (DevMgrSet.Count > 0)
                SelectedMng = DevMgrSet[0]; //test ahn
            else
            {
                Close();
                return;
            }

            statusStrip1.Visible = false;
            statusStrip2.Visible = false;
            //UpdateToolstripCmdEnc();

            foreach (string path in SelectedMng.GetPath())
            {
                if (path.ToUpper() == "TO ORG") continue;
                    comboBoxEdit_Path.Properties.Items.Add(path);
            }

            var subscription =
                SignalManager.FilteredSignalSubject
                .Subscribe(fs =>
                {
                    switch(fs.Enum)
                    {
                        case SignalEnum.UEmergency:
                            if (fs.Value)
                                _cts = new CancellationTokenSource();
                            else
                                _cts.Cancel();

                            break;

                        case SignalEnum.UOriginSensor:
                            break;
                    }
                });

            FormClosed += (s, e) => subscription.Dispose();

            timer1.Interval = 500;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
        }



        private void action1_Update(object sender, System.EventArgs e)
        {
            //if (SelectedMng == null)
            //    return;

            //simpleButton_Stop.Enabled = SelectedMng.IsOpened;
            //simpleButton_Go.Enabled = SelectedMng.IsOpened;
            //simpleButton_Pause.Enabled = SelectedMng.IsOpened;
            //simpleButton_Back.Enabled = SelectedMng.IsOpened;
            //simpleButton_HomeSetPos.Enabled = SelectedMng.IsOpened;
            //simpleButton_Emergency.Enabled = SelectedMng.IsOpened;
        }



        private void simpleButton_Stop_Click(object sender, EventArgs e)
        {
            //if (SelectedMng == null)
            //    return;
            foreach (var m in DevMgrSet)
                m.StopMotion();
        }

        private void simpleButton_Emergency_Click(object sender, EventArgs e)
        {
            // on emergency, we need to stop everything...
            //if (SelectedMng == null)
            //    return;

            _cts.Cancel();
            foreach (var m in DevMgrSet)
                m.StopMotionEmergency();
        }


        private void CMD_ButtonClick(bool backward)
        {
            if (SelectedMng == null)
                return;

            SelectedMng.SetDirection(!backward);
            SelectedMng.SetParametor(100 * 60 / LEAD
                , 100 * 60 / LEAD
                , 100 * 60 / LEAD);


            SelectedMng.SetJogMove();

            //switch (comboBoxEdit_MODE.SelectedItem.ToString())
            //{
            //    case "JOG": SelectedMng.SetJogMove(); break;
            //    case "INC": SelectedMng.SetRelMove(Convert.ToDouble(numericTextBox_CMD_Distance.Text) / LEAD); break;
            //    case "ABS": SelectedMng.SetAbsMove(Convert.ToDouble(numericTextBox_CMD_Distance.Text) / LEAD); break;
            //}
        }



        private void simpleButton_HomeSetPos_Click(object sender, EventArgs e)
        {
            if (SelectedMng == null)
                return;

            if (CheckMultiRobot())
            {
                SelectedMng.SetCommandEncorderZeroSetting();
                PaixManagerBase.IsOriginCalibrated = true;
            }
        }

        private void buttonEdit1_Properties_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (SelectedMng == null)
                return;
            if (!CheckMultiRobot())
                return;

            OpenFileDialog opendialog = new OpenFileDialog();
            opendialog.Multiselect = true;
            opendialog.Filter = "path Files (*.poses)|*.poses";
            if (opendialog.ShowDialog() != DialogResult.OK)
                return;

            lstPath = opendialog.FileNames.ToList();
            SelectedMng.SetModePath(lstPath);
        }

        private void simpleButton_EditPath_Click(object sender, EventArgs e)
        {
            if (SelectedMng == null)
                return;

            if (CheckMultiRobot())
            {
                SelectedMng.OpenManualDialog();
            }
        }

        private async void simpleButton_PathRun_Click(object sender, EventArgs e)
        {
            if (SelectedMng == null)
                return;

            if (comboBoxEdit_Path.SelectedItem == null)
                return;

            if (CheckMultiRobot())
            {
                var path = comboBoxEdit_Path.SelectedItem.ToString();
                try
                {
                    _cts = new CancellationTokenSource();
                    await SelectedMng.MovePath(path, _cts.Token);
                }
                catch (Exception ex)
                {
                    if (ex is TaskCanceledException)
                        LogError($"Move cancel on path={path}");
                    else
                        LogError($"Error on moving path={path}");
                }
            }
        }

        private bool CheckMultiRobot()
        {
            if (!((ClsMotionInfo)SelectedMng.DeviceInfo).AXIS_ID.Contains(';'))
            {
                UtilTextMessageBox.UIMessageBoxForWarning("Robot Error", "Select Multi Axis Device");
                return false;
            }
            else
                return true;
        }

        private void frmManuRobot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( ! Dsu.Common.Utilities.Globals.IsDeveloperMode )
            {
                if (new frmAdmin().ShowDialog() != DialogResult.OK)
                {
                    if (!PaixManagerBase.IsOriginCalibrated)
                    {
                        UtilTextMessageBox.UIMessageBoxForWarning("Robot Error", "Origin Calibration is not excute");
                        e.Cancel = true;
                    }
                }
            }

            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                simpleButton_HomeSetPos.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibratable;

                simpleButton_MoveInput.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && PaixManagerBase.IsPart8;
                simpleButton_MoveMiddle.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && PaixManagerBase.IsPart8;
                simpleButton_MoveOutput.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && PaixManagerBase.IsPart8;
                simpleButton_MoveOddEven.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && PaixManagerBase.IsPart7;

                var optMoving0 = SelectedMng.IsMove(0);
                var optMoving1 = SelectedMng.IsMove(1);
                var optMoving2 = SelectedMng.IsMove(2);
                var optMoving3 = SelectedMng.IsMove(3);

                simpleButton_PlayAxisX.Enabled = optMoving0.HasValue && optMoving0.Value;
                simpleButton_PlayAxisY.Enabled = optMoving1.HasValue && optMoving1.Value;
                simpleButton_PlayAxisZ.Enabled = optMoving2.HasValue && optMoving2.Value;
                simpleButton_PlayAxisR.Enabled = optMoving3.HasValue && optMoving3.Value;


                simpleButton_MoveReady.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated;
                if (Dsu.Common.Utilities.Globals.IsDeveloperMode)
                    simpleButton_PathRun.Enabled = true;
                else
                    simpleButton_PathRun.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated;

                simpleButton_HomeMove.Enabled = !_homeMoving;
                simpleButton_XMinus.Enabled = !_homeMoving;
                simpleButton_XPlus.Enabled = !_homeMoving;
                simpleButton_YMinus.Enabled = !_homeMoving;
                simpleButton_YPlus.Enabled = !_homeMoving;
                simpleButton_ZMinus.Enabled = !_homeMoving;
                simpleButton_ZPlus.Enabled = !_homeMoving;
                simpleButton_RMinus.Enabled = !_homeMoving;
                simpleButton_RPlus.Enabled = !_homeMoving;
            }
            catch (Exception ex)
            {
                LogError($"Exception on ucManuRobot.timer1_Tick: {ex.Message}");
            }
        }


        private bool _homeMoving = false;
        /// 1. Goto Robot Home 
        /// 2. Reset coordinates system
        /// 3. Goto Origin point
        /// 4. Reset coordinates system
        private async void simpleButton_HomeMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedMng == null)
                    throw new ExceptionWithCode(ErrorCodes.APP_ConfigurationError, "Can't find robot motion manager.");

                if (_homeMoving)
                    throw new ExceptionWithCode(ErrorCodes.APP_InternalError, "Duplicated home moving operation.");

                if (new frmAdmin().ShowDialog() != DialogResult.OK) return;
                _cts = new CancellationTokenSource();


                _homeMoving = true;
                PaixManagerBase.IsOriginCalibratable = false;
                PaixManagerBase.IsOriginCalibrated = false;
                /*
                 * WARNING : While home move operation, if you push "STOP ALL" in EtherMotion,
                 *           It may causes error.   In that case, cp assumes home move operation completed, and performs next step.
                 */
                LogInfo("Moving Robot HOME..");
                LogInfo("\tUnbreaking..");

                SetDio(CpSignalManager.GetParsedSignal(SignalEnum.UBreakTilt), true);
                SetDio(CpSignalManager.GetParsedSignal(SignalEnum.UBreakZ), true);

                LogInfo("Home moving..");
                var succeeded = await SelectedMng.HomeMove(false, _cts.Token);
                if (!succeeded)
                {
                    LogInfo("Home move failed.");
                    return;
                }


                if (_cts.IsCancellationRequested)
                {
                    LogInfo("Home move canceled..");
                    return;
                }
                else
                    LogInfo("Finished moving Robot HOME..");

                //if (CpDevRobot_PAIXCtrl.HomeMoved)        // ghost effect ????????
                {
                    LogInfo("Moving to origin..");
                    // Move to kiss origin, and set cmd, enc position
                    await SelectedMng.MovePath("TO ORG", _cts.Token);

                    //LogInfo("\tBreaking..");
                    //dio.SetDOutState(ioIndex, false);       // break

                    LogInfo("Finished moving to origin..");
                }
                //else
                //    LogError($"Home moved marker not found. Value={CpDevRobot_PAIXCtrl.HomeMoved}");
            }
            catch (Exception ex)
            {
                _cts = new CancellationTokenSource();
                if (ex is TaskCanceledException)
                    LogError($"Home move canceled..");
                else
                    LogError($"Home move canceled..  Exception={ex}");
            }
            finally
            {
                _homeMoving = false;
            }
        }

        private void SetDio(ParsedSignal parsedSignalT, bool bOn)
        {
            var dio = lstDioDevMgr.FirstOrDefault(d => d.DeviceInfo.Device_ID == parsedSignalT.DeviceId);
            if (dio == null)
                throw new ExceptionWithCode(ErrorCodes.APP_ConfigurationError, "Can't find break IO point.");
            else
                dio.SetDOutState(int.Parse(parsedSignalT.Address), bOn);
        }

        private double _relMoveSpeed = 100;

        private async void simpleButton_MoveInput_Click(object sender, EventArgs e)
        {
            try
            {
                _cts = new CancellationTokenSource();
                await SelectedMng.MovePath("READY TO INPUT", _cts.Token);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private async void simpleButton_MoveMiddle_Click(object sender, EventArgs e)
        {
            try
            {
                _cts = new CancellationTokenSource();
                await SelectedMng.MovePath("READY TO MIDDLE", _cts.Token);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private async void simpleButton_MoveOutput_Click(object sender, EventArgs e)
        {
            try
            {
                _cts = new CancellationTokenSource();
                await SelectedMng.MovePath("READY TO OUTPUT", _cts.Token);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private async void simpleButton_MoveOddEven_Click(object sender, EventArgs e)
        {
            try
            {
                _cts = new CancellationTokenSource();
                await SelectedMng.MovePath("7DCT READY TO ODD EVEN", _cts.Token);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private async void simpleButton_MoveReady_Click(object sender, EventArgs e)
        {
            try
            {
                _cts = new CancellationTokenSource();
                await SelectedMng.MovePath("TO READY", _cts.Token);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private void simpleButton_XMinus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(0, 1000, 1000, 1000);
            SelectedMng.SetRelMove(0, -100);
        }

        private void simpleButton_XPlus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(0, 1000, 1000, 1000);
            SelectedMng.SetRelMove(0, +100);
        }

        private void simpleButton_YMinus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(1, 1000, 1000, 1000);
            SelectedMng.SetRelMove(1, -100);
        }

        private void simpleButton_YPlus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(1, 1000, 1000, 1000);
            SelectedMng.SetRelMove(1, +100);
        }

        private void simpleButton_ZMinus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(2, 1000, 1000, 1000);
            SelectedMng.SetRelMove(2, -100);
        }

        private void simpleButton_ZPlus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(2, 1000, 1000, 1000);
            SelectedMng.SetRelMove(2, +100);
        }

        private void simpleButton_RMinus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(3, 100, 100, 100);
            SelectedMng.SetRelMove(3, -100);
        }

        private void simpleButton_RPlus_Click(object sender, EventArgs e)
        {
            SelectedMng.SetRobotSpeed(3, 100, 100, 100);
            SelectedMng.SetRelMove(3, +100);
        }

    


        private void layoutControlItem10_DoubleClick_1(object sender, EventArgs e)
        {
            if (SelectedMng == null)
                return;

            if (new frmDeveloper().ShowDialog() != DialogResult.OK)
                return;

            //statusStrip1.Visible = true;
            //statusStrip2.Visible = true;

            if (CheckMultiRobot())
            {
                SelectedMng.OpenManualDialog();
            }
        }
    }
}
