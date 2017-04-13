using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtMacro
    {
        [TestMethod]
        public void TestMethodMacro()
        {
            Assert.IsTrue(!3.InClosedRange(0, 1));
            Assert.IsTrue(1.InClosedRange(0, 3));
            Assert.IsTrue(Tools.Overlap_p(0, 5, 1, 3));
            Assert.IsTrue(Tools.Overlap_p(0, 5, 4, 6));
            Assert.IsTrue(Tools.Overlap_p(0, 5, -1, 2));
            Assert.IsTrue(!Tools.Overlap_p(0, 5, 6, 7));

            Assert.IsTrue(1.IsOneOf(0, 1, 2));
            Assert.IsTrue(!5.IsOneOf(0, 1, 2));
            Assert.IsTrue("One".IsOneOf("Zero", "One", "Two"));
            Assert.IsTrue(!"Five".IsOneOf("Zero", "One", "Two"));
        }
    }
}
