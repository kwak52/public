using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using CpTesterPlatform.CpTStepDev;
using DevExpress.XtraEditors;
using Dsu.Common.Utilities.DX;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities;
using Dsu.Driver.Base;
using Dsu.Driver.Paix;
using Dsu.Driver.Util.Emergency;
using static CpBase.CpLog4netLogging;
using static LanguageExt.FSharp;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualRobotAudit78
        //: Form  // design time
        : FormManualRobotAudit
    {
        public enum AxisEnumAudit78 { X = 0, Y, Z, Tilt };
        private IEnumerable<short> AllAxes => SelectedMng.GetAxesList();

        private double LEAD { get { return Convert.ToDouble(((ClsMotionInfo)SelectedMng.DeviceInfo).CFG_DISTANCE_PER_REVOLUTION); } }
        private System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();

        private List<string> lstPath = new List<string>();
        protected override ComboBoxEdit ComboEditPath { get { return comboBoxEdit_Path; } }

        public FormManualRobotAudit78()
        {
            if (!DriverBaseGlobals.TesterType.IsOneOf(CpTesterEnum.A_78KGP, CpTesterEnum.A_78KVP))
                throw ExceptionWithCode.Create(ErrorCodes.APP_ConfigurationError, $"Tester type {DriverBaseGlobals.TesterType} mismatch.");

            InitializeComponent();
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
            var subscription = System.Reactive.Linq.Observable.Interval(TimeSpan.FromMilliseconds(250))
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
                                UpdateLabelIndicator(toolStripStatusLabelX, encPositions[(int)AxisEnumAudit78.X]);
                                UpdateLabelIndicator(toolStripStatusLabelY, encPositions[(int)AxisEnumAudit78.Y]);
                                UpdateLabelIndicator(toolStripStatusLabelZ, encPositions[(int)AxisEnumAudit78.Z]);
                                UpdateLabelIndicator(toolStripStatusLabelTilt, encPositions[(int)AxisEnumAudit78.Tilt]);

                                // command position
                                var cmdPositions = allAxes.Select(ax => String.Format("{0:n0}", SelectedMng.GetCmdPos(ax))).ToArray();
                                UpdateLabelIndicator(toolStripStatusLabelCmdX, cmdPositions[(int)AxisEnumAudit78.X]);
                                UpdateLabelIndicator(toolStripStatusLabelCmdY, cmdPositions[(int)AxisEnumAudit78.Y]);
                                UpdateLabelIndicator(toolStripStatusLabelCmdZ, cmdPositions[(int)AxisEnumAudit78.Z]);
                                UpdateLabelIndicator(toolStripStatusLabelCmdTilt, cmdPositions[(int)AxisEnumAudit78.Tilt]);

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

        private async void ucManuMotion_Load(object sender, EventArgs arg)
        {
            statusStrip1.Visible = false;
            statusStrip2.Visible = false;

            OnLoad();
            //UpdateToolstripCmdEnc();

            await UnparkingAudit78();

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
            foreach (var m in MotionDeviceManagers)
                m.StopMotion();
        }

        private void simpleButton_Emergency_Click(object sender, EventArgs e)
        {
            // on emergency, we need to stop everything...
            //if (SelectedMng == null)
            //    return;

            EmergencyStop();
        }


        private void simpleButton_HomeSetPos_Click(object sender, EventArgs e)
        {
            if (SelectedMng == null)
                return;

            if (CheckMultiRobot())
            {
                SelectedMng.SetCommandEncorderZeroSetting(null);
                PaixManagerBase.IsOriginCalibrated = true;
            }
        }

        private bool _forceHomeSetExecuted = false;
        public void ForceHomeSetOnDeveloperMode()
        {
            Debug.Assert(Globals.IsDeveloperMode);
            if (FormAppSs.HasAlarmClearHistory)
            {
                ShowMessageBox("Alarm clear executed, so you must run home move anyway.");
                return;
            }

            var allAxesHomeMoved =
                LanguageExt.Prelude.match(fs(RobotDeviceManager.PaixRobot.Paix.GetHomeStatus()),
                    Some: v => AllAxes.ForAll(ax => v.nStatusFlag[ax] == 0),
                    None: () => false);

            if ( !allAxesHomeMoved )
            {
                ShowMessageBox("No home move history, so you must run home move anyway.");
                return;
            }


            // should *not* call SetCommandEncorderZeroSetting()
            if (AskMessageBox("Is it OK to assume homeset already done?") == DialogResult.Yes)
            {
                if (SelectedMng.GetCmdPositions().ForAll(p => Math.Abs(p) < 10))
                {
                    if (AskMessageBox("Robot seems to be rebooted.  Is it really in KISS origin?") != DialogResult.Yes)
                        return;
                }

                PaixManagerBase.IsOriginCalibrated = true;
                _forceHomeSetExecuted = true;
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

        private void simpleButton_EditPath_Click(object sender, EventArgs e) => OnEditPath();

        private async void simpleButton_PathRun_Click(object sender, EventArgs e)
        {
            try
            {
                await RunSelectedPath();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Failed to run path: {ex.Message}");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                simpleButton_HomeSetPos.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibratable;

                simpleButton_MoveInput.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && CpSignalManager.IsPart8;
                simpleButton_MoveMiddle.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && CpSignalManager.IsPart8;
                simpleButton_MoveOutput.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && CpSignalManager.IsPart8;
                simpleButton_MoveOddEven.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated && CpSignalManager.IsPart7;

                if (Globals.IsDeveloperMode)
                {
                    layoutControlGroup_Move.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem_RunPath.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem_comboBox.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem_forcehome.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
                else
                {
                    layoutControlGroup_Move.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem_RunPath.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem_comboBox.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem_forcehome.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }

                simpleButton_MoveInput.Visible = Globals.IsDeveloperMode;
                simpleButton_MoveMiddle.Visible = Globals.IsDeveloperMode;
                simpleButton_MoveOutput.Visible = Globals.IsDeveloperMode;
                simpleButton_MoveOddEven.Visible = Globals.IsDeveloperMode;
                simpleButton_PathRun.Visible = Globals.IsDeveloperMode;

                var optMoving0 = SelectedMng?.IsMove((int)AxisEnumAudit78.X);
                var optMoving1 = SelectedMng?.IsMove((int)AxisEnumAudit78.Y);
                var optMoving2 = SelectedMng?.IsMove((int)AxisEnumAudit78.Z);
                var optMoving3 = SelectedMng?.IsMove((int)AxisEnumAudit78.Tilt);

                simpleButton_PlayAxisX.Enabled = optMoving0.HasValue && optMoving0.Value;
                simpleButton_PlayAxisY.Enabled = optMoving1.HasValue && optMoving1.Value;
                simpleButton_PlayAxisZ.Enabled = optMoving2.HasValue && optMoving2.Value;
                simpleButton_PlayAxisR.Enabled = optMoving3.HasValue && optMoving3.Value;


                simpleButton_MoveReady.Enabled = !_homeMoving && PaixManagerBase.IsOriginCalibrated;
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
                btnDeveloperModeDialog.Visible = Globals.IsDeveloperMode && !_forceHomeSetExecuted;

            }
            catch (Exception ex)
            {
                LogError($"Exception on ucManuRobot.timer1_Tick: {ex.Message}");
            }
        }

        private async void simpleButton_HomeMove_Click(object sender, EventArgs e)
        {
            _cts = new CancellationTokenSource();
            await HomeMove(_cts.Token, true);
        }

        /// 1. Goto Robot Home 
        /// 2. Reset coordinates system
        /// 3. Goto Origin point
        /// 4. Reset coordinates system
        public async Task HomeMove(CancellationToken token, bool bGotoKissHome)
        {
            try
            {
                using (var waitor = new SplashScreenWaitor(this))
                {
                    var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, token).Token;
                    waitor.ProgressDescription = "Home moving..";
                    //waitor.WaitForm.Location = this.Location;

                    HomeMovePrologue();

                    LogInfo("Moving Robot HOME..");
                    LogInfo("\tUnbreaking..");

                    SetDioOn(SignalEnum.UBreakTilt, bWriteAlways: false);
                    SetDioOn(SignalEnum.UBreakZ, bWriteAlways: false);

                    LogInfo("Home moving..");
                    var resetCmdEncPosition = bGotoKissHome;
                    var succeeded = await SelectedMng.HomeMove(linkedToken, resetCmdEncPosition);
                    if (!succeeded)
                    {
                        LogErrorWithMessageBox("Home move failed.");
                        return;
                    }


                    if (bGotoKissHome)
                    {
                        if (linkedToken.IsCancellationRequested)
                        {
                            LogErrorWithMessageBox("Home move canceled..");
                            return;
                        }
                        else
                            LogInfo("Finished moving Robot HOME..");

                        //if (CpDevPaixRobot.HomeMoved)        // ghost effect ????????
                        {
                            LogInfo("Moving to origin..");
                            // Move to kiss origin, and set cmd, enc position

                            await SelectedMng.MovePath(FormAppSs.ToOriginPoseName, linkedToken);     // "ToOrg_Vietnam" or "ToOrg_Gunpo";

                            //LogInfo("\tBreaking..");
                            //dio.SetDOutState(ioIndex, false);       // break

                            LogInfo("Finished moving to origin..");

                            FormAppSs.HasAlarmClearHistory = false;
                        }
                        //else
                        //    LogError($"Home moved marker not found. Value={CpDevPaixRobot.HomeMoved}");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                    LogError($"Home move canceled..");
                else
                    LogErrorWithMessageBox($"Home move failed.", ex);
            }
            finally
            {
                _homeMoving = false;
            }
        }

        private async Task OnPathMoveButtonClick(string pathName)
        {
            try
            {
                _cts = new CancellationTokenSource();
                await MovePathAsync(pathName, _cts.Token);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }


        private async void simpleButton_MoveInput_Click(object sender, EventArgs e) { await OnPathMoveButtonClick("ReadyToInput"); }
        private async void simpleButton_MoveMiddle_Click(object sender, EventArgs e) { await OnPathMoveButtonClick("ReadyToMiddle"); }
        private async void simpleButton_MoveOutput_Click(object sender, EventArgs e) { await OnPathMoveButtonClick("ReadyToOutput"); }
        private async void simpleButton_MoveOddEven_Click(object sender, EventArgs e) { await OnPathMoveButtonClick("7DCT_ReadyToOddEven"); }
        private async void simpleButton_MoveReady_Click(object sender, EventArgs e) { await OnPathMoveButtonClick("ToReady"); }

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

            if (!FormDeveloper.DoModal())
                return;

            //statusStrip1.Visible = true;
            //statusStrip2.Visible = true;

            if (CheckMultiRobot())
            {
                SelectedMng.OpenManualDialog();
            }
        }


        public override async Task MoveToRobotOrigin(CancellationToken token)
        {
            if (!PaixManagerBase.IsOriginCalibrated)
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot origin not calibrated.");

            var toOriPose = RobotDeviceManager.PaixRobot.Poses[FormAppSs.ToOriginPoseName];
            var y = -1 * toOriPose.Select(p => p.V1).Max();
            var z = -1 * toOriPose.Select(p => p.V2).Max();
            var t = -1 * toOriPose.Select(p => p.V3).Max();
            var x = -1 * toOriPose.Select(p => p.V0).Max();
            Trace.WriteLine($"{x}, {y}, {z}, {t}");

            await UntilAxesStoppedAsync(token);
            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.Z, z);
            await UntilAxesStoppedAsync(token);

            token.ThrowIfCancellationRequested();

            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.X, x);
            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.Y, y);
            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.Tilt, t);

            await UntilAxesStoppedAsync(token);
        }

        
        public async Task MoveToKissOrigin(CancellationToken token)
        {
            if (!PaixManagerBase.IsOriginCalibrated)
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot origin not calibrated.");

            await UntilAxesStoppedAsync(token);

            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.X, 0);
            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.Y, 0);
            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.Tilt, 0);

            await UntilAxesStoppedAsync(token);

            token.ThrowIfCancellationRequested();
            RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.Z, 0);
            await UntilAxesStoppedAsync(token);
        }




        private static async Task BreakOnAudit78(bool bBreakOn)
        {
            if (!DriverBaseGlobals.IsAudit78())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot parking/unparking only allowed on Audit78.");

            if (bBreakOn)
            {
                CpSignalManager.TheCpSignalManager.BreakDevicesOn();
                await Task.Delay(500);
                RobotDeviceManager.BreakServoChange(false);
            }
            else
            {
                RobotDeviceManager.BreakServoChange(true);
                await Task.Delay(500);
                CpSignalManager.TheCpSignalManager.BreakDevicesOff();
            }
        }

        public static async Task ParkingAudit78() => await BreakOnAudit78(true);
        public static async Task UnparkingAudit78(bool adjustEncPositions=true)
        {
            if (adjustEncPositions)
            {
                var axes = new[] { (short)AxisEnumAudit78.Z, (short)AxisEnumAudit78.Tilt };
                var axesEncs = axes.Select(ax => new { Axis = ax, Enc = RobotDeviceManager.PaixRobot.GetEncPos(ax) }).ToArray();
                await BreakOnAudit78(false);
                axesEncs.ForEach(info => RobotDeviceManager.PaixRobot.Paix.SetCmdPos(info.Axis, info.Enc));
            }
            else
                await BreakOnAudit78(false);
        }

        private async void FormManualRobotAudit78_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDeveloperModeInvoke())
            {
                e.Cancel = true;
                return;
            }

            if (!PaixManagerBase.IsOriginCalibrated)
            {
                ShowMessageBox("Robot Origin Calibration is not excute", "Robot Error");
                e.Cancel = true;
                return;
            }

            DialogResult = DialogResult.OK;

            await ParkingAudit78();

            timer1.Stop();
        }
    }
}
