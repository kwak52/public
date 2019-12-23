using System.Collections.Generic;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class AddressConvertRuleSpecialRelay
        : AddressConvertRuleBase
        , IACRSpecialRelay
    {
        public override string Convert(string sourceAddress) => TargetRepr;
        public override bool IsMatch(string sourceAddress) => SourceRepr == sourceAddress;

        public override IEnumerable<string> GenerateSourceSamples()
        {
            yield return SourceRepr;
        }
        public override IEnumerable<(string, string)> GenerateTranslations()
        {
            yield return (SourceRepr, TargetRepr);
        }

        public AddressConvertRuleSpecialRelay(string sourceRepr, string targetRepr)
            : base(sourceRepr, targetRepr)
        {
        }
    }
}
