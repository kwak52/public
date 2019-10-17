using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Core.ExtensionMethods;

namespace Dsu.PLCConvertor.Common
{
    public static class RungExtension
    {
        // https://www.graphviz.org/pdf/dotguide.pdf
        /// <summary>
        /// Rung 을 graphviz 를 이용해서 graph 생성한다.
        /// </summary>
        public static Image GraphViz(this Rung rung)
        {
            var tmpDot = Path.Combine(Path.GetTempPath(), "tmp.dot");
            var tmpPng = Path.Combine(Path.GetTempPath(), "tmp.png");
            File.WriteAllLines(tmpDot, GenerateDot(rung).ToArray());
            var start = new ProcessStartInfo()
            {
                FileName = @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe",
                Arguments = $"-Tpng {tmpDot} -o {tmpPng}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            using (Process process = Process.Start(start))
            using (StreamReader stdout = process.StandardOutput)
            using (StreamReader stderr = process.StandardError)
            {
                var result = stdout.ReadToEnd();
                var error = stderr.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    var msg = $"Error while converting dot file {tmpDot}:\r\n{error}";
                    //Logger.Error(msg);
                    Trace.WriteLine(msg);
                }
            }

            // Image.FromFile() locks file.
            Image img;
            using (var bmpTemp = new Bitmap(tmpPng))
                img = new Bitmap(bmpTemp);

            return img;
        }

        /// StoryBoard 내용에 기반하여 GraphViz 용 입력 text file (dot file) 을 생성한다.
        private static IEnumerable<string> GenerateDot(this Rung rung)
        {
            var subRung = rung as SubRung;
            if (subRung == null)
            {
                yield return
    @"digraph ""graph"" {
    rankdir=LR;
";
            }

            if (rung is Rung4Parsing)
            {
                var rung4parsing = rung as Rung4Parsing;
                int i = 0;
                foreach (var sr in rung4parsing.LadderStack)
                {
                    yield return $"  subgraph cluster{i} {{";
                    yield return $"\tlabel=\"stack #{i}\";";
                    i++;

                    foreach (var s in sr.GenerateDot())
                        yield return s;
                    yield return $"  }}";
                }

                if (rung4parsing.CurrentBuildingLD != null)
                {
                    foreach (var s in rung4parsing.CurrentBuildingLD.GenerateDot())
                        yield return s;
                }
            }

            yield return "\tnode [shape=record, fontsize=8, height=.1];";   // ,

            var query1 = rung.Nodes.Select(n =>
            {
                var styles = string.Join(", ", generateStyles()).NonNullEmptySelector("shape=rectangle");
                //return $"\t\"{GetId(n)}\" [{styles}, label=<{n.Name}>];";
                return $"\t\"{GetId(n)}\" [{styles}, label=<{n.ToShortString()}>];";

                IEnumerable<string> generateStyles()
                {
                    if (n is TerminalNode)
                    {
                        yield return "shape=ellipse";
                        yield return "style=filled, color=gray";
                    }

                    if (n is AuxNode)
                        yield return "style=dashed";

                    if (n is AuxNode)
                        yield return "color=blue";

                }
            });

            foreach (var t in query1)
                yield return t;

            // edge 그리기
            var showEdgeLabel = true;
            var query = rung.Edges.Select(e =>
            {
                var from = e.Start;
                var to = e.End;
                //(var from, var to) = (e.Start, e.End);
                var edgeStyle = "";
                //if (from is StartAction)
                //    edgeStyle += "color=red, ";
                var label = showEdgeLabel ? $"label=\"{e.Data.ToString()}\", " : "";
                return $"\t\"{GetId(from)}\" -> \"{GetId(to)}\" [{edgeStyle} {label} fontsize=8];";
            });

            foreach (var t in query)
                yield return t;

            if (subRung == null)
                yield return "}";

            //string HtmlEncode(string html) => System.Net.WebUtility.HtmlEncode(html);
            //string Small(string text, int size = 10, string color = "red") => $"<FONT POINT-SIZE=\"{size}\" COLOR=\"{color}\">{text}</FONT>";
            string GetId(Point a) => a.Guid.ToString();
        }


