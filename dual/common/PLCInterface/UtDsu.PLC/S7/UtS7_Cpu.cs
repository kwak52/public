using System.Diagnostics;
using Dsu.PLC.Siemens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtDsu.PLC.S7
{
    partial class UtS7
    {
        [TestMethod]
        public void UtS7CpuStartStop()
        {
            var cpu = _connection.Cpu;
            cpu.Stop();
            Assert.IsFalse(cpu.IsRunning);
            cpu.Run();
            Assert.IsTrue(cpu.IsRunning);
        }


        [TestMethod]
        public void UtS7CpuStatus()
        {
            var cpu = (S7Cpu)_connection.Cpu;
            Trace.WriteLine($"Current CPU status = {cpu.State}");
        }
    }
}
