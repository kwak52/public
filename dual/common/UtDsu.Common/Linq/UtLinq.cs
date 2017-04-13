using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;
using static LanguageExt.Prelude;

namespace Dsu.Common.Utilities.Test
{
    [TestClass]
    public class UtLinq
    {
        [TestMethod]
        public void TestMethodGroupBy()
        {
            var list = new List<string> { "Foo1", "Foo2", "Foo3", "Foo2", "Foo3", "Foo3", "Foo1", "Foo1" };
            list.GroupBy(s => s).ForEach(g =>
            {
                Debug.WriteLine("{0} has {1} occurrence.", g.Key, g.Count());
                g.ForEach(e => 
                {
                    Debug.WriteLine("\t{0}({1})", e, e.GetType());                    
                });
            });
            /*
                Foo1 has 3 occurrence.
	                Foo1(System.String)
	                Foo1(System.String)
	                Foo1(System.String)
                Foo2 has 2 occurrence.
	                Foo2(System.String)
	                Foo2(System.String)
                Foo3 has 3 occurrence.
	                Foo3(System.String)
	                Foo3(System.String)
	                Foo3(System.String)
             */
            Debug.WriteLine("Done!");
        }

	    [TestMethod]
	    public void UtEnumrableApply()
	    {
		    var result = new Func<int, int>[] { (n) => n + 1, (n) => n * n }.Apply(new[] { 1, 3, 5 });
			Assert.IsTrue(result.SequenceEqual(new [] {2, 4, 6, 1, 9, 25}));
	    }

		/// <summary>
		/// Return -> Apply = Map 결과가 동일함을 보임.  http://fsharpforfunandprofit.com/posts/elevated-world/
		/// </summary>
		[TestMethod]
		public void UtEnumerableApplyVsMap()
		{
			var f = new Func<int, int>((n) => n + 1);
			var src = new[] {1, 3, 5};

			var returnApply = f.ReturnEnumerable().Apply(src);
			var map = src.Map(f);
			var select = src.Select(f);

			Assert.IsTrue(returnApply.SequenceEqual(new [] {2, 4, 6}));
			Assert.IsTrue(returnApply.SequenceEqual(map));
			Assert.IsTrue(map.SequenceEqual(select));
		}

		[TestMethod]
		public void UtOptionApplyVsMap()
		{
			var f = new Func<int, int>((n) => n + 1);
			var src = 2.ToOption();

			var returnApply = f.ToOption().Apply(src);
			var map = f.MapOption(src);

			Assert.IsTrue(returnApply.IsSome);
			Assert.IsTrue(returnApply.GetValueUnsafe() == 3);
			Assert.IsTrue(map == returnApply);
		}

		[TestMethod]
		public void UtTryApplyVsMap()
		{
			var f = new Func<int, int>((n) => n + 1);
			var src = 2.ToTry();

			var returnApply = f.ToTry().Apply(src);
			var map = f.MapTry(src);


			int result1 = match(returnApply,
				Succ: v => v,
				Fail: -1);
			int result2 = match(map,
				Succ: v => v,
				Fail: -1);
			Assert.IsTrue(result1 == 3);
			Assert.IsTrue(result2 == 3);
		}


		[TestMethod]
		public void TestMethodLift2()
		{
			var f = new Func<int, int, string>((n1, n2) => $"{n1}.{n2}");
			var src1 = new[] { 1, 3, 5 };
			var src2 = new[] { 11, 33, 55 };

			var result = EmLinq.Lift2(src1, src2, f);
			Assert.IsTrue(result.SequenceEqual(new []{"1.11", "3.33", "5.55"}));
		}

		[TestMethod]
		public void TestMethodLift3()
		{
			var f = new Func<int, int, int, string>((n1, n2, n3) => $"{n1}.{n2}.{n3}");
			var src1 = new[] { 1, 3, 5 };
			var src2 = new[] { 11, 33, 55 };
			var src3 = new[] { 111, 333, 555 };

			var result = EmLinq.Lift3(src1, src2, src3, f);
			Assert.IsTrue(result.SequenceEqual(new[] { "1.11.111", "3.33.333", "5.55.555" }));
		}
	}
}
