using System;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Manager;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualTempHumi : Form
    {
        private Timer timer1 = new Timer();
        private CpMngTriggerIO MngTempHumi;
        public FormManualTempHumi(CpMngTriggerIO mngTempHumi)
        {
            InitializeComponent();
            MngTempHumi = mngTempHumi;
        }

        private void frmManuTempHumi_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            linearScaleLevelComponent1.Value = Convert.ToSingle(MngTempHumi.GetTemperature().ToString("##.###"));
            linearScaleLevelComponent2.Value = Convert.ToSingle(MngTempHumi.GetHumidity().ToString("##.###"));

            layoutControlItem1.Text = string.Format("{0} : {1}", "Temperature", linearScaleLevelComponent1.Value);
            layoutControlItem2.Text = string.Format("{0} : {1}", "Humidity", linearScaleLevelComponent2.Value);
        }

        private void frmManuTempHumi_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
