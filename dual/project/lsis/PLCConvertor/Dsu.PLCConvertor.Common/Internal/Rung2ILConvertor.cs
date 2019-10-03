using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    interface IStackItem {}
    class StackItemEdge : IStackItem
    {
        public Edge<Point, Wire> Edge { get; private set; }
        public StackItemEdge(Edge<Point, Wire> edge)
        {
            Edge = edge;
        }
    }

    class StackItemPoint : IStackItem
    {
        public Point Point { get; private set; }
        public StackItemPoint(Point point)
        {
            Point = point;
        }
    }

    class StackItemCommand : IStackItem
    {
        public string Command { get; private set; }
        public StackItemCommand(string command)
        {
            Command = command;
        }
    }

    class IncomingEdgeManager
    {
        Point _node;
        public int NumTotalIncomingEdge { get; private set; }
        public int EdgeIndex { get; internal set; }

        /// <summary>
        /// 모든 Incoming edge 에 대해서 처리가 끝나면 false 반환
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() => ++EdgeIndex < NumTotalIncomingEdge;
        public IncomingEdgeManager(Rung rung, Point node)
        {
            _node = node;
            NumTotalIncomingEdge = rung.GetIncomingDegree(node);
        }
    }


    internal class Rung2ILConvertor
    {
        Rung _rung;
        bool _hasMultipleOutput;
        Rung2ILConvertor(Rung rung)
        {
            _rung = rung;
            _hasMultipleOutput = _rung.Sinks.Count() > 1;
        }

        Stack<IStackItem> _stack;

        /// <summary>
        /// Active current point location
        /// </summary>
        Point _current;
        IEnumerable<string> Convert()
        {
            var initials = _rung.Sources.Reverse().Select(s => new StackItemPoint(s));
            _stack = new Stack<IStackItem>(initials);
            while (_stack.Any())
            {
                var element = _stack.Pop();
                switch(element)
                {
                    case StackItemPoint sp:
                        foreach (var m in FollowNode(sp.Point))
                            yield return m;

                        break;
                    case StackItemEdge se:
                        //yield return $"LD(init) {se.Edge.Start.Name}";
                        foreach (var m in FollowNode(se.Edge.End))
                            yield return m;
                        break;
                    case StackItemCommand sc:
                        yield return sc.Command;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                        
                }
            }
        }

        IEnumerable<string> ProcessIncomingEdge(Point node)
        {
            if (node.IncomingEdgeManager == null)
            {
                node.IncomingEdgeManager = new IncomingEdgeManager(_rung, node) { EdgeIndex = 1 };
                yield break;
            }

            yield return "ORLD";
            if (node.IncomingEdgeManager.MoveNext())
            {
            }
            else
            {
                yield return $"OUT1 {node.Name}";
            }
        }

        IEnumerable<string> FollowNode(Point sp, int c=0)
        {
            _current = sp;
            var oEdges = _rung.GetOutgoingEdges(_current).ToArray();
            var cmd = c == 0 ? "LD" : "AND";

            switch (oEdges.Length)
            {
                // terminal node 에 도착 : OUT
                case 0:
                    if (sp.IncomingEdgeManager.EdgeIndex == sp.IncomingEdgeManager.NumTotalIncomingEdge)
                        yield return $"OUT2 {_current.Name}";
                    break;
                case 1:
                    yield return $"{cmd} {_current.Name}";
                    //yield return $"{sp.ILSentence}";

                    var end = oEdges[0].End;
                    foreach (var x in ProcessIncomingEdge(end))
                        yield return x;
                    foreach (var x in FollowNode(end, ++c))
                        yield return x;
                    break;
                default:
                    yield return $"{cmd} {_current.Name}";
                    //yield return $"{sp.ILSentence}";

                    _stack.Push(new StackItemCommand("ANDLD"));
                    oEdges.Iter(e => _stack.Push(new StackItemEdge(e)));
                    break;
            }
        }


        public static string[] Convert(Rung rung) => new Rung2ILConvertor(rung).Convert().ToArray();
    }
}
