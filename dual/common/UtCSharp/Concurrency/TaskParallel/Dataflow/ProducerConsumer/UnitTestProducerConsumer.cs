using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * TPL Dataflow
 */
namespace CSharp.Test.Concurrency.TaskParallel.Dataflow.ProducerConsumer
{
    public class ProducerConsumerBase
    {
        protected static int _n = 10;
        protected int Produce(int millisecondsTimeout=500)
        {
            Thread.Sleep(millisecondsTimeout);
            Trace.WriteLine(String.Format("Producing {0}", _n-1));
            return --_n;
        }

        protected void Process(int n)
        {
            Trace.WriteLine(String.Format("Got {0}", n));
        }        
    }
    /// <summary>
    /// TPLdataflow.pdf
    /// </summary>
    [TestClass]
    public class UnitTestAsync : ProducerConsumerBase
    {
        private BufferBlock<int> m_buffer = new BufferBlock<int>();

        // Producer
        private void Producer()
        {
            while (true)
            {
                int n = Produce();
                m_buffer.Post(n);
                if (n < 0)
                    break;
            }
        }


        // Consumer
        private void Consumer()
        {
            while (true)
            {
                int n = m_buffer.Receive();
                if (n < 0)
                    break;

                Process(n);
            }
        }
        // Consumer Async
        private async Task ConsumerAsync()
        {
            while (true)
            {
                int n = await m_buffer.ReceiveAsync();
                if (n < 0)
                    break;
                Process(n);
            }
        }


        [TestMethod]
        public void TestMethodSync()
        {
            var p = Task.Factory.StartNew(Producer);
            var c = Task.Factory.StartNew(Consumer);
            Task.WaitAll(p, c);
        }

        [TestMethod]
        public void TestMethodAsync()
        {
            var p = Task.Factory.StartNew(Producer);
            var c = ConsumerAsync();
            Task.WaitAll(p, c);
        }
    }



    [TestClass]
    public class UnitTestAsyncProducerConsumerWithThrottledProducer : ProducerConsumerBase
    {
        private BufferBlock<int> m_buffer = new BufferBlock<int>(new DataflowBlockOptions { BoundedCapacity = 10 });

        // Producer
        private async Task ProducerAsync()
        {
            while (true)
            {
                int n = Produce(100);
                await m_buffer.SendAsync(n);

                if (n < 0)
                    break;
            }
        }

        // Consumer Async
        private async Task ConsumerAsync()
        {
            while (true)
            {
                int n = await m_buffer.ReceiveAsync();
                if (n < 0)
                    break;

                Process(n);
            }
        }
        

        [TestMethod]
        public async Task TestMethodAsyncThrottled()
        {
            _n = 100;
            await Task.WhenAll(ProducerAsync(), ConsumerAsync());
            Trace.WriteLine("Done!");
        }
    }
}
