using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using Dsu.Driver;
using System;
using System.Linq;
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
using static LanguageExt.FSharp;
using Dsu.Driver.UI.Paix;
using System.IO;
using System.Windows.Forms;
using Dsu.Driver.Util.Emergency;
using Dsu.Common.Utilities.Core.FSharpInterOp;
using Dsu.Driver.Base;

namespace CpTesterPlatform.CpDevices
{
#if false
    public partial class CpDevRobot_PAIXCtrl : IMotion
    {
        private enum AxisEnumAudit78 { X = 0, Y, Z, Tilt };
        private enum AxisEnumAuditGCVT { WheelRotate, Tilt, Z, WheelAdvance };

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

        /// axis, ccw
        private List<Tuple<short, bool>> _axisWithDirList = new List<Tuple<short, bool>>();
        private IEnumerable<short> AxisList { get { return _axisWithDirList.Select(tpl => tpl.Item1); } }

        private Paix.Manager _paix;
        private Dictionary<string, AuditPoses> _poses = new Dictionary<string, AuditPoses>(StringComparer.OrdinalIgnoreCase);
        private CancellationTokenSource _cts;
        private bool _isPlaying;

        private CancellationTokenSource ResetCancellationTokenSource()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            return _cts;
        }

        public bool DevClose()
        {

            if (_cts != null)
                _cts.Cancel();

            //foreach (var axis in axisList)  //test ahn
            //{
            //    paix.SetCurrentOn(axis, 0);
            //    paix.SetServoOn(axis, 0);
            //}

            _paix.Close();

            return true;
        }

        private void CheckValue(bool b, string errorDescription)
        {
            if ( ! b)
            {
                var message = $"EMC Para logic error: {errorDescription} error.";
                LogErrorRobot(message);
                throw new ExceptionWithCode(ErrorCodes.DEV_PaixError, message);
            }
        }

        private string AxesToString(IEnumerable<short> axes) => String.Join(", ", axes.Select(ax => ax.ToString()));

