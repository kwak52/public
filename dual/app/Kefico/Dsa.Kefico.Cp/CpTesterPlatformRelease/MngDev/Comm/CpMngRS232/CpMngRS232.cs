using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using System;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpMngLib.Manager
{
    /// <summary>
    /// Example to represent how to build a communication device manager.
    /// A Communication Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
    /// The named dll for a communication device is bound in run-time environment as well.    
    /// Communication Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
    /// </summary>
    public class CpMngRS232 : CpDeviceManagerBase, IRS232Manager
	{
		public CpMngRS232(bool activeHw)
			: base(activeHw)
		{
		}

		public bool IsCreated { private set; get; }	= false;
		public bool IsOpened { private set; get; }	= false;
		public bool IsClosed { private set; get; }	= false;
		public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;

		public bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
		{
			throw new NotImplementedException();
		}

		public bool OpenDevice()
		{
			IRS232 devRS232 = DeviceInstance as IRS232;
			ClsRS232Info infoRS232 = DeviceInfo as ClsRS232Info;
			
			if (devRS232 == null || infoRS232 == null)
				return false;

			FuncEvtHndl = new CpFunctionEventHandler();
			
			((IDevice) devRS232).FuncEvtHndl = FuncEvtHndl;
			((IDevice) devRS232).DeviceID = DeviceInfo.Device_ID;
						
			devRS232.PortName = infoRS232.HwName;
			devRS232.BaudRate = infoRS232.BAUDRATE;
			devRS232.DataBits = infoRS232.DATABITS;
			devRS232.UseCarrigeReturn = infoRS232.CR;
			devRS232.UseLineFeed = infoRS232.LF;

			IsOpened = devRS232.DevOpen();

			return IsOpened;
		}

		public bool CloseDevice()
		{
            if (IsClosed) return true;
			IRS232 devRS232 = DeviceInstance as IRS232;

			IsClosed = devRS232.DevClose();

			return IsClosed;
		}

		public bool	CreateDevice(ClsDeviceInfoBase info)
		{
			var oResult = TryFunc(() =>
			{
				IsCreated = CreateInstanceFromDll(info.DllName);

				if(!IsCreated)
					return IsCreated;

				DeviceInfo = info;

				return IsCreated;
			});

			if (oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create RS232.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}

		public bool InitManager()
		{
			return true;
		}

		public bool ResetDevice()
		{
			return true;
		}

		public bool WriteData(string strMsg)
		{
			IRS232 devRS232 = DeviceInstance as IRS232;

			return  devRS232.WriteData(strMsg);
		}
		public string QueryData(string strMsg)
		{
			IRS232 devRS232 = DeviceInstance as IRS232;
			string strResult = string.Empty;

			if (devRS232.WriteData(strMsg) == false)
				return strResult;

			strResult = ReadData();

			return strResult;
		}
		public string ReadData()
		{
			IRS232 devRS232 = DeviceInstance as IRS232;
			string strResult = string.Empty;

			if(!devRS232.ReadData(ref strResult))
				Console.WriteLine("RS232 Comm Error");

			return strResult;
		}
	}
}
