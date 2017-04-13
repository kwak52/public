using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.Graph;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtGraph
    {
        [TestMethod]
        public void TestMethodGraph()
        {
            Graph<int, string> g = new Graph<int, string>();
            g.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            g.AddEdge(1, 2, "1->2");
            g.AddEdge(2, 3, "2->3");
            g.AddEdge(3, 4, "3->4");
            g.AddEdge(5, 2, "5->2");
            g.AddEdge(7, 8, "7->8");
            g.Dump();

            foreach (Graph<int, string> subG in g.GetConnectedComponents())
            {
                if (subG.NodeCount == 1)
                    continue;

                subG.Dump();

                for (int i = 0; ; i++)
                {
                    var path = subG.PullOutPath();
                    if (path == null || path.Count() == 0)
                        break;

                    foreach (Edge<int, string> e in path)
                    {
                        DEBUG.WriteLine("Path{0} : {1}", i, e.Data.ToString());
                    }

                }
            }

        }
    }
}
