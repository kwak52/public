using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestCircularDAQ;
using System.Diagnostics;

namespace UtCircularBuffer
{
    [TestClass]
    public class UnitTest1
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
            Assert.AreNotEqual(default(long), buffer.Enqueue(6));
            Assert.AreNotEqual(default(long), buffer.Enqueue(7));

            var i = 0;
            foreach (var value in buffer)
                Trace.WriteLine($"{i++} = {value}");
        }

    }
}
