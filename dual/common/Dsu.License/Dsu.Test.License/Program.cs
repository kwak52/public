using Dsu.LicenseKeyChecker;
using Microsoft.Win32;
using System;
using System.Windows.Forms;



namespace LicenseWoker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool bOK;
            try
            {
                CLicense cLicense = new CLicense("License TEST Program", Application.StartupPath);
                bOK = cLicense.CheckLicense();
            }
            catch (Exception ex)
            {
                bOK = false; Console.WriteLine("License Exception... Key Checking Please.." + ex);
                MessageBox.Show("License Exception... Key Checking Please..");
            }

            if (bOK)
                MessageBox.Show("License OK");

        }
    }
}
