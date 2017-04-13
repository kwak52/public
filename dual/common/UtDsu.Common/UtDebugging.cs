// http://www.codeproject.com/Tips/323212/Accurate-way-to-tell-if-an-assembly-is-compiled-in

using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtDebugging
    {
        private static object AnonymousReturn()
        {
            return new { Name = "Pranay", EmailID = "pranayamr@gmail.com" };
        }

        [TestMethod]
        public void TestMethodDebuggingMode()
        {
            bool bDebugMode = DEBUG.DebugMode_p(Assembly.GetExecutingAssembly());
#if DEBUG
            Assert.IsTrue(bDebugMode);
#else
            Assert.IsFalse(bDebugMode);
#endif

            Version ver = Environment.Version;
            DEBUG.WriteLine("DOTNET version : {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);

            // Unit Test environment does not work for DEBUG.LaunchFromVisualStudio_p()
            /*
            Assert.IsTrue(DEBUG.LaunchFromVisualStudio_p());

            if (DEBUG.LaunchFromVisualStudio_p())
                Tools.ShowMessage("Launched from Visual Studio!!");
            */

            // dynamic only works on DotNet 4.5 or greater
            /*
            dynamic o = AnonymousReturn();
            if (o.Name == "Pranay")
            {
                Console.WriteLine("Good");
            }
            */
        }

        [TestMethod]
        public void TestMethodDebuggingHandleException()
        {
            /* This will bug you due to dialog box */
            //try
            //{
            //    int a = 0;
            //    int b = 10 / a;
            //}
            //catch (System.Exception ex)
            //{
            //    DEBUG.HandleException(ex);
            //}
        }
    }
}
