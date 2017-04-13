using System;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using static CpBase.CpLog4netLogging;
using CpTesterPlatform.CpTStepDev.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dsu.Driver.Util.Emergency;
using System.Threading;
using System.Linq;
using CpTesterPlatform.CpDevices;

namespace CpTesterPlatform.CpMngLib.Manager
{
    public class CpMngMotion : CpDeviceManagerDsBase, IMotionManager
    {
        public CpMngMotion(bool activeHw) : base(activeHw)
        {
        }

        private CpDevPaixMotion _paixMotion { get { return DeviceInstance as CpDevPaixMotion; } }
        private CpDevPaixMotor PaixMotor { get { return DeviceInstance as CpDevPaixMotor; } }
        public CpDevPaixRobot PaixRobot { get { return DeviceInstance as CpDevPaixRobot; } }

        public Dsu.Driver.Paix.Manager Paix => _paixMotion.Paix;


        protected override IDevice CreateDeviceInstance(string dllHint)
        {
            LogInfo($"+ Creating device instance from {dllHint}.");
            switch (dllHint)
            {
                case "CpDevMotion_PAIXCtrl":
                    return new CpDevPaixMotor();
                case "CpDevRobot_PAIXCtrl":
                    return new CpDevPaixRobot();
                default:
                    throw new NotImplementedException($"Unknown device type{dllHint}.");
            }
        }


        public override bool InitManager()
        {
            return true;
        }

        public override bool OpenDevice()
        {
            var devMotion = _paixMotion;
            ClsMotionInfo infoMotion = DeviceInfo as ClsMotionInfo;

            if (devMotion == null || infoMotion == null)
                return false;

            FuncEvtHndl = new CpFunctionEventHandler();

            devMotion.FuncEvtHndl = FuncEvtHndl;
            devMotion.DeviceID = DeviceInfo.Device_ID;
            devMotion.CONTROLLER_ADDRESS = DeviceInfo.HwName;
            devMotion.AXIS_ID = infoMotion.AXIS_ID;
            devMotion.AXIS_DIRECTION = infoMotion.AXIS_DIRECTION;
            devMotion.CFG_MAX_RPM_RER_SEC = infoMotion.CFG_MAX_RPM > 5000 ? 5000 : infoMotion.CFG_MAX_RPM;
            devMotion.CFG_ACC_RPM_RER_SEC = infoMotion.CFG_ACC_RPM_RER_SEC > 1000 ? 1000 : infoMotion.CFG_ACC_RPM_RER_SEC;
            devMotion.CFG_DEC_RPM_RER_SEC = infoMotion.CFG_DEC_RPM_RER_SEC > 1000 ? 1000 : infoMotion.CFG_DEC_RPM_RER_SEC;
            devMotion.CFG_DISTANCE_PER_REVOLUTION = infoMotion.CFG_DISTANCE_PER_REVOLUTION;

            IsOpened = devMotion.DevOpen();
            IsClosed = !IsOpened;
            return IsOpened;
        }

        public override bool ResetDevice()
        {
            if (!IsOpened) return false;

            _paixMotion.StopMotion(bEmergency: true);

            return true;
        }

        public Nullable<bool> IsMove(short axis)
        {
            if (!IsOpened) return false;
            return _paixMotion.IsMove(axis);
        }


        public void SetRpmSpeed(double dRpm, bool overrideSpeed)
        {
            if (!IsOpened) return;
            PaixMotor.SetRpmSpeed(dRpm, overrideSpeed);
        }

        public void SetJogMove()
        {
            if (!IsOpened) return;
            if (SignalManager.IsEmergency) return;            
            PaixMotor.SetJogMove();
        }


        /// <summary>
        /// CW = TRUE
        /// CCW = FALSE
        /// </summary>
        /// <param name="bCW"></param>
        public void SetDirection(bool bCW)
        {
            if (!IsOpened) return;
            PaixMotor.SetDirection(bCW);
        }

        public void SetRelMove(double dPos)
        {
            if (!IsOpened) return;
            if (SignalManager.IsEmergency) return;
            PaixMotor.SetRelMove(dPos);
        }

        public void SetRelMove(short axis, double dPos)
        {
            if (!IsOpened) return;
            if (SignalManager.IsEmergency) return;
            PaixRobot.SetRelMove(axis, dPos);
        }

        public void SetAbsMove(double dPos)
        {
            if (!IsOpened) return;
            if (SignalManager.IsEmergency) return;
            PaixMotor.SetAbsMove(dPos);
        }