        /// <summary>
        /// start node 이하에서 coil output node 에 해당하는 node 들을 enumerate
        /// </summary>
        public static IEnumerable<Point> EnumerateTerminalNodes(this Rung rung, Point start)
        {
            return rung.DepthFirstSearch(start)
                .Where(n => /*n != start &&*/ rung.GetOutgoingDegree(n) == 0);
        }

        /// <summary>
        /// rung graㅔㅗ 
        /// </summary>
        /// <param name="rung"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IEnumerable<Point> ReverseDepthFirstSearch(this Rung rung, Point start)
        {
            var stack = new Stack<Point>();
            var visitedNodes = new HashSet<Point>();
            stack.Push(start);
            while (stack.Count > 0)
            {
                var curr = stack.Pop();
                if (!visitedNodes.Contains(curr))
                {
                    visitedNodes.Add(curr);
                    yield return curr;
                    IEnumerable<Point> adjs = rung.GetIncomingNodes(curr);
                    foreach (var next in adjs)
                    {
                        if (!visitedNodes.Contains(next))
                            stack.Push(next);
                    }
                }
            }
        }

        /// <summary>
        /// 주어진 node (start) 에서 시작해서 rung 의 앞쪽으로 검사하면서 node 가 circle 구성원인지 판별
        /// </summary>
        public static bool IsInCircularWithBackward(this Rung rung, Point start)
        {
            var isCircular =
                rung.GetIncomingNodes(start)
                    .Select(n => rung.ReverseDepthFirstSearch(n))
                    .EnumeratePaired()
                    .Any(pr => pr.First().Intersect(pr.Last()).Any())
                    ;
            if (isCircular)
                Global.Logger?.Debug($"Detected circular member node {start.Name}");

            return isCircular;
        }

        /// <summary>
        /// node 의 유일한 incoming node 를 반환한다.   incoming node 갯수가 하나가 아니면 fail 하거나 null 을 return 한다.
        /// </summary>
        public static Point GetTheIncomingNode(this Rung rung, Point node, bool returnNullOnFail)
        {
            var incNodes = rung.GetIncomingNodes(node).ToArray();
            if (incNodes.Length == 1)
                return incNodes[0];

            if (returnNullOnFail)
                return null;

            throw new Exception($"Incoming Node is not uniq.  Num incoming nodes={incNodes.Length}");            
        }

        /// <summary>
        /// node 의 유일한 outgoing node 를 반환한다.   outgoing node 갯수가 하나가 아니면 fail 하거나 null 을 return 한다.
        /// </summary>
        public static Point GetTheOutgoingNode(this Rung rung, Point node, bool returnNullOnFail)
        {
            var outgNodes = rung.GetOutgoingNodes(node).ToArray();
            if (outgNodes.Length == 1)
                return outgNodes[0];

            if (returnNullOnFail)
                return null;

            throw new Exception($"Outgoing Node is not uniq.  Num outgoing nodes={outgNodes.Length}");
        }

        /// <summary>
        /// rung 의 graph 에서 node 를 삭제한다.  단 rung graph 상에서 node 의 incoming / outgoing edge 는 서로 연결해서 복원한다.
        /// </summary>
        public static void OmitPoint(this Rung rung, Point node)
        {
            var ies = rung.GetIncomingEdges(node).ToArray(); // 컬렉션이 수정되었습니다.  열거 작업이 ..
            var oes = rung.GetOutgoingEdges(node).ToArray();
            ies.Iter(inE => {
                oes.Iter(outE =>
                {
                    var eData =
                        string.Join("+",
                            new string[] { inE.Data.Output, outE.Data.Output}
                            .Where(n => n.NonNullAny())
                            );

                    rung.AddEdge(inE.Start, outE.End, new Wire(eData));
                });
            });
            rung.Remove(node);
        }


        public static Point AddNode(this Rung rung, Point node)
        {
            rung.Add(node);
            return node;
        }
    }
}
