using System;
using System.Linq;
using System.Windows.Forms;
using static Dsu.Driver.Math.Statistics;
using Dsu.Driver.Math;

namespace Dsu.Driver.UI.NiDaq
{
    public partial class FormWaveAnaysis : Form
    {
        public double[] Data { get { return waveNumericCtrl1.Data; } set { waveNumericCtrl1.Data = value; } }
        public bool IsOpenRawData { get; set; }
        public double SamplingRate { get; set; }
        public FormWaveAnaysis(double samplingRate)
        {
            InitializeComponent();
            SamplingRate = samplingRate;
            // Data = data;     <== 생성자 이후에 실행해야 한다.
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            var noisy = Data;
            var parameters = new DaqSquareWaveDecisionParameters();

            try
            {
                if (IsOpenRawData)
                {
                    var form = new FormDaqChart("Noisy/Clean", noisy, SamplingRate, Data.Length);
                    form.Show();
                }
                else
                {
                    var daqSqWave = new DaqSquareWave(parameters, noisy, SamplingRate, 1.07);
                    if ( ! daqSqWave.IsSucceeded )
                    {
                        MessageBox.Show($"Failed to create DaqSuarewave: {daqSqWave.ErrorMessage}");
                        return;
                    }

                    var form = new FormDaqChart("Noisy/Clean", daqSqWave.Data, SamplingRate, daqSqWave.Data.Length);
                    form.Show();

                    noisy = noisy.Skip(parameters.StartIndex).Take(parameters.EndIndex - parameters.StartIndex + 1).ToArray();



                    var interval = daqSqWave.IntervalTime;
                    var duration = daqSqWave.DurationTime;
                    var duty = daqSqWave.Duty;
                    var numHigh = daqSqWave.NumHighSamples;
                    var numLow = daqSqWave.NumLowSamples;
                    var decisions = daqSqWave.Decisions;
                    var numRisingEdges = daqSqWave.NumRisingEdges;
                    var numFallingEdges = daqSqWave.NumFallingEdges;
                    var dutyDuration = (daqSqWave.DutyDuration * 1000.0).ToString("0.0000");


                    MessageBox.Show($@"
Number of samples: {Data.Length}
Sample interval: {interval} ms
Sample duration: {duration} ms
Sampling ratio: {1.0 / interval} KHz
Duty: {duty}
Duty duration: {dutyDuration} microsec
Num. Rising edges: {numRisingEdges}
Num. Falling edges: {numFallingEdges}
"
        , "DAQ statistics summary");
                }
            }
            catch (Exception)
            {
            }

        }

        private void btnWidthAnal_Click(object sender, EventArgs args)
        {
            var parameters = new DaqSquareWaveDecisionParameters();
            double[] data = Data;

            if (IsOpenRawData)
            {
                var widthAnal = new DaqWidthAnalyzer(data, SamplingRate, -0.5, 1.07, 2.0);
                var widths = widthAnal.Widths.Select(n => (double)n).ToArray();
                var duties = widthAnal.Duties.Select(n => (double)n).ToArray();
                var form = new FormDaqChart("Widths", widths, SamplingRate, widths.Length);
                form.Load += (s, e) => form.DevChart.AddAdditionalSeries(duties, "Duties");

                var nd = duties.Length;
                var skip = (int)(nd / 10);
                var dutiesFiltered = duties.OrderBy(d => d).Skip(skip).Take(nd - skip).ToArray();
                var widthsFiltered = widths.OrderBy(d => d).Skip(skip).Take(nd - skip).ToArray();
                var intervalTime = 1000.0 / SamplingRate;

                var duty = dutiesFiltered.Average();    // * intervalTime;
                var width = widthsFiltered.Average();
                MessageBox.Show($"Width={width.ToString(".##")}, Highes={duty.ToString(".##")}, dutyRatio={(duty/width).ToString("#.##")}");

                form.Show();
            }
        }
    }
}
