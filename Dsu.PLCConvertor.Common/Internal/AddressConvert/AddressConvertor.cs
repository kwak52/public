using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class AddressConvertor
    {
        List<AddressConvertRule> _normalRules;
        Dictionary<string, NamedAddressConvertRule> _namedAddressRules;
        public AddressConvertor(IEnumerable<AddressConvertRule> rules)
        {
            _normalRules = rules.OfNotType<AddressConvertRule, NamedAddressConvertRule>().ToList();
            _namedAddressRules = rules.OfType<NamedAddressConvertRule>().ToDictionary(r => r.Name);
        }

        public string Convert(string sourceAddress) => convert(sourceAddress, _normalRules);
        public string ConvertWithNamedRule(string ruleName, string sourceAddress) => convert(sourceAddress, new[] { _namedAddressRules[ruleName] });

        public string convert(string sourceAddress, IEnumerable<AddressConvertRule> rules)
        {
            var matchedRules = rules.Where(r => r.IsMatch(sourceAddress)).ToArray();
            if (matchedRules.Length != 1)
                throw new Exception($"Total {matchedRules.Length} rules matched for [{sourceAddress}].");

            return matchedRules[0].Convert(sourceAddress);
        }
    }

    public static class DoTest
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
		}
	}


}
