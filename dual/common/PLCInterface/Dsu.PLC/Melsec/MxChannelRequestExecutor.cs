using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Dsu.Common.Utilities.Core;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;
using Dsu.PLC.Utilities;
using static LanguageExt.Prelude;

namespace Dsu.PLC.Melsec
{
	internal abstract class MxChannelRequestExecutor : ChannelRequestExecutor
	{
		public MxConnection MxConnection { get { return (MxConnection) Connection; } }
		protected MxTag[] _wordDevices;
		protected MxTag[] _doubleWordDevices;
		public MxChannelRequestExecutor(MxConnection connection, IEnumerable<MxTag> tags)
			: base(connection, tags)
		{
			_wordDevices = tags.SelectWordDevices().OrderBy(t => t.DeviceType).ThenBy(t => t.Addreess).ToArray();
			_doubleWordDevices = tags.SelectDoubleWordDevices().OrderBy(t => t.DeviceType).ThenBy(t => t.Addreess).ToArray();
		}
	}

	/// <summary>
	/// RandomRead 를 통한 request executor
	/// </summary>
	internal class MxChannelRequestExecutorRandom : MxChannelRequestExecutor
	{
        /// <summary>
        /// 각 tag 별로 response byte array 의 몇번째 byte 부터 읽어야 하는지의 정보를 담고 있는 dictionary
        /// </summary>
		private Dictionary<MxTag, int> _tagOffsetMap = new Dictionary<MxTag, int>();
        private List<MxTag> RemainingTags { get; }

        public MxChannelRequestExecutorRandom(MxConnection connection, IEnumerable<MxTag> tags)
			: base(connection, tags)
		{
			Contract.Requires(tags.NonNullAny());
			int globalOffset = 0;

		    bool hasRemainingTags = false;
		    const int pointMax = 192;
		    int pointCount = 0;

            // word device major : request 에 word 단위로 적어도 하나씩 포함되도록 하기 위함
            List<MxTag> wdMajors = new List<MxTag>();

            // double word device major : request 에 double word 단위로 적어도 하나씩 포함되도록 하기 위함
            // bitwise range 지정된 항목은 추후 처리를 위해서 제외
            List<MxTag> dwdMajors = new List<MxTag>();

            try
            {
                // word device groups
                var wGroups =
                    from t in _wordDevices.Where(t => t.ByteOffset.IsSome)
                    let byteOffset = t.ByteOffset.GetValueUnsafe()
                    let wOffset = byteOffset / 2
                    group t by new { t.DeviceType, wOffset } into g            // device type 과 byte offset 에 따라 grouping
                    select new
                    {
                        WOffset = g.Key.wOffset,
                        DeviceType = g.Key.DeviceType,
                        Tags = g.ToList(),
                    };

                foreach (var grp in wGroups)
                {
                    grp.Tags.OrderBy(t => t.Addreess).ForEach(t => _tagOffsetMap.Add(t, globalOffset + t.ByteOffset.GetValueUnsafe() % 2));
                    globalOffset += 2;

                    wdMajors.Add(grp.Tags.First());

                    if (++pointCount == pointMax)
                    {
                        hasRemainingTags = true;
                        return;
                    }
                }


                // double word device.
                // Bitwise range (K2, K4 등)를 포함할 수 있다.
                // _doubleWordDevices 를 device type 과 doubleword(=4byte) offset 에 따라 grouping
                // e.g {X[0..15], Y[0..15]} 의 32 개 device 를 grouping 하면, 
                // X[0..31], Y[0..31] 의 2개로 grouping 된다.
                var dwGroups =
                    from t in _doubleWordDevices
                    where t.ByteOffset.IsSome
                    where t.BitwiseRange.IsNone     // bitwise range 지정된 항목은 추후 처리를 위해서 제외
                    let byteOffset = t.ByteOffset.GetValueUnsafe()
                    let dwOffset = byteOffset / 4
                    group t by new { t.DeviceType, dwOffset } into g            // device type 과 byte offset 에 따라 grouping
                    select new
                    {
                        DWOffset = g.Key.dwOffset,
                        DeviceType = g.Key.DeviceType,
                        Tags = g.ToList(),
                    };


                foreach (var grp in dwGroups)
                {
                    if (grp == null)
                        continue;

                    var grpTags = grp.Tags.OrderBy(t => t.Addreess);
                    if (grpTags.Any(t => !_tagOffsetMap.ContainsKey(t)))
                    {
                        // group 내의 tag 중에 _tagOffsetMap 에 하나라도 포함되지 않은 것이 있을 떄에만,
                        // _tagOffsetMap 에 포함되지 않은 것들에 대해서 _tagOffsetMap 에 추가.
                        grpTags
                            .Where(t => !_tagOffsetMap.ContainsKey(t))
                            .ForEach(t =>
                            {
                                _tagOffsetMap.Add(t, globalOffset + t.ByteOffset.GetValueUnsafe() % 4);
                            });

                        globalOffset += 4;

                        dwdMajors.Add(grp.Tags.First());

                        if (++pointCount == pointMax)
                        {
                            hasRemainingTags = true;
                            return;
                        }
                    }
                }

                // bitwise range 지정된 항목에 대한 처리
                _doubleWordDevices
                    .Where(t => t.BitwiseRange.IsSome)
                    .ForEach(t =>
                    {
                        _tagOffsetMap.Add(t, _tagOffsetMap[t.Aliases[0]]);
                    });

            }
            finally
		    {
                if (hasRemainingTags)
                {
                    RemainingTags = tags.Except(_tagOffsetMap.Keys).ToList();
                }

                RequestPacket = connection.McProtocol.GenerateRandomRequestPacket(connection, wdMajors, dwdMajors, packAlign: true).ToArray();
            }
        }


