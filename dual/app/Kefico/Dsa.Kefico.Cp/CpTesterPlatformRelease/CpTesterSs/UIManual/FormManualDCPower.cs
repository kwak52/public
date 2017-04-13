using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualDCPower : Form
    {
        private List<CpMngPowerSupply> DevMgrSet = new List<CpMngPowerSupply>();
        private CpMngPowerSupply SelectedMng;
        private Timer timer1 = new Timer();

        public FormManualDCPower(IDevManager DevMgr)
        {
            InitializeComponent();
            SelectedMng = DevMgr as CpMngPowerSupply;
        }

        private void ucManuDCPower_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
        }

        private void comboBoxEdit_Device_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterableTextBox_Voltage.Text = SelectedMng.GetVoltage().ToString();
            filterableTextBox_Current.Text = (SelectedMng.GetCurrent() * 100000).ToString();
        }

        private void action1_Update(object sender, System.EventArgs e)
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

        private void windowsUIButtonPanel_Control_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch (e.Button.Properties.Caption)
            {
                case "DefaultSet":
                    SelectedMng.SetCurrent(Convert.ToDouble(numericTextBox_Current.Text));
                    SelectedMng.SetVoltage(Convert.ToDouble(numericTextBox_Volt.Text));
                    SelectedMng.SetOutput(true); break;
                case "Current":
                    SelectedMng.SetCurrent(Convert.ToDouble(numericTextBox_Current.Text)); break;
                case "Voltage":
                    SelectedMng.SetVoltage(Convert.ToDouble(numericTextBox_Volt.Text)); break;
                case "Active":
                    SelectedMng.SetOutput(true); break;
                case "Deactive":
                    SelectedMng.SetOutput(false); break;
                case "GetData":
                    break;
            }
        }

        private void simpleButton_Cur_Click(object sender, EventArgs e)
        {
            SelectedMng.SetCurrent(Convert.ToDouble(numericTextBox_Current.Text)/1000);
        }

        private void simpleButton_Volt_Click(object sender, EventArgs e)
        {
            SelectedMng.SetVoltage(Convert.ToDouble(numericTextBox_Volt.Text));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            filterableTextBox_Current.Text = (SelectedMng.GetObservedCurrent() * 1000).ToString();
            filterableTextBox_Voltage.Text = SelectedMng.GetObservedVoltage().ToString();

            arcScaleComponent1.Value = Convert.ToSingle(filterableTextBox_Current.Text);
            arcScaleComponent2.Value = Convert.ToSingle(filterableTextBox_Voltage.Text);
        }
        private void frmManuDCPower_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
