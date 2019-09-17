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
        SubRung _currentLD;

        string[] _mnemonics;
        public Rung4Parsing(IEnumerable<string> mnemonics)
        {
            _mnemonics = mnemonics.ToArray();
            var ldStack = new Stack<SubRung>();

            foreach (var m in mnemonics)
            {
                var sentence = new ILSentence(m);
                var arg0 = sentence.Args.IsNullOrEmpty() ? null : sentence.Args[0];
                var arg0N = new Node(arg0);
                switch(sentence.Command)
                {
                    case "LD" when arg0.StartsWith("TR"):
                        _currentLD.LDTR(arg0);
                        break;

                    case "LD":
                        if (_currentLD != null)
                            ldStack.Push(_currentLD);
                        _currentLD = new SubRung(this, arg0N);
                        break;

                    case "AND":
                        _currentLD.AND(arg0N, sentence);
                        break;
                    case "ANDLD":
                        _currentLD = ldStack.Pop().ANDLD(_currentLD);
                        break;

                    case "OR":
                        _currentLD.OR(arg0N, sentence);
                        break;
                    case "ORLD":
                        _currentLD = ldStack.Pop().ORLD(_currentLD);
                        break;

                    case "OUT" when arg0.StartsWith("TR"):
                        _currentLD.OUTTR(new AuxNode(arg0), sentence);
                        break;
                    case "OUT":
                        _currentLD.OUT(arg0N, sentence);
                        break;
                    default:
                        break;
                }
            }

            Debug.Assert(ldStack.IsNullOrEmpty());
            Console.WriteLine("");
        }

        /// <summary>
        /// Rung 구축을 위한 단계로 사용된 Rung4Parsing 로부터 최종 Rung 을 생성해서 반환한다.
        /// Rung 구축 중간에 사용된 임시 node 들을 제거
        /// </summary>
        /// <returns></returns>
        public Rung ToRung()
        {
            var rung = new Rung(_mnemonics);
            rung.MergeGraph(_currentLD);
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

            return rung;
        }

    }

    /// <summary>
    /// Parsing 을 위해서 보조적으로 사용되는 node
    /// </summary>
    internal class AuxNode : Node
    {
        public AuxNode(string name) : base(name)
        {

        }
    }

    /// <summary>
    /// Rung 을 구성하는 내부의 sub rung 을 표현하기 위한 class
    /// </summary>
    internal class SubRung : Rung
    {
        AuxNode _start = new AuxNode("START");
        Node _end = new AuxNode("END");
        Rung4Parsing _masterRung;
        public SubRung(Rung4Parsing masterRung, string node)
            : this(masterRung, new Node(node))
        {
        }
        public SubRung(Rung4Parsing masterRung, Node node)
        {
            _masterRung = masterRung;
            Add(_start);
            Add(node);
            Add(_end);
            AddEdge(_start, node);
            AddEdge(node, _end);
        }

        public void AND(Node node, ILSentence sentence)
        {
            Add(node);
            MakeEndNode(node);
        }

        void MakeEndNode(Node start)
        {
            var oldEnd = _end;
            _end = new AuxNode("END");
            Add(_end);
            var incomingNodes = GetIncomingNodes(oldEnd).ToArray();
            incomingNodes.Iter(s =>
            {
                AddEdge(s, start);
                AddEdge(start, _end);
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

            GetIncomingNodes(_end).Iter(s =>
            {
                GetOutgoingNodes(next._start).Iter(e =>
                {
                    AddEdge(s, e, new Edge($"{s}->{e}"));
                });
            });

            return this;
        }

        /// <summary>
        /// this graph 내용에 other 로 주어진 graph 를 병합한다.
        /// </summary>
        public override void MergeGraph(Graph<Node, Edge> other)
        {
            base.MergeGraph(other);

            var kvs = _masterRung.TRmap.Where(kv => kv.Value == other).ToArray();
            foreach (var kv in kvs)
            {
                _masterRung.TRmap[kv.Key] = this;
            }
        }

        public void OR(Node node, ILSentence sentence)
        {
            Add(node);
            AddEdge(_start, node, new Edge(sentence));
            AddEdge(node, _end, new Edge(sentence));
        }

        public SubRung ORLD(SubRung next)
        {
            MergeGraph(next);

            AddEdge(next._start, _start);
            AddEdge(next._end, _end);

            return this;
        }
        public void OUT(Node node, ILSentence sentence)
        {
            Add(node);
            AddEdge(_end, node, new Edge(sentence));
        }

        public void OUTTR(AuxNode tr, ILSentence sentence)
        {
            _masterRung.TRmap.Add(tr, this);
            Add(tr);
            AddEdge(_end, tr, new Edge(sentence));
            _end = new AuxNode("END");
            Add(_end);
            AddEdge(tr, _end);
        }

        public void LDTR(string tr)
        {
            var trEntry = _masterRung.TRmap.First(kv => kv.Key.Name == tr);
            _end = trEntry.Key;
        }
    }
}
