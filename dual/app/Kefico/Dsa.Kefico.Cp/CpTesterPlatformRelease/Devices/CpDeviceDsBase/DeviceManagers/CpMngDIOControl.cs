using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpDevices;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepDev.Interface;
using System;
using System.Collections.Generic;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpMngLib.Manager
{
    public class CpMngDIOControl : CpDeviceManagerDsBase, IDIOControlMananger
    {
        private CpDevDIOControl_PAIXCtrl _dioControl => DeviceInstance as CpDevDIOControl_PAIXCtrl;
        public CpDevDIOControl_PAIXCtrl DioControl => _dioControl;
        public CpMngDIOControl(bool activeHw) : base(activeHw)
        {
        }

        public override bool CreateDevice(ClsDeviceInfoBase info)
        {
            var oResult = TryFunc(() =>
            {
                if (info.DllName == "CpDevDIOControl_PAIXCtrl")
                {
                    DeviceInstance = new CpDevDIOControl_PAIXCtrl();
                    IsCreated = true;
                    DeviceInfo = info;
                    return true;
                }

                return false;
            });

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Create CpMngDIOControl.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            return oResult.Result;
        }

        public override bool OpenDevice()
        {
            ClsDIOControlInfo infoDIO = DeviceInfo as ClsDIOControlInfo;

            if (_dioControl == null || infoDIO == null)
                return false;

            FuncEvtHndl = new CpFunctionEventHandler();

            _dioControl.FuncEvtHndl = FuncEvtHndl;
            _dioControl.DeviceID = DeviceInfo.Device_ID;
            _dioControl.CONTROLLER_ADDRESS = DeviceInfo.HwName;
            _dioControl.DIGITAL_INPUT_POINT = infoDIO.DIGITAL_INPUT_POINT;
            _dioControl.DIGITAL_OUTPUT_POINT = infoDIO.DIGITAL_OUTPUT_POINT;

            IsOpened = _dioControl.DevOpen();
            IsClosed = !IsOpened;

            if (IsOpened == false)
                DeviceInstance = null;

            return IsOpened;
        }

        public override bool ResetDevice()
        {
            if (!IsOpened) return false; //DIO는 별도의 Station 별로 초기화 처리 ResetDeviceBit
            return true;
        }

        public bool ResetDeviceBit(int bit, bool on  = false)
        {
            if (!IsOpened) return false;

            _dioControl.SetDOutState(bit, on);

            return true;
        }

        public delegate void evtDIOStateChange(bool bCurState);
        public event evtDIOStateChange OnDIOStateChange;

        public void NotifyDIOStateChange(bool bCurState)
        {
            OnDIOStateChange?.Invoke(bCurState);
        }

        public void SetDOutState(int nPointIdx, bool bState)
        {
            _dioControl.SetDOutState(nPointIdx, bState);
        }

        public bool GetDOutState(int nPointIdx)
        {
            return _dioControl.GetDOutState(nPointIdx);
        }

        public bool GetDInState(int nPointIdx)
        {
            return _dioControl.GetDInState(nPointIdx);
        }

        public List<bool> GetDInState()
        {
            return _dioControl.CURRENT_DI_STATE;
        }

        public List<bool> GetDOutState()
        {
            return _dioControl.CURRENT_DO_STATE;
        }

        public void OnReceiveAlarm(object AbstractedData)
        {            
            //NotifyDIOStateChange(Convert.ToBoolean(AbstractedData));
        }

    }
}
