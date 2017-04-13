using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtDsu.PLC
{
    partial class UtMx
    {
        [TestMethod]
        public void UtMxCpuReadModelName()
        {
            var model = _connection.Cpu.Model;
            Trace.WriteLine(model);
        }

        [TestMethod]
        public void UtMxCpuStop()
        {
            McProtocol.RemoteStop();
        }

        [TestMethod]
        public void UtMxCpuRun()
        {
            McProtocol.RemoteRun();
        }

    }
}