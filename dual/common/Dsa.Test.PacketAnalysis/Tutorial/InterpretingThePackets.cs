using System;
using System.Net;
using System.Collections.Generic;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace Dsa.Test.PacketAnalysis
{
    public static class InterpretingThePackets
    {
        private static string monitoringIp = "192.168.0.100";      // localhost 와의 통신 monitoring host 의 ip
        private static string filterExpression = "ip host " + monitoringIp + " and (ip and (tcp or udp))";
        private static int counter = 0;
        private static readonly string localIp;
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        static InterpretingThePackets()
        {
            localIp = GetLocalIPAddress();
        }

        public static void Do()
        {
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                Console.WriteLine("No interfaces found! Make sure WinPcap is installed.");
                return;
            }

            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (No description available)");
            }

            PacketDevice selectedDevice = null;
            if (allDevices.Count == 1)
                selectedDevice = allDevices[0];
            else
            {
                int deviceIndex = 0;
                do
                {
                    Console.WriteLine("Enter the interface number (1-" + allDevices.Count + "):");
                    string deviceIndexString = Console.ReadLine();
                    if (!int.TryParse(deviceIndexString, out deviceIndex) ||
                        deviceIndex < 1 || deviceIndex > allDevices.Count)
                    {
                        deviceIndex = 0;
                    }
                } while (deviceIndex == 0);

                // Take the selected adapter
                selectedDevice = allDevices[deviceIndex - 1];
            }

            // Open the device
            using (PacketCommunicator communicator =
                selectedDevice.Open(65536,                                  // portion of the packet to capture
                // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    1000))                                  // read timeout
            {
                // Check the link layer. We support only Ethernet for simplicity.
                if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
                {
                    Console.WriteLine("This program works only on Ethernet networks.");
                    return;
                }

                // Compile the filter
                using (BerkeleyPacketFilter filter = communicator.CreateFilter(filterExpression))
                {
                    // Set the filter
                    communicator.SetFilter(filter);
                }

                Console.WriteLine("Listening on " + selectedDevice.Description + "...");

                // start the capture
                communicator.ReceivePackets(0, PacketHandler);
            }
        }

        // Callback function invoked by libpcap for every incoming packet
        private static void PacketHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;
            TcpDatagram tcp = ip.Tcp;

            // wireshark 에서 data 로 보이는 부분
            Datagram data = udp == null ? tcp.Payload : udp.Payload;

            var sourceIp = ip.Source.ToString();
            var destIp = ip.Destination.ToString();
            //if ((sourceIp == localIp || sourceIp == monitoringIp) && (destIp == localIp || destIp == monitoringIp))
            {
                // print timestamp and length of the packet
                //Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length);

                // print ip addresses and udp ports
                //Console.WriteLine("  " + ip.Source + ":" + udp.SourcePort + " -> " + ip.Destination + ":" + udp.DestinationPort);
                Console.WriteLine(String.Format("  Data({0})={1}", data.Length, data.ToHexadecimalString()));
                System.Diagnostics.Trace.WriteLine(String.Format("[{0,-3}] {1,-4}{2}", counter++, data.Length, data.ToHexadecimalString()));
            }
        }
    }
}
