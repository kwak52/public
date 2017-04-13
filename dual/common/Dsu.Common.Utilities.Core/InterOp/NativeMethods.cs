using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities
{
    public class NativeMethods
    {
        // http://ecomnet.tistory.com/41
        // http://www.codeproject.com/Articles/27298/Dynamic-Invoke-C-DLL-function-in-C

        [DllImport("kernel32.dll")]
        public static extern int LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(int hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(int hModule);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Ftn_Void_Void();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Ftn_Int_Void();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Ftn_Int_String(string args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string Ftn_String_String(string args);


        #region Console attach/detach
        // http://www.codeproject.com/Tips/68979/Attaching-a-Console-to-a-WinForms-application
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int FreeConsole();

        private void ShowConsoleAttachmentSample()
        {
            NativeMethods.AllocConsole();
            Console.WriteLine("Debug Console");
            //Console.ReadKey();

        }
        #endregion



        // http://bytes.com/topic/c-sharp/answers/229029-p-invoke-returning-passing-bools-between-c-c
        [DllImport("my.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool SomeFunctionReturningBool();

        [DllImport("my.dll")][return: MarshalAs(UnmanagedType.I1)]
        private static extern bool SomeFunctionReturningBool2();


        public void ShowSample()
        {
            //int hModule = LoadLibrary(@"V:\WorkingSVN\PLCStudio\trunk\bin\Gp.dll");
            //IntPtr proc = GetProcAddress(hModule, "CheckDummyCall");
            //Ftn_String_String ftn = (Ftn_String_String)Marshal.GetDelegateForFunctionPointer(proc, typeof(Ftn_String_String));
            //string result = ftn("test function");
        }
    }
}
