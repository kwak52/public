using System;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using LanguageExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LanguageExt.Prelude;

namespace Ut.Dsu.Common
{
	[TestClass]
	public class UtLanguageExt_Try
	{
		private Try<Uri> CreateUri(string uri) => () => new Uri(uri);

		[TestMethod]
		public void UtTryCatch()
		{
			Try<Uri> uriTry = CreateUri("http://github.com");

			uriTry.Match(
				Succ: uri => Assert.IsTrue(uri != null),
				Fail: ex => Assert.IsFalse(true)
			);

			match(uriTry,
				Succ: uri => Assert.IsTrue(uri != null),
				Fail: ex => Assert.IsFalse(true)
			);

			Uri uriSuccess = match(uriTry,
				Succ: uri => uri,
				Fail: ex => null
			);
			Assert.IsNotNull(uriSuccess);


			var uriSuccess2 = match(Try(() => new Uri("http://github.com")),
				Succ: uri => uri,
				Fail: ex => null
			);
			Assert.IsNotNull(uriSuccess2);

			Uri uriFail = match(CreateUri("rubbish"), 
				Succ: uri => uri,
				Fail: ex => null
			);
			Assert.IsNull(uriFail);





			uriTry = CreateUri("rubbish");

			uriTry.Match(
				Succ: uri => Assert.IsFalse(true),
				Fail: ex =>
				{
					Assert.IsTrue(ex != null);
				}
			);
		}


		[TestMethod]
		public void UtTryEnumerable()
		{
			var strNums = new[] {null, "0.0", "1", "0.1", null, "3", "5"};
			var tries = strNums
				.Select(s => Try(() =>
					{
						Trace.WriteLine($"Evaluating {s?? "null"}");
						return Int32.Parse(s);
					}))
				.ToArray()
				;

			Assert.IsTrue(tries.Length == strNums.Length);
			Trace.WriteLine(tries[0].GetType());

			var optResult = tries[2].ToOption();
			Assert.IsTrue(optResult.IsSome);
			Assert.IsTrue(optResult == 1);
			Assert.IsTrue(tries[0].ToOption().IsNone);
			Assert.IsTrue(tries[1].ToOption().IsNone);
			Assert.IsTrue(tries[1].ToOption() == None);
			Assert.IsTrue(tries[2].ToOption() == 1);
			Assert.IsTrue(tries[3].ToOption() == None);
			Assert.IsTrue(tries[4].ToOption() == None);
			Assert.IsTrue(tries[5].ToOption() == 3);
			Assert.IsTrue(tries[6].ToOption() == 5);


			var numerics = tries.Select(n => n.ToOption()).Somes();
			Assert.IsTrue(numerics.SequenceEqual(new [] {1, 3, 5}));


			int n2 = tries[2].Match(
				Succ: v => v,
				Fail: ex => -1
				);
			Assert.IsTrue(n2 == 1);
		}
	}
}
