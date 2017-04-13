using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestRegistryKey
    {
        [TestMethod]
        public void TestMethodOpenNonExistingKey()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\UDMTEK\Sharp3dXXXX", true);
            if (key == null)
                Registry.CurrentUser.CreateSubKey(@"Software\UDMTEK\Sharp3dXXXX");
            Trace.WriteLine(key);
        }
    }
}
