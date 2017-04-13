using System;
using System.Linq;

namespace Dsu.PLC.Melsec
{
    partial class McProtocolApp
    {
        private bool _isQL_series;   // Q or L series
        internal MxCpu Cpu { get; private set; }
        internal UInt32 Password { get { return (UInt32)(_isQL_series ? 0x20202020 : 0); } }

        /// <summary>
        /// Reads CPU model name
        /// </summary>
        public string ReadModelName()
        {
            byte[] response = ExecuteCommand(DeviceAccessCommand.ReadCpuModelName, new byte[] { });
            return System.Text.ASCIIEncoding.ASCII.GetString(response, 0, 8).Trim();
        }


        public void RemoteRun(bool force=true)
        {
            UInt16 mode = (UInt16) (force ? 0x0003 : 0x0001);
            byte clearMode = 0x02;
            byte fixedValue = 0;
            var data = BitConverter.GetBytes((UInt16) mode)
                    .Concat(new byte[] {clearMode})
                    .Concat(new byte[] {fixedValue})
                ;

            ExecuteCommand(DeviceAccessCommand.RemoteRun, data.ToArray());
        }

        public void RemoteStop(bool force = true)
        {
            UInt16 mode = (UInt16)(force ? 0x0003 : 0x0001);
            var data = BitConverter.GetBytes((UInt16)mode);

            ExecuteCommand(DeviceAccessCommand.RemoteStop, data.ToArray());
        }

        public void RemotePause(bool force = true)
        {
            UInt16 mode = (UInt16)(force ? 0x0003 : 0x0001);
            var data = BitConverter.GetBytes((UInt16)mode);

            ExecuteCommand(DeviceAccessCommand.RemotePause, data.ToArray());
        }

        public void RemoteReset(bool force = true)
        {
            UInt16 mode = (UInt16)(force ? 0x0003 : 0x0001);
            var data = BitConverter.GetBytes((UInt16)mode);

            ExecuteCommand(DeviceAccessCommand.RemoteReset, data.ToArray());
        }

        public void RemoteLatchClear(bool force = true)
        {
            UInt16 mode = (UInt16)(force ? 0x0003 : 0x0001);
            var data = BitConverter.GetBytes((UInt16)mode);

            ExecuteCommand(DeviceAccessCommand.RemoteLatchClear, data.ToArray());
        }
    }
}
