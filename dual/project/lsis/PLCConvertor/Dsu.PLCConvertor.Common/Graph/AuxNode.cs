namespace Dsu.PLCConvertor.Common
{

    /// <summary>
    /// Parsing 을 위해서 보조적으로 사용되는 node
    /// </summary>
    internal class AuxNode : Point
    {
        protected AuxNode(string name) : base(name)
        {
        }
        protected AuxNode(string name, ILSentence sentence) : base(name, sentence)
        {
        }
    }


    internal class StartNode : AuxNode
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
    internal class EndNode : AuxNode
    {
        public EndNode(string name)
            : base($"END:{name}")
        {
        }
        public StartNode StartNode { get; set; }
        public override string ToString() => StartNode == null ? Name : $"{Name}({StartNode.Guid}-{Guid})";
    }


    internal class TRNode : AuxNode
    {
        public TRNode(string name, ILSentence sentence)
            : base(name, sentence)
        {
        }
    }

    internal class DummyNode : AuxNode
    {
        public DummyNode(string name)
            : base(name)
        {
        }
    }


    internal class TerminalNode : Point
    {
        public TerminalNode(string name, ILSentence sentence)
            : base(name, sentence)
        {
        }
    }
}
