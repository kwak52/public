using System;
using DevExpress.XtraCharts;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Driver.UI.NiDaq
{
    public static class EmDevChartSeries
    {
        public static SwiftPlotDiagram GetSwiftPlotDiagram(this ChartControl chart) => GetCastedDiagram<SwiftPlotDiagram>(chart);
        public static T GetCastedDiagram<T>(this ChartControl chart) where T : Diagram => chart.Diagram as T;
        public static void SetMinMax(this SwiftPlotDiagram diagram, object min, object max)
        {
            diagram.AxisX.WholeRange.SetMinMaxValues(min, max);
        }

        /// <summary>
        /// DevExpress chart 에 data 를 빠르게 draw 한다.
        /// SeriesPoint[] points 에 대규모 array 를 생성하는데 걸리는 시간을 save 하기 위해서 사전에 할당된 points 를 사용하고자 할 때 이용
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="data"></param>
        /// <param name="series"></param>
        /// <param name="points"></param>
        public static void DrawDataFast(this ChartControl chart, double[] data, SeriesPointCollection series, SeriesPoint[] points)
        {
            var arg = DateTime.Now;
            var n = data.Length;
            Parallel.For(0, n, i =>
            {
                points[i] = new SeriesPoint(arg + TimeSpan.FromMilliseconds(i), data[i]);
            });

            var sc = series.Count;
            if (sc > 0)
                series.RemoveRange(0, sc);

            series.AddRange(points);
            chart.GetSwiftPlotDiagram().SetMinMax(arg, arg + TimeSpan.FromMilliseconds(n));
        }

        /// DevExpress chart 에 data 를 draw 한다.
        public static void DrawData(this ChartControl chart, double[] data, bool enableCustomDrawCrosshair=true)
        {
            if (enableCustomDrawCrosshair)
                chart.EnableCustomDrawCrosshair();

            var series = chart.Series[0].Points;
            var points = new SeriesPoint[data.Length];
            chart.DrawDataFast(data, series, points);
        }


        private static CustomDrawCrosshairEventHandler _defaultDrawCustomhairFunc;


        // Cross hair 는 DevExpress chart 에서 cursor 를 따라다니는 세로줄의 guide 를 의미한다.
        // Cross hair 를 움직일 시, series 의 해당 point 값을 popup 으로 표시하는데, 
        // 기본 값은 date time 으로 되어 있는데, 이를 series 의 array index 로 변경하기 위한 작업 수행.
        // 기본 가정: series 의 point array 는 1 millisecond 의 간격으로 입력되어 있어야 한다.
        /// <summary>
        /// Cross hair 의 custom draw 를 활성화한다.  cross hair 에서 시간 대신 array index 로 표시한다.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="drawCustomhairFunc">사용자 지정 custom draw handler.  null 이면 default handler 가 사용된다. </param>
        public static void EnableCustomDrawCrosshair(this ChartControl chart, CustomDrawCrosshairEventHandler drawCustomhairFunc=null)
        {
            var axis = chart.GetSwiftPlotDiagram().AxisX;
            //axis.Visibility = DevExpress.Utils.DefaultBoolean.False;        // label 이 너무 많아서 혼잡해 보이는 것을 방지
            axis.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;

            if (drawCustomhairFunc != null)
                _defaultDrawCustomhairFunc = drawCustomhairFunc;

            if (_defaultDrawCustomhairFunc == null)
            {
                // https://documentation.devexpress.com/#WindowsForms/CustomDocument14535
                _defaultDrawCustomhairFunc = (sndr, e) =>
                {
                    try
                    {
                        var points = chart.Series[0].Points;
                        //if ( points.Count > 0 )
                        {
                            var min = points[0].DateTimeArgument;
                            foreach (CrosshairElementGroup group in e.CrosshairElementGroups)
                            {
                                if (group.CrosshairElements[0] != null)
                                    group.HeaderElement.Text = $"Total {points.Count} samples.";

                                foreach (var ele in group.CrosshairElements)
                                {
                                    var span = ele.SeriesPoint.DateTimeArgument - min;
                                    var index = span.TotalMilliseconds;
                                    var value = ele.SeriesPoint.Values[0];
                                    ele.LabelElement.Text = $"[{index}] = {value}";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Ignoreing unhandled exception: {ex}");
                    }

                };
            }


            chart.CustomDrawCrosshair += _defaultDrawCustomhairFunc;
        }

        public static void DisableCustomDrawCrosshair(this ChartControl chart)
        {
            chart.CustomDrawCrosshair -= _defaultDrawCustomhairFunc;
        }

        public static void ShowLegend(this ChartControl chart) => chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
        public static void HideLegend(this ChartControl chart) => chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        public static void AddAdditionalSeries(this ChartControl chart, double[] data, string legend)
        {
            var series0 = chart.Series[0].Points;
            var points = new SeriesPoint[data.Length];
            var arg = series0[0].DateTimeArgument;
            var n = data.Length;
            Parallel.For(0, n, i =>
            {
                points[i] = new SeriesPoint(arg + TimeSpan.FromMilliseconds(i), data[i]);
            });

            var newSeries = new Series(legend, ViewType.SwiftPlot) { LegendText = legend };
            newSeries.ColorDataMember = "Black";
            newSeries.Points.AddRange(points);
            chart.Series.Add(newSeries);

            //var sc = series0.Count;
            //if (sc > 0)
            //    series0.RemoveRange(0, sc);

            chart.GetSwiftPlotDiagram().SetMinMax(arg, arg + TimeSpan.FromMilliseconds(n));
        }


        public static void SetTitle(this ChartControl chart, string title)
        {
            chart.Titles.Clear();
            if ( title.NonNullAny() )
            {
                var chartTitle = new DevExpress.XtraCharts.ChartTitle() { Text = title };
                chart.Titles.Add(chartTitle);
            }
        }
    }
}
