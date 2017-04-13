using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CSharp.Test.TaskParallel
{
    [TestClass]
    public class UnitTestDelay
    {
        /// <summary>
        /// Sync test method
        /// </summary>
        [TestMethod]
        public void TestMethodDelay()
        {
            // Asynchronously wait 1 second.
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Real async test method
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestMethodDelayAsync()
        {
            // Asynchronously wait 1 second.
            await Task.Delay(TimeSpan.FromSeconds(1));
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void TestMethod1()
        {
            Trace.WriteLine("WriteLine");
            new Action(() => { Trace.WriteLine("Done"); }).DelayedDoAsync(5000);

            Trace.WriteLine("Waiting..");
            Thread.Sleep(5000);
            Trace.WriteLine("Waited.");
        }



        async Task<int> Go()
        {
            return await GetAnswerToLife();
        }
        async Task<int> GetAnswerToLife()
        {
            await Task.Delay(5000);
            int answer = 21 * 2;
            Trace.WriteLine("Done");
            return answer;
        }

        [TestMethod]
        public void TestMethodGo()
        {
            Trace.WriteLine("Going..");
            Go().Forget();
            Trace.WriteLine("Waiting..");
            Thread.Sleep(5000);
            Trace.WriteLine("Waited.");
        }
    }
}
