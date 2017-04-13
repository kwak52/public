using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dsu.DB.MySQL;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Dsa.Hmc.Spc
{
    public class MySqlClient
    {
        MySqlConnection _conn;

        public MySqlClient()
        {
            string connStr = string.Format("server={0};user={1};database={2};password={3};port={4};Allow User Variables=True"
                 , "localhost"
                 , "root"
                 , "spc"
                 , "ahn!!"
                 , "3306"
                 );

            _conn = new MySqlConnection(connStr);
        }


        public bool Open()
        {
            _conn.Open();
            return true;
        }

        public DataTable GetDataFromDBView(string sql)
        {
            DataTable dt = new DataTable();

            dt = _conn.ExecuteReaderIntoDataTable(sql);

            return dt;
        }

        public bool Execute(string sql)
        {
            _conn.ExecuteNonQuery(sql);
            return true;
        }

        public int GetRecordCount(string TableName)
        {
            int RecordCount = -1;
            string sql = string.Format("SELECT COUNT(*)  FROM {0}", TableName);

            var count = _conn.ExecuteScalar(sql);
            RecordCount = Convert.ToInt32(count);


            return RecordCount;
        }
    }
}