        private List<string> CheckErrorStatus(IEnumerable<short> axes)
        {
            var realAxes = axes ?? AxisList;
            List<string> errors = new List<string>();
            Prelude.match(fs(_paix.GetAxesExpress()),
                Some: v =>
                {
                    if (axes.Any(ax => v.nBusy[ax] == 1))   // BUSY : Pulse 출력 상태(0:Idle, 1:Busy)
                        errors.Add($"Busy Axes:[{AxesToString(axes.Where(ax => v.nBusy[ax] == 1))}]");
                    if (axes.Any(ax => v.nError[ax] == 1))  // Error 발생 여부(0:None error, 1:Error)
                        errors.Add($"Error on axes:[{AxesToString(axes.Where(ax => v.nError[ax] == 1))}]");
                    if (axes.Any(ax => v.nAlarm[ax] == 1))  // Alarm Sensor 입력 상태(0:OFF, 1:ON)
                        errors.Add($"Alarm on axes:[{AxesToString(axes.Where(ax => v.nAlarm[ax] == 1))}]");
                    if (axes.Any(ax => v.nSReady[ax] == 1))  // Servo Ready 입력 상태(0:OFF, 1:ON)
                        errors.Add($"Servo not ready:[{AxesToString(axes.Where(ax => v.nSReady[ax] == 1))}]");
                },
                None: () => { });

            return errors;
        }

        
        private void CheckParaLogicAuditGCVT()
        {
            LogInfoRobot("Checking NMC2 Para logics...");
            foreach (var axis in AxisList)
            {
                Prelude.match(fs(_paix.GetParaLogicEx(axis)),
                    Some: v =>
                    {
                        
                        CheckValue(v.nEmg == 1, "Emergency Logic");   // Emergency 로직(0:LOW, 1:HIGH) Emergency는 그룹별로 Logic이 설정 되며,0축,4축이 기준이 되업니다.
                        CheckValue(v.nEncCount == 0, "Encoder Counter Mode");      // 엔코더 카운트 모드 (0:4체배, 1:2체배, 2:1체배)
                        CheckValue(v.nEncDir == 1, "Encoder Counter Direction");    // 엔코더 카운트 방향 (0:A|B(+, 1:B|A(-), 2:Up|Down, 3:Down|Up

                        CheckValue(v.nEncZ == 1, "Enocoder Z");      // 엔코더 Z 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nNear == 0, "Near logic");      // NEAR(Org)센서 로직  (0:LOW, 1:HIGH)

                        /* Gamma CVT 의 경우, +/- Limit sensor logic 만 다르다. */
                        var limitSensorLogic = (axis == 1) ? 1 : 0;
                        CheckValue(v.nMLimit == limitSensorLogic, "Minus limit sensor logic");    // - Limit Sensor 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nPLimit == limitSensorLogic, "Plus limit sensor logic");    // + Limit Sensor 로직 (0:LOW, 1:HIGH)


                        CheckValue(v.nAlarm == 1, "Alarm Sensor logic");     // Alarm Sensor  로직 (0:LOW, 1:HIGH)
                        /* 위치 결정 ?? */ CheckValue(v.nInp == 0, "InPosition logic");       // Inposition 입력 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nSReady == 0, "Servo Ready logic");    // Servo Ready 입력 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nPulseMode == 0, "Pulse mode"); // PulseMode (0:2pulse Low CW/CCW, 1:2pulse Low CCW/CW
                                                                     // 2:2pulse HighLow CW/CCW, 3:2pulse High CCW/CW
                                                                     // 4:1pulse Low CW/CCW, 5:1pulse Low CCW/CW
                                                                     // 6:1pulse HighLow CW/CCW, 7:1pulse High CCW/CW


                        CheckValue(v.nLimitStopMode == 0, "H/W Limit stop mode");   // Limit 시 정지 모드 (0:긴급 정지, 1:감속 정지)
                        CheckValue(v.nBusyOff == 0, "Busy Off mode");               // Busy Off 모드 (0: 펄스 출력, 1:위치 결정
                        CheckValue(v.nSWEnable == 0, "Software Limit Usage");       // Software Limit 사용 여부(0:사용 X, 1:사용 O)

                        /*
                         * HomeDone flag 를 cancel 상태로 강제로 write 하기 위한 것으로 우리의 시험기 요구 사항과는 무관함. (by PAIX)
                         */
                        //CheckValue(v.nHDoneCancelAlarm == 0, "Home done cancel on alarm");                // 알람 발생 시 원점 완료상태 해지 사용여부
                        //CheckValue(v.nHDoneCancelServoOff == 0, "Home done cancel on servo off");         // 서보 오프 발생 시 원점 완료상태 해지 사용여부
                        //CheckValue(v.nHDoneCancelCurrentOff == 0, "Home done cancel on current off");       // 전류 오프 발생 시 원점 완료상태 해지 사용여부
                        //CheckValue(v.nHDoneCancelServoReady == 0, "Home done cancel on servo ready");       // 서보 레디 발생 시 원점 완료상태 해지 사용여부
                    },
                    None: () => {
                        CheckValue(false, "Failed to extract Para logic info from NMC driver.");
                    });
            }

            LogInfoRobot(" = Finished Checking NMC2 Para logics.");
        }

