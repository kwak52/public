using System;
using System.Data;
using System.Data.SQLite;

// http://zetcode.com/db/sqlitecsharp/intro/

namespace Dsu.DB.SQLite
{
    public static class EmSQLiteCommand
    {
        /// <summary>
        /// Prepare 구문에서 사용하기 위한 '?' 에 해당하는 parameter 설정 함수.  parameter 가 이미 설정되어 있건, 없건 상관없이 설정 가능.
        /// </summary>
        /// <param name="cmd">prepare 에 사용할 sql command</param>
        /// <param name="name">parameter name</param>
        /// <param name="value">parameter value</param>
        public static void SetOrReplaceParameter(this SQLiteCommand cmd, string name, object value)
        {
            if (!name.StartsWith("@"))
                throw new Exception("SQLiteCommand parameter name should start with '@'.");

            if (cmd.Parameters.Contains(name))
                cmd.Parameters[name].Value = value;
            else
                cmd.Parameters.AddWithValue(name, value);
        }
    }

    /// <summary>
    /// SQLite connection wrapper
    /// </summary>
    public static class EmSQLiteConnection
    {
        public static int ExecuteNonQuery(this SQLiteConnection conn, string sql)
        {
            return new SQLiteCommand(sql, conn).ExecuteNonQuery();
        }

        public static SQLiteDataReader ExecuteReader(this SQLiteConnection conn, string sql)
        {
            return new SQLiteCommand(sql, conn).ExecuteReader();
        }

        public static object ExecuteScalar(this SQLiteConnection conn, string sql)
        {
            return new SQLiteCommand(sql, conn).ExecuteScalar();
        }

        public static DataTable ExecuteReaderIntoDataTable(this SQLiteConnection conn, string sql)
        {
            var adaptor = new SQLiteDataAdapter(new SQLiteCommand(sql, conn));
            DataTable dt = new DataTable();
            adaptor.Fill(dt);
            return dt;
        }

        public static DataSet ExecuteReaderIntoDataSet(this SQLiteConnection conn, string sql)
        {
            var adaptor = new SQLiteDataAdapter(new SQLiteCommand(sql, conn));
            DataSet ds = new DataSet();
            adaptor.Fill(ds);
            return ds;
        }

        public static SQLiteCommand Prepare(this SQLiteConnection conn, string sql, SQLiteTransaction tr = null)
        {
            var cmd = new SQLiteCommand(sql, conn, tr);
            cmd.Prepare();
            return cmd;
        }

        public static object GetUserVariable(this SQLiteConnection conn, string variable)
        {
            if (variable.StartsWith("@"))
                return ExecuteScalar(conn, String.Format("SELECT {0};", variable));

            return null;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/24229785/sqlite-net-sqlitefunction-not-working-in-linq-to-sql/26155359#26155359
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="function"></param>
        public static void BindFunction(this SQLiteConnection connection, SQLiteFunction function)
        {
            var attributes = function.GetType().GetCustomAttributes(typeof(SQLiteFunctionAttribute), true) as SQLiteFunctionAttribute[];
            if (attributes.Length == 0)
            {
                throw new InvalidOperationException("SQLiteFunction doesn't have SQLiteFunctionAttribute");
            }
            connection.BindFunction(attributes[0], function);
        }
    }
}
