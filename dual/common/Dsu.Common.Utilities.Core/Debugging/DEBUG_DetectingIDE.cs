// http://www.codeproject.com/Tips/152775/Does-Your-App-Know-Where-it-s-Running

using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Dsu.Common.Utilities
{
    partial class DEBUG
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GetModuleFileName([In]IntPtr hModule,
                                                    [Out]StringBuilder lpFilename,
                                                    [In][MarshalAs(UnmanagedType.U4)] int nSize);

        /// <summary> return true if application is launched from Visual Studio IDE </summary>
        public static bool LaunchFromVisualStudio_p()
        {
            StringBuilder moduleName = new StringBuilder(1024);
            int result = GetModuleFileName(IntPtr.Zero, moduleName, moduleName.Capacity);
            string appName = Application.ExecutablePath.ToLower();
            return (appName != moduleName.ToString().ToLower());
        }

        // http://www.codeproject.com/Tips/262338/Self-debugger-attach-to-process
        [DllImport("kernel32.dll")]
        static extern bool IsDebuggerPresent();

        [DllImport("kernel32.dll")]
        static extern void DebugBreak();

        // http://www.codeproject.com/Articles/670193/Csharp-Detect-if-Debugger-is-Attached
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);
    }
}