        public void SetRevCountMove(double dRevCnt)
        {
            if (!IsOpened) return;
            if (SignalManager.IsEmergency) return;
            double dRelDistance = dRevCnt * 360;

            SetRelMove(dRelDistance);
        }

        public IEnumerable<short> GetAxesList()
        {
            if (!IsOpened) return Enumerable.Empty<short>();
            return _paixMotion.GetAxesList();
        }
        public int GetCmdPos(short axis)
        {
            if (!IsOpened) return 0;
            return _paixMotion.GetCmdPos(axis);
        }

        public int GetEncPos(short axis)
        {
            if (!IsOpened) return 0;
            return _paixMotion.GetEncPos(axis);
        }



        public int[] GetCmdPositions()
        {
            if (!IsOpened) return new int[] { };
            return PaixRobot.GetCmdPositions();
        }
        public int[] GetEncPositions()
        {
            if (!IsOpened) return new int[] { };
            return PaixRobot.GetEncPositions();
        }

        public void SetCommandEncorderZeroSetting(IEnumerable<short> axes)
        {
            if (!IsOpened) return;
            PaixRobot.SetCommandEncorderZeroSetting(axes);
        }

        public void SetCommandZeroSetting(IEnumerable<short> axes)
        {
            if (!IsOpened) return;
            PaixRobot.SetCommandZeroSetting(axes);
        }


        public void VerifiyCommandEncoderZero(IEnumerable<short> axes, int tolerance)
        {
            if (!IsOpened) return;
            PaixRobot.VerifiyCommandEncoderZero(axes, tolerance);
        }

        public bool IsCommandEncoderZero(IEnumerable<short> axes, int tolerance)
        {
            if (!IsOpened) return false;
            return PaixRobot.IsCommandEncoderZero(axes, tolerance);
        }



        public void SetModePath(List<string> paths)
        {
            if (!IsOpened) return;
            PaixRobot.SetModePath(paths);
        }

        public async Task<bool> MovePath(string pathName, CancellationToken cancelToken)
        {
            if (!IsOpened) return false;
            if (SignalManager.IsEmergency) return false;
            if (cancelToken.IsCancellationRequested) return false;

            return await PaixRobot.MovePath(pathName, cancelToken);
        }

        public async Task<bool> HomeMove(CancellationToken cancelToken, bool resetCmdEncPosition=true)
        {
            if (!IsOpened) return false;

            return await PaixRobot.HomeMove(cancelToken, resetCmdEncPosition);
        }

        public double GetCurrentPos()
        {
            if (!IsOpened) return 0.0;
            return PaixMotor.GetCurrentPos();
        }

        public double GetCommandPos()
        {
            if (!IsOpened) return 0.0;
            return PaixMotor.GetCommandPos();
        }

        public List<string> GetPath()
        {
            if (!IsOpened) return new List<string>();
            return PaixRobot.GetPath();
        }

        public double GetCurrentRpm()
        {
            if (!IsOpened) return 0.0;
            return PaixMotor.GetCurrentRpm();
        }

        public void StopMotion()
        {
            if (!IsOpened) return;
            _paixMotion.StopMotion(false);
        }

        public static explicit operator CpMngMotion(List<IDevManager> v)
        {
            throw new NotImplementedException();
        }

        public void StopMotionEmergency()
        {
            if (!IsOpened) return;
            _paixMotion.StopMotion(true);
        }

        public void ClearAlarm()
        {
            if (!IsOpened) return;

            _paixMotion.ClearAlarm();
        }

        /// returns true if alarm exists and it is cleared
        public bool ClearAlarmOnDemand()
        {
            if (!IsOpened) return false;

            return _paixMotion.ClearAlarmOnDemand();
        }

        public bool BreakServoChange(bool bOn)
        {
            if (!IsOpened) return false;

            return _paixMotion.BreakServoChange(bOn);
        }

        public void OpenManualDialog()
        {
            if (!IsOpened) return;
            PaixRobot.OpenManualDialog();
        }

        public void SetParametor(double MAX, double ACC, double DEC)
        {
            if (!IsOpened) return;

            PaixMotor.CFG_MAX_RPM_RER_SEC = MAX;
            PaixMotor.CFG_ACC_RPM_RER_SEC = ACC;
            PaixMotor.CFG_DEC_RPM_RER_SEC = DEC;

            PaixMotor.SetRpmSpeed(MAX, false);
        }

        public void SetRobotSpeed(short axis, double MAX, double ACC, double DEC, bool overrideSpeed = false)
        {
            if (!IsOpened) return;
            PaixRobot.SetRobotSpeed(axis, MAX, ACC, DEC);
        }
    }
}

