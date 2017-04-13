using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DriverTestWinform
{
    static class Program
    {
        private static async Task<string> GetString()
        {
            await Task.Delay(2000);
            Trace.WriteLine("Done");
            return "Done";
        }

        private static void Test()
        {
            Stopwatch xSW = new Stopwatch();
            xSW.Start();

            Trace.WriteLine($"1:{xSW.ElapsedMilliseconds}");
            Task<string> tstr = Task.Factory.StartNew(async () => await GetString()).Result;
            Trace.WriteLine($"2:{xSW.ElapsedMilliseconds}");
            var str = tstr.Result;
            Trace.WriteLine($"3:{xSW.ElapsedMilliseconds}");
            Trace.WriteLine($"Done with {str}");
            //            Task.Run(() => GetString());
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Trace.WriteLine("Starting");
            Test();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
