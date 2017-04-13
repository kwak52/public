using System;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using System.Reflection;
using static Dsu.Driver.NiVISA;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevLVDT_Sony : ILVDT
    {
        public CpFunctionEventHandler FuncEvtHndl { get; set; }

        public string COM_PORT { get; set; }
        public string DeviceID { get; set; }
        private Rs232cManager Lvdt;

        public bool DevClose()
        {
            Lvdt = null;
            return true;
        }

        public bool DevOpen()
        {
            Lvdt = new Rs232cManager(COM_PORT);
            //Lvdt.BaudRate = 38400;
            //Console.WriteLine(Lvdt.QuerySimple("VER=?\r\n"));
            //Console.WriteLine(Lvdt.QuerySimple("*r\r\n"));   //데이터 패턴 A-01.6040, A+00.7241
            return true;
        }

        public bool DevReset()
        {
            return true;
        }

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }

        public double GetFuntionDimension()
        {
            string readData = Lvdt.QuerySimple("*r\r\n");   //데이터 패턴 A-01.6040, A+00.7241

            return Convert.ToDouble(readData.Substring(1, readData.Length - 1));
        }

        public void SetMasterDimension()
        {
            Lvdt.Write("*RCL\r\n");
        }
    }
}
