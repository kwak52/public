using Dsa.Kefico.PDV.EventHandler;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dsu.DB.MySQL;
using System.Configuration;

namespace Dsa.Kefico.PDV
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
                , "pdv"
                , "kefico"
                , "pdv"
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

        public DataTable GetDataFromDBView(string sql, string viewTables)
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

        public bool Execute(string sql)
        {
            try
            {
                _conn.ExecuteNonQuery(sql);
                return true;
            }
            catch (MySqlException err)
            {
                UEventSqlException?.Invoke(err);
                return false;
            }
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

        public int GetLastID(string TableName, string columnID)
        {
            int LastID = -1;
            string sql = string.Format("SELECT max({0}) FROM {1}", columnID, TableName);

            try
            {
                var max = _conn.ExecuteScalar(sql);
                LastID = Convert.ToInt32(max);
            }
            catch (MySqlException err)
            {
                UEventSqlException?.Invoke(err);
                Console.WriteLine("Error: " + err.ToString());
            }

            return LastID;
        }
    }
}
