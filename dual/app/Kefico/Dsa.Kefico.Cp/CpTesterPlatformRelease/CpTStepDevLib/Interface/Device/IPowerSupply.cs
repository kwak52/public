namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IPowerSupply : IDevice
    {
        string CONTROLLER_ADDRESS { set; get; }
        int ChannelID { set; get; }

        bool SetOutput(bool enable);
        bool SetVoltage(double voltage);        
        bool SetCurrent(double current);

        bool GetOutput();
        double GetVoltage();
        double GetCurrent();
        double GetObservedVoltage();
        double GetObservedCurrent();
    }
}
