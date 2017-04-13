using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualLCR : Form
    {
        private List<CpMngLCRMeter> DevMgrSet = new List<CpMngLCRMeter>();
        private CpMngLCRMeter SelectedMng;
        private Timer timer1 = new Timer();
        public FormManualLCR(List<IDevManager> lstDevMgr)
        {
            InitializeComponent();
            foreach (var mng in lstDevMgr)
                DevMgrSet.Add(mng as CpMngLCRMeter);
        }

        private void ucManuLCR_Load(object sender, EventArgs e)
        {
            if (DevMgrSet.Count > 0)
                SelectedMng = DevMgrSet[0];

            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
           // timer1.Start();
        }


        private void action1_Update(object sender, EventArgs e)
        {
            if (SelectedMng == null)
                return;
        }

        private void simpleButton_Open_Click(object sender, EventArgs e)
        {
            SelectedMng.OpenDevice();
        }

        private void simpleButton_Close_Click(object sender, EventArgs e)
        {
            SelectedMng.CloseDevice();
        }

       

        private void simpleButton_Resister_Click(object sender, EventArgs e)
        {
            filterableTextBox_Resister.Text = SelectedMng.GetResistance().ToString();
        }

        private void simpleButton_Capacitor_Click(object sender, EventArgs e)
        {
            filterableTextBox_Capacitor.Text = (SelectedMng.GetCapicatance() * 1000000000).ToString("###.####");
        }

        private void frmManuLCR_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            filterableTextBox_LoadingID.Text = SelectedMng.GetSettingFile().ToString();
            filterableTextBox_Capacitor.Text = (SelectedMng.GetCapicatance() * 1000000000).ToString("###.####");
            filterableTextBox_Resister.Text = SelectedMng.GetResistance().ToString();
        }

        private void simpleButton_Cur_Click(object sender, EventArgs e)
        {
            SelectedMng.SetSettingFile(Convert.ToInt16(numericTextBox1.Text));
            filterableTextBox_LoadingID.Text = SelectedMng.GetSettingFile().ToString();

        }
    }
}
