using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dsu.Driver.PaixWrapper
{
    /// <summary>
    /// PAIX 의 native dll NMC.dll 를 환경에 맞게 laoding 하기 위한 class
    /// http://stackoverflow.com/questions/10852634/using-a-32bit-or-64bit-dll-in-c-sharp-dllimport
    /// </summary>
    public static class PlatformSelector
    {
        private static bool _platformSelected = false;

        /// <summary>
        /// NMC.dll 이 32/64 bit platform 고정이므로, 실행 환경에 맞는 dll 을 찾아서 loading 하도록 한다.
        /// </summary>
        public static void SelectPlatform()
        {
            if ( !_platformSelected )
            {
                var prefix = Environment.Is64BitProcess ? "x64" : "x86";
                var exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var platformDll = Path.Combine(exeFolder, Path.Combine(prefix, "NMC2.DLL"));
                //File.Copy(platformDll, Path.Combine(exeFolder));
                LoadLibrary(platformDll);
                _platformSelected = true;
            }
        }

        static PlatformSelector()
        {
            SelectPlatform();
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
    }
}
