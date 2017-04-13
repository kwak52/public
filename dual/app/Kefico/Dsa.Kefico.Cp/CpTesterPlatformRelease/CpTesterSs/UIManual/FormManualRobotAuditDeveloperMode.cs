using Dsu.Driver.Base;
using Dsu.Driver.Paix;
using Dsu.Driver.Util.Emergency;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using static CpBase.CpLog4netLogging;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualRobotAuditDeveloperMode : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _operating = false;

        private static void VerifyAudit78()
        {
            if (!DriverBaseGlobals.IsAudit78())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Not a Audit78 mode!");
        }
        private static void VerifyAuditGCVT()
        {
            if (!DriverBaseGlobals.IsAuditGCVT())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Not a AuditGCVT mode!");
        }

        private FormManualRobotAudit _parentForm;
        private FormManualRobotAudit78 ParentForm78 => _parentForm as FormManualRobotAudit78;
        public FormManualRobotAuditDeveloperMode(FormManualRobotAudit parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void FormManualRobotAuditDeveloperMode_Load(object sender, EventArgs e)
        {
            btnMoveToKissOrigin.Visible = false;
            if (DriverBaseGlobals.IsAudit78())
            {
                tabControl1.TabPages.Remove(tabPageGCVT);
                btnMoveToKissOrigin.Visible = true;
            }
            else if (DriverBaseGlobals.IsAuditGCVT())
                tabControl1.TabPages.Remove(tabPageAudit78);
        }

        private void btnForceHomeSet_Click(object sender, EventArgs e)
        {
            VerifyAudit78();
            ParentForm78.ForceHomeSetOnDeveloperMode();
        }

        /// 원점 수행까지만.  (기구 원점으로의 이동은 하지 않음)
        private async void btnDoMachineOrigin_Click(object sender, EventArgs e)
        {
            VerifyAudit78();
            _operating = true;
            try
            {
                _cts = new CancellationTokenSource();
                await FormManualRobotAudit78.UnparkingAudit78(cbAdjustEncPosition.Checked);

                await ParentForm78.HomeMove(_cts.Token, bGotoKissHome: false);
            }
            finally
            {
                _operating = false;
            }
        }

        private async void btnTestBreak_Click(object sender, EventArgs e)
        {
            VerifyAudit78();
            _cts = new CancellationTokenSource();
            for (int i = 0; i < 30; i++)
            {
                if (_cts.IsCancellationRequested)
                {
                    ShowMessageBox("Canceled!");
                    return;
                }

                await FormManualRobotAudit78.UnparkingAudit78(cbAdjustEncPosition.Checked);
                await FormManualRobotAudit78.ParkingAudit78();
                Trace.WriteLine($"{i}-th breaking.");
            }
            ShowMessageBox("Done!");
        }

        private async void btnPark_Click(object sender, EventArgs e)
        {
            VerifyAudit78();
            await FormManualRobotAudit78.ParkingAudit78();
        }

        private async void btnUnpark_Click(object sender, EventArgs e)
        {
            VerifyAudit78();
            await FormManualRobotAudit78.UnparkingAudit78(cbAdjustEncPosition.Checked);
        }

        private async void btnMoveToRobotOrigin_Click(object sender, EventArgs e)
        {
            _operating = true;
            try
            {
                _cts = new CancellationTokenSource();
                await _parentForm.MoveToRobotOrigin(_cts.Token);
            }
            finally
            {
                _operating = false;
            }
        }
        private async void btnMoveToKissOrigin_Click(object sender, EventArgs e)
        {
            _operating = true;
            try
            {
                var z = FormManualRobotAudit78.RobotDeviceManager.PaixRobot.GetEncPos((short)FormManualRobotAudit78.AxisEnumAudit78.Z);
                if ( z > -300000 )
                {
                    ShowMessageBox("Dangerous start position to go to kiss origin.");
                    return;
                }

                _cts = new CancellationTokenSource();
                await ParentForm78.MoveToKissOrigin(_cts.Token);
            }
            finally
            {
                _operating = false;
            }
        }

        private void simpleButton_Emergency_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            FormManualRobotAudit.RobotDeviceManager.Paix.AllAxisStop(1);   // 1 : emergency stop
        }

        private void action1_Update(object sender, EventArgs e)
        {
            btnMoveToRobotOrigin.Enabled = !_operating && PaixManagerBase.IsOriginCalibrated;
            btnMoveToKissOrigin.Enabled = !_operating && PaixManagerBase.IsOriginCalibrated;

            btnDoMachineOrigin.Enabled = !_operating;
            btnPark.Enabled = !_operating;
            btnUnpark.Enabled = !_operating;
        }

    }
}
