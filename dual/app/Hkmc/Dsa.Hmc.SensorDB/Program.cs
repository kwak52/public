using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using Dsu.Common.Utilities;
using Dsu.LicenseKeyChecker;

namespace Dsa.Hmc.SensorDB
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            UserLookAndFeel.Default.SkinName = "DevExpress Dark Style";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CLicense cLicense = new CLicense("SensorDB", Application.StartupPath);
            if (cLicense.CheckLicense())
                Application.Run(new MainForm());
        }
    }
}
