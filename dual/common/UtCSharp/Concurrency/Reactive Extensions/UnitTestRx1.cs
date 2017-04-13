using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Reactive_Extensions
{
    [TestClass]
    public class UnitTestRx1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //IObservable<int> source = Observable.Empty<int>(); ;
            //IObservable<int> source = Observable.Throw<int>(new Exception("Oops"));
            //IObservable<int> source = Observable.Return(42);
            //IObservable<int> source = Observable.Range(5, 3);
            //IObservable<int> source = Observable.Generate(0, i => i < 5, i => i + 1, i => i * i);
            //IObservable<int> source = Observable.Never<int>();

            // observable 에 추후에 추가할 수 있는 slot
            // http://stackoverflow.com/questions/9314060/how-do-i-update-add-items-in-to-an-iobservableint-dynamically
            var subject = new Subject<int>();
            //IObservable<int> source = Observable.ToObservable<int>(new int[] { 1, 2, 3, 4, 5 })
            //    .Concat(subject);
            IObservable<int> source = subject;
            IObserver<int> handler = null;

            IDisposable subscription = source.Subscribe(
                x => Trace.WriteLine(String.Format("OnNext: {0}", x)),
                ex => Trace.WriteLine(String.Format("OnError: {0}", ex.Message)),
                () => Trace.WriteLine("OnCompleted")
            );

            subject.OnNext(100);
            subject.OnNext(101);

            Trace.WriteLine("Press ENTER to unsubscribe...");
            subject.OnNext(201);

            Thread.Sleep(1000);     // pause
            
            subscription.Dispose();
            subject.OnNext(999);

        }

        [TestMethod]
        public void TestMethod2()
        {
            // http://leecampbell.blogspot.kr/2011/06/rx-v1010425breaking-changes.html
            // GenerateWithTime is now just an overload of Generate. CreateWithDisposable is now just an overload of Create.
            IObservable<int> source = Observable.Generate(
                0, i => i < 5,
                i => i + 1,
                i => i*i,
                i => TimeSpan.FromSeconds(i));

            using (source.Subscribe(
                x => Trace.WriteLine(String.Format("OnNext: {0}", x)),
                ex => Trace.WriteLine(String.Format("OnError: {0}", ex.Message)),
                () => Trace.WriteLine("OnCompleted")
                ))
            {
                Console.WriteLine("Press ENTER to unsubscribe...");
                Thread.Sleep(1000);     // pause
                //Console.ReadLine();
            }
        }



        [TestMethod]
        public void TestMethodMultiThreaded()
        {
            // observable 에 추후에 추가할 수 있는 slot
            // http://stackoverflow.com/questions/9314060/how-do-i-update-add-items-in-to-an-iobservableint-dynamically
            var subject = new Subject<int>();
            IObservable<int> source = subject;
            IObserver<int> handler = null;

            Random random = new Random(System.Environment.TickCount);

            IDisposable subscription = subject.Subscribe(
                x =>
                {
                    int sleep = random.Next(10, 100);
                    Trace.WriteLine(String.Format("OnNext: {0} : {1}", x, sleep));
                    Thread.Sleep(sleep);
                },
                ex => Trace.WriteLine(String.Format("OnError: {0}", ex.Message)),
                () => Trace.WriteLine("OnCompleted")
            );


            Parallel.For(1, 100, i =>
            {
                subject.OnNext(i);
                subject.OnNext(i);
                subject.OnNext(i);
                subject.OnNext(i);
                subject.OnNext(i);
                subject.OnNext(i);
                subject.OnNext(i);
            });

            Thread.Sleep(5000);     // pause

            subscription.Dispose();
            subject.OnNext(999);

        }
    }
}