        private void CheckParaLogicAudit78()
        {
            LogInfoRobot("Checking NMC2 Para logics...");
            foreach (var axis in AxisList)
            {
                Prelude.match(fs(_paix.GetParaLogicEx(axis)),
                    Some: v =>
                    {
                        
                        CheckValue(v.nEmg == 1, "Emergency Logic");   // Emergency 로직(0:LOW, 1:HIGH) Emergency는 그룹별로 Logic이 설정 되며,0축,4축이 기준이 되업니다.
                        CheckValue(v.nEncCount == 0, "Encoder Counter Mode");      // 엔코더 카운트 모드 (0:4체배, 1:2체배, 2:1체배)
                        CheckValue(v.nEncDir == 1, "Encoder Counter Direction");    // 엔코더 카운트 방향 (0:A|B(+, 1:B|A(-), 2:Up|Down, 3:Down|Up

                        CheckValue(v.nEncZ == 1, "Enocoder Z");      // 엔코더 Z 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nNear == 0, "Near logic");      // NEAR(Org)센서 로직  (0:LOW, 1:HIGH)
                        CheckValue(v.nMLimit == 1, "Minus limit sensor logic");    // - Limit Sensor 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nPLimit == 1, "Plus limit sensor logic");    // + Limit Sensor 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nAlarm == 1, "Alarm Sensor logic");     // Alarm Sensor  로직 (0:LOW, 1:HIGH)
                        /* 위치 결정 ?? */ CheckValue(v.nInp == 0, "InPosition logic");       // Inposition 입력 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nSReady == 0, "Servo Ready logic");    // Servo Ready 입력 로직 (0:LOW, 1:HIGH)
                        CheckValue(v.nPulseMode == 0, "Pulse mode"); // PulseMode (0:2pulse Low CW/CCW, 1:2pulse Low CCW/CW
                                                                     // 2:2pulse HighLow CW/CCW, 3:2pulse High CCW/CW
                                                                     // 4:1pulse Low CW/CCW, 5:1pulse Low CCW/CW
                                                                     // 6:1pulse HighLow CW/CCW, 7:1pulse High CCW/CW


                        CheckValue(v.nLimitStopMode == 0, "H/W Limit stop mode");   // Limit 시 정지 모드 (0:긴급 정지, 1:감속 정지)
                        CheckValue(v.nBusyOff == 0, "Busy Off mode");               // Busy Off 모드 (0: 펄스 출력, 1:위치 결정
                        CheckValue(v.nSWEnable == 0, "Software Limit Usage");       // Software Limit 사용 여부(0:사용 X, 1:사용 O)

                        /*
                         * HomeDone flag 를 cancel 상태로 강제로 write 하기 위한 것으로 우리의 시험기 요구 사항과는 무관함. (by PAIX)
                         */
                        //CheckValue(v.nHDoneCancelAlarm == 0, "Home done cancel on alarm");                // 알람 발생 시 원점 완료상태 해지 사용여부
                        //CheckValue(v.nHDoneCancelServoOff == 0, "Home done cancel on servo off");         // 서보 오프 발생 시 원점 완료상태 해지 사용여부
                        //CheckValue(v.nHDoneCancelCurrentOff == 0, "Home done cancel on current off");       // 전류 오프 발생 시 원점 완료상태 해지 사용여부
                        //CheckValue(v.nHDoneCancelServoReady == 0, "Home done cancel on servo ready");       // 서보 레디 발생 시 원점 완료상태 해지 사용여부
                    },
                    None: () => {
                        CheckValue(false, "Failed to extract Para logic info from NMC driver.");
                    });
            }

            LogInfoRobot(" = Finished Checking NMC2 Para logics.");
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
                foreach (var axisID in AXIS_ID.Split(';'))
                {
                    if (!axisID.StartsWith("+") && !axisID.StartsWith("-"))
                        throw new ExceptionWithCode(ErrorCodes.APP_ConfigurationError, "Axis direction not specified in CPTesterConfiguration.xml.");

                    var rotationAxis = Convert.ToInt16(axisID);
                    _axisWithDirList.Add(Tuple.Create(Math.Abs(rotationAxis), axisID.StartsWith("+")));
                }

                _paix = new Paix.Manager(CONTROLLER_ADDRESS, false);
                _paix.SetProtocolMethod(0); //  1 is UDP, 0 is TCP
                _paix.OpenPaix();

                Thread.Sleep(200);
                var version = _paix.GetFirmVersion()?.Value;

                //var paixParaLogic = Directory.GetCurrentDirectory() + "\\Configure\\HwConfig\\ROM_DATA.AxesLgc";
                //if ( 0 != _paix.SetParaLogicFile(paixParaLogic.AsEnumerable().ToArray()) )
                //    throw new ExceptionWithCode(ErrorCodes.DEV_PaixError, "Failed to write logic file.");

                if (!DriverBaseGlobals.IsAudit())
                    throw new ExceptionWithCode(ErrorCodes.APP_InternalError, "DevOpen for Robot should be called in Audit tester.");

                if (DriverBaseGlobals.IsAudit78())
                    CheckParaLogicAudit78();
                else
                    CheckParaLogicAuditGCVT();

                foreach (var axis in AxisList)
                {
                    //var axis = tpl.Item1;
                    _paix.SetCurrentOn(axis, 1);
                    _paix.SetServoOn(axis, 1);
                    //Unit 당 Pulse는 항상 1:1 (10000 Pulse/ 1Cycle 는 모터 엔코더 설정 따르며 0.0001 설정시 10000 배 Pulse 발생하여 위험)
                    _paix.SetUnitPerPulse(axis, 1);
                    _paix.SetDisconectedStopMode(1000, 1);  // 1초간 통신 없으면 모터 비상정지  (0 : 감속정지 1 : 비상정지)

                }


                //var SavedPos = OpenRobotAxisPos();
                //for (int i=0; i < SavedPos.Count; i++)
                //    _paix.SetCmdEncPos((short)i, SavedPos[i]);


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

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }

