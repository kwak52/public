using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.PLC.Fuji
{
    internal class FjPacketRequest
    {
        /// <summary>
        /// Protocol header part
        /// All values are fixed.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<byte> GetProtocolHeader()
        {
            yield return 0xFB;  // DP(Destination Port)
            yield return 0x80;  // SP(Source Port)
            yield return 0x80;  // Transaction
            yield return 0x00;  // Transmission status
        }

        private static IEnumerable<byte> GetConnection(ConnectionMode connectionMode = ConnectionMode.Cpu0, byte connectionId = 0x0)
        {
            yield return (byte)OperationStatusType.Send;
            yield return (byte)connectionMode;
            yield return connectionId;      // Connection ID(L)
            yield return 0x0;               // Connection ID(H)
        }

        private static IEnumerable<byte> GetCommandMode(CommandType command, ModeType mode)
        {
            yield return 0x11;
            yield return 0x00;
            yield return 0x00;
            yield return 0x00;
            yield return 0x00;
            yield return 0x00;

            yield return (byte)command;
            yield return (byte)mode;

            yield return 0x00;
            yield return 0x01;
        }

        private static IEnumerable<byte> GetMemoryRequest(MemoryType memoryType, uint startAddress, ushort requestByteLength)
        {
			Contract.Requires(requestByteLength % 2 == 0);
	        ushort requestWordLength = (ushort) (requestByteLength / 2);

			yield return (byte)memoryType;

            // Address L, M, H bytes
            foreach (var b in BitConverter.GetBytes(startAddress).Skip(1))
                yield return b;

            foreach (var b in BitConverter.GetBytes(requestWordLength))
                yield return b;
        }

        private static IEnumerable<byte> GetRequestPacket(CommandType command, ModeType mode, MemoryType memoryType, uint startAddress, ushort requestByteLength,
            ConnectionMode connectionMode, byte connectionId)
        {
            var memRequest = GetMemoryRequest(memoryType, startAddress, requestByteLength).ToArray();
            ushort length = (ushort)memRequest.Length;
            return GetProtocolHeader()
                    .Concat(GetConnection(connectionMode, connectionId))
                    .Concat(GetCommandMode(command, mode))
                    .Concat(BitConverter.GetBytes(length))
                    .Concat(memRequest)
                ;
        }

        public static IEnumerable<byte> GetReadRequestPacket(MemoryType memoryType, uint startAddress,
            ushort requestByteLength, ConnectionMode connectionMode = ConnectionMode.Cpu0, byte connectionId = 0x0)
        {
            return GetRequestPacket(CommandType.Read, ModeType.Read, memoryType, startAddress, requestByteLength, connectionMode, connectionId);
        }



	    public static bool ValidateSendPacket(byte[] packet)
	    {
		    return packet.Length == 26
				&& packet[0] == 0xFB
				&& packet[1] == 0x80
				&& packet[2] == 0x80
				&& packet[3] == 0x00
				&& packet[4] == 0xFF				// Operation status : Send(0xFF)
				&& ((int)packet[5]).IsOneOf(0x7A, 0x7B)	// Connection mode : CPU0(0x7A), others(0x7B)
				&& ((int)packet[6]).InRange(0, 238)		// ConnectionID(L)
				&& packet[7] == 0x00				// ConnectionID(H)

				&& packet[8] == 0x11
				&& packet[9] == 0x00
				&& packet[10] == 0x00
				&& packet[11] == 0x00
				&& packet[12] == 0x00
				&& packet[13] == 0x00

				&& ((int)packet[14]).IsOneOf(0, 1)		// command : read(0), write(1)
				&& packet[15] == 0x00			// mode : Read/Write 시 0, CPU 는 다른 값
				&& packet[16] == 0x00			// fixed
				&& packet[17] == 0x01           // fixed
				//&& packet[18] == ??			// Number of bytes of data part(L)
				//&& packet[19] == ??			// Number of bytes of data part(H)
				&& ((int)packet[20]).IsOneOf(1, 2, 4, 8, 0xFF)    // Memory type : IO(1), Standard(2), Retain(4), System(8), P/PE-link, FL-net(0xFF)
				//&& packet[21] == ??			// Address(L)
				//&& packet[22] == ??			// Address(M)
				//&& packet[23] == ??			// Address(H)
				//&& packet[24] == ??			// Read request length(L)
				//&& packet[25] == ??			// Read request length(H)
				;
		}
	}
}
