using System;
using System.Data;
using DevExpress.XtraEditors;
using Dsu.UI.XbarChart;
using System.Collections.Generic;

namespace Dsa.Hmc.SensorDB
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public partial class ItemDetailPage : XtraUserControl
    {
        public ucChartXbar chart { get { return ucChartXbar1; } }

        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public void CreateChart(string name, DataTable tableLog, TagDataSource tagDataSource)
        {
            chart.ClearChart();

            if (name == tagDataSource.GroupA || name == tagDataSource.GroupB)
            {
                List<string> Columns = new List<string>();
                foreach (TagData t in tagDataSource.Items)
                {
                    if (t.Group == name && !Columns.Contains(t.LogName))
                        Columns.Add(t.LogName);
                }
                chart.ShowChart(tableLog, "시간", Columns, true, "");
                chart.TitleMain = name;
                chart.TitleSub = "";
            }
            else
            {
                List<string> Columns = new List<string>();
                Columns.Add(tagDataSource.PositionA + " " + name);
                Columns.Add(tagDataSource.PositionB + " " + name);

                DataTable dt = CreateData(tableLog, Columns, name, tagDataSource.SelectCarType.CarType, tagDataSource);
                chart.ShowChart(dt, "시간", Columns, true, "");
                chart.TitleMain = name;
                chart.TitleSub = "";
            }
        }


        private DataTable CreateData(DataTable dtOrg, List<string> Columns, string sensor, string carType, TagDataSource tagDataSource)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("시간", typeof(DateTime));
            foreach (string s in Columns)
                dt.Columns.Add(s, typeof(double));

            List<object> log = new List<object>();
            string key = tagDataSource.PositionB;

            DataRow[] SortedRow = dtOrg.Select(string.Format("차종 = '{0}'", carType, "시간 DESC"));

            foreach (DataRow dr in SortedRow)
            {
                if (tagDataSource.PositionA != dr["구분"].ToString())
                {
                    log = new List<object>();
                    log.Add(dr["시간"]);
                    log.Add(dr[sensor]);
                }
                else if (tagDataSource.PositionB != dr["구분"].ToString())
                {
                    if (log.Count == 2)
                    {
                        log.Add(dr[sensor]);
                        dt.Rows.Add(log.ToArray());
                    }
                }
            }
            return dt;
        }
    }
}
