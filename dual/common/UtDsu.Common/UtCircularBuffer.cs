using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Dsu.Common.Utilities;

namespace UtCircularBuffer
{
    [TestClass]
    public class UtCircularBuffer
    {
        [TestMethod]
        public void TestOverwrite()
        {
            var buffer = new CircularBuffer<long>(3);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            Assert.AreEqual(1, buffer.Enqueue(4));
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(3, buffer.Dequeue());
            Assert.AreEqual(4, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void TestUnderwrite()
        {
            var buffer = new CircularBuffer<long>(5);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(1, buffer.Dequeue());
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(3, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void TestIncreaseCapacityWhenFull()
        {
            var buffer = new CircularBuffer<long>(3);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            Assert.AreEqual(3, buffer.Count);
            buffer.Capacity = 4;
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(1, buffer.Dequeue());
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(3, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void TestDecreaseCapacityWhenFull()
        {
            var buffer = new CircularBuffer<long>(3);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            Assert.AreEqual(3, buffer.Count);
            buffer.Capacity = 2;
            Assert.AreEqual(2, buffer.Count);
            Assert.AreEqual(1, buffer.Dequeue());
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void TestEnumerationWhenFull()
        {
            var buffer = new CircularBuffer<long>(3);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            var i = 0;
            foreach (var value in buffer)
                Assert.AreEqual(++i, value);
            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void TestEnumerationWhenPartiallyFull()
        {
            var buffer = new CircularBuffer<long>(3);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            var i = 0;
            foreach (var value in buffer)
                Assert.AreEqual(++i, value);
            Assert.AreEqual(i, 2);
        }

        [TestMethod]
        public void TestEnumerationWhenEmpty()
        {
            var buffer = new CircularBuffer<long>(3);
            foreach (var value in buffer)
                Assert.Fail("Unexpected Value: " + value);
        }

        [TestMethod]
        public void TestRemoveAt()
        {
            var buffer = new CircularBuffer<long>(5);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            Assert.AreEqual(default(long), buffer.Enqueue(4));
            Assert.AreEqual(default(long), buffer.Enqueue(5));
            buffer.RemoveAt(buffer.IndexOf(2));
            buffer.RemoveAt(buffer.IndexOf(4));
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(1, buffer.Dequeue());
            Assert.AreEqual(3, buffer.Dequeue());
            Assert.AreEqual(5, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            Assert.AreEqual(default(long), buffer.Enqueue(4));
            Assert.AreEqual(default(long), buffer.Enqueue(5));
            buffer.RemoveAt(buffer.IndexOf(1));
            buffer.RemoveAt(buffer.IndexOf(3));
            buffer.RemoveAt(buffer.IndexOf(5));
            Assert.AreEqual(2, buffer.Count);
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(4, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);

        }

        [TestMethod]
        public void TestOverflow()
        {
            var buffer = new CircularBuffer<long>(5);
            Assert.AreEqual(default(long), buffer.Enqueue(1));
            Assert.AreEqual(default(long), buffer.Enqueue(2));
            Assert.AreEqual(default(long), buffer.Enqueue(3));
            Assert.AreEqual(default(long), buffer.Enqueue(4));
            Assert.AreEqual(default(long), buffer.Enqueue(5));
            Assert.AreEqual(1, buffer.Enqueue(6));
            Assert.AreEqual(2, buffer.Enqueue(7));
            Assert.AreEqual(3, buffer.Enqueue(8));
            Assert.AreEqual(4, buffer.Enqueue(9));
            Assert.AreEqual(5, buffer.Enqueue(10));

            Assert.AreEqual(6, buffer.Enqueue(11));
            Assert.AreEqual(7, buffer.Enqueue(12));
            Assert.AreEqual(8, buffer.Enqueue(13));
            Assert.AreEqual(9, buffer.Enqueue(14));
            Assert.AreEqual(10, buffer.Enqueue(15));


            var i = 0;
            foreach (var value in buffer)
                Trace.WriteLine($"{i++} = {value}");

            buffer.Dequeue();
            buffer.Dequeue();
            buffer.Dequeue();
            buffer.Dequeue();
        }


        [TestMethod]
        public void TestEnqueueMultiples()
        {
            var buffer = new CircularBuffer<long>(5);
            buffer.Enqueue(new long[] { 1, 2, 3, 4, 5 });
            //Assert.AreEqual(default(long), buffer.Enqueue(1));
            //Assert.AreEqual(default(long), buffer.Enqueue(2));
            //Assert.AreEqual(default(long), buffer.Enqueue(3));
            //Assert.AreEqual(default(long), buffer.Enqueue(4));
            //Assert.AreEqual(default(long), buffer.Enqueue(5));
            Assert.AreEqual(1, buffer.Enqueue(6));
            Assert.AreEqual(2, buffer.Enqueue(7));
            Assert.AreEqual(3, buffer.Enqueue(8));
            Assert.AreEqual(4, buffer.Enqueue(9));
            Assert.AreEqual(5, buffer.Enqueue(10));

            Assert.AreEqual(6, buffer.Enqueue(11));
            Assert.AreEqual(7, buffer.Enqueue(12));
            Assert.AreEqual(8, buffer.Enqueue(13));
            Assert.AreEqual(9, buffer.Enqueue(14));
            Assert.AreEqual(10, buffer.Enqueue(15));


            var i = 0;
            foreach (var value in buffer)
                Trace.WriteLine($"{i++} = {value}");

            buffer.Dequeue();
            buffer.Dequeue();
            buffer.Dequeue();
            buffer.Dequeue();
        }
    }
}
