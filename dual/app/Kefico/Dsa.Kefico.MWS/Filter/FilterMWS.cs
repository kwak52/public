using System;
using System.Data;

namespace Dsa.Kefico.MWS
{
    //   DateTime.Tostring("yyyy-MM-dd");
    //d: 6/15/2008 
    //D: Sunday, June 15, 2008 
    //f: Sunday, June 15, 2008 9:15 PM
    //F: Sunday, June 15, 2008 9:15:07 PM
    //g: 6/15/2008 9:15 PM
    //G: 6/15/2008 9:15:07 PM
    //m: June 15 
    //o: 2008-06-15T21:15:07.0000000 
    //R: Sun, 15 Jun 2008 21:15:07 GMT
    //s: 2008-06-15T21:15:07 
    //t: 9:15 PM
    //T: 9:15:07 PM
    //u: 2008-06-15 21:15:07Z
    //U: Monday, June 16, 2008 4:15:07 AM
    //y: June, 2008 

    public class FilterMWS
    {
        private bool _IsDurtySummary = false;
        private bool _IsDurtyStep = false;
        private bool _IsDurtyMeasure = false;
        private DateTime _dtStart = DateTime.Now;
        private DateTime _dtEnd = DateTime.Now;
        private string _lineGroup;
        private string _batchName;
        private bool _IsDanamic = false;
        private int _SummaryID = -1;
        private int _PositionID = -1;
        private int _MeasureID = -1;
        private int _CcsID = -1;
        private int _PdvID = -1;
        
        public FilterMWS()
        {
        }
        public bool IsDurtySummary { get { return _IsDurtySummary; } }
        public bool IsDurtyStep { get { return _IsDurtyStep; } }
        public bool IsDurtyMeasure { get { return _IsDurtyMeasure; } }
        public DateTime StartTime { get { return _dtStart; } }
        public DateTime EndTime { get { return _dtEnd; } }
        public string LineGroup { get { return _lineGroup; } }
        public bool IsDanamic { get { return _IsDanamic; } }
        public int SummaryID { get { return _SummaryID; } }
        public int PositionID { get { return _PositionID; } }
        public int MeasureID { get { return _MeasureID; } }

        public void SetFilterSummary(DateTime TimeSummaryStart, DateTime TimeSummaryEnd, string LineGroup)
        {
            _dtStart = TimeSummaryStart;
            _dtEnd = TimeSummaryEnd;
            _lineGroup = LineGroup;

            _IsDurtySummary = true;
        }

        public void SetFilterProduct(DataRow dr)
        {
            _SummaryID = Convert.ToInt32(dr[SchemaMWS.TSV_id]);
            _CcsID = Convert.ToInt32(dr[SchemaMWS.TSV_ccsId]);
            _PdvID = Convert.ToInt32(dr[SchemaMWS.TSV_pdvId]);
            _IsDanamic = Convert.ToBoolean(dr[SchemaMWS.TSV_isFromDynamic]);
            _batchName = dr[SchemaMWS.TSV_batchName].ToString();
        }
        public void SetFilterStep(DataRow dr)
        {
            _PositionID = Convert.ToInt32(dr[SchemaMWS.SSV_positionId]);
        }
        public void SetFilterTotalReport(DataRow dr)
        {
            _PositionID = Convert.ToInt32(dr[SchemaMWS.BV_positionId]);
        }
        public void SetFilterSpcTable(DataRow dr)
        {
            _MeasureID = Convert.ToInt32(dr[SchemaMWS.PSV_measureid]);
        }
        public void SetFilterSelectTest(DataRow dr)
        {
            _MeasureID = Convert.ToInt32(dr[SchemaMWS.PSV_id]);
        }

        public string GetQueryMeasure()
        {
            string Query1 = string.Format("{0} >= '{1}' AND {0} <= '{2}'", SchemaMWS.MV_startDay, _dtStart.ToString("yyyy-MM-dd"), _dtEnd.ToString("yyyy-MM-dd"));
            string Query2 = string.Format("{0} = '{1}'", SchemaMWS.MV_batchName, _batchName);

            return Query1 + " and " + Query2;
        }

        public string GetSummary()
        {
            _IsDurtySummary = false;
            string dayStart = string.Format("{0}", _dtStart.ToString("yyyy-MM-dd"));
            string dayEnd = string.Format("{0}", _dtEnd.ToString("yyyy-MM-dd"));
            string QuickView = string.Format("{0}", Convert.ToInt32(false));

            return string.Format("'{0}','{1}','{2}'", dayStart, dayEnd, QuickView);
        }

        public string GetDay()
        {
            _IsDurtySummary = false;
            string dayStart = string.Format("{0}", _dtStart.ToString("yyyy-MM-dd"));
            string dayEnd = string.Format("{0}", _dtEnd.ToString("yyyy-MM-dd"));

            return string.Format("'{0}','{1}'", dayStart, dayEnd);
        }
   
        public string GetPSV()
        {
            string summaryID = string.Format("{0}", _SummaryID);
            string danamic = string.Format("{0}", Convert.ToInt32(_IsDanamic));
            string positionID = string.Format("{0}", Convert.ToInt32(_PositionID));

            return string.Format("'{0}','{1}','{2}'", summaryID, danamic, positionID);
        }

        public string GetTSV()
        {
            string summaryID = string.Format("{0}", _SummaryID);
            string danamic = string.Format("{0}", Convert.ToInt32(_IsDanamic));

            return string.Format("'{0}','{1}'", summaryID, danamic);
        }

        public string GetMeasureInfo()
        {
            string summaryID = string.Format("{0}", _SummaryID);
            string danamic = string.Format("{0}", Convert.ToInt32(_IsDanamic));
            string positionID = string.Format("{0}", _PositionID);

            return string.Format("'{0}','{1}','{2}'", summaryID, danamic, positionID);
        }
        public string GetRt()
        {
            string summaryID = string.Format("{0}", _SummaryID);
            string danamic = string.Format("{0}", Convert.ToInt32(_IsDanamic));
            string positionID = string.Format("{0}", _PositionID);

            return string.Format("'{0}','{1}','{2}'", summaryID, danamic, positionID);
        }


        public string GetBundle()
        {
            string BundleID = string.Format("{0}", Convert.ToInt32(_MeasureID));

            return string.Format("'{0}'", BundleID);
        }
        public string GetTSVExtend()
        {
            string dayStart = string.Format("{0}", _dtStart.ToString("yyyy-MM-dd"));
            string dayEnd = string.Format("{0}", _dtEnd.ToString("yyyy-MM-dd"));
            string ccsID = string.Format("{0}", _CcsID);
            string pdvID = string.Format("{0}", _PdvID);
            //string batchName = string.Format("{0}", _batchName);

            return string.Format("'{0}','{1}','{2}','{3}',{4}", dayStart, dayEnd, ccsID, pdvID, "null");
        }
    }
}