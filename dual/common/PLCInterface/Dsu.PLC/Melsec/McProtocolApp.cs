using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dsu.Common.Utilities;
using Dsu.PLC.Melsec;

namespace Dsu.PLC.Melsec
{
    public abstract partial class McProtocolApp : PlcMX
    {
        private object locker = new object();
        // ====================================================================================
        public McFrame CommandFrame { get; set; } // use frame
        public string HostName { get; set; } // host name or IP address
        public int PortNumber { get; set; } // port number
        public int TimeOut { get; set; } = 2000; // Receive Timeout millisecond
        // ====================================================================================
        // Constructor
        protected McProtocolApp(string iHostName, int iPortNumber)
        {
            CommandFrame = McFrame.MC3E;
            HostName = iHostName;
            PortNumber = iPortNumber;
        }

        // ====================================================================================
        // Post-processing
        public void Dispose()
        {
            Close();
        }

        // ====================================================================================
        public int Open()
        {
            DoConnect();
            Command = new McCommand(CommandFrame, this);
            Cpu = new MxCpu(this);
            _isQL_series = Cpu.Model != null ? Cpu.Model.StartsWith("Q") || Cpu.Model.StartsWith("L") : true;
            return 0;
        }
        // ====================================================================================
        public int Close()
        {
            DoDisconnect();
            return 0;
        }
        // ====================================================================================
        public int SetBitDevice(string iDeviceName, int iSize, int[] iData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return SetBitDevice(type, addr, iSize, iData);
        }
        // ====================================================================================
        public int SetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] iData)
        {
            lock (locker)
            {
                var type = iType;
                var addr = iAddress;
                var data = new List<byte>(6)
                {
                    (byte) addr
                    , (byte) (addr >> 8)
                    , (byte) (addr >> 16)
                    , (byte) type
                    , (byte) iSize
                    , (byte) (iSize >> 8)
                };
                var d = (byte)iData[0];
                var i = 0;
                while (i < iData.Length)
                {
                    if (i % 2 == 0)
                    {
                        d = (byte)iData[i];
                        d <<= 4;
                    }
                    else
                    {
                        d |= (byte)(iData[i] & 0x01);
                        data.Add(d);
                    }
                    ++i;
                }
                if (i % 2 != 0)
                {
                    data.Add(d);
                }
                byte[] sdCommand = Command.SetCommand(DeviceAccessCommand.BatchWrite, DeviceAccessType.Bit, data.ToArray());
                byte[] rtResponse = TryExecution(sdCommand);
                int rtCode = Command.SetResponse(rtResponse);
                return rtCode;
            }
        }
        // ====================================================================================
        public int GetBitDevice(string iDeviceName, int iSize, int[] oData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return GetBitDevice(type, addr, iSize, oData);
        }
        // ====================================================================================
        public int GetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] oData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
            {
                (byte) addr
                , (byte) (addr >> 8)
                , (byte) (addr >> 16)
                , (byte) type
                , (byte) iSize
                , (byte) (iSize >> 8)
            };
            byte[] sdCommand = Command.SetCommand(DeviceAccessCommand.BatchRead, DeviceAccessType.Bit, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            byte[] rtData = Command.Response;
            for (int i = 0; i < iSize; ++i)
            {
                if (i % 2 == 0)
                {
                    oData[i] = (rtCode == 0) ? ((rtData[i / 2] >> 4) & 0x01) : 0;
                }
                else
                {
                    oData[i] = (rtCode == 0) ? (rtData[i / 2] & 0x01) : 0;
                }
            }
            return rtCode;
        }
        // ====================================================================================
        public int WriteDeviceBlock(string iDeviceName, int iSize, int[] iData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return WriteDeviceBlock(type, addr, iSize, iData);
        }
        // ====================================================================================
        public int WriteDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] iData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
            {
                (byte) addr
                , (byte) (addr >> 8)
                , (byte) (addr >> 16)
                , (byte) type
                , (byte) iSize
                , (byte) (iSize >> 8)
            };
            foreach (int t in iData)
            {
                data.Add((byte)t);
                data.Add((byte)(t >> 8));
            }
            byte[] sdCommand = Command.SetCommand(DeviceAccessCommand.BatchWrite, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            return rtCode;
        }
        // ====================================================================================
        public int ReadDeviceBlock(string iDeviceName, int iSize, int[] oData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return ReadDeviceBlock(type, addr, iSize, oData);
        }
        // ====================================================================================
        public int ReadDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] oData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
            {
                (byte) addr
                , (byte) (addr >> 8)
                , (byte) (addr >> 16)
                , (byte) type
                , (byte) iSize
                , (byte) (iSize >> 8)
            };
            byte[] sdCommand = Command.SetCommand(DeviceAccessCommand.BatchRead, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            byte[] rtData = Command.Response;
            for (int i = 0; i < iSize; ++i)
            {
                oData[i] = (rtCode == 0) ? BitConverter.ToInt16(rtData, i * 2) : 0;
            }
            return rtCode;
        }
        // ====================================================================================
        public int SetDevice(string iDeviceName, int iData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return SetDevice(type, addr, iData);
        }
        // ====================================================================================
        public int SetDevice(PlcDeviceType iType, int iAddress, int iData)
        {
            lock (locker)
            {
                PlcDeviceType type = iType;
                int addr = iAddress;
                var data = new List<byte>(8)
                {
                    (byte) addr
                    , (byte) (addr >> 8)
                    , (byte) (addr >> 16)
                    , (byte) type
                    , 0x01
                    , 0x00
                    , (byte) iData
                    , (byte) (iData >> 8)
                };
                byte[] sdCommand = Command.SetCommand(DeviceAccessCommand.BatchWrite, data.ToArray());
                byte[] rtResponse = TryExecution(sdCommand);
                int rtCode = Command.SetResponse(rtResponse);
                return rtCode;
            }
        }
        // ====================================================================================
        public int GetDevice(string iDeviceName, out int oData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return GetDevice(type, addr, out oData);
        }
        // ====================================================================================
        public int GetDevice(PlcDeviceType iType, int iAddress, out int oData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
            {
                (byte) addr
                , (byte) (addr >> 8)
                , (byte) (addr >> 16)
                , (byte) type
                , 0x01
                , 0x00
            };
            byte[] sdCommand = Command.SetCommand(DeviceAccessCommand.BatchRead, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            if (0 < rtCode)
            {
                oData = 0;
            }
            else
            {
                byte[] rtData = Command.Response;
                oData = BitConverter.ToInt16(rtData, 0);
            }
            return rtCode;
        }
        // ====================================================================================
        //public int GetCpuType(out string oCpuName, out int oCpuType)
        //{
        //    int rtCode = Command.Execute(0x0101, 0x0000, new byte[0]);
        //    oCpuName = "dummy";
        //    oCpuType = 0;
        //    return rtCode;
        //}
        // ====================================================================================
        public static PlcDeviceType GetDeviceType(string s)
        {
            PlcDeviceType type = s.GetEnumValue<PlcDeviceType>();
            return Enum.IsDefined(typeof(PlcDeviceType), type) ? type : PlcDeviceType.Max;
        }

	    /// <summary>
	    /// Bit devices
	    /// </summary>
	    public static bool IsBitDevice(PlcDeviceType type) => !IsWordDevice(type);

		/// <summary>
		/// Word devices
		/// </summary>
		public static bool IsWordDevice(PlcDeviceType type)
	    {
			return (type == PlcDeviceType.D)
					 || (type == PlcDeviceType.SD)
					 || (type == PlcDeviceType.Z)
					 || (type == PlcDeviceType.ZR)
					 || (type == PlcDeviceType.R)
					 || (type == PlcDeviceType.W)

					 /* added by kwak */
					 || (type == PlcDeviceType.TN)
					 || (type == PlcDeviceType.SN)
					 || (type == PlcDeviceType.CN)
					 || (type == PlcDeviceType.SW)
					 ;
		}

		// ====================================================================================
		/// <summary>
		/// Hexadecimal device number range
		/// Address 증가가 십진이 아닌 십육진으로 구성되는 devices
		/// </summary>
		public static bool IsHexDevice(PlcDeviceType type)
        {
            return (type == PlcDeviceType.X)
                   || (type == PlcDeviceType.Y)
                   || (type == PlcDeviceType.B)
                   || (type == PlcDeviceType.W)

				   /* added by kwak */
				   || (type == PlcDeviceType.DX)
				   || (type == PlcDeviceType.DY)
				   || (type == PlcDeviceType.SB)
				   || (type == PlcDeviceType.SW)
				   //|| (type == PlcDeviceType.ZR)  //ZR은 메뉴얼에는 hex 이지만 실사용은 decimal device
				   ;
		}

        // ====================================================================================
        public static void GetDeviceCode(string iDeviceName, out PlcDeviceType oType, out int oAddress)
        {
            string s = iDeviceName.ToUpper();
            string strAddress;

            // Take out character
            string strType = s.Substring(0, 1);
	        int addressStartIndex = 1;
            switch (strType)
            {
                case "A":
                case "B":
                case "F":
                case "L":
                case "M":
                case "R":
                case "V":
                case "W":
                case "X":
                case "Y":
                    // Second and subsequent characters to convert because the supposed numbers
                    strAddress = s.Substring(1);
                    break;

				case "D":
		            if (s[1] == 'X' || s[1] == 'Y')
			            addressStartIndex = 2;
					strType = s.Substring(0, addressStartIndex);
					strAddress = s.Substring(addressStartIndex);
					break;

				case "Z":
                    // Take out more character
                    // If the file register: 2
                    // If the index register: 1
                    if (s[1] == 'R')
                        addressStartIndex = 2;
                    strType = s.Substring(0, addressStartIndex);
                    strAddress = s.Substring(addressStartIndex);
                    break;
                case "T":
                case "C":
                    if (s[1] == 'C' || s[1] == 'M' || s[1] == 'N' || s[1] == 'S' || s[1] == 'T')
                    {
                        addressStartIndex = 2;
                        strType = s.Substring(0, addressStartIndex);
                    }
                    else
                        strType = s.Substring(0, addressStartIndex) + 'N';  //T001,C001 형태는 TN, CN으로 취득
                    strAddress = s.Substring(addressStartIndex);
                    break;
                case "S":
                    // Take out more character
                    strType = s.Substring(0, 2);
                    switch (strType)
                    {
                        case "SD":
                        case "SM":

						case "SS":
						case "SC":
						case "SN":
						case "SB":
						case "SW":
						case "ST":
							strAddress = s.Substring(2);
                            break;
                        default:
                            throw new Exception("Invalid format.");
                    }
                    break;
              
                default:
                    throw new Exception("Invalid format.");
            }

            oType = GetDeviceType(strType);
            oAddress = IsHexDevice(oType) ? Convert.ToInt32(strAddress, BlockSize) :
                Convert.ToInt32(strAddress);
        }
        // &&&&& protected &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        abstract protected void DoConnect();
        abstract protected void DoDisconnect();
        abstract protected byte[] Execute(byte[] iCommand);
        // &&&&& private &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        private const int BlockSize = 0x0010;
        private McCommand Command { get; set; }
        // ================================================================================
        private byte[] TryExecution(byte[] iCommand)
        {
            byte[] rtResponse;
            int tCount = 10;
            do
            {
                rtResponse = Execute(iCommand);
                --tCount;
                if (tCount < 0)
                {
                    throw new Exception("You can not get the correct value from the PLC.");
                }
            } while (Command.IsIncorrectResponse(rtResponse));
            return rtResponse;
        }
        // ####################################################################################
        // Inner class that represents the command to be used for communication
        class McCommand
        {
            private McProtocolApp McProtocol { get; }       // parent connection
            private McFrame FrameType { get; set; }         // frame type
            private uint SerialNumber { get; set; }         // serial number
            private uint NetwrokNumber { get; set; }        // network number
            private uint PcNumber { get; set; }             // PC number
            private uint IoNumber { get; set; }             // request destination unit I / O number
            private uint ChannelNumber { get; set; }        // request destination unit station number
            private uint CpuTimer { get; set; }             // CPU monitoring timer
            private int ResultCode { get; set; }            // End code
            public byte[] Response { get; private set; }    // response data
            // ================================================================================
            // constructor
            public McCommand(McFrame iFrame, McProtocolApp mcProtocol)
            {
                FrameType = iFrame;
                SerialNumber = 0x0001u;
                NetwrokNumber = 0x0000u;
                PcNumber = 0x00FFu;
                IoNumber = 0x03FFu;
                ChannelNumber = 0x0000u;
                CpuTimer = 0x0010u;
                McProtocol = mcProtocol;
            }


            public byte[] SetCommand(DeviceAccessCommand command, byte[] iData)
            {
                uint iSubCommand = McProtocol.GetSubCommand(command);
                return SetCommand(command, iSubCommand, iData);
            }

            public byte[] SetCommand(DeviceAccessCommand command, DeviceAccessType accessType, byte[] iData)
            {
                uint iSubCommand = McProtocol.GetSubCommandWithType(command, accessType);
                return SetCommand(command, iSubCommand, iData);

            }
            // ================================================================================
            private byte[] SetCommand(DeviceAccessCommand command, uint iSubCommand, byte[] iData)
            {
                uint iMainCommand = (uint) command;
                var dataLength = (uint)(iData.Length + 6);
                var ret = new List<byte>(iData.Length + 20);
                uint frame = (FrameType == McFrame.MC3E) ? 0x0050u :
                    (FrameType == McFrame.MC4E) ? 0x0054u : 0x0000u;
                ret.Add((byte)frame);
                ret.Add((byte)(frame >> 8));
                if (FrameType == McFrame.MC4E)
                {
                    ret.Add((byte)SerialNumber);
                    ret.Add((byte)(SerialNumber >> 8));
                    ret.Add(0x00);
                    ret.Add(0x00);
                }
                ret.Add((byte)NetwrokNumber);
                ret.Add((byte)PcNumber);
                ret.Add((byte)IoNumber);
                ret.Add((byte)(IoNumber >> 8));
                ret.Add((byte)ChannelNumber);
                ret.Add((byte)dataLength);
                ret.Add((byte)(dataLength >> 8));
                ret.Add((byte)CpuTimer);
                ret.Add((byte)(CpuTimer >> 8));
                ret.Add((byte)iMainCommand);
                ret.Add((byte)(iMainCommand >> 8));
                ret.Add((byte)iSubCommand);
                ret.Add((byte)(iSubCommand >> 8));
                ret.AddRange(iData);
                return ret.ToArray();
            }



            /// <summary>
            /// see Q Corresponding MELSEC Communication....pdf, pp. 3-7.
            /// </summary>
            /// <param name="iResponse"></param>
            /// <returns></returns>
            public int SetResponse(byte[] iResponse)
            {
                int min = (FrameType == McFrame.MC3E) ? 11 : 15;
                if (min <= iResponse.Length)
                {
                    var btCount = new[] { iResponse[min - 4], iResponse[min - 3] };
                    var btCode = new[] { iResponse[min - 2], iResponse[min - 1] };
                    int rsCount = BitConverter.ToUInt16(btCount, 0);
                    ResultCode = BitConverter.ToUInt16(btCode, 0);
                    Response = new byte[rsCount - 2];
                    Buffer.BlockCopy(iResponse, min, Response, 0, Response.Length);
                }
                return ResultCode;
            }
            // ================================================================================
            public bool IsIncorrectResponse(byte[] iResponse)
            {
                var min = (FrameType == McFrame.MC3E) ? 11 : 15;
                var btCount = new[] { iResponse[min - 4], iResponse[min - 3] };
                var btCode  = new[] { iResponse[min - 2], iResponse[min - 1] };
                var rsCount = BitConverter.ToUInt16(btCount, 0) - 2;
                var rsCode  = BitConverter.ToUInt16(btCode, 0);
                return (rsCode == 0 && rsCount != (iResponse.Length - min));
            }
        }
    }
}