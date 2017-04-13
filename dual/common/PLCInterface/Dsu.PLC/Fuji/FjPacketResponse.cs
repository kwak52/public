using System;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities.Core.ExtensionMethods;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.PLC.Fuji
{
    internal class FjPacketResponse
    {
        public readonly OperationStatusType OperationStatus;
        public readonly ConnectionMode ConnectionMode;
        public readonly byte ConnectionId;
        public readonly CommandType CommandType;
        public readonly ModeType ModeType;
        public readonly MemoryType MemoryType;
        public readonly byte[] Data;

        public bool IsOK => OperationStatus == OperationStatusType.Success;
        public string ErrorDescription => IsOK ? null : OperationStatus.ToString();

        private FjPacketResponse(OperationStatusType op)
        {
            OperationStatus = op;
        }

        private FjPacketResponse(OperationStatusType op, ConnectionMode cm, byte cid, CommandType ct, ModeType mode, MemoryType mem, byte[] data)
        {
            OperationStatus = op;
            ConnectionMode = cm;
            ConnectionId = cid;
            CommandType = ct;
            ModeType = mode;
            MemoryType = mem;
            Data = data;
        }


        public static FjPacketResponse Analyze(byte[] response, byte[] request)
        {
            if (response.IsNullOrEmpty())
                return null;

            int i = 0;

            if (response[i++] != 0x80 || response[i++] != 0xFB || response[i++] != 0xC0 || response[i++] != 0x00)
                return null;

            OperationStatusType operationStatus = (OperationStatusType)response[i++];
            if ( operationStatus != OperationStatusType.Success )
                return new FjPacketResponse(operationStatus);

            ConnectionMode connectionMode = (ConnectionMode)response[i++];
            byte connectionId = response[i++];    // Connection ID(L)
            i++;    // Connection ID(H)

            if (response[i++] != 0x11)
                return null;

            for (int j = 0; j < 5; j++)
                if (response[i++] != 0x00)
                    return null;

            CommandType command = (CommandType)response[i++];
            ModeType mode = (ModeType)response[i++];

            if (response[i++] != 0x00 || response[i++] != 0x01)
                return null;

            var dataLength = BitConverter.ToUInt16(response, i);
            i += 2;

            var memoryType = (MemoryType)response[i++];

            var startAddress = BitConverter.ToUInt32(response, i);        // 3 byte integer
            startAddress &= 0x00FFFFFF;
            i += 3;

            var numReadBytes = 2 * BitConverter.ToUInt16(response, i);
            i += 2;

            var readData = response.BlockCopy(i, numReadBytes);
            Trace.WriteLine("Done");

            // compare with request
            var compareLength = 13;
            if ( ! request.Skip(5).Take(compareLength).SequenceEqual(response.Skip(5).Take(compareLength)) )
                throw new Exception("Response packet error.");

            return new FjPacketResponse(operationStatus, connectionMode, connectionId, command, mode, memoryType, readData);
        }
    }
}
