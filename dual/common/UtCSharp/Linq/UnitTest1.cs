using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharp.Test.Linq
{
    [TestClass]
    public class UnitTestLinqWhere
    {
        [TestMethod]
        public void TestMethodWhere()
        {
            // selects only even indexed items
            var odds = Enumerable.Range(0, 100).Where((n, i) => i % 2 == 0).ToList();
            Assert.IsTrue(odds.Count == 50);
        }
    }
}
