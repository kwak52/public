using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test
{
    // http://stackoverflow.com/questions/578967/how-can-i-write-a-unit-test-to-determine-whether-an-object-can-be-garbage-collec
    [TestClass]
    public class UnitTestWeakReference
    {
        [TestMethod]
        public void TestMethodWeakReference()
        {
            WeakReference reference = null;
            new Action(() =>
            {
                var sb = new StringBuilder();
                // Do things with service that might cause a memory leak...

                reference = new WeakReference(sb, true);
                Assert.IsNotNull(reference.Target);
            })();

            // Service should have gone out of scope about now, 
            // so the garbage collector can clean it up
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsNull(reference.Target);
        }

        internal class A
        {
            public static int Counter = 0;
            public B B { get; set; }
            public A() { Counter++; }
            ~A() { --Counter; }
        }

        internal class B
        {
            public static int Counter = 0;
            public A A { get; set; }
            public B() { Counter++; }
            ~B() { --Counter; }
        }

        [TestMethod]
        public void TestMethodStrongCyclicReference()
        {
            new Action(() =>
            {
                A a = new A();
                B b = new B();
                a.B = b;
                b.A = a;
            })();

            // Service should have gone out of scope about now, 
            // so the garbage collector can clean it up
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsTrue(A.Counter == 0 && B.Counter == 0);
        }


        [TestMethod]
        public void TestMethodWeakReferenceGeneric()
        {
            WeakReference<StringBuilder> reference = null;
            new Action(() =>
            {
                var sb = new StringBuilder();
                // Do things with service that might cause a memory leak...

                reference = new WeakReference<StringBuilder>(sb, true);
                StringBuilder target = null;
                Assert.IsTrue(reference.TryGetTarget(out target));
                Assert.IsNotNull(target);
            })();

            // Service should have gone out of scope about now, 
            // so the garbage collector can clean it up
            GC.Collect();
            GC.WaitForPendingFinalizers();

            {
                StringBuilder target = null;
                Assert.IsFalse(reference.TryGetTarget(out target));
                Assert.IsNull(target);                
            }
        }


        [TestMethod]
        public void TestMethodWeakReferenceHolder()
        {
            WeakReferenceHolder<StringBuilder> reference = null;
            new Action(() =>
            {
                var sb = new StringBuilder();
                // Do things with service that might cause a memory leak...

                reference = new WeakReferenceHolder<StringBuilder>(sb, true);
                Assert.IsNotNull(reference.Target);
            })();

            // Service should have gone out of scope about now, 
            // so the garbage collector can clean it up
            GC.Collect();
            GC.WaitForPendingFinalizers();


            Assert.IsNull(reference.Target);
        }


    }
}
