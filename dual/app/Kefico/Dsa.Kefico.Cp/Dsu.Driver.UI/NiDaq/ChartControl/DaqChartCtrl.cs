using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraCharts;
using Dsu.Common.Utilities.ExtensionMethods;
using NiDaqFs = Dsu.Driver.NiDaq;
using Dsu.Common.Utilities;
using System.Diagnostics;
using System.Drawing;


namespace Dsu.Driver.UI.NiDaq
{
    /// <summary>
    /// DevExpress Chart 의 SwiftPlot 을 지원하기 위한 user control.   see EmDevChartSeries.cs
    /// </summary>
    /*
     * 연속 측정 graph 출력을 위한 코드
     * 
     * Core 부분은 CollectAsync 후에 RedrawChart 의 반복이다.
     * RedrawChart 를 빨리 수행할 수 있도록 만드는 것이 관건이다.
     *  - new 를 통한 memory allocation 을 없앨 것.
     *   . _newPoints[] array 에 사용할 SerisePoint 객체를 미리 만들어 둔다.
     *   . 병렬 처리를 통해 빨리 값을 할당한다.
     *   RedrawChart 의 core 부분은 가장 쉽게는 다음과 같이 구현가능하다.
     *     _newPoints[i] = new SeriesPoint(arg + TimeSpan.FromMilliseconds(i), data[i]);
     *   ==> Redraw chart 에서 CLR 에 부담을 주어 속도 저하 유발
     *   . 한번 생성한 SeriesPoint 를 재사용할 수 있는 방법이 필요.
     *   . DevExpress 의 SeriesPoint 는 기본적으로 생성자에서 모든 argument 를 받도록 하고 있고, argument 및 value 값을 변경하는 것은 private 로 막혀 있다.
     *   . Reflection 을 통해서 private method 를 호출
     *     - SeriesPoint::SetDateTimeArgument 및 SeriesPoint::InitializeValues method
     */
    public class DaqChartCtrl : TimeSeriesChartCtrl
    {
        public SwiftPlotDiagram Diagram => (SwiftPlotDiagram)_chart.Diagram;
        public string Channel { get; set; }
        public double SamplingRate { get; set; }
        public int NumSamplesPerBuffer { get; set; }
        public int CollectNumberMultiplier { get; set; } = 1;
        public Nullable<int> LowpassCutoffFrequency { get; set; }
        public bool UseFixedYRange { get; set; }

        //public bool ActivateDynamicDraw { get; set; }

        /// No. of samples visible on chart window
        private int _visibleNumberOfSamples;

        // { 속도 증가를 위한 사전 allocation 구조들
        private SeriesPoint[] _newPoints;
        private List<object[]> _parameters1;
        private List<object[]> _parameters2;
        private List<double[]> _values;
        // }

        private object _lock = new object();
        private DateTime _argMin = DateTime.MinValue;


        private MethodInfo _miSetDateTimeArgument;
        private MethodInfo _miInitializeValues;

        public DaqChartCtrl()
        {
            //ActivateDynamicDraw = true;
            var t = typeof(SeriesPoint);
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            _miSetDateTimeArgument = t.GetMethod("SetDateTimeArgument", bindingFlags);
            _miInitializeValues = t.GetMethod("InitializeValues", bindingFlags, null, new[] { typeof(double[]) }, null);
        }

        protected override void RegisterEventHandler()
        {
            // on mouse Ctrl R-click, shows tooltips on DAQ device information. 
            _chart.MouseClick += (sndr, e) =>
            {
                bool shift = ModifierKeys == Keys.Shift;
                if (e.Button == MouseButtons.Right && shift)
                {
                    var msg = $"\r\nChannel={Channel}\r\nSampling rate={SamplingRate}\r\nVisible No. samples={_visibleNumberOfSamples}";
                    new PopupToolTip("DAQ info").Show(msg, _chart, duration: 5000);
                }
            };
        }

        public void Iniitialize(string channel, double samplingRatio, int numSamplesPerBuffer, Nullable<int> lowpassCutoffFrequency)
        {
            Channel = channel;
            SamplingRate = samplingRatio;
            NumSamplesPerBuffer = numSamplesPerBuffer;
            LowpassCutoffFrequency = lowpassCutoffFrequency;
        }

