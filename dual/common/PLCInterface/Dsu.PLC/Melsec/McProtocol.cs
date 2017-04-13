using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace Dsu.PLC.Melsec
{
	public enum McFrame
    {
        MC3E      // 0x0050u
        , MC4E      // 0x0054u
        // else 0x0000u
    }


// GX Developer 에서 Project tree 창 > Parameter > PLC Parameter 더블클릭 > Device tab 에서 확인
// see 2.3 Device Types @ Data-book.pdf
// see 4.6 Differences between LONG type functions and SHORT type/INT type functions
/*
--------------------------------------------------------------------------------------------------
Device                  Device Name     Device No. Type     Device No.          Remarks
--------------------------------------------------------------------------------------------------
Function input          FX              Bit                 Decimal             [0..16]
Function output         FY              Bit                 Decimal             [0..16]
Function register       FD              Word                Decimal             4 words/1 point  1
Special relay           SM              Bit                 Decimal             —
Special register        SD              Word                Decimal             —
Input relay             X               Bit                 Hexadecimal         X000 ~ X7FF
Output relay            Y               Bit                 Hexadecimal         Y000 ~ Y7FF
Internal relay          M               Bit                 Decimal             M0 ~ M8191, Octal for FXCPU
Latch relay             L               Bit                 Decimal             2
Annunciator             F               Bit                 Decimal             F0 ~ F1023
Edge relay              V               Bit                 Decimal             —
Link relay              B               Bit                 Hexadecimal             —
Data register           D               Word                Decimal             —
File register           R               Word                Decimal             —
File register           ZR              Word                Decimal             —
Link register           W               Word                Hexadecimal         —
--------------------------------------------------------------------------------------------------
       |Contact         TS              Bit                 Decimal             —
TIMER  |Coil            TC              Bit                 Decimal             —
       |Present value   TN              Word                Decimal             —
--------------------------------------------------------------------------------------------------
       |Contact         CS              Bit                 Decimal             —
COUNTER|Coil            CC              Bit                 Decimal             — Counter
       |Present value   CN              Word                Decimal             For FXCPU, 200 or more is 32-bit data
--------------------------------------------------------------------------------------------------
 * 
 * 
        ReadDeviceRandom사용시 B0라고 넣으면 해당bit 값만 읽기 가능 M,B,L,X,Y
        ReadDeviceRandom사용시 K8B0라고 넣으면 B0~B31까지 32BIT 읽기가 가능 M,B,L,X,Y(K8은 상수, B0시작주소 맘데로 변경가능)
 * 
 *      K * 4 로 해석해야 함... : K8 이면 4*8=32 bit, K7=4*7=28 points, K1 = 4points
 *      3.2.3 Data type @ MELSEC Q Series data book.pdf
 *      3.2.3 Accessable devices @ MELSEC Q Series data book.pdf
 *          Input : X000 ~ X7FF
 *          Output : Y000 ~ Y7FF
 *          Internal Relay : M0 ~ M8191
 *          .....
 */

    // Enumeration that defines the type of PLC device
    public enum PlcDeviceType
    {
        // Device for PLC
        M = 0x90
        , SM = 0x91
        , L  = 0x92
        , F  = 0x93
        , V  = 0x94
        , S  = 0x98
        , X  = 0x9C
        , Y  = 0x9D
        , B  = 0xA0
        , SB = 0xA1
        , DX = 0xA2
        , DY = 0xA3
        , D  = 0xA8
        , SD = 0xA9
        , R  = 0xAF
        , ZR = 0xB0
        , W  = 0xB4
        , SW = 0xB5
        , TC = 0xC0
        , TS = 0xC1
        , TN = 0xC2
        , CC = 0xC3
        , CS = 0xC4
        , CN = 0xC5
        , SC = 0xC6
        , SS = 0xC7
        , SN = 0xC8
        , Z  = 0xCC
        , TT
        , TM
        , CT
        , CM
        , A
        , Max
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    // Define a common interface for connection to a PLC
    public interface PlcMX : IDisposable 
    {
        int Open();
        int Close();
        int SetBitDevice(string iDeviceName, int iSize, int[] iData);
        int SetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        int GetBitDevice(string iDeviceName, int iSize, int[] oData);
        int GetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        int WriteDeviceBlock(string iDeviceName, int iSize, int[] iData);
        int WriteDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        int ReadDeviceBlock(string iDeviceName, int iSize, int[] oData);
        int ReadDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        int SetDevice(string iDeviceName, int iData);
        int SetDevice(PlcDeviceType iType, int iAddress, int iData);
        int GetDevice(string iDeviceName, out int oData);
        int GetDevice(PlcDeviceType iType, int iAddress, out int oData);
    }
    // ########################################################################################

    // ########################################################################################
    internal class McProtocolTcp : McProtocolApp
    {
        // ====================================================================================
        // constructor
        public McProtocolTcp() : this("", 0) { }
        public McProtocolTcp(string iHostName, int iPortNumber)
            : base(iHostName, iPortNumber)
        {
            Client = new TcpClient();
        }

        // &&&&& protected &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        override protected void DoConnect()
        {
            TcpClient c = Client;
            if (!c.Connected)
            {
                // Implementation of Keep Alive function
                var ka = new List<byte>(sizeof(uint) * 3);
                ka.AddRange(BitConverter.GetBytes(1u));
                ka.AddRange(BitConverter.GetBytes(45000u));
                ka.AddRange(BitConverter.GetBytes(5000u));
                c.Client.IOControl(IOControlCode.KeepAliveValues, ka.ToArray(), null);
                c.Connect(HostName, PortNumber);
                Stream = c.GetStream();
            }
        }
        // ====================================================================================
        override protected void DoDisconnect()
        {
            TcpClient c = Client;
            if (c.Connected)
            {
                c.Close();
            }
        }
        // ================================================================================
        override protected byte[] Execute(byte[] iCommand)
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
        // &&&&& private &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        private TcpClient Client { get; set; }
        private NetworkStream Stream { get; set; }
    }
    // ########################################################################################
    internal class McProtocolUdp : McProtocolApp
    {
        // ====================================================================================
        // constructor
        public McProtocolUdp(int iPortNumber) : this("", iPortNumber) { }
        public McProtocolUdp(string iHostName, int iPortNumber)
            : base(iHostName, iPortNumber)
        {
            Client = new UdpClient(); //Client Port는 PC 내부 자동설정 : 사용중 포트 충돌발생
        }

        // &&&&& protected &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        override protected void DoConnect()
        {
            UdpClient c = Client;
            c.Connect(HostName, PortNumber);
        }
        // ====================================================================================
        override protected void DoDisconnect()
        {
            UdpClient c = Client;
            c.Close();
        }
        // ================================================================================
        override protected byte[] Execute(byte[] iCommand)
        {
            UdpClient c = Client;
            c.Client.ReceiveTimeout = TimeOut;
            // Transmission
            c.Send(iCommand, iCommand.Length);

            using (var ms = new MemoryStream())
            {
                IPAddress ip = IPAddress.Parse(HostName);
                var ep = new IPEndPoint(ip, PortNumber);
                do
                {
                    // Reception
                    byte[] buff = c.Receive(ref ep);
                    ms.Write(buff, 0, buff.Length);
                } while (0 < c.Available);
                return ms.ToArray();
            }
        }
        // &&&&& private &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        private UdpClient Client { get; set; }
    }
}