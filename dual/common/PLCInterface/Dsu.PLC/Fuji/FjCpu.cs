using System;

namespace Dsu.PLC.Fuji
{
    public class FjCpu : ICpu
    {
        private FjProtocol FjProtocol { get; }
        public string Model { get; }
        public bool IsRunning { get { throw new NotSupportedException("PLC status check not supported for MELSEC PLC."); } }

        public void Stop() { throw new NotImplementedException(); } // => FjProtocol.RemoteStop();
        public void Run() { throw new NotImplementedException(); } //=> FjProtocol.RemoteRun();

        public FjCpu(FjProtocol fjProtocol)
        {
            FjProtocol = fjProtocol;
            //Model = FjProtocol.ReadModelName();
        }
    }
}
