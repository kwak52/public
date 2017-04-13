using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtPointerWrapper
    {
        //[TestMethod]
        //public void TestMethodPointerWrapper()
        //{
        //    PointerWrapperInt b = new PointerWrapperInt(0);
        //    Assert.IsTrue(b.Value == 0);
        //    PointerWrapperInt c = b;
        //    Assert.IsTrue(c.Value == 0);
        //    b.Value = 999;
        //    Assert.IsTrue(b.Value == 999);
        //}

        [TestMethod]
        public void TestMethodHolder()
        {
            int myValue = 1;

            var myRef = new ValueReferer<int>(() => myValue, (v) => { myValue = v; });
            Assert.IsTrue(myRef.Value == 1);
            myRef.Value = 2;
            Assert.IsTrue(myValue == 2);
        }

    }
}
