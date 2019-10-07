using Dsu.Common.Utilities.Graph;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// Rung 을 구성하는 내부의 sub rung 을 표현하기 위한 class
    /// </summary>
    internal class SubRung : Rung
    {
        StartNode _start = new StartNode("S");
        EndNode _end = new EndNode("E");

        /// <summary>
        /// Stack Rung 이 아닌, 현재 build 중인 current active ladder
        /// </summary>
        Rung4Parsing _masterRung;
        public SubRung(Rung4Parsing masterRung, Point node)
        {
            _masterRung = masterRung;
            Add(_start);
            Add(node);
            Add(_end);
            AddEdge(_start, node, new Wire("//LD//3"));
            AddEdge(node, _end);

            _start.EndNode = _end;
            _end.StartNode = _start;
        }


        /// <summary>
        /// oldEnd - node - newEnd
        /// </summary>
        void AppendToEndNode(Point node)
        {
            Add(node);

            var theIncomingEnd = this.GetTheIncomingNode(_end, true);
            if (theIncomingEnd != null)
            {
                AddEdge(theIncomingEnd, node);
                AddEdge(node, _end);
                RemoveEdge(theIncomingEnd, _end);
            }
            else
            {
                AddEdge(_end, node);
                _end = new EndNode(node.Name);    // new end
                Add(_end);
                AddEdge(node, _end);
            }

        }

        public void AND(Point node, ILSentence sentence)
        {
            AppendToEndNode(node);
        }


        /// <summary>
        /// this Ladder 와 next ladder 를 AND 결합한다.
        /// </summary>
        public SubRung ANDLD(SubRung next)
        {
            MergeGraph(next);
            AddEdge(_end, next._start);
            var dummy = this.AddNode(new DummyNode("Dummy"));
            AddEdge(next._end, dummy, new Wire("ANDLD//999"));
            var newEnd = this.AddNode(new EndNode("END//123")) as EndNode;
            AddEdge(dummy, newEnd);

            _end = newEnd;

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
            AddEdge(_start, node, new Wire($"//OR//1:{sentence}"));
            AddEdge(node, _end);    //, new Wire($"OR//2:{sentence}"));
        }

        public SubRung ORLD(SubRung next)
        {
            MergeGraph(next);

            AddEdge(_start, next._start, new Wire($"//ORLD//1"));
            AddEdge(next._end, _end, new Wire($"ORLD//2"));

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
            AddEdge(_end, tr);
            _end = new EndNode(tr.Name);
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
            _end = new EndNode(tn.Name);
            Add(_end);
            AddEdge(tn, _end);
        }
    }
}
