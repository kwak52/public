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


    internal class StartMarker : AuxNode
    {
        public StartMarker(string name)
            : base(name)
        {
        }
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
