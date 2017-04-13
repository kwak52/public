using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraCharts;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Diagnostics.Contracts;

namespace Dsu.Driver.UI.NiDaq
{
    /// <summary>
    /// Run chart 와 같이, 최대 window size 는 정해져 있고,
    /// 1 sample 씩 shift 되면서 변하는 chart 에 대한 user control
    /// </summary>
    public class SwiftChartCtrl : TimeSeriesChartCtrl
    {
        private int _visibleSampleSize;
        private DateTime _argMin = DateTime.MinValue;
        private DateTime _argEnd;

        private SeriesPointCollection _series => _chart.Series[0].Points;

        public SwiftChartCtrl(int visibleSampleSize=0, IEnumerable<double> data=null)
        {
            if (visibleSampleSize > 0 && ! data.IsNullOrEmpty())
                Initialize(visibleSampleSize, data);
        }

        public void Initialize(int visibleSampleSize, IEnumerable<double> data)
        {
            Contract.Requires(visibleSampleSize > 0);
            var n = data.Count();
            double[] data_ = null;
            if (n > visibleSampleSize)
            {
                data_ = data.Skip(n - visibleSampleSize).Take(visibleSampleSize).ToArray();
                n = visibleSampleSize;
            }
            else
                data_ = data.ToArray();

            _visibleSampleSize = visibleSampleSize;
            var seriesPoints = new SeriesPoint[n];

            Parallel.For(0, n, i =>
            {
                seriesPoints[i] = new SeriesPoint(_argMin + TimeSpan.FromMilliseconds(i), data_[i]);
            });
            _argEnd = _argMin + TimeSpan.FromMilliseconds(n);

            _series.AddRange(seriesPoints);

            _chart.EnableCustomDrawCrosshair();
        }


        public void AddPoint(double point)
        {
            if (_series.Count == _visibleSampleSize)     // full-blown
                _series.RemoveAt(0);

            _argEnd = _argEnd + TimeSpan.FromMilliseconds(1);
            var sp = new SeriesPoint(_argEnd, point);
            _series.Add(sp);
        }
    }
}
