using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SQLite;
using Dsu.DB.SQLite;

namespace DotNetConnector.Test
{
    // http://stackoverflow.com/questions/172735/create-use-user-defined-functions-in-system-data-sqlite
    // taken from http://sqlite.phxsoftware.com/forums/p/348/1457.aspx#1457
    [SQLiteFunction(Name = "ToUpper", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class ToUpper : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return args[0].ToString().ToUpper();
        }
    }



    [TestClass]
    public class UnitTestSQLite
    {
        SQLiteConnection _conn;

        [TestInitialize]
        public void Initialize()
        {
            string connStr = "Data source=sqlite-test.db";
            _conn = new SQLiteConnection(connStr);
            _conn.Open();

            Trace.WriteLine(String.Format("SQLite Server Version = {0}", _conn.ServerVersion));

            _conn.ExecuteNonQuery("DROP TABLE IF EXISTS cars;");
            _conn.ExecuteNonQuery("CREATE TABLE cars(id INTEGER PRIMARY KEY, name VARCHAR(255), price INT);");

            TestMethodTransaction();
        }

        [TestCleanup]
        public void TestFinalize()
        {
            if (_conn != null)
            {
                _conn.Close();
            }
        }

        [TestMethod]
        public void TestSQL()
        {
            _conn.ExecuteNonQuery("INSERT INTO cars(name, price) VALUES('Benz', 1000);");
            var price = _conn.ExecuteScalar("SELECT price FROM cars WHERE name='Benz';");
            Assert.AreEqual(price, 1000);
        }

        [TestMethod]
        public void TestMethodTransaction()
        {
            SQLiteTransaction tr = null;
            try
            {
                tr = _conn.BeginTransaction();

                _conn.ExecuteNonQuery("INSERT INTO cars(name, price)"
                                    + "VALUES('sonata', 2000)");

                _conn.ExecuteNonQuery("INSERT INTO cars(name, price)"
                                    + "VALUES('grandeure', 3000)");

                _conn.ExecuteNonQuery("INSERT INTO cars(name, price)"
                                    + "VALUES('i40', 4000)");

                tr.Commit();
            }
            catch (SQLiteException ex)
            {
                try
                {
                    tr.Rollback();
                }
                catch (SQLiteException ex1)
                {
                    Console.WriteLine("Error: {0}", ex1.ToString());
                }

                Console.WriteLine("Error: {0}", ex.ToString());
            }

            Assert.AreEqual(_conn.ExecuteScalar("SELECT price FROM cars WHERE name='sonata';"), 2000);
            Assert.AreEqual(_conn.ExecuteScalar("SELECT price FROM cars WHERE name='grandeure';"), 3000);
            Assert.AreEqual(_conn.ExecuteScalar("SELECT price FROM cars WHERE name='i40';"), 4000);
        }


        [TestMethod]
        public void TestMethodTransactionWithPrepare()
        {
            SQLiteTransaction tr = null;
            try
            {
                tr = _conn.BeginTransaction();
                var sql = "INSERT INTO cars(name, price)"
                                    + "VALUES(@model, @price)";
                //var sql = "INSERT INTO cars(name, price)"
                //                    + "VALUES('@model, @price)";

                SQLiteCommand cmd = _conn.Prepare(sql, tr);

                for (int i = 0; i < 10; i++)
                {
                    cmd.SetOrReplaceParameter("@model", i.ToString());
                    cmd.SetOrReplaceParameter("@price", i * 10);
                    cmd.ExecuteNonQuery();
                }

                tr.Commit();
            }
            catch (SQLiteException ex)
            {
                try
                {
                    tr.Rollback();
                }
                catch (SQLiteException ex1)
                {
                    Console.WriteLine("Error: {0}", ex1.ToString());
                }

                Console.WriteLine("Error: {0}", ex.ToString());
            }
        }

        [TestMethod]
        public void TestMethodReadIntoDataTable()
        {
            var dataTable = _conn.ExecuteReaderIntoDataTable("SELECT * FROM cars;");
            //Trace.WriteLine(dataTable.ConvertToString());
        }

        [TestMethod]
        public void TestMethodSQLiteDoesNotSupportStoredProcedure()
        {
            _conn.BindFunction(new ToUpper());

            // example SQL:  SELECT * FROM Foo WHERE Foo.Name REGEXP '$bar'
            var model = _conn.ExecuteScalar("SELECT ToUpper(name) FROM cars limit 1;");
            Trace.WriteLine(model);
        }
    }
}
