using System;
using System.Windows.Forms;
using System.Linq;
using System.Reactive.Linq;
//using Dsu.Common.Utilities.Core.FSharpInterOp;
using Microsoft.FSharp.Core;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.Math;
using Dsu.Driver.UI.NiDaq;
using static Dsu.Driver.Base.Sampling;
using static Dsu.Driver.NiVISA;
using System.Diagnostics;
using static Socket;
using static Dsu.Driver.Sorensen;
using System.Threading;
using System.Threading.Tasks;

namespace DriverTestWinform
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            var noisy = makeSquareWaveNoisySamples(duty: 0.2, numSamplesPerCycle: 50, numCycles: 20, burstRatio: 0.2, flatNoiseRatio: 0.15);
            var parameters = new DaqSquareWaveDecisionParameters()
            {
                TrimRatioFront = 0.1,
                TrimRatioRear = 0.1,
                NumberOfTeeth = 5,
            };
            var daqSqWave = new DaqSquareWave(parameters, noisy, 10.0*1000.0);
            noisy = noisy.Skip(daqSqWave.StartIndex).Take(daqSqWave.EndIndex + 1 - daqSqWave.StartIndex).ToArray();

            var interval = daqSqWave.IntervalTime;
            var duration = daqSqWave.DurationTime;
            var duty = daqSqWave.Duty;
            var numRisingEdges = daqSqWave.NumRisingEdges;
            var numFallingEdges = daqSqWave.NumFallingEdges;
            var teethSamples = daqSqWave.TeethSamples;

            var toothAnal = daqSqWave.ToothAnalysis;

            var form = new FormDaqChart("Noisy/Clean", noisy, 100000.0, noisy.Length);
            form.Show();

            if (FSharpOption<ToothAnalysis[]>.get_IsSome(toothAnal))
                new FormTeethAnalysis(toothAnal.Value).Show();
        }


        private SwiftChartCtrl _swiftChart;
        private void btnSwiftChart_Click(object sender, EventArgs e)
        {
            var data = new[] { 0.0, 1.0, -1.0, 0.0, 1.0, -1.0, 0.0, 1.0, };
            _swiftChart = new SwiftChartCtrl(10, data);
 
            var form = new Form();
            _swiftChart.EmbedToControl(form);
            form.Show();
        }

        private Random _rand = new Random();
        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            _swiftChart.AddPoint(_rand.Next(100) / 100.0);
        }


        private Rs232cMonitor _rs232;
        private void btnRs232c_Click(object sender, EventArgs e)
        {
            // RS232 통신을 통해 event 를 받았을 때, 수행할 action 을 등록한다. (F# fun 으로 변환해서 등록)
            // event 는 object type 으로, 정상 상태의 읽은 string 과 오류 상태의 Exception 이 올 수 있다.
            // Exception 이 NationalInstruments.VisaNS.VisaException 이고, VISA error code -1073807339 (0xBFFF0015) 인 ErrorTimeout 은 
            // 232 통신에서 자주 발생하는 것으로 보이며, 무시해도 괜찮을 듯하다.
            var f = FuncConvert.ToFSharpFunc((object evt) =>
            {
                if (evt is string)
                    Trace.WriteLine($"Lambda__RS232C: {evt}");
                else //if (evt is Exception)
                    Trace.WriteLine($"EXCEPTION__RS232C: {evt}");
            });
            _rs232 = new Rs232cMonitor("ASRL3::INSTR", f) { BaudRate = 115200 };
        }

        private void btnPaixPath_Click(object sender, EventArgs e)
        {
            new Dsu.Driver.UI.Paix.FormPathPlanner(null)
            //{
            //    Poses = new []
            //    {
            //        new FormPathPlanner.MyPose(0, 0, 0, 0, "group1", "comment1"),
            //        new FormPathPlanner.MyPose(1, 0, 0, 0, "group2", "comment1"),
            //        new FormPathPlanner.MyPose(2, 0, 0, 0, "group3", "comment1"),
            //    }.ToList(),
            //    AxisSpecTransposed = new []
            //    {
            //        new AxisSpecTransposed(1, 0, 0, 0),     // Speed
            //        new AxisSpecTransposed(1, 2, 0, 0),     // Acceleration
            //        new AxisSpecTransposed(1, 2, 3, 0),     // Deceleration
            //        new AxisSpecTransposed(1, 2, 3, 4),     // Max
            //    }.ToList(),
            //}
            .Show();
        }

        private void btnPollyTest_Click(object sender, EventArgs e)
        {
            TcpClientManager manager;

            manager = new TcpClientManager("212.170.136.45", 80);
            Trace.WriteLine("Succeeded.");
            //var sleeps = Enumerable.Range(1, 10).Select(n => TimeSpan.FromMilliseconds(n * 100));
            //int retrial = 0;
            //Policy
            //    .Handle<Exception>()
            //    .WaitAndRetry(sleepDurations:sleeps, onRetry:(exn, sleep, counter, context) => {
            //    //.Retry(10, (exn, counter) => {
            //        Trace.WriteLine($"Retrying {counter} on {exn}");
            //        retrial++;
            //    })
            //    .Execute(() =>
            //    {
            //        manager = new TcpClientManager( retrial < 8 ? "212.170.136.45" : "112.170.136.45", 80);
            //        Trace.WriteLine("Succeeded.");
            //    });
        }

        private void btnSorensen232_Click(object sender, EventArgs e)
        {
            var sorensen232 = new PowerSupplierManager("ASRL3::INSTR", 1) { BaudRate = 115200 };

            while (true)
            {
                var x = sorensen232.Rs232cManager.Read();
                Trace.WriteLine(x);
                Thread.Sleep(100);
            }

        }

//        private int _counter = 0;
        private async void btnAsyncTest_Click(object sender, EventArgs e)
        {
            await Task.Delay(0);
//#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//            Task.Run(async () =>
//            {
//                this.Do(() =>
//                {
//                    Thread.Sleep(500);
//                    Interlocked.Increment(ref _counter);
//                    labelMessage.Text = $"{_counter}";
//                    this.Do(async () =>
//                    {
//                        Thread.Sleep(1000);
//                        Interlocked.Increment(ref _counter);
//                        labelMessage.Text = $"{_counter}";
//                    });

//                });
//            });
//#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

//            this.Do(() =>
//            {
//                Thread.Sleep(1000);
//                Interlocked.Increment(ref _counter);
//                labelMessage.Text = $"{_counter}";
//                this.Do(() =>
//                {
//                    Thread.Sleep(2000);
//                    Interlocked.Increment(ref _counter);
//                    labelMessage.Text = $"{_counter}";
//                });

//            });


            //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            //            Task.Run(async () =>
            //            {
            //                await this.DoAsync(async () =>
            //                {
            //                    await Task.Delay(3000);
            //                    Interlocked.Increment(ref _counter);
            //                    labelMessage.Text = $"{_counter}";
            //                    await this.DoAsync(async () =>
            //                    {
            //                        await Task.Delay(3000);
            //                        Interlocked.Increment(ref _counter);
            //                        labelMessage.Text = $"{_counter}";
            //                    });

            //                });
            //            });
            //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            //            await this.DoAsync(async () =>
            //            {
            //                await Task.Delay(3000);
            //                Interlocked.Increment(ref _counter);
            //                labelMessage.Text = $"{_counter}";
            //                await this.DoAsync(async () =>
            //                {
            //                    await Task.Delay(3000);
            //                    Interlocked.Increment(ref _counter);
            //                    labelMessage.Text = $"{_counter}";
            //                });

            //            });

        }
    }
}
