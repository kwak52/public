using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtExtensionMethods
    {
        [TestMethod]
        public void TestMethodExtensionMethodArrayIsNullOrEmpty()
        {
            string[] strings = null;
            Assert.IsTrue(strings.IsNullOrEmpty());
            strings = new string[0];
            Assert.IsTrue(strings.IsNullOrEmpty());
            strings = new string[]{"a", "b"};
            Assert.IsFalse(strings.IsNullOrEmpty());


            List<string> lst = null;
            Assert.IsTrue(lst.IsNullOrEmpty());
            lst = new List<string>();
            Assert.IsTrue(lst.IsNullOrEmpty());
            lst = new List<string>(new string[]{"a", "b"});
            Assert.IsFalse(lst.IsNullOrEmpty());
        }

        [TestMethod]
        public void TestMethodExtensionMethodInRange()
        {
            Assert.IsTrue(3.InRange(3, 3));
            Assert.IsTrue(3.InRange(3, 4));
            Assert.IsTrue(3.InRange(1, 3));


            Assert.IsFalse(3.InRange(4, 5));
            Assert.IsFalse(3.InRange(4, 4));
            Assert.IsFalse(3.InRange(1, 2));
        }

        [TestMethod]
        public void TestMethodExtensionMethodClamp()
        {
            Assert.IsTrue(3.Clamp(1, 5) == 3);
            Assert.IsTrue(3.Clamp(4, 5) == 4);
            Assert.IsTrue(3.Clamp(1, 2) == 2);
            Assert.IsTrue(3.Clamp(1, 3) == 3);
            Assert.IsTrue(3.Clamp(3, 5) == 3);
        }
    }
}
