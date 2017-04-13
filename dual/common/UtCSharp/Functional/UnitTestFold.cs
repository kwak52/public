using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.FunctionalP
{
    [TestClass]
    public class UnitTestFold
    {
        /// <summary>
        /// fold == reduce == aggregate
        /// </summary>
        // Signature : (a -> b -> a) -> E<b> -> a -> a
        // (func) -> E<b> -> a(==seed) -> a(==sum)
        [TestMethod]
        public void TestMethodFunctionalFold()
        {
            int fact10 = Enumerable.Range(1, 10).Aggregate(1, (n1, n2) => n1*n2);
            Assert.IsTrue(fact10 == 3628800);   // 10!
            Trace.WriteLine(fact10);
        }
    }
}
