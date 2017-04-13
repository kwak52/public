using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Dsu.Common.Utilities.ExtensionMethods;
using NationalInstruments.DAQmx;
using System.Collections.Generic;
using NiDaqFs = Dsu.Driver.NiDaq;
//using Dsu.Common.Utilities.Core.FSharpInterOp;
using Microsoft.FSharp.Core;

namespace Dsu.Driver.UI.NiDaq
{
    /// <summary>
    /// NI DAQ AO 을 통해서 wave form 을 생성하여 출력하기 위한 form
    /// </summary>
    public partial class FormMeasureAI : Form
    {
        private string[] _specifiedChannelNames;
        private List<FormDaqChart> _channelsForm = new List<FormDaqChart>();

        private bool _isRunning;

        private bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                btnStop.Enabled = _isRunning;
                btnRun.Enabled = !_isRunning;
            }
        }

        internal int NumSamplesPerBuffer => (int)numericTextBoxNumSamplesPerBuffer.GetDoubleValue();
        private double SamplingRate => numericTextBoxSamplingRate.GetDoubleValue();
        private int TargetNumberOfSamples => numericTextBoxTargetNumberOfSamples.GetIntValue();
        private void CalculateBufferDuration(object sender, EventArgs e)
        {
            var duration = 1000 * NumSamplesPerBuffer / SamplingRate;
            numericTextBoxBufferDuration.Text = duration.ToString();
            _daqChartCtrl.NumSamplesPerBuffer = NumSamplesPerBuffer;
            if (sender is TextBox)
                ((TextBox)sender).Focus();
        }


        private bool EnableLowpassFilter
        {
            get { return checkBoxEnableLowpassFilter.Checked; }
            set
            {
                checkBoxEnableLowpassFilter.Checked = value;
                checkBoxEnableLowpassFilter.Enabled = value;
                comboBoxLowpassFilter.Enabled = value;
            }
        }



        public FormMeasureAI(params string[] channelNames)
        {
            InitializeComponent();

            var devices = DaqSystem.Local.Devices;
            _specifiedChannelNames = channelNames;

            comboBoxDevice.DataSource = devices;
            comboBoxDevice.SelectedValueChanged += ComboBoxDevice_SelectedValueChanged;
            numericTextBoxTargetNumberOfSamples.TextChanged += (sndr, evt) => { _daqChartCtrl.NumSamplesPerBuffer = numericTextBoxTargetNumberOfSamples.GetIntValue(); };

            if (channelNames.Length == 0)
            {
                if (devices.Any())
                    comboBoxDevice.Enabled = devices.Length > 1;
                else
                    throw new Exception("No DAQ device found.");
            }
            else
            {
                var parsed = (
                                from ch in channelNames
                                let match = Regex.Match(ch, @"([^/].*)/(.*)")
                                select new { Device = match.Groups[1].ToString(), Channel = match.Groups[2].ToString() }
                             ).ToArray();

                var deviceNames = parsed.Select(p => p.Device).Distinct();
                if (deviceNames.Count() > 1)
                    throw new Exception("Only one device can be specified.");

                comboBoxDevice.DataSource = new[] { deviceNames.First() };
                comboBoxDevice.Enabled = false;
            }

            ComboBoxDevice_SelectedValueChanged(null, null);
        }

        private void ComboBoxDevice_SelectedValueChanged(object sender, EventArgs e)
        {
            while (tabControlChannels.TabPages.Count > 0)
                tabControlChannels.TabPages.RemoveTail();

            var device = DaqSystem.Local.LoadDevice(comboBoxDevice.SelectedValue.ToString());
            var channels = device.AIPhysicalChannels.Where(ch => _specifiedChannelNames.IsNullOrEmpty() || _specifiedChannelNames.Contains(ch));
            foreach (var ch in channels)
            {
                var page = new TabPage(ch);
                var form = new FormDaqChart(ch, null, SamplingRate, TargetNumberOfSamples);
                page.Tag = form;
                _channelsForm.Add(form);
                form.EmbedToControl(page);
                tabControlChannels.TabPages.Add(page);
            }

            comboBoxLowpassFilter.DataSource = device.LowpassCutoffFrequenciesDiscreteValues;
            EnableLowpassFilter = device.LowpassCutoffFrequenciesDiscreteValues.NonNullAny();
        }

        private NiDaqMcAi.DaqMcAiManager GetExistingDaqManager()
        {
            var existing = NiDaqMcAi.managerSingleton();
            //return existing.IsSome() ? existing.Value : null;
            return FSharpOption<NiDaqMcAi.DaqMcAiManager>.get_IsSome(existing) ? existing.Value : null;
        }

        private void FormMeasureAI_Load(object sender, EventArgs e)
        {
            labelMeasure.Text = "";
            if (MeasureParameters.IsDaqManagerCreationAllowed)
            {
                numericTextBoxNumSamplesPerBuffer.TextChanged += (sndr, evt) =>
                {
                    CalculateBufferDuration(sndr, evt);
                    //_channelForm.NumSamplesPerBuffer = NumSamplesPerBuffer;
                };
                numericTextBoxSamplingRate.TextChanged += (sndr, evt) => CalculateBufferDuration(sndr, evt);
                cbAutoScale.CheckedChanged += AutoScaleChanged;
            }
            else
            {
                var existing = GetExistingDaqManager();
                if ( existing == null )
                    throw new Exception("No existing background daq service found.");                

                numericTextBoxNumSamplesPerBuffer.Enabled = false;
                numericTextBoxSamplingRate.Enabled = false;

                numericTextBoxNumSamplesPerBuffer.Text = existing.Parameters.NumberOfSamples.ToString();
                numericTextBoxSamplingRate.Text = ((int)existing.Parameters.SamplingRate).ToString();
            }

            ComboBoxDevice_SelectedValueChanged(null, null);
            tabControlChannels.SelectedIndex = 0;
            CalculateBufferDuration(null, null);

            numericTextBoxMin.TextChanged += AutoScaleChanged;
            numericTextBoxMax.TextChanged += AutoScaleChanged;
        }

        private NiDaqScAi.DaqScAiManager CreateScAiManager(string channel)
        {
            var singleChannelParams = new NiDaqParams.DaqScAiParams(channel)
            {
                LowpassEnable = EnableLowpassFilter,
                LowpassCutoffFrequency = 50 * 1000,
                Min = -5.0,
                Max = +5.0
            };
            if (EnableLowpassFilter)
                singleChannelParams.LowpassCutoffFrequency = (double)comboBoxLowpassFilter.SelectedValue;

            var perChannelParams = new[] { singleChannelParams };

            var mcParams = new NiDaqParams.DaqMcAiParams(perChannelParams) { SamplingRate = SamplingRate, NumberOfSamples = NumSamplesPerBuffer };

            NiDaqFs.CreateMcManager(mcParams);
            //_daqManager = new DaqMcAiManager(mcParams);
            var scManager = NiDaqFs.CreateScManager(channel);
            return scManager;
        }


        private async void btnCollect_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            var channel = _specifiedChannelNames[0];// tabControlChannels.SelectedTab.Text;
            var scManager = CreateScAiManager(channel);
            var creationAllowed = MeasureParameters.IsDaqManagerCreationAllowed;

            btnCollect.Enabled = false;
            btnRun.Enabled = false;

            if (creationAllowed)
                await System.Threading.Tasks.Task.Delay(300);

            var data = await scManager.CollectAsync(TargetNumberOfSamples);

            if (creationAllowed)
                NiDaqFs.McManager().Dispose();

            _daqChartCtrl.DevChart.DrawData(data);
            _channelForm.Data = data;

            btnCollect.Enabled = true;
            btnRun.Enabled = true;
            UseWaitCursor = false;
        }
    }
}
