using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dsu.PLCConvertor.Common.Internal.AddressConvertorSerializer;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 변환 중에 필요한 임시 디바이스 영역 할당자
    /// 산전 PLC 의 device 영역이 할당되어야 한다.
    /// NamedAddressConvertRule 와 기능이 비슷해서 이를 차용해서 쓴다.  (실제 변환의 의미는 아니다)
    /// </summary>
    public class TemporaryAddressAllocator
    {
        Dictionary<string, NamedAddressConvertRule> _ruleDic;
        public TemporaryAddressAllocator(IEnumerable<NamedAddressConvertRule> rules)
        {
            _ruleDic = rules.ToDictionary(r => r.Name);
        }
        public static TemporaryAddressAllocator LoadFromJsonString(string json)
        {
            // serialization 용 rule 을 file 에서 loading 한 후, 일반 rule 로 변경해서 반환
            var rules = JsonConvert.DeserializeObject<TASRule[]>(json, MyJsonSerializer.JsonSettingsSimple);
            return new TemporaryAddressAllocator(rules.Select(r => r.ToNormalRule()).Cast<NamedAddressConvertRule>());
        }

        public void SaveToJsonFile(string jsonFile)
        {
            // 일반 rule 을 serialization 용 rule 로 변경 후, serialize
            var serializer = _ruleDic.Values.Select(r => TASRule.Create(r)).ToArray();
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
            var r1 = new NamedAddressConvertRule(
                    "LB",
                    "(%d).(%2d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 15) },
                    "P(%04d)(%x)", new[] { "$0 * 1000", "$1" });

            var xx = r1.GenerateSourceSamples().Take(20).ToArray();
            Console.WriteLine("");

            var ruleset = new TemporaryAddressAllocator(new[] { r1 });
            ruleset.SaveToJsonFile("test.temp.json");

            //var rules = new [] {
            //    new NamedAddressConvertRule(
            //        "L",
            //        "L(%04d)", new[] { Tuple.Create(0, 99) },
            //        "P(%04d)(%x)", new[] { "$0 * 1000", "$1" }),
            //    new NamedAddressConvertRule(
            //        "LB",
            //        "(%d).(%2d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 15) },
            //        "P(%04d)(%x)", new[] { "$0 * 1000", "$1" }),

        }
    }
}
