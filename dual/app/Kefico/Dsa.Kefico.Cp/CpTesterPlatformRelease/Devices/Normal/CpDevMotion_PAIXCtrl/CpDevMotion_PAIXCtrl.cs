using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using Dsu.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Paix = Dsu.Driver.Paix;
using static CpCommon.ExceptionHandler;
using static Dsu.Driver.PaixMotionControler.NMC2;
using static CpBase.CpLog4netLogging;
using LanguageExt;
using static LanguageExt.Prelude;
using static LanguageExt.FSharp;
using Dsu.Driver.UI.Paix;

namespace CpTesterPlatform.CpDevices
{
#if false
    public class CpDevMotion_PAIXCtrl : IMotion
    {
        public CpFunctionEventHandler FuncEvtHndl { get; set; }
        public string DeviceID { get; set; }

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
        /// True = CW, FALSE = CCW
        private bool REV_DIRECTION = true;

        private short axis;

        private Paix.Manager paix;
        private NMCAXESEXPR nmcSensor;
        private CancellationTokenSource cancelToken;

        public bool DevClose()
        {
            if (cancelToken != null)
                cancelToken.Cancel();


            paix.SetCurrentOn(axis, 0);
            paix.SetServoOn(axis, 0);
            paix.Close();

            return true;
        }

        /// <summary>
        /// Device Open Process:
        /// Get Controller Version
        /// </summary>
        /// <returns></returns>
        public bool DevOpen()
        {
            var oResult = TryFunc(() =>
            {
                paix = new Paix.Manager(CONTROLLER_ADDRESS, false);
                paix.SetProtocolMethod(0); //  1 is UDP, 0 is TCP
                paix.OpenPaix();

                Thread.Sleep(100);
                var version = paix.GetFirmVersion().Value;

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

                paix.SetCurrentOn(axis, 1);
                paix.SetServoOn(axis, 1);
                //Unit 당 Pulse는 항상 1:1 (10000 Pulse/ 1Cycle 는 모터 엔코더 설정 따르며 0.0001 설정시 10000 배 Pulse 발생하여 위험)
                paix.SetUnitPerPulse(axis, 1);
                paix.SetDisconectedStopMode(2000, 1);  // 2초간 통신 없으면 모터 비상정지  (0 : 감속정지 1 : 비상정지)

                Task taskWatch = new Task(() => taskWatchSensor());
                taskWatch.Start();

                if (EnableLimit())
                    return true;
                else
                    return false;
            });

            if (oResult.HasException || oResult.Result == false)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Open a Motion Axis", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return false;
            }

            return oResult.Result;
        }

        private async void taskWatchSensor()
        {
            cancelToken = new CancellationTokenSource();

            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                    return;

                await Task.Delay(50);
                //Thread.Sleep(50);
                //Task.Delay(1000);///Equal sleep time with a PAIX example
                updateCmdEnc();
            }
        }


        private void updateCmdEnc()
        {
            var returnData = paix.GetAxesExpress();
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


        public bool DevReset()
        {
            SetCommandPosition(0);
            SetEncoderPosition(0);

            return true;
        }

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }

#region CHECK_MOTION_STATUS
        /// <summary>
        /// Not working properly.
        /// Even, in the Ethermotion -> Not working
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            ///PAIX Defined Time, at Manual 51Page 
            Thread.Sleep(100);
            NMCAXESEXPR nmcStatus = paix.GetAxesExpress().Value;
            int nState = nmcStatus.nSReady[axis];

            return Convert.ToBoolean(nState);
        }



        public bool IsMove()
        {
            int nRet = paix.GetBusyStatus(axis).Value;

            return Convert.ToBoolean(nRet);
        }

        public Nullable<bool> IsMove(short axisSelected)
        {
            int nRet = paix.GetBusyStatus(axisSelected).Value;

            return Convert.ToBoolean(nRet);
        }

        public bool IsInposition()
        {
            NMCAXESEXPR nmcStatus = paix.GetAxesExpress().Value;
            int nState = nmcStatus.nInpo[axis];

            return Convert.ToBoolean(nState);
        }
