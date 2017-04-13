using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Threading.ThreadState;
using Timer = System.Timers.Timer;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmThread
    {
        //http://stackoverflow.com/questions/2374451/how-to-tell-if-a-thread-is-the-main-thread-in-c-sharp
        private static int _mainThread = Thread.CurrentThread.ManagedThreadId;
        public static bool IsMainThread()
        {
            return SynchronizationContext.Current != null;
            //return System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThread;
        }

        /// <summary>
        /// Tells whether a thread is blocked or not
        /// C# 5.0 in a Nutshell, pp. 551
        /// </summary>
        public static bool IsBlocked(this Thread thread)
        {
            return (thread.ThreadState & ThreadState.WaitSleepJoin) != 0;
        }

        /// <summary>
        /// Tells whether a thread is ThreadPool thread or not
        /// C# 5.0 in a Nutshell, pp. 563
        /// </summary>
        public static bool IsThreadPoolThread(this Thread thread)
        {
            return thread.IsThreadPoolThread;
        }


        /// <summary> Most values, however, are redundant, unused, or deprecated.  So simplifies it. </summary>
        /// C# 5.0 in a Nutshell
        public static ThreadState Simplify(this ThreadState ts)
        {
            return ts & (ThreadState.Unstarted
                | ThreadState.Running
                | ThreadState.WaitSleepJoin
                | ThreadState.Stopped
                );
        }

        /// <summary>
        /// C# 5.0 in a Nutshell. pp.572
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static Task Delay(int milliseconds)
        {
            var tcs = new TaskCompletionSource<object>();
            var timer = new Timer(milliseconds) { AutoReset = false };
            timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(null); };
            timer.Start();
            return tcs.Task;
        }

        /// <summary>
        /// e.g new Action(() => { FmtTrace.WriteLine("Done");}).DelayedDo(5000);
        /// </summary>
        /// <param name="action"></param>
        /// <param name="milliseconds"></param>
        public static Task DelayedDoAsync(this Action action, int milliseconds)
        {
            return EmAsync.ExceptionSafeTaskRun(() =>
            {
                DelayedDo(action, milliseconds);
            });
        }
        public static void DelayedDo(this Action action, int milliseconds)
        {
            Delay(milliseconds).GetAwaiter().OnCompleted(action);

            // ??? Task.Delay(milliseconds).ContinueWith(action);
        }


        /// <summary>
        /// C# 5.0 in a Nutshell. pp.600
        /// see Dsu.Common.Utilities.Tools.ExecuteWithTimeLimit
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [Obsolete("Use ExecuteWithTimeLimit() instead.")]
        static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            return await WithTimeout(task, timeout.Milliseconds);
        }

        // see Dsu.Common.Utilities.Tools.
        [Obsolete("Use ExecuteWithTimeLimit() instead.")]
        static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, int timeoutMillisec)
        {
            Task winner = await (Task.WhenAny(task, Task.Delay(timeoutMillisec)));
            if (winner != task) throw new TimeoutException();
            return await task; // Unwrap result/re-throw
        }

        private static Task<TResult> WithCancellation<TResult>(this Task<TResult> task, CancellationToken cancelToken)
        {
            var tcs = new TaskCompletionSource<TResult>();
            var reg = cancelToken.Register(() => tcs.TrySetCanceled());
            task.ContinueWith(ant =>
            {
                reg.Dispose();
                if (ant.IsCanceled)
                    tcs.TrySetCanceled();
                else if (ant.IsFaulted)
                    tcs.TrySetException(ant.Exception.InnerException);
                else
                    tcs.TrySetResult(ant.Result);
            });
            return tcs.Task;
        }

        /// <summary>
        /// works like WhenAll, except that if any of the tasks fault, the resultant task faults immediately:
        /// </summary>
        public static async Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks)
        {
            var killJoy = new TaskCompletionSource<TResult[]>();
            foreach (var task in tasks)
                task.ContinueWith(ant =>
                {
                    if (ant.IsCanceled)
                        killJoy.TrySetCanceled();
                    else if (ant.IsFaulted)
                        killJoy.TrySetException(ant.Exception.InnerException);
                }).Forget();
            return await await Task.WhenAny(killJoy.Task, Task.WhenAll(tasks));
        }


        /// <summary>
        /// http://stackoverflow.com/questions/2744295/how-to-find-the-active-thread-count
        /// </summary>
        /// <returns> number of active threads </returns>
        public static int CountActiveThreadInCurrentProcess()
        {
            return GetThreadsInCurrentProcess()
                .Where(t => t.ThreadState == System.Diagnostics.ThreadState.Running)
                .Count();
        }

        public static IEnumerable<ProcessThread> GetThreadsInCurrentProcess()
        {
            return Process.GetCurrentProcess().Threads.OfType<ProcessThread>();
        }
        public static int CountThreadInCurrentProcess()
        {
            return GetThreadsInCurrentProcess().Count();
        }
    }
}
