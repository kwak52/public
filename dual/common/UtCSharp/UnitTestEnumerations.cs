using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CSharp.Test
{
    public class HelloCollection
    {
        public IEnumerator<string> GetEnumerator()
        {
            yield return "Hello";
            yield return "World";
        }
    }

    public class HelloWorld
    {
        public IEnumerable<string> GetGreetings()
        {
            yield return "Hello";
            yield return "World";            
        }
    }


    [TestClass]
    public class UnitTestEnumerations
    {
        [TestMethod]
        public void TestEnumeration()
        {
            var hc = new HelloCollection();
            foreach (var str in hc)
            {
                Trace.WriteLine(str);
            }
            var enumerator = hc.GetEnumerator();


            
            var hw = new HelloWorld();
            foreach (var str in hw.GetGreetings() )
            {
                Trace.WriteLine(str);
            }
            // var enumerator2 = hw.GetEnumerator();       // invalid
        }

        [TestMethod]
        public void TestEmlinq()
        {
            Assert.IsTrue(new [] { 1, 1, 1, 1}.AllEqual());
            Assert.IsTrue(new[] { 1, 1, 1, 1 }.AllEqual(1));
            Assert.IsFalse(new[] { 1, 1, 1, 1 }.AllEqual(2));
            Assert.IsFalse(new[] { 0, 1, 1, 1 }.AllEqual());
            Assert.IsFalse(new[] { 0, 0, 1, 1 }.AllEqual());

            Assert.IsTrue(new[] { "Hello", "Hello", "Hello", "Hello",  }.AllEqual());
        }
    }
}
