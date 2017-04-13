using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities;
using Dsu.PLC.Common;
using LanguageExt;

namespace Dsu.PLC.Melsec
{
    public class MxTag : TagBase
    {
        private MxConnection Connection => (MxConnection)ConnectionBase;
        public PlcDeviceType DeviceType { get; private set; }
        public int Addreess { get; private set; } = -1;
        public string AddressString => IsHexDevice ? Addreess.ToString("X") : Addreess.ToString();
        public Option<int> BitwiseRange { get; private set; }  //K2X0, K4X0, K8X0 Bit 디바이스를 byte 단위로 묶음

        /// <summary>
        /// BitwiseRange 를 갖는 device 는 직접 읽을 수 없으므로, 이를 읽기 위해서 새로 도입한 tag.
        /// e.g this 가 "K4X31"  tag 라면, "X31" 이 Alias 가 된다.
        /// </summary>
        public List<MxTag> Aliases { get; } = new List<MxTag>();

        /// <summary>
        /// this tag 가 다른 bitwise range tag 의 값을 얻어오기 위해서 생성된 tag 일때 true 값을 가짐.
        /// 그렇다고 하더라도, 실제로 this tag 가 symbol table 에 존재해서 독립적으로도 사용되면 false 값을 가짐
        /// </summary>
        public bool IsHelperTag { get; private set; }
        public Option<int> WordDotBit { get; private set; }  //D100.F, WFF.9  Word 디바이스를 bit 단위로 처리

        public bool IsBitDevice { get { return DeviceType.IsBitDevice(); } }
        public bool IsHexDevice { get { return DeviceType.IsHexDevice(); } }
        public bool IsWordDevice { get { return DeviceType.IsWordDevice(); } }

        /// <summary>
        /// e.g "K4X3" 에서 bitwise range specifier("K4") 를 제거하고 남은 부분의 이름.  => "X3"
        /// </summary>
        public Option<string> SubName { get; private set; } = Option<string>.None;
        public sealed override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                PlcDeviceType deviceType;
                int address;
                string nameFilter = Name;
                if (Name.Substring(0,1).ToUpper() == "K")  
                {
                    BitwiseRange = Convert.ToInt32(Name.Substring(1, 1));
                    nameFilter = Name.Substring(2);
                    SubName = nameFilter;
                }
                else if(Name.Contains('.')) //K4X00.0 BitArrary이면서 WordPoint 타입은 없음
                {
                    WordDotBit = Convert.ToInt32(Name.Split('.')[1], 0x0010); //hexa bit
                    nameFilter = Name.Split('.')[0];
                    SubName = nameFilter;
                }

                McProtocolApp.GetDeviceCode(nameFilter, out deviceType, out address);
                DeviceType = deviceType;
                Addreess = address;
	            if (IsBitAddress)
				{
					ByteOffset = Addreess / 8;
					BitOffset = Addreess%8;
				}
				else
					ByteOffset = Addreess * 2;
			}
		}

	    public override string ToString() => Name;

	    public override bool IsBitAddress {get { return IsBitDevice;} protected internal set {throw new WillNotBeReimplementedException("Do not call me");} }

        private MxTag(MxConnection connection, string name, bool isHelperTag)
            : base(connection)
        {
            IsHelperTag = isHelperTag;
            Name = name;
            //Trace.WriteLine($"Creating tag {name}");
            connection?.AddMonitoringTag(this);
        }

        public static MxTag Create(MxConnection connection, string name, bool isHelperTag=false)
        {
            if (connection == null)
                return new MxTag(null, name, isHelperTag);

            if (connection.Tags.ContainsKey(name))
            {
                var tag = connection.Tags[name] as MxTag;
                if (!isHelperTag)
                    tag.IsHelperTag = isHelperTag;

                return tag;
            }

            return new MxTag(connection, name, isHelperTag);
        }


        public byte[] GetDeviceDesignation(int packAlignBit = 0)
        {
			var address = Addreess;
			if (packAlignBit != 0 && IsBitDevice)
				address = Addreess - (Addreess%packAlignBit);

            return BitConverter.GetBytes(address).Take(3)
                .Concat(new byte[] {(byte) DeviceType}).ToArray();
        }

		/// <summary> /// Read request 를 위해서 한꺼번에 읽을 수 있는 tag 들끼리 grouping 한다. /// </summary>
		public static IEnumerable<IEnumerable<MxTag>> ChannelizeTags(IEnumerable<MxTag> tags)
	    {
			var typeGroup = tags.GroupBy(t => t.DeviceType);
			foreach (var g in typeGroup)
				yield return g.ToList();
		}
	}
}
