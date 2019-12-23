using System.Collections.Generic;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class CxtInfoResource : CxtInfo
    {
        public string Name { get; private set; }
        public List<CxtInfoProgram> Programs { get; } = new List<CxtInfoProgram>();

        public override IEnumerable<CxtInfo> Children => Programs;

        internal override void ClearMyResult() { }

        public CxtInfoResource(string name)
            : base("RESOURCE")
        {
            Name = name;
        }
    }
}
