using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// Rung 기능 중, 실제 rung 을 구성하고 build 하기 위한 구조.  build 가 끝나면 Rung 구조로 다시 반환(ToRung())
    /// </summary>
    internal class Rung4Parsing : Rung
    {
        public Dictionary<AuxNode, SubRung> TRmap = new Dictionary<AuxNode, SubRung>();

        /// <summary>
        /// Rung build 중에 작업하고 있는 현재의 sub rung
        /// </summary>
        public SubRung CurrentBuildingLD { get; private set; }

        string[] _mnemonics;
        public Stack<SubRung> LadderStack;
        public Rung4Parsing(IEnumerable<string> mnemonics)
        {
            _mnemonics = mnemonics.ToArray();
            LadderStack = new Stack<SubRung>();
        }


        public IEnumerable<int> CoRoutineRungParser()
        {
            int i = 0;
            foreach (var m in _mnemonics)
            {
                var sentence = new ILSentence(m);
                var arg0 = sentence.Args.IsNullOrEmpty() ? null : sentence.Args[0];
                var arg0N = new Point(arg0) { ILSentence = sentence };
                switch (sentence.Command)
                {
                    case "LD" when arg0.StartsWith("TR"):
                        CurrentBuildingLD.LDTR(arg0);
                        break;

                    case "LD":
                        if (CurrentBuildingLD != null)
                            LadderStack.Push(CurrentBuildingLD);
                        CurrentBuildingLD = new SubRung(this, arg0N);
                        break;

                    case "AND":
                        CurrentBuildingLD.AND(arg0N, sentence);
                        break;
                    case "ANDLD":
                        CurrentBuildingLD = LadderStack.Pop().ANDLD(CurrentBuildingLD);
                        break;

                    case "OR":
                        CurrentBuildingLD.OR(arg0N, sentence);
                        break;
                    case "ORLD":
                        CurrentBuildingLD = LadderStack.Pop().ORLD(CurrentBuildingLD);
                        break;

                    case "OUT" when arg0.StartsWith("TR"):
                        CurrentBuildingLD.OUTTR(new AuxNode(arg0), sentence);
                        break;
                    case "OUT":
                        CurrentBuildingLD.OUT(arg0N, sentence);
                        break;
                    default:
                        break;
                }

                yield return i++;
            }

            Debug.Assert(LadderStack.IsNullOrEmpty());
            Console.WriteLine("");
        }

        /// <summary>
        /// Rung 구축을 위한 단계로 사용된 Rung4Parsing 로부터 최종 Rung 을 생성해서 반환한다.
        /// Rung 구축 중간에 사용된 임시 node 들을 제거
        /// </summary>
        /// <returns></returns>
        public Rung ToRung(bool removeAuxNode=true)
        {
            var rung = new Rung(_mnemonics);
            rung.MergeGraph(CurrentBuildingLD);
            if (removeAuxNode)
            {
                var auxNodes = rung.Nodes.OfType<AuxNode>().ToArray();
                auxNodes.Iter(n =>
                {
                    var incomings = rung.GetIncomingNodes(n).ToArray();
                    var outgoings = rung.GetOutgoingNodes(n).ToArray();
                    incomings.Iter(i =>
                    {
                        outgoings.Iter(o =>
                        {
                            rung.AddEdge(i, o);
                        });
                    });
                    rung.Remove(n);
                });
            }

            return rung;
        }
    }

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


    /// <summary>
    /// Rung 을 구성하는 내부의 sub rung 을 표현하기 위한 class
    /// </summary>
    internal class SubRung : Rung
    {
        AuxNode start = new AuxNode("S");
        Point end = new EndMarker("E");
        Rung4Parsing _masterRung;
        public SubRung(Rung4Parsing masterRung, string node)
            : this(masterRung, new Point(node))
        {
        }
        public SubRung(Rung4Parsing masterRung, Point node)
        {
            _masterRung = masterRung;
            Add(start);
            Add(node);
            Add(end);
            AddEdge(start, node);
            AddEdge(node, end);
        }

        public void AND(Point node, ILSentence sentence)
        {
            Add(node);
            MakeEndNode(node);
        }

        void MakeEndNode(Point start)
        {
            var oldEnd = end;
            end = new EndMarker(start.Name);
            Add(end);
            var incomingNodes = GetIncomingNodes(oldEnd).ToArray();
            incomingNodes.Iter(s =>
            {
                AddEdge(s, start);
                AddEdge(start, end);
            });

            if (! oldEnd.Name.StartsWith("TR"))
                Remove(oldEnd);
        }

        /// <summary>
        /// this Ladder 와 next ladder 를 AND 결합한다.
        /// </summary>
        public SubRung ANDLD(SubRung next)
        {
            MergeGraph(next);

            GetIncomingNodes(end).Iter(s =>
            {
                GetOutgoingNodes(next.start).Iter(e =>
                {
                    AddEdge(s, e, new Edge($"{s}->{e}"));
                });
            });

            return this;
        }

        /// <summary>
        /// this graph 내용에 other 로 주어진 graph 를 병합한다.
        /// </summary>
        public override void MergeGraph(Graph<Point, Edge> other)
        {
            base.MergeGraph(other);

            var kvs = _masterRung.TRmap.Where(kv => kv.Value == other).ToArray();
            foreach (var kv in kvs)
            {
                _masterRung.TRmap[kv.Key] = this;
            }
        }

        public void OR(Point node, ILSentence sentence)
        {
            Add(node);
            AddEdge(start, node, new Edge(sentence));
            AddEdge(node, end, new Edge(sentence));
        }

        public SubRung ORLD(SubRung next)
        {
            MergeGraph(next);

            AddEdge(next.start, start);
            AddEdge(next.end, end);

            return this;
        }
        public void OUT(Point node, ILSentence sentence)
        {
            Add(node);
            AddEdge(end, node, new Edge(sentence));
        }

        public void OUTTR(AuxNode tr, ILSentence sentence)
        {
            _masterRung.TRmap.Add(tr, this);
            Add(tr);
            AddEdge(end, tr, new Edge(sentence));
            end = new EndMarker(tr.Name);
            Add(end);
            AddEdge(tr, end);
        }

        public void LDTR(string tr)
        {
            var trEntry = _masterRung.TRmap.First(kv => kv.Key.Name == tr);
            end = trEntry.Key;
        }
    }
}
