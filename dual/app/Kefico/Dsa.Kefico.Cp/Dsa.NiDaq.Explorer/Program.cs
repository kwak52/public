using System;
using System.Windows.Forms;
using System.IO;
using log4net;
using log4net.Config;
using Dsu.Driver.UI.NiDaq;


namespace Dsa.NiDaq.Explorer
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure log4net, reading the TopshelfLog4net.config file
            XmlConfigurator.ConfigureAndWatch(new FileInfo("NiDaqExplorerLog4net.xml"));
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            var logger = LogManager.GetLogger("MyLogger");
            Log4NetWrapper.SetLogger(logger);  // on Dsu.Common.Utilities.FS

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormNiDaqExplorer(allowManagerCreation:true));
        }
    }
}
