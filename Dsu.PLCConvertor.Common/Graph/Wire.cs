using Dsu.Common.Utilities.ExtensionMethods;
using System.Diagnostics;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// PLC rung 에서 접점 간 연결을 표현하는 edge class
    /// </summary>

    [DebuggerDisplay("{Output}:{Comment}")]
    public class Wire
    {
        public string Output { get; internal set; }
        public string Comment { get; internal set; }
        public Wire(ILSentence sentence)           
        {
            Comment = sentence.ToString();
        }
        public Wire() {}
        public Wire(string output)
        {
            Output = output;
        }
        public override string ToString()
        {
            return Output.NonNullEmptySelector(Comment);
        }
    }
}
