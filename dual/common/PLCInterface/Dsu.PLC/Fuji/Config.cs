using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using LanguageExt;


namespace Dsu.PLC.Fuji
{
    /// <summary>
	/// Fuji PLC 의 ioarea.init 파일의 config section 하나에 해당하는 class
	/// </summary>
	internal class Config
	{
		/// <summary>
		/// Config name.  e.g "Config", "Config.1", "Config.2", etc
		/// </summary>
		public string Name { get; private set; }


		/// <summary>
		/// Slots.  Config section 에 포함된 slot 혹은 remote card 구성을 표현하는 Config
		/// Top level config ("Config") 는 slot / config 를 포함할 수 있으나, 
		/// Sub level config (e.g "Config.1") 은 slot 만 포함한다.
		/// </summary>
		EitherLazy<SlotInfo, Config>[] _slots = new EitherLazy<SlotInfo, Config>[256];

		public Config(ref int startByteOffset, FjHwConfig.IOAreaSection sec)
		{
			ParseSection(ref startByteOffset, sec, null);
		}

		public Config(ref int startByteOffset, FjHwConfig.IOAreaSection topSection, IEnumerable<FjHwConfig.IOAreaSection> subSections)
		{
			ParseSection(ref startByteOffset, topSection, subSections);
		}

		/// <summary>
		/// ioarea.init 파일 정보를 토대로 생성가능한 모든 I/O tag 이름을 word 단위로 생성한다.
		/// </summary>
		/// <param name="link"></param>
		/// <returns></returns>
		public IEnumerable<string> EnumerateValidInputOutputTags(int? link=null)
		{
			for (int s = 0; s < _slots.Length; s++)
			{
				var slot = _slots[s];
				if ( slot != null)
				{
					if (slot.IsLeft())
					{
						SlotInfo si = slot.Left();
						var io = si.IsInput ? "I" : "Q";
						for (int w = 0; w < si.WordLength; w++)
						{
							if (link.HasValue)
								yield return $"%{io}W{link.Value}.{s}.{w}";
							else
								yield return $"%{io}W{s}.{w}";
						}
					}
					else
					{
						foreach (var tag in slot.Right().EnumerateValidInputOutputTags(s))
							yield return tag;						
					}
				}
			}
		}


		/// <summary>
		/// 주어진 slotNumber 의 IO 카드 정보(SlotInfo) 를 반환한다.
		/// </summary>
		/// <param name="slotNumber">slot 번호</param>
		/// <returns></returns>
		public SlotInfo GetStartWordOffset(int slotNumber)
		{
			Debug.Assert(_slots[slotNumber].IsLeft());
			return _slots[slotNumber].Left();
		}

		/// <summary>
		/// 주어진 slotNumber 에 해당하는 remote card 의 subSlotNumber slot 정보를 반환한다.
		/// </summary>
		/// <param name="slotNumber">slot 번호</param>
		/// <param name="subSlotNumber">remote CPU 의 slot 번호</param>
		/// <returns></returns>
		public SlotInfo GetStartWordOffset(int slotNumber, int subSlotNumber)
		{
			Debug.Assert(_slots[slotNumber].IsRight());
			return _slots[slotNumber].Right().GetStartWordOffset(subSlotNumber);
		}

		/// <summary>
		/// 주어진 slotNumber 이 차지하는 memory 영역 크기를 반환한다.
		/// IO 카드(SlotInfo, Left) 이면 해당 IO 카드의 word 접점수를 반환하고,
		/// Remote card (Config, right)이면 remote configuration 의 모든 slot 접점수의 합을 반환한다.
		/// </summary>
		/// <param name="slotNumber"></param>
		/// <returns></returns>
		private int GetSlotWordLength(int slotNumber)
		{
			return _slots[slotNumber].Match(
				r => r._slots.Where(c => c != null).Sum(c => c.Left().WordLength),
				l => l.WordLength
			)();
		}

		/// <summary>
		/// ioarea.init 의 section 구조를 parsing 하여 Config instance 를 생성한다.
		/// </summary>
		/// <param name="startByteOffset"></param>
		/// <param name="sec"></param>
		/// <param name="subSections"></param>
		private void ParseSection(ref int startByteOffset, FjHwConfig.IOAreaSection sec, IEnumerable<FjHwConfig.IOAreaSection> subSections)
		{
			bool topLevel = subSections.NonNullAny();
			Name = sec.SectionName;

			foreach (var pr in sec.Dictionary.Where(e => Regex.IsMatch(e.Key, @"\d+")))
			{
				var slotNumber = Int32.Parse(pr.Key);
				var match = Regex.Match(pr.Value, @"([^ ]*) (@[^ ]*) (.*)");
				if (match.Groups.Count != 4)
					throw new UnexpectedCaseOccurredException($"Invalid format :'{pr.Value}'");

				var slotName = match.Groups[1].ToString();      // "DC_Input_64points" or something
				var type = match.Groups[2].ToString();          // {"@IO", "@LNK"}
				var tail = match.Groups[3].ToString();          // "IW 4", or "Config.1"

				if (type == "@IO")
				{
					var tailTokens = tail.Split(new char[] {' '});
					var numBytes = 2 * Int32.Parse(tailTokens[1]);

					bool isInput = tail.Contains("IW");
					Debug.Assert(isInput || tail.Contains("OW"));

					var slotInfo = new SlotInfo(slotName, isInput, startByteOffset, numBytes);
					startByteOffset += numBytes;
					_slots[slotNumber] = () => slotInfo;
				}
				else if (type == "@LNK")
				{
					Debug.Assert(topLevel);
					var subconfig = new Config(ref startByteOffset, subSections.Where(c => c.SectionName == tail).First());
					_slots[slotNumber] = () => subconfig;
				}

				// 후지 PLC 에서는 top level config("Config")에서 모든 slot 은 짝수의 word number (4의 배수 byte) 로 padding 하여 사용한다.
				if (topLevel)
				{
					var slotWordLength = GetSlotWordLength(slotNumber);
					Trace.WriteLine($"SlotWordLength [{slotNumber}] = {slotWordLength}");
					if (slotWordLength % 2 == 1)
						startByteOffset += 2;
				}
			}
		}
	}
}
