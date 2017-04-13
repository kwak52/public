using System;
using System.Collections.Generic;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpMngLib.Manager
{
	/// <summary>
	/// Example to represent how to build a device manager.
	/// A Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
	/// The named dll for a device is bound in run-time environment as well.    
	/// Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
	/// </summary>
	public class CpMngPowerSupply : CpDeviceManagerBase, IPowerSupplyManager
	{
		public CpMngPowerSupply(bool activeHw) : base(activeHw)
		{
		}

		public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;
		public bool IsClosed { private set; get; } = false;
		public bool IsCreated { private set; get; } = false;
		public bool IsOpened { private set; get; } = false;
		
		public bool CloseDevice()
		{            
            if (IsClosed) return true;
			IPowerSupply devPowSupply = DeviceInstance as IPowerSupply;
			
			if (devPowSupply == null)
				return true;

            IsClosed = devPowSupply.DevClose();
            IsOpened = !IsClosed;


            return IsClosed;
        }



        /// <summary>
        /// If "Virtual = true"
        /// Device manager creates a device as virtual.
        /// Virtual device is built with communication device binding.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="devComm"></param>
        /// <returns></returns>
        public bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
		{
			var oResult = TryFunc(() =>
			{
				if (info.VirtualDevice)
				{
					IsCreated = CreateInstanceFromDll(info.DllName);

					if (!IsCreated)
						return IsCreated;

					DeviceInfo = info;
					IsCreated = true;
					IsOpened = true;

					((CpVitualDeviceManagerBase)DeviceInstance).SetCommDeviceMgr(devComm);

					return true;
				}

				return false;
			});

			if (oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create CpMngPowerSupply.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}

		/// <summary>
		/// If "Virtual = false"
		/// Device manager creates a device as normal.
		/// Normal device is built without communication device binding.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="devComm"></param>
		/// <returns></returns>
		public bool CreateDevice(ClsDeviceInfoBase info)
		{
			var oResult = TryFunc(() =>
			{
				if (!CreateInstanceFromDll(info.DllName))
					return false;

				DeviceInfo = info;

				return true;
			});

			if (oResult.HasException)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create CpMngPowerSupply.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			return oResult.Result;
		}
		
		public bool InitManager()
		{
			return true;
		}

        /// <summary>
        /// Device Initialization
        /// </summary>
        /// <returns></returns>
        public bool OpenDevice()
        {
            IPowerSupply devPowSupply = DeviceInstance as IPowerSupply;
            ClsPowerSupplyInfo infoPowSupply = DeviceInfo as ClsPowerSupplyInfo;

            if (devPowSupply == null || infoPowSupply == null)
                return false;

            FuncEvtHndl = new CpFunctionEventHandler();

            devPowSupply.FuncEvtHndl = FuncEvtHndl;
            devPowSupply.DeviceID = DeviceInfo.Device_ID;
            devPowSupply.ChannelID = infoPowSupply.CHANNEL;
            devPowSupply.CONTROLLER_ADDRESS = DeviceInfo.HwName;

            IsOpened = devPowSupply.DevOpen();
            IsClosed = !IsOpened;

            return IsOpened;
        }

		public bool ResetDevice()
		{            
            if (!IsOpened) return false;
            IPowerSupply devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return true;

			return devPowSupply.DevReset();
		}

		public bool SetOutput(bool enable)
		{
			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return false;

			return devPowSupply.SetOutput(enable);
		}

		public bool SetCurrent(double current)
		{
			ClsPowerSupplyInfo infoPowSupply = DeviceInfo as ClsPowerSupplyInfo;
			if (current > infoPowSupply.VOLTAGE_LIMIT)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("[Error CpMngPowerSupply]: Current exceeded the maximum current.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				return false;
			}

			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;
			
			if (devPowSupply == null)
				return false;

			return devPowSupply.SetCurrent(current);
		}
		
		public bool SetVoltage(double voltage)
		{
			ClsPowerSupplyInfo infoPowSupply = DeviceInfo as ClsPowerSupplyInfo;
			if (voltage > infoPowSupply.VOLTAGE_LIMIT)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("[Error CpMngPowerSupply]: Current exceeded the maximum voltage.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				return false;
			}

			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return false;

			return devPowSupply.SetVoltage(voltage);
		}

		public bool GetOutput()
		{
			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return false;

			return devPowSupply.GetOutput();
		}

		public double GetVoltage()
		{
			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return 0.0;

			return devPowSupply.GetVoltage();
		}

		public double GetCurrent()
		{
			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return 0.0;

			return devPowSupply.GetCurrent();
		}

		public double GetObservedVoltage()
		{
			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return 0.0;

			return devPowSupply.GetObservedVoltage();
		}

		public double GetObservedCurrent()
		{
			IPowerSupply devPowSupply = null;

			devPowSupply = DeviceInstance as IPowerSupply;

			if (devPowSupply == null)
				return 0.0;

			return devPowSupply.GetObservedCurrent();
		}
	}
}