	    public static IEnumerable<MxChannelRequestExecutorRandom> CreateChannelRequestExecutorRandoms(
	        MxConnection connection, IEnumerable<MxTag> tags)
	    {
	        var remainingTags = tags;
	        while (remainingTags.NonNullAny())
	        {
                var channel = new MxChannelRequestExecutorRandom(connection, remainingTags);
	            yield return channel;
	            remainingTags = channel.RemainingTags;
	        }
	    }

        public override bool ExecuteRead()
		{
			// byte 읽기 순서
			// byte 0: <-----------------(bit0)
			// byte 1: <-----------------(bit8 = X8)
			// byte 2: <-----------------(bit16 = X10)
			// byte 3: <-----------------(bit24 = X18)
			// byte 4: <-----------------(bit32 = X20)
			// byte 5: <-----------------(bit40 = X28)

			var response = MxConnection.McProtocol.ExecuteCommand(DeviceAccessCommand.RandomRead, RequestPacket);
		    var now = DateTime.Now;

			foreach (var pr in _tagOffsetMap)
			{
				var tag = pr.Key;
				var offset = pr.Value;
			    var address = tag.Addreess;

                tag.Timestamp = now;

                if (tag.IsBitDevice)
                {
                    match(tag.BitwiseRange,
                        Some: v =>
                        {
                            var start = address%32/* - (offset%4) * 8*/;
                            var end = start + v*4 - 1;

                            ulong pattern = BitConverter.ToUInt32(response, offset);

                            if (v*4 > 32 - address%32)
                            {
                                if(response.Length - offset < 8)
                                    response = response.Concat(BitConverter.GetBytes((int) 0)).ToArray();
                                 
                                pattern = BitConverter.ToUInt64(response, offset);
                            }

                            ulong value = pattern.GetBitsValue(start, end);

                            switch (v)
                            {
                                case 1:
                                case 2:
                                    tag.Value = (byte) value;
                                    break;
                                case 3:
                                case 4:
                                    tag.Value = (ushort) value;
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    tag.Value = value;
                                    break;
                                default:
                                    throw new Exception($"Unknown bitwise: {tag.Name}");
                            }
                        },
                        None: () =>
                        {
                            byte b = response[offset];
                            int mask = 1 << tag.BitOffset.GetValueUnsafe();
                            tag.Value = (b & mask) != 0;
                        }
                    );
                }
                else
                {
                    ushort n = BitConverter.ToUInt16(response, offset);
                    match(tag.WordDotBit,
                        Some: v =>      //ex) D100.F ,WFF.2 WordDotBit 타입
                        {
                            int mask = 1 << v;
                            tag.Value = (n & mask) != 0;

                        },
                        None: () => tag.Value = n
                    );
                }
            }

			return true;
		}
	}
}
