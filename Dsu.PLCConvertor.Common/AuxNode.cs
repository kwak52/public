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
    public class EndMarker : AuxNode
    {
        public EndMarker(string name)
            : base($"END:{name}")
        {
        }
    }


    public class TRMarker : AuxNode
    {
        public TRMarker(string name)
            : base(name)
        {
        }
    }
}
