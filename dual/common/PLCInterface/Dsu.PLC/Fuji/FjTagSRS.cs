using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Dsu.PLC.Fuji
{
	/// <summary>
	/// Standard/Retain/System memory tag
	/// %MX{Cpu No}.{Memory Classification No}.{Word No}.{Bit No}
	/// %MW{Cpu No}.{Memory Classification No}.{Word No}
	/// </summary>
	public class FjTagSRS : FjTag
	{
		private string[] _addressPatternsSRSBit = new[]
		{
			null,
			null,
			@"^%(MX)([0-9]+).([0-9]+).([0-9]+)$", // 2 dot
			@"^%(MX)([0]?[0-7]).([0-9]+).([0-9]+).([0-9]+)$", // 3 dot
		};
		private string[] _addressPatternsSRSWord = new[]
		{
			null,
			@"^%(MW)([0-9]+).([0-9]+)$", // 1 dot
			@"^%(MW)([0]?[0-7]).([0-9]+).([0-9]+)$", // 2 dot
		};


		private Option<int> _cpuNumber;

		/// <summary>
		/// [0..7]
		/// </summary>
		public Option<int> CpuNumber
		{
			get { return _cpuNumber; }
			private set
			{
				int val = value.IfNone(-1);
				if (value.IsSome && ! val.InRange(0, 7))
					throw new PlcExceptionTag($"Invalid CPU number: {val}");
				_cpuNumber = value;
			}
		}

		public override Option<int> UnitNumber { get { return CpuNumber; } }
		public int MemoryClassificationNumber { get; private set; }		// 1: Standard memory, 3: Retain memory, 10 : System memory

		public FjTagSRS(FjConnection connection, string name)
			: base(connection, name)
		{
			Contract.Requires(Regex.IsMatch(name, @"^%M[XW].*$"));
		}

		protected override void ConstructionEpilogue()
		{
			Contract.Requires(NumDots == 2 || (IsBitAddress && NumDots == 3) || (! IsBitAddress && NumDots == 1));

			if (NumDots != 2 && (! IsBitAddress || NumDots != 3) && (IsBitAddress || NumDots != 1))
				throw new PlcExceptionTag($"Tag address specification error : {Name}", this);

			var pattern = IsBitAddress ? _addressPatternsSRSBit : _addressPatternsSRSWord;
			Tokens = Name.ParseAgainstRegex(pattern[NumDots]);
			if ( Tokens.IsNullOrEmpty() )
				throw new PlcExceptionTag("Invalid tag format.", this);

			Debug.Assert(IsBitAddress == (Tokens[0][1] == 'X'));     // Tokens[0] = One of {"IW", "IX", "QW", "QX"}


			int i = 1;
			bool hasCpuNumberSpec = (IsBitAddress && NumDots == 3) || (!IsBitAddress && NumDots == 2);
			if (hasCpuNumberSpec)
			{
				CpuNumber = Int32.Parse(Tokens[i++]);
			}

			MemoryClassificationNumber = Int32.Parse(Tokens[i++]);
			if ( ! MemoryClassificationNumber.IsOneOf(1, 3, 10))       // 1:Standard Memory, 3:Retain Memory, 10:System Memory
				throw new PlcExceptionTag($"Invalid memory classification number: {MemoryClassificationNumber}", this);

			switch (MemoryClassificationNumber)
			{
				case 1: MemoryType = MemoryType.StandardMemory; break;
				case 3: MemoryType = MemoryType.RetainMemory; break;
				case 10: MemoryType = MemoryType.SystemMemory; break;
			}


			Address = Int32.Parse(Tokens[i++]);
			if (IsBitAddress)
			{
				try
				{
					var bitOffset = parseInt(Tokens[i]).IfNone(-1);
					if (!bitOffset.InRange(0, 15))
						throw new PlcExceptionTag($"Invalid BitOffset number: {Tokens[i]}", this);
					BitOffset = bitOffset;
				}
				finally { i++; }
			}
		}
	}
}