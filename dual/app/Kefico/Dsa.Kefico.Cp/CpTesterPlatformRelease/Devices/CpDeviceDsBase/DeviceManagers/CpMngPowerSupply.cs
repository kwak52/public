using System;
using System.Collections.Generic;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using static CpBase.CpLog4netLogging;
using CpTesterPlatform.CpDevices;

namespace CpTesterPlatform.CpMngLib.Manager
{
	/// <summary>
	/// Example to represent how to build a device manager.
	/// A Device Manager constructs a device in run-time environment using a defined dll name in the configuration xml.
	/// The named dll for a device is bound in run-time environment as well.    
	/// Device manager is an inherited class from a CpDeviceManagerBase and a corresponding interface.
	/// </summary>
	public class CpMngPowerSupply : CpDeviceManagerDsBase, IPowerSupplyManager
	{
		public CpMngPowerSupply(bool activeHw) : base(activeHw)
		{
		}


        protected override IDevice CreateDeviceInstance(string dllHint)
        {
            LogInfo($"+ Creating device instance from {dllHint}.");
            switch (dllHint)
            {
                case "CpDevPowerSupply_Sorensen":
                    LogInfo("Creating device CpDevPowerSupply_Sorensen.");
                    return new CpDevPowerSupply_Sorensen();
                default:
                    throw new NotImplementedException($"Unknown device type{dllHint}.");
            }
        }

        /// <summary>
        /// Device Initialization
        /// </summary>
        /// <returns></returns>
        public override bool OpenDevice()
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
