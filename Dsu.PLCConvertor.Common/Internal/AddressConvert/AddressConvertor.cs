using Dsu.Common.Utilities.ExtensionMethods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    public partial class AddressConvertor
    {
        List<IAddressConvertRule> _normalRules;
        internal Dictionary<string, NamedAddressConvertRule> _namedAddressRules;

        
        List<IAddressConvertRule> _rules;
        
        [JsonProperty()]
        internal List<IAddressConvertRule> Rules
        {
            get { return _rules; }
            set
            {
                _rules = value;
                _normalRules = value.OfNotType<IAddressConvertRule, NamedAddressConvertRule>().ToList();
                _namedAddressRules = value.OfType<NamedAddressConvertRule>().ToDictionary(r => r.Name);
            }
        }

        [JsonConstructor]
        private AddressConvertor() { }

        public AddressConvertor(IEnumerable<IAddressConvertRule> rules)
        {
            Rules = rules.ToList();
        }

        public bool IsMatch(string sourceAddress) => _normalRules.Any(r => r.IsMatch(sourceAddress));
        public string Convert(string sourceAddress) => IsMatch(sourceAddress) ? convert(sourceAddress, _normalRules) : sourceAddress;
        public string ConvertWithNamedRule(string ruleName, string sourceAddress)
            => convert(sourceAddress, new[] { _namedAddressRules[ruleName] });

        string convert(string sourceAddress, IEnumerable<IAddressConvertRule> rules)
        {
            var matchedRules = rules.Where(r => r.IsMatch(sourceAddress)).ToArray();
            if (matchedRules.Length != 1)
                throw new ConvertorException($"Total {matchedRules.Length} rules matched for [{sourceAddress}].");

            return matchedRules[0].Convert(sourceAddress);
        }


        public IEnumerable<string> GenerateAllSourceSamples() => Rules.SelectMany(r => r.GenerateSourceSamples());
        public IEnumerable<(string, string)> GenerateTranslations() => Rules.SelectMany(r => r.GenerateTranslations());

        public static AddressConvertor LoadFromJsonFile(string jsonFile)
            => AddressConvertorSerializer.LoadFromJsonString(File.ReadAllText(jsonFile)).Validate();

        public static AddressConvertor LoadFromJsonString(string json)
            => AddressConvertorSerializer.LoadFromJsonString(json).Validate();
        public void SaveToJsonFile(string jsonFile) => AddressConvertorSerializer.SaveToJsonFile(this, jsonFile);


        /// <summary>
        /// Address 변환 rule 적합성 검사.   중복 rule 존재하는지 체크
        /// </summary>
        public AddressConvertor Validate()
        {
            try
            {
                Global.UIMessageSubject.OnNext("주소 변환 규칙 적합성 체크..");
                var hash = new HashSet<string>();
                GenerateAllSourceSamples()
                    .Iter(s =>
                    {
                        if (hash.Contains(s))
                            throw new ConvertorException($"Symbol {s} duplicated.");

                        hash.Add(s);
                    });

                return this;
            }
            finally
            {
                Global.UIMessageSubject.OnNext("");
            }
        }
    }


#if DEBUG
    public static class AddressConvertorTester
    {
        public static void Test()
		{
            AddressConvertRule.Test();
            var rule = new AddressConvertRule(
                "(%d).(%2d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 15) },
                "P(%04d)(%x)", new[] { "$0 * 1000", "$1" });

            var samples = rule.GenerateSourceSamples().ToArray();
            var converted = samples.Select(s => $"{s} => {rule.Convert(s)}").ToArray();

            var defaultRules = Cx2Xg5k.CreateDefaultAddressConvertRuleSets();
            defaultRules.GenerateTranslations()
                .Select(pr => $"{pr.Item1}\t{pr.Item2}")
                .Iter(ln => Trace.WriteLine(ln))
                ;

            var jsonFile = "defaultAddressMapping.json";
            defaultRules.SaveToJsonFile(jsonFile);

            var dup = AddressConvertor.LoadFromJsonFile(jsonFile);
            var namedTimer = dup._namedAddressRules["TIMER"];
            var gen = namedTimer.GenerateSourceSamples().ToArray();
            dup.GenerateTranslations()
                .Select(pr => $"{pr.Item1}\t{pr.Item2}")
                .Iter(ln => Trace.WriteLine(ln))
                ;
        }
    }
#endif
}
