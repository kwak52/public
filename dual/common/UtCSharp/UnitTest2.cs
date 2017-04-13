using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    public static class MyAny
    {
        public static bool DoesAny<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                return false;
            return source.Any();
        }
        
    }

    [TestClass]
    public class UnitTestLinq
    {
        [TestMethod]
        public void TestMethodLinq()
        {
            string[] strings = null;
            Assert.IsFalse(strings.DoesAny());
        }
    }
}
