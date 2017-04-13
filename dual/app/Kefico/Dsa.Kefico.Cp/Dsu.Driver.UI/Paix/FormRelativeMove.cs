using System;
using System.Windows.Forms;
using Dsu.Driver.Paix;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Linq;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Drawing;
using Dsu.Driver.Base;

namespace Dsu.Driver.UI.Paix
{
    public partial class FormRelativeMove : Form
    {
        ///  Paix manager
        private Manager _manager;
        private short[] _allAxes;

        public FormRelativeMove(Manager manager)
        {
            InitializeComponent();

            if (DriverBaseGlobals.IsAudit78())
                _allAxes = AuditPose78.StaticAllAxes;
            else if (DriverBaseGlobals.IsAuditGCVT())
                _allAxes = AuditPoseGCVT.StaticAllAxes;
            _manager = manager;
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

        private void btnStop_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {

        }

        private void btnRight_Click(object sender, EventArgs e)
        {

        }

        private void btnRear_Click(object sender, EventArgs e)
        {

        }

        private void btnFront_Click(object sender, EventArgs e)
        {

        }

        private void btnUp_Click(object sender, EventArgs e)
        {

        }

        private void btnDown_Click(object sender, EventArgs e)
        {

        }

        private void btnCW_Click(object sender, EventArgs e)
        {

        }

        private void btnCCW_Click(object sender, EventArgs e)
        {

        }

        private void FormRelativeMove_Load(object sender, EventArgs args)
        {
            var allLabels = new[] { toolStripStatusLabelX, toolStripStatusLabelY, toolStripStatusLabelZ, toolStripStatusLabelTilt, };

            // 정해진 주기로 status strip 에 X, Y, Z, tilt 의 좌표를 갱신함.
            var subscription = Observable.Interval(TimeSpan.FromMilliseconds(250))
                .Subscribe(tick =>
                {
                    if (_manager == null)
                    {
                        allLabels.ForEach(l => { l.Text = "?"; l.BackColor = SystemColors.Control; });
                    }
                    else
                    {
                        try
                        {
                            // Encoding position
                            var curPositions = _allAxes.Select(ax => String.Format("{0:n0}", _manager.GetEncPos(ax).Value)).ToArray();
                            UpdateLabelIndicator(toolStripStatusLabelX, curPositions[0]);
                            UpdateLabelIndicator(toolStripStatusLabelY, curPositions[1]);
                            UpdateLabelIndicator(toolStripStatusLabelZ, curPositions[2]);
                            UpdateLabelIndicator(toolStripStatusLabelTilt, curPositions[3]);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"{ex}");
                        }
                    }
                });

            FormClosed += (s, e) => subscription.Dispose();
        }
    }
}
