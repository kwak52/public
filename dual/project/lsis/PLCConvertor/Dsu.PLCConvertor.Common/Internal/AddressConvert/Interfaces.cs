using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// Address Convert Rule interface
    /// </summary>
    public interface IAddressConvertRule
    {
        string SourceRepr { get; }
        string TargetRepr { get; }
        bool IsMatch(string sourceAddress);
        string Convert(string sourceAddress);
        IEnumerable<string> GenerateSourceSamples();
        IEnumerable<(string, string)> GenerateTranslations();
    }

    /// <summary>
    /// Address Convert Rule interface : 특수 relay.  e.g P_On.
    /// 1:1 대응 관계
    /// </summary>
    public interface IACRSpecialRelay : IAddressConvertRule { }

    /// <summary>
    /// Address Convert Rule interface : format 을 지정할 수 있는 rule.
    /// </summary>
    public interface IACRFormatSpecifiable : IAddressConvertRule
    {
    }


    /// <summary>
    /// Address Convert Rule interface : 사용자에게 open 할 수 있는 rule.
    /// </summary>
    public interface IACRUserCustomizable : IAddressConvertRule { }
}
