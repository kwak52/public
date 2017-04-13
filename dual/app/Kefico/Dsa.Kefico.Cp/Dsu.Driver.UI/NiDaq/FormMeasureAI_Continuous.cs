using System;
using System.Threading;

namespace Dsu.Driver.UI.NiDaq
{
    public partial class FormMeasureAI
    {
        private double Min { get { return numericTextBoxMin.GetDoubleValue(); } }
        private double Max { get { return numericTextBoxMax.GetDoubleValue(); } }

        //private CancellationTokenSource _cts;
        private FormDaqChart _channelForm => (FormDaqChart)tabControlChannels.SelectedTab.Tag;
        private DaqChartCtrl _daqChartCtrl => _channelForm.DaqChartCtrl;


        private async void btnRun_Click(object sender, EventArgs e)
        {
            IsRunning = true;
            btnCollect.Enabled = false;
            btnRun.Enabled = false;

            _daqChartCtrl.RedrawChartProc = (counter) => { labelMeasure.Text = $"Redrawing {counter}"; };

            await _daqChartCtrl.Run();

            btnCollect.Enabled = true;
            btnRun.Enabled = true;
            UseWaitCursor = false;
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            IsRunning = false;
            //_cts?.Cancel();
            _daqChartCtrl.Stop();
        }

        private void AutoScaleChanged(object sender, EventArgs e)
        {
            var auto = cbAutoScale.Checked;
            numericTextBoxMin.Enabled = !auto;
            numericTextBoxMax.Enabled = !auto;

            if (auto)
                _daqChartCtrl.SetAutoRange();
            else
                _daqChartCtrl.SetMinMaxRange(Min, Max);
        }
    }
}


