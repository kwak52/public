using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools;

namespace Dsa.Test.DbReader
{
    public class MySql
    {
        public string connStr = "server=dualsoft.co.kr;user=securekwak;database=ahn_db;password=kwak;";
        public string DatabaseName = "ahn_db";
        public string DataTableName = "CUSTOMER_ORDER";

        public MySql()
        {
        }

        public DataTable Read0(string query)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Connection.Open();
                var table = new DataTable();
                using (var r = cmd.ExecuteReader())
                {
                    DataTable dtSchema = r.GetSchemaTable();
                    for (int i = 0; i < dtSchema.Rows.Count; i++)
                        table.Columns.Add(new DataColumn((string)dtSchema.Rows[i]["ColumnName"], (Type)dtSchema.Rows[i]["DataType"]));

                    while (r.Read())
                    {
                        object[] arrObj = new object[r.FieldCount];
                        r.GetValues(arrObj);
                        table.Rows.Add(arrObj);
                    }
                }
                return table;
            }
        }

        public DataTable Read1(string query)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Connection.Open();
                var table = new DataTable();
                using (var r = cmd.ExecuteReader())
                    table.Load(r);
                return table;
            }
        }

        public DataTable Read2<S>(string query) where S : IDbDataAdapter, IDisposable, new()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            using (var da = new S())
            {
                using (da.SelectCommand = conn.CreateCommand())
                {
                    da.SelectCommand.CommandText = query;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        public IEnumerable<S> Read3<S>(string query, Func<IDataReader, S> selector)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Connection.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        yield return selector(r);
            }
        }

        public S[] Read4<S>(string query, Func<IDataRecord, S> selector)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Connection.Open();
                using (var r = cmd.ExecuteReader())
                    return ((DbDataReader)r).Cast<IDataRecord>().Select(selector).ToArray();
            }
        }

        public List<S> Read5<S>(string query, Func<IDataReader, S> selector)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Connection.Open();
                using (var r = cmd.ExecuteReader())
                {
                    var items = new List<S>();
                    while (r.Read())
                        items.Add(selector(r));
                    return items;
                }
            }
        }

     
    }
}