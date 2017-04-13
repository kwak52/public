using CpTesterPlatform.CpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CpCommon.ExceptionHandler;
using static Dsu.Driver.PaixMotionControler.NMC2;
using static CpBase.CpLog4netLogging;
using System.Threading;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevPaixMotor : CpDevPaixMotion
    {
        /// True = CW, FALSE = CCW
        private bool REV_DIRECTION = true;
        public override IEnumerable<short> GetAxesList() { yield return Convert.ToInt16(AXIS_ID); }

        /// <summary>
        /// Device Open Process:
        /// Get Controller Version
        /// </summary>
        /// <returns></returns>
        public override bool DevOpen()
        {
            var oResult = TryFunc(() =>
            {
                if (!base.DevOpen())
                    return false;

                if (AXIS_ID.Contains(";"))
                {
                    UtilTextMessageBox.UIMessageBoxForWarning("Robot parameter", "Robot parameter error");
                    return false;
                }

                axis = Convert.ToInt16(AXIS_ID);
                if (!ClearAlarm())
                {
                    LogError("Failed to clear alarm.");
                    return false;
                }

                _paix.SetCurrentOn(axis, 1);
                _paix.SetServoOn(axis, 1);
                //Unit 당 Pulse는 항상 1:1 (10000 Pulse/ 1Cycle 는 모터 엔코더 설정 따르며 0.0001 설정시 10000 배 Pulse 발생하여 위험)
                _paix.SetUnitPerPulse(axis, 1);
                _paix.SetDisconectedStopMode(2000, 1);  // 2초간 통신 없으면 모터 비상정지  (0 : 감속정지 1 : 비상정지)

                Task taskWatch = new Task(() => taskWatchSensor());
                taskWatch.Start();

                return true;
            });

            if (oResult.HasException || oResult.Result == false)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Open a Motion Axis", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return false;
            }

            return oResult.Result;
        }

        public override bool DevClose()
        {
            base.DevClose();

            _paix.SetCurrentOn(axis, 0);
            _paix.SetServoOn(axis, 0);
            _paix.Close();

            return true;
        }

        private async void taskWatchSensor()
        {
            _cts = new CancellationTokenSource();

            while (true)
            {
                if (_cts.IsCancellationRequested)
                    return;

                await Task.Delay(50);
                //Thread.Sleep(50);
                //Task.Delay(1000);///Equal sleep time with a PAIX example
                updateCmdEnc();
            }
        }


        private void updateCmdEnc()
        {
            var returnData = _paix.GetAxesExpress();
            if (returnData == null)
                return;

            NMCAXESEXPR nmcStatus = returnData.Value;
            if (nmcSensor.dCmd == null
                || nmcStatus.dCmd[axis] != nmcSensor.dCmd[axis]
                || nmcStatus.dEnc[axis] != nmcSensor.dEnc[axis]
                || nmcStatus.nEmer[0] != nmcSensor.nEmer[0]
                || nmcStatus.nBusy[axis] != nmcSensor.nBusy[axis]
                || nmcStatus.nNear[axis] != nmcSensor.nNear[axis]
                || nmcStatus.nPLimit[axis] != nmcSensor.nPLimit[axis]
                || nmcStatus.nMLimit[axis] != nmcSensor.nMLimit[axis]
                || nmcStatus.nAlarm[axis] != nmcSensor.nAlarm[axis]
                || nmcStatus.nHome[axis] != nmcSensor.nHome[axis])
            {
                nmcSensor = nmcStatus;
                FuncEvtHndl.DoTcpIpReceive(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}"
                    , nmcSensor.dCmd[axis]
                    , nmcSensor.dEnc[axis]
                    , nmcSensor.nEmer[0]
                    , nmcSensor.nBusy[axis]
                    , nmcSensor.nNear[axis]
                    , nmcSensor.nPLimit[axis]
                    , nmcSensor.nMLimit[axis]
                    , nmcSensor.nAlarm[axis]
                    , nmcSensor.nHome[axis]));  //test ahn
            }
        }

        public void SetDirection(bool bCW)
        {
            REV_DIRECTION = bCW;
        }

        private short SetDirection()
        {
            short Jog = (short)(REV_DIRECTION ? 1 : 0);

            if (AXIS_DIRECTION == "CCW")
            {
                if (Jog == 0) Jog = 1;
                else if (Jog == 1) Jog = 0;
            }

            return Jog;
        }

        public void SetJogMove()
        {
            if (!EnableLimit())
                return;

            short Jog = SetDirection();

            _paix.JogMove(axis, Jog);
        }

        public void SetRelMove(double dPos)
        {
            if (!EnableLimit())
                return;

            short Jog = SetDirection();

            if (Jog == 1)
                dPos = dPos * (-1);

            var a = _paix.RelMove(axis, CFG_PULSE_PER_REVOLUTION * dPos);
        }

        public void SetAbsMove(double dPos)
        {
            if (!EnableLimit())
                return;

            _paix.AbsMove(axis, CFG_PULSE_PER_REVOLUTION * dPos);
        }




        protected override bool EnableLimit()
        {
            if (CFG_ACC_RPM_RER_SEC > 3000 || CFG_DEC_RPM_RER_SEC > 3000 || CFG_MAX_RPM_RER_SEC > 5000)
            {
                UtilTextMessageBox.UIMessageBoxForWarning("Motion Limit Error"
                    , string.Format("Motion Limit Error \r\nacc RPM/sec (Limit 1000) {0} dec RPM/sec (Limit 1000) {1} max RPM (Limit 5000) {2}", CFG_ACC_RPM_RER_SEC, CFG_DEC_RPM_RER_SEC, CFG_MAX_RPM_RER_SEC));
                return false;
            }
            else
                return true;
        }

        public override bool DevReset()
        {
            SetCommandPosition(0);
            SetEncoderPosition(0);

            return true;
        }

        /// <summary>
        /// Not working properly.
        /// Even, in the Ethermotion -> Not working
        /// </summary>
        /// <returns></returns>
        public override bool IsReady()
        {
            ///PAIX Defined Time, at Manual 51Page 
            Thread.Sleep(100);
            NMCAXESEXPR nmcStatus = _paix.GetAxesExpress().Value;
            int nState = nmcStatus.nSReady[axis];

            return Convert.ToBoolean(nState);
        }

        public override bool IsInposition()
        {
            NMCAXESEXPR nmcStatus = _paix.GetAxesExpress().Value;
            int nState = nmcStatus.nInpo[axis];

            return Convert.ToBoolean(nState);
        }

        public void SetCommandPosition(double dCmdPos)
        {
            _paix.SetCmdPos(axis, dCmdPos);
        }

        public void SetEncoderPosition(double dEncPos)
        {
            _paix.SetEncPos(axis, dEncPos);
        }

        public override void StopMotion(bool bEmergency)
        {
            if (bEmergency)
                _paix.SuddenStop(axis);
            else
                _paix.DecStop(axis);
        }

        public void SetRpmSpeed(double dMoveVel, bool overrideSpeed = false)
        {
            if (!EnableLimit())
                return;

            _paix.SetSpeed(axis
             , 0
             , CFG_PULSE_PER_REVOLUTION * CFG_ACC_RPM_RER_SEC / 60
             , CFG_PULSE_PER_REVOLUTION * CFG_DEC_RPM_RER_SEC / 60 * -1
             , CFG_PULSE_PER_REVOLUTION * dMoveVel / 60);

            if (overrideSpeed)
            {
                _paix.SetOverrideDriveSpeed(axis, CFG_PULSE_PER_REVOLUTION * dMoveVel / 60);
            }
        }
    }
}
