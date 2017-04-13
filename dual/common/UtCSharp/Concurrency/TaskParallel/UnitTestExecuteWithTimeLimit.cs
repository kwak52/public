using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;
using System.Threading.Tasks;

namespace CSharp.Test.TaskParallel
{
    [TestClass]
    public class UnitTestExecuteWithTimeLimit
    {
        private Task<int> SleepWell()
        {
            Task.Delay(5000);
            return Task.FromResult(5);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var task = SleepWell();
            var waited = task.Wait(6000);
            Trace.WriteLine(DateTime.Now.ToString());
            var retValue = task.Result;
            Trace.WriteLine(DateTime.Now.ToString());

            bool finishied = false;
            bool succeeded = false;

            try
            {
                succeeded = ToolsDateTime.ExecuteWithTimeLimit(() =>
                {
                    Trace.WriteLine("Starting action 1.");
                    Thread.Sleep(3000);
                    Trace.WriteLine("Finished action 1.");
                    finishied = true;
                }, 1000, cancelOnExpire: true);  // if throwOnExpire

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                Assert.IsFalse(succeeded);
                Assert.IsFalse(finishied);      // in this case, not finished.
            }



            finishied = false;
            succeeded = ToolsDateTime.ExecuteWithTimeLimit(() =>
            {
                Trace.WriteLine("Starting action 2.");
                Thread.Sleep(3000);
                Trace.WriteLine("Finished action 2.");
                finishied = true;
            }, 1000, cancelOnExpire: false);

            Assert.IsFalse(succeeded);
            Assert.IsTrue(finishied);           // in this case, finished anyway.


            finishied = false;
            succeeded = ToolsDateTime.ExecuteWithTimeLimit(() =>
            {
                Trace.WriteLine("Starting action 3.");
                Thread.Sleep(1000);
                Trace.WriteLine("Finished action 3.");
                finishied = true;
            }, 2000);

            Assert.IsTrue(succeeded);
            Assert.IsTrue(finishied);


            /*
             * 성공하는 case : 더 기다려서
             */

            finishied = false;
            succeeded = ToolsDateTime.ExecuteWithTimeLimit(() =>
            {
                Trace.WriteLine("Starting action 99.");
                Thread.Sleep(1000);
                Trace.WriteLine("Finished action 99.");
                finishied = true;
            }, 2000);

            Assert.IsTrue(succeeded);
            Assert.IsTrue(finishied);

        }
    }
}