        public bool DevReset()
        {
            return true;
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
            NMCAXESEXPR nmcStatus = _paix.GetAxesExpress().Value;
            bool bReady = false;

            foreach (var axis in AxisList)
            {
                if (nmcStatus.nSReady[axis] == 1)
                    bReady = true;
                else
                {
                    bReady = false;
                    break;
                }
            }

            return bReady;
        }

        public Nullable<bool> IsMove(short axis)
        {
            var busyStatus = _paix.GetBusyStatus(axis);
            if (busyStatus.IsNone())
                return null;
            return busyStatus.Value == 1;
        }


        public bool IsInposition()
        {
            return Prelude.match(fs(_paix.GetAxesExpress()),
                Some: nmcStatus =>
                {
                    bool bInposition = false;
                    foreach (var axis in AxisList)
                    {
                        if (nmcStatus.nInpo[axis] == 1)
                            bInposition = true;
                        else
                        {
                            bInposition = false;
                            break;
                        }

                    }
                    return bInposition;
                },
                None: () => false);
        }
#endregion

        public void SetCommandPosition(double dCmdPos)
        {
            throw new NotImplementedException();

        }

        public void SetEncoderPosition(double dEncPos)
        {
            throw new NotImplementedException();

        }

        public void SetDirection(bool bCW)
        {
            REV_DIRECTION = bCW;

        }

        public void SetRelMove(double dPos)
        {
            throw new NotImplementedException();

        }


        public void SetRelMove(short axis, double dPos)
        {
            if (!EnableLimit())
                return;

            foreach (var tpl in _axisWithDirList)
            {
                if (axis == tpl.Item1)
                {
                    var clockwise = tpl.Item2;
                    if (clockwise)
                        _paix.RelMove(axis, dPos);
                    else
                        _paix.RelMove(axis, -dPos);
                }
            }
        }

        public void SetRpmSpeed(double dMoveVel, bool overrideSpeed = false)
        {
            throw new NotImplementedException();
        }

        public void SetRobotSpeed(short axis, double MAX, double ACC, double DEC, bool overrideSpeed = false)
        {
            if (!EnableLimit())
                return;

            if (overrideSpeed)
                _paix.SetOverrideDriveSpeed(axis, MAX);
            else
                _paix.SetSpeed(axis, 0, ACC, -DEC, MAX);
        }

        private bool EnableLimit()
        {
            if (CFG_ACC_RPM_RER_SEC > 1000 || CFG_DEC_RPM_RER_SEC > 1000 || CFG_MAX_RPM_RER_SEC > 5000)
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
            throw new NotImplementedException();

        }

        public void SetJogMove()
        {
            throw new NotImplementedException();

        }

        private short SetDirection()
        {
            throw new NotImplementedException();

        }

        public double GetCommandPos()
        {
            throw new NotImplementedException();

        }


        public double GetCurrentPos()
        {
            throw new NotImplementedException();

        }

        public double GetCommandRpm()
        {
            throw new NotImplementedException();

        }

        public double GetCurrentRpm()
        {
            throw new NotImplementedException();

        }

