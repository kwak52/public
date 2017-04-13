using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    /// <summary>
    /// pp. 57.  Functional Programming in C#.pdf
    /// </summary>
    [TestClass]
    public class UnitTestClosure
    {
        static private int Closures()
        {
            var result = GetClosureFunction()(30);
            Trace.WriteLine(result);
            return result;
        }

        private static Func<int, int> GetClosureFunction()
        {
            int val = 10;
            Func<int, int> internalAdd = x => x + val;
            Trace.WriteLine(internalAdd(10));
            val = 30;
            Trace.WriteLine(internalAdd(10));
            return internalAdd;
        }

        [TestMethod]
        public void TestMethodClosure()
        {
            Closures();
            // 순서대로 20, 40, 60 출력
        }


        private static Func<int, int> GetAFunc()
        {
            var myVar = 1;
            Func<int, int> inc = delegate(int var1)
            {
                myVar = myVar + 1;
                return var1 + myVar;
            };
            return inc;
        }

        /// <summary>
        /// http://www.codethinked.com/c-closures-explained
        /// </summary>
        [TestMethod]
        public void TestMethodClosure2()
        {
            var inc = GetAFunc();
            Trace.WriteLine(inc(5));
            Trace.WriteLine(inc(6));
            // 순서대로 7, 9
            Trace.WriteLine("Done");
        }


    }
}
