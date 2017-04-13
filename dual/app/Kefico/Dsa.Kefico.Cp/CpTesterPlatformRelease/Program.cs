//#define CP_TESTER_CU
#define CP_TESTER_SS

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CpTesterPlatform.CpTester;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CpTesterPlatform
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			// install exception handler
			new UnhandledExceptionHandler() { /*Icon = Icons.SomeIcon*/ }.Install();

#if CP_TESTER_CU
			var form = new FormAppCu();
#elif CP_TESTER_SS
			var form = new FormAppSs();
#endif
			var exe = Assembly.GetEntryAssembly().Location;
			var dir = Path.GetDirectoryName(exe);

            // log4net 에서 loggging 을 수행할 대상 assemblies
#if CP_TESTER_CU
            var dlls = new[] { "CpCommon.dll", "CpTesterCu.dll", "CpApplication.dll" /*"CpTesterCu.dll"*/ };
#elif CP_TESTER_SS
			var dlls = new[] { "CpCommon.dll", "CpTesterSs.dll", "CpApplication.dll" /*"CpTesterCu.dll"*/ };
#endif
            var assemblies = Assembly.GetEntryAssembly().ToEnumerable()
				.Union(dlls.Select(d => Assembly.LoadFrom(Path.Combine(dir, d))));

			// log4net configuration file
	        var configfile = "log4netCp.xml";

			// log file name "*.log"
			var logfile = Path.Combine(dir, Path.GetFileNameWithoutExtension(exe));     // .log will be automatically attached

			Log4NetWrapper.Install(form, configfile, assemblies, logfile);
			LogProxy.CurrentLoggers.ForEach(l => l.EnableAll());

			Application.Run(form);
		}
	}
}
