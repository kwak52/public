using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Paix_MotionControler;
using Dsu.Driver;
using Paix = Dsu.Driver.Paix;

namespace IO
{
    public partial class Form1 : Form
    {
        short g_ndevId;
        short g_nDevopen;
        Thread TdWatchSensor;
        Paix.Manager _paix;

        public Form1()
        {
            _paix = new Paix.Manager("192.168.0.11", false);
            _paix.SetProtocolMethod(0); //  1 is UDP, 0 is TCP
            _paix.OpenPaix();

            InitializeComponent();
            TdWatchSensor = new Thread(new ThreadStart(watchSensor));
            g_ndevId = 0;
            g_nDevopen = 0;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            short nret;
            g_ndevId = Convert.ToInt16(textBoxDevNo.Text);

            if (buttonOpen.Text == "Open")
            {

                // 방화벽을 확인해 주십시요.
                if (NMC2.nmc_PingCheck(g_ndevId, 10) != 0)
                {
                    MessageBox.Show("Ping Check Error");
                    return;
                }
                nret = NMC2.nmc_OpenDevice(g_ndevId);
                if (nret == 0)
                {
                    g_nDevopen = 1;
                    switch (TdWatchSensor.ThreadState)
                    {
                        case ThreadState.Stopped:
                            TdWatchSensor = new Thread(new ThreadStart(watchSensor));
                            break;
                        case ThreadState.Unstarted:
                            break;
                        default:
                            TdWatchSensor.Abort();
                            //                        TdWatchSensor.Join();
                            while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }
                            break;
                    }

                    TdWatchSensor.Start();
                    buttonOpen.Text = "Close";
                }
            }
            else if (buttonOpen.Text == "Close")
            {
                g_nDevopen = 0;
                Paix_MotionControler.NMC2.nmc_CloseDevice(g_ndevId);

                TdWatchSensor.Abort();
                TdWatchSensor.Join();

                while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }

                buttonOpen.Text = "Open";
            }
        }
        public void watchSensor()
        {
            //PaixMotion.NMC2.NMC_AXES_EXPR NmcData;
            while (true)
            {
                System.Threading.Thread.Sleep(50);
                if (g_nDevopen == 0) break;
                this.Invoke(new delegateUpdateOutIn(updateOutIn));
            }

        }
        private delegate void delegateUpdateOutIn();

        private void updateOutIn()
        {
            if (g_nDevopen == 0) return;
            short nret,i;
            short[] OutStatus = new short[128];


            nret = NMC2.nmc_GetDIOOutput(g_ndevId, OutStatus);
            if (nret != 0) return;


            for (i = 0; i < 32; i++)
            {
                Button btn = (Controls.Find("Btn_Out" + i.ToString(), true) [0] as Button);
                btn.ImageIndex = OutStatus[i];
            }

            nret = NMC2.nmc_GetDIOInput(g_ndevId, OutStatus);
            if (nret != 0) return;
            for (i = 0; i < 32; i++)
            {
                Button btn = (Controls.Find("Btn_In" + i.ToString(), true)[0] as Button);
                btn.ImageIndex = OutStatus[i];
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (g_nDevopen == 1)
            {
                Paix_MotionControler.NMC2.nmc_CloseDevice(g_ndevId);

                TdWatchSensor.Abort();
                TdWatchSensor.Join();

                while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }
            }        
        }

        private void Btn_Out0_Click(object sender, EventArgs e)
        {
            if (g_nDevopen == 0) return;
            Button btn = (Button)sender;
            short bitno = Convert.ToInt16 (btn.Text);
            NMC2.nmc_SetDIOOutputTog(g_ndevId, bitno);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (g_nDevopen == 0) return;
            short bitno = (short)comboBox1.SelectedIndex;
            NMC2.nmc_SetDIOOutputBit(g_ndevId, bitno, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (g_nDevopen == 0) return;
            short bitno = (short)comboBox1.SelectedIndex;
            Paix_MotionControler.NMC2.nmc_SetDIOOutputBit(g_ndevId, bitno, 0);

        }

        private void buttonTestIt_Click(object sender, EventArgs e)
        {
            //short[] anOutStatus0 = new short[128];
            //short result = _paix.GetDIOOutput128(anOutStatus0);
            //System.Diagnostics.Trace.WriteLine(result);


            // get input
            System.Threading.Tasks.Task.Run(async () =>
            {
                while (true)
                {
                    await System.Threading.Tasks.Task.Delay(50);
                    short[] anInStatus = new short[128];
                    short result = _paix.GetDIOInput128(anInStatus);

                    if (result != 0)
                    {
                        if (result == -1)
                            System.Diagnostics.Trace.WriteLine("NMC2 Connection error");
                        else
                            System.Diagnostics.Trace.WriteLine($"NMC2 error = {result}");
                    }
                }
            });


            //// set output
            System.Threading.Tasks.Task.Run(async () =>
            {
                short[] anOutStatus0 = new short[128];
                short[] anOutStatus1 = Enumerable.Range(0, 128).Select(n => (short)1).ToArray();
                int i = 0;
                while (true)
                {
                    i++;
                    await System.Threading.Tasks.Task.Delay(1);
                    short result = _paix.SetDIOOutPin((short)(i % 128), (short)(i % 2));
                    System.Diagnostics.Debug.Assert(result == 0);
                    //result = Paix_MotionControler.NMC2.nmc_SetDIOOutPin(g_ndevId, (short)(i % 128), (short)(i % 2));
                    //System.Diagnostics.Debug.Assert(result == 0);

                    ////short result = 0;   // Paix_MotionControler.NMC2.nmc_SetDIOOutput(g_ndevId, i++ % 2 == 0 ? anOutStatus0 : anOutStatus1);
                    short[] anInStatus = new short[128];
                    short result2 = Paix_MotionControler.NMC2.nmc_GetDIOInput128(g_ndevId, anInStatus);
                    System.Diagnostics.Debug.Assert(result2 == 0);
                    if (result != 0 || result2 != 0)
                    {
                        System.Diagnostics.Trace.WriteLine("ERRRO");
                        if (result == -1)
                            System.Diagnostics.Trace.WriteLine("NMC2 Connection error");
                        else
                            System.Diagnostics.Trace.WriteLine($"NMC2 error = {result}");
                    }
                }
            });
        }
    }
}
