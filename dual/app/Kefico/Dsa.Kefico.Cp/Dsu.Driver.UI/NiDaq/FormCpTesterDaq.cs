using System;
using System.Windows.Forms;
using NiDaqFs = Dsu.Driver.NiDaq;
using Dsu.Driver.Math;
using static Dsu.Driver.Math.Statistics;

namespace Dsu.Driver.UI.NiDaq
{
    /// <summary>
    /// FormMeasureAI 대용으로 CpPlatform UI 를 위해서 새로 만든 form
    /// 기능은 FormMeasureAI 와 동일하며, code 는 중복됨.
    /// </summary>
    public partial class FormCpTesterDaq : Form
    {
        private NiDaqScAi.DaqScAiManager _scManager;
        private double _samplingRate => _scManager.MultiChannelParameters.SamplingRate;
        private int TargetNumberOfSamples => numericTextBoxTargetNumberOfSamples.GetIntValue();
        private double Min { get { return numericTextBoxMin.GetDoubleValue(); } }
        private double Max { get { return numericTextBoxMax.GetDoubleValue(); } }
        private string Channel { get { return daqChartCtrl1.Channel; } set { daqChartCtrl1.Channel = value; } }

        public FormCpTesterDaq(string channel)
        {
            InitializeComponent();
            MeasureParameters.IsDaqManagerCreationAllowed = false;
            Channel = channel;
        }


        private void FormCpTesterDaq_Load(object sender, EventArgs e)
        {
            this.cbAutoScale.CheckedChanged += AutoScaleChanged;
            numericTextBoxMin.TextChanged += AutoScaleChanged;
            numericTextBoxMax.TextChanged += AutoScaleChanged;

            //_scManager = NiDaqFs.CreateScManager(_channel);

            // 임시
            _scManager = CreateScAiManager(Channel);
        }

        // 임시 함수
        private NiDaqScAi.DaqScAiManager CreateScAiManager(string channel)
        {
            var singleChannelParams = new NiDaqParams.DaqScAiParams(channel)
            {
                LowpassEnable = true,
                LowpassCutoffFrequency = 50 * 1000,
                Min = -10.0,
                Max = +10.0
            };

            var perChannelParams = new[] { singleChannelParams };

            var mcParams = new NiDaqParams.DaqMcAiParams(perChannelParams) { SamplingRate = 1000000.0, NumberOfSamples = 200000 };

            NiDaqFs.CreateMcManager(mcParams);
            //_daqManager = new DaqMcAiManager(mcParams);
            var scManager = NiDaqFs.CreateScManager(channel);
            return scManager;
        }

        private async void btnCollect_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;

            btnCollect.Enabled = false;
            textBoxAnalysis.Text = "";

            var data = await _scManager.CollectAsync(TargetNumberOfSamples);

            daqChartCtrl1.DevChart.DrawData(data);

            btnCollect.Enabled = true;
            UseWaitCursor = false;


            var noisy = data;
            var parameters = new DaqSquareWaveDecisionParameters()
            {
                TrimRatioFront = 0.1,
                TrimRatioRear = 0.1
            };

            try
            {
                var daqSqWave = new DaqSquareWave(parameters, noisy, _samplingRate);

                var interval = daqSqWave.IntervalTime;
                var duration = daqSqWave.DurationTime;
                var duty = daqSqWave.Duty;
                var dutyPercent = (duty * 100).ToString("0.00");
                var avg = daqSqWave.Average.ToString("0.00");
                var numHigh = daqSqWave.NumHighSamples;
                var numLow = daqSqWave.NumLowSamples;
                var N = numHigh + numLow;
                var decisions = daqSqWave.Decisions;
                var numRisingEdges = daqSqWave.NumRisingEdges;
                var numFallingEdges = daqSqWave.NumFallingEdges;
                var dutyDuration = daqSqWave.DutyDuration.ToString("0.0000");

                textBoxAnalysis.Text = 
    $@"Duty={dutyPercent}%, 1 Duty duration={dutyDuration}ms, Sample interval={interval}ms, Sample duration: {duration}ms
    Average: Total={avg}
    No. Samples: Total={N}, High={numHigh}, Low={numLow}, No. Cycles = {numRisingEdges}
    ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed with exception: {ex}");
            }
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            btnCollect.Enabled = false;
            btnRun.Enabled = false;

            await daqChartCtrl1.Run();

            btnCollect.Enabled = true;
            btnRun.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            daqChartCtrl1.Stop();
        }

        private void AutoScaleChanged(object sender, EventArgs e)
        {
            var auto = cbAutoScale.Checked;
            numericTextBoxMin.Enabled = !auto;
            numericTextBoxMax.Enabled = !auto;

            if (auto)
                daqChartCtrl1.SetAutoRange();
            else
                daqChartCtrl1.SetMinMaxRange(Min, Max);
        }
    }
}
