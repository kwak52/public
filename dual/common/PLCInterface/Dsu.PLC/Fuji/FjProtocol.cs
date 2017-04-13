using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Dsu.PLC.Fuji
{
    public class FjProtocol : IDisposable
    {
        internal FjCpu Cpu { get; private set; }
        private TcpClient Client { get; set; }
        private NetworkStream Stream { get; set; }
        public string HostName { get; set; } // host name or IP address
        public int PortNumber { get; set; } // port number

        public FjProtocol(string iHostName, int iPortNumber)
        {
            Client = new TcpClient();
            HostName = iHostName;
            PortNumber = iPortNumber;
        }

        public void Dispose()
        {
            Close();
        }


        public int Open()
        {
            DoConnect();
            Cpu = new FjCpu(this);
            return 0;
        }

        public int Close()
        {
            DoDisconnect();
            return 0;
        }

        protected void DoConnect()
        {
            TcpClient c = Client;
            if (!c.Connected)
            {
                // Implementation of Keep Alive function
                var ka = new List<byte>(sizeof(uint)*3);
                ka.AddRange(BitConverter.GetBytes(1u));
                ka.AddRange(BitConverter.GetBytes(45000u));
                ka.AddRange(BitConverter.GetBytes(5000u));
                c.Client.IOControl(IOControlCode.KeepAliveValues, ka.ToArray(), null);
                c.Connect(HostName, PortNumber);
                Stream = c.GetStream();
            }
        }

        protected void DoDisconnect()
        {
            TcpClient c = Client;
            if (c.Connected)
            {
                c.Close();
            }
        }

        internal byte[] Execute(byte[] iCommand)
        {
            NetworkStream ns = Stream;
            ns.Write(iCommand, 0, iCommand.Length);
            ns.Flush();

            using (var ms = new MemoryStream())
            {
                var buff = new byte[256];
                do
                {
                    int sz = ns.Read(buff, 0, buff.Length);
                    if (sz == 0)
                    {
                        throw new Exception("Disconnected");
                    }
                    ms.Write(buff, 0, sz);
                } while (ns.DataAvailable);
                return ms.ToArray();
            }
        }




		//internal IEnumerable<byte> GetReadRequestPacket(MemoryType memoryType, uint startAddress,
		//    ushort requestByteLength, int cpuNumber, byte busNumber)
		//{
		//    ConnectionMode connectionMode = ConnectionMode.Cpu0;
		//    byte connectionId = (byte)0;
		//    if (cpuNumber != 0)
		//    {
		//        connectionMode = ConnectionMode.NonCpu0;
		//        connectionId = busNumber;
		//    }

		//    return FjPacketRequest.GetReadRequestPacket(memoryType, startAddress, requestByteLength, connectionMode, connectionId);

		//}

		internal IEnumerable<byte> GetReadRequestPacket(MemoryType memoryType, uint startAddress,
            ushort requestByteLength, ConnectionMode connectionMode = ConnectionMode.Cpu0, byte connectionId = 0x0)
        {
            return FjPacketRequest.GetReadRequestPacket(memoryType, startAddress, requestByteLength, connectionMode, connectionId);
        }

    }
}
