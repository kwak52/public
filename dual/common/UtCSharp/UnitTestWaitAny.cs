using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestWaitAny
    {
        [TestMethod]
        public void TestLiteralTypes()
        {
            Assert.IsTrue(1.0.GetType().ToString() == "System.Double");
            Assert.IsTrue(1E06.GetType().ToString() == "System.Double");
            Assert.IsTrue(1.GetType().ToString() == "System.Int32");
            Assert.IsTrue(0xF0000000.GetType().ToString() == "System.UInt32");
            Assert.IsTrue(1.23M.GetType().ToString() == "System.Decimal");

            Assert.IsTrue(1.0.GetType() == typeof(System.Double));

            Assert.IsTrue(1.0.GetType().Name == "Double");      // not "System.Double"
        }

        [TestMethod]
        public void TestConvertRounded()
        {
            // 단순 casting 은 round 하지 않으나, System.Convert 는 round 를 고려한다.
            Assert.IsTrue((int)1.99999 == 1);
            Assert.IsTrue(System.Convert.ToInt32(1.99999) == 2);
        }


        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void TestDivideByZero()
        {
            // int a = 2/0;        // compile error
            int b = 0;
            int c = 5 / b;          // DivideByZeroException exception 을 발생시켜야 한다.
        }

        [TestMethod]
        public void TestArithematicOverflowSilent()
        {
            int a = int.MaxValue;

            // OverflowException will not triggered : not checked context
            a++;
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void TestArithematicOverflowExplicit()
        {
            int a = int.MaxValue;

            // compiler switch : /checked+   see unchecked

            //int c = 0;
            //c = checked(a++);
            checked
            {
                a++;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestIndexOutOfRange()
        {
            int[] arr = new int[3];
            arr[3] = 1; // IndexOutOfRangeException thrown
        }

        [TestMethod]
        public void TestDefaultValues()
        {
            int n = default(int);
            Assert.IsTrue(n == 0);
            Assert.IsTrue(default(object) == null);
        }


        [TestMethod]
        public void TestBoxingUnboxing()
        {
            // Boxing copies the value-type instance into the new object, and 
            // unboxing copies the contents of the object back into a value-type instance. 
            int i = 3;
            object boxed = i;
            i = 5;
            Assert.IsTrue((int)boxed == 3);


            object obj1 = (int) 2;
            object obj2 = obj1;
            obj1 = (int) 3;
            Debug.WriteLine(obj2);
        }
    }
}
