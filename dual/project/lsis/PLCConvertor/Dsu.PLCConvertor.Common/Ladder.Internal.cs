namespace Dsu.PLCConvertor.Common
{

    /// <summary>
    /// Parsing 을 위해서 보조적으로 사용되는 node
    /// </summary>
    internal class AuxNode : Point
    {
        public AuxNode(string name) : base(name)
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
}
