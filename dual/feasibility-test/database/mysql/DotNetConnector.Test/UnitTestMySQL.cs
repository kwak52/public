#define Functional 


using System;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading;
using Microsoft.FSharp.Core;


#if Functional
    using Dsu.DB.FS;
    using LanguageExt;
    using static LanguageExt.Prelude;
//using Dsu.Common.Utilities.FS;
#else
    using Dsu.DB.MySQL;
    using Dsu.DB.MySQL;
#endif


// http://zetcode.com/db/mysqlcsharptutorial/

namespace DotNetConnector.Test
{
    [TestClass]
    public class UnitTestMySQL
    {
        MySqlConnection _conn;

        [TestInitialize]
        public void Initialize()
        {
            string connStr = "server=dualsoft.co.kr;user=securekwak;database=kefico;port=3306;password=kwak;Allow User Variables=True";
            //string connStr = "server=dualsoft.co.kr;user=securekwak;database=ahn_db;port=3306;password=kwak;Allow User Variables=True";
            //_conn = new MySqlConnection(connStr);
            //_conn.Open();

            //var xconn = new MySqlConnection(connStr);
            //xconn.Open();

            Trace.WriteLine("Connecting.......");
#if Functional
            _conn = MySQLFSExt.connect(connStr);
#else
            _conn = new MySqlConnection(connStr);
            _conn.Open();
#endif
            _conn.ExecuteNonQuery("SET @host=9001; SET @sec='1-1';");
            _conn.ExecuteNonQuery("SELECT id INTO @ccs FROM ccs WHERE host=9001 AND sec='1-1';");

            var ccsId = _conn.ExecuteScalar("SELECT id FROM ccs WHERE host=9001 AND sec='1-1';");
            Trace.WriteLine(String.Format("MySQL Server Version = {0}, CCS ID={1}", _conn.ServerVersion, ccsId));
        }

        [TestCleanup]
        public void TestFinalize()
        {
            if (_conn != null)
            {
                _conn.Close();
            }

        }


#if Functional
        private T GetValue<T>(FSharpOption<T> opt, T defaultValue = default(T))
        {
            return match(FSharp.fs<T>(opt),
                None: () => defaultValue,
                Some: v => v);
        }
#else
        private T GetValue<T>(T var) => var;
#endif


        [TestMethod]
        public void TestMethodUserVariable()
        {
            _conn.ExecuteNonQuery("SET @user='kwak';");
            Assert.AreEqual(_conn.ExecuteScalar("SELECT @user;"), "kwak");
            var user = _conn.GetUserVariable("@user");
            Assert.AreEqual(GetValue(user), "kwak");
            var ccs = _conn.GetUserVariable("@ccs");
            Trace.WriteLine(String.Format("My ccs id = {0}", GetValue(ccs)));

            Assert.AreEqual(GetValue(_conn.GetUserVariable("@host")), 9001L);
            Assert.AreEqual(GetValue(_conn.GetUserVariable("@sec")), "1-1");
        }

        [TestMethod]
        public void TestMethodTransaction()
        {
            MySqlTransaction tr = null;
            try
            {
                tr = _conn.BeginTransaction();

                _conn.ExecuteNonQuery("INSERT INTO bundle(day, measureId, stepId, value, message, ok)"
                                    + "VALUES('2016-08-16', 7, 21, 11111, 'Test Insert', 1)");

                _conn.ExecuteNonQuery("XINSERT INTO bundle(day, measureId, stepId, value, message)"
                                    + "VALUES('2016-08-16', 7, 21, 11111, 'Test Insert', 1)");

                _conn.ExecuteNonQuery("INSERT INTO bundle(day, measureId, stepId, value, message)"
                                    + "VALUES('2016-08-16', 7, 21, 11111, 'Test Insert', 1)");

                tr.Commit();
            }
            catch (MySqlException ex)
            {
                try
                {
                    tr.Rollback();
                }
                catch (MySqlException ex1)
                {
                    Console.WriteLine("Error: {0}", ex1.ToString());
                }

                Console.WriteLine("Error: {0}", ex.ToString());
            }
        }


        [TestMethod]
        public void TestMethodTransactionWithPrepare()
        {
            MySqlTransaction tr = null;
            try
            {
                tr = _conn.BeginTransaction();
                var sql = "INSERT INTO bundle(startDay, measureId, positionId, value)"
                                    + "VALUES('2016-08-15', 21, @positionId, @value)";

                MySqlCommand cmd = _conn.Prepare(sql, tr);

                for (int i = 6; i < 25; i++ )
                {
                    cmd.SetOrReplaceParameter("@positionId", i);
                    cmd.SetOrReplaceParameter("@value", i * 10);
                    cmd.ExecuteNonQuery();
                }

                tr.Commit();

            }
            catch (MySqlException ex)
            {
                try 
                {
                    tr.Rollback();
                }
                catch (MySqlException ex1)
                {
                    Console.WriteLine("Failed to rollback: {0}", ex1.ToString());
                }

                Console.WriteLine("Error: {0}", ex.ToString());
            }
        }

        [TestMethod]
        public void TestMethodReadIntoDataTable()
        {
            //var dataTable = _conn.ExecuteReaderIntoDataTable("SELECT * FROM CUSTOMER_ORDER;");

            var dataTable = _conn.ExecuteReaderIntoDataTable("SELECT * FROM measure;");
            IEnumerable<DataRow> measures =
                from measure in dataTable.AsEnumerable()
                select measure;

            foreach (var row in measures)
                Trace.WriteLine(row.Field<string>("ecuid"));

            Trace.WriteLine(dataTable.ConvertToString());
        }
        [TestMethod]
        public void TestMethodExecuteRecoderIntoDataTable()
        {
            var dataTable = _conn.ExecuteRecoderIntoDataTable("SELECT * FROM pdv;");
            Trace.WriteLine(dataTable.ConvertToString());
        }
        

        [TestMethod]
        public void TestMethodReadIntoDataSet()
        {
            var dataSet = _conn.ExecuteReaderIntoDataSet("CALL showAll();");
            Trace.WriteLine(dataSet.ToString());
        }
    }
}
