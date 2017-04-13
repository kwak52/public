using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.Common.Utilities.Exceptions;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmAsync
    {
        public static TResult WaitResultAsyncFunction<TResult>(this Func<Task<TResult>> function)
        {
            var task = Task.Run(() => function());
            task.Wait();
            return task.Result;
        }

        public static void WaitAsyncFunction(this Func<Task> function)
        {
            function().Wait();
        }


        private static Action WrapExceptionSafe(this Action action, Action<Exception> exceptionHandler)
        {
            return new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    if (exceptionHandler == null)
                    {
                        if ( FormAppCommon.UISynchronizationContext == null )
                            MessageBox.Show(ex.Message, "Exception occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            FormAppCommon.UISynchronizationContext.Send(ignore => UnhandledExceptionHandler.ShowUnhandledException(ex), null);
                    }
                    else
                        exceptionHandler(ex);
                }                
            });
        }


        public static Func<Task<T>> WrapExceptionSafe<T>(this Func<Task<T>> function, Action<Exception> exceptionHandler)
        {
            return new Func<Task<T>>(() =>
            {
                try
                {
                    return function();
                }
                catch (Exception ex)
                {
                    if (exceptionHandler == null)
                    {
                        if (FormAppCommon.UISynchronizationContext == null)
                            MessageBox.Show(ex.Message, "Exception occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            FormAppCommon.UISynchronizationContext.Send(ignore => UnhandledExceptionHandler.ShowUnhandledException(ex), null);
                    }
                    else
                        exceptionHandler(ex);

                    return null;
                }
            });
            
        }

        /// <summary>
        /// 단순히 Task.Run(()=>{}) 수행시, exception 을 catch 할수 없으므로, 다음 extension 을 사용해서 해결함
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        public static Task ExceptionSafeTaskRun(this Action action, Action<Exception> exceptionHandler=null)
        {
            return Task.Run(WrapExceptionSafe(action, exceptionHandler));
        }
        public static Task<T> ExceptionSafeTaskRun<T>(this Func<Task<T>> function, Action<Exception> exceptionHandler=null)
        {
            return Task.Run(WrapExceptionSafe(function, exceptionHandler));
        }


        private static void HandleException(this Task task)
        {
            var ex = task.Exception;
            if (ex != null)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                Globals.Logger?.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Disabling CS4014
        /// http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
        /// warning CS4014: Because this call is not awaited, execution of the current method continues before the call is completed.
        /// Consider applying the 'await' operator to the result of the call.
        /// </summary>
        /// <param name="task"></param>
        public static void Forget(this Task task)
        {
            // Async in C# 5.0, pp.57
            // Task 를 실행만 하고, await 하지 않는 fire & forget 모델에서, exception 이 발생하였을 경우를 체크 하기 위한 용도.
            task.ContinueWith(ant => HandleException(ant));
        }

        /// <summary>
        /// http://stackoverflow.com/questions/2565166/net-best-way-to-execute-a-lambda-on-ui-thread-after-a-delay
        /// </summary>
        /// Usage
        /// <code>
        /// SynchronizationContext.Current.Post(TimeSpan.FromSeconds(1), () => textBlock.Text="Done");
        /// </code>
        public static void Post(this SynchronizationContext context, TimeSpan delay, Action action)
        {
            System.Threading.Timer timer = null;

            timer = new System.Threading.Timer((ignore) =>
            {
                timer.Dispose();

                context.Post(ignore2 => action(), null);
            }, null, delay, TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// Executes action after delay.
        /// C# 5.0 in a Nutshell, pp. 573
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delayMillisec"></param>
        /// <returns></returns>
        public static Task ExecuteWithDelay(this Action action, int delayMillisec)
        {
            return Task.Delay(delayMillisec).ContinueWith(ant => action);
        }

        /// <summary>
        /// pp.20.  Concurrency in C# Cookbook
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<T> FromResult<T>(this T value)
        {
            return Task.FromResult(value);
        }
    }
}
