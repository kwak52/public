using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtContainerString
    {
        [TestMethod]
        public void TestMethodContainer_String()
        {
            string[] astr = new string[] {"Hello", "world", "!!"};
            Assert.AreEqual(astr[0], "Hello");

            // string[] ==> object[]
            object[] oastr = astr.Cast<object>().ToArray();
            Assert.AreEqual(oastr[0], "Hello");

            // string[] ==> System.Array
            Array sysastr = astr.ToArray();
            Assert.AreEqual(sysastr.GetValue(0), "Hello");

            // string[] ==> List<>
            List<string> lststr = astr.ToList();
            Assert.AreEqual(lststr[0], "Hello");

            // object[] ==> string[]
            string[] astr2 = oastr.Cast<string>().ToArray();
            Assert.AreEqual(astr2[0], "Hello");

            // System.Array ==> string[]
            string[] astr3 = sysastr.Cast<string>().ToArray();
            Assert.AreEqual(astr3[0], "Hello");

            // List<> ==> string[]
            string[] astr4 = lststr.ToArray();
            Assert.AreEqual(astr4[0], "Hello");
        }
    }
}
