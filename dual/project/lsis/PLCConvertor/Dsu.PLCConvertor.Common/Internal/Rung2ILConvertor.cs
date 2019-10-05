using Dsu.Common.Utilities.ExtensionMethods;
using log4net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    //enum EdgeMuliplicity
    //{
    //    Zero,
    //    One,
    //    Many,
    //};

    //enum InOutEdgeMultiplicity
    //{
    //    /// <summary>
    //    /// Start case : S -> A
    //    /// </summary>
    //    ZeroOne,
    //    /// <summary>
    //    /// Start case : S -> {A, B}
    //    /// </summary>
    //    ZeroMany,
    //    /// <summary>
    //    /// Out (Coil) case : A -> O
    //    /// </summary>
    //    OneZero,
    //    OneOne,
    //    OneMany,
    //    ManyZero,
    //    ManyOne,
    //    ManyMany,
    //}


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
        }


        public static string[] Convert(Rung rung) => new Rung2ILConvertor(rung).Convert().ToArray();
    }
}
