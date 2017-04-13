using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    public class Dummy
    {
        public static bool IsCreated { get; private set; }
        public Dummy()
        {
            IsCreated = true;
            Trace.WriteLine("Created dummy object");
        }
    }
    /// <summary>
    /// http://stackoverflow.com/questions/6425133/lazy-initialization-in-net-4
    /// </summary>
    [TestClass]
    public class UnitTestLazyVariable
    {
        private Lazy<Dummy> _myDummy = new Lazy<Dummy>();

        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsFalse(Dummy.IsCreated);
            var x = _myDummy.Value;
            Assert.IsTrue(Dummy.IsCreated);
        }
    }
}
