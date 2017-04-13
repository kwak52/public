using System;
using CpTesterPlatform.CpTStepDev.Interface;
using static Dsu.Driver.NiVISA;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevLVDT_Sony : CpDevNormalBase, ILVDT
    {
        public string COM_PORT { get; set; }
        private Rs232cManager Lvdt;

        public override bool DevClose()
        {
            Lvdt = null;
            return true;
        }

        public override bool DevOpen()
        {
            Lvdt = new Rs232cManager(COM_PORT);
            //Lvdt.BaudRate = 38400;
            //Console.WriteLine(Lvdt.QuerySimple("VER=?\r\n"));
            //Console.WriteLine(Lvdt.QuerySimple("*r\r\n"));   //데이터 패턴 A-01.6040, A+00.7241
            return true;
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
