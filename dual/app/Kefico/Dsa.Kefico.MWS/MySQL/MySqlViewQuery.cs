using System;
using Dsa.Kefico.MWS.Enumeration;

namespace Dsa.Kefico.MWS
{
    public class ViewQuery
    {
    
        public ViewQuery()
        {
        }

        public string Query(FilterMWS filterMWS,  EnumTable Views)
        {
            string viewQuery = string.Empty; ;

            if (Views == EnumTable.showTotalSummaryView)
                viewQuery = ViewShowTotalSummaryView(filterMWS);
            else if (Views == EnumTable.showSPSV)
                viewQuery = ViewShowSPSV(filterMWS);
            else if (Views == EnumTable.showPHV || Views == EnumTable.showPHVChart)
                viewQuery = ViewShowPHV(filterMWS);
            else if (Views == EnumTable.showSTSV)
                viewQuery = ViewShowSTSV(filterMWS);
            else if (Views == EnumTable.showBundle)
                viewQuery = ViewShowBundle(filterMWS);
            else if (Views == EnumTable.showRT)
                viewQuery = ViewRT(filterMWS);
            else if (Views == EnumTable.showTCT)
                viewQuery = ViewTCT(filterMWS);
            else if (Views == EnumTable.showSPEV)
                viewQuery = ViewSPEV(filterMWS);
            else if (Views == EnumTable.showSPEDV)
                viewQuery = ViewSPEDV(filterMWS);
            else if (Views == EnumTable.showSOV)
                viewQuery = ViewSOV(filterMWS);


            return viewQuery;
        }

        private string ViewSOV(FilterMWS filterMWS)
        {
            string Function = "CALL showSOV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetDay());

            return Procedure;
        }

        private string ViewSPEDV(FilterMWS filterMWS)
        {
            string Function = "CALL showSPEDV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }

        private string ViewSPEV(FilterMWS filterMWS)
        {
            string Function = "CALL showSPEV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }

        private string ViewSPSV(FilterMWS filterMWS)
        {
            string Function = "CALL showSPSV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }

        private string ViewTCT(FilterMWS filterMWS)
        {
            string Function = "CALL showTCT";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }

        private string ViewSTSV(FilterMWS filterMWS)
        {
            string Function = "CALL showSTSV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }

        private string ViewRT(FilterMWS filterMWS)
        {
            string Function = "CALL showRT";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }

        public string QueryInfo(FilterMWS filterMWS, EnumTable Views)
        {
            string viewQuery = string.Empty; ;

            if (Views == EnumTable.showPHVChart)
                viewQuery = InfoTableSummary(filterMWS);
           
            return viewQuery;
        }

        private string InfoTableSummary(FilterMWS filterMWS)
        {
            string Function = "CALL showMeasureInfo";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetMeasureInfo());

            return Procedure;
        }

        private string ViewShowTotalSummaryView(FilterMWS filterMWS)
        {
            string Function = "CALL showTotalSummaryView";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetSummary());

            return Procedure;
        }
        private string ViewShowSPSV(FilterMWS filterMWS)
        {
            string Function = "CALL showSPSV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }
        private string ViewShowPHV(FilterMWS filterMWS)
        {
            string Function = "CALL showPHV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetPSV());

            return Procedure;
        }

        private string ViewShowSTSV(FilterMWS filterMWS)
        {
            string Function = "CALL showSTSV";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetTSV());

            return Procedure;
        }
       
        private string ViewShowBundle(FilterMWS filterMWS)
        {
            string Function = "CALL showBundle";
            string Procedure = string.Format("{0}({1})", Function, filterMWS.GetBundle());

            return Procedure;
        }
       
    }
}
