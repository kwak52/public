using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Threading;
using Dsu.Driver.Util.Emergency;
using Dsu.Driver.Paix;
using static CpBase.CpLog4netLogging;
using System.Reactive.Linq;
using Dsu.Driver.Base;
using DevExpress.XtraEditors;
using Dsu.Common.Utilities.DX;
using CpTesterSs.UIManual;
using CpTesterPlatform.CpMngLib.Manager;
using System.Drawing;
using Dsu.Common.Utilities;
using System.Linq;
using Dsu.Driver.UI.Paix;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualRobotAuditGCVT
        //: Form  // design time
        : FormManualRobotAudit
    {
        protected override ComboBoxEdit ComboEditPath { get { return comboBoxEdit_Path; } }

        private static bool _machineStatusRead;
        private static bool _tiltAdvanced;
        private static bool _motorSlideAdvanced;

        private static bool _wheelSensorBit1;      // 7
        private static bool _wheelSensorBit2;      // 8
        private static bool _wheelSensorBit3;      // 9

        private static bool IsPartTurbine { get; set; }
        private static bool IsPartPrimary { get; set; }
        private static bool IsPartOutput { get; set; }

        private static bool IsWheelTurbine => _wheelSensorBit1 && _wheelSensorBit2;
        private static bool IsWheelPrimary => _wheelSensorBit3;
        private static bool IsWheelOutput => _wheelSensorBit1 && !_wheelSensorBit2;
        //private int JogSpeed => Convert.ToInt32(spinEdit_Speed.EditValue);

        private static void ReadMachineStatus(IEnumerable<CpMngDIOControl> dios)
        {
            _machineStatusRead = false;

            _tiltAdvanced = !GetDio(SignalEnum.UTiltPusherReturned, dios);
            _motorSlideAdvanced = GetDio(SignalEnum.UGcvtMotorSlideAdvanced, dios);

            _wheelSensorBit1 = GetDio(SignalEnum.UGcvtWheelSensorBit1, dios);
            _wheelSensorBit2 = GetDio(SignalEnum.UGcvtWheelSensorBit2, dios);
            _wheelSensorBit3 = GetDio(SignalEnum.UGcvtWheelSensorBit3, dios);

            IsPartTurbine = GetDio(SignalEnum.UGcvtPartSensorTurbine, dios);
            IsPartPrimary = GetDio(SignalEnum.UGcvtPartSensorPrimary, dios);
            IsPartOutput = GetDio(SignalEnum.UGcvtPartSensorOutput, dios);

            var total = (IsPartTurbine ? 1 : 0) + (IsPartPrimary ? 1 : 0) + (IsPartOutput ? 1 : 0);
            if (total != 0 && total != 1)
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, $"Part sensor error. total={total}");

            _machineStatusRead = true;
        }

        private static int GetPartSensedTotal() => (IsPartTurbine ? 1 : 0) + (IsPartPrimary ? 1 : 0) + (IsPartOutput ? 1 : 0);
        private static bool IsWheelAndPartMatch() => (IsWheelPrimary == IsPartPrimary) || (IsWheelTurbine == IsPartTurbine) || (IsWheelOutput == IsPartOutput);
        private static string GetSelectedWheelName()
        {
            if (IsWheelTurbine) return "Turbine";
            if (IsWheelPrimary) return "Primary";
            if (IsWheelOutput) return "Output";
            throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Wheel sensor error.");
        }
        private static string GetSelectedPartName()
        {
            if (IsPartTurbine) return "Turbine";
            if (IsPartPrimary) return "Primary";
            if (IsPartOutput) return "Output";
            throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Part sensor error.");
        }

        /// 시험 조건이 맞으면 null string 을, 맞지 않으면 해당 원인을 non-empty string 으로 반환
        private static bool IsTestlistOK()
        {
            var testlist = FormTestlistSelectorAGCVT.SelectedStation.Name;
            return ((IsWheelOutput && testlist.Contains("OUPU"))
                || (IsWheelPrimary && testlist.Contains("PRIM"))
                || (IsWheelTurbine && testlist.Contains("TURB")));
        }

        public static string IsItOK(IEnumerable<CpMngDIOControl> dios)
        {
            if ( !_machineStatusRead )
                ReadMachineStatus(dios);

            if (!IsTestlistOK())
                return $"Test list({FormTestlistSelectorAGCVT.SelectedStation.Name}) and wheel({GetSelectedWheelName()}) mismatch.";

            if (GetPartSensedTotal() != 1)
                return "Part sensor error or no part loaded.";

            if (!PaixManagerBase.IsOriginCalibrated)
                return "Not executed robot home move!!";

            if (!IsWheelAndPartMatch())
                return "Wheel and part mismatch!!";

            return null;    // it's OK
        }




        public FormManualRobotAuditGCVT()
        {
            if (!DriverBaseGlobals.IsAuditGCVT())
                throw ExceptionWithCode.Create(ErrorCodes.APP_ConfigurationError, $"Tester type {DriverBaseGlobals.TesterType} mismatch.");

            InitializeComponent();
        }

        private void FormManualRobotAuditGCVT_Load(object sender, System.EventArgs args)
        {
            OnLoad();
            this.action1.Update += Action1_Update;

            ReadMachineStatus(DIODeviceManagers);


            if (!IsTestlistOK())
            {
                ShowMessageBox($"Test list({FormTestlistSelectorAGCVT.SelectedStation.Name}) and wheel({GetSelectedWheelName()}) mismatch!!\r\nRe-select test list!!");
                DialogResult = DialogResult.Abort;
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }


            ShowActivePartIcon();

            var subscription =
                SignalManager.FilteredSignalSubject
                .Subscribe(fs =>
                {
                    var calibrated = PaixManagerBase.IsOriginCalibrated;
                    var value = fs.Value;
                    if (value)   // only check ON case 
                    {
                        switch (fs.Enum)
                        {
                            case SignalEnum.UTiltPusherAdvanced:
                                _tiltAdvanced = true;
                                break;
                            case SignalEnum.UTiltPusherReturned:
                                _tiltAdvanced = false;
                                break;

                            case SignalEnum.UGcvtMotorSlideAdvanced:
                                _motorSlideAdvanced = true;
                                break;
                            case SignalEnum.UGcvtMotorSlideReturned:
                                _motorSlideAdvanced = false;
                                break;

                            default:
                                break;
                        }
                    }

                    switch (fs.Enum)
                    {
                        case SignalEnum.UEmergency:
                            if (value)
                                _cts = new CancellationTokenSource();
                            else
                                _cts.Cancel();

                            break;
                        case SignalEnum.UGcvtWheelSensorBit1: _wheelSensorBit1 = value; ShowActivePartIcon(); break;
                        case SignalEnum.UGcvtWheelSensorBit2: _wheelSensorBit2 = value; ShowActivePartIcon(); break;
                        case SignalEnum.UGcvtWheelSensorBit3: _wheelSensorBit3 = value; ShowActivePartIcon(); break;

                        case SignalEnum.UGcvtPartSensorTurbine: IsPartTurbine = value; ShowActivePartIcon(); break;
                        case SignalEnum.UGcvtPartSensorPrimary: IsPartPrimary = value; ShowActivePartIcon(); break;
                        case SignalEnum.UGcvtPartSensorOutput: IsPartOutput = value; ShowActivePartIcon(); break;

                    }
                });

            FormClosed += (s, e) => subscription.Dispose();
        }


        private void btnPath_Click(object sender, EventArgs e) => OnEditPath();

        private Font _boldFont;

        private void ShowActivePartIcon()
        {
            this.Do(() =>
            {
                if (_boldFont == null)
                {
                    _boldFont = new Font(simpleButton_Turbine.Font, FontStyle.Bold);
                    simpleButton_Up.ForeColor = Color.Gray;
                    simpleButton_Down.Font = _boldFont;
                }

                pictureEditTurbine.Enabled = false;
                pictureEditPrimary.Enabled = false;
                pictureEditOutput.Enabled = false;

                if (IsPartTurbine && IsWheelTurbine)
                {
                    pictureEditTurbine.Enabled = true;
                    simpleButton_Turbine.Font = _boldFont;
                    simpleButton_Primary.ForeColor = Color.Gray;
                }
                else if (IsPartPrimary && IsWheelPrimary)
                {
                    pictureEditPrimary.Enabled = true;
                    simpleButton_Primary.Font = _boldFont;
                    simpleButton_Turbine.ForeColor = Color.Gray;
                }
                else if (IsPartOutput && IsWheelOutput)
                {
                    pictureEditOutput.Enabled = true;
                    simpleButton_Turbine.Font = _boldFont;
                    simpleButton_Primary.ForeColor = Color.Gray;
                }
            });
        }


        private async void simpleButton_HomeMove_Click(object sender, System.EventArgs e)
        {
            try
            {
                using (var waitor = new SplashScreenWaitor(this))
                {
                    waitor.ProgressDescription = "Home moving..";
                    _cts = new CancellationTokenSource();

                    HomeMovePrologue();

                    SetDioOff(SignalEnum.UYAxisHomeMovePulse);
                    SetDioOff(SignalEnum.UZAxisHomeMovePulse);
                    await Task.Delay(200);

                    SetDioOn(SignalEnum.UYAxisHomeMovePulse);
                    SetDioOn(SignalEnum.UZAxisHomeMovePulse);

                    await Task.Delay(300);

                    SetDioOff(SignalEnum.UYAxisHomeMovePulse);
                    SetDioOff(SignalEnum.UZAxisHomeMovePulse);

                    var zFinished = false;
                    var yFinished = false;
                    await Task.Run(async () =>
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            _cts.Token.ThrowIfCancellationRequested();
                            if (!zFinished && GetDio(SignalEnum.UGcvtZAxisHomeMoveCompleted))
                                zFinished = true;

                            if (!yFinished && GetDio(SignalEnum.UGcvtYAxisHomeMoveCompleted))
                                yFinished = true;

                            if (zFinished && yFinished)
                                return;

                            await Task.Delay(500);      // await total 50*200 milliseconds
                        }

                        throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "HomeMove timeout.");
                    });

                    _cts.Token.ThrowIfCancellationRequested();

                    // for tilt axis, home move using Z encoder....
                    if (!await SelectedMng.HomeMove(_cts.Token))
                        LogErrorWithMessageBox("Home move failed.");

                    // check whether IAI robot home move completed.. or not
                    if (!await UntilDioValue(SignalEnum.UGcvtYAxisHomeMoveCompleted, true, 5000)
                        || !await UntilDioValue(SignalEnum.UGcvtZAxisHomeMoveCompleted, true, 5000))
                    {
                        LogErrorWithMessageBox("Home move timeout.");
                    }

                    _cts.Token.ThrowIfCancellationRequested();
                    SelectedMng.SetCommandEncorderZeroSetting(null);
                    PaixManagerBase.IsOriginCalibrated = true;
                    _anyPathRan = false;
                }
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                    LogError($"Home move canceled..");
                else
                    LogError($"Home move failed..  Exception={ex}");
            }
            finally
            {
                _homeMoving = false;
            }
        }


        private void simpleButton_Emergency_Click(object sender, EventArgs e) => EmergencyStop();



        private bool _anyPathRan = false;
        private async void simpleButton_PathRun_Click(object sender, EventArgs e)
        {
            try
            {
                var pathName = await RunSelectedPath();
                if (pathName.NonNullAny() && !pathName.ToUpper().StartsWith("TOORG"))
                    _anyPathRan = true;
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Failed to run path: {ex.Message}");
            }
        }


        private void Action1_Update(object sender, EventArgs e)
        {
            var calibrated = PaixManagerBase.IsOriginCalibrated;

            simpleButton_Primary.Enabled = calibrated && !_anyPathRan && _motorSlideAdvanced;
            simpleButton_Turbine.Enabled = calibrated && !_anyPathRan && !_motorSlideAdvanced;
            simpleButton_Up.Enabled = calibrated && _tiltAdvanced;
            simpleButton_Down.Enabled = calibrated && !_tiltAdvanced;

            simpleButton_PathRun.Enabled = calibrated;
            simpleButton_HomeMove.Enabled = !_homeMoving;

            btnDeveloperModeDialog.Visible = Globals.IsDeveloperMode;


            if (_homeMoving)
                simpleButton_HomeMove.Text = "Home Moving..";
            else
                simpleButton_HomeMove.Text = "Home Move";
        }

        private void simpleButton_HomeSetPos_Click(object sender, EventArgs e)
        {
            //to do:  adjust offset
        }

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton_Primary_Click(object sender, EventArgs e)
        {
            SetDioOff(SignalEnum.UMotorSlidePusher);
        }

        private void simpleButton_Turbine_Click(object sender, EventArgs e)
        {
            SetDioOn(SignalEnum.UMotorSlidePusher);
        }

        private void simpleButton_Up_Click(object sender, EventArgs e)
        {
            SetDioOff(SignalEnum.UTiltPusher);
        }

        private void simpleButton_Down_Click(object sender, EventArgs e)
        {
            SetDioOn(SignalEnum.UTiltPusher);
        }

        public override async Task MoveToRobotOrigin(CancellationToken token)
        {
            await Task.Delay(0);
            //await UntilAxesStoppedAsync(token);
            //RobotDeviceManager.Paix.AbsMove((short)AxisEnumAudit78.Z, 0);
            //await UntilAxesStoppedAsync(token);
            //new[] { (short)AxisEnumAudit78.X, (short)AxisEnumAudit78.Y, (short)AxisEnumAudit78.Tilt }
            //.ForEach(ax => RobotDeviceManager.Paix.AbsMove(ax, 0))
            //;
            //await UntilAxesStoppedAsync(token);
        }


        private void FormManualRobotAuditGCVT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDeveloperModeInvoke())
            {
                e.Cancel = true;
                return;
            }

            /*
             * GammaCVT 에서는 조건 체크 사항이 많으므로, manual dialog 에서 모든 조건이 충족되지 않더라도 해당 창을 
             * 닫을 수 있도록 허가한다.   시험 시작에서 해당 조건을 다시 체크한다.
             */
            var error = IsItOK(DIODeviceManagers);
            if (error.NonNullAny())
            {
                ShowMessageBox(error);
                DialogResult = DialogResult.Abort;
                //e.Cancel = true;
                return;
            }

            DialogResult = DialogResult.OK;
        }

    }
}
