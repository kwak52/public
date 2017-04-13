using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestGeneric
    {
        private static class Generic
        {
            public static bool IsZero<T>(T value)
            {
                return value.Equals(Activator.CreateInstance(typeof(T)));
            }            
        }
        [TestMethod]
        public void TestMethodGenericZero()
        {
            Assert.IsTrue(Generic.IsZero(0));
            Assert.IsTrue(Generic.IsZero(0.0));
            Assert.IsTrue(Generic.IsZero(0L));
        }
    }
}
