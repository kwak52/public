using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtMinMax
    {
        [TestMethod]
        public void TestMethodMinMax()
        {
            Assert.AreEqual((int)Tools.max(1, 2), 2);
            Assert.AreEqual((int)Tools.max(1, 2, 3, 4, 5, 6, 7, 8, 9, 10), 10);
            Assert.AreEqual((int)Tools.min(1, 2), 1);
            Assert.AreEqual((int)Tools.min(1, 2, 3, 4, 5, 6, 7, 8, 9, 10), 1);

            Assert.AreEqual((string)Tools.min("A", "B", "C"), "A");
            Assert.AreEqual((string)Tools.max("A", "B", "C"), "C");


            Assert.AreEqual((double)Tools.max(1, 2), 2);
            Assert.AreEqual((double)Tools.max(1, 2, 3, 4, 5, 6, 7, 8, 9, 10), 10);
            Assert.AreEqual((double)Tools.min(1, 2), 1);
            Assert.AreEqual((double)Tools.min(1, 2, 3, 4, 5, 6, 7, 8, 9, 10), 1);


            int[] arrInt = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] arrDbl = { 1.1, 2, 3.9, 3, };

            string strExceptionMessage = null;
            try { int n = Tools.max<int>(null); }
            catch (System.Exception ex) { strExceptionMessage = ex.Message; }
            Assert.IsTrue(strExceptionMessage != null);

            strExceptionMessage = null;
            try { int n = Tools.min<int>(null); }
            catch (System.Exception ex) { strExceptionMessage = ex.Message; }
            Assert.IsTrue(strExceptionMessage != null);


            Assert.AreEqual(Tools.max<int>(arrInt.ToArray()), 10);
            Assert.AreEqual(Tools.min<int>(arrInt.ToArray()), 1);
            Assert.AreEqual(Tools.max<double>(arrDbl.ToArray()), 3.9);
            Assert.AreEqual(Tools.min<double>(arrDbl.ToArray()), 1.1);


            Assert.AreEqual(Tools.mid3(1, 2, 3), 2);
            Assert.AreEqual(Tools.mid3(1, 3.5, 5), 3.5);
            Assert.AreEqual(Tools.avg(1, 2, 3, 4), 2.5);

            Assert.AreEqual(Tools.avg(arrDbl), 2.5);
        }
    }
}
