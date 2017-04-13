using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Concurrency.TaskParallel.ProDotNet4ParallelProgrammingBook
{
    [TestClass]
    public class UnitTestLocalVariableEvaluation
    {
        [TestMethod]
        public void TestMethod1()
        {
            // create and start the "bad" tasks
            for (int i = 0; i < 5; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    // write out a message that uses the loop counter
                    Trace.WriteLine(String.Format("BAD Task {0} has counter value: {1}", Task.CurrentId, i));
                });
            }
            // create and start the "good" tasks
            for (int i = 0; i < 5; i++)
            {
                Task.Factory.StartNew((stateObj) =>
                {
                    // cast the state object to an int
                    int loopValue = (int)stateObj;
                    // write out a message that uses the loop counter
                    Trace.WriteLine(String.Format("GOOD Task {0} has counter value: {1}", Task.CurrentId, loopValue));
                }, i);
            }

            // create and start the "better" tasks
            for (int i = 0; i < 5; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    int ii = i;
                    // write out a message that uses the loop counter
                    Trace.WriteLine(String.Format("BETTER Task {0} has counter value: {1}", Task.CurrentId, ii));
                });
            }
            
            
            // wait for input before exiting
            Trace.WriteLine("Main method complete. Press enter to finish.");
        }
    }
}
