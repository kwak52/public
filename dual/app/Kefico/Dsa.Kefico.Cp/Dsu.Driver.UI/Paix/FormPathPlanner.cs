using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using Dsu.Driver.Paix;
using System.Threading;
using System.Threading.Tasks;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.DX;
using System.Reactive.Linq;
using System.Drawing;
using Dsu.Common.Resources;
using Dsu.Driver.Util.Emergency;
using Dsu.Driver.Base;

namespace Dsu.Driver.UI.Paix
{
    public partial class FormPathPlanner : Form
    {
        ///  Paix manager
        private Manager _manager;
        private short[] _allAxes;

        private static List<FormPathPlanner> _forms = new List<FormPathPlanner>();
        private static bool _isAutoMode = false;
        /// Automatic operation mode
        /// Only on manual control mode, enable this dialog.
        public static bool IsAutoMode
        {
            get { return _isAutoMode; }
            set
            {
                _isAutoMode = value;
                _forms.ForEach(f => f.EnableForm(!_isAutoMode));
            }
        }

        private SpeedSpec CreateDefaultSpeedSpec() => new SpeedSpec(
            new[]
            {
                new AxisSpec(10000, 10000, 10000, 10000),     // Axis 0
                new AxisSpec(10000, 10000, 10000, 10000),     // Axis 1
                new AxisSpec(10000, 10000, 10000, 10000),     // Axis 2
                new AxisSpec(1000, 1000, 1000, 1000),     // Axis 3   
            });

        private AuditPoses _poses;
        public AuditPoses Poses { get { return _poses; } set { _poses = value; gridControlPosition.DataSource = value; gridViewPosition.LayoutChanged(); } }

        private AuditPose[] _clipboardPoses;
        private AuditPose _master;
        private AuditPose _selectedPose;

        private string[] _fieldNames;

        public FormPathPlanner(Manager manager)
        {
            InitializeComponent();

            if (!DriverBaseGlobals.IsAudit())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "FormPathPlanner should be called in Audit tester.");

            if (DriverBaseGlobals.IsAudit78())
            {
                _allAxes = AuditPose78.StaticAllAxes;
                _poses = new AuditPoses78();
                _fieldNames = new[] { "X", "Y", "Z", "Tilting" };
            }
            else
            {
                _allAxes = AuditPoseGCVT.StaticAllAxes;
                _poses = new AuditPosesGCVT();
                _fieldNames = new[] { "Wheel Rotate", "Tilting", "Z", "Wheel Advance" };
            }

            _manager = manager;
            _forms.Add(this);
            this.FormClosed += (s, e) => _forms.Remove(this);
            var susbscription = SignalManager.RawSignalSubject.Subscribe(s =>
            {
                if (SignalManager.IsEmergency)
                    btnStop_Click(null, null);
            });
            FormClosed += (s, e) => susbscription.Dispose();
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

        private void EnableForm(bool enable)
        {
            this.DoAsync(() =>
            {
                Controls.ToEnumerable()
                    .OfType<Button>()
                    .Where(b => b.Name != "btnStop")
                    .ForEach(b => b.Enabled = enable)
                ;
                gridControlAxes.Enabled = enable;
                gridControlPosition.Enabled = enable;
            });
        }

