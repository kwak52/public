using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace McProtocolTester
{
    static class Program
    {
        /// <summary>
        /// Is the application's main entry point.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