        private NiDaqScAi.DaqScAiManager CreateScAiManager(string channel)
        {
            var singleChannelParams = new NiDaqParams.DaqScAiParams(channel);
            var perChannelParams = new[] { singleChannelParams };

            var mcParams = new NiDaqParams.DaqMcAiParams(perChannelParams) { SamplingRate = SamplingRate, NumberOfSamples = NumSamplesPerBuffer };

            NiDaqFs.CreateMcManager(mcParams);
            //_daqManager = new DaqMcAiManager(mcParams);
            var scManager = NiDaqFs.CreateScManager(channel);
            return scManager;
        }

        private ConstantLine _minLine;
        private ConstantLine _maxLine;
        public void SetMinMax()
        {
            if (_minLine != null)
                Diagram.AxisY.ConstantLines.Remove(_minLine);
            if (_maxLine != null)
                Diagram.AxisY.ConstantLines.Remove(_maxLine);

            _minLine = null;
            _maxLine = null;
        }

        public void SetMinMaxGuideLine(double min, double max)
        {
            SetMinMax();    // clear previous min/max
            _minLine = new ConstantLine("Min", min) { Color = Color.DarkRed, ShowInLegend = false };
            _maxLine = new ConstantLine("Max", max) { Color = Color.DarkRed, ShowInLegend = false };
            //Diagram.AxisY.WholeRange.SetMinMaxValues(min-0.5, max+0.5);
            Diagram.AxisY.ConstantLines.Add(_minLine);
            Diagram.AxisY.ConstantLines.Add(_maxLine);
        }

        protected double[] CompressData(double[] data)
        {
            if (data.Length == NumSamplesPerBuffer)
                return data;

            if (data.Length > NumSamplesPerBuffer)
            {
                return data.Skip(data.Length - NumSamplesPerBuffer).ToArray();

                //var everyNth = data.Length / NumSamplesPerBuffer;
                //var compressedData = new double[NumSamplesPerBuffer];

                //int i = 0, j = 0;
                //for ( ; i < data.Length; i+=everyNth, j++ )
                //    compressedData[j] = data[i];
                //return compressedData;

                ////return data.Where((int i, double d) => i % everyNth == 0).ToArray();
            }

            return data;
        }

        public void Prepare()
        {
            _newPoints = new SeriesPoint[NumSamplesPerBuffer];


            // { 속도 증가를 위한 사전 allocation 구조들
            _parameters1 = new List<object[]>(NumSamplesPerBuffer);
            _parameters2 = new List<object[]>(NumSamplesPerBuffer);
            _values = new List<double[]>(NumSamplesPerBuffer);
            for (int i = 0; i < NumSamplesPerBuffer; i++)
            {
                var dvalues = new double[] { 0.0 };
                _values.Add(dvalues);
                _parameters1.Add(new object[] { dvalues });
                _parameters2.Add(new object[] { null, false });
            }

            Parallel.For(0, NumSamplesPerBuffer, i => {
                _newPoints[i] = new SeriesPoint(DateTime.Now, 0.0);
            });

            // }
        }

