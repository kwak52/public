using System;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    class NodeInfo4ILConvert
    {
        internal Point Node { get; private set; }
        Rung _rung;
        public int NumTotalIncomingEdge => _rung.GetIncomingDegree(Node);
        public int NumTotalOutgoingEdge => _rung.GetOutgoingDegree(Node);
        public int CurrentIncomingEdgeIndex { get; internal set; }

        /// <summary>
        /// Node 기준으로 incoming / outgoing edge 의 multiplicity
        /// </summary>
        public InOutEdgeMultiplicity InOutEdgeMultiplicity { get; private set; }
        public EdgeMuliplicity IncomingEdgeMultiplicity { get; private set; }
        public EdgeMuliplicity OutgoingEdgeMultiplicity { get; private set; }

        /// <summary>
        /// 모든 Incoming edge 에 대해서 처리가 끝나면 false 반환
        /// </summary>
        /// <returns></returns>
        public bool MoveIncomingEdgeNext() => ++CurrentIncomingEdgeIndex < NumTotalIncomingEdge;

        /// <summary>
        /// 다중 outgoing edge 일때 non-null.
        /// </summary>
        internal Nullable<bool> RequireMPush = null;


        internal Point[] TerminalNodesFollowing;
        public NodeInfo4ILConvert(Rung rung, Point node)
        {
            _rung = rung;
            Node = node;

            TerminalNodesFollowing = rung.EnumerateTerminalNodes(node).ToArray();

            InOutEdgeMultiplicity m;
            int i = NumTotalIncomingEdge;
            int o = NumTotalOutgoingEdge;
            switch(i)
            {
                case 0:
                    switch (o)
                    {
                        case 0: throw new Exception("Something wrong!");
                        case 1: m = InOutEdgeMultiplicity.ZeroOne; break;
                        default: m = InOutEdgeMultiplicity.ZeroMany; break;
                    }
                    break;
                case 1:
                    switch (o)
                    {
                        case 0: m = InOutEdgeMultiplicity.OneZero; break;   // OUT case
                        case 1: m = InOutEdgeMultiplicity.OneOne; break;
                        default: m = InOutEdgeMultiplicity.OneMany; break;
                    }
                    break;
                default:
                    switch (o)
                    {
                        case 0: m = InOutEdgeMultiplicity.ManyZero; break;   // OUT case
                        case 1: m = InOutEdgeMultiplicity.ManyOne; break;
                        default: m = InOutEdgeMultiplicity.ManyMany; break;
                    }
                    break;
            }

            switch (i)
            {
                case 0: IncomingEdgeMultiplicity = EdgeMuliplicity.Zero; break;
                case 1: IncomingEdgeMultiplicity = EdgeMuliplicity.One; break;
                default: IncomingEdgeMultiplicity = EdgeMuliplicity.Many; break;
            }
            switch (o)
            {
                case 0: OutgoingEdgeMultiplicity = EdgeMuliplicity.Zero; break;
                case 1: OutgoingEdgeMultiplicity = EdgeMuliplicity.One; break;
                default: OutgoingEdgeMultiplicity = EdgeMuliplicity.Many; break;
            }

            InOutEdgeMultiplicity = m;
        }
    }
}
