using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Threading;
using System.Reactive.Linq;
using DevExpress.XtraEditors;
using Dsu.Driver.Util.Emergency;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpCommon;
using System.Threading.Tasks;
using static CpBase.CpLog4netLogging;
using Dsu.Driver.Paix;
using Dsu.Common.Utilities.DX;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities;
using Dsu.Driver.UI.Paix;

namespace CpTesterPlatform.CpTester
{
    public abstract class FormManualRobotAudit : Form
    {
        protected CpMngMotion SelectedMng;
        protected CancellationTokenSource _cts = new CancellationTokenSource();
        protected abstract ComboBoxEdit ComboEditPath { get; }


        public static List<CpMngMotion> MotionDeviceManagers;
        public static List<CpMngDIOControl> DIODeviceManagers;
        public static void SetDeviceManagers(IEnumerable<CpMngMotion> motionManagers, IEnumerable<CpMngDIOControl> dioManagers)
        {
            DIODeviceManagers = dioManagers.ToList();
            MotionDeviceManagers = motionManagers.ToList();
        }
        public static CpMngMotion RobotDeviceManager => MotionDeviceManagers.Where(m => ((ClsMotionInfo)m.DeviceInfo).AXIS_ROBOT).First();

        protected void ResetCancellationToken()
        {
            _cts = new CancellationTokenSource();
            LogDebug("Robot manual dialog: created cancellation token.");
        }

        protected void RaiseCancel()
        {
            _cts.Cancel();
            LogDebug("Robot manual dialog: operation canceled.");
        }

        public static bool ExistsRobotDialogue { get; protected set; }
        protected FormManualRobotAudit()
        {
            if (ExistsRobotDialogue)
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Only one robot manual dialog allowed.");

            if ( DIODeviceManagers.IsNullOrEmpty() || MotionDeviceManagers.IsNullOrEmpty())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Device managers not ready on manual robot dialog.");

            ExistsRobotDialogue = true;

            FormClosed += (s, e) => { ExistsRobotDialogue = false; };
        }


        protected bool CheckMultiRobot()
        {
            if (!((ClsMotionInfo)SelectedMng.DeviceInfo).AXIS_ID.Contains(';'))
            {
                UtilTextMessageBox.UIMessageBoxForWarning("Robot Error", "Select Multi Axis Device");
                return false;
            }
            else
                return true;
        }

        protected void OnLoad()
        {
            if (MotionDeviceManagers.Count > 0)
            {
                SelectedMng = MotionDeviceManagers[0];
                if (SelectedMng == null || !CheckMultiRobot())
                    throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot manager not created.");
            }
            else
            {
                Close();
                return;
            }

            foreach (string path in SelectedMng.GetPath())
            {
                if (path.StartsWith("ToOrg_", ignoreCase: true, culture: System.Globalization.CultureInfo.InvariantCulture)) continue;
                ComboEditPath.Properties.Items.Add(path);
            }

            var subscription =
                SignalManager.FilteredSignalSubject
                .Subscribe(fs =>
                {
                    switch (fs.Enum)
                    {
                        case SignalEnum.UEmergency:
                            if (fs.Value)
                                ResetCancellationToken();
                            else
                                RaiseCancel();

                            break;

                        case SignalEnum.UOriginSensor:
                            break;
                    }
                });

            FormClosed += (s, e) => subscription.Dispose();
        }

        protected void EmergencyStop()
        {
            LogInfo("Emergency stop requested on Robot manual dialog.");
            RaiseCancel();
            foreach (var m in MotionDeviceManagers)
                m.StopMotionEmergency();
        }

        protected async Task MovePathAsync(string pathName, CancellationToken token)
        {
            using (var waitor = new SplashScreenWaitor(this))
            {
                waitor.ProgressDescription = $"Path Moving {pathName}..";
                await SelectedMng.MovePath(pathName, token);
            }
        }

        protected async Task<string> RunSelectedPath()
        {
            if (ComboEditPath.SelectedItem == null)
                return null;

            var path = ComboEditPath.SelectedItem.ToString();
            try
            {
                _cts = new CancellationTokenSource();
                await MovePathAsync(path, _cts.Token);
                return path;
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                    LogError($"Move cancel on path={path}");
                else
                    LogError($"Error on moving path={path} : Exception={ex.Message}");
            }

            return null;
        }

        protected void OnEditPath() => SelectedMng.OpenManualDialog();

        protected bool GetDio(SignalEnum sigEnum) => CpSignalManager.GetDio(sigEnum, DIODeviceManagers);
        protected static bool GetDio(SignalEnum sigEnum, IEnumerable<CpMngDIOControl> dioDevMgrs) => CpSignalManager.GetDio(sigEnum, dioDevMgrs);
        protected void SetDioOn(SignalEnum sigEnum, bool bWriteAlways=true) => CpSignalManager.SetDioOn(sigEnum, DIODeviceManagers, bWriteAlways);
        protected void SetDioOff(SignalEnum sigEnum, bool bWriteAlways=true) => CpSignalManager.SetDioOff(sigEnum, DIODeviceManagers, bWriteAlways);

        protected Task<bool> UntilDioValue(SignalEnum sigEnum, bool targetValue, Nullable<int> timeOutMilliseconds=null)
        {
            return Task.Run(async () =>
            {
                var timeSpent = 0;
                while (GetDio(sigEnum) != targetValue)
                {
                    if (timeOutMilliseconds.HasValue && timeOutMilliseconds.Value < timeSpent)
                        return false;

                    timeSpent += 100;
                    await Task.Delay(100);
                }

                return true;
            });
        }


        protected bool _homeMoving = false;
        protected void HomeMovePrologue()
        {
            if (_homeMoving)
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Duplicated home moving operation.");

            if ( SelectedMng.Paix.AlaramedAxes(SelectedMng.GetAxesList()).Any() )
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Alarm exists.  Need reboot the tester.");

            _homeMoving = true;
            PaixManagerBase.IsOriginCalibratable = false;
            PaixManagerBase.IsOriginCalibrated = false;
        }


        public abstract Task MoveToRobotOrigin(CancellationToken token);

        protected async Task UntilAxesStoppedAsync(CancellationToken token)
        {
            var allAxes = SelectedMng.GetAxesList();
            while (!RobotDeviceManager.Paix.IsAxesStop(allAxes) && !token.IsCancellationRequested)
                await Task.Delay(30);
        }


        protected bool IsDeveloperModeInvoke()
        {
            if (Keyboard.IsShiftKeyPressed)
            {
                FormDeveloper.DoModal();
                if (Globals.IsDeveloperMode)
                {
                    if (Keyboard.IsControlKeyPressed)
                    {
                        var paix = MotionDeviceManagers.FirstOrDefault(d => d.PaixRobot != null).PaixRobot.Paix;
                        new FormPathPlanner(paix).Show();
                    }
                }

                return true;
            }
            else if (Keyboard.IsControlKeyPressed)
            {
                Globals.IsDeveloperMode = false;
                return true;
            }

            return false;
        }

        protected void btnDeveloperModeDialog_Click(object sender, EventArgs e)
        {
            if (Globals.IsDeveloperMode)
                new FormManualRobotAuditDeveloperMode(this).Show();
        }
    }
}
