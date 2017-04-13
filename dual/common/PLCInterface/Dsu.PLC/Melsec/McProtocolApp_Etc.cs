using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;

namespace Dsu.PLC.Melsec
{
    partial class McProtocolApp
    {
        public enum DeviceAccessType
        {
            Bit = 0,
            Word = 1,
        }
        private UInt16 GetSubCommandWithType(DeviceAccessCommand command, DeviceAccessType accessType)
        {
			Contract.Requires(command == DeviceAccessCommand.BatchRead 
				|| command == DeviceAccessCommand.BatchWrite 
				|| command == DeviceAccessCommand.RandomRead 
				|| command == DeviceAccessCommand.RandomWrite);

			if (accessType == DeviceAccessType.Word)
                return (UInt16)(_isQL_series ? 0 : 2);      // 0x0200 ??

			return (UInt16)(_isQL_series ? 1 : 3);          // 0x0100 : 0x0300 ??
		}


        private UInt16 GetSubCommand(DeviceAccessCommand command)
        {
            //Contract.Requires(command != DeviceAccessCommand.BatchRead
            //    && command != DeviceAccessCommand.BatchWrite
            //    && command != DeviceAccessCommand.RandomRead
            //    && command != DeviceAccessCommand.RandomWrite);

            switch (command)
            {
                case DeviceAccessCommand.FileOpen:
                    // 0. MELSEC Communication Protocol-sh080008w.pdf, pp. 236.
                    if (Cpu.Model.StartsWith("L"))
                        return 0x0004;
                    break;
                case DeviceAccessCommand.FileClose:
                case DeviceAccessCommand.FileWrite:
                case DeviceAccessCommand.FileRead:
                    return (UInt16) 0;
            }

            return (UInt16)(_isQL_series ? 0 : 0x0040);
        }

        public byte[] ExecuteCommand(DeviceAccessCommand command, byte[] data)
        {
            byte[] sendCommand = Command.SetCommand(command, data.ToArray());

            byte[] returnResponse = TryExecution(sendCommand);
            int rtCode = Command.SetResponse(returnResponse);
            byte[] response = Command.Response;

            if (rtCode == 0)
                return response;

            Debug.Assert(response.Length == 9 || response.Length == 20);

            throw new PlcException($"Failed command {command}");
        }
    }
}
