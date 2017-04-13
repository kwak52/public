using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtBoolean
    {
        [TestMethod]
        unsafe public void TestMethodBoolean()
        {
            bool f = false;
            using (new CBooleanEnabler(&f))
                Assert.IsTrue(f);
            Assert.IsFalse(f);


            using (new CBooleanDisabler(&f))
                Assert.IsFalse(f);
            Assert.IsFalse(f);

            using (new CBooleanToggler(&f))
                Assert.IsTrue(f);
            Assert.IsFalse(f);



            bool t = true;
            using (new CBooleanEnabler(&t))
                Assert.IsTrue(t);
            Assert.IsTrue(t);


            using (new CBooleanDisabler(&t))
                Assert.IsFalse(t);
            Assert.IsTrue(t);

            using (new CBooleanToggler(&t))
                Assert.IsFalse(t);
            Assert.IsTrue(t);
        }

        [TestMethod]
        public void TestMethodBooleanSetter()
        {
            bool myBool = true;
            BooleanHolder boolRef = new BooleanHolder(()=>myBool, (v) => { myBool = v; });
            using (new BooleanSetter(boolRef, false))
            {
                Assert.IsFalse(boolRef.Value);
                Assert.IsTrue(boolRef.Value == myBool);
            }
            Assert.IsTrue(boolRef.Value);
            Assert.IsTrue(boolRef.Value == myBool);


            boolRef.Value = false;
            using (new BooleanSetter(boolRef, false))
            {
                Assert.IsFalse(boolRef.Value);
                Assert.IsTrue(boolRef.Value == myBool);
            }
            Assert.IsFalse(boolRef.Value);
            Assert.IsTrue(boolRef.Value == myBool);

            boolRef.Value = false;
            using (new BooleanSetter(boolRef, true))
            {
                Assert.IsTrue(boolRef.Value);
                Assert.IsTrue(boolRef.Value == myBool);
            }
            Assert.IsFalse(boolRef.Value);
            Assert.IsTrue(boolRef.Value == myBool);

            boolRef.Value = true;
            using (new BooleanSetter(boolRef, true))
            {
                Assert.IsTrue(boolRef.Value);
                Assert.IsTrue(boolRef.Value == myBool);
            }
            Assert.IsTrue(boolRef.Value);
            Assert.IsTrue(boolRef.Value == myBool);
        }
    }
}
