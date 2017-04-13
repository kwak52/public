using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.TaskParallel
{
    [TestClass]
    public class UnitTestThreadProperty
    {
        [TestMethod]
        public void TestMethodThreadProperty()
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Assert.IsTrue(Thread.CurrentThread.IsAlive);
            //Assert.IsFalse(Thread.CurrentThread.IsBackground);
            //Assert.IsFalse(Thread.CurrentThread.IsThreadPoolThread);

            Thread thread = null;
            thread = new Thread(() =>
            {
                Assert.IsTrue(thread.IsAlive);
                Assert.IsFalse(thread.IsBackground);
                Assert.IsFalse(thread.IsThreadPoolThread);
                Thread.Sleep(100);
            });
            Assert.IsFalse(thread.IsAlive);
            thread.Start();
            Assert.IsTrue(thread.IsAlive);
            thread.Join();
            Assert.IsFalse(thread.IsAlive);


            Task.Run(() =>
            {
                Assert.IsTrue(id != Thread.CurrentThread.ManagedThreadId);
                Assert.IsTrue(Thread.CurrentThread.IsAlive);
                Assert.IsTrue(Thread.CurrentThread.IsBackground);
                Assert.IsTrue(Thread.CurrentThread.IsThreadPoolThread);
            }).Wait();


            var longRunTask = new Task(() =>
            {
                Trace.WriteLine("Starting long-running task.");
                Thread.Sleep(1000);
                Trace.WriteLine("Finished long-running task.");
                Assert.IsFalse(Thread.CurrentThread.IsThreadPoolThread);
            }, TaskCreationOptions.LongRunning);
            longRunTask.Start();
            longRunTask.Wait();

            Trace.WriteLine("Done!");
        }
    }
}
