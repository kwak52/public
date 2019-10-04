using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Graph;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// Rung 을 구성하는 내부의 sub rung 을 표현하기 위한 class
    /// </summary>
    internal class SubRung : Rung
    {
        StartMarker _start = new StartMarker("S");
        Point _end = new EndMarker("E");

        /// <summary>
        /// Stack Rung 이 아닌, 현재 build 중인 current active ladder
        /// </summary>
        Rung4Parsing _masterRung;
        //public SubRung(Rung4Parsing masterRung, string node)
        //    : this(masterRung, new Point(node))
        //{
        //}
        public SubRung(Rung4Parsing masterRung, Point node)
        {
            _masterRung = masterRung;
            Add(_start);
            Add(node);
            Add(_end);
            AddEdge(_start, node);
            AddEdge(node, _end);
        }


        /// <summary>
        /// oldEnd - node - newEnd
        /// </summary>
        void AppendToEndNode(Point node)
        {
            Add(node);
            AddEdge(_end, node);
            _end = new EndMarker(node.Name);    // new end
            AddEdge(node, _end);
        }

        void MakeEndNode(Point start)
        {
            var oldEnd = _end;
            _end = new EndMarker(start.Name);
            Add(_end);
            var incomingNodes = GetIncomingNodes(oldEnd).ToArray();
            incomingNodes.Iter(s =>
            {
                AddEdge(s, start);
                AddEdge(start, _end);
            });

            if (!oldEnd.Name.StartsWith("TR"))
                Remove(oldEnd);
        }
        public void AND(Point node, ILSentence sentence)
        {
            Add(node);
            MakeEndNode(node);
        }


        /// <summary>
        /// this Ladder 와 next ladder 를 AND 결합한다.
        /// </summary>
        public SubRung ANDLD(SubRung next)
        {
            MergeGraph(next);
            var s = _end;
            var e = next._start;
            AddEdge(s, e, new Wire($"{s}->{e}"));
            _end = next._end;

            return this;
        }

        /// <summary>
        /// this graph 내용에 other 로 주어진 graph 를 병합한다.
        /// </summary>
        public override void MergeGraph(Graph<Point, Wire> other)
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
            var oldEnd = _end;
            _end = new EndMarker(node.Name);
            Add(_end);
            AddEdge(oldEnd, _end);

            AddEdge(_start, node, new Wire(sentence));
            AddEdge(node, _end, new Wire(sentence));
        }

        public SubRung ORLD(SubRung next)
        {
            MergeGraph(next);

            AddEdge(_start, next._start);
            AddEdge(_end, next._end);

            _end = next._end;

            return this;
        }

        public void OUT(Point node, ILSentence sentence)
        {
            Add(node);
            AddEdge(_end, node, new Wire(sentence));
        }

        public void OUTTR(AuxNode tr, ILSentence sentence)
        {
            _masterRung.TRmap.Add(tr, this);
            Add(tr);
            AddEdge(_end, tr, new Wire(sentence));
            _end = new EndMarker(tr.Name);
            Add(_end);
            AddEdge(tr, _end);
        }

        /// <summary>
        /// TR register 위치를 찾아서 그 다음 위치를 새로운 _end position 으로 잡아 준다.
        /// </summary>
        /// <param name="tr"></param>
        public void LDTR(string tr)
        {
            var trEntry = _masterRung.TRmap.First(kv => kv.Key.Name == tr);
            var tn = trEntry.Key;
            _end = new EndMarker(tn.Name);
            Add(_end);
            AddEdge(tn, _end);
        }
    }
}
