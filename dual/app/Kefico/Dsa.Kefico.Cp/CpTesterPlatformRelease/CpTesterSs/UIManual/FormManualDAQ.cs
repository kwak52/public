using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.Functions;
using CpTesterPlatform.CpTStepDev.Interface;
using Dsu.Driver.UI.NiDaq;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualDAQ : Form
    {
        private List<CpMngAIControl> DevMgrSet = new List<CpMngAIControl>();
        private CpMngAIControl SelectedMng;
        private Timer timer1 = new Timer();

        public FormManualDAQ(List<IDevManager> lstDevMgr)
        {
            InitializeComponent();
            foreach (var mng in lstDevMgr)
                DevMgrSet.Add(mng as CpMngAIControl);
        }

        private void ucManuDAQ_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();

            if (DevMgrSet.Count > 0)
                SelectedMng = DevMgrSet[0];
            //this.action1.Update += action1_Update;
            //foreach (CpMngAIControl mng in DevMgrSet)
            //    comboBoxEdit_Device.Properties.Items.Add(mng.DeviceInfo.Device_ID);

            //propertyGridControl.Rows.Add(new EditorRow("Device_ID"));
            //propertyGridControl.GetLast().Properties.Caption = "Device ID";

            //propertyGridControl.Rows.Add(new EditorRow("HwName"));
            //propertyGridControl.GetLast().Properties.Caption = "H/W Info";

            //propertyGridControl.Rows.Add(new EditorRow("COMMENT"));
            //propertyGridControl.GetLast().Properties.Caption = "comment";


            //propertyGridControl.Height = 16;

            //comboBoxEdit_Device.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void action1_Update(object sender, System.EventArgs e)
        {
            //    throw new System.NotImplementedException();
        }

        private void frmManuDAQ_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private async void simpleButton_Get_Click(object sender, EventArgs e)
        {
            if (SelectedMng == null)
                return;

            IAnalogInput analogInput = SelectedMng.DeviceInstance as IAnalogInput;

            double[] vresult = await analogInput.GetPeriodicV(1, 100000);
            var daqSqWave = CpUtilDaq.GetDaqSquareWave(vresult, 0.00001, 1.0725);

            if (daqSqWave == null)
                return;

            digitalGauge_Duty.Text = Math.Round(daqSqWave.Duty * 100, 3).ToString(); 
            digitalGauge_high.Text = Math.Round(daqSqWave.HighAverage, 3).ToString();
            digitalGauge_Low.Text = Math.Round(daqSqWave.LowAverage, 3).ToString();
            digitalGauge_width.Text = Math.Round((daqSqWave.Duty * ((double)(daqSqWave.NumHighSamples + daqSqWave.NumLowSamples) / daqSqWave.NumRisingEdges) * daqSqWave.IntervalTime  * 1000000.0), 3).ToString();

            daqChartCtrl1.DevChart.DrawData(vresult.Take(10000).ToArray());
            analogInput.ReturnBuffer(vresult);
        }

        private void simpleButton_Clear_Click(object sender, EventArgs e)
        {
            digitalGauge_Duty.Text = "0";
            digitalGauge_high.Text = "0";
            digitalGauge_Low.Text = "0";
            digitalGauge_width.Text = "0";

            daqChartCtrl1.DevChart.DrawData(new double[1] { 0 });
        }


    }
}
