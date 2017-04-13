using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.TaskParallel
{
    [TestClass]
    public class UnitTestCancellationTokenSource
    {
        [TestMethod]
        public void TestMethod1()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Task myTask = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++ )
                {
                    token.ThrowIfCancellationRequested();
                    // Body of for loop.
                    Thread.Sleep(100);
                }
            }, token);
            // ... elsewhere ...
            cts.Cancel();
        }
    }
}
