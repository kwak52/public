using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.Exceptions
{
    /// <summary>
    /// Thread 에서 unhandled exception 발생해서 crash 되는 것을 방지하기 위한 helper class.
    /// - Main thread 에서 발생하는 unhandled exception 은 UnhandledExceptionHandler 에서 처리가능
    /// 
    /// - 사용법
    /// <para/> * ExceptionSafeThreadStarter.Start(form, action, reportingAction);
    /// <para/> * ExceptionSafeThreadStarter.Start(form, action);
    /// <para/> * new ExceptionSafeThreadStarter(){MainThreadControl = form, Action=action, ReportingAction=reportingAction}.Start();
    /// 
    /// http://stackoverflow.com/questions/6114976/application-threadexception-vs-appdomain-unhandledexception
    /// Application.ThreadException can only trap exceptions that are raised in the UI thread.
    /// In code that's run due to Windows notifications. Or in technical terms, 
    /// the events that are triggered by the message loop. Most any Winforms event fit this category.  
    /// 
    /// What it does not trap are exceptions raised on any non-UI thread,
    /// like a worker thread started with Thread.Start(), ThreadPool.QueueUserWorkItem or a delegate's BeginInvoke() method.
    /// Any unhandled exception in those will terminate the app, AppDomain.UnhandledException is the last gasp.
    /// 
    /// http://stackoverflow.com/questions/1554181/exception-handling-in-threads
    /// Exception thrown in a thread normally couldn't be caught in another thread.    /// </summary>
    public class ExceptionSafeThreadStarter
    {
        public Control MainThreadControl { get; set; }
        public Action Action { get; set; }
        public Action ReportingAction { get; set; }

        public void Start()
        {
            Start("UnknownThread", MainThreadControl, Action, ReportingAction);
        }

        /// <summary> Thread 에서 unhandled exception 발생을 방지하기 위해서, thread action 최상위에서 exception 을 catch 한다. </summary>
        /// <param name="threadName"></param>
        /// <param name="mainUIThreadControl">Main thread 에서 생성한 임의의 control.  통상 Form 을 던져 주면 된다. </param>
        /// <param name="action">Thread 에서 수행할 action</param>
        /// <param name="isBackground"></param>
        /// <param name="icon"></param>
        public static Thread Start(string threadName, Control mainUIThreadControl, Action action, bool isBackground = false, Icon icon = null)
        {
            Contract.Requires(mainUIThreadControl != null && action != null);

            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    var a = new Action(() =>
                    {
                        var handler = new UnhandledExceptionHandler();
                        if (icon != null)
                            handler.Icon = icon;

                        handler.ShowUnhandledExceptionDlg(ex);
                    });

                    if (mainUIThreadControl.InvokeRequired )
                    {
                        mainUIThreadControl.Invoke(a);
                    }
                    else if (mainUIThreadControl.IsHandleCreated) // 창 핸들을 만들기 전까지 컨트롤에서 Invoke 또는 BeginInvoke를 호출할 수 없습니다
                        a();
                }
            }) { Name = threadName, IsBackground = isBackground };

            thread.Start();
            
            return thread;
        }


        public static Thread Start(string threadName, SynchronizationContext context, Action action, bool isBackground = false, Icon icon = null)
        {
            Contract.Requires(context != null && action != null);

            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    var a = new Action(() =>
                    {
                        var handler = new UnhandledExceptionHandler();
                        if (icon != null)
                            handler.Icon = icon;

                        handler.ShowUnhandledExceptionDlg(ex);
                    });

                    context.Send(ignore => a(), null);
                }
            }) { Name = threadName, IsBackground = isBackground };

            thread.Start();

            return thread;
        }

        public static Thread Start(string threadName, Control mainThreadControl, Action action, Icon icon)
        {
            return Start(threadName, mainThreadControl, action, false, icon);
        }


        /// <summary> Thread 에서 unhandled exception 발생을 방지하기 위해서, thread action 최상위에서 exception 을 catch 한다. </summary>
        /// <param name="threadName"></param>
        /// <param name="mainThreadControl">Main thread 에서 생성한 임의의 control.  통상 Form 을 던져 주면 된다. </param>
        /// <param name="action">Thread 에서 수행할 action</param>
        /// <param name="reportingAction">Exception 발생시, reporting 수행 방법.  null 이면 기본 action 수행</param>
        public static Thread Start(string threadName, Control mainThreadControl, Action action, Action reportingAction)
        {
            Contract.Requires(mainThreadControl != null && action != null && reportingAction != null);

            var thread = new Thread(() =>
            {
                try { action(); } 
                catch (Exception)
                {
                    /*
                     * - Thread 에서 발생한 exception 은 해당 thread 내에서만 catch 할 수 있다.
                     * - Thread 에서 추가적인 form 생성을 위해서는, main thread 에 invoke 로 호출 할 수 밖에 없다.
                     */
                    mainThreadControl.Invoke(reportingAction);
                }
            }) { Name=threadName };

            thread.Start();

            return thread;
        }

    }
}
