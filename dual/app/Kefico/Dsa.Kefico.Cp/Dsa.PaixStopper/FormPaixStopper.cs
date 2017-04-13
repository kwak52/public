using Dsu.Common.Utilities.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.Driver.Paix;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Threading;

namespace Dsa.PaixStopper
{
    /// <summary>
    /// PAIX 긴급 정지를 위한 button application
    /// </summary>
    public partial class FormPaixStopper : Form
    {
        private Manager _managerRobot;       // PAIX manager 
        private Manager _managerMotor;       // PAIX manager 
        private Manager _managerUDIO;       // PAIX manager 
        private short emergency;
        private List<short> lstMc = new List<short>();

        public FormPaixStopper()
        {
            InitializeComponent();
        }

        private void FormPaixStopper_Load(object sender, EventArgs e)
        {
            try
            {
                var formMotion = new FormSimpleEditor()
                {
                    Title = "Enter Motion IP:",
                    Multiline = true,
                    ReadOnly = false,
                    Contents = "Robot IP\t:192.168.0.13\r\nMotor IP\t:192.168.0.14",
                };

                if (formMotion.ShowDialog() == DialogResult.OK)
                {
                    string[] info = formMotion.Contents.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (info.Length != 2)
                        Close();
                    _managerRobot = new Manager(info[0].Split(':')[1], true);
                    _managerMotor = new Manager(info[1].Split(':')[1], true);
                }
                else
                    Close();

                var formUdio = new FormSimpleEditor()
                {
                    Title = "Enter DIO Infomation:",
                    Multiline = true,
                    ReadOnly = false,
                    Contents = "IP\t:192.168.0.12\r\nEMG BIT\t:8\r\nMC BIT\t:0;1;2;3;4",
                };

                if (formUdio.ShowDialog() == DialogResult.OK)
                {
                    string[] info = formUdio.Contents.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (info.Length != 3)
                        Close();

                    _managerUDIO = new Manager(info[0].Split(':')[1], false);
                    _managerUDIO.SetProtocolMethod(1); //  1 is UDP, 0 is TCP
                    _managerUDIO.OpenPaix();
                    emergency = Convert.ToInt16(info[1].Split(':')[1]);
                    info[2].Split(':')[1].Split(';').ForEach(MC => lstMc.Add(Convert.ToInt16(MC)));

                    SetMcControl(true);

                    Task taskWatch = new Task(() => taskWatchSensor());
                    taskWatch.Start();
                }
                else
                    Close();

                btnStop.BackColor = Color.Green;
                TopMost = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create stopper.\n{ex.ToString()}", "Exception:");
                Application.Exit();
            }
        }

        private void taskWatchSensor()
        {
            while (true)
            {
                Thread.Sleep(50);

                short[] anInStatus = new short[128];
                _managerUDIO.GetDIOInput128(anInStatus);
                if (anInStatus[emergency] == 0)   // emergency index 8 
                {
                    StopAll();
                    btnStop.BackColor = Color.Red;
                }
                else
                    btnStop.BackColor = Color.Green;

            }
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            StopAll();
        }

        private void StopAll()
        {
            short[] allAxes = { 0, 1, 2, 3, 4, 5, 6, 7 };
            allAxes.ForEach(ax => _managerRobot.SuddenStop(ax));
            allAxes.ForEach(ax => _managerMotor.SuddenStop(ax));
        }

        private void FormPaixStopper_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (_managerUDIO != null)
            //    SetMcControl(false);   // mc off => servo error => alarmclear => break off
        }

        private void SetMcControl(bool bOn)
        {
            for (int i = 0; i < lstMc.Count; i++)
                _managerUDIO.SetDIOOutputBit(lstMc[i], (short)1);
        }

    }
}
