using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtRTTI
    {
        [TestMethod]
        public void TestMethodRTTI()
        {
            List<string> lststr = new List<string>();
            Assert.IsTrue(RTTI.HasProperty_p(lststr.GetType(), "Count"));
            Assert.IsTrue(RTTI.HasProperty_p(typeof(List<string>), "Count"));
            Assert.IsTrue(RTTI.HasMethod_p(typeof(List<string>), "Add"));

            Assert.IsTrue(!RTTI.HasMethod_p(typeof(List<string>), "SomeNonExistingMethod"));
            Assert.IsTrue(!RTTI.HasProperty_p(lststr.GetType(), "SomeNonExistingProperty"));

            List<string> lstprop = RTTI.GetProperties(lststr.GetType());
            Assert.IsTrue(lstprop.Contains("Capacity"));
            Assert.IsTrue(lstprop.Contains("Count"));
            Assert.IsTrue(lstprop.Contains("Item"));

            Assert.IsTrue(Tools.IsNullOrEmpty(new List<string>()));
            Assert.IsTrue(Tools.IsNullOrEmpty(null));
            Assert.IsTrue(Tools.IsNullOrEmpty(""));

            string strExceptionMessage = null;
            try { bool b = Tools.IsNullOrEmpty(3); }        // runtime error should be raised
            catch (System.Exception ex) { strExceptionMessage = ex.Message; }
            Assert.IsTrue(strExceptionMessage != null);

            strExceptionMessage = null;
            int n = 3;
            try { bool b = Tools.IsNullOrEmpty(n); }        // runtime error should be raised
            catch (System.Exception ex) { strExceptionMessage = ex.Message; }
            Assert.IsTrue(strExceptionMessage != null);
        }
    }
}
