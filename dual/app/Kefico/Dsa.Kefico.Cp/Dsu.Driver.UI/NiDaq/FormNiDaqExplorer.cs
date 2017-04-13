using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.UI;
using Dsu.Driver.Math;

namespace Dsu.Driver.UI.NiDaq
{
    /// <summary>
    /// NI DAQ Explorer
    /// </summary>
    // Design view 가 열리지 않을 경우, DeviceTreeCtrl.DeviceTreeCtrl_Load 내용을 comment 처리 한 후에 open 시도해 볼 것
    public partial class FormNiDaqExplorer : Form
    {
        public FormNiDaqExplorer(bool allowManagerCreation=false)
        {
            InitializeComponent();
            MeasureParameters.IsDaqManagerCreationAllowed = allowManagerCreation;
        }

        private void generateAOWavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormGenerateAO().ShowDialog();
        }

        private void FormNiDaqExplorer_Load(object sender, EventArgs e)
        {
            deviceTreeCtrl1.MouseClickEventHandler = (o, args) =>
            {
                var panel = splitContainer1.Panel2;

                var tag = args.Node.Tag;
                if (tag != null)
                {
                    switch (args.Button)
                    {
                        case MouseButtons.Left:
                            panel.Controls.RemoveAll();

                            Trace.WriteLine($"Drawing information by Click {args.Node.Text}");

                            // 선택된 node 객체의 property 를 보여 준다.
                            // NI DAQ Device 의 property 는 http://zone.ni.com/reference/en-XX/help/370473H-01/TOC420.htm 를 참조할 것.
                            var form = new FormObjectBrowser() { Dock = DockStyle.Fill };
                            form.Initialize(tag);
                            form.EmbedToControl(panel);

                            break;

                        case MouseButtons.Right:
                            {
                                var tagString = tag.ToString();
                                var menu = new ContextMenuStrip();
                                if (tagString.Contains("/ai"))
                                {
                                    menu.Items.Add(new ToolStripMenuItem("Measure", null, (obj, a) =>
                                    {
                                        panel.Controls.RemoveAll();

                                        var formM = new FormMeasureAI(tagString) { Dock = DockStyle.Fill };
                                        //form.Initialize(tag);
                                        formM.EmbedToControl(panel);

                                        // 임시
                                        var formT = new FormCpTesterDaq(tagString);
                                        formT.Show();
                                    }));
                                }
                                else if (tagString.Contains("/ao"))
                                {
                                    menu.Items.Add(new ToolStripMenuItem("Function generator", null, (obj, a) =>
                                    {
                                        panel.Controls.RemoveAll();
                                        var formG = new FormGenerateAO(tagString) { Dock = DockStyle.Fill };
                                        //form.Initialize(tag);
                                        formG.EmbedToControl(panel);
                                    }));
                                    menu.Items.Add(new ToolStripMenuItem("Function generator in separated window", null, (obj, a) =>
                                    {
                                        new FormGenerateAO(tagString).Show();
                                    }));
                                }
                                menu.Show(this, this.PointToClient(MousePosition));
                            }
                            break;
                    }
                }
            };
        }

        private void openData(bool openRawData)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            var lines = File.ReadLines(openFileDialog1.FileName);

            // shift key 가 눌린 상태에서는 최대 1만개까지만 사용
            if (ModifierKeys == Keys.Shift)
                lines = lines.TakeWhile( (s, i) => i < 10000);

            var data = lines.Select(s => Double.Parse(s)).ToArray();

            // square wave 판정 샘플
            //var parameters = new DaqSquareWaveDecisionParameters(0, 2.58, 0.1, 0.1);
            //var isSquaureWave = Dsu.Driver.Math.DaqSquareWave.IsSquareWave(data, 1.07);
            //var daq = new Math.DaqSquareWave(parameters, data, 1000000.0);

            var form = new FormWaveAnaysis(1000000) {Data = data, IsOpenRawData = openRawData};
            form.Show();
        }

        private void openDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openData(true);
        }

        private void openSquarewaveFilteredDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openData(false);
        }



        private string AnalyzeData(string fileName, double[] data)
        {
            var samplingRate = 1000000;
            var widthAnal = new DaqWidthAnalyzer(data, samplingRate, -0.5, 1.07, 4.0);
            var widths = widthAnal.Widths.Select(n => (double)n).ToArray();
            var duties = widthAnal.Duties.Select(n => (double)n).ToArray();

            var nd = duties.Length;
            var skip = (int)(nd / 10);
            var dutiesFiltered = duties.OrderBy(d => d).Skip(skip).Take(nd - skip).ToArray();
            var widthsFiltered = widths.OrderBy(d => d).Skip(skip).Take(nd - skip).ToArray();
            var intervalTime = 1000.0 / samplingRate;

            var duty = dutiesFiltered.Average();    // * intervalTime;
            var width = widthsFiltered.Average();

            var summary = $"{fileName}\tWidth={width.ToString(".##")}, Highes={duty.ToString(".##")}, dutyRatio={(duty / width).ToString("#.##")}";
            Trace.WriteLine(summary);
            return summary;

        }
        private void batchWidthAnalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    var targetFolder = folderDialog.SelectedPath;
                    var datas =
                        from f in System.IO.Directory.GetFiles(targetFolder)
                        let lines = File.ReadLines(f)
                        let data = lines.Select(s => Double.Parse(s)).ToArray()
                        select new { File = f, Data = data }
                    ;

                    string summary = "";
                    foreach (var item in datas)
                        summary += AnalyzeData(item.File, item.Data);

                    MessageBox.Show(summary);
                }
            }
        }
    }
}
