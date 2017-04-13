using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using log4net;
using log4net.Config;


namespace Cpt.Winform
{
    static class Program
    {
        private static ILog _logger;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Configure log4net, reading the TopshelfLog4net.config file
            XmlConfigurator.ConfigureAndWatch(new FileInfo(CptManagerModule.log4netConfigFile));
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            _logger = LogManager.GetLogger("CptLogger");
            CptManagerModule.CptManager.SetLogger(_logger);

            _logger.Info(@"-------- CP Tester system started. --------");

            var allocConsole = ConfigurationManager.AppSettings["allocConsole"];
            if (allocConsole == "yes")
            {
                AllocConsole();
                Console.SetBufferSize(Console.BufferWidth, 3000);
                Console.Title = "Cpt.Winform: " + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            }


            /* test parsing */
            //var filePath = "C:/pruef_cp/testList/MMXX/p9001270003.CpXv01e";
            //var ttnrvar = new KeyValuePair<string, string>("9001270003", "01");
            //var gatelist = new[] {"H"}.ToList();
            //var gaudiReadData = PsCCS.PsCCSGaudiFile.PsCCSPaseDataApi(filePath, ttnrvar, gatelist, null);


            Application.Run(new FormCptRequest(_logger));
            //Application.Run(new FormLauncher(logger));
            //Application.Run(new FormCptApp(logger));
            //   Application.Run(new FormPdvApp(logger));
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
