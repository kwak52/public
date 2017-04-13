using System;
using Dsu.PLC.Melsec;

namespace Dsu.PLC.Melsec
{
    internal class MxCpu : ICpu
    {
        private McProtocolApp McProtocol { get; }
        public string Model { get; }
        public bool IsRunning { get { throw new NotSupportedException("PLC status check not supported for MELSEC PLC."); } }

        public void Stop() => McProtocol.RemoteStop();
        public void Run() => McProtocol.RemoteRun();

        public MxCpu(McProtocolApp mcProtocol)
        {
            McProtocol = mcProtocol;
           // Model = mcProtocol.ReadModelName();  //Q06UDV 에서 return error 코드 49241 발생
        }
    }
}
