﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Network
{
    internal class EthernetNetworkInterface : NetworkInterface
    {
        public EthernetNetworkInterface()
        {
            this.NetworkInterfaceType = NetworkType.Ethernet;
        }
        public bool UseIso { get; set; }
        public PhysicalAddress Mac { get; set; }
        public bool UseIp { get; set; }
        public IPAddress IpAddress { get; set; }
        public IPAddress SubnetMask { get; set; }
        public bool UseRouter { get; set; }
        public IPAddress Router { get; set; }

        public override string ToString()
        {
            return (this.Name ?? "") + ", Ip: " + IpAddress.ToString();
        }
    }
}
