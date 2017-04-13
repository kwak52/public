using System;
using System.Data;

namespace Dsa.Kefico.MWS
{
    public class ChartSPC
    {
        private DataTable _Datasource = new DataTable();
        private string _Title;
        private int _MeasureID = 0;
        private float _MinY = -10;
        private float _MaxY = 10;
        private float _UpperY = -10;
        private float _LowY = 10;
        private string _FieldArgement;
        private string _FieldValue;
        private string _DisplayType;

        

        public ChartSPC()
        {
        }
    
        public DataTable Source { get { return _Datasource; } }
        public string Title { get { return _Title; } }
        public int MeasureID { get { return _MeasureID; } }
        public float MinY { get { return _MinY; } set {  _MinY = value; } }
        public float MaxY { get { return _MaxY; } set { _MaxY = value; } }
        public float UpperY { get { return _UpperY; } }
        public float LowY { get { return _LowY; } }
        public string Argement { get { return _FieldArgement; } }
        public string Value { get { return _FieldValue; } }
        public string DisplayType { get { return _DisplayType; } }

        public void SetDatasource(DataTable dt, DataTable dtInfo)
        {
            if (dt.Columns.Count == 0)
                return;

            _Datasource = dt;

            _Title = GetTilte(dtInfo);
            _FieldArgement = SchemaMWS.PSV_startDateTime;
            _FieldValue = SchemaMWS.PSV_value;
            _MinY = Convert.ToSingle(dtInfo.Rows[0][SchemaMWS.SPC_min]);
            _MaxY = Convert.ToSingle(dtInfo.Rows[0][SchemaMWS.SPC_max]);
            _DisplayType = dtInfo.Rows[0][SchemaMWS.BV_displayTypeSpc].ToString();
            if (!dt.Columns.Contains(_FieldArgement))
                dt.Columns.Add(_FieldArgement, typeof(DateTime));
            foreach (DataRow dr in dt.Rows)
            {
                DateTime dTime = (DateTime)dr[SchemaMWS.PSV_startDay];
                dr[_FieldArgement] = dTime.Add((TimeSpan)dr[SchemaMWS.PSV_startTime]);
            }
        }

        private string GetTilte(DataTable dtInfo)
        {
            if (dtInfo.Rows.Count == 0)
                return string.Empty;


            string Title = string.Format("{0}: {1} ", "START", dtInfo.Rows[0][SchemaMWS.SPC_startDateTime]);
            Title += string.Format("{0}: {1} ", SchemaMWS.SPC_host, dtInfo.Rows[0][SchemaMWS.SPC_host]);
            Title += string.Format("{0}: {1} ", SchemaMWS.SPC_partNumber, dtInfo.Rows[0][SchemaMWS.SPC_partNumber]);
            Title += string.Format("{0}: {1} ", SchemaMWS.SPC_position, dtInfo.Rows[0][SchemaMWS.SPC_position]);
            Title += string.Format("{0}: {1} ", SchemaMWS.SPC_step, dtInfo.Rows[0][SchemaMWS.SPC_step]);
            Title += string.Format("\r\n{0}: {1} ", "MODULE", dtInfo.Rows[0][SchemaMWS.SPC_modName]);
            if (_DisplayType == "H")
            {
                Title += string.Format("{0}: {1:X} ", SchemaMWS.SPC_min, Convert.ToInt32(dtInfo.Rows[0][SchemaMWS.SPC_min]));
                Title += string.Format("{0}: {1:X} ", SchemaMWS.SPC_max, Convert.ToInt32(dtInfo.Rows[0][SchemaMWS.SPC_max]));
            }
            else if (_DisplayType == "I")
            {
                Title += string.Format("{0}: {1} ", SchemaMWS.SPC_min, Convert.ToInt32(dtInfo.Rows[0][SchemaMWS.SPC_min]));
                Title += string.Format("{0}: {1} ", SchemaMWS.SPC_max, Convert.ToInt32(dtInfo.Rows[0][SchemaMWS.SPC_max]));
            }
            else
            {
                Title += string.Format("{0}: {1:0.00} ", SchemaMWS.SPC_min, dtInfo.Rows[0][SchemaMWS.SPC_min]);
                Title += string.Format("{0}: {1:0.00} ", SchemaMWS.SPC_max, dtInfo.Rows[0][SchemaMWS.SPC_max]);
            }
            Title += string.Format("{0}: {1} ", SchemaMWS.SPC_dim, dtInfo.Rows[0][SchemaMWS.SPC_dim]);

            return Title;
        }

  
    }
}