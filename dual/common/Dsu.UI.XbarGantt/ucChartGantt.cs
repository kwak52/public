using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using DevExpress.XtraCharts;

namespace Dsu.UI.XbarGantt
{
    public partial class ucChartGantt : UserControl
    {
        public List<BarPoint> _lstPoint { get; set; }
        private string fieldTag = "memoryId";
        private string fieldStartTime = "timeStampS";
        private string fieldEndTime = "timeStampE";
        private Series series = new Series("POINT", ViewType.Gantt);
        private int CurrentRow = 0;

        public ucChartGantt()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {
            this.chartControl1.CustomDrawSeriesPoint += ChartControl1_CustomDrawSeriesPoint;
            this.chartControl1.Padding.Top = 30;
        }

        private void ChartControl1_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            BarPoint bp = e.SeriesPoint.Tag as BarPoint;

            switch (bp.Type)
            {
                case "I":
                    e.SeriesDrawOptions.Color = Color.DodgerBlue;
                    break;
                case "Q":
                    e.SeriesDrawOptions.Color = Color.IndianRed;
                    break;
                case "M":
                    e.SeriesDrawOptions.Color = Color.LimeGreen;
                    break;
                default: break;
            }
        }

        public void BoundDataChanged(List<long> ShowMemory)
        {
            chartControl1.Series[0].SeriesPointsSorting = SortingMode.None;

            AssignOrder(ShowMemory);
        }

        public bool ShowChart(DataTable dtInput, DataTable dtLog)
        {
            //GenerateDataSourceCP(dtInput, dtLog);
            //GenerateDataSourceTest();
            _lstPoint = new List<BarPoint>();

            AddSerise();

            AddConstantLine();

            DataSourceBinding();

            ApplyChartOption();

            AddToolTip();

            AddRelation();

            return true;
        }

        private void GenerateDataSourceTest()
        {
            DateTime dt = DateTime.Now;
            Random r = new Random();

            IEnumerable<string> Symbols = GetSymbolName();

            foreach (string s in Symbols)
            {
                int sortId1 = r.Next(1, 50);
                int sortId2 = r.Next(1, 50);
                int sortId3 = r.Next(1, 50);
                dt = dt.AddMilliseconds(sortId1 * 999);

                //    _lstPoint.Add(new BarPoint(_lstPoint.Count, string.Format("{0}{1:000} {2}", "M", sortId1, s), "M", sortId1, dt.AddMilliseconds(sortId1 * 321), dt.AddMilliseconds(sortId1 * 1521)));
                _lstPoint.Add(new BarPoint(_lstPoint.Count, string.Format("{0}{1:000} {2}", "X", sortId2, s), "I", sortId2, dt.AddMilliseconds(sortId2 * 235), dt.AddMilliseconds(sortId2 * 1521)));
                _lstPoint.Add(new BarPoint(_lstPoint.Count, string.Format("{0}{1:000} {2}", "Y", sortId3, s), "Q", sortId3, dt.AddMilliseconds(sortId3 * 234), dt.AddMilliseconds(sortId3 * 1521)));
            }

        }

        
        private IEnumerable<string> GetSymbolName()
        {
            yield return "#202_MD213차    슬라이드전진단";
            yield return "#202_MD213차    슬라이드후진단";
            yield return "#202_MD214차 핀 254,256 전진단";
            yield return "#202_MD214차 핀 254,256 후진단";
            yield return "#202_MD215차U263잠김단";
            yield return "#202_MD215차U263풀림단";
            yield return "#202_MD216차U265잠김단";
            yield return "#202_MD216차U265풀림단";
            yield return "#202_MD217차 INR핀 래치 전진단";
            yield return "#202_MD217차 INR핀 래치 후진단";
            yield return "#202_MD218차    INR핀   전진단";
            yield return "#202_MD218차    INR핀   후진단";
            yield return "#202_MD23,4,5차 유닛 ALL잠김단";
            yield return "#202_MD23,4,5차 유닛 ALL풀림단";
            yield return "#202대차록크    잠김SET";
            yield return "#202대차록크    풀림SET";
            yield return "#202    급전    전진SET";
            yield return "#202    급전    후진SET";
            yield return "#202    급기    전진SET";
            yield return "#202    급기    후진SET";
            yield return "#202    셔틀    후크핀  상승SET";
            yield return "#202    셔틀    후크핀  하강SET";
            yield return "#202 작업완료 SET";

        }

     
        private void AddRelation()
        {
            //    chartControl1.Series[0].Points[0].Relations.AddRange(new DevExpress.XtraCharts.Relation[] { new DevExpress.XtraCharts.TaskLink(1) });
        }

