using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Reactive_Extensions
{
    [TestClass]
    public class UnitTestRxFromEventPattern
    {
        /// <summary>
        /// Generic version
        /// </summary>
        [TestMethod]
        public void TestMethodFromEventPattern()
        {
            var timer = new System.Timers.Timer(interval: 1000) {Enabled = true};
            var ticks = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(
                handler => (s, a) => handler(s, a),
                handler => timer.Elapsed += handler,
                handler => timer.Elapsed -= handler);
            ticks.Subscribe(data => Trace.WriteLine("OnNext: " + data.EventArgs.SignalTime));
            Thread.Sleep(3000);
            //await Task.Delay(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Non-generic with reflection.  magic keyword "Elapsed" and needs type casting
        /// </summary>
        [TestMethod]
        public void TestMethodFromEventPattern2()
        {
            var timer = new System.Timers.Timer(interval: 1000) { Enabled = true };
            var ticks = Observable.FromEventPattern(timer, "Elapsed");
            ticks.Subscribe(data => Trace.WriteLine("OnNext: " + ((ElapsedEventArgs)data.EventArgs).SignalTime));
            Thread.Sleep(3000);
            //await Task.Delay(TimeSpan.FromSeconds(1));
        }


        [TestMethod]
        public void TestMethodObservable()
        {
            IObservable<DateTimeOffset> timestamps =
            Observable.Interval(TimeSpan.FromSeconds(1))
            .Timestamp()
            .Where(x => x.Value % 2 == 0)
            .Select(x => x.Timestamp);
            timestamps.Subscribe(x => Trace.WriteLine(x));

            Thread.Sleep(3000);
            //await Task.Delay(TimeSpan.FromSeconds(1));
        }

    }
}
