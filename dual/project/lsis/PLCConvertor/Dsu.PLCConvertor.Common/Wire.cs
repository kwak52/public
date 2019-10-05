using System.Diagnostics;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// PLC rung 에서 접점 간 연결을 표현하는 edge class
    /// </summary>

    [DebuggerDisplay("{Name}")]
    public class Wire
    {
        public string Name { get; private set; }
        public Wire(ILSentence sentence)           
        {
            Name = sentence.ToString();
        }
        public Wire(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
