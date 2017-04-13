using System;
using System.Configuration;
using System.Data;

namespace Dsa.Kefico.PDV
{
    public class FilterPDV
    {
        public FilterPDV()
        {
        }

        private string Path { get; set; }
        private string LineGroup { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        public void SetFilter(DateTime TimeSummaryStart, DateTime TimeSummaryEnd, string lineGroup)
        {
            StartTime = TimeSummaryStart;
            EndTime = TimeSummaryEnd;
            LineGroup = lineGroup;
        }

        public string GetDayWhere()
        {
            string dayStart = string.Format("{0}", StartTime.ToString("yyyy-MM-dd") + " 00:00:00");
            string dayEnd = string.Format("{0}", EndTime.ToString("yyyy-MM-dd") + " 23:59:59");

            return string.Format("{0} >= '{1}' and {2} <= '{3}'", SchemaPDV.PDV_createdDt, dayStart, SchemaPDV.PDV_createdDt, dayEnd);
        }
    }
}