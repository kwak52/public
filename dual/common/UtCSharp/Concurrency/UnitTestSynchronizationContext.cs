using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test.Concurrency
{
    [TestClass]
    public class UnitTestSynchronizationContext
    {
        private static ReaderWriterLock _rwLock = new ReaderWriterLock();
        static async Task MainProgramm()
        {
            Trace.WriteLine(String.Format("We are on Thread No {0}", Thread.CurrentThread.ManagedThreadId));
            var client = new WebClient();
            var webContentHomePage = await client.DownloadStringTaskAsync("http://www.gamlor.info/wordpress/");

            Trace.WriteLine(String.Format("Downloaded {0} characters, continue with the work", webContentHomePage.Length));
            Trace.WriteLine(String.Format("We are on Thread No {0} ", Thread.CurrentThread.ManagedThreadId));
        }
        /// <summary>
        /// http://www.gamlor.info/wordpress/2010/10/c-5-0-async-feature-be-aware-of-the-synchronization-context/
        /// </summary>
        [TestMethod]
        public async Task TestMethodSynchronizationContext()
        {
            /* Prints out
                We are on Thread No 11
                Downloaded 42541 characters, continue with the work
                We are on Thread No 10 
             */
            await Task.Run(async () =>
            {
                await MainProgramm();
            });
        }

        [TestMethod]
        public async Task TestMethodSynchronizationContext2()
        {
            // will not terminate on batch test
            if ( false )
            {
                /* Prints out
                    We are on Thread No 11
                    Downloaded 42541 characters, continue with the work
                    We are on Thread No 11 
                 */

                // Setup our synchronisation context
                SynchronizationContextEx ctx = new SynchronizationContextEx();
                SynchronizationContextEx.SetSynchronizationContext(ctx);

                // The first thing to process is our main application
                ctx.Post(async obj => await MainProgramm(), null);

                // Then we kick of the message pump
                ctx.RunMessagePump();
            }
        }


        [TestMethod]
        public async Task TestMethodSynchronizationContext3()
        {
            AsyncPump.Run(async () => await MainProgramm());
        }

        private async Task DemoAsync()
        {

            var d = new Dictionary<int, int>();

            for (int i = 0; i < 10000; i++)
            {

                int id = Thread.CurrentThread.ManagedThreadId;

                int count;

                d[id] = d.TryGetValue(id, out count) ? count + 1 : 1;



                await Task.Yield();

            }

            foreach (var pair in d)
                Trace.WriteLine(pair);

        }
        [TestMethod]
        public async Task TestMethodSynchronizationContext11()
        {
            await DemoAsync();
        }

        [TestMethod]
        public async Task TestMethodSynchronizationContext12()
        {
            AsyncPump.Run(async delegate
            {
                await DemoAsync();
            });

        }

    }
}
