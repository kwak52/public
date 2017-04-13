using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Concurrency.TaskParallel.Dataflow
{
    /// <summary>
    /// TPLDataflow.pdf
    /// http://www.jayway.com/2013/11/15/an-actor-model-implementation-in-c-using-tpl-dataflow/
    /// </summary>
    [TestClass]
    public class UnitTestActionBlock
    {
        private static int _nActors = 0;

        /// <summary>
        /// - ActionBlock 은 기본적으로 actor 기능을 대신할 수 있다.
        /// - Post/SendAsync 등으로 보내진 message 는 기본적으로 one-by-one 하나씩 처리된다.
        ///  * ExecutionDataflowBlockOptions 로 변경할 수는 있다.
        /// </summary>
        [TestMethod]
        public async Task TestMethodTDFActor()
        {
            Trace.WriteLine(String.Format("caller thread={0}", Thread.CurrentThread.ManagedThreadId));
            var ab = new ActionBlock<int>((i) =>
            {
                Assert.IsTrue(_nActors == 0);
                Interlocked.Increment(ref _nActors);

                Task.Delay(20).Wait();
                Trace.WriteLine(String.Format("Processing {0}, thread={1}", i, Thread.CurrentThread.ManagedThreadId));
  
                Interlocked.Decrement(ref _nActors);
            }/*, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 4, SingleProducerConstrained = true }*/);


            Parallel.For(0, 100, i =>
            {
                ab.SendAsync(i);
            });

            //Task.Run(async () =>
            //{
            //    for (int i = 0; i < 100; i++)
            //        await ab.SendAsync(i);        // or ab.Post(i)
            //});

            for (int i = 0; i < 100; i++)
                await ab.SendAsync(i + 1000);
            //Task.Run(async () =>
            //{
            //});


            Trace.WriteLine("Posted all");
            //Task.Delay(1000).Wait();
            ab.Complete();
            ab.Completion.Wait();
            Trace.WriteLine("Finished all");
        }
    }
}
