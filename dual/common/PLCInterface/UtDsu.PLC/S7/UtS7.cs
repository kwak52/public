using System;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Siemens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtDsu.PLC.S7
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public partial class UtS7
    {
        private S7Connection _connection;

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void Initialize()
        {
            try
            {
                _connection = new S7Connection(new S7ConnectionParameters("192.168.0.101"));
                _connection.Connect();
            }
            catch (Exception)
            {
                _connection.Dispose();
            }
        }

        [TestCleanup()]
        public void CleanUp()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void UtS7ReadWrite()
        {
            var tnames = new[] {"DB2.DBW4", "DB5.DBW4", "I0.0", "I0.1", "Q0.0", "QW0", "QW1", "QW2"};
            var tags = _connection.CreateTags(tnames);
            //var tags = (from tname in tnames select new S7Tag(_connection, tname)).ToList();
            _connection.SingleScan(prepare: true);
            //_connection.ReadAllChannels().Realize();
            tags.ForEach(t => Trace.WriteLine(String.Format("{0} = {1}", t.Name, t.Value)));
        }

    }
}
