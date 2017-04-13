using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestArrayReference
    {
        [TestMethod]
        public void TestMethodArrayReference()
        {
            int[] src = new int[] { 1, 2, 3, 4 };
            int[] tgt = src;    // reference the same array
            tgt[0] = 999;
            Assert.IsTrue(src[0] == 999);
            src[0] = 1;

            tgt = src.Select(n => n + 1).ToArray();
            Assert.IsTrue(src[0] == 1);
            Assert.IsTrue(tgt[0] == 2);

            tgt = src.ToArray();    // ToArray() copies the array
            src[0] = 999;
            Assert.IsTrue(tgt[0] == 1);
        }
    }
}
