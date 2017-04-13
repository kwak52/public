using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dsa.Kefico.MWS.Enumeration
{
    public enum EnumTable
    {
        showTotalSummaryView,
        showSPSV,
        showPHV,
        showSTSV,
        showBundle,
        showPHVChart,
        showRT,
        showTCT,
        showSPEV, //showSPSV error 기준으로 정렬하여 사용 대체
        showSPEDV,
        showSOV
    }

    public static class ViewMWS
    {
        private const string showTotalSummaryView = "Overview";
        private const string showSPSV = "Measure List";
        private const string showPHV = "Statistical Process Control ";
        private const string showSTSV = "Select a Test";
        private const string showBundle = "Total Report";
        private const string showPHVChart = "SPC Chart";
        private const string showRT = "Re-test";
        private const string showTCT = "Test Cycle time";
        private const string showSPEV = "Overview Error";
        private const string showSPEDV = "Overview Error Detail";
        private const string showSOV = "Overview Good/NG";

        public static string GetName(EnumTable enumViewTables)
        {
            string name = string.Empty; ;
            if (enumViewTables == EnumTable.showTotalSummaryView)
                name = ViewMWS.showTotalSummaryView;
            else if (enumViewTables == EnumTable.showSPSV)
                name = ViewMWS.showSPSV;
            else if (enumViewTables == EnumTable.showPHV)
                name = ViewMWS.showPHV;
            else if (enumViewTables == EnumTable.showSTSV)
                name = ViewMWS.showSTSV;
            else if (enumViewTables == EnumTable.showBundle)
                name = ViewMWS.showBundle;
            else if (enumViewTables == EnumTable.showPHVChart)
                name = ViewMWS.showPHVChart;
            else if (enumViewTables == EnumTable.showRT)
                name = ViewMWS.showRT;
            else if (enumViewTables == EnumTable.showTCT)
                name = ViewMWS.showTCT;
            else if (enumViewTables == EnumTable.showSPEV)
                name = ViewMWS.showSPEV;
            else if (enumViewTables == EnumTable.showSPEDV)
                name = ViewMWS.showSPEDV;
            else if (enumViewTables == EnumTable.showSOV)
                name = ViewMWS.showSOV;

            return name;
        }

        public static EnumTable GetType(string name)
        {
            EnumTable enumViewTables = EnumTable.showTotalSummaryView;
            if (name == ViewMWS.showTotalSummaryView)
                enumViewTables = EnumTable.showTotalSummaryView;
            else if (name == ViewMWS.showSPSV)
                enumViewTables = EnumTable.showSPSV;
            else if (name == ViewMWS.showPHV)
                enumViewTables = EnumTable.showPHV;
            else if (name == ViewMWS.showSTSV)
                enumViewTables = EnumTable.showSTSV;
            else if (name == ViewMWS.showBundle)
                enumViewTables = EnumTable.showBundle;
            else if (name == ViewMWS.showPHVChart)
                enumViewTables = EnumTable.showPHVChart;
            else if (name == ViewMWS.showRT)
                enumViewTables = EnumTable.showRT;
            else if (name == ViewMWS.showTCT)
                enumViewTables = EnumTable.showTCT;
            else if (name == ViewMWS.showSPEV)
                enumViewTables = EnumTable.showSPEV;
            else if (name == ViewMWS.showSPEDV)
                enumViewTables = EnumTable.showSPEDV;
            else if (name == ViewMWS.showSOV)
                enumViewTables = EnumTable.showSOV;

            return enumViewTables;
        }
    }
}
