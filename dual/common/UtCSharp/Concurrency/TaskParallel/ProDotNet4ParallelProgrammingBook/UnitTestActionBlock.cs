using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Concurrency.TaskParallel.ProDotNet4ParallelProgrammingBook
{
    [TestClass]
    public class UnitTestActionBlock
    {
        [TestMethod]
        public void TestMethod1()
        {
            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // create the cancellation token
            CancellationToken token = tokenSource.Token;
            // create the task
            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Trace.WriteLine("Task cancel detected");
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        Trace.WriteLine(String.Format("Int value {0}", i));
                    }
                }
            }, token);
            // register a cancellation delegate
            token.Register(() =>
            {
                Trace.WriteLine(">>>>>> Delegate Invoked\n");
            });

            // wait for input before we start the task
            Trace.WriteLine("starting task");
            // start the task
            task.Start();
            Thread.Sleep(1000);

            // cancel the task
            Trace.WriteLine("Cancelling task");
            tokenSource.Cancel();

            // wait for input before exiting
            Trace.WriteLine("Main method complete.");
            Thread.Sleep(1000);
        }
    }
}
