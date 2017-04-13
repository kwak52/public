using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Akka.Actor;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Forms;

namespace Dsu.Common.Utilities
{
    public abstract class CommonApplication : IApplication, ITopLevelHelpProvider, ISubscribable
    {
        private static LogProxy _logger;

        public static LogProxy Logger
        {
            get { return _logger; }
        }

        /// <summary>
        /// AKKA actor system
        /// </summary>
        public static ActorSystem ActorSystem { get; private set; }

        /// <summary>
        /// Application 내에서 subscription 한 내용들을 전부 관리
        /// </summary>
        protected List<IDisposable> _applicationSubscriptions = new List<IDisposable>();

        public void AddSubscription(IDisposable subscription)
        {
            _applicationSubscriptions.Add(subscription);
        }

        public Subject<IObservableEvent> ApplicationSubject = new Subject<IObservableEvent>();

        public virtual string ApplicationName { get; protected set; }
        public abstract string RegistryLocation { get; protected set; }
        public abstract string StatusBarText { get; set; }
        public abstract Icon DefaultIcon { get; }

        public abstract HelpProvider ContextHelp { get; }

        public virtual string HelpFilePath
        {
            get { return Application.StartupPath + "\\DualsoftApp.chm"; }
        }

        public int PID
        {
            get { return Process.GetCurrentProcess().Id; }
        }

        public virtual string Version { get; set; }

        /// <summary> Application 의 최상위 Form  </summary>
        public static Form TheOuterApplicationForm { get; protected set; }

        public static CommonApplication TheCommonApplication { get; private set; }

        public static bool IsInstalledVersion
        {
            get
            {
                var pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                return Assembly.GetExecutingAssembly()
                    .Location.StartsWith(pf, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static string GetProfilePath(string programName = "DualsoftApp", bool makeDirOnDemand = true)
        {
            var myProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dir = String.Format(@"{0}\Dualsoft\{1}", myProfilePath, programName);
            if (makeDirOnDemand && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        public static void ShowChangeLog(string changeLogFile)
        {
            using (new CwdChanger(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)))
                new FormSimpleEditor()
                {
                    Title = "Change log"
                    ,
                    Multiline = true
                    ,
                    ReadOnly = true
                    ,
                    Contents = System.IO.File.ReadAllText(changeLogFile)
                }.Show();
        }

        public static CommonConfiguration TheCommonConfiguration
        {
            get { return CommonConfiguration.TheCommonConfiguration; }
        }


        public event EventHandler ActiveViewChangedHook;
        public event EventHandler ActiveDocumentChangedHook;

        protected EventHandler GetActiveViewChangedHook()
        {
            return ActiveViewChangedHook;
        }

        protected EventHandler GetActiveDocumentChangedHook()
        {
            return ActiveDocumentChangedHook;
        }

        public virtual IDocument ActiveDocument
        {
            get { throw new NotReimplementedException(); }
            set
            {
                ActiveDocumentChangedHook.Handle(this, new ApplicationEventArgs() {Application = this, Document = value});
            }
        }

        public virtual IView ActiveView
        {
            get { throw new NotReimplementedException(); }
            set { ActiveViewChangedHook.Handle(this, new ApplicationEventArgs() {Application = this, View = value}); }
        }


        public CommonApplication(LogProxy logger)
        {
            _logger = logger;
            TheCommonApplication = this;

            try
            {
                Version = String.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
                ApplicationName = Application.ProductName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to set Application version and name. exception = " + ex.ToString());
            }

            ActorSystem = ActorSystem.Create("ActorSystem");

            ApplicationIdleTimer.ApplicationIdle += args => { OnApplicationIdle(args); };
        }

        public virtual void OnApplicationIdle(ApplicationIdleTimer.ApplicationIdleEventArgs args)
        {
            //FmtTrace.WriteLine("Application idle.");
        }

        public static void ShowErrorFormat()
        {
            throw new NotImplementedException("ShowErrorFormat");
        }

        public static void ShowError(string message, string title = "")
        {
            System.Media.SystemSounds.Beep.Play();
			if(_logger != null) _logger.Error(message);
            MessageBox.Show(message, title.NonNullEmptySelector("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void DisplayError(string message)
        {
            //if (verbose)
            ShowError(message);
            //else
            _logger.Error(message);
        }

        public static void ShowWarn(string message, string title = "")
        {
            System.Media.SystemSounds.Beep.Play();
            _logger.Warn(message);
            MessageBox.Show(message, title.NonNullEmptySelector("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ShowErrorStatic(string message, string title = "") { ShowError(message, title); }

        public virtual string GetApplicationInfo()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Name: {0}\r\n", ApplicationName);
            sb.AppendFormat("Path: {0}\r\n", Assembly.GetEntryAssembly().Location);
            sb.AppendFormat("Version: {0}\r\n", Version);

            sb.AppendFormat("SVN Version: {0}\r\n", SvnInfo.Version);
            sb.AppendFormat("SVN Branch: {0}\r\n", SvnInfo.Branch);
            sb.AppendFormat("SVN Author: {0}\r\n", SvnInfo.Author);
            sb.AppendFormat("SVN LastChangeDate: {0}\r\n", SvnInfo.LastChangeDate);
            sb.AppendFormat("Installer build date: {0}\r\n", SvnInfo.PackageInstallDate);
            return sb.ToString();
        }

        private bool _disposed;

        ~CommonApplication()
        {
            Dispose(false);     // false : 암시적 dispose 호출
        }
        public void Dispose()
        {
            Dispose(true);      // true : 명시적 dispose 호출
            GC.SuppressFinalize(this);  // 사용자에 의해서 명시적으로 dispose 되었으므로, GC 는 이 객체에 대해서 손대지 말것을 알림.
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // AwaitTermination() call hangs.  Need to check Akka document.
                // CommonApplication.ActorSystem.AwaitTermination();
                _applicationSubscriptions.ForEach(s => s.Dispose());
            }

            _disposed = true;
        }
    }


    public class ApplicationEventArgs : GenericEventArgs
    {
        public IApplication Application { get; set; }
        public IDocument Document { get; set; }
        public IView View { get; set; }

        public object Tag { get; set; }

        public ApplicationEventArgs(EventArgs innerArgs = null)
            : base(innerArgs)
        {
        }
    }


    //public delegate void ApplicationEventHandler(object sender, ApplicationEventArgs e);
}
