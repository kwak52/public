using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    internal interface IRuleSerializer
    {
        IAddressConvertRule ToNormalRule();
    }

    /// <summary>
    /// 주소 변환 규칙을 serialize 하기 위한 내부 class
    /// </summary>
    class AddressConvertorSerializer
    {
        /// <summary>
        /// 1:1 대응 규칙
        /// </summary>
        public class OneToOneRule : IRuleSerializer
        {
            public string From { get; set; }
            public string To { get; set; }

            public virtual IAddressConvertRule ToNormalRule()
            {
                return new AddressConvertRuleSpecialRelay(From, To);
            }

            public OneToOneRule(AddressConvertRuleSpecialRelay rule)
            {
                From = rule.SourceRepr;
                To = rule.SourceRepr;
            }

            [JsonConstructor] private OneToOneRule() {}
        }

        public class MinMax
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public MinMax(Tuple<int, int> tpl)
            {
                Min = tpl.Item1;
                Max = tpl.Item2;
            }
            [JsonConstructor] private MinMax() { }
        }
        public class RangedRule : IRuleSerializer
        {
            [JsonProperty(Order=1)] public string SourceRepr { get; set; }

            [JsonProperty(Order=2)] public string TargetRepr { get; set; }

            [JsonProperty(Order=3)] public MinMax[] SourceArgsMinMax { get; set; }

            [JsonProperty(Order=4)] public string[] TargetArgsExpr { get; set; }

            public IEnumerable<Tuple<int, int>> ToEnumerableMinMax()
            {
                return SourceArgsMinMax.Select(mm => Tuple.Create(mm.Min, mm.Max));
            }

            public virtual IAddressConvertRule ToNormalRule()
            {
                return new AddressConvertRule(SourceRepr, ToEnumerableMinMax(), TargetRepr, TargetArgsExpr);
            }

            public RangedRule(AddressConvertRule rule)
            {
                SourceRepr = rule.SourceRepr;
                TargetRepr = rule.TargetRepr;
                SourceArgsMinMax = rule.SourceArgsMinMax.Select(tpl => new MinMax(tpl)).ToArray();
                TargetArgsExpr = rule.TargetArgsExpr;
            }
            [JsonConstructor] protected RangedRule() { }
        }

        public class NamedRule : RangedRule
        {
            public string Name { get; set; }
            public override IAddressConvertRule ToNormalRule()
            {
                return new NamedAddressConvertRule(Name, SourceRepr, ToEnumerableMinMax(), TargetRepr, TargetArgsExpr);
            }
            public NamedRule(NamedAddressConvertRule nr)
                : base(nr)
            {
                Name = nr.Name;
            }
            [JsonConstructor] protected NamedRule() { }
        }


        public OneToOneRule[] OneToOne { get; set; }
        public RangedRule[] Ranged { get; set; }
        public NamedRule[] Named { get; set; }
        public static AddressConvertor LoadFromJsonString(string json)
        {
            var serializer = JsonConvert.DeserializeObject<AddressConvertorSerializer>(json, MyJsonSerializer.JsonSettingsSimple);
            var rules =
                new[] { serializer.OneToOne.Cast<IRuleSerializer>(), serializer.Ranged, serializer.Named, }
                .SelectMany(rs => rs)
                ;
            return new AddressConvertor(rules.Select(r => r.ToNormalRule()));
        }
        public static void SaveToJsonFile(AddressConvertor convertor, string jsonFile)
        {
            var serializer = new AddressConvertorSerializer(convertor.Rules);
            var json = JsonConvert.SerializeObject(serializer, MyJsonSerializer.JsonSettingsSimple);
            File.WriteAllText(jsonFile, json);
        }

        [JsonConstructor]
        private AddressConvertorSerializer() { }
        public AddressConvertorSerializer(IEnumerable<IAddressConvertRule> rules)
        {
            List<OneToOneRule> _oneToOne = new List<OneToOneRule>();
            List<RangedRule> _ranged = new List<RangedRule>();
            List<NamedRule> _named = new List<NamedRule>();
            foreach (var r in rules)
            {
                var serialRule = r.ToSerializeRule();
                if (r is AddressConvertRuleSpecialRelay)
                    _oneToOne.Add(serialRule as OneToOneRule);
                else if (r is NamedAddressConvertRule)
                    _named.Add(serialRule as NamedRule);
                else
                    _ranged.Add(serialRule as RangedRule);
            }

            OneToOne = _oneToOne.ToArray();
            Ranged = _ranged.ToArray();
            Named = _named.ToArray();
        }
    }
    static class EmAddressConverterHelper
    {
        public static IAddressConvertRule ToNormalRule(this IRuleSerializer serializer) => serializer.ToNormalRule();
        public static IRuleSerializer ToSerializeRule(this IAddressConvertRule rule)
        {
            switch (rule)
            {
                case NamedAddressConvertRule nr:
                    return new AddressConvertorSerializer.NamedRule(nr);
                case AddressConvertRule cr:
                    return new AddressConvertorSerializer.RangedRule(cr);
                case AddressConvertRuleSpecialRelay sr:
                    return new AddressConvertorSerializer.OneToOneRule(sr);
                default:
                    return null;
            }
        }
    }

}
