using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;
using Dsu.PLC.Utilities;
using LanguageExt;

namespace Dsu.PLC.Fuji
{
	/// <summary>
	/// Input/Output tag
	/// %IW{SX Bus Unit No}.{Word No}
	/// %IX{SX Bus Unit No}.{Word No}.{Bit No}
	/// 
	/// %IW{Link No}.{SX Bus Unit No}.{Word No}
	/// %IX{Link No}.{SX Bus Unit No}.{Word No}.{Bit No}
	/// </summary>
	public class FjTagIO : FjTag
	{
		private string[] _addressPatternsIOWord = new[]
		{
			@"^%([IQ]W)([0-9]+)$",                      // 0 dot
			@"^%([IQ]W)([0-9]+).([0-9]+)$",             // 1 dot
			@"^%([IQ]W)([0-9]+).([0-9]+).([0-9]+)$",    // 2 dot
		};

		private string[] _addressPatternsIOBit = new[]
		{
			null,
			@"^%([IQ]X)([0-9]+).([0-9]+)$",                     // 1 dot
			@"^%([IQ]X)([0-9]+).([0-9]+).([0-9]+)$",            // 2 dot
			@"^%([IQ]X)([0-9]+).([0-9]+).([0-9]+).([0-9]+)$",   // 3 dot
		};

		private Option<int> _busNumber;
		private Option<int> _linkNumber;

		public Option<int> BusUnitNumber
		{
			get { return _busNumber; }
			private set
			{
				if (value.IsSome && !value.GetValueUnsafe().InRange(0, 238))		// 문서 상에는 [1..238]
					throw new PlcExceptionTag($"Invalid SX bus number: {value.GetValueUnsafe()}");
				_busNumber = value;
			}
		}

		public Option<int> LinkNumber
		{
			get { return _linkNumber; }
			private set
			{
				if (value.IsSome && !value.GetValueUnsafe().InRange(1, 238))
					throw new PlcExceptionTag($"Invalid link number: {value.GetValueUnsafe()}");
				_linkNumber = value;
			}
		}


		public override Option<int> UnitNumber { get { return BusUnitNumber; } }


		private int? _byteOffset = null;
		public override Option<int> ByteOffset
		{
			get
			{
				if (_byteOffset == null)
				{
					SlotInfo slotInfo = null;

					if (BusUnitNumber.IsSome)
					{
						if (LinkNumber.IsSome)
							slotInfo = Connection.Config.GetStartWordOffset(LinkNumber.GetValueUnsafe(), BusUnitNumber.GetValueUnsafe());
						else
							slotInfo = Connection.Config.GetStartWordOffset(BusUnitNumber.GetValueUnsafe());
					}

					if (Address >= slotInfo.ByteLength)
						throw new PlcExceptionTag($"Address out of range : {Name}");

					if (slotInfo.IsInput != Name.StartsWith("%I"))
						throw new PlcExceptionTag($"Input/Output mismatch: {Name}");

					_byteOffset = slotInfo.StartByteOffset + 2 * Address;
				}

				return _byteOffset.Value;
			}
		}


		public FjTagIO(FjConnection connection, string name)
			: base(connection, name)
		{
			Contract.Requires(Regex.IsMatch(name, @"^%[IQ][XW].*$"));

			MemoryType = MemoryType.InputOutputMemory;

		}

		protected override void ConstructionEpilogue()
		{
			Contract.Requires(NumDots == 1 || NumDots == 2 || NumDots == 3);
			var pattern = IsBitAddress ? _addressPatternsIOBit : _addressPatternsIOWord;
			Tokens = Name.ParseAgainstRegex(pattern[NumDots]);
			Debug.Assert(IsBitAddress == (Tokens[0][1] == 'X'));     // Tokens[0] = One of {"IW", "IX", "QW", "QX"}

			int i = 1;
			bool hasLinkNumberSpec = (IsBitAddress && NumDots == 3) || (!IsBitAddress && NumDots == 2);
			bool hasBusNumberSpec = hasLinkNumberSpec || (IsBitAddress && NumDots == 2) || (!IsBitAddress && NumDots == 1);
			if (hasLinkNumberSpec)
				LinkNumber = Int32.Parse(Tokens[i++]);

			if (hasBusNumberSpec)
				BusUnitNumber = Int32.Parse(Tokens[i++]);

			Address = Int32.Parse(Tokens[i++]);
			if (IsBitAddress)
				BitOffset = Int32.Parse(Tokens[i++]);
		}
	}
}