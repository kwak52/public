using CpTesterPlatform.CpTStepDev.Interface;
using Dsu.Driver;
using static Dsu.Driver.Ups;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevUPS_APC : CpDevNormalBase, IUPS
    {
        public string IP_ADDRESS { get; set; }
        private UpsManager ups() => Ups.manager();

        public override bool DevClose()
        {
            ups().Close();
            return true;
        }

        public override bool DevOpen()
        {
            Ups.createManager(IP_ADDRESS);
            return true;
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
