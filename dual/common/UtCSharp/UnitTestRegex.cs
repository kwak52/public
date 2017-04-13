using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestRegex
    {
        [TestMethod]
        public void TestMethodRegex()
        {
            Assert.IsTrue(Regex.Replace(@"`~!@#$^&*()", @"[`~!@#$^&*()]", "").IsNullOrEmpty());
            Assert.IsTrue(Regex.Replace(@"`~!@#$^&*()", @"[`~!@#$^&*()]", "-") == "-----------");
            Assert.IsTrue(Regex.Replace(@"Hello`~!@#$^&*()World", @"[`~!@#$^&*()]", "-") == "Hello-----------World");

            Assert.IsTrue(Regex.Replace(@"\\/", @"[\\/]", "").IsNullOrEmpty());
        }
    }
}
