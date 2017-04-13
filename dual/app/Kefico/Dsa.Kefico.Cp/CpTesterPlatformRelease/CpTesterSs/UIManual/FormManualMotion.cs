using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualMotion : Form
    {
        private CpMngMotion SelectedMng;
        private Timer timer1 = new Timer();
        private float RPM = 0.0f;

        public FormManualMotion(IDevManager DevMgr)
        {
            InitializeComponent();
            SelectedMng = DevMgr as CpMngMotion;
        }

        private void ucManuMotion_Load(object sender, EventArgs e)
        {
            SelectedMng.FuncEvtHndl.OnTcpIpReceive += FuncEvtHndl_OnTcpIpReceive;

            timer1.Interval = 300;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
        }


        private void FuncEvtHndl_OnTcpIpReceive(string strData)
        {
            var dataArr = strData.Split(';');
            if (dataArr.Length != 9)
                return;

            RPM = Convert.ToSingle(Math.Round(SelectedMng.GetCurrentRpm(), 4));
        }

        private void action1_Update(object sender, System.EventArgs e)
        {
            if (SelectedMng == null)
                return;

            simpleButton_Stop.Enabled = SelectedMng.IsOpened;
            simpleButton_Go.Enabled = SelectedMng.IsOpened;
            simpleButton_Back.Enabled = SelectedMng.IsOpened;
            simpleButton_Emergency.Enabled = SelectedMng.IsOpened;
        }


        private void simpleButton_Close_Click(object sender, EventArgs e)
        {
            SelectedMng.CloseDevice();
        }

        private void simpleButton_Stop_Click(object sender, EventArgs e)
        {
            SelectedMng.StopMotion();
        }

        private void simpleButton_Emergency_Click(object sender, EventArgs e)
        {
            SelectedMng.StopMotionEmergency();
        }


        private void CMD_ButtonClick(bool backward)
        {
            SelectedMng.SetDirection(!backward);
            SelectedMng.SetParametor(numericTextBox_MAX.GetDoubleValue()
                , numericTextBox_MAX.GetDoubleValue()
                , numericTextBox_MAX.GetDoubleValue());

            SelectedMng.SetJogMove();

            //case "JOG": SelectedMng.SetJogMove(); break;
            //case "INC": SelectedMng.SetRelMove(Convert.ToDouble(numericTextBox_CMD_Rpm.Text)); break;
            //case "ABS": SelectedMng.SetAbsMove(Convert.ToDouble(numericTextBox_CMD_Rpm.Text)); break;                            
        }

        private void simpleButton_XMinus_Click(object sender, EventArgs e)
        {
            CMD_ButtonClick(true);
        }

        private void simpleButton_Pause_Click(object sender, EventArgs e)
        {
            SelectedMng.StopMotion();
        }

        private void simpleButton_XPlus_Click(object sender, EventArgs e)
        {
            CMD_ButtonClick(false);
        }

        private void frmManuMotion_FormClosing(object sender, FormClosingEventArgs e)
        {
            SelectedMng.FuncEvtHndl.OnTcpIpReceive -= FuncEvtHndl_OnTcpIpReceive;
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            filterableTextBox_CMD.Text = numericTextBox_MAX.Text;
            filterableTextBox_RPM.Text = RPM.ToString();
            arcScaleComponent1.Value = Math.Abs(RPM);
            if(filterableTextBox_CMD.Text != "")
                arcScaleComponent2.Value = Convert.ToSingle(filterableTextBox_CMD.Text);

            // filterableTextBox_CMD.Text = dataArr[0];
            // filterableTextBox_ENC.Text = dataArr[1];

            //windowsUIButtonPanel_MonitorAxis.Buttons["EMG"].Properties.Checked = dataArr[2] != "0";
            //windowsUIButtonPanel_MonitorAxis.Buttons["BUSY"].Properties.Checked = dataArr[3] != "0";
            //windowsUIButtonPanel_MonitorAxis.Buttons["ORG"].Properties.Checked = dataArr[4] != "0";
            //windowsUIButtonPanel_MonitorAxis.Buttons["+Limit"].Properties.Checked = dataArr[5] != "0";
            //windowsUIButtonPanel_MonitorAxis.Buttons["-Limit"].Properties.Checked = dataArr[6] != "0";
            //windowsUIButtonPanel_MonitorAxis.Buttons["Alarm"].Properties.Checked = dataArr[7] != "0";
            //windowsUIButtonPanel_MonitorAxis.Buttons["Home"].Properties.Checked = dataArr[8] != "0";
        }
    }
}
