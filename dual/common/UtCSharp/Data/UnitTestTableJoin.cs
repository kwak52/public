using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Data
{
    [TestClass]
    public class UnitTestTableJoin
    {
        [TestMethod]
        public void TestMethodLinqJoin()
        {
            // http://stackoverflow.com/questions/665754/inner-join-of-datatables-in-c-sharp
            // http://stackoverflow.com/questions/2379747/create-combined-datatable-from-two-datatables-joined-with-linq-c-sharp
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("CustID", typeof(int));
            dt1.Columns.Add("ColX", typeof(int));
            dt1.Columns.Add("ColY", typeof(int));

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("CustID", typeof(int));
            dt2.Columns.Add("ColZ", typeof(int));

            for (int i = 1; i <= 5; i++)
            {
                DataRow row = dt1.NewRow();
                row["CustID"] = i;
                row["ColX"] = 10 + i;
                row["ColY"] = 20 + i;
                dt1.Rows.Add(row);

                row = dt2.NewRow();
                row["CustID"] = i;
                row["ColZ"] = 30 + i;
                dt2.Rows.Add(row);
            }
            var x = dt1.AsEnumerable();

            //var results = from table1 in dt1.AsEnumerable()
            //              join table2 in dt2.AsEnumerable() on (int)table1["CustID"] equals (int)table2["CustID"]
            //              select new
            //              {
            //                  CustID = (int)table1["CustID"],
            //                  ColX = (int)table1["ColX"],
            //                  ColY = (int)table1["ColY"],
            //                  ColZ = (int)table2["ColZ"]
            //              };
            var results = from table1 in dt1.AsEnumerable()
                join table2 in dt2.AsEnumerable() on (int) table1["CustID"] equals (int) table2["CustID"]
                select table1.ItemArray.Concat(table2.ItemArray.Skip(1)).ToArray();

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("CustID", typeof(int));
            dtNew.Columns.Add("ColX", typeof(int));
            dtNew.Columns.Add("ColY", typeof(int));
            dtNew.Columns.Add("ColZ", typeof(int));

            foreach (var item in results)
            {
                dtNew.Rows.Add(item);
                //Trace.WriteLine(String.Format("ID = {0}, ColX = {1}, ColY = {2}, ColZ = {3}", item.CustID, item.ColX, item.ColY, item.ColZ));
            }

            dtNew.Rows[0][0] = -100;
            var xxx = dt1.Rows[0][0];
            Console.WriteLine(xxx);
        }
    }
}
