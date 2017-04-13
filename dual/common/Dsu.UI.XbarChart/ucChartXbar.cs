using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using Dsu.UI.XbarChart.EventHandler;

namespace Dsu.UI.XbarChart
{
    public partial class ucChartXbar : UserControl
    {
        public UEventHandlerBoundDataChanged UEventBoundDataChanged;
        private float _MinY;
        private float _MaxY;
        private string _DisplayType;
        public ucChartXbar()
        {
            InitializeComponent();
            InitializeChart();
            chartControl1.BoundDataChanged += ChartControl1_BoundDataChanged;
        }

        public int Points
        {
            get
            {
                if (chartControl1.Series.Count > 0)
                    return chartControl1.Series[0].Points.Count;
                else
                    return 0;
            }
        }

        private void ChartControl1_BoundDataChanged(object sender, EventArgs e)
        {
            if (UEventBoundDataChanged != null)
                UEventBoundDataChanged(sender, chartControl1.Series[0].Points.Count);
        }

        public string TitleMain { get { return chartControl1.Titles[0].Text; } set { chartControl1.Titles[0].Text = value; } }
        public string TitleSub { get { return chartControl1.Titles[1].Text; } set { chartControl1.Titles[1].Text = value; } }


        private void InitializeChart()
        {

            ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
            ChartTitle chartTitle3 = new DevExpress.XtraCharts.ChartTitle();
            ChartTitle chartTitle4 = new DevExpress.XtraCharts.ChartTitle();

            chartTitle1.Font = new System.Drawing.Font("Tahoma", 18F);
            chartTitle1.Text = "Title";
            chartTitle1.WordWrap = true;
            chartTitle2.Font = new System.Drawing.Font("Tahoma", 9F);
            chartTitle2.Text = "Sub Title";
            chartTitle2.WordWrap = true;
            chartTitle3.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
            chartTitle3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartTitle3.Text = "Time of Test";
            chartTitle4.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Left;
            chartTitle4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartTitle4.Text = "Value";
            this.chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1,
            chartTitle2,
            chartTitle3,
            chartTitle4});
        }

        public bool ShowChart(DataTable dataTable, string ColumnArgument, string ColumnValue, bool bScaleAuto, string DisplayType, bool ShowLevel = true, float Min = 0, float Max = 0, int LimitRecord = 10000)
        {
            if (dataTable.Columns.Count == 0)
                return false;

            _DisplayType = DisplayType;
            _MinY = Min;
            _MaxY = Max;

            AddSerise(ColumnValue, Color.Red);
            AddToolTip("{A:MMMMd HH:mm:ss} \r\n {V:#0.00}");
            AddConstantLine(ShowLevel);
            AddDayStrip(-1);

            SetAxisXRange(DateTimeMeasureUnit.Second, DateTimeGridAlignment.Second);
            SetAxisYRange();
            AxisYRangeAuto(bScaleAuto);

            DataSourceBinding(dataTable, ColumnArgument, ColumnValue, LimitRecord);

            return true;
        }

        public bool ShowChart(DataTable dataTable, string ColumnArgument, List<string> ColumnValues, bool bScaleAuto, string DisplayType, float Min = 0, float Max = 0, int LimitRecord = 10000)
        {
            if (dataTable.Columns.Count == 0)
                return false;

            _DisplayType = DisplayType;
            _MinY = Min;
            _MaxY = Max;

            Random random = new Random(3);
            for (int i = 0; i < ColumnValues.Count; i++)
            {
                int r = random.Next(0, 255);
                int g = random.Next(0, 255);
                int b = random.Next(0, 255);

                AddSerise(ColumnValues[i], Color.FromArgb(r,g,b));
                DataSourceBinding(dataTable, ColumnArgument, ColumnValues[i], LimitRecord);
            }

            AddToolTip("{A:MMMMd HH:mm:ss} \r\n {V:#0.00}");

            SetAxisXRange(DateTimeMeasureUnit.Second, DateTimeGridAlignment.Second);
            SetAxisYRange();
            AxisYRangeAuto(bScaleAuto);

            return true;
        }

        public void ClearChart()
        {
            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;
            if (diagram != null)
            {
                diagram.AxisY.ConstantLines.Clear();
                diagram.AxisX.Strips.Clear();
            }
            chartControl1.Series.Clear();
        }

        public void AxisYRangeAuto(bool bAuto)
        {
            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;
            if (diagram == null)
                return;

            diagram.AxisY.WholeRange.Auto = bAuto;
            if (!bAuto)
            {
                float UnitSize = Math.Abs((_MaxY - _MinY) / 10);
                diagram.AxisY.WholeRange.SetMinMaxValues(_MinY - UnitSize, _MaxY + UnitSize);
            }
        }


        private void DataSourceBinding(DataTable dataTable, string ColumnArgument, string ColumnValue, int LimitRecord = 10000)
        {
            if (dataTable.Rows.Count > LimitRecord)
                dataTable = GetDataTableLastRecords(dataTable, LimitRecord);

            chartControl1.Series[ColumnValue].DataSource = dataTable;
            chartControl1.Series[ColumnValue].ArgumentDataMember = dataTable.Columns[ColumnArgument].ColumnName;
            chartControl1.Series[ColumnValue].ValueDataMembers.AddRange(dataTable.Columns[ColumnValue].ColumnName);
        }

        private void SetAxisXRange(DateTimeMeasureUnit dtimeUnit, DateTimeGridAlignment dtimeGrid)
        {
            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = dtimeUnit;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = dtimeGrid;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
        }

        private void AddSerise(string name, Color seriesColor)
        {
            Series series = new Series(name, ViewType.Line);
            chartControl1.Series.Add(series);
            series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            series.ValueScaleType = ScaleType.Numerical;

            LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            lineSeriesView1.Color = seriesColor;
            lineSeriesView1.LineMarkerOptions.Color = seriesColor;
            lineSeriesView1.LineMarkerOptions.Size = 3;
            lineSeriesView1.LineStyle.Thickness = 1;
            lineSeriesView1.MarkerVisibility = DefaultBoolean.True;
            series.View = lineSeriesView1;
        }

        // https://documentation.devexpress.com/#CoreLibraries/DevExpressXtraChartsSeriesBase_ToolTipPointPatterntopic
        private void AddToolTip(string ToolTipPattern)
        {
            // Disable a crosshair cursor.
            chartControl1.CrosshairEnabled = DefaultBoolean.False;

            // Enable chart tooltips. 
            chartControl1.ToolTipEnabled = DefaultBoolean.True;

            ToolTipController controller = new ToolTipController();
            chartControl1.ToolTipController = controller;
            controller.ShowBeak = true;

            // Change the default tooltip mouse position to relative position.
            ToolTipRelativePosition relativePosition = new ToolTipRelativePosition();
            chartControl1.ToolTipOptions.ToolTipPosition = relativePosition;

            // Specify the tooltip relative position offsets.  
            relativePosition.OffsetX = 2;
            relativePosition.OffsetY = 2;

            // Specify the tooltip point pattern.
            chartControl1.Series[0].ToolTipPointPattern = ToolTipPattern;
        }

        private DataTable GetDataTableLastRecords(DataTable dataTable, int LastRecordCount)
        {
            DataTable dt = dataTable.Clone();
            int RowStartIndex = dataTable.Rows.Count - LastRecordCount;
            int RowEndIndex = dataTable.Rows.Count - 1;

            DataRow row = null;
            for (int i = RowStartIndex; i <= RowEndIndex; i++)
            {
                row = dt.NewRow();

                for (int c = 0; c < dataTable.Columns.Count; c++)
                    row[c] = dataTable.Rows[i][c];

                dt.Rows.Add(row);
            }

            return dt;
        }

        private void AddConstantLine(bool ShowLevel = true)
        {
            if (_MinY == 0 && _MaxY == 0)
                return;

            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;
            string Min;
            string Max;

            if (_DisplayType == "HEX")
            {
                Min = string.Format("Min : {0:X}", Convert.ToInt32(_MinY));
                Max = string.Format("Min : {0:X}", Convert.ToInt32(_MaxY));
            }
            else
            {
                Min = string.Format("Min : {0}", _MinY);
                Max = string.Format("MAX : {0}", _MaxY);
            }

            ConstantLine constantLine1 = new ConstantLine(Min);
            ConstantLine constantLine2 = new ConstantLine(Max);
            constantLine1.AxisValueSerializable = "8";
            constantLine1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            constantLine1.LineStyle.Thickness = 2;
            constantLine2.AxisValueSerializable = "2";
            constantLine2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            constantLine2.LineStyle.Thickness = 2;
            diagram.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
            constantLine1,
            constantLine2});

            if (ShowLevel)
            {
                ConstantLine constantLine3 = new ConstantLine("Lower Level");
                ConstantLine constantLine4 = new ConstantLine("Upper Level");
                constantLine3.AxisValueSerializable = "7";
                constantLine3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
                constantLine4.AxisValueSerializable = "3";
                constantLine4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
                diagram.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
                constantLine3,
                constantLine4});

                float UnitSize = Math.Abs((_MaxY - _MinY) / 10);
                diagram.AxisY.ConstantLines[2].AxisValue = _MinY + UnitSize * 1.5;
                diagram.AxisY.ConstantLines[3].AxisValue = _MaxY - UnitSize * 1.5;
            }
        }



        private void SetAxisYRange()
        {
            if (_MinY >= _MaxY)
                return;

            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;

            diagram.AxisY.ConstantLines[0].AxisValue = _MinY;
            diagram.AxisY.ConstantLines[1].AxisValue = _MaxY;
        }

        private void AddDayStrip(int day)
        {
            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;

            string sDay = "Yesterday";
            if (day != -1) sDay = string.Format("Today {0}", day);

            Strip strip = new Strip(sDay, DateTime.Today.AddDays(day - 1), DateTime.Today.AddDays(day));
            strip.Color = Color.LightYellow;
            diagram.AxisX.Strips.Add(strip);
        }

        private void chartControl1_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            if (e.CursorType == CursorType.Hand || e.CursorType == CursorType.Grab)
            {
                e.Cursor = Cursors.Default;
            }
        }
    }
}
