using CpTesterPlatform.CpTStepDev.Interface;
using static Dsu.Driver.Sorensen;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevPowerSupply_Sorensen : CpDevNormalBase, IPowerSupply
    {
        public string CONTROLLER_ADDRESS { set; get; } = "0.0.0.0";
        public int ChannelID { set; get; }

        private PowerSupplierManager power;
        public override bool DevClose()
        {
            power.Clear(ChannelID);
            return true;
        }

        public override bool DevOpen()
        {
            var oResult = TryFunc(() =>
            {
                power = new PowerSupplierManager(CONTROLLER_ADDRESS, 9221, ChannelID);
             //   power = new PowerSupplierManager("COM1", ChannelID); //RS232
                power.Reset(ChannelID);

                return true;
            });

            if (oResult.HasException) return false;
            return oResult.Result;
        }

        
        public override bool DevReset()
        {
            SetVoltage(0);
            SetCurrent(0);
            SetOutput(false);
            return true;
        }


        public string GetVersion()
        {
            return "";
        }

        public bool GetOutput() { return power.IsActive; }
        public double GetVoltage() { return power.Voltage; }
        public double GetCurrent() { return power.Current; }
        public double GetObservedVoltage() { return power.VoltageObserve; }
        public double GetObservedCurrent() { return power.CurrentObserve; }  
        public bool SetOutput(bool enable)
        {
            var oResult = TryFunc(() =>
            {
                power.Clear(ChannelID);
                power.IsActive = enable;
                return true;
            });

            if (oResult.HasException) return false;
            return oResult.Result;
        }

        public bool SetVoltage(double voltage)
        {
            var oResult = TryFunc(() =>
            {
                power.Clear(ChannelID);
                power.IsActive = true;
                power.Voltage = voltage;
                return true;
            });

            if (oResult.HasException) return false;
            return oResult.Result;
        }

        public bool SetCurrent(double current)
        {
            var oResult = TryFunc(() =>
            {
                power.Clear(ChannelID);
                power.IsActive = true;
                power.Current = current;
                return true;
            });

            if (oResult.HasException) return false;
            return oResult.Result;
        }
    }
}
