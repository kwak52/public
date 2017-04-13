namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface ISmu : IDevice
    {
        bool DevOpen(string boardName);
        double GetCurrentLimit(int channel);
        double GetVoltageLimit(int channel);
        int NumberOfChannel();
        bool SetVoltage(string channel, double value);
        bool SetCurrent(string channel, double value);
    }
}
