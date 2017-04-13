﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using System.Threading;
using static CpCommon.ExceptionHandler;
using static Dsu.Driver.PaixMotionControler.NMC2;
using static CpBase.CpLog4netLogging;
using LanguageExt;
using static LanguageExt.FSharp;
using Dsu.Driver.UI.Paix;
using System.IO;
using Dsu.Driver.Util.Emergency;
using Dsu.Driver.Base;
using Dsu.Driver.Paix;

namespace CpTesterPlatform.CpDevices
{
    public partial class CpDevPaixRobot : CpDevPaixMotion
    {
        private enum AxisEnumAudit78 { X = 0, Y, Z, Tilt };
        private enum AxisEnumAuditGCVT { WheelRotate, Tilt, Z, WheelAdvance };


        /// axis, ccw
        private List<Tuple<short, bool>> _axisWithDirList = new List<Tuple<short, bool>>();
        private IEnumerable<short> AxisList { get { return _axisWithDirList.Select(tpl => tpl.Item1); } }

        private Dictionary<string, AuditPoses> _poses = new Dictionary<string, AuditPoses>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, AuditPoses> Poses => _poses;
        //private Paix.Manager _paix;
        //private CancellationTokenSource _cts;
        private bool _isPlaying;

        private CancellationTokenSource ResetCancellationTokenSource()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            return _cts;
        }

        public override bool DevClose()
        {
            base.DevClose();
            _paix.Close();

            return true;
        }

