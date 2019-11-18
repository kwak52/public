using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    public partial class AddressConvertor
    {
        /// <summary>
        /// 주소 변환 규칙을 serialize 하기 위한 내부 class
        /// </summary>
        private class AddressConvertorSerializer
        {
            public AddressConvertRuleSpecialRelay[] OneToOne { get; set; }
            public AddressConvertRule[] Ranged { get; set; }
            public NamedAddressConvertRule[] Named { get; set; }
            public static AddressConvertor LoadFromJsonString(string json)
            {
                var serializer = JsonConvert.DeserializeObject<AddressConvertorSerializer>(json, MyJsonSerializer.JsonSettingsSimple);
                var rules =
                    new [] { serializer.OneToOne.Cast<IAddressConvertRule>(), serializer.Ranged, serializer.Named, }
                    .SelectMany(rs => rs)
                    ;
                return new AddressConvertor(rules);
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
                List<AddressConvertRuleSpecialRelay> _oneToOne = new List<AddressConvertRuleSpecialRelay>();
                List<AddressConvertRule> _ranged = new List<AddressConvertRule>();
                List<NamedAddressConvertRule> _named = new List<NamedAddressConvertRule>();
                foreach ( var r in rules)
                {
                    if (r is AddressConvertRuleSpecialRelay)
                        _oneToOne.Add(r as AddressConvertRuleSpecialRelay);
                    else if (r is NamedAddressConvertRule)
                        _named.Add(r as NamedAddressConvertRule);
                    else
                        _ranged.Add(r as AddressConvertRule);
                }

                OneToOne = _oneToOne.ToArray();
                Ranged = _ranged.ToArray();
                Named = _named.ToArray();
            }
        }
    }
}
