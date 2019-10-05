using Dsu.Common.Utilities.Core.ExtensionMethods;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Graph;
using log4net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    //interface IStackItem {}

    //[DebuggerDisplay("{Edge.Start.Name} -> {Edge.End.Name}")]
    //class StackItemEdge : IStackItem
    //{
    //    public int EdgeIndex { get; private set; }
    //    public Edge<Point, Wire> Edge { get; private set; }
    //    public StackItemEdge(Edge<Point, Wire> edge, int edgeIndex)
    //    {
    //        Edge = edge;
    //        EdgeIndex = edgeIndex;
    //    }
    //}

    ////class StackItemPoint : IStackItem
    ////{
    ////    public Point Point { get; private set; }
    ////    public StackItemPoint(Point point)
    ////    {
    ////        Point = point;
    ////    }
    ////}

    //class StackItemCommand : IStackItem
    //{
    //    public string Command { get; private set; }
    //    public StackItemCommand(string command)
    //    {
    //        Command = command;
    //    }
    //}


    enum EdgeMuliplicity
    {
        Zero,
        One,
        Many,
    };

    enum InOutEdgeMultiplicity
    {
        /// <summary>
        /// Start case : S -> A
        /// </summary>
        ZeroOne,
        /// <summary>
        /// Start case : S -> {A, B}
        /// </summary>
        ZeroMany,
        /// <summary>
        /// Out (Coil) case : A -> O
        /// </summary>
        OneZero,
        OneOne,
        OneMany,
        ManyZero,
        ManyOne,
        ManyMany,
    }


    internal class Rung2ILConvertor
    {
        ILog Logger = Global.Logger;
        Rung _rung;
        bool _hasMultipleOutput;
        Rung2ILConvertor(Rung rung)
        {
            _rung = rung;
            _hasMultipleOutput = _rung.Sinks.Count() > 1;
        }

        //Stack<IStackItem> _stack;

        ///// <summary>
        ///// Active current edge location
        ///// </summary>
        //StackItemEdge _current;


        //void CollectNodeInfos()
        //{
        //    _rung.Nodes.Iter(n =>
        //    {
        //        n.NodeInfo = new NodeInfo4ILConvert(_rung, n);
        //    });

        //    _rung.Nodes
        //        .Select(n => n.NodeInfo)
        //        .Iter(ni =>
        //        {
        //            // 노드의 outgoing edge 가 복수개 일 때에, LDOR 결합인지 MPUSH, MREAD 결합인지 판별
        //            // LDOR 는 동일한 출력으로 모이는 경우이고,
        //            // MPUSH, MREAD 는 출력 node (output, coil) 이 갈라지는 경우이므로
        //            // 현재 node 의 OutgoingNode 들에 대해서 coil 들을 모아서 이들이 전부 같은지를 비교한다.
        //            // 전부 같은 output 이면 LDOR 을 따른다.
        //            if (ni.OutgoingEdgeMultiplicity == EdgeMuliplicity.Many)
        //            {
        //                bool merged = 
        //                    _rung.GetOutgoingNodes(ni.Node)
        //                        .Select(on => on.NodeInfo.TerminalNodesFollowing/* _rung.EnumerateTerminalNodes(on)*/)
        //                        .EnumerateWindowed(2)
        //                        .All(s => s.First().SetEqual(s.Last()))
        //                        ;
        //                Logger?.Debug($"{ni.Node} merged={merged}");

        //                ni.RequireMPush = !merged;
        //            }
        //        });
        //}

        IEnumerable<AuxNode> EnumerateTerminalArticulationPoints()
        {
            var auxNodes = _rung.Nodes.OfType<AuxNode>();
            if (auxNodes.OfType<TRNode>().Any())
                return
                    from tr in auxNodes.OfType<TRNode>()
                    let descendantAxns =
                        _rung.ReverseDepthFirstSearch(tr)
                            .OfType<TRNode>()
                            .Where(n => n != tr)
                    where descendantAxns.IsNullOrEmpty()
                    select tr
                    ;
            else
                return
                    from axn in auxNodes
                    let descendantAxns =
                        _rung.DepthFirstSearch(axn)
                            .OfType<AuxNode>()
                            .Where(n => n != axn)
                    where descendantAxns.IsNullOrEmpty()
                    select axn
                    ;
        }

        IEnumerable<string> ConvertBackward(Point node, int call)
        {
            _visitedNodes.Add(node);

            var inNs = _rung.GetIncomingNodes(node).ToArray();
            if (inNs.Length == 0)   // start point
            {
                yield return $"LD {node.Name}";
                yield break;
            }

            var incomingEdgeTrans =
                inNs.Select(n => ConvertBackward(n, call+1).ToArray())
                .ToArray()
                ;

            int i = 0;
            switch (node)
            {
                case TRNode tr:
                    foreach (var edgeT in incomingEdgeTrans)
                    {
                        if (call > 0)
                        {
                            if (i == 1)
                                yield return "MPUSH";
                            else if (i == incomingEdgeTrans.Length - 1)
                                yield return "MPOP//1";
                            else if (i > 0 && i < incomingEdgeTrans.Length - 1)
                                yield return "MREAD";
                        }
                        i++;
                        foreach (var x in edgeT)
                            yield return x;
                    }
                    break;
                case EndNode end:
                    foreach (var edgeT in incomingEdgeTrans)
                    {
                        foreach (var x in edgeT)
                            yield return x;
                        if (i++ > 0)
                            yield return "ORLD";
                    }
                    break;
                case Point pt:
                    Debug.Assert(inNs.Length == 1);
                    foreach (var x in incomingEdgeTrans.First())
                        yield return x;

                    yield return $"AND {pt.Name}";
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }

        IEnumerable<string> ConvertForward(Point node)
        {
            _visitedNodes.Add(node);

            if (node is TerminalNode)
            {
                yield return $"OUT {node.Name}";
                yield break;
            }


            var outNs = _rung.GetOutgoingNodes(node).ToArray();
            var outgoingEdgeTrans =
                outNs.Select(n => ConvertForward(n).ToArray())
                .ToArray()
                ;

            int i = 0;
            switch (node)
            {
                case TRNode tr:
                    foreach (var edgeT in outgoingEdgeTrans)
                    {
                        if (i == 0)
                            yield return "MPUSH";
                        else if (i == outgoingEdgeTrans.Length - 1)
                            yield return "MPOP//2";
                        else // if (i > 0 && i < outgoingEdgeTrans.Length - 1)
                            yield return "MREAD";
                        i++;
                        foreach (var x in edgeT)
                            yield return x;
                    }
                    break;

                case TerminalNode tn:
                    yield return $"OUT {tn.Name}";
                    break;
                case EndNode end:
                    foreach (var edgeT in outgoingEdgeTrans)
                        foreach (var x in edgeT)
                            yield return x;
                    break;
                case Point pt:
                    yield return $"AND {pt.Name}";
                    foreach (var edgeT in outgoingEdgeTrans)
                        foreach (var x in edgeT)
                            yield return x;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }



        /// <summary>
        /// IL 변환 중에 방문한 node.  모든 node 를 cover 하지 않으면 오류가 있는 것으로 봐야 한다.
        /// </summary>
        HashSet<Point> _visitedNodes = new HashSet<Point>();
        /// <summary>
        /// 주어진 rung 구조를 IL 리스트로 변환한다.  변환이 시작되는 위치
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> Convert()
        {
            //CollectNodeInfos();

            var aps = EnumerateTerminalArticulationPoints().ToArray();
            var apsMsg = string.Join(", ", aps.Select(n => n.Name));
            Logger?.Debug($"Found total {aps.Length} terminal nodes.\r\n{apsMsg}");

            var mnemonics = aps.SelectMany(ap => ConvertBackward(ap, 0).Concat(ConvertForward(ap))).Realize();
            var unvisitedNodes = _rung.Nodes.Where(n => !_visitedNodes.Contains(n)).ToArray();
            if (unvisitedNodes.Any())
            {
                var msg = string.Join(", ", unvisitedNodes.Select(n => n.Name));
                Logger?.Error($"Total {unvisitedNodes.Length} points untranslated.\r\n{msg}");
                Debug.Assert(false);
            }

            return mnemonics;


            //var initials = _rung.Sources.Reverse().Select( (s, n) => new StackItemEdge(new Edge<Point, Wire>(null, s, null), n));
            //_stack = new Stack<IStackItem>(initials);
            //while (_stack.Any())
            //{
            //    var element = _stack.Pop();
            //    switch(element)
            //    {
            //        //case StackItemPoint sp:
            //        //    foreach (var m in FollowNode(sp.Point))
            //        //        yield return m;

            //        //    break;
            //        case StackItemEdge se:
            //            //yield return $"LD(init) {se.Edge.Start.Name}";
            //            foreach (var m in FollowEdge(se))
            //                yield return m;
            //            break;
            //        case StackItemCommand sc:
            //            yield return sc.Command;
            //            break;

            //        default:
            //            Debug.Assert(false);
            //            break;
                        
            //    }
            //}
        }


        //Dictionary<Point, int> _visitedNodes = new Dictionary<Point, int>();

        ///// <summary>
        ///// edge 를 따라가면서 IL 출력을 생성한다.  edge 의 start 부분은 이미 처리되었다고 가정하고, end 부분부터 처리한다.
        ///// </summary>
        ///// <param name="edge"></param>
        ///// <returns></returns>
        //IEnumerable<string> FollowEdge(StackItemEdge ee)
        //{
        //    var edge = ee.Edge;
        //    _current = ee;
        //    var prvNode = edge.Start;
        //    var nxtNode = edge.End;
        //    var prvNi = prvNode?.NodeInfo;
        //    var nxtNi = nxtNode.NodeInfo;

        //    if (prvNode == null)
        //        yield return $"LD {nxtNode.Name}";
        //    else
        //    {
        //        var prvOEM = prvNi.OutgoingEdgeMultiplicity;       // Previous Outgoing : number of Outgoing edges of Previous node 
        //        var nxtIEM = nxtNi.IncomingEdgeMultiplicity;       // Next Incoming : number of Incoming edges of Next node 

        //        switch (prvOEM)
        //        {
        //            case EdgeMuliplicity.One:
        //                switch (nxtIEM)
        //                {
        //                    case EdgeMuliplicity.One:
        //                        yield return $"{nxtNode.ILSentence}";
        //                        break;
        //                    case EdgeMuliplicity.Many:
        //                        if (_visitedNodes.ContainsKey(nxtNode))
        //                        {
        //                            yield return $"ORLD";
        //                            if (_visitedNodes[nxtNode] == nxtNi.NumTotalIncomingEdge)
        //                                yield return $"{nxtNode.ILSentence}//Test";

        //                        }
        //                        else
        //                            _visitedNodes.Add(nxtNode, 0);

        //                        _visitedNodes[nxtNode] = _visitedNodes[nxtNode] + 1;

        //                        break;
        //                    default:
        //                        Debug.Assert(false);
        //                        break;
        //                }
        //                break;
        //            case EdgeMuliplicity.Many:
        //                if (prvNi.RequireMPush.Value)
        //                {
        //                    var edgeIndex = ee.EdgeIndex;
        //                    if (edgeIndex == 0)
        //                        yield return $"MPUSH";
        //                    else if (edgeIndex == prvNode.NodeInfo.NumTotalOutgoingEdge - 1)
        //                        yield return $"MPOP";
        //                    else
        //                        yield return $"MREAD";

        //                    yield return $"AND {nxtNode.Name}";
        //                }
        //                else
        //                    yield return $"LD {nxtNode.Name}";

        //                break;

        //            default:
        //                Debug.Assert(false);
        //                break;
        //        }
        //    }

        //    var oEdges = _rung.GetOutgoingEdges(nxtNode);
        //    if (oEdges.IsNullOrEmpty() && _visitedNodes.ContainsKey(nxtNode) && _visitedNodes[nxtNode] == nxtNode.NodeInfo.NumTotalIncomingEdge)
        //        yield return $"{nxtNode.ILSentence}";
        //    else
        //        oEdges
        //            .Reverse()
        //            .Select((e, n) => new StackItemEdge(e, n))
        //            .Reverse()
        //            .Iter(e => _stack.Push(e))
        //            ;

            
        //    //var ni = sp.NodeInfo;
        //    //var oEdges = _rung.GetOutgoingEdges(_current).ToArray();
        //    //var cmd = c == 0 ? "LD" : "AND";

        //    //switch (ni.OutputEdgeMultiplicity)
        //    //{
        //    //    // terminal node 에 도착 : OUT
        //    //    case EdgeMuliplicity.Zero:
        //    //        if (sp.NodeInfo.CurrentIncomingEdgeIndex == sp.NodeInfo.NumTotalIncomingEdge)
        //    //            yield return $"OUT {_current.Name}";
        //    //        break;

        //    //    case EdgeMuliplicity.One:
        //    //        yield return $"{cmd} {_current.Name}";
        //    //        //yield return $"{sp.ILSentence}";

        //    //        var end = oEdges[0].End;
        //    //        foreach (var x in ProcessIncomingEdge(end))
        //    //            yield return x;
        //    //        foreach (var x in FollowEdge(end, ++c))
        //    //            yield return x;
        //    //        break;

        //    //    case EdgeMuliplicity.Many:
        //    //        yield return $"{cmd} {_current.Name}";
        //    //        //yield return $"{sp.ILSentence}";

        //    //        foreach (var x in ProcessOutgoingEdge(sp))
        //    //            yield return x;


        //    //        _stack.Push(new StackItemCommand("ANDLD"));
        //    //        oEdges.Iter(e => _stack.Push(new StackItemEdge(e)));
        //    //        break;

        //    //    default:
        //    //        throw new Exception("Unexpected.");
        //    //}


        //    //IEnumerable<string> ProcessIncomingEdge(Point node)
        //    //{
        //    //    try
        //    //    {
        //    //        if (node.NodeInfo.CurrentIncomingEdgeIndex == 0)
        //    //            yield break;

        //    //        yield return "ORLD";
        //    //    }
        //    //    finally
        //    //    {
        //    //        node.NodeInfo.MoveIncomingEdgeNext();
        //    //    }
        //    //}
        //    //IEnumerable<string> ProcessOutgoingEdge(Point node)
        //    //{
        //    //    if (node.NodeInfo.RequireMPush.HasTrueValue())
        //    //        yield return "MPUSH";
        //    //        //_stack.Push(new StackItemCommand("MPUSH"));

        //    //    yield break;
        //    //}
        //}


        public static string[] Convert(Rung rung) => new Rung2ILConvertor(rung).Convert().ToArray();
    }
}