#endregion

        public void SetCommandPosition(double dCmdPos)
        {
            paix.SetCmdPos(axis, dCmdPos);
        }

        public void SetEncoderPosition(double dEncPos)
        {
            paix.SetEncPos(axis, dEncPos);
        }

        public void SetDirection(bool bCW)
        {
            REV_DIRECTION = bCW;

        }

        public void SetRpmSpeed(double dMoveVel, bool overrideSpeed = false)
        {
            if (!EnableLimit())
                return;

            paix.SetSpeed(axis
             , 0
             , CFG_PULSE_PER_REVOLUTION * CFG_ACC_RPM_RER_SEC / 60
             , CFG_PULSE_PER_REVOLUTION * CFG_DEC_RPM_RER_SEC / 60 * -1
             , CFG_PULSE_PER_REVOLUTION * dMoveVel / 60);

            if (overrideSpeed)
            {
                paix.SetOverrideDriveSpeed(axis, CFG_PULSE_PER_REVOLUTION * dMoveVel / 60);
            }
        }

        public void SetRelMove(double dPos)
        {
            if (!EnableLimit())
                return;

            short Jog = SetDirection();

            if (Jog == 1)
                dPos = dPos * (-1);

            var a = paix.RelMove(axis, CFG_PULSE_PER_REVOLUTION * dPos);
        }

        private bool EnableLimit()
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

        public void SetAbsMove(double dPos)
        {
            if (!EnableLimit())
                return;

            paix.AbsMove(axis, CFG_PULSE_PER_REVOLUTION * dPos);
        }

        public void SetJogMove()
        {
            if (!EnableLimit())
                return;

            short Jog = SetDirection();

            paix.JogMove(axis, Jog);
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

        public double GetCommandPos()
        {
            int nPos = paix.GetCmdPos(axis).Value;

            return nPos / CFG_PULSE_PER_REVOLUTION;
        }


        public double GetCurrentPos()
        {
            int nPos = paix.GetEncPos(axis).Value;

            return nPos / CFG_PULSE_PER_REVOLUTION;
        }

        public double GetCommandRpm()
        {
            var adCmdSpeed = paix.GetAxesCmdSpeed().Value;

            return adCmdSpeed[axis] / CFG_PULSE_PER_REVOLUTION * 60;
        }

        public double GetCurrentRpm()
        {
            var adEncSpeed = paix.GetAxesEncSpeed().Value;

            int Direction = 1;

            if (AXIS_DIRECTION == "CW")
                Direction = Direction * (-1);

            return adEncSpeed[axis] / CFG_PULSE_PER_REVOLUTION * 60 * Direction;
        }

        public void StopMotion(bool bEmergency)
        {
            if (bEmergency)
                paix.SuddenStop(axis);
            else
                paix.DecStop(axis);
        }

        /// Goto axis hardware origin
        public Task<bool> HomeMove(CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<short> GetAxesList() { yield return Convert.ToInt16(AXIS_ID); }
        public int GetCmdPos(short axis) { return paix.GetCmdPos(axis).Value; }
        public int GetEncPos(short axis) { return paix.GetEncPos(axis).Value; }
        public int[] GetCmdPositions() { throw new NotImplementedException(); }
        public int[] GetEncPositions() { throw new NotImplementedException(); }
        public void SetCommandEncorderZeroSetting(IEnumerable<short> axes) { throw new NotImplementedException(); }
        public void SetCommandZeroSetting(IEnumerable<short> axes) { throw new NotImplementedException(); }

        public void VerifiyCommandEncoderZero(IEnumerable<short> axes, int tolerance) { throw new NotImplementedException(); }
        public bool IsCommandEncoderZero(IEnumerable<short> axes, int tolerance) { throw new NotImplementedException(); }
        public Task<bool> MovePath(string pathName, CancellationToken cancelToken) { throw new NotImplementedException(); }

        public bool ClearAlarm()
        {
            if (0 != paix.SetAlarmResetOn(axis, 1))
                return false;

            Thread.Sleep(500);

            if (0 != paix.SetAlarmResetOn(axis, 0))
                return false;

            return true;
        }

        public void SetModePath(List<string> paths)
        {
            throw new NotImplementedException();

        }

        public void OpenManualDialog()
        {
            throw new NotImplementedException();
        }

        public void SetRelMove(short axis, double dPos)
        {
            throw new NotImplementedException();
        }

        public void SetRobotSpeed(short axis, double MAX, double ACC, double DEC, bool overrideSpeed = false)
        {
            throw new NotImplementedException();
        }

        public List<string> GetPath()
        {
            throw new NotImplementedException();
        }
    }
#endif
}