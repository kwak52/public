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
    internal class EndMarker : AuxNode
    {
        public EndMarker(string name)
            : base($"END:{name}")
        {
        }
    }


    internal class TRMarker : AuxNode
    {
        public TRMarker(string name)
            : base(name)
        {
        }
    }
}
