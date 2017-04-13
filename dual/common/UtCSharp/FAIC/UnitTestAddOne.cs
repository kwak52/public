// http://ericlippert.com/2013/03/04/monads-part-three/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.FAIC
{
    [TestClass]
    public class UnitTestAddOne
    {
        delegate T OnDemand<T>();

        static Nullable<T> CreateSimpleNullable<T>(T item) where T : struct
        { return new Nullable<T>(item); }
        static OnDemand<T> CreateSimpleOnDemand<T>(T item)
        { return () => item; }
        static IEnumerable<T> CreateSimpleSequence<T>(T item)
        { yield return item; }

        static Lazy<T> CreateSimpleLazy<T>(T item)
        { return new Lazy<T>(new Func<T>(() => { return item; })); }

        static Nullable<int> AddOne(Nullable<int> nullable)
        {
            if (nullable.HasValue)
                return new Nullable<int>(nullable.Value + 1);

            return new Nullable<int>();
        }
        static OnDemand<int> AddOne(OnDemand<int> onDemand)
        {
            return () =>
            {
                int unwrapped = onDemand();
                int result = unwrapped + 1;
                return result;
            };
        }

        static Lazy<int> AddOne(Lazy<int> lazy)
        {
            return new Lazy<int>(() =>
            {
                return lazy.Value + 1;
            });
            
        }

        static async Task<int> AddOne(Task<int> task)
        {
            return 1 + await task;
        }

        static IEnumerable<int> AddOne(IEnumerable<int> seq)
        {
            foreach (var v in seq)
            {
                yield return v + 1;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            Nullable<int> n = 3;
            var x = n + 2;
            Nullable<int> y = x;

            OnDemand<int> o = CreateSimpleOnDemand(3);
            var oo = AddOne(o);
            OnDemand<int> k = () => DateTime.Now.Second;
            var pp = AddOne(k);
            var ppp = pp;
            Assert.IsTrue(oo() == CreateSimpleOnDemand(4)());
        }
    }
}
