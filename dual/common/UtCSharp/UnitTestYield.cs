using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestYield
    {
        private IEnumerable<string> YieldSub1() { yield break; }
        private IEnumerable<string> YieldSub2() { yield return "sub2"; }
        
        private IEnumerable<string> YieldMain()
        {
            foreach (var s in YieldSub1())
                yield return s;

            foreach (var s in YieldSub2())
                yield return s;
        }
        [TestMethod]
        public void TestMethodYield()
        {
            foreach (var str in YieldMain())
            {
                Debug.WriteLine(str);
            }
        }
    }
}
