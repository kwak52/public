using System;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using DotNetSiemensPLCToolBoxLibrary.DataTypes;

namespace Dsu.PLC.Siemens
{
    internal class S7Cpu : ICpu
    {
        private PLCConnection _connection;

        public string Model { get; }
        public PLCState State => _connection.PLCGetState();

        public S7Cpu(PLCConnection connection)
        {
            _connection = connection;
        }

        public bool IsRunning => State == PLCState.Running;
        public void Stop() => _connection.PLCStop();
        public void Run() => _connection.PLCStart();

        public DateTime ReadTime() => _connection.PLCReadTime();
        public void SetTime(DateTime dt) => _connection.PLCSetTime(dt);
    }
}
