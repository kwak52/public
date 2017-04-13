using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.PLC.Fuji
{
	/// <summary>
	/// Fuji PLC H/W Configuration : I/O 카드 설정에 따른 tag 주소 handling
	/// </summary>
	public class FjHwConfig
	{
		internal Config Config { get; private set; }

		/// <summary>
		/// ioarea.init 파일을 파싱한 low level 구조체.   Config 구조체를 만들기위한 중간 과정
		/// high level 은 Config 클래스 참고.
		/// </summary>
		internal class IOAreaSection
		{
			public string SectionName { get; private set; }
			public Dictionary<string, string> Dictionary { get; } = new Dictionary<string, string>();
			public IOAreaSection(string sectionName)
			{
				SectionName = sectionName;
			}

			public void Add(string key, string value) => Dictionary.Add(key, value);

			public string this[string key] // indexer
			{
				get { return Dictionary[key]; }
				set { Dictionary[key] = value; }
			}
		}

		private List<IOAreaSection> _ioAreaSections = new List<IOAreaSection>();
		public FjHwConfig(string ioAreaInitFilePath)
		{
			ParseIOAreaFile(ioAreaInitFilePath);
		}

		/// <summary>
		/// Fuji PLC 의 ioarea.init 파일을 parsing 한다.
		/// </summary>
		private void ParseIOAreaFile(string ioAreaInitFilePath)
		{
			// http://stackoverflow.com/questions/25293104/how-can-i-split-a-textfile-by-three-empty-lines
			string[] groups = Regex.Split(File.ReadAllText(ioAreaInitFilePath, Encoding.GetEncoding(932)), @"(?:\r?\n){2}");

			foreach (var grp in groups)
			{
				var lines = Regex.Split(grp, @"(?:\r?\n)").Where(l => l.NonNullAny());
				if (lines.Count() <= 1)
					continue;

				var sectionName = lines.First().Strip("[", "]");
				if ( ! sectionName.Contains("Config"))
					continue;

				var section = new IOAreaSection(sectionName);
				foreach (var line in lines.Skip(1).Where(l => !l.Contains("@Not")))
				{
					var match = Regex.Match(line, "([^=]*)=(.*)");
					if (match.Groups.Count != 3)
						throw new Exception($"{ioAreaInitFilePath} parsing error: {line}");
					section.Add(match.Groups[1].ToString(), match.Groups[2].ToString());
				}

				_ioAreaSections.Add(section);
			}

			Console.WriteLine(_ioAreaSections.Count);

			foreach (var section in _ioAreaSections)
			{
				Trace.WriteLine($"Section {section.SectionName}");
				foreach (var pr in section.Dictionary)
					Trace.WriteLine($"\t{pr.Key} = {pr.Value}");
			}

			BuildConfig();
		}


		private void BuildConfig()
		{
			var topSection = _ioAreaSections.Where(s => s.SectionName == "Config").First();
			var subSections = _ioAreaSections.Where(s => s.SectionName.StartsWith("Config."));      // Config.1, Config.2, etc

			int startByteOffset = 0;
			Config = new Config(ref startByteOffset, topSection, subSections);
		}
	}
}
