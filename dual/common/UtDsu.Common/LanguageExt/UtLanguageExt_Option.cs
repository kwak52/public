using System;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using LanguageExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LanguageExt.Prelude;

namespace Ut.Dsu.Common.LanguageExt
{
	[TestClass]
	public class UtLanguageExt_Option
	{
		private Option<int> MyOption { get; }

		[TestMethod]
		public void UtOption()
		{
			match(parseInt("123"),
				Some: v => Assert.IsTrue(v == 123),
				None: () => Assert.IsFalse(true)
			);

			int n =
			match(parseInt("123"),
				Some: v => v,
				None: () => 0
			);

			Assert.IsTrue(n == 123);
		}


		[TestMethod]
		public void UtOptionDefault()
		{
			// Option<> 은 struct 이므로 null 일 수 없다.  또한 초기화를 위해서 new 로 생성해 주지 않아도 된다.
			Assert.IsNotNull(MyOption);
			Assert.IsTrue(MyOption.IsNone);


			var defaultInt = new Option<int>();
			Assert.IsTrue(defaultInt.IsNone);

			defaultInt = 3;
			Assert.IsTrue(defaultInt.IsSome);

			defaultInt = None;
			Assert.IsTrue(defaultInt.IsNone);
		}

		[TestMethod]
		[ExpectedException(typeof(ValueIsNoneException))]
		public void UtOptionExtractValue()
		{

			var opt = Some(3);
			Assert.IsTrue(opt.GetType() == typeof(Option<int>));

			Assert.IsTrue(opt.GetValueUnsafe() == 3);

			opt = None;
			Assert.IsTrue(opt.IsNone);
			Assert.IsTrue(opt.IfNone(-1) == -1);
			var crash = opt.GetValueUnsafe();       // throws ValueIsNoneException on None
		}


		[TestMethod]
		public void UtOptionSelectSomes()
		{
			var options = new[]
			{
				Some(1),		// or Option<int>.Some(1)
				None,			// or Option<int>.None
				Some(2),
				None,
			};

			Assert.IsTrue(options.Somes().Sum() == 3);

			Assert.IsTrue(options[0].GetValueUnsafe() == 1);
		}
	}
}
