using CpTesterPlatform.CpTStepDev.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using System.Threading;
using Paix = Dsu.Driver.Paix;
using static Dsu.Driver.PaixMotionControler.NMC2;
using static CpCommon.ExceptionHandler;
using System.Reflection;
using Dsu.Common.Utilities.Core.FSharpInterOp;

namespace CpTesterPlatform.CpDevices
{
    public abstract class CpDevPaixMotion : CpDevNormalBase, IMotion
    {
        public string CONTROLLER_ADDRESS { set; get; } = "0.0.0.0";
        public string AXIS_ID { set; get; } = "0";
        public string AXIS_DIRECTION { set; get; } = "CW";
        // RPM per Sec  초당 회전 RMP가속도 (0 ~ 1000)  1000 이상 입력시 1000 할당
        public double CFG_ACC_RPM_RER_SEC { set; get; } = 10;
        // RPM per Sec  초당 회전 RMP감속도 (0 ~ 1000)  1000 이상 입력시 1000 할당
        public double CFG_DEC_RPM_RER_SEC { set; get; } = 10;
        public double CFG_MAX_RPM_RER_SEC { set; get; } = 3000;
        // 모터 1회 전시 필요한 Pulse
        public double CFG_PULSE_PER_REVOLUTION { get; set; } = 10000;
        // 모터 1회 전시 이동거리 mm 기어비
        public double CFG_DISTANCE_PER_REVOLUTION { get; set; } = 100;
        /// True = ROBOT, FALSE = WHEEL
        public bool AXIS_ROBOT { get; set; }

        protected short axis;
        private IEnumerable<short> AxisList { get { yield return axis; } }

        protected Paix.Manager _paix;
        protected NMCAXESEXPR nmcSensor;

        public Paix.Manager Paix => _paix;

        public override bool DevClose()
        {
            if (_cts != null)
                _cts.Cancel();

            return true;
        }

        /// <summary>
        /// Device Open Process:
        /// Get Controller Version
        /// </summary>
        /// <returns></returns>
        public override bool DevOpen()
        {
            var oResult = TryFunc(() =>
            {
                _paix = new Paix.Manager(CONTROLLER_ADDRESS, false);
                _paix.SetProtocolMethod(0); //  1 is UDP, 0 is TCP
                _paix.OpenPaix();

                Thread.Sleep(100);

                return EnableLimit();
            });

            if (oResult.HasException || oResult.Result == false)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Open a Motion Axis", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return false;
            }

            return oResult.Result;
        }




        public override bool DevReset() { return true; }


        #region CHECK_MOTION_STATUS

        public abstract bool IsReady();




        public bool IsMove()
        {
            int nRet = _paix.GetBusyStatus(axis).Value;

            return Convert.ToBoolean(nRet);
        }


        public Nullable<bool> IsMove(short axis)
        {
            var busyStatus = _paix.GetBusyStatus(axis);
            if (busyStatus.IsNone())
                return null;
            return busyStatus.Value == 1;
        }



        public abstract bool IsInposition();

        #endregion



        protected abstract bool EnableLimit();



        public double GetCommandPos()
        {
            int nPos = _paix.GetCmdPos(axis).Value;

            return nPos / CFG_PULSE_PER_REVOLUTION;
        }


        public double GetCurrentPos()
        {
            int nPos = _paix.GetEncPos(axis).Value;

            return nPos / CFG_PULSE_PER_REVOLUTION;
        }

        public double GetCommandRpm()
        {
            var adCmdSpeed = _paix.GetAxesCmdSpeed().Value;

            return adCmdSpeed[axis] / CFG_PULSE_PER_REVOLUTION * 60;
        }

        public double GetCurrentRpm()
        {
            var adEncSpeed = _paix.GetAxesEncSpeed().Value;

            int Direction = 1;

            if (AXIS_DIRECTION == "CW")
                Direction = Direction * (-1);

            return adEncSpeed[axis] / CFG_PULSE_PER_REVOLUTION * 60 * Direction;
        }

        public abstract void StopMotion(bool bEmergency);


        /// Goto axis hardware origin
        public virtual Task<bool> HomeMove(CancellationToken cancelToken, bool resetCmdEncPosition)
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerable<short> GetAxesList();
        public int GetCmdPos(short axis) { return _paix.GetCmdPos(axis).Value; }
        public int GetEncPos(short axis) { return _paix.GetEncPos(axis).Value; }

        public abstract bool ClearAlarm();
        public virtual bool ClearAlarmOnDemand()
        {
            throw new NotImplementedException();
        }
        public virtual bool BreakServoChange(bool bOn)
        {
            throw new NotImplementedException();
        }
    }
}
