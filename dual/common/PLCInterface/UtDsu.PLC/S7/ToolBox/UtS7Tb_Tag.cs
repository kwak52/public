using System;
using System.Diagnostics;
using System.Linq;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtDsu.PLC.S7.ToolBox
{
    partial class UtS7Tb
    {
        [TestMethod]
        public void TestMethodReadWriteSiemensToolBox()
        {
            var tnames = new[] { "DB2.DBW4", "DB5.DBW4", "I0.0", "I0.1", "Q0.0", "QW0", "QW1", "QW2" };
            var tags = (from tname in tnames select new PLCTag(tname)).ToList();
            _connection.ReadValues(tags);
            tags.ForEach(t => Trace.WriteLine(t));
        }
    }
}
