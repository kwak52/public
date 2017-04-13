using System;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraCharts;

namespace Dsu.Driver.UI.NiDaq
{
    public partial class TimeSeriesChartCtrl : UserControl
    {
        public ChartControl DevChart => chartControl1;
        protected ChartControl _chart => chartControl1;

        protected CancellationTokenSource _cts;
        /// Redraw 시의 pause (milliseconds)
        public int RedrawPause { get; set; } = 2000;
        public bool IsRunning { get; set; }

        public bool IsCrosshairEnabled
        {
            get { return _chart.CrosshairEnabled == DevExpress.Utils.DefaultBoolean.True; }
            set { _chart.CrosshairEnabled = value ? DevExpress.Utils.DefaultBoolean.True : _chart.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False; }
        }

        public Action<int> RedrawChartProc { get; set; }

        protected int _redrawCounter = 0;
        public int RedrawCounter { get { return _redrawCounter; } set { _redrawCounter = value; } }


        public TimeSeriesChartCtrl()
        {
            InitializeComponent();
        }

        private void TimeSeriesChartCtrl_Load(object sender, EventArgs e)
        {
            RegisterEventHandler();
        }

        protected virtual void RegisterEventHandler() {}


        public void Stop()
        {
            _cts?.Cancel();
        }

        public void SetAutoRange()
        {
            SwiftPlotDiagram diagram = _chart.Diagram as SwiftPlotDiagram;
            diagram.AxisY.WholeRange.Auto = true;
        }


        public void SetMinMaxRange(double min, double max)
        {
            SwiftPlotDiagram diagram = _chart.Diagram as SwiftPlotDiagram;
            diagram.AxisY.WholeRange.Auto = false;
            diagram.AxisY.WholeRange.SetMinMaxValues(min, max);
        }
    }
}
