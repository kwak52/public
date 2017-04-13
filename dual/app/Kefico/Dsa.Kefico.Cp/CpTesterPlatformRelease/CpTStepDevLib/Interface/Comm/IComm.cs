namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IComm
    {
        bool WriteData(byte[] writeBuffer);
        bool WriteData(string writeData);

        bool ReadData(ref byte[] readBuffer);
        bool ReadData(ref string readData);
    }
}
