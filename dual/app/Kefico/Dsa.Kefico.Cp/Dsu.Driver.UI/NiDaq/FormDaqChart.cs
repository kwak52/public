using System;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using Dsu.Common.Utilities.ExtensionMethods;


namespace Dsu.Driver.UI.NiDaq
{
    public partial class FormDaqChart : Form
    {
        public double[] Data { get; set; }
        public double SamplingRate { get; set; }

        public DaqChartCtrl DaqChartCtrl => daqChartCtrl1;
        public ChartControl DevChart => daqChartCtrl1.DevChart;


        public FormDaqChart(string channel, double[] data, double samplingRate, int targetNumberOfSamples)
        {
            InitializeComponent();
            Text = channel;
            Data = data;
            SamplingRate = samplingRate;
            daqChartCtrl1.Iniitialize(channel, samplingRate, targetNumberOfSamples, null);
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            var form = new FormWaveAnaysis(SamplingRate) { Data = Data };
            form.Show();
        }

        private void FormDaqChart_Load(object sender, EventArgs e)
        {
            if (Data.NonNullAny())
                DevChart.DrawData(Data);
        }
    }
}
