using Dsu.Common.Utilities.Graph;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    internal class StartNode : Point, IAuxNode
    {
        public StartNode(string name)
            : base(name)
        {
        }
        public EndNode EndNode { get; set; }
        public override string ToString() => EndNode == null ? Name : $"{Name}({Guid}-{EndNode.Guid})";
    }
    /// <summary>
    /// Parsing 을 위해서 보조적으로 사용되는 node
    /// </summary>
    internal class EndNode : Point, IAuxNode
    {
        public EndNode(string name)
            : base($"END:{name}")
        {
        }
        public StartNode StartNode { get; set; }
        public override string ToString() => StartNode == null ? Name : $"{Name}({StartNode.Guid}-{Guid})";
    }


    internal class TRNode : Point, IAuxNode
    {
        public TRNode(string name, ILSentence sentence)
            : base(name, sentence)
        {
        }
    }

    internal class DummyNode : Point, IAuxNode
    {
        public DummyNode(string name)
            : base(name)
        {
        }
    }


    internal class OutNode : Point, ITerminalNode
    {
        public OutNode(string name, ILSentence sentence)
            : base(name, sentence)
        {
        }
    }
}
