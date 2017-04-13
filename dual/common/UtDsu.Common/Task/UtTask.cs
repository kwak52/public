using System;
using Dsu.Common.Utilities.Core.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Dus.Common.Task
{
	[TestClass]
	public class UtTask
	{
		[TestMethod]
		public void UtTaskMap()
		{
			var t1 = 1.FromResult();
			var t2 = t1.TaskMap((n1) => { return n1 + 1; });
			int result = t2.Result;
			Assert.IsTrue(result == 2);
		}


		[TestMethod]
		public void UtTaskBind()
		{
			var t1 = 1.FromResult();
			var t2 = t1.TaskBind((n1) => { return (n1 + 1).TaskReturn(); });
			int result = t2.Result;
			Assert.IsTrue(result == 2);
		}
	}
}
