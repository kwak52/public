namespace Dsu.PLCConvertor.Common
{

    /// <summary>
    /// Parsing 을 위해서 보조적으로 사용되는 node
    /// </summary>
    public class AuxNode : Point
    {
        protected AuxNode(string name) : base(name)
        {
        }
    }


    public class StartNode : AuxNode
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
    public class EndNode : AuxNode
    {
        public EndNode(string name)
            : base($"END:{name}")
        {
        }
        public StartNode StartNode { get; set; }
        public override string ToString() => StartNode == null ? Name : $"{Name}({StartNode.Guid}-{Guid})";
    }


    public class TRNode : AuxNode
    {
        public TRNode(string name)
            : base(name)
        {
        }
    }


    public class TerminalNode : Point
    {
        public TerminalNode(string name)
            : base(name)
        {
        }
    }
}
