using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;
using Dsu.PLC.Utilities;

namespace Dsu.PLC.Fuji
{
	internal class FjChannelRequestExecutor : ChannelRequestExecutor
	{
		public FjConnection FjConnection { get { return (FjConnection) Connection; } }
		public FjChannelRequestExecutor(FjConnection connection, MemoryType memType, int startByte, int endByte, IEnumerable<TagBase> tags, ConnectionMode connectionMode = ConnectionMode.Cpu0, byte connectionId = 0x0)
			: base(connection, tags)
		{
			Contract.Requires(tags.NonNullAny());

			ushort requestByteLength = (ushort)(1 + endByte - startByte);
			if (requestByteLength%2 == 1)
				requestByteLength++;

			RequestPacket = connection.FjProtocol.GetReadRequestPacket(memType, (uint)startByte, requestByteLength, connectionMode, connectionId).ToArray();
		}

		public override bool ExecuteRead()
		{
            var packet = FjConnection.FjProtocol.Execute(RequestPacket);
            var response = FjPacketResponse.Analyze(packet, RequestPacket);

		    var data = response.Data;
		    foreach (var t in Tags)
		    {
                var byteOffset = t.ByteOffset.GetValueUnsafe();
                if (t.IsBitAddress)
                    t.Value = (data[byteOffset] & (1 << t.BitOffset.GetValueUnsafe())) != 0;
                else
                    t.Value = BitConverter.ToUInt16(data, byteOffset);
            }

            return true;
		}
	}
}
