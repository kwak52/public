using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CpTesterPlatform.CpTesterSs;

namespace CpTesterPlatform.CpTester
{
    public partial class ucPLCMonitor : UserControl
    {
        private CpPlcIF PlcIF;
        private Timer timer1 = new Timer();
        public string GROUP_PLC { get { return groupControl_PLC.Text; } set { groupControl_PLC.Text = value; } }
        public string GROUP_PC { get { return groupControl_PC.Text; } set { groupControl_PC.Text = value; } }
        public string PLC_M0 { get { return simpleButton_M0.Text; } set { simpleButton_M0.Visible = true; simpleButton_M0.Text = value; } }
        public string PLC_M1 { get { return simpleButton_M1.Text; } set { simpleButton_M1.Visible = true; simpleButton_M1.Text = value; } }
        public string PLC_M2 { get { return simpleButton_M2.Text; } set { simpleButton_M2.Visible = true; simpleButton_M2.Text = value; } }
        public string PLC_M3 { get { return simpleButton_M3.Text; } set { simpleButton_M3.Visible = true; simpleButton_M3.Text = value; } }
        public string PLC_M4 { get { return simpleButton_M4.Text; } set { simpleButton_M4.Visible = true; simpleButton_M4.Text = value; } }
        public string PLC_M5 { get { return simpleButton_M5.Text; } set { simpleButton_M5.Visible = true; simpleButton_M5.Text = value; } }
        public string PLC_M6 { get { return simpleButton_M6.Text; } set { simpleButton_M6.Visible = true; simpleButton_M6.Text = value; } }
        public string PLC_M7 { get { return simpleButton_M7.Text; } set { simpleButton_M7.Visible = true; simpleButton_M7.Text = value; } }
        public string PLC_M8 { get { return simpleButton_M8.Text; } set { simpleButton_M8.Visible = true; simpleButton_M8.Text = value; } }
        public string PLC_M9 { get { return simpleButton_M9.Text; } set { simpleButton_M9.Visible = true; simpleButton_M9.Text = value; } }

        public string PC_M0 { get { return simpleButton_PC_M0.Text; } set { simpleButton_PC_M0.Visible = true; simpleButton_PC_M0.Text = value; } }
        public string PC_M1 { get { return simpleButton_PC_M1.Text; } set { simpleButton_PC_M1.Visible = true; simpleButton_PC_M1.Text = value; } }
        public string PC_M2 { get { return simpleButton_PC_M2.Text; } set { simpleButton_PC_M2.Visible = true; simpleButton_PC_M2.Text = value; } }
        public string PC_M3 { get { return simpleButton_PC_M3.Text; } set { simpleButton_PC_M3.Visible = true; simpleButton_PC_M3.Text = value; } }
        public string PC_M4 { get { return simpleButton_PC_M4.Text; } set { simpleButton_PC_M4.Visible = true; simpleButton_PC_M4.Text = value; } }
        public string PC_M5 { get { return simpleButton_PC_M5.Text; } set { simpleButton_PC_M5.Visible = true; simpleButton_PC_M5.Text = value; } }
        public string PC_M6 { get { return simpleButton_PC_M6.Text; } set { simpleButton_PC_M6.Visible = true; simpleButton_PC_M6.Text = value; } }
        public string PC_M7 { get { return simpleButton_PC_M7.Text; } set { simpleButton_PC_M7.Visible = true; simpleButton_PC_M7.Text = value; } }
        public string PC_M8 { get { return simpleButton_PC_M8.Text; } set { simpleButton_PC_M8.Visible = true; simpleButton_PC_M8.Text = value; } }
        public string PC_M9 { get { return simpleButton_PC_M9.Text; } set { simpleButton_PC_M9.Visible = true; simpleButton_PC_M9.Text = value; } }

        public ucPLCMonitor()
        {
            InitializeComponent();

            simpleButton_M0.Visible = false;
            simpleButton_M1.Visible = false;
            simpleButton_M2.Visible = false;
            simpleButton_M3.Visible = false;
            simpleButton_M4.Visible = false;
            simpleButton_M5.Visible = false;
            simpleButton_M6.Visible = false;
            simpleButton_M7.Visible = false;
            simpleButton_M8.Visible = false;
            simpleButton_M9.Visible = false;


            simpleButton_PC_M0.Visible = false;
            simpleButton_PC_M1.Visible = false;
            simpleButton_PC_M2.Visible = false;
            simpleButton_PC_M3.Visible = false;
            simpleButton_PC_M4.Visible = false;
            simpleButton_PC_M5.Visible = false;
            simpleButton_PC_M6.Visible = false;
            simpleButton_PC_M7.Visible = false;
            simpleButton_PC_M8.Visible = false;
            simpleButton_PC_M9.Visible = false;

            timer1.Enabled = true;
            timer1.Interval = 200;

        }

        public void SetButtonState(int index, bool bPLC, bool bOn)
        {
            if (bPLC)
            {
                switch (index)
                {
                    case 0: simpleButton_M0.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 1: simpleButton_M1.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 2: simpleButton_M2.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 3: simpleButton_M3.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 4: simpleButton_M4.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 5: simpleButton_M5.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 6: simpleButton_M6.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 7: simpleButton_M7.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 8: simpleButton_M8.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 9: simpleButton_M9.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0: simpleButton_PC_M0.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 1: simpleButton_PC_M1.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 2: simpleButton_PC_M2.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 3: simpleButton_PC_M3.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 4: simpleButton_PC_M4.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 5: simpleButton_PC_M5.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 6: simpleButton_PC_M6.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 7: simpleButton_PC_M7.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 8: simpleButton_PC_M8.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                    case 9: simpleButton_PC_M9.Appearance.BackColor = bOn ? Color.ForestGreen : Color.Gray; break;
                }

            }
        }

        public void SetDefaultText()
        {
            GROUP_PLC = "PLC";
            PLC_M0 = "READY";
            PLC_M1 = "AUTO";
            PLC_M2 = "STOP";
            PLC_M3 = "START";
            PLC_M4 = "PASS";

            GROUP_PC = "PC";
            PC_M0 = "READY";
            PC_M1 = "AUTO";
            PC_M2 = "STOP";
            PC_M3 = "RUNNING";
            PC_M4 = "FINISH";
        }


        public void SetPlcIF(CpPlcIF plcIF)
        {
            PlcIF = plcIF;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            SetButtonState(0, true, PlcIF.PLC_READY);
            SetButtonState(1, true, PlcIF.PLC_AUTO);
            SetButtonState(2, true, PlcIF.PLC_STOP);
            SetButtonState(3, true, PlcIF.PLC_START);
            SetButtonState(4, true, PlcIF.PLC_PASS);
            SetButtonState(0, false, PlcIF.PC_READY);
            SetButtonState(1, false, PlcIF.PC_AUTO);
            SetButtonState(2, false, PlcIF.PC_STOP);
            SetButtonState(3, false, PlcIF.PC_RUNNING);
            SetButtonState(4, false, PlcIF.PC_FINISHED);
        }
    }
}
