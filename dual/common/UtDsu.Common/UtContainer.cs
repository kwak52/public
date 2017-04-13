using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;
using System.Linq;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtContainer
    {
        [TestMethod]
        public void TestMethodContainer_Array()
        {
            Array arrEnums = Enum.GetValues(typeof(DialogResult));
            Assert.AreEqual(arrEnums.GetValue(0), DialogResult.None);
            Assert.AreEqual(arrEnums.GetValue(1), DialogResult.OK);

            {   // System.Array ==> List<>
                List<DialogResult> lstEnums = arrEnums.Cast<DialogResult>().ToList();
                Assert.AreEqual(lstEnums[0], DialogResult.None);
                Assert.AreEqual(lstEnums[1], DialogResult.OK);
            }

            {   // System.Array ==> List<>
                List<DialogResult> lstEnums = Tools.ToList<DialogResult>(arrEnums);
                Assert.AreEqual(lstEnums[0], DialogResult.None);
                Assert.AreEqual(lstEnums[1], DialogResult.OK);
            }

            // System.Array ==> object[]
            object[] objsEnums = arrEnums.Cast<object>().ToArray();
            Assert.AreEqual(objsEnums[0], DialogResult.None);
            Assert.AreEqual(objsEnums[1], DialogResult.OK);
        }


        [TestMethod]
        public void TestMethodContainer_ObjectArray()
        {
            object[] objsEnums = Enum.GetValues(typeof(DialogResult)).Cast<object>().ToArray();

            {   // object[] ==> System.Array
                Array arrEnums = objsEnums as Array;
                Assert.AreEqual(arrEnums.GetValue(0), DialogResult.None);
                Assert.AreEqual(arrEnums.GetValue(1), DialogResult.OK);
            }

            {   // object[] ==> List<> : For simple data type
                int[] ints = new[] { 10, 20, 10, 34, 113 };
                List<int> lst = ints.ToList();
            }

            {   // object[] ==> List<> : For custom data type
                List<DialogResult> lstEnums = objsEnums.Cast<DialogResult>().ToList();
                Assert.AreEqual(lstEnums[0], DialogResult.None);
                Assert.AreEqual(lstEnums[1], DialogResult.OK);
            }
        }

        [TestMethod]
        public void TestMethodContainer_List()
        {
            List<DialogResult> lstEnums = Enum.GetValues(typeof(DialogResult)).Cast<DialogResult>().ToList();
            Assert.AreEqual(lstEnums[0], DialogResult.None);
            Assert.AreEqual(lstEnums[1], DialogResult.OK);

            // List<> ==> System.Array
            Array arrEnums = lstEnums.ToArray();
            Assert.AreEqual(arrEnums.GetValue(0), DialogResult.None);
            Assert.AreEqual(arrEnums.GetValue(1), DialogResult.OK);

            // List<> ==> object[]
            object[] objsEnums = lstEnums.Cast<object>().ToArray();
            Assert.AreEqual(objsEnums[0], DialogResult.None);
            Assert.AreEqual(objsEnums[1], DialogResult.OK);
        }

        [TestMethod]
        public void TestMethodContainer_Enumerable()
        {
            Array arrEnums = Enum.GetValues(typeof(DialogResult));
            List<DialogResult> lstEnums = arrEnums.Cast<DialogResult>().ToList();

            {
                // List<> ==> IEnumerable<> : no conversion needed
                IEnumerable<DialogResult> enumerable = lstEnums;

                // IEnumerable<> ==> List<>
                List<DialogResult> lstEnums2 = enumerable.ToList();
                Assert.AreEqual(lstEnums2[0], DialogResult.None);
                Assert.AreEqual(lstEnums2[1], DialogResult.OK);
            }

            {
                // System.Array ==> IEnumeable
                IEnumerable<DialogResult> enumerable = arrEnums.Cast<DialogResult>();

                // IEnumerable<> ==> List<>
                List<DialogResult> lstEnums2 = enumerable.ToList();
                Assert.AreEqual(lstEnums2[0], DialogResult.None);
                Assert.AreEqual(lstEnums2[1], DialogResult.OK);
            }

        }
    }
}
