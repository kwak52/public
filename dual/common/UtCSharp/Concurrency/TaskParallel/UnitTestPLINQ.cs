using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CSharp.Test.TaskParallel
{
    [TestClass]
    public class UnitTestPLINQ
    {

        private bool Check(int n)
        {
            Trace.WriteLine(String.Format("\tBegin sub {0}[1]", n, Thread.CurrentThread.ManagedThreadId));
            Thread.Sleep(n * 100);
            Trace.WriteLine(String.Format("\tEnd sub {0}[1]", n, Thread.CurrentThread.ManagedThreadId));
            return n == 5;
        }

        [TestMethod]
        public void TestMethodFirstOrDefault()
        {
            //int result = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9}.AsParallel().FirstOrDefault(n => Check(n));
            int result = ParallelEnumerable.Range(0, 20).FirstOrDefault(n => Check(n));
            Trace.WriteLine(result);
        }


        public static async Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks)
        {
            var tcs = new TaskCompletionSource<TResult[]>();        // killJoy
            foreach (var task in tasks)
                task.ContinueWith(ant =>
                {
                    if (ant.IsCanceled)
                        tcs.TrySetCanceled();
                    else if (ant.IsFaulted)
                        tcs.TrySetException(ant.Exception.InnerException);
                }).Forget();
            return await await Task.WhenAny(tcs.Task, Task.WhenAll(tasks));
        }


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
                Thread.Sleep(n * 50);
                Trace.WriteLine(String.Format("\tEnd sub {0}[1]", n, Thread.CurrentThread.ManagedThreadId));
                return n;
            });
        }

        public static async Task<TResult[]> WhenAnySuccessful<TResult>(/*params */Task<TResult>[] tasks, Func<TResult, bool> successCheckFunc)
        {
            var tcs = new TaskCompletionSource<TResult[]>();        // killJoy
            foreach (var task in tasks)
                task.ContinueWith(ant =>
                {
                    if (successCheckFunc(ant.Result))
                    {
                        Trace.WriteLine("Got the result");
                        tcs.TrySetCanceled();
                    }

                    if (ant.IsCanceled)
                        tcs.TrySetCanceled();
                    else if (ant.IsFaulted)
                        tcs.TrySetException(ant.Exception.InnerException);
                }).Forget();
            return await await Task.WhenAny(tcs.Task, Task.WhenAll(tasks));
        }

        [TestMethod]
        public void TestMethodWhenAnySuccess()
        {
            List<Task<int>> tasks = new List<Task<int>>();
            Enumerable.Range(0, 20).ForEach(n => tasks.Add(CheckNumberAsync(n)));
            Func<int, bool> successCheckFunc = n =>
            {
                if (n == 5)
                    return true;
                return false;
            };
            //WhenAnySuccessful(tasks.ToArray(), successCheckFunc);


            //Task.WhenAny(tasks);
            WaitOne(tasks);

            Trace.WriteLine("MAIN Done");
        }

        private async void WaitOne<T>(IEnumerable<Task<T>> tasks)
        {
            var task = await Task.WhenAll(tasks);
            CancellationTokenSource cts = new CancellationTokenSource();
            //Task.WaitAll(tasks.Cast<Task>().ToArray(), cts.Token);
            //Task.WaitAny(tasks.Cast<Task>().ToArray(), cts.Token);
            Trace.WriteLine("WAit Done");
        }

    }
}
