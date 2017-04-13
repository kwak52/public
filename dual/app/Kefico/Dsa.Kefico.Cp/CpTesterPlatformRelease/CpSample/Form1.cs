using LanguageExt;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.Driver;
using Paix = Dsu.Driver.Paix;
using static Dsu.Driver.Ups;
using static Dsu.Driver.Hioki;
using static Dsu.Driver.Sorensen;
using static Dsu.Driver.NiVISA;

namespace CpSample
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            TestLCR();
//             var plc = new MxPlcManager();
//             var a = plc.GetDevice("X1");

            //             var a = Lvdt.BaudRate;
            //             Lvdt.Query("*r=?\r\n");
            //             Console.WriteLine(a);

            //            var cpuType = MelsecType.CpuType.NewQnACpu((QnACpuType)Enum.Parse(typeof(QnACpuType), "Q02H"));
            //            var parameters = new PlcParametersUdp(cpuType, "192.168.0.102");
            //            parameters.ActPortNumber = 11111;
            //            var plc = new PlcManager(parameters);
            //            plc.GetDevice("X1");
            //             var a1 = new PaixManager("192.168.0.11", true);
            //            Paix.createManager("192.168.0.11", true);
            //                        Sorensen.createManager("192.168.0.34", 9221, 1);
            //                        Sorensen.createManager("192.168.0.34", 9221, 2);
            //                        Sorensen.createManager("192.168.0.110", 9221, 2);

            //             var a3 = new PaixManager("192.168.0.11", true);
            //             var a4 = new PaixManager("192.168.0.11", true);
            //             var a5 = new PaixManager("192.168.0.11", true);
            //             var a6 = new PaixManager("192.168.0.11", true);
            //             var a7 = new PaixManager("192.168.0.11", true);
            //             var a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            //             a8 = new PaixManager("192.168.0.11", true);
            // 
            //             var s0 = new PowerSupplierManager("192.168.0.34", 9221, 1);
            //             s0.Voltage = 11;
            //             var s1 = new PowerSupplierManager("192.168.0.34", 9221, 2);
            //             s1.Voltage = 12;
            //             var s2 = new PowerSupplierManager("192.168.0.110", 9221, 1);
            //             s2.Voltage = 13;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            //Rs232cManager Lvdt = new Rs232cManager("COM6");
            ////  var x = Lvdt.Serial.Query("VER=?\r\n");
            //var a = Lvdt.QuerySimple("VER=?\r\n");
            // a = Lvdt.QuerySimple("*r?\r\n");
            //Console.WriteLine(a);

            //  TestPaix();

            //  TestLCR();
            TestUps();
            // TestPowerSupply();

            //TestPaixMotion();
            //    Console.WriteLine(Lvdt.Query("*P=?"));
            //      Console.WriteLine(Lvdt.Query("*Ver=?"));
            // var a = Lvdt.Write("*r? " + "\r\n");
            //var a =  Lvdt.Read("*r? " + "\r\n");
            //             var a = Lvdt.ReadAsync().Result;
            //             Console.WriteLine(a.ToString());


            //   Console.WriteLine(Lvdt.Query("*Ver=? \r\n"));
        }

        private Paix.Manager paix() => Paix.Module.manager();
        private UpsManager ups() => Ups.manager();
        private LCRMeterManager lcr() => Hioki.manager();
        private PowerSupplierManager ps() => Sorensen.manager();
        private PowerSupplierManager daq() => Sorensen.manager();
        private PowerSupplierManager plc() => Sorensen.manager();

        private void TestPaixMotion()
        {
            try
            {
                paix().SetCurrentOn(0, 1);
                paix().SetServoOn(0, 1);
                paix().SetUnitPerPulse(0, 1); //Unit 당 Pulse는 항상 1:1 (모터 엔코더 설정 따름 10000 Pulse/ 1Cycle)
                paix().SetSpeed(1, 0, 10000, 10000, 1000);
                paix().SetDisconectedStopMode(1000, 0);

                //                 paix().JogMove(0, 1);
                // 
                //                 Thread.Sleep(100);
                //                 int nRet = paix().GetBusyStatus(0).Value;
                // 
                //                 //paix().SuddenStop(0); 비상정지
                //                 paix().DecStop(0);
                //                    paix().SetCmdPos(0, 100);
                //                   paix().SetEncPos(0, 200);
                paix().RelMove(0, -1000000);
                var adCmdSpeed = paix().GetAxesCmdSpeed().Value;

                var aa = adCmdSpeed[Convert.ToInt16(0)];
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Exception occurred during PAIX manager creation:\n{ex}");
            }
        }


        private void TestPaix()
        {
            try
            {
                Paix.Module.createManager("192.168.0.12", true);

                short[] OutStatus = new short[128];
                short[] InStatus = new short[128];

                paix().GetDIOOutput(OutStatus);
                paix().SetDIOOutputTog(0);
                paix().GetDIOOutput(OutStatus);
                paix().GetDIOInput(InStatus);

                paix().SetDIOOutputTog(2);
                paix().SetDIOOutputTog(3);
                paix().SetDIOOutputTog(4);
                paix().SetDIOOutputTog(5);

                var a = paix().GetDIOInputBit(2)?.Value;
 
//                  paix().SetAlarmResetOn(0, 0);
//  
//                  var retALgc = paix().SetAlarmLogic(0, 0);
//                  //var retAlSt = paix().SetAlarmResetOn(0, 1);
//  
//                  var retPing = paix().PingCheck(100);
//                  var retOpen = paix().OpenDevice();
//                  var retCOn = paix().SetCurrentOn(0, 1);
//                  //var retDeg = paix().Set2PulseDir
//                  var retSOn = paix().SetServoOn(0, 1);
//                  var retSVel = paix().SetSpeed(0, 10, 1000, 1000, 1000);
//                  var retMove = paix().AbsMove(0, 10);
//                 //     paix().SetDIOOutput()
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Exception occurred during PAIX manager creation:\n{ex}");
            }
        }

        private void TestLCR()
        {
            try
            {
                Hioki.createManager("192.168.0.51", 3500);
                if (lcr() != null)
                {
                    lcr().Level = "V";
                    lcr().Mode = "LCR";
                    lcr().Speed = "MEDIUM";
                    lcr().Trigger = "INTERNAL";
                    lcr().Format = "ASCii";
                    lcr().Display1 = "CS";
                    lcr().Display2 = "LS";
                    lcr().Display3 = "RDC";
                    lcr().Display4 = "OFF";
                    lcr().MeasureItem = "72,64,0"; //MEASure 대상 설정 Default " Cs, Ls, Rdc" = "72,64,0"
                    lcr().Load = 3;
                    lcr().Reset = "RST";

                    Thread.Sleep(1000);

                    string rx = lcr().MEASure; //MeasureItem = "72,64,0" 일 경우 사용
                    double Cs = Convert.ToDouble(rx.Split(',')[0]);
                    double Ls = Convert.ToDouble(rx.Split(',')[1]);
                    double Rdc = Convert.ToDouble(rx.Split(',')[2]);

                    Console.WriteLine(string.Format("Cs {0}, Ls {1}, Rdc {2}",   Cs, Ls, Rdc));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred during Hioki manager creation:\n{ex}");
            }

        }

        private void TestUps()
        {
            Ups.createManager("192.168.0.30");

            for (int i = 0; i < 10000; i++)
            {
                Thread.Sleep(1000);
                double Temperature = ups().GetTemparature();
                double Humidity = ups().GetHumidity();
            }
        }

        private void TestPowerSupply()
        {
            try
            {
                 Sorensen.createManager("192.168.0.110", 9221, 1);
                ps().Current = 2;
                ps().Voltage = 24;
                ps().IsActive = true;
//                 ps().SetCurrent(1, 1);
//                  ps().SetVoltage(2, 22);
//                  ps().SetCurrent(2, 2);
//                  double current = ps().GetCurrent(1);
//                  double voltage = ps().GetVoltage(1);
//                  current = ps().GetCurrent(2);
//                  voltage = ps().GetVoltage(2);
//                  ps().SetIsActive(1, true);  // Active 미 수행시 특정 시간 1분?? 타임아웃으로 Remote 통신 자체가 안됨
//                  ps().SetIsActive(2, true);  // Active 미 수행시 특정 시간 1분?? 타임아웃으로 Remote 통신 자체가 안됨

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred during PAIX manager creation:\n{ex}");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var adCmdSpeed = paix().GetAxesCmdSpeed().Value;
            var aa = adCmdSpeed[Convert.ToInt16(0)];
            //paix().SuddenStop(0); //비상정지
            // paix().DecStop(0);
            paix().AllAxisStop(1);
            
        }
    }
}