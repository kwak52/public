using System;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtDsu.PLC.S7.ToolBox
{
    [TestClass]
    public partial class UtS7Tb
    {
        private PLCConnection _connection;

        [TestInitialize()]
        public void Initialize()
        {
            try
            {
                _connection = new PLCConnection("test", "192.168.0.101");
                _connection.Connect();
            }
            catch (Exception)
            {
                _connection.Dispose();
            }
        }

        [TestCleanup()]
        public void Cleanup()
        {
            _connection.Dispose();
        }        
    }
}
