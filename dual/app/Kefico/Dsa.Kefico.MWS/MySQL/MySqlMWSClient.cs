using Dsa.Kefico.MWS.EventHandler;
using Dsu.DB.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Configuration;

namespace Dsa.Kefico.MWS
{
    public class MySqlMWSClient
    {
        MySqlConnection _conn;
        private List<string> _lstTestTable = new List<string>();

        public UEventHandlerSqlReadRecordPosition UEventSqlReadRecordPosition;
        public UEventHandlerSqlReadRecordCount UEventSqlReadRecordCount;
        public UEventHandlerSqlException UEventSqlException;

        public MySqlMWSClient()
        {
            string dbServerIp = ConfigurationManager.AppSettings["dbServer"];
            string connStr = string.Format("server={0};user={1};database={2};password={3};port={4};Allow User Variables=True"
                , dbServerIp
                , "mws"
                , "kefico"
                , "mws"
                , "3306"
                );

            _conn = new MySqlConnection(connStr);
        }

        public List<string> lstTestTable
        { get { return _lstTestTable; } }
  
        public bool Open()
        {
            try
            {
                _conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                UEventSqlException?.Invoke(ex);

                return false;
            }
        }

        public DataTable GetDataFromDBView(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = _conn.ExecuteReaderIntoDataTable(sql);
            }
            catch (MySqlException err)
            {
                UEventSqlException?.Invoke(err);
            }

            UEventSqlReadRecordPosition?.Invoke(this, 0);

            return dt;
        }


        public int GetRecordCount(string TableName)
        {
            int RecordCount = -1;
            string sql = string.Format("SELECT COUNT(*)  FROM {0}", TableName);

            try
            {
                var count = _conn.ExecuteScalar(sql);
                RecordCount = Convert.ToInt32(count);
            }
            catch (MySqlException err)
            {
                UEventSqlException?.Invoke(err);
                Console.WriteLine("Error: " + err.ToString());
            }

            UEventSqlReadRecordCount?.Invoke(this, RecordCount);

            return RecordCount;
        }


        public DataTable GetDataFromSample()
        {
            int RecordCount = 10000;

            DataTable dt = new DataTable();

            dt.Columns.Add("Argument", typeof(DateTime));
            dt.Columns.Add("Value", typeof(float));

            DateTime dtime = DateTime.Now;
            Random random = new Random();

            for (int n = 0; n < RecordCount; n++)
            {
                dtime = dtime.AddSeconds(-100);
                if (n % 70 == 0)
                    dt.Rows.Add(dtime, random.NextDouble() * 10 - 5);
                else
                    dt.Rows.Add(dtime, random.Next(0, 10) / 10.0 - 1);
            }

            dt.Rows.Add(dtime.AddSeconds(-100), 100000);

            return dt;
        }
    }
}
