using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    /// <summary>
    /// pp. 80.  Functional Programming in C#.pdf
    /// </summary>
    public static class Functional
    {
        public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(
            this Func<T1, T2, TR> func)
        {
            return par1 => par2 => func(par1, par2);
        }
    }



    [TestClass]
    public class UnitTestCurrying
    {
        private int Square(int x) {  return x * x; }

        [TestMethod]
        public void TestMethodCurrying()
        {
            // this is invalid : compile error CS0815: Cannot assign lambda expression to an implicitly-typed local variable
            // var mult0 = (int x, int y) => x * y;

            // this is valid
            var curriedMult = Functional.Curry((int x, int y) => x * y);

            // this is valid, too
            var mult = new Func<int, int, int>((x, y) => x * y);

            //Func<int, int, int> mult = (x, y) => x * y;
            var curriedMult2 = mult.Curry();
            
            
            Func < int, int, int > add = (x, y) => x + y;
            var curriedAdd = Functional.Curry(add);
            var curriedAdd2 = Functional.Curry < int, int, int > ((x, y) => x + y);
            //curriedAdd(3)

            //Func < int, int, int > add = delegate (x, y) => x + y;
            //Func < int, Func < int, int > > curriedAdd = x = > (y = > x + y);

            var xxx = new Func<int,int> (t => t*t);          
        }

        [TestMethod]
        public void TestMethodCurrying2()
        {
            Func<int, int, int> add = (x, y) => x + y;
            Func<int,Func<int,int>> curriedAdd = add.Curry();
        }
    }
}