        /// <summary>
        /// 연속 sampling 결과를 chart 에 표시한다.
        /// 안정적 parameter
        ///     - AO(출력): 터미널=RSE, 생성 모드=연속 샘플, 생성할 샘플=2K, 속도=220K
        ///     - UI 설정 : Sampling Rate=2K, Num Samples/Buffer=200
        /// </summary>
        public async Task Run()
        {
            Prepare();

            IsRunning = true;
            //UseWaitCursor = true;

            _cts = new CancellationTokenSource();
            var scManager = CreateScAiManager(Channel);

            //https://www.devexpress.com/Support/Center/Example/Details/E310
            var chart = _chart;
            var series = chart.Series[0].Points;

            await Task.Run(async () =>
            {
                try
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        // control 이 안보이는 상태로 있을 때에 계속 그리면, queue 에 누적만 되고 실제 drawing 은 발생하고 있지 않다가
                        // control 이 보일 때(minimize 해제시), queue 에 쌓인 event 들을 처리하느라 UI 가 먹통되는 현상이 발생한다.
                        // control 이 처음 보일 때는 Paint event 를 받아서 처리하면 되지만, minimize 되는 순간을 catch 할 수 없다.
                        // - DevXpress Swift chart drawing 방식은 Paint 와 별 관련 없이 data 만 수정해서 chart update 되는 것 처럼 보이므로
                        //   Paint event 를 이용해서 처리할 수 없다.
                        if (!this.IsControlVisibleToUser())
                            continue;

                        //if ( ! ActivateDynamicDraw )
                        //{
                        //    await Task.Delay(RedrawPause);
                        //    continue;
                        //}

                        /*
                         * 비동기로 정해진 샘플 수를 획득한 후,
                         * chart 에 그린다.
                         */
                        var data = await scManager.CollectAsync(CollectNumberMultiplier * NumSamplesPerBuffer);
                        if (CollectNumberMultiplier > 1)
                            data = data.Where((d, i) => i % CollectNumberMultiplier == 0).ToArray();

                        _visibleNumberOfSamples = data.Length;

                        await RedrawChart(series, data);
                        await Task.Delay(RedrawPause);
                        scManager.ReturnBuffer(data);
                    }
                }
                catch (Exception ex)
                {
                    Log4NetWrapper.gLogger?.Error($"Exception on DaqChart: {ex}");
                }
            }, _cts.Token);

            //UseWaitCursor = false;
            IsRunning = false;
        }

        public void Clear()
        {
            _chart.Series[0].Points.Clear();
            _chart.Titles.Clear();
        }

        protected async Task RedrawChart(double[] data)
        {
            SeriesPointCollection series = _chart.Series[0].Points;
            await RedrawChart(series, data);
        }


        // see EmDevChartSeries.DrawDataFast
        protected async Task RedrawChart(SeriesPointCollection series, double[] dataOriginal)
        {
            await Task.Delay(0);

            //_argMin = DateTime.Now;
            lock (_lock)
            {
                ++_redrawCounter;

                var data = CompressData(dataOriginal);
                _visibleNumberOfSamples = data.Length;

                var arg = _argMin;
                var n = data.Length;

                var sc = series.Count;
                if (sc > 0)
                    series.RemoveRange(0, sc);

                Parallel.For(0, n, i => {
                    /* 기본 algorithm 은 다음과 같으나, 속도 증가를 위해서 풀어 씀 
                     * _newPoints[i] = new SeriesPoint(arg + TimeSpan.FromMilliseconds(i), data[i]);
                     */
                    var point = _newPoints[i];
                    var parameters1 = _parameters1[i];
                    var parameters2 = _parameters2[i];
                    _values[i][0] = data[i];    // parameters1[0] = new[] { data[i] }; 와 동일한 효과.
                    _miInitializeValues.Invoke(point, parameters1);

                    parameters2[0] = arg + TimeSpan.FromMilliseconds(i);
                    _miSetDateTimeArgument.Invoke(point, parameters2);
                });


                // Cannot await in the body of a lock statement
                /*await*/
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                this.DoAsync(() =>
                {
                    if (UseFixedYRange && _minLine != null && _maxLine != null)
                    {
                        var min = (double)_minLine.AxisValue;
                        var max = (double)_maxLine.AxisValue;
                        var gap = (max - min) * 3.0;

                        if (data.Any(d => d < min - gap || d > max + gap))
                            Trace.WriteLine("Contains invalid value.");

                        Diagram.AxisY.WholeRange.MaxValue = max + gap;
                        Diagram.AxisY.WholeRange.MinValue = min - gap;
                        Diagram.AxisY.WholeRange.Auto = false;
                    }


                    Diagram.AxisX.WholeRange.MinValue = _argMin;
                    Diagram.AxisX.WholeRange.MaxValue = _argMin + TimeSpan.FromMilliseconds(n);
                    Diagram.AxisX.VisualRange.MinValue = _argMin;
                    Diagram.AxisX.VisualRange.MaxValue = _argMin + TimeSpan.FromMilliseconds(n);



                    if (RedrawChartProc != null)
                        RedrawChartProc(RedrawCounter);

                    if (data.Length == _newPoints.Length)
                        series.AddRange(_newPoints);
                    else
                    {
                        NumSamplesPerBuffer = data.Length;
                        Prepare();
                        return;
                    }

                    _argMin = arg + TimeSpan.FromMilliseconds(n);
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

    }

}
