using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    internal class Rung4Parsing : Rung
    {
        SubRung _currentLD;
        public Rung4Parsing(IEnumerable<string> mnemonics)
        {
            var ldStack = new Stack<SubRung>();

            foreach (var m in mnemonics)
            {
                var sentence = new ILSentence(m);
                var arg0 = new Node(sentence.Args[0]);
                switch(sentence.Command)
                {
                    case "LD" when sentence.Args[0].StartsWith("TR"):
                        throw new NotImplementedException();

                    case "LD":
                        if (_currentLD != null)
                            ldStack.Push(_currentLD);
                        _currentLD = new SubRung(arg0);
                        break;

                    case "AND":
                        _currentLD.AND(arg0, sentence);
                        break;
                    case "ANDLD":
                        _currentLD = ldStack.Pop().ANDLD(_currentLD);
                        break;

                    case "OR":
                        _currentLD.OR(arg0, sentence);
                        break;
                    case "ORLD":
                        _currentLD = ldStack.Pop().ORLD(_currentLD);
                        break;

                    case "OUT":
                        _currentLD.OUT(arg0, sentence);
                        break;
                    default:
                        break;
                }
            }

            Debug.Assert(ldStack.IsNullOrEmpty());
            Console.WriteLine("");
        }

        public Rung ToRung()
        {
            var rung = new Rung();
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
    internal class SubRung : Rung
    {
        AuxNode _start = new AuxNode("START");
        Node _end = new AuxNode("END");
        public SubRung(string node)
            : this(new Node(node))
        {
        }
        public SubRung(Node node)
        {
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
            var ies = GetIncomingNodes(oldEnd).ToArray();
            ies.Iter(s =>
            {
                AddEdge(s, start);
                AddEdge(start, _end);
            });
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

        public void OR(Node node, ILSentence sentence)
        {
            Add(node);
            AddEdge(_start, node, new Edge(sentence));
            AddEdge(node, _end, new Edge(sentence));
        }

        public SubRung ORLD(SubRung next)
        {
            throw new NotImplementedException();
        }
        public void OUT(Node node, ILSentence sentence)
        {
            Add(node);
            AddEdge(_end, node, new Edge(sentence));
        }

    }
}
