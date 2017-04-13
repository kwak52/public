using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpDevices
{
    /// <summary>
    /// Example to represent how to build a communication device.
    /// A Communication Device is constructed in run-time environment by a communication device manager.
    /// Communication Device is an inherited class from a corresponding interface.
    /// </summary>
    public class CpDevNetworkTCPIP : INetworkTcpIp    
	{		
		public CpFunctionEventHandler FuncEvtHndl { get; set; }
		public string DeviceID { get; set; }	

		public bool UseCarrigeReturn { set; get; } = false;
		public bool UseLineFeed { set; get; } = false; 

		public TcpClient TcpClient { set; get; } = null; 
		public IPAddress AddrIp  { set; get; } = null; 
		public int Port  { set; get; } = 0;      	
		
		bool Terminated { set; get; } = false;
		bool LookFinished { set; get; } = true;
		
		const char		CARRIAGE_RETURN		= '\r';
		const char		LINE_FEED			= '\n';
		
		public string GetModuleName()
		{
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
		}
					  
		public bool DevOpen()
		{
			var oResult = TryFunc(() =>
			{
				if (!ExePingTest())
					return false;

				TcpClient.Connect(AddrIp, Port);

				Task looker = new Task(() => LookReceivedBuffer());

				looker.Start();

				return true;
			});

			if(oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create TCP/IP Sender.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
				
				return false;
			}

			return oResult.Result;
		}
			  
		public bool DevClose()
		{
			var oResult = TryFunc(() =>
			{
				Terminated = true;

				while (true) { if (LookFinished == true) break; }

				TcpClient.Close();

				return true;
			});

			if (oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create TCP/IP Sender.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;           
		}
		public bool DevInit()
		{
			return true;
		}

		public bool DevReset()
		{
			return true;
		}

		public bool ExePingTest(bool bWriteToConsole = false)
		{
			var oResult = TryFunc(() =>
			{
				Ping pingSender = new Ping();
				PingOptions options = new PingOptions();                

				// Use the default Ttl value which is 128,
				// but change the fragmentation behavior.
				options.DontFragment = true;

				PingReply reply = pingSender.Send(AddrIp.ToString(), 1000);
				if (reply.Status == IPStatus.Success)
				{
					if (bWriteToConsole)
					{
						Console.WriteLine("Address: {0}", reply.Address.ToString());
						Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
						Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
						Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
						Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
					}
					return true;
				}
				else
					return false;
			});

			if (oResult.HasException)
			{				
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to ExePingTest TCP/IP Sender.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}

		public bool InitTcpClient(string deviceId, string deviceIp, int portNumber, bool bCR = false, bool bLF = false)
		{

			var oResult = TryFunc(() =>
			{
				IPAddress ipInput = new IPAddress(0);	

				if(!IPAddress.TryParse(deviceIp, out ipInput))
					return false;

				TcpClient = new TcpClient();	
				AddrIp = ipInput;			
				Port = portNumber;
				UseCarrigeReturn = bCR;
				UseLineFeed = bLF;

				return true;
			});

			if (oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Add TcpClient. ", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}

		public bool ReadData(ref string readData)
		{
			return true;
		}

		public bool ReadData(ref byte[] readBuffer)
		{
			return true;
		}

		public bool SendData(string writeData)
		{
			List<byte> abData = CvStr2ByteArray(writeData);

			if(UseCarrigeReturn == true)
				abData.Add(Convert.ToByte(CARRIAGE_RETURN));

			if(UseLineFeed == true)
				abData.Add(Convert.ToByte(LINE_FEED));
						
			SendData(abData.ToArray());

			return true;
		}
		
		public bool SendData(byte[] writeBuffer)
		{
			var oResult = TryFunc(() =>
			{
				TcpClient.GetStream().Write(writeBuffer, 0, writeBuffer.Length);

				return true;
			});

			if (oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Write Data at Sender ID: " + DeviceID, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}
		
		void LookReceivedBuffer()
		{
			string strResult = string.Empty;

			LookFinished = false;

			while(true)
			{
				if(TcpClient.GetStream().DataAvailable == true)
				{	
					byte [] cChecker = new byte[1];
					int nLength = TcpClient.GetStream().Read(cChecker, 0, 1);
														
					if(nLength == 0 || cChecker[0] == Convert.ToByte(CARRIAGE_RETURN) || cChecker[0] == Convert.ToByte(LINE_FEED))
					{
						if(strResult != string.Empty)
							FuncEvtHndl.DoTcpIpReceive(strResult);
						strResult = string.Empty;
					}
					else
						strResult += Convert.ToChar(cChecker[0]);
				}

				if(Terminated == true)
					break;
			}

			LookFinished = true;
		}
		
		public static List<byte> CvStr2ByteArray(string strData)
		{
			List<byte> abResult = new List<byte>();
			
			foreach(char ch in strData.ToCharArray())
				abResult.Add(Convert.ToByte(ch));
				
			return abResult;
		}
	}
}
