using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dsu.PLCConvertor.Common.Internal.AddressConvertorSerializer;

namespace Dsu.PLCConvertor.Common.Internal
{

    internal class TemporaryAddressSearchResult
    {
        /// <summary>
        /// Allocated Temporary Address
        /// </summary>
        public string Temporary { get; set; }
        public IEnumerable<string> PrologRungILs { get; set; } = Enumerable.Empty<string>();
        public TemporaryAddressSearchResult(string tempDevice)
        {
            Temporary = tempDevice;
        }
        public TemporaryAddressSearchResult(string tempDevice, IEnumerable<string> mnemonics)
        {
            Temporary = tempDevice;
            PrologRungILs = mnemonics;
        }
    }
    /// <summary>
    /// ONS : 
    /// 변환 중에 필요한 임시 디바이스 영역 할당자
    /// 산전 PLC 의 device 영역이 할당되어야 한다.
    /// NamedAddressConvertRule 와 기능이 비슷해서 이를 차용해서 쓴다.  (실제 변환의 의미는 아니다)
    /// 
    /// %ANDNOT 의 경우, 산전의 ANDP + NOT 의 기능이 합쳐진 것으로, 산전에 대응 명령어가 없다.
    /// rung 에서 이러한 명령어를 만난 경우, (e.g %ANDNOT L1000),
    ///  - 해당 rung 이전에 LOADP를 통해 OSR 을 temporary 변수에 저장하는 명령을 삽입하고 (e.g LOADP L1000; OUT TMPVAR; )
    ///  - 해당 rung 변환 부분에서는 ANDNOT TMPVAR 로 구현한다.
    /// 
    /// </summary>
    internal class TemporaryAddressAllocator
    {
        Dictionary<string, NamedAddressConvertRule> _ruleSet;
        /// <summary>
        /// e.g: "
        /// </summary>
        Dictionary<string, string> _cache = new Dictionary<string, string>();

        Dictionary<string, IEnumerator<string>> _generators;
        public TemporaryAddressAllocator(IEnumerable<NamedAddressConvertRule> rules)
        {
            _ruleSet = rules.ToDictionary(r => r.Name);
            _generators =
                rules.Select(r => new { Rule = r, Enumerator = r.GenerateSourceSamples().GetEnumerator() })
                .ToDictionary(pr => pr.Rule.Name, pr => pr.Enumerator)
                ;
        }

        public static TemporaryAddressAllocator TheInstance { get; private set; }
        static TemporaryAddressAllocator()
        {
            TheInstance = LoadFromJsonFile(ConfigurationManager.AppSettings["temporaryAddressAllocatorFile"]);
        }

        public static TemporaryAddressAllocator Dup() => new TemporaryAddressAllocator(TheInstance._ruleSet.Values);


        public TemporaryAddressSearchResult Allocate(string tempAllocatorName, ILSentence sentence, string device) // e.g : device = "%0.01"
        {
            if (_cache.ContainsKey(device))
                return new TemporaryAddressSearchResult(_cache[device]);

            var gen = _generators[tempAllocatorName];
            if (!gen.MoveNext())
                throw new ConvertorException($"No more availabe temp device. {tempAllocatorName}");

            var temp = gen.Current;
            var NP = device[0] == '@' ? 'P' : 'N';
            var address = device.Substring(1, device.Length - 1);
            var ss = sentence._sourceILSentence;
            var prologRungs = new[]
            {
                $"CMT\t{Cx2Xg5kOption.LabelHeader} 명령어({ss.Sentence}) 를 위한 임시 변수({temp}) 설정 ",
                $"LOAD{NP}\t{address}",
                $"OUT\t{temp}",
            };

            _cache.Add(device, temp);

            return new TemporaryAddressSearchResult(temp, prologRungs);
        }

        public static TemporaryAddressAllocator LoadFromJsonFile(string jsonFile)
            => LoadFromJsonString(File.ReadAllText(jsonFile));
        public static TemporaryAddressAllocator LoadFromJsonString(string json)
        {
            // serialization 용 rule 을 file 에서 loading 한 후, 일반 rule 로 변경해서 반환
            var rules = JsonConvert.DeserializeObject<TASRule[]>(json, MyJsonSerializer.JsonSettingsSimple);
            var normalRules = rules.Select(r => r.ToNormalRule() as NamedAddressConvertRule);
            return new TemporaryAddressAllocator(normalRules);
        }

        public void SaveToJsonFile(string jsonFile)
        {
            // 일반 rule 을 serialization 용 rule 로 변경 후, serialize
            var serializer = _ruleSet.Values.Select(r => TASRule.Create(r));
            var json = JsonConvert.SerializeObject(serializer, MyJsonSerializer.JsonSettingsSimple);
            File.WriteAllText(jsonFile, json);
        }

        /// <summary>
        /// Temprary Address Rule for Serialization
        /// </summary>
        class TASRule
        {
            public string Name { get; set; }
            public string Pattern { get; set; }
            public MinMax[] MinMax { get; set; }
            public string[] Expr { get; internal set; }     // TargetArgsExpr
            public NamedAddressConvertRule ToNormalRule()
            {
                var minMax = MinMax.Select(mm => Tuple.Create(mm.Min, mm.Max));
                return new NamedAddressConvertRule(Name, Pattern, minMax, null, null);
            }

            public static TASRule Create(NamedAddressConvertRule r)
                => new TASRule()
                {
                    Name = r.Name,
                    Pattern = r.SourceRepr,
                    MinMax = r.SourceArgsMinMax.Select(tpl => new MinMax(tpl)).ToArray(),
                };

        }
        public static void Test()
        {
            //var r1 = new NamedAddressConvertRule(
            //        "LB",
            //        "(%d).(%2d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 15) },
            //        null, new string[] {});

            //var xx = r1.GenerateSourceSamples().Take(20).ToArray();
            //Console.WriteLine("");

            //var rule = new TemporaryAddressAllocator(r1);
            //rule.SaveToJsonFile("test.temp.json");

            ////var rules = new [] {
            ////    new NamedAddressConvertRule(
            ////        "L",
            ////        "L(%04d)", new[] { Tuple.Create(0, 99) },
            ////        "P(%04d)(%x)", new[] { "$0 * 1000", "$1" }),
            ////    new NamedAddressConvertRule(
            ////        "LB",
            ////        "(%d).(%2d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 15) },
            ////        "P(%04d)(%x)", new[] { "$0 * 1000", "$1" }),

        }

        internal TemporaryAddressAllocator LoadFromJsonFile(object p)
        {
            throw new ConvertorException("Not implemented");
        }
    }
}
