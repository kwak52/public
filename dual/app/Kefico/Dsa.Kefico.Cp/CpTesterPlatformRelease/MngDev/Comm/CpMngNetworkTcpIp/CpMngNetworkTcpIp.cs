using System;
using System.Collections.Generic;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpMngLib.Manager
{
    /// <summary>
    /// Example to represent how to build a communication device manager.
    /// A Communication Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
    /// The named dll for a communication device is bound in run-time environment as well.    
    /// Communication Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
    /// </summary>
    public class CpMngNetworkTcpIp : CpDeviceManagerBase, INetworkTcpIpManager
	{
		public CpMngNetworkTcpIp(bool activeHw) : base(activeHw)
		{
		}

		private int ServerPort { set; get; } = 0;

		public bool IsCreated { private set; get; }	= false;
		public bool IsOpened { private set; get; }	= false;
		public bool IsClosed { private set; get; }	= false;
		public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;

		public List<string> ReceivedMessageList { set; get; } = new List<string>();
		bool IsReceived = false;

		public bool CloseDevice()
		{
            if (IsClosed) return true;
            INetworkTcpIp devTcp = DeviceInstance as INetworkTcpIp;
			IsClosed = devTcp?.DevClose() ?? false;
			return IsClosed;
		}

		public bool CreateDevice(ClsDeviceInfoBase info)
		{
			var oResult = TryFunc(() =>
			{
				IsCreated = CreateInstanceFromDll(info.DllName);

				DeviceInfo = info;

				return IsCreated;
			});

			if (oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create TCP/IP Sender.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}

		public bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
		{
			throw new NotImplementedException();
		}

		public bool InitManager()
		{
			INetworkTcpIp devTcp = DeviceInstance as INetworkTcpIp;
			return devTcp?.DevInit() ?? false;
		}

		public bool OpenDevice()
		{
			INetworkTcpIp devTcp = DeviceInstance as INetworkTcpIp;
			ClsNetworkTcpIpInfo infoTcp = DeviceInfo as ClsNetworkTcpIpInfo;

			if (devTcp == null || devTcp == null)
			{
				return false;
			}
			FuncEvtHndl = new CpFunctionEventHandler();

			FuncEvtHndl.OnTcpIpReceive += new CpFunctionEventHandler.evtTcpIpReceiveHandler(OnReceiveMessage);

			devTcp.FuncEvtHndl = FuncEvtHndl;

			devTcp.InitTcpClient(infoTcp.Device_ID, infoTcp.IP_ADDRESS, infoTcp.PORT_NO, infoTcp.CR, infoTcp.LF);

			IsOpened = devTcp.DevOpen();

			return IsOpened;
		}
		
		public bool ResetDevice()
		{
            if (!IsOpened) return false;

            INetworkTcpIp devTcp = DeviceInstance as INetworkTcpIp;
			return devTcp?.DevReset() ?? false;
		}

		public void SendData(string writeData)
		{
			INetworkTcpIp devTcp = DeviceInstance as INetworkTcpIp;
			
			devTcp?.SendData(writeData);
		}

		public string SendQueryData(string writeData)
		{
			INetworkTcpIp devTcp = DeviceInstance as INetworkTcpIp;

			IsReceived = false;
			devTcp?.SendData(writeData);

			while(true)
			{
				if(IsReceived == true)
					break;
			}

			return GetLatestMessage();
		}

		public string GetLatestMessage()
		{
			return ReceivedMessageList.Count > 0 ? ReceivedMessageList[ReceivedMessageList.Count - 1] : string.Empty;
		}
		
		public void OnReceiveMessage(string strMsg)
		{
			ReceivedMessageList.Add(strMsg);

			IsReceived = true;
		}
				
	}
}
