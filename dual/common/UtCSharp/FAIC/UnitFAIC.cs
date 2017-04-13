using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.FAIC
{
    [TestClass]
    public class UtFAIC
    {
        delegate T OnDemand<T>();
        static OnDemand<T> CreateSimpleOnDemand<T>(T item)
        { return () => item; }

        static Nullable<int> ApplyFunction( Nullable<int> nullable, Func<int, int> function)
        {
            if (nullable.HasValue)
                return function(nullable.Value);

            return new Nullable<int>();
        }
        static OnDemand<int> ApplyFunction(OnDemand<int> onDemand, Func<int, int> function)
        {
            return new OnDemand<int>(() =>
            {
                var result = function(onDemand());
                return result;
            });
        }

        static Nullable<R> ApplyFunctionG<A, R>(Nullable<A> nullable, Func<A, R> function) where A : struct where R : struct
        {
            if (nullable.HasValue)
                return new Nullable<R>(function(nullable.Value));

            return new Nullable<R>();
        }

        static Nullable<int> AddOne(Nullable<int> nullable)
        {
            return ApplyFunction(nullable, n => n + 1);
        }

        static OnDemand<int> AddOne(OnDemand<int> onDemand)
        {
            return ApplyFunction(onDemand, n => n + 1);
        }


        [TestMethod]
        public void TestMethod1()
        {
            Nullable<int> n = 3;
            Assert.IsTrue(AddOne(n).Value == 4);

            OnDemand<int> o = CreateSimpleOnDemand(3);
            OnDemand<int> oPlus1 = AddOne(o);
            Assert.IsTrue(oPlus1() == 4);
        }
    }
}