        private void GenerateDataSourceCP(DataTable dtInput, DataTable dtLog)
        {
            CurrentRow = dtInput.Rows.Count;
            _lstPoint.Clear();
            foreach (DataRow row in dtInput.Rows)
            {
                DataRow[] SortedRow = dtLog.Select(string.Format("Address = '{0}'", row["Address"]));

                if (SortedRow.Length == 1)
                {
                    BarPoint point = CreatePoint(row, SortedRow, 0, 100);
                    _lstPoint.Add(point);
                }
                else
                {
                    for (int i = 0; i < SortedRow.Length - 1; i++)
                    {
                        if ((int)SortedRow[0]["Value"] == 0
                            && SortedRow[0] == SortedRow[i]
                            && SortedRow[0]["Address"].ToString().StartsWith("M"))
                            continue;

                        BarPoint point = CreatePoint(row, SortedRow, i);
                        _lstPoint.Add(point);
                        i++;
                    }
                }
            }
        }

        private BarPoint CreatePoint(DataRow row, DataRow[] SortedRow, int i, int AddMilliseconds = 0)
        {
            BarPoint point;
            if (SortedRow.Length == 1)
            {
                point = new BarPoint(_lstPoint.Count
               , row["Symbol"].ToString()
               , row["Type"].ToString()
               , (Int64)row[fieldTag]
               , (DateTime)SortedRow[0]["Time"]
               , ((DateTime)SortedRow[0]["Time"]).AddMilliseconds(AddMilliseconds)
               , string.Format("{0}-{1}", SortedRow[i]["Value"], SortedRow[0]["Value"]));

            }
            else
            {
                point = new BarPoint(_lstPoint.Count
                    , row["Symbol"].ToString()
                    , row["Type"].ToString()
                    , (Int64)row[fieldTag]
                    , (DateTime)SortedRow[i]["Time"]
                    , (DateTime)SortedRow[i + 1]["Time"]
                    , string.Format("{0}-{1}", SortedRow[i]["Value"], SortedRow[i + 1]["Value"]));
            }
            return point;
        }

        public void UpdateChart()
        {
            chartControl1.Update();
        }

        private void GenerateDataSource(DataTable dtInput)
        {
            foreach (DataRow row in dtInput.Rows)
            {
                if (_lstPoint.Count % 3 == 0)
                    _lstPoint.Add(new BarPoint(_lstPoint.Count, "AHN" + row[fieldTag].ToString(), "I", _lstPoint.Count, (DateTime)row[fieldStartTime], (DateTime)row[fieldEndTime]));
                else if (_lstPoint.Count % 3 == 1)
                    _lstPoint.Add(new BarPoint(_lstPoint.Count, "AHN" + row[fieldTag].ToString(), "Q", Convert.ToInt64(row[fieldTag]), (DateTime)row[fieldStartTime], (DateTime)row[fieldEndTime]));
                else
                    _lstPoint.Add(new BarPoint(_lstPoint.Count, "AHN" + row[fieldTag].ToString(), "M", Convert.ToInt64(row[fieldTag]), (DateTime)row[fieldStartTime], (DateTime)row[fieldEndTime]));
            }
        }

        private void ApplyChartOption()
        {
            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;
            //diagram.AxisY.Alignment = AxisAlignment.Far;
            diagram.AxisX.Interlaced = true;
            diagram.AxisX.Alignment = AxisAlignment.Far;
            //diagram.AxisY.Visibility = DefaultBoolean.False;

            diagram.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            diagram.AxisY.Label.ResolveOverlappingOptions.AllowStagger = false;

            diagram.EnableAxisYScrolling = true;
            diagram.EnableAxisYZooming = true;
            diagram.EnableAxisXScrolling = true;
            //diagram.EnableAxisXZooming = true;
            diagram.ZoomingOptions.AxisYMaxZoomPercent = 10000;
            diagram.AxisY.Label.TextPattern = "{V:yy-M-d H\'H\'}";
            diagram.AxisY.VisualRange.Auto = false;
            //             diagram.AxisY.WholeRange.Auto = false;
            //             diagram.AxisY.WholeRange.SetMinMaxValues(0, 100000);
            //    diagram.AxisY.VisualRange.SetMinMaxValues(DateTime.Now, DateTime.Now.AddMinutes(1));
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


        private void DataSourceBinding()
        {
            chartControl1.Series[0].ArgumentDataMember = "Name";
            chartControl1.Series[0].ValueDataMembers.AddRange(new string[] { "StartTime", "EndTime"});
            chartControl1.Series[0].DataSource = _lstPoint;

            DataFilter df = new DataFilter("SortId", "System.Int64", DevExpress.XtraCharts.DataFilterCondition.GreaterThanOrEqual, 0xFFFFFFFF);
            chartControl1.Series[0].DataFiltersConjunctionMode = ConjunctionTypes.Or;
            chartControl1.Series[0].DataFilters.Clear();
            chartControl1.Series[0].DataFilters.Add(df);
        }

        private void AddSerise()
        {
            series = new Series("POINT", ViewType.Gantt);

            series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            series.ValueScaleType = DevExpress.XtraCharts.ScaleType.DateTime;

            DevExpress.XtraCharts.OverlappedGanttSeriesView overlappedGanttSeriesView1 = new DevExpress.XtraCharts.OverlappedGanttSeriesView();
            //overlappedGanttSeriesView1.BarWidth = 0.7;
            overlappedGanttSeriesView1.Color = Color.GhostWhite;

            series.View = overlappedGanttSeriesView1;

            chartControl1.Series.Add(series);
        }

        private void AddToolTip()
        {
            chartControl1.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.Fuchsia;
            chartControl1.CrosshairOptions.ValueLineColor = System.Drawing.Color.DarkBlue;
            chartControl1.CrosshairOptions.ShowArgumentLine = true;
            chartControl1.CrosshairOptions.ShowArgumentLabels = true;
            chartControl1.CrosshairOptions.ShowValueLabels = true;
            chartControl1.CrosshairOptions.ShowValueLine = true;

            chartControl1.CustomDrawCrosshair += ChartControl1_CustomDrawCrosshair;
        }

        private void ChartControl1_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            foreach (var group in e.CrosshairElementGroups)
            {
                foreach (CrosshairElement element in group.CrosshairElements)
                {
                    chartControl1.Series[0].CrosshairLabelPattern = "{A}\r\n{V1:H:mm:ss.fff}\r\n{V2:H:mm:ss.fff}\r\n{VD}\r\n" + ((BarPoint)element.SeriesPoint.Tag).Value;
                }
            }
        }

