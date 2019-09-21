using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Dsu.Common.Utilities.ExtensionMethods;

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
                var shape = "shape=rectangle;";
                return $"\t\"{GetId(n)}\" [{shape}label=<{n.Name}>];";
            });

            foreach (var t in query1)
                yield return t;

            // edge 그리기
            var showEdgeLabel = false;
            var query = rung.Edges.Select(e =>
            {
                var from = e.Start;
                var to = e.End;
                //(var from, var to) = (e.Start, e.End);
                var edgeStyle = "";
                //if (from is StartAction)
                //    edgeStyle += "color=red, ";
                var label = showEdgeLabel ? $"label=\"{e.Data.Name}\", " : "";

                //return $"\t\"{GetId(from)}\" -> \"{GetId(to)}\" [{edgeStyle} label = \"{e.Data.ToString()}\"];";
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
    }
}
