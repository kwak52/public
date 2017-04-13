using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities.Test
{
    [TestClass]
    public class UtEmLinq
    {
        [TestMethod]
        public void TestMethodTee()
        {
            var x = Enumerable.Range(1, 10).Select(n => n.Tee((i) => Trace.WriteLine(String.Format("Inner log={0}", i))));
            //var x = 3.Tee(() => Trace.WriteLine("3"));

            Trace.WriteLine("X is :" + x);
        }

        [TestMethod]
        public void TestMethodForEach()
        {
            var nums = Enumerable.Range(1, 10)
                .ForEachTee(n => Trace.WriteLine(String.Format("first loop ={0}", n)))
                .ForEachTee(n => Trace.WriteLine(String.Format("second loop ={0}", n)))
                .Take(5)
                .ToArray()      // purge from lazy evaluation
                ;
			Assert.IsTrue(nums.SequenceEqual(Enumerable.Range(1, 5)));
			Trace.WriteLine("Done");
        }

		[TestMethod]
		public void TestMethodSplitByN()
		{
			var chunks = Enumerable.Range(1, 10).SplitByN(3).ToArray();
			Assert.IsTrue(chunks.Length == 4);
			Assert.IsTrue(chunks[0].SequenceEqual(new int[] { 1, 2, 3 }));
			Assert.IsTrue(chunks[1].SequenceEqual(new int[] { 4, 5, 6 }));
			Assert.IsTrue(chunks[2].SequenceEqual(new int[] { 7, 8, 9 }));
			Assert.IsTrue(chunks[3].SequenceEqual(new int[] { 10 }));
			Trace.WriteLine("Done");
		}

	    [TestMethod]
	    public void TestMethodOfNotNull()
	    {
			var arr = new[] { "a", null, "b" }.OfNotNull().ToArray();
			Assert.IsTrue(arr.Length == 2);
			Assert.IsTrue(arr[0] == "a");
			Assert.IsTrue(arr[1] == "b");
	    }
	}
}