        private void AddConstantLine()
        {
            GanttDiagram diagram = (GanttDiagram)chartControl1.Diagram;
            ConstantLine constantLine1 = new DevExpress.XtraCharts.ConstantLine("Day-1");

            constantLine1.AxisValueSerializable = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            constantLine1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            constantLine1.LineStyle.Thickness = 2;
            constantLine1.Name = "Day-1";
            diagram.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
            constantLine1});

        }

        private void AssignOrder(List<long> lstShow)
        {
            //SortId  < FFFF 는 filter 대상으로 보이지 않음
            long id = 0;
            foreach (BarPoint bp in _lstPoint)
            {
                id = bp.SortId % 0xFFFFFFFF;

                if (!lstShow.Contains(id))
                    bp.SortId = id; //bar hide
                else
                    bp.SortId = (lstShow.IndexOf(id) + 1) * 0xFFFFFFFF + id + 0xFFFEFFFF0001; //bar show
            }

            _lstPoint.Sort(delegate (BarPoint a, BarPoint b)
            {
                if (a.SortId == b.SortId)
                    return a.Id.CompareTo(b.Id);
                else
                    return a.SortId.CompareTo(b.SortId);
            });

            chartControl1.RefreshData();
        }
        private void chartControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChartHitInfo hi = chartControl1.CalcHitInfo(e.Location);

            if (hi.InSeries)
            {
                if (ModifierKeys == Keys.Control)
                {
                    List<SeriesPoint> lstSp = FindPointPre(hi.SeriesPoint);
                    hi.SeriesPoint.Relations.Clear();
                    foreach (SeriesPoint sp in lstSp)
                    {
                        hi.SeriesPoint.Relations.Add(sp, TaskLinkType.FinishToStart);
                    }
                }
                else
                {
                    List<SeriesPoint> lstSp = FindPointNext(hi.SeriesPoint);
                    foreach (SeriesPoint sp in lstSp)
                    {
                        sp.Relations.Clear();
                        sp.Relations.Add(hi.SeriesPoint, TaskLinkType.FinishToStart);
                    }
                }
            }
            else if (hi.InAxis)//Ascending Sorting
            {
                chartControl1.Series[0].SeriesPointsSorting = SortingMode.Ascending;
                chartControl1.Series[0].SeriesPointsSortingKey = SeriesPointKey.Value_1;
            }
        }

        private List<SeriesPoint> FindPointPre(SeriesPoint spLink)
        {
            List<SeriesPoint> lstFind = new List<SeriesPoint>();
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = ((BarPoint)spLink.Tag).StartTime;

            foreach (SeriesPoint ap in chartControl1.Series[0].Points)
            {
                if (((BarPoint)ap.Tag).EndTime > dtStart && ((BarPoint)ap.Tag).EndTime < dtEnd)
                    dtStart = ((BarPoint)ap.Tag).EndTime;
            }

            foreach (SeriesPoint ap in chartControl1.Series[0].Points)
            {
                if (((BarPoint)ap.Tag).EndTime == dtStart)
                    lstFind.Add(ap);
            }

            lstFind.Remove(spLink);

            return lstFind;
        }

        private List<SeriesPoint> FindPointNext(SeriesPoint spLink)
        {
            List<SeriesPoint> lstFind = new List<SeriesPoint>();
            DateTime dtStart = ((BarPoint)spLink.Tag).EndTime;
            DateTime dtEnd = DateTime.MaxValue;

            foreach (SeriesPoint ap in chartControl1.Series[0].Points)
            {
                if (((BarPoint)ap.Tag).StartTime > dtStart && ((BarPoint)ap.Tag).StartTime < dtEnd)
                    dtEnd = ((BarPoint)ap.Tag).StartTime;
            }

            foreach (SeriesPoint ap in chartControl1.Series[0].Points)
            {
                if (((BarPoint)ap.Tag).StartTime == dtEnd)
                    lstFind.Add(ap);

            }

            lstFind.Remove(spLink);

            return lstFind;
        }

    }
}
