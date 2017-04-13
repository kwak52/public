using System;
//using SentinelLicLib;
//using Interop.Gp;
using System.Reflection;

namespace Dsu.Common.Utilities
{
    public class License
    {
        static public bool CheckLicense(string strAppName) { return CheckLicense(strAppName, true, true, "CheckLicense2"); }
        static public bool CheckLicense(string strAppName, bool bCheckUsbSerial, bool bCheckSwSerial)   { return CheckLicense(strAppName, bCheckUsbSerial, bCheckSwSerial, "CheckLicense2"); }
        static public bool CheckLicense(string strAppName, bool bCheckUsbSerial/*=true*/, bool bCheckSwSerial/*=true*/, string strCheckFunction/*="CheckLicense2"*/)
        {
            const string progId = "LicenseChecker.LicenseChecker";

            try
            {
                object oLicenseCheckerComInstance = CProcess.GetApplicationInstanceFromProgId(progId);
                if (oLicenseCheckerComInstance == null)
                {
                    Tools.ShowMessage("Failed to create instance of {0}!!!", progId);
                    return false;
                }

                // TIPS : Invoking named method
                Type licenseServerType = System.Type.GetTypeFromProgID(progId);
                object[] parameters = new object[3] { strAppName, bCheckUsbSerial, bCheckSwSerial };
                object oResult = licenseServerType.InvokeMember(strCheckFunction, BindingFlags.InvokeMethod, null, oLicenseCheckerComInstance, parameters);

                return (bool)oResult;
            }
            catch (System.Exception e)
            {
                Tools.ShowMessageOnce("Exception on License.CheckLicense({0}) :\r\n{1}", strAppName, e.Message);
                return false;
            }
        }
    }
}