        private void AddImageToButton(Button button, Image image)
        {
            button.Image = image;
            button.ImageAlign = ContentAlignment.MiddleRight;
            button.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void FormPathPlanner_Load(object sender, EventArgs args)
        {
            var allLabels = new[] { toolStripStatusLabelX, toolStripStatusLabelY, toolStripStatusLabelZ, toolStripStatusLabelTilt,
                                    toolStripStatusLabelCmdX, toolStripStatusLabelCmdY, toolStripStatusLabelCmdZ, toolStripStatusLabelCmdTilt};

            ResetCancellationTokenSource();

            AddImageToButton(btnAdd, Images.ActionAdd);
            AddImageToButton(btnDelete, Images.ActionCancel);
            AddImageToButton(btnUp, Images.ArrowUp);
            AddImageToButton(btnDown, Images.ArrowDown);
            AddImageToButton(btnPlay, Images.Start);
            saveToolStripMenuItem.Image = Images.Save;
            openToolStripMenuItem.Image = Images.Open;
            newToolStripMenuItem.Image = Images.New;

            if ( DriverBaseGlobals.IsAudit78() )
            {
                // 정해진 주기로 status strip 에 X, Y, Z, tilt 의 좌표를 갱신함.
                var subscription = Observable.Interval(TimeSpan.FromMilliseconds(250))
                    .Subscribe(async (tick) =>
                    {
                        if (_manager == null)
                        {
                            allLabels.ForEach(l => { l.Text = "?"; l.BackColor = SystemColors.Control; });
                        }
                        else
                        {
                            try
                            {
                                await this.DoAsync(() =>
                                {
                                // Encoder position
                                var encPositions = _allAxes.Select(ax => String.Format("{0:n0}", _manager.GetEncPos(ax).Value)).ToArray();
                                    UpdateLabelIndicator(toolStripStatusLabelX, encPositions[0]);
                                    UpdateLabelIndicator(toolStripStatusLabelY, encPositions[1]);
                                    UpdateLabelIndicator(toolStripStatusLabelZ, encPositions[2]);
                                    UpdateLabelIndicator(toolStripStatusLabelTilt, encPositions[3]);

                                // command position
                                var cmdPositions = _allAxes.Select(ax => String.Format("{0:n0}", _manager.GetCmdPos(ax).Value)).ToArray();
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


            gridControlPosition.DataSource = Poses;


            var columnGo = gridViewPosition.Columns["Go"];
            columnGo.Width = 60;
            repositoryItemButtonEditGo.Buttons[0].Width = columnGo.Width - 8;
            columnGo.OptionsColumn.FixedWidth = true;

            columnGo.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            repositoryItemButtonEditGo.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            repositoryItemButtonEditGo.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.SpinRight;

            columnGo.VisibleIndex = 100;

            gridViewPosition.Columns["Group"].VisibleIndex = 0;
            gridViewPosition.Columns["Comment"].VisibleIndex = 1;

            labelFilePath.Text = "";


            gridViewPosition.SelectionChanged += (s, e) => PositionSelectionChanged();

            columnGo.ColumnEdit = repositoryItemButtonEditGo;
            repositoryItemButtonEditGo.Click += async (s, e) =>
            {
                try
                {
                    var pose = (AuditPose)gridViewPosition.GetFocusedRow();
                    Trace.WriteLine($"Going {pose.Group}");
                    if (_isPlaying)
                    {
                        MessageBox.Show("On operation!!  Try it later.");
                        return;
                    }

                    _isPlaying = true;
                    await pose.Goto(_manager, ResetCancellationTokenSource().Token);
                }
                catch (Exception ex)
                {
                    Logging.Logger.Error($"Exception while executing Go: {ex}");
                    if (!_cts.IsCancellationRequested)
                    {
                        _cts.Cancel();
                    }
                }
                finally
                {
                    _isPlaying = false;
                }
            };


            var columnCheck = gridViewPosition.Columns["Checked"];
            columnCheck.ColumnEdit = repositoryItemCheckEdit1;
            repositoryItemCheckEdit1.EditValueChanged += (s, e) => gridViewPosition.PostEditor();

            // gridcolumn field name renaming
            new[] { "V0", "V1", "V2", "V3" }.Iter((colName, i) =>
            {
                var col = gridViewPosition.Columns[colName];
                col.Caption = _fieldNames[i];
                col.ColumnEdit = repositoryItemTextEdit1;
                repositoryItemTextEdit1.EditValueChanged += (s, e) => gridViewPosition.PostEditor();
            });

            CreatePositionViewPopupMenus();
            CreateAxesViewPopupMenus();

            gridViewAxes.IndicatorWidth = 100;      // row header text width
            gridViewAxes.CustomDrawRowIndicator += (s, e) =>
            {
                if (e.RowHandle >= 0)
                    e.Info.DisplayText = _fieldNames[e.RowHandle];
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var n = gridViewPosition.GetFocusedDataSourceRowIndex();
            AuditPose newPose = null;
            if (_master != null)
                newPose = _master.Duplicate();
            else if (DriverBaseGlobals.IsAudit78())
                newPose = new AuditPose78(0, 0, 0, 0, "", "", false, CreateDefaultSpeedSpec());
            else if (DriverBaseGlobals.IsAuditGCVT())
                newPose = new AuditPoseGCVT(0, 0, 0, "", "", false, CreateDefaultSpeedSpec());

            if (n < 0)
                Poses.Add(newPose);
            else
                Poses.Insert(n + 1, newPose);

            gridViewPosition.LayoutChanged();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            gridViewPosition.DeleteSelectedRows();
            gridViewPosition.LayoutChanged();
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            // Force stop all axes.
            _allAxes.ForEach(ax => _manager.SuddenStop(ax));
            _isPlaying = false;
        }

        private bool _isPlaying = false;
        private CancellationTokenSource _cts;
        private CancellationTokenSource ResetCancellationTokenSource()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            return _cts;
        }


        private async void btnPlay_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
            {
                MessageBox.Show("On operation!!  Try it later.");
                return;
            }

            try
            {
                _isPlaying = true;
                var token = ResetCancellationTokenSource().Token;
                await _poses.PathMove(_manager, token);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            finally
            {
                _isPlaying = false;
            }

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            Poses.ForEach(p => p.Checked = true);
            gridViewPosition.LayoutChanged();
        }

        private void action1_Update(object sender, EventArgs e)
        {
            var nChkSel = Poses.Sum(p => p.Checked ? 1 : 0);
            var nRowSel = gridViewPosition.SelectedRowsCount;
            btnPlay.Enabled = nChkSel > 0 && !_isPlaying && !SignalManager.IsEmergency;
            gridControlPosition.Enabled = btnPlay.Enabled;
            btnUp.Enabled = btnDown.Enabled = btnDelete.Enabled = nRowSel > 0;
            saveToolStripMenuItem.Enabled = _activeFileName.NonNullAny();
        }




        private void MoveRow(int row, bool up)
        {
            var down = !up;
            if ( (up && row == 0) || (down && row == Poses.Count() - 1) )
                return;
            var pose = Poses[row].Duplicate();
            Poses.RemoveAt(row);
            Poses.Insert(up ? row - 1 : row + 1, pose);
        }

        private bool MoveRows(IEnumerable<int> rows, bool up)
        {
            var down = !up;
            if ((up && rows.Min() == 0) || (down && rows.Max() == Poses.Count() - 1))
                return false;

            var oldFocus = gridViewPosition.FocusedRowHandle;
            if (up)
                rows.ForEach(r => MoveRow(r, up));
            else
                rows.OrderByDescending(r => r).ForEach(r => MoveRow(r, up));
            gridViewPosition.FocusedRowHandle = up ? oldFocus - 1 : oldFocus + 1;
            PositionSelectionChanged();
            gridViewPosition.LayoutChanged();
            return true;
        }

        private void UpdateUISpeedSpec(SpeedSpec selectedSpec = null)
        {
            if (selectedSpec != null)
            {
                gridControlAxes.DataSource = selectedSpec.AxesSpec;
            }
        }

        private void showPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var valuesCmd = _allAxes.Select((ax, i) => $"\t{i}={_manager.GetCmdPos(ax).Value}");
            var statusCmd = String.Join("\n", valuesCmd);


            var valuesEnc = _allAxes.Select((ax, i) => $"\t{i}={_manager.GetEncPos(ax).Value}");
            var statusEnc = String.Join("\n", valuesEnc);
            MessageBox.Show($"EncStatus\n{statusEnc}\n\nCmdStatus\n{statusCmd}\n\n");
        }

        private void btnUp_Click(object sender, EventArgs e) => MoveUpSelectedRows();
        private void btnDown_Click(object sender, EventArgs e) => MoveDownSelectedRows();

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridControlAxes.DataSource = null;
            gridViewAxes.LayoutChanged();

            Poses.Clear();
            gridViewPosition.LayoutChanged();
        }
    }
}
