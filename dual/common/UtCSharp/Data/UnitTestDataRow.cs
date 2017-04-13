using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestDataRow
    {
        private class Phone
        {
            public int Id { get; set; }
            public string PhoneNumber { get; set; }

            public Phone() { }
            public Phone(int id, string phone)
            {
                Id = id;
                PhoneNumber = phone;
            }
        }




        [TestMethod]
        public void TestMethodDataRowSerializationWithReflection()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            new [] {"Id", "PhoneNumber"}.ForEach( c => dataTable.Columns.Add(c) );
            dataTable.Columns["Id"].DataType = typeof (int);

            var record = new Phone(1, "010-1234-5678");
            dataTable.Rows.Add(record.Id, record.PhoneNumber);

            var r = dataTable.Rows[0];
            var p = r.CreateItem<Phone>();

            Assert.IsTrue(p.PhoneNumber == record.PhoneNumber);
        }
    }
}
