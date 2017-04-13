using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using LanguageExt;
using Dsu.Common.Utilities.ExtensionMethods;
namespace CSharp.Test.UnitTestLinqChoose
{
    [TestClass]
    public class UnitTestLinqChoose
    {
        [TestMethod]
        public void TestMethodLinqChoose()
        {
            // Choose : Filter + Map = where + select
            var ranges = Enumerable.Range(0, 100);
            var nums =
                ranges.Choose(n => n % 2 == 0 ? Option<string>.Some(n.ToString()) : Option<string>.None);
            System.Diagnostics.Trace.WriteLine(nums);

            var tensZeroths =   // {0, 10, 20, ..}
                ranges.EveryNth(10).ToArray();

            var tensFirsts =    // {1, 11, 21, ..}
                ranges.Skip(1).EveryNth(10).ToArray();

            var spans =
                ranges.SplitByN(10).ToArray();      // {{0..9}, {10..19}, ...}

            var spans0 = spans[0].ToArray();
            var spans7 = spans[7].ToArray();
            System.Diagnostics.Trace.WriteLine(spans);

        }
    }
}
