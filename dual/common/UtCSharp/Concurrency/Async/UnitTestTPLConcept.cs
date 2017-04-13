using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Concurrency.Async
{
    [TestClass]
    public class UnitTestCommonAsyncMistake
    {
        async Task WaitAsync()
        {
            // This await will capture the current context ...
            await Task.Delay(TimeSpan.FromSeconds(1));
            // ... and will attempt to resume the method here in that context.
        }

        /// <summary>
        /// This code will deadlock if called from a UI 
        /// </summary>
        [TestMethod]
        public void TestMethodDeadlock()
        {
            // Start the delay.
            Task task = WaitAsync();
            // Synchronously block, waiting for the async method to complete.
            task.Wait();
        }
    }
}
