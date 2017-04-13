using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CSharp.Test.TaskParallel
{
    [TestClass]
    public class UnitTestSyncAsyncBasedOnAsync
    {
        private int CheckNumber(int n)
        {
            var task = Task.Run(async () => await CheckNumberAsync(n));
            task.Wait();
            return task.Result;
        }

        private async Task<int> CheckNumberAsync(int n)
        {
            return await Task.Run(() =>
            {
                Trace.WriteLine(String.Format("\tBegin sub {0}[1]", n, Thread.CurrentThread.ManagedThreadId));
                Thread.Sleep(n * 100);
                Trace.WriteLine(String.Format("\tEnd sub {0}[1]", n, Thread.CurrentThread.ManagedThreadId));
                return n;
            });
        }

        [TestMethod]
        public void TestMethodCallSync()
        {
            Trace.WriteLine(String.Format("MAIN: Calling[{0}]", Thread.CurrentThread.ManagedThreadId));
            CheckNumber(0);
            CheckNumber(1);
            CheckNumber(2);
            CheckNumber(3);
            CheckNumber(4);
            Trace.WriteLine(String.Format("MAIN: Finished[{0}]", Thread.CurrentThread.ManagedThreadId));
        }

        [TestMethod]
        public void TestMethodCallSync2()
        {
            Trace.WriteLine(String.Format("MAIN: Calling[{0}]", Thread.CurrentThread.ManagedThreadId));
            CheckNumberAsync(0).Wait();
            CheckNumberAsync(1).Wait();
            CheckNumberAsync(2).Wait();
            CheckNumberAsync(3).Wait();
            CheckNumberAsync(4).Wait();
            Trace.WriteLine(String.Format("MAIN: Finished[{0}]", Thread.CurrentThread.ManagedThreadId));
        }


        [TestMethod]
        public void TestMethodCallAsync()
        {
            Trace.WriteLine(String.Format("MAIN: Calling[{0}]", Thread.CurrentThread.ManagedThreadId));
            List<Task<int>> tasks = new List<Task<int>>();
            tasks.Add(CheckNumberAsync(0));
            tasks.Add(CheckNumberAsync(1));
            CheckNumber(2);
            tasks.Add(CheckNumberAsync(3));
            tasks.Add(CheckNumberAsync(4));
            Trace.WriteLine(String.Format("MAIN: Finished[{0}]", Thread.CurrentThread.ManagedThreadId));
            Task.WaitAll(tasks.ToArray());
            Trace.WriteLine("Bye");
        }


        [TestMethod]
        public void TestMethodCallAsyncOnSync()
        {
            /* 
             * warning CS4014: 
             * Because this call is not awaited, execution of the current method continues before the call is completed.
             * Consider applying the 'await' operator to the result of the call.
             */
            Trace.WriteLine(String.Format("MAIN: Calling[{0}]", Thread.CurrentThread.ManagedThreadId));
            CheckNumberAsync(0).Forget();    // warning CS4014
            CheckNumberAsync(1).Forget();
            CheckNumberAsync(2).Forget();
            Trace.WriteLine(String.Format("MAIN: Finished[{0}]", Thread.CurrentThread.ManagedThreadId));
        }
    }






    [TestClass]
    public class UnitTestSyncAsyncBasedOnSync
    {
        private int CheckNumber(int n)
        {
            Trace.WriteLine(String.Format("\tBegin sub {0}[1]", n, Thread.CurrentThread.ManagedThreadId));
            Thread.Sleep(n * 100);
            Trace.WriteLine(String.Format("\tEnd sub {0}[1]", n, Thread.CurrentThread.ManagedThreadId));
            return n;
        }

        private Task<int> CheckNumberAsync(int n)
        {
            return Task.Run(() => CheckNumber(n));
        }

        [TestMethod]
        public void TestMethodCallSync()
        {
            Trace.WriteLine(String.Format("MAIN: Calling[{0}]", Thread.CurrentThread.ManagedThreadId));
            CheckNumber(0);
            CheckNumber(1);
            CheckNumber(2);
            CheckNumber(3);
            CheckNumber(4);
            Trace.WriteLine(String.Format("MAIN: Finished[{0}]", Thread.CurrentThread.ManagedThreadId));
        }


        [TestMethod]
        public void TestMethodCallSync2()
        {
            Trace.WriteLine(String.Format("MAIN: Calling[{0}]", Thread.CurrentThread.ManagedThreadId));
            CheckNumberAsync(0).Wait();
            CheckNumberAsync(1).Wait();
            CheckNumberAsync(2).Wait();
            CheckNumberAsync(3).Wait();
            CheckNumberAsync(4).Wait();
            Trace.WriteLine(String.Format("MAIN: Finished[{0}]", Thread.CurrentThread.ManagedThreadId));
        }

        [TestMethod]
        public void TestMethodCallAsync()
        {
            Trace.WriteLine(String.Format("MAIN: Calling[{0}]", Thread.CurrentThread.ManagedThreadId));
            List<Task<int>> tasks = new List<Task<int>>();
            tasks.Add(CheckNumberAsync(0));
            tasks.Add(CheckNumberAsync(1));
            CheckNumber(2);
            tasks.Add(CheckNumberAsync(3));
            tasks.Add(CheckNumberAsync(4));
            Trace.WriteLine(String.Format("MAIN: Finished[{0}]", Thread.CurrentThread.ManagedThreadId));
            Task.WaitAll(tasks.ToArray());
            Trace.WriteLine("Bye");
        }

    }
}