        public void StopMotion(bool bEmergency)
        {
            if (_cts != null)
                _cts.Cancel();
            foreach (var axis in AxisList)
            {
                if (bEmergency)
                    _paix.SuddenStop(axis);
                else
                    _paix.DecStop(axis);
            }

            _isPlaying = false;
            _paix.ContiRunStop(0, 0);
        }



        public IEnumerable<short> GetAxesList() { return _axisWithDirList.Select(tpl => tpl.Item1); }
        public int GetCmdPos(short axis) { return _paix.GetCmdPos(axis).Value; }
        public int GetEncPos(short axis) { return _paix.GetEncPos(axis).Value; }


        public int[] GetCmdPositions()
        {
            return AxisList.Select(ax => _paix.GetCmdPos(ax).Value).ToArray();
        }
        public int[] GetEncPositions()
        {
            return AxisList.Select(ax => _paix.GetEncPos(ax).Value).ToArray();
        }

        public void SetCommandEncorderZeroSetting(IEnumerable<short> axes)
        {
            LogInfoRobot("Set Command/Encoder value into zero.");
            var realAxes = axes ?? AxisList;
            foreach (var axis in realAxes)
            {
                // DO not use SetCmdPos/SetEncPos
                _paix.SetCmdEncPos(axis, 0);
            }
        }

        public void SetCommandZeroSetting(IEnumerable<short> axes)
        {
            LogInfoRobot("Set Command value into zero.");
            var realAxes = axes ?? AxisList;
            foreach (var axis in realAxes)
            {
                _paix.SetCmdPos(axis, 0);
            }
        }

        public bool IsCommandEncoderZero(IEnumerable<short> axes, int tolerance =0)
        {
            var realAxes = axes ?? AxisList;
            foreach (var axis in realAxes)
            {
                if ( Math.Abs(_paix.GetCmdPos(axis).Value) > tolerance || Math.Abs(_paix.GetEncPos(axis).Value) > tolerance)
                    return false;
            }

            return true;
        }

        public void VerifiyCommandEncoderZero(IEnumerable<short> axes, int tolerance)
        {
            if ( ! IsCommandEncoderZero(axes, tolerance) )
                throw new ExceptionWithCode(ErrorCodes.DEV_PaixError, "Failed to set Cmd/Enc as Zero.");
        }

        public bool ClearAlarm()
        {
            foreach (var axis in AxisList)  //SetOutLimitTimePin(short ioType, short pinNo, short on, int time);
            {
                if (0 != _paix.SetAlarmResetOn(axis, 1))
                    return false;

                Thread.Sleep(500);
                if (0 != _paix.SetAlarmResetOn(axis, 0))
                    return false;
            }

            return true;
        }


        public List<string> GetPath()
        {
            return _poses.Keys.ToList(); 
        }

        public List<int> GetAxisLastPos()
        {
            List<int> lstPos = new List<int>();
            foreach (var axis in AxisList)
                lstPos.Add(_paix.GetEncPos(axis).Value);

            return lstPos;
        }

        public void SetModePath(List<string> paths)
        {
            _poses.Clear();
            foreach (var path in paths)
            {
                var file = new FileInfo(path);
                _poses.Add(file.Name.Replace(file.Extension, ""), AuditPoses.LoadFromXml(path));
            }
        }

        public async Task<bool> MovePath(string pathName, CancellationToken cancelToken)
        {
            try
            {
                if (!_poses.ContainsKey(pathName))
                    return false;

                if (_isPlaying)
                    return false;

                _isPlaying = true;

                var token = cancelToken == null ? ResetCancellationTokenSource().Token : cancelToken;
                foreach (var pose in _poses[pathName])
                    pose.Checked = true;

                LogInfoRobot($"MovePath({pathName}) executing..");
                await _poses[pathName].PathMove(_paix, token);
                LogInfoRobot($"MovePath({pathName}) finished.");

                return true;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
            finally
            {
                _isPlaying = false;
            }
        }

        public void OpenManualDialog()
        {
            new FormPathPlanner(_paix).Show();
        }
    }
#endif
}