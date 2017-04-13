/*
 *                  UnhandledExceptionDlg Class v. 1.1
 * 
 *                      Copyright (c)2006 Vitaly Zayko
 * 
 * History:
 * September 26, 2006 - Added "ThreadException" handler, "SetUnhandledExceptionMode", OnShowErrorReport event 
 *                      and updated the Demo and code comments;
 * August 29, 2006 - Updated information about Microsoft Windows Error Reporting service and its link;
 * July 18, 2006 - Initial release.
 * 
 */

/* More info on MSDN: 
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnbda/html/exceptdotnet.asp
 * http://msdn2.microsoft.com/en-us/library/system.windows.forms.application.threadexception.aspx
 * http://msdn2.microsoft.com/en-us/library/system.appdomain.unhandledexception.aspx
 * http://msdn2.microsoft.com/en-us/library/system.windows.forms.unhandledexceptionmode.aspx
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Forms;

namespace Dsu.Common.Utilities.Exceptions
{
    /// <summary>
    /// handles unhandled exception.
    /// </summary>
    public class UnhandledExceptionHandler
    {
        /// <summary>
        /// unhandled exception 발생시, error report form 이 뜨는데, 강제로 이를 비활성화 할 때에 사용한다.
        /// </summary>
        public static bool IsDisableErrorReporting
        {
            get { return _isDisableErrorReporting; }
            set
            {
//                if (value != _isDisableErrorReporting)
//                {
//                    if (value)
//                    {
//                        if (DialogResult.No == MessageBox.Show(
//                            @"Do you really disable error reporting?
//If disabled, critical errors are hidden.
//So enable it as soon as possible.",
//                            "WARNING"))
//                            return;
//                    }
//                }

                _isDisableErrorReporting = value;
            }
        }
        private static bool _isDisableErrorReporting = false;

        public static string ApplicationInfo { get; set; }
        
        public static SmtpClientEx SmtpClient { get; set; }

	    /// <summary>
	    /// Bug reporing 을 받을 수신인을 지정
	    /// </summary>
	    public static string Recipient { get; set; } = "bugs@dualsoft.co.kr";
        public Icon Icon { get; set; }

        private void Initialize()
        {
            // Uncheck "Restart App" check box by default:
            RestartApp = false;

            // Add handling of OnShowErrorReport.
            // If you skip this then link to report details won't be showing.
            OnShowErrorReport += delegate(object sender, SendExceptionClickEventArgs ar)
            {
                new FormExceptionStackTrace(ar.UnhandledException).Show();
            };            
        }

        public UnhandledExceptionHandler()
        {
            Initialize();

            // Implement your sending protocol here. You can use any information from System.Exception
            OnSendExceptionClick += OnSendErrorReport;
        }

        public UnhandledExceptionHandler(SendExceptionClickHandler handler)
        {
            Initialize();
            OnSendExceptionClick += handler;
        }


        private void OnSendErrorReport(object sender, SendExceptionClickEventArgs ar)
        {
            // User clicked on "Send Error Report" button:
            if (ar.SendExceptionDetails)
            {
                if (SmtpClient == null)
                {
                    SmtpClient = FormSmtpClientConfig.GetSmtpClientConfig();
                }

                var sb = new StringBuilder("----------------- Exception informations ----------------------");
                sb.AppendLine();
                sb.AppendLine();
                var appName = Process.GetCurrentProcess().ProcessName;
                sb.Append("Application:" + appName + Environment.NewLine);
                sb.Append("\r\nAssembly:");
                sb.AppendLine(Assembly.GetEntryAssembly().FullName);
                sb.Append("\r\nException type:");
                sb.Append(ar.UnhandledException.GetType().ToString());
                sb.Append("\r\nException message:");
                sb.Append(ar.UnhandledException.Message);
                sb.Append("\r\nStack trace:\r\n");
                sb.Append(ar.UnhandledException.StackTrace);

                sb.Append("\r\n");
                sb.Append("\r\nSVN infos-------------------\r\n");
                sb.Append("\r\nSVN Version:" + SvnInfo.Version);
                sb.Append("\r\nSVN Branch:" + SvnInfo.Branch);

                sb.AppendLine();
                sb.AppendFormat("Is64BitOperatingSystem={0}\r\n", Environment.Is64BitOperatingSystem);
                sb.AppendFormat("Is64BitProcess={0}\r\n", Environment.Is64BitProcess);
                sb.AppendFormat("ProcessorCount={0}\r\n", Environment.ProcessorCount);
                sb.AppendFormat("MachineName={0}\r\n", Environment.MachineName);
                sb.AppendFormat("OSVersion={0}\r\n", Environment.OSVersion);
                sb.AppendLine();
                sb.AppendFormat("UserName={0}\r\n", Environment.UserName);
                sb.AppendFormat("Version={0}\r\n", Environment.Version);
                sb.AppendFormat("WorkingSet={0}\r\n", Environment.WorkingSet);
                sb.AppendFormat("CommandLine={0}\r\n", Environment.CommandLine);
                sb.AppendFormat("CommandLineArgs={0}\r\n", Environment.GetCommandLineArgs());
                
                sb.AppendFormat("SystemDirectory={0}\r\n", Environment.SystemDirectory);
                sb.AppendFormat("UserInteractive={0}\r\n", Environment.UserInteractive);
                sb.AppendFormat("SystemPageSize={0}\r\n", Environment.SystemPageSize);

                if (!String.IsNullOrEmpty(ApplicationInfo))
                    sb.AppendFormat("Application INFO\r\n{0}\r\n", ApplicationInfo);
                
                
                var title = "Bugs on program " + appName;

                var frm = new FormErrorReportViaMail()
                {
                    Title = title,
                    To = Recipient,
                    ReplyTo = SmtpClient.User,
                    Contents = @"
- How to re-produce this bug:
- Any more information:
",
                    AutoGeneratedContents = sb.ToString(),
                    SmtpClient = SmtpClient,
                };

                frm.Show();
            }

            // User wants to restart the App:
            if (ar.RestartApp)
            {
                Console.WriteLine("The App will be restarted...");
                System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath);
            }
        }

        /// <summary>
        /// installs unhandled exception handler
        /// http://www.codeproject.com/Articles/14912/A-Simple-Class-to-Catch-Unhandled-Exceptions-in-Wi
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
        /// Exception thrown in a thread normally couldn't be caught in another thread.
        /// </summary>
        public void Install()
        {
            // Add the event handler for handling UI thread exceptions to the event:
            Application.ThreadException += ThreadExceptionFunction;

            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler:
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event:
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionFunction;
        }



	    /// <summary>
	    /// Set to true if you want to restart your App after falure
	    /// </summary>
	    private bool RestartApp { get; set; } = true;

        public delegate void SendExceptionClickHandler(object sender, SendExceptionClickEventArgs args);

        //public delegate void ShowErrorReportHandler(object sender, System.EventArgs args);

        /// <summary>
        /// Occurs when user clicks on "Send Error report" button
        /// </summary>
        public event SendExceptionClickHandler OnSendExceptionClick;

        /// <summary>
        /// Occurs when user clicks on "click here" link lable to get data that will be send
        /// </summary>
        public event SendExceptionClickHandler OnShowErrorReport;


        /// <summary>
        /// Handle the UI exceptions by showing a dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreadExceptionFunction(Object sender, ThreadExceptionEventArgs e)
        {
            ShowUnhandledExceptionDlg(e.Exception);
        }

        /// <summary>
        /// Handle the UI exceptions by showing a dialog box
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="args">Passing arguments: original exception etc.</param>
        private void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
            ShowUnhandledExceptionDlg((Exception)args.ExceptionObject);
        }

        public static int TotalNumberOfUnhandledException { get; private set; }
        public static Subject<Exception> UnhandledExceptionSubject = new Subject<Exception>();

        /// <summary>
        /// Raise Exception Dialog box for both UI and non-UI Unhandled Exceptions
        /// </summary>
        public void ShowUnhandledExceptionDlg(Exception e, string title=null)
        {
            // unhandled exception 이 발생했음을 공지
            TotalNumberOfUnhandledException = TotalNumberOfUnhandledException + 1;
            UnhandledExceptionSubject.OnNext(e);

            Globals.Logger.Error($"Unhandled Exception:\r\n{e.ToString()}");

            /*
             * Visual studio 에서 실행했다면, error reporting 을 할 필요가 없음.  exception stack 만 보여 주고 끝!
             */
            if (DEBUG.LaunchFromVisualStudio_p())
            {
                var form = new FormExceptionStackTrace(e) { Icon = Icon };
                form.Text = title.NonNullEmptySelector(form.Text);
                form.Show();
                return;
            }

            if (IsDisableErrorReporting)
            {
                ExceptionHider.SwallowException(e);
                return;
            }


            Exception unhandledException = e;

            if(unhandledException == null)
                unhandledException = new Exception("Unknown unhandled Exception was occurred!");

            FormUnhandledException exDlgForm = new FormUnhandledException() { Icon = Icon };
            try
            {
                string appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                exDlgForm.Text = appName;
                exDlgForm.labelTitle.Text = String.Format(exDlgForm.labelTitle.Text, appName);
                exDlgForm.checkBoxRestart.Text = String.Format(exDlgForm.checkBoxRestart.Text, appName);
                exDlgForm.checkBoxRestart.Checked = this.RestartApp;

                // Do not show link label if OnShowErrorReport is not handled
                exDlgForm.labelLinkTitle.Visible = (OnShowErrorReport != null);
                exDlgForm.linkLabelData.Visible = (OnShowErrorReport != null);

                // Disable the Button if OnSendExceptionClick event is not handled
                exDlgForm.buttonSend.Enabled = (OnSendExceptionClick != null);

                // Attach reflection to checkbox checked status
                exDlgForm.checkBoxRestart.CheckedChanged += delegate(object o, EventArgs ev)
                {
                    RestartApp = exDlgForm.checkBoxRestart.Checked;
                };

                // Handle clicks on report link label
                exDlgForm.linkLabelData.LinkClicked += delegate(object o, LinkLabelLinkClickedEventArgs ev)
                {
                    if(OnShowErrorReport != null)
                    {
                        SendExceptionClickEventArgs ar = new SendExceptionClickEventArgs(true, unhandledException, RestartApp);
                        OnShowErrorReport(this, ar);
                    }
                };

                // Show the Dialog box:
                bool sendDetails = (exDlgForm.ShowDialog() == System.Windows.Forms.DialogResult.Yes);

                if(OnSendExceptionClick != null)
                {
                    SendExceptionClickEventArgs ar = new SendExceptionClickEventArgs(sendDetails, unhandledException, RestartApp);
                    OnSendExceptionClick(this, ar);
                }
            }
            finally
            {
                exDlgForm.Dispose();
            }
        }

        public static void ShowUnhandledException(Exception e, string title=null)
        {
            new UnhandledExceptionHandler().ShowUnhandledExceptionDlg(e, title);
        }
    }


    public class SendExceptionClickEventArgs : EventArgs
    {
        public bool SendExceptionDetails;
        public Exception UnhandledException;
        public bool RestartApp;

        public SendExceptionClickEventArgs(bool SendDetailsArg, Exception ExceptionArg, bool RestartAppArg)
        {
            SendExceptionDetails = SendDetailsArg;     // TRUE if user clicked on "Send Error Report" button and FALSE if on "Don't Send"
            UnhandledException = ExceptionArg;         // Used to store captured exception
            RestartApp = RestartAppArg;                // Contains user's request: should the App to be restarted or not
        }
    }

    /// <summary>
    /// SmtpClient 에 저장하는 credentails (user 및 passwd) 정보는 다시 꺼내올 방법이 없어서,
    /// 나중에 꺼내기 위해서 credentail 에 저장할 때에 user 및 password 를 따로 보관한다.
    /// </summary>
    public class SmtpClientEx : SmtpClient
    {
        public SmtpClientEx() { }
        public SmtpClientEx(string server, int port)
            : base(server, port)
        { }

        public string User { get; set; }
        public string Password { get; set; }
    }

}
