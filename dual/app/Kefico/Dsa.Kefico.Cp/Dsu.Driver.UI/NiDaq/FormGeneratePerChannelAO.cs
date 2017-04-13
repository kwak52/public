using System;
using System.Linq;
using System.Windows.Forms;
using Dsu.Driver.Base;
using static Dsu.Driver.Base.Sampling;
using Dsu.Common.Utilities.ExtensionMethods;
using DevExpress.XtraCharts;


namespace Dsu.Driver.UI.NiDaq
{
    /// <summary>
    /// AO (아날로그 출력)을 위한 form
    /// </summary>
    public partial class FormGeneratePerChannelAO : Form
    {
        public ChartControl DevChart => daqChartCtrl1.DevChart;
        private double Min => numericTextBoxMin.GetDoubleValue();
        private double Max => numericTextBoxMax.GetDoubleValue();
        private double Duty => 1 - numericTextBoxDuty.GetDoubleValue();
        public int NumSamplesPerBuffer { get; set; }

        public double[] Pattern { get; private set; }
        public string ChannelName { get; private set; }
        public bool IsEnabled { get { return cbEnable.Checked; } set { cbEnable.Checked = value; } }

        public FormGeneratePerChannelAO(string channelName, int numSamplesPerBuffer)
        {
            InitializeComponent();
            ChannelName = channelName;
            NumSamplesPerBuffer = numSamplesPerBuffer;
        }

        private void FormGeneratePerChannelAO_Load(object sender, EventArgs e)
        {
            enumEditorWaveType.EnumType = typeof(Sampling.WaveType);
            enumEditorWaveType.EnumValue = (long)Sampling.WaveType.Square;
            //enumEditorWaveType.LayoutMode = Dsu.Common.Utilities.LayoutMode.Landscape;
            enumEditorWaveType.Change += (sndr, evt) => Preview();

            numericTextBoxMax.TextChanged += (sndr, evt) => CalculateAmplitude(sndr, evt);
            numericTextBoxMin.TextChanged += (sndr, evt) => CalculateAmplitude(sndr, evt);
            numericTextBoxDuty.TextChanged += (sndr, evt) => Preview();

            cbEnable.CheckedChanged += (oa, ea) => UpdateUIWithEnabledStatus();

            CalculateAmplitude(null, null);
            UpdateUIWithEnabledStatus();
        }

        private void CalculateAmplitude(object sender, EventArgs e)
        {
            var amplitude = numericTextBoxMax.GetDoubleValue() - numericTextBoxMin.GetDoubleValue();
            textBoxAmplitude.Text = amplitude.ToString();
            Preview();
            if (sender is TextBox)
                ((TextBox)sender).Focus();
        }

        internal void Preview()
        {
            switch ((WaveType)enumEditorWaveType.EnumValue)
            {
                case WaveType.Sine:
                    Pattern = Sampling.generateSineWave(1.0, 2.0, NumSamplesPerBuffer);
                    break;
                case WaveType.Square:
                    Pattern = Sampling.generateSquareWave(Duty, Min, Max, NumSamplesPerBuffer);
                    break;
            }

            var axis = DevChart.GetSwiftPlotDiagram().AxisX;
            axis.Visibility = DevExpress.Utils.DefaultBoolean.False;    // label 이 너무 많아서 혼잡해 보이는 것을 방지

            DevChart.DrawData(Pattern);

            numericTextBoxDuty.Visible = labelDuty.Visible = (WaveType)enumEditorWaveType.EnumValue == WaveType.Square;
        }

        private void UpdateUIWithEnabledStatus()
        {
            this.Controls.Cast<Control>().Where(c => c != cbEnable).ForEach(c => c.Visible = cbEnable.Checked);
        }
    }
}
