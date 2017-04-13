using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using System;
using static CpCommon.ExceptionHandler;
using static CpBase.CpLog4netLogging;
using CpTesterPlatform.CpDevices;


//namespace CpDeviceDsBase
namespace CpTesterPlatform.CpMngLib.Manager
{
    public abstract class CpDeviceManagerDsBase : CpDeviceManagerBase
    {
        protected CpDeviceManagerDsBase(bool activeHw) : base(activeHw)        {}
        public bool IsCreated { protected set; get; } = false;
        public bool IsOpened { protected set; get; } = false;
        public bool IsClosed { protected set; get; } = false;
        public DeviceControlState ControlState { private set; get; } = DeviceControlState.Normal;

        public virtual bool CloseDevice()
        {
            if (!IsOpened) return true;

            if (DeviceInstance == null)
                return true;

            IsClosed = DeviceInstance.DevClose();
            IsOpened = !IsClosed;

            return IsClosed;
        }

        protected virtual IDevice CreateDeviceInstance(string dllHint)
        {
            LogInfo($"+ Creating device instance from {dllHint}.");
            if (CreateInstanceFromDll(dllHint))
                return DeviceInstance;

            return null;
        }

        public virtual bool CreateDevice(ClsDeviceInfoBase info)
        {
            var oResult = TryFunc(() =>
            {
                DeviceInstance = CreateDeviceInstance(info.DllName);
                if ( DeviceInstance != null )
                {
                    DeviceInfo = info;
                    IsCreated = true;
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



        public virtual bool CreateVirtualDevice(ClsDeviceInfoBase info, CpDeviceManagerBase devComm)
        {
            var oResult = TryFunc(() =>
            {
                if (info.VirtualDevice)
                {
                    switch(info.DllName)
                    {
                        case "CpVDevAI_NIDAQ":
                            DeviceInstance = new CpVDevAI_NIDAQ();
                            break;
                        default:
                            DeviceInstance = CreateDeviceInstance(info.DllName);
                            break;
                    }

                    IsCreated = DeviceInstance != null;
                    if (!IsCreated)
                        return false;

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
                UtilTextMessageEdits.UtilTextMsgToConsole($"Failed to Create {info.DllName}.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            return oResult.Result;
        }

        public virtual bool ResetDevice()
        {
            if (!IsOpened) return false;
            if (DeviceInstance == null)
                return true;

            return DeviceInstance.DevReset();
        }

        public virtual bool OpenDevice()
        {
            if (DeviceInstance == null)
                return false;

            IsOpened = DeviceInstance.DevOpen();
            IsClosed = !IsOpened;
            return IsOpened;
        }



        public virtual bool InitManager()
        {
            return true;
        }

    }
}
