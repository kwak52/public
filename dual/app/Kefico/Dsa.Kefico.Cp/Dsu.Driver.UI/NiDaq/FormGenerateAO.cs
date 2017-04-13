using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static Dsu.Driver.NiDaqXcAo;
using Dsu.Common.Utilities.ExtensionMethods;
using NationalInstruments.DAQmx;
using System.Collections.Generic;

namespace Dsu.Driver.UI.NiDaq
{
    /// <summary>
    /// NI DAQ AO 을 통해서 wave form 을 생성하여 출력하기 위한 form
    /// </summary>
    public partial class FormGenerateAO : Form
    {
        private DaqMcAoManager _daqManager;
        private string[] _specifiedChannelNames;
        private List<FormGeneratePerChannelAO> _channelsForm = new List<FormGeneratePerChannelAO>();

        internal int NumSamplesPerBuffer => (int)numericTextBoxNumSamplesPerBuffer.GetDoubleValue();
        private double SamplingRate => numericTextBoxSamplingRate.GetDoubleValue();
        private void Preview() { _channelsForm.ForEach(d => d.Preview()); }

        private void CalculateBufferDuration(object sender, EventArgs e)
        {
            var duration = NumSamplesPerBuffer / SamplingRate;
            textBoxBufferDuration.Text = duration.ToString();
            Preview();
            if (sender is TextBox)
                ((TextBox)sender).Focus();
        }




        public FormGenerateAO(params string[] channelNames)
        {
            InitializeComponent();

            var devices = DaqSystem.Local.Devices;
            _specifiedChannelNames = channelNames;

            comboBoxDevice.DataSource = devices;
            comboBoxDevice.SelectedValueChanged += ComboBoxDevice_SelectedValueChanged;

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

            bool first = true;
            var device = DaqSystem.Local.LoadDevice(comboBoxDevice.SelectedValue.ToString());
            var channels = device.AOPhysicalChannels.Where(ch => _specifiedChannelNames.IsNullOrEmpty() || _specifiedChannelNames.Contains(ch));
            foreach (var ch in channels)
            {
                var page = new TabPage(ch);
                var form = new FormGeneratePerChannelAO(ch, NumSamplesPerBuffer);
                if (first)
                    form.IsEnabled = true;
                first = false;
                _channelsForm.Add(form);
                form.EmbedToControl(page);
                tabControlChannels.TabPages.Add(page);
            }
        }

        private void FormGenerateAO_Load(object sender, EventArgs e)
        {
            numericTextBoxNumSamplesPerBuffer.TextChanged += (sndr, evt) =>
            {
                CalculateBufferDuration(sndr, evt);
                _channelsForm.ForEach(d => d.NumSamplesPerBuffer = NumSamplesPerBuffer);
            };
            numericTextBoxSamplingRate.TextChanged += (sndr, evt) => CalculateBufferDuration(sndr, evt);

            CalculateBufferDuration(null, null);
            Preview();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var perChannelParams =
                from form in _channelsForm.Where(f => f.IsEnabled)
                select new NiDaqParams.DaqScAoParams(form.ChannelName, form.Pattern);
            var mcParams = new NiDaqParams.DaqMcAoParams(perChannelParams) { SamplingRate = SamplingRate, NumberOfSamples = NumSamplesPerBuffer };

            _daqManager = new DaqMcAoManager(mcParams);
            
            btnRun.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _daqManager.Dispose();
            _daqManager = null;
            btnStop.Enabled = false;
            btnRun.Enabled = true;
        }
    }
}
