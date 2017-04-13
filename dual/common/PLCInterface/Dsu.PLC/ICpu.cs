namespace Dsu.PLC
{
    public interface ICpu
    {
        /// <summary> PLC cpu model </summary>
        string Model { get; }

        /// <summary> True if PLC is RUNNING state. </summary>
        bool IsRunning { get; }

        /// <summary> Stop the PLC </summary>
        void Stop();

        /// <summary> Start the PLC </summary>
        void Run();
    }
}
