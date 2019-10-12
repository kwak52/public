using Dsu.Common.Utilities.ExtensionMethods;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    //partial class Rung2ILConvertor
    //{
    //    IEnumerable<AuxNode> EnumerateTerminalArticulationPoints()
    //    {
    //        var auxNodes = _rung.Nodes.OfType<AuxNode>();
    //        if (auxNodes.OfType<TRNode>().Any())
    //            return
    //                from tr in auxNodes.OfType<TRNode>()
    //                let descendantAxns =
    //                    _rung.ReverseDepthFirstSearch(tr)
    //                        .OfType<TRNode>()
    //                        .Where(n => n != tr)
    //                where descendantAxns.IsNullOrEmpty()
    //                select tr
    //                ;
    //        else
    //            return
    //                from axn in auxNodes
    //                let descendantAxns =
    //                    _rung.DepthFirstSearch(axn)
    //                        .OfType<AuxNode>()
    //                        .Where(n => n != axn)
    //                where descendantAxns.IsNullOrEmpty()
    //                select axn
    //                ;
    //    }

    //    IEnumerable<string> ConvertBackward(Point node, int call)
    //    {
    //        if (_visitedNodes.Contains(node))
    //            yield break;

    //        _visitedNodes.Add(node);

    //        var inNs = _rung.GetIncomingNodes(node).ToArray();
    //        if (inNs.Length == 0)   // start point
    //        {
    //            yield return $"LD {node.Name}";
    //            yield break;
    //        }

    //        var incomingEdgeTrans =
    //            inNs.Select(n => ConvertBackward(n, call + 1).ToArray())
    //            .ToArray()
    //            ;

    //        int i = 0;
    //        switch (node)
    //        {
    //            case TRNode tr:
    //                foreach (var edgeT in incomingEdgeTrans)
    //                {
    //                    if (call > 0)
    //                    {
    //                        if (i == 1)
    //                            yield return "MPUSH";
    //                        else if (i == incomingEdgeTrans.Length - 1)
    //                            yield return "MPOP//1";
    //                        else if (i > 0 && i < incomingEdgeTrans.Length - 1)
    //                            yield return "MLOAD";
    //                    }
    //                    i++;
    //                    foreach (var x in edgeT)
    //                        yield return x;
    //                }
    //                break;
    //            case EndNode end:
    //                foreach (var edgeT in incomingEdgeTrans)
    //                {
    //                    foreach (var x in edgeT)
    //                        yield return x;
    //                    if (i++ > 0)
    //                        yield return "ORLD";
    //                }

    //                if (inNs.Length > 1 && _rung.IsInCircularWithBackward(end))       // and, detects loop
    //                    yield return $"ANDLD//123{node.Name}";
    //                break;
    //            case Point pt:
    //                Debug.Assert(inNs.Length == 1);
    //                foreach (var x in incomingEdgeTrans.First())
    //                    yield return x;

    //                var cmd = inNs[0] is AuxNode && _rung.GetOutgoingDegree(inNs[0]) > 1 ? "LD" : "AND";
    //                yield return $"{cmd} {pt.Name}//1";
    //                break;

    //            default:
    //                Debug.Assert(false);
    //                break;
    //        }
    //    }

    //    IEnumerable<string> ConvertForward(Point node)
    //    {
    //        _visitedNodes.Add(node);

    //        if (node is TerminalNode)
    //        {
    //            yield return $"OUT {node.Name}//111";
    //            yield break;
    //        }


    //        var outNs = _rung.GetOutgoingNodes(node).ToArray();
    //        var outgoingEdgeTrans =
    //            outNs.Select(n => ConvertForward(n).ToArray())
    //            .ToArray()
    //            ;

    //        int i = 0;
    //        switch (node)
    //        {
    //            case TRNode tr:
    //                foreach (var edgeT in outgoingEdgeTrans)
    //                {
    //                    if (i == 0)
    //                        yield return "MPUSH";
    //                    else if (i == outgoingEdgeTrans.Length - 1)
    //                        yield return "MPOP//2";
    //                    else // if (i > 0 && i < outgoingEdgeTrans.Length - 1)
    //                        yield return "MLOAD";
    //                    i++;
    //                    foreach (var x in edgeT)
    //                        yield return x;
    //                }
    //                break;

    //            case TerminalNode tn:
    //                yield return $"OUT {tn.Name}//123";
    //                break;
    //            case EndNode end:
    //                foreach (var edgeT in outgoingEdgeTrans)
    //                    foreach (var x in edgeT)
    //                        yield return x;
    //                break;
    //            case Point pt:
    //                yield return $"AND {pt.Name}//2";
    //                foreach (var edgeT in outgoingEdgeTrans)
    //                    foreach (var x in edgeT)
    //                        yield return x;
    //                break;
    //            default:
    //                Debug.Assert(false);
    //                break;
    //        }
    //    }

    //    internal IEnumerable<string> Convert_OldVersion()
    //    {
    //        var aps = EnumerateTerminalArticulationPoints().ToArray();
    //        var apsMsg = string.Join(", ", aps.Select(n => n.Name));
    //        Logger?.Debug($"Found total {aps.Length} terminal nodes.\r\n{apsMsg}");

    //        var mnemonics = aps.SelectMany(ap => ConvertBackward(ap, 0).Concat(ConvertForward(ap))).Realize();
    //        var unvisitedNodes = _rung.Nodes.Where(n => !_visitedNodes.Contains(n)).ToArray();
    //        if (unvisitedNodes.Any())
    //        {
    //            var msg = string.Join(", ", unvisitedNodes.Select(n => n.Name));
    //            Logger?.Error($"Total {unvisitedNodes.Length} points untranslated.\r\n{msg}");
    //            Debug.Assert(false);
    //        }

    //        return mnemonics;
    //    }


    //}
}
