using System;
using System.IO;
using System.Reflection;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpTStepDev
{
    public class CpTStepDevLib
    {
        static public object createInstance(string libName)
        {
            var tResult = TryFunc(() =>
            {
                Assembly asm = Assembly.LoadFile(Directory.GetCurrentDirectory() + @"\CpTStepDev" + libName + ".dll");

                if (asm == null)
                {
                    return null;
                }

                Type typeLib = asm.GetType("KCpDevice.Cls" + libName);

                return Activator.CreateInstance(typeLib);
            });
            return tResult.Succeeded ? tResult.Result : null;
        }
    }
}
