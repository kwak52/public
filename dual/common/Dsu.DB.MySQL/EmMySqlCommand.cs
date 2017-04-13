using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dsu.DB.MySQL
{
    public static class EmMySqlCommand
    {
        /// <summary>
        /// Prepare 구문에서 사용하기 위한 '?' 에 해당하는 parameter 설정 함수.  parameter 가 이미 설정되어 있건, 없건 상관없이 설정 가능.
        /// </summary>
        /// <param name="cmd">prepare 에 사용할 sql command</param>
        /// <param name="name">parameter name</param>
        /// <param name="value">parameter value</param>
        public static void SetOrReplaceParameter(this MySqlCommand cmd, string name, object value)
        {
            if (!name.StartsWith("@"))
                throw new Exception("MySqlCommand parameter name should start with '@'.");
  
            if (cmd.Parameters.Contains(name))
                cmd.Parameters[name].Value = value;
            else
                cmd.Parameters.AddWithValue(name, value);
        }
    }


    /// <summary>
    /// MySQL connection wrapper
    /// </summary>
    public static class EmMySqlConnection
    {
        /// <summary>
        /// C# extension method on MySqlConnection : ExecuteNonQuery on connection
        /// </summary>
        /// <param name="conn">MySqlConnection</param>
        /// <param name="sql">SQL query string</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this MySqlConnection conn, string sql)
        {
            return new MySqlCommand(sql, conn).ExecuteNonQuery();
        }

        public static MySqlDataReader ExecuteReader(this MySqlConnection conn, string sql)
        {
            return new MySqlCommand(sql, conn).ExecuteReader();
        }

        public static object ExecuteScalar(this MySqlConnection conn, string sql)
        {
            return new MySqlCommand(sql, conn).ExecuteScalar();
        }

        public static DataTable ExecuteReaderIntoDataTable(this MySqlConnection conn, string sql)
        {
            var adaptor = new MySqlDataAdapter(new MySqlCommand(sql, conn));
            DataTable dt = new DataTable();
            adaptor.Fill(dt);
            return dt;
        }

        public static DataTable ExecuteRecoderIntoDataTable(this MySqlConnection conn, string sql)
        {
            DataTable dt = new DataTable();
            using (MySqlDataReader rdr = (new MySqlCommand(sql, conn)).ExecuteReader())
            {
                DataTable dtSchema = rdr.GetSchemaTable();
                for (int i = 0; i < dtSchema.Rows.Count; i++)
                    dt.Columns.Add(new DataColumn((string)dtSchema.Rows[i]["ColumnName"], (Type)dtSchema.Rows[i]["DataType"]));

                while (rdr.Read())
                {
                    object[] arrObj = new object[rdr.FieldCount];
                    rdr.GetValues(arrObj);
                    dt.Rows.Add(arrObj);
                }
            }
            return dt;
        }

        public static DataSet ExecuteReaderIntoDataSet(this MySqlConnection conn, string sql)
        {
            var adaptor = new MySqlDataAdapter(new MySqlCommand(sql, conn));
            DataSet ds = new DataSet();
            adaptor.Fill(ds);
            return ds;
        }

        public static MySqlCommand Prepare(this MySqlConnection conn, string sql, MySqlTransaction tr=null)
        {
            var cmd = new MySqlCommand(sql, conn, tr);
            cmd.Prepare();
            return cmd;
        }

        public static object GetUserVariable(this MySqlConnection conn, string variable)
        {
            if (variable.StartsWith("@"))
                return ExecuteScalar(conn, String.Format("SELECT {0};", variable));

            return null;
        }
    }
}
