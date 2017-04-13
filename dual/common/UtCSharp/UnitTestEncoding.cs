using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestEncoding
    {
        [TestMethod]
        public void TestMethodBase64()
        {
            var testSets = new string[]
            {
                "Hello, world!!!!",
                @"!@#$%^&*()_+-*/=\;|':,<.>/?`~",
            };

            foreach (var s in testSets)
            {
                var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s));
                var decoded = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encoded));
                Assert.IsTrue(s == decoded);                
            }
        }
    }
}
