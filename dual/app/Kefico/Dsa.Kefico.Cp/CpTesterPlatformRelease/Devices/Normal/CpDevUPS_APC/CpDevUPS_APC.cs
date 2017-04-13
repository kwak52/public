using System;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using System.Reflection;
using Dsu.Driver;
using static Dsu.Driver.Ups;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevUPS_APC : IUPS
    {
        public CpFunctionEventHandler FuncEvtHndl { get; set; }

        public string IP_ADDRESS { get; set; }
        public string DeviceID { get; set; }
        private UpsManager ups() => Ups.manager();

        public bool DevClose()
        {
            ups().Close();
            return true;
        }

        public bool DevOpen()
        {
            Ups.createManager(IP_ADDRESS);
            return true;
        }

        public bool DevReset()
        {
            throw new NotImplementedException();
        }

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }

        public double GetTemperature()
        {
            return ups().GetTemparature();
        }

        public double GetHumidity()
        {
            return ups().GetHumidity();
        }
    }
}