        private void CheckValue(bool b, string errorDescription)
        {
            if (!b)
            {
                var message = $"EMC Para logic error: {errorDescription} error.";
                LogErrorRobot(message);
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, message);
            }
        }

        private string AxesToString(IEnumerable<short> axes) => String.Join(", ", axes.Select(ax => ax.ToString()));


        private void TryClearErrorBeforeHomeMove(IEnumerable<short> axes)
        {
            return;

            //Prelude.match(fs(_paix.GetStopInfo()),
            //    Some: v => //axes.ForAll(ax => v[ax] != 1),       // 1 : stop by emergency, pp.189
            //    {
            //        axes.Iter(ax =>
            //        {
            //            switch(v[ax])
            //            {
            //                case 0: // normal stop
            //                    break;
            //                case 1: // emergency : try clear emergency stop bit by moving.
            //                    //_paix.RelMove(ax, 1);
            //                    //_paix.RelMove(ax, -1);
            //                    break;
            //                case 2: // MinusLimit
            //                    //_paix.RelMove(ax, 1000);
            //                    break;
            //                case 3: // PlusLimit
            //                    //_paix.RelMove(ax, -1000);
            //                    break;
            //                case 4: // AlarmOn,
            //                    throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Alarm exists.  Need reboot the tester.");
            //                    //_paix.ClearAlarm(new[] { ax });
            //                    //break;
            //            }
            //        });
            //    },
            //    None: () => { });
        }

        private List<string> CheckErrorStatusBeforeHomeMove(IEnumerable<short> axes)
        {
            var realAxes = axes ?? AxisList;
            List<string> errors = new List<string>();
            Prelude.match(fs(_paix.GetAxesExpress()),
                Some: v =>
                {
                    if (axes.Any(ax => v.nBusy[ax] == 1))   // BUSY : Pulse 출력 상태(0:Idle, 1:Busy)
                        errors.Add($"Busy Axes:[{AxesToString(axes.Where(ax => v.nBusy[ax] == 1))}]");

                    /* Home move 시 오류는 무시한다 => limit, emergency stop,  */
                    //if (axes.Any(ax => v.nError[ax] == 1))  // Error 발생 여부(0:None error, 1:Error)
                    //    errors.Add($"Error on axes:[{AxesToString(axes.Where(ax => v.nError[ax] == 1))}]");

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
                                                                             /* 위치 결정 ?? */
                        CheckValue(v.nInp == 0, "InPosition logic");       // Inposition 입력 로직 (0:LOW, 1:HIGH)
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
                                                                             /* 위치 결정 ?? */
                        CheckValue(v.nInp == 0, "InPosition logic");       // Inposition 입력 로직 (0:LOW, 1:HIGH)
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
        public override bool DevOpen()
        {
            var oResult = TryFunc(() =>
            {
                if (!base.DevOpen())
                    return false;

                foreach (var axisID in AXIS_ID.Split(';'))
                {
                    if (!axisID.StartsWith("+") && !axisID.StartsWith("-"))
                        throw ExceptionWithCode.Create(ErrorCodes.APP_ConfigurationError, "Axis direction not specified in CPTesterConfiguration.xml.");

                    var rotationAxis = Convert.ToInt16(axisID);
                    _axisWithDirList.Add(Tuple.Create(Math.Abs(rotationAxis), axisID.StartsWith("+")));
                }


                //var paixParaLogic = Directory.GetCurrentDirectory() + "\\Configure\\HwConfig\\ROM_DATA.AxesLgc";
                //if ( 0 != _paix.SetParaLogicFile(paixParaLogic.AsEnumerable().ToArray()) )
                //    throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Failed to write logic file.");

                if (!DriverBaseGlobals.IsAudit())
                    throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "DevOpen for Robot should be called in Audit tester.");

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


                return true;
            });

            if (oResult.HasException || oResult.Result == false)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Open a Motion Axis", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return false;
            }

            return oResult.Result;
        }



        #region CHECK_MOTION_STATUS
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


        public override bool IsInposition()
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

        //public void SetDirection(bool bCW)
        //{
        //    REV_DIRECTION = bCW;
        //}

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


        public void SetRobotSpeed(short axis, double MAX, double ACC, double DEC, bool overrideSpeed = false)
        {
            if (!EnableLimit())
                return;

            if (overrideSpeed)
                _paix.SetOverrideDriveSpeed(axis, MAX);
            else
                _paix.SetSpeed(axis, 0, ACC, -DEC, MAX);
        }

        protected override bool EnableLimit()
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


        public override void StopMotion(bool bEmergency)
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



        public override IEnumerable<short> GetAxesList() { return _axisWithDirList.Select(tpl => tpl.Item1); }


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

        public bool IsCommandEncoderZero(IEnumerable<short> axes, int tolerance = 0)
        {
            var realAxes = axes ?? AxisList;
            foreach (var axis in realAxes)
            {
                if (Math.Abs(_paix.GetCmdPos(axis).Value) > tolerance || Math.Abs(_paix.GetEncPos(axis).Value) > tolerance)
                    return false;
            }

            return true;
        }

        public void VerifiyCommandEncoderZero(IEnumerable<short> axes, int tolerance)
        {
            if (!IsCommandEncoderZero(axes, tolerance))
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Failed to set Cmd/Enc as Zero.");
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
                    throw ExceptionWithCode.CreateFatal(ErrorCodes.APP_InternalError, $"Move path {pathName} failed: Path not found.");

                if (_isPlaying)
                    throw ExceptionWithCode.CreateFatal(ErrorCodes.APP_InternalError, $"Move path {pathName} failed: Already moving.");

                // 서보 원점 수행 정보 유실 검사 <- 기기 reboot 시 원점 수행 이력 없어짐.
                var axes = DriverBaseGlobals.IsAudit78() ? AxisList : new[] { (short)AxisEnumAuditGCVT.Tilt };
                var allAxesHomeMoveHistory =
                    Prelude.match(fs(_paix.GetHomeStatus()),
                        Some: v => axes.ForAll(ax => v.nStatusFlag[ax] == 0),
                        None: () => false);
                if (!allAxesHomeMoveHistory)
                {
                    PaixManagerBase.IsOriginCalibrated = false;
                    throw ExceptionWithCode.CreateFatal(ErrorCodes.DEV_PaixError, $"Move path {pathName} failed: Robot Home move information lost.");
                }


                if (!pathName.ToLower().StartsWith("toorg_") && !PaixManagerBase.IsOriginCalibrated)
                    throw ExceptionWithCode.CreateFatal(ErrorCodes.APP_InternalError, $"Move path {pathName} failed: Origin not calibrated.");

                // 모든 축에 대해서 servo motor 가 들어 와 있는지 검사
                Prelude.match(fs(_paix.GetAxesMotionOut()),
                    Some: v =>
                    {
                        if (AxisList.Any(ax => v.nCurrentOn[ax] != 1))      // 모터 전류 출력 상태(0=OFF, 1:ON)
                            throw ExceptionWithCode.CreateFatal(ErrorCodes.APP_InternalError, $"Move path {pathName} failed: Current off'ed on some axis.");
                        if (AxisList.Any(ax => v.nServoOn[ax] != 1))        // ServoOn 출력 상태(0=OFF, 1:ON)
                            throw ExceptionWithCode.CreateFatal(ErrorCodes.APP_InternalError, $"Move path {pathName} failed: Servo off'ed on some axis.");
                    },
                    None: () => { });


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

        public override bool ClearAlarm()
        {
            foreach (var axis in AxisList)      //Do not use alarm clear [ audit robot Alarm Clear => Servo off ]
            {
                if (0 != _paix.SetAlarmResetOn(axis, 1))
                    return false;

                Thread.Sleep(500);
                if (0 != _paix.SetAlarmResetOn(axis, 0))
                    return false;
            }

            return true;
        }

        /// returns true if alarm exists and it is cleared
        public override bool ClearAlarmOnDemand()
        {
            var hasAnyAlarm =
                Prelude.match(fs(_paix.GetAxesExpress()),
                    Some: v => AxisList.Any(ax => v.nAlarm[ax] == 1),   // Alarm Sensor 입력 상태(0:OFF, 1:ON)
                    None: () => { throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Paix not responding."); });

            if (hasAnyAlarm)
            {
                LogInfo("Alaram cleared.");
                if (ClearAlarm())
                    return true;

                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Paix failed to clear alarm.");
            }

            return false;
        }


        public override bool BreakServoChange(bool bOn)
        {
            if (!DriverBaseGlobals.IsAudit78())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot parking/unparking only allowed on Audit78.");

            foreach (var axis in AxisList)      //Do not use alarm clear [ audit robot Alarm Clear => Servo off ]
            {
                if ((int)AxisEnumAudit78.Tilt == axis || (int)AxisEnumAudit78.Z == axis)
                    Paix.SetServoOn(axis, Convert.ToInt16(bOn));
            }

            return true;
        }
    }
}
