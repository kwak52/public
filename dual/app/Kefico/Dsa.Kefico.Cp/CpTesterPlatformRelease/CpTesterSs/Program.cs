using System;
using System.IO;
using System.Windows.Forms;
using CpTesterPlatform.CpTester;
using Dsu.Common.Utilities.Exceptions;
using log4net;
using log4net.Config;
using DevExpress.UserSkins;

namespace CpTesterPlatform
{
    static class Program
    {
        private static ILog _logger;

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // install exception handler
            UnhandledExceptionHandler.IsDisableErrorReporting = true;
            new UnhandledExceptionHandler() { /*Icon = Icons.SomeIcon*/ }.Install();

            // Configure log4net
            XmlConfigurator.ConfigureAndWatch(new FileInfo("CpTesterPlatformLog4net.xml"));
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            // declares our logger
            _logger = LogManager.GetLogger("CptPlatformLogger");
            Log4NetWrapper.SetLogger(_logger);  // on Dsu.Common.Utilities.FS
            Dsu.Common.Utilities.Globals.Logger = _logger;
            CpBase.CpLog4netLogging.Logger = _logger;    // on CpFunctionDefault
            Dsu.Driver.UI.Logging.Logger = _logger;

            _logger.Info("-------------------------------");
            _logger.Info("Common platform tester started.");
            _logger.Info("-------------------------------");


            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(SplashScreenGHI1), true, true);
            var form = new FormAppSs() { Logger = _logger };
            Application.Run(form);
        }
    }
}
