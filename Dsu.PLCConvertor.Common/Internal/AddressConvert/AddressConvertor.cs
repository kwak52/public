﻿using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class AddressConvertor
    {
        List<IAddressConvertRule> _normalRules;
        Dictionary<string, NamedAddressConvertRule> _namedAddressRules;

        
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
        public string Convert(string sourceAddress) => convert(sourceAddress, _normalRules);
        public string ConvertWithNamedRule(string ruleName, string sourceAddress)
            => convert(sourceAddress, new[] { _namedAddressRules[ruleName] });

        string convert(string sourceAddress, IEnumerable<IAddressConvertRule> rules)
        {
            var matchedRules = rules.Where(r => r.IsMatch(sourceAddress)).ToArray();
            if (matchedRules.Length != 1)
                throw new Exception($"Total {matchedRules.Length} rules matched for [{sourceAddress}].");

            return matchedRules[0].Convert(sourceAddress);
        }


        public IEnumerable<string> GenerateSourceSamples() => Rules.SelectMany(r => r.GenerateSourceSamples());
        public IEnumerable<(string, string)> GenerateTranslations() => Rules.SelectMany(r => r.GenerateTranslations());


        public static AddressConvertor LoadFromJsonFile(string jsonFile) => LoadFromJsonString(File.ReadAllText(jsonFile));
        public static AddressConvertor LoadFromJsonString(string json)
        {
            //return JsonConvert.DeserializeObject<AddressConvertor>(json, MyJsonSerializer.JsonSettings);
            return JsonConvert.DeserializeObject<AddressConvertor>(json, MyJsonSerializer.JsonSettings2);
        }

        public void SaveToJsonFile(string jsonFile)
        {
            //var json = JsonConvert.SerializeObject(this, Formatting.Indented, MyJsonSerializer.JsonSettings);
            var json = JsonConvert.SerializeObject(this, MyJsonSerializer.JsonSettings2);
            File.WriteAllText(jsonFile, json);
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

            Console.WriteLine("");

            var defaultRules = Cx2Xg5k.CreateDefaultAddressConvertRuleSets();
            defaultRules.GenerateTranslations()
                .Select(pr => $"{pr.Item1}\t{pr.Item2}")
                .Iter(ln => Trace.WriteLine(ln))
                ;

            var jsonFile = "defaultAddressMapping.json";
            defaultRules.SaveToJsonFile(jsonFile);

            var dup = AddressConvertor.LoadFromJsonFile(jsonFile);
            Console.WriteLine("");
            dup.GenerateTranslations()
                .Select(pr => $"{pr.Item1}\t{pr.Item2}")
                .Iter(ln => Trace.WriteLine(ln))
                ;
        }
    }
#endif
}
