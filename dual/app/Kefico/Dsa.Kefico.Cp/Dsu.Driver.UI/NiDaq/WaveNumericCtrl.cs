using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraGrid.Views.Grid;

namespace Dsu.Driver.UI.NiDaq
{
    public partial class WaveNumericCtrl : UserControl
    {
        public double[] Data { get; set; }
        public double SamplingRate { get; set; }

        // for Designer
        internal WaveNumericCtrl()
        {
            InitializeComponent();
        }

        public WaveNumericCtrl(double[] data, double samplingRate)
        {
            InitializeComponent();
            Data = data;
            SamplingRate = samplingRate;
        }

        private IEnumerable<double> GetSelectedData()
        {
            var selectedRows = new HashSet<int>(((GridView)gridControl1.MainView).GetSelectedRows());
            return Data.Where((d, i) => selectedRows.Contains(i));
        }
        private void WaveNumericCtrl_Load(object sender, EventArgs e)
        {
            if (Data == null)
                return;

            gridControl1.DataSource = Data.Select((d, i) => {
                return new { Index = i, Value = d, Diff = i == 0 ? 0 : Data[i - 1] - d };
            });

            var menu = new ContextMenuStrip();
            menu.Items.Add(new ToolStripMenuItem("Save as...", null, (o, args) => {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var selectedData = GetSelectedData().Select(d => d.ToString());
                    File.WriteAllLines(saveFileDialog1.FileName, selectedData);
                }
            }));
            menu.Items.Add(new ToolStripMenuItem("Chart...", null, (o, args) => {
                new FormDaqChart("Partial chart", GetSelectedData().ToArray(), SamplingRate, Data.Length).Show();
            }));
            gridControl1.ContextMenuStrip = menu;
        }
    }
}
