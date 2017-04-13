using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test
{

    public static class StringExtension
    {
        public static void Foo(this string s)
        {
            DEBUG.WriteLine("Foo invoked for {0}", s);
        }
    }

    [TestClass]
    public class UnitTestExtensionMethod
    {
        [TestMethod]
        public void TestMethodExtensionMethods()
        {
            string s = "Hello";
            s.Foo();
        }
    }
}
