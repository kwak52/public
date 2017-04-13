using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test.TaskParallel
{
    [TestClass]
    public class UnitTestWaitAny
    {
        private Task<bool> DoProcessAsync(int sleepSecond)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(sleepSecond * 1000);
                Trace.WriteLine(String.Format("\tSlept {0} seconds[{1}].", sleepSecond, Thread.CurrentThread.ManagedThreadId));
                return true;
            });
        }

        [TestMethod]
        public void TestMethodWaitAny()
        {
            List<Task<bool>> tasks = new List<Task<bool>>();
            int[] waitArgs = { 1, 2, 3, 4, 5 };
            Trace.WriteLine("Adding each tasks");
            foreach (var n in waitArgs)
                tasks.Add(DoProcessAsync(n));

            Trace.WriteLine("Finished adding");

        }

        [TestMethod]
        public void TestMethodWaitAll()
        {
            List<Task<bool>> tasks = new List<Task<bool>>();
            int[] waitArgs = { 1, 2, 3, 4, 5 };
            Trace.WriteLine(String.Format("Adding each tasks [{0}]", Thread.CurrentThread.ManagedThreadId));
            foreach (var n in waitArgs)
                tasks.Add(DoProcessAsync(n));

            Trace.WriteLine(String.Format("Finished adding [{0}]", Thread.CurrentThread.ManagedThreadId));
            Task.WaitAll(tasks.ToArray());
            Trace.WriteLine(String.Format("Done TestMethodWaitAll()  [{0}]", Thread.CurrentThread.ManagedThreadId));
        }

    }
}
