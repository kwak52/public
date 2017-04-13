using CpTesterPlatform.CpMngLib.Manager;
using Dsu.Driver.Util.Emergency;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reactive.Linq;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using System.Diagnostics;
using CpTesterPlatform.Functions;
using Dsu.Common.Utilities.ExtensionMethods;
using CpTesterSs.Event;
using System.Threading.Tasks;
using Dsu.Driver.Paix;
using static CpBase.CpLog4netLogging;
using Dsu.Common.Utilities;
using Dsu.Driver.Base;

namespace CpTesterPlatform.CpTester
{
    public class CpSignalManager : SignalManager
    {
        private List<CpMngPlc> _plcManagers;
        private List<CpMngDIOControl> _udioManagers;

        public static CpSignalManager TheCpSignalManager { get; private set; }
        private SignalEnum[] _powerSignals;
        private SignalEnum[] _breakSignals;

        static private bool _isColorBit1;
        static private bool _isColorBit2;
        static private bool _isColorBit3;

        static public bool IsColorBlack => _isColorBit1 && (!_isColorBit2 && !_isColorBit3);
        static public bool IsColorYellow => _isColorBit2 && (!_isColorBit1 && !_isColorBit3);
        static public bool IsColorRed => _isColorBit1 && _isColorBit2 && (!_isColorBit3);
        static public bool IsColorGray => _isColorBit3 && (!_isColorBit1 && !_isColorBit2);

        static public bool IsPart7 { get; set; }
        static public bool IsPart8 { get; set; }



        private bool GetSignalValue(SignalEnum signal, bool readFromCache = true)
        {
            var signalSpec = FindKey(signal);
            var tokens = signalSpec.Split(';').ToArray();
            var i = 0;

            if (SignalManager.IsUDIOSignal(signal))
            {
                var type = tokens[i++];
                var index = Int32.Parse(tokens[i++]);
                var deviceId = tokens[i++];
                var dio = _udioManagers.First(m => m.DeviceInfo.Device_ID == deviceId).DioControl;
                var value = readFromCache ? dio.GetDInState(index) : dio.PaixReadPort.GetDIOInputBit((short)index).Value != 0;
                Trace.WriteLine($"SIGNAL {signal}={value}");
                return value;
            }
            else
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, $"Unknown signal enumeration {signal} on configuration file.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partNumber">e.g "9024180021"</param>
        /// <param name="partSpecs">e.g "9024180021=BLACK;9024180022=YELLOW;9024180023=GRAY;9024180024=RED" or with out color, "9024180021;9024180022;9024180023" </param>
        private static bool IsPartMatch(string partNumber, SignalEnum sigEnum, bool colorCheck)
        {
            var parsedSignal = CpSignalManager.GetParsedSignal(sigEnum);
            var partSpecs = parsedSignal.Message;   // e.g "9024180021=BLACK;9024180022=YELLOW;9024180023=GRAY;9024180024=RED"

            var matchSpec = partSpecs.Split(';').FirstOrDefault(s => s.Contains(partNumber));
            if (matchSpec == null)
            {
                ShowErrorFormShortly(SignalEnum.XPartMismatched, "Part mismatch.");
                return false;
            }

            if (!colorCheck)
                return true;

            if (matchSpec.Contains('='))        // need color match
            {
                var cr = matchSpec.Split('=').Skip(1).FirstOrDefault();
                bool bColorOk = false;
                switch (cr.ToLower())
                {
                    case "black": bColorOk = CpSignalManager.IsColorBlack; break;
                    case "yellow": bColorOk = CpSignalManager.IsColorYellow; break;
                    case "gray": bColorOk = CpSignalManager.IsColorGray; break;
                    case "red": bColorOk = CpSignalManager.IsColorRed; break;
                    default:
                        {
                            LogError($"Unknown color specification:{cr}.  Check {FormAppSs.SignalDefinitionFile}.");
                            return false;
                        }
                }

                if (!bColorOk)
                {
                    string CurrentColor = "";
                    if (CpSignalManager.IsColorBlack) CurrentColor = "black";
                    else if (CpSignalManager.IsColorYellow) CurrentColor = "yellow";
                    else if (CpSignalManager.IsColorGray) CurrentColor = "gray";
                    else if (CpSignalManager.IsColorRed) CurrentColor = "red";
                    else
                        CurrentColor = "Unknown";

                    ShowErrorFormShortly(SignalEnum.XPartMismatched, $"Color mismatch Selected Part: {cr} CurrentColor : {CurrentColor}");
                    return false;
                }
            }

            return true;
        }


        public bool IsPartColorMatched(bool colorCheck)
        {
            var partNumber =
                FormAppSs.Stations
                .First(stn => stn.CnfStation.Enable)
                .MngTStep.GaudiReadData.TestListInfo.PartNum
                ;
            var sensed7 = CpSignalManager.IsPart7;
            var sensed8 = CpSignalManager.IsPart8;
            SignalEnum signal = SignalEnum.Undefined;
            if (!sensed7 && !sensed8)
            {
                ShowErrorFormShortly(SignalEnum.XNoPartsLoaded, "No parts loaded.");
                return false;   //  No parts loaded
            }
            else if (sensed7 && sensed8)
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Sensor error. Both part7 and part8 sensed.");

            if (sensed7)
                signal = SignalEnum.UPart7;
            else // if (sensed8)
                signal = SignalEnum.UPart8;

            if (!IsPartMatch(partNumber, signal, colorCheck))
                return false; // Loaded part mismatch.

            return true;
        }

        /// returns whether starable.  if not startable, annotates reason
        private Tuple<bool, string> IsStartable()
        {
            var uEmergency = !GetSignalValue(SignalEnum.UEmergency);
            if (uEmergency)
                return Tuple.Create(false, "Can't start: Emergency stop button pushed.");

            if (FormAppSs.TheMainForm.Starting)
            {
                return Tuple.Create(false, "Can't start: Already running!");
            }

            if ( DriverBaseGlobals.IsAudit() && !PaixManagerBase.IsOriginCalibrated )
                return Tuple.Create(false, "Can't start: Robot home move not executed!");

            if (FormManualRobotAudit.ExistsRobotDialogue)
                return Tuple.Create(false, "Can't start: Close robot manual operation dialog, first.");

            var uStart1 = GetSignalValue(SignalEnum.UStart1);
            var uStart2 = GetSignalValue(SignalEnum.UStart2);
            if (!uStart1 || !uStart2)
            return Tuple.Create(false, "Can't start: Not both of start button pushed.");

            if (_startSum != 2)
                return Tuple.Create(false, "Can't start: Not both of start button pushed.");

            if ( DriverBaseGlobals.IsAudit78() || DriverBaseGlobals.IsLine8FF())
            {
                if (!CpSignalManager.TheCpSignalManager.IsPartColorMatched(colorCheck: false))
                    return Tuple.Create(false, "Can't start: Color not matched.");
            }

            if (DriverBaseGlobals.IsAuditGCVT())
            {
                var error = FormManualRobotAuditGCVT.IsItOK(_udioManagers);
                if (error.NonNullAny())
                    return Tuple.Create(false, error);
            }

            // skip further check when developer mode
            if (Globals.IsDeveloperMode)
                return Tuple.Create(true, "");

            // temparature & humidity check
            var tempaHumid = CpUtil.GetManagerDevice(FormAppSs.TheSystemManager, CpDeviceType.TRIGGER_IO) as CpMngTriggerIO;
            var tempa = tempaHumid.GetTemperature();
            var humid = tempaHumid.GetHumidity();
            var mainForm = FormAppSs.TheMainForm;
            var tempaMin = mainForm.TemperatureMin;
            var tempaMax = mainForm.TemperatureMax;
            var humidMin = mainForm.HumidityMin;
            var humidMax = mainForm.HumidityMax;

            if (!tempa.InClosedRange(tempaMin, tempaMax))
            {
                ShowErrorFormShortly(SignalEnum.XEnvironmentTemperature, "Temperature not acceptable");
                return Tuple.Create(false, $"Can't start: Environment not accepatable. temparature={tempa}, humidity={humid}.");
            }
            else if (!humid.InClosedRange(humidMin, humidMax))
            {
                ShowErrorFormShortly(SignalEnum.XEnvironmentHumidity, "Humidity not acceptable");
                return Tuple.Create(false, $"Can't start: Environment not accepatable. temparature={tempa}, humidity={humid}.");
            }

            var uDoor1Closed = ! GetSignalValue(SignalEnum.UDoor1);
            var uDoor2Closed = ! GetSignalValue(SignalEnum.UDoor2);

            bool uDoor3Closed = true;
            bool uDoor4Closed = true;
            //var uDoor3Closed = !GetSignalValue(SignalEnum.UDoor3);
            //var uDoor4Closed = !GetSignalValue(SignalEnum.UDoor4);

            if (DriverBaseGlobals.IsAudit78() || DriverBaseGlobals.IsLine8FF())
            {
                uDoor3Closed = !GetSignalValue(SignalEnum.UDoor3);
                uDoor4Closed = !GetSignalValue(SignalEnum.UDoor4);
            }
            if ( !uDoor1Closed || !uDoor2Closed || !uDoor3Closed || !uDoor4Closed)
                return Tuple.Create(false, "Can't start: Door is opened.");

            return Tuple.Create(true, "");
        }

        private async void ShowErrorForm(SignalEnum signal, string description)
        {
            await FormAppSs.TheMainForm.DoAsync(() =>
            {
                var errorForm = FormError.TheForm;
                LogError(description);
                if (errorForm == null)
                {
                    errorForm = new FormError(signal, description);
                    errorForm.DoModal();
                }
                else
                {
                    errorForm.Visible = true;
                    errorForm.AddReason(signal, description);
                }
            });
        }

        public static async void ShowErrorFormShortly(SignalEnum signal, string description, int expire=5000)
        {
            await FormAppSs.TheMainForm.DoAsync(() =>
            {
                var errorForm = FormError.TheForm;
                LogError(description);
                if (errorForm == null)
                {
                    errorForm = new FormError(signal, description);
                    if ( expire > 0 )
                        Task.Run(async () => { await Task.Delay(expire); FormError.ClearError(signal); });
                    errorForm.Show();
                }
                else
                {
                    errorForm.Visible = true;

                    //    errorForm.AddReason(signal, description);
                }
            });
        }


        private void ClearError(SignalEnum signal)
        {
            FormError.ClearError(signal);
        }


        private void OnEmergencyStatusChanged(bool emergency)
        {
            IsEmergency = emergency;
            WriteDIO(SignalEnum.ULampRed, emergency);
            WriteDIO(SignalEnum.ULampBuzzer, emergency);
            if (emergency)
            {
                // emergency stop code, here
                ShowErrorForm(SignalEnum.UEmergency, "ERROR: emergency button.");
                FormAppSs.TheMainForm.StopTest();
                //sigManager.PowerOffDevices(); // power off => servo on off error
            }
            else
            {
                // emergency stop cleared
                PowerOnDevices();
                ClearError(SignalEnum.UEmergency);
            }
        }

        private int _startSum = 0;
        /// <summary>
        /// Constructs CpSignalManager
        /// </summary>
        /// <param name="configXmlPath">XML file for signal definition</param>
        /// <param name="deviceManagers">Devices (PLC / UDIO) for value references</param>
        public CpSignalManager(string configXmlPath, IEnumerable<CpMngPlc> plcManagers, IEnumerable<CpMngDIOControl> udioManagers)
            : base(configXmlPath)
        {
            _plcManagers = plcManagers.ToList();
            _udioManagers = udioManagers.ToList();
            TheCpSignalManager = this;
            var sigManager = this;

            /*
             *  초기 시작 불가능 옵션 체크
             * - Emergency button 눌린 경우
             * - Door 개방 상태로 시작한 경우
             */
            var isEmergency = false;
            if (DriverBaseGlobals.IsAudit())
            {
                isEmergency = !GetSignalValue(SignalEnum.UEmergency, readFromCache: false);
                if (isEmergency)
                {
                    // OnEmergencyStatusChanged(true);
                    ShowMessageBox("Emergency button is on.\r\nRestart CP tester after release emergency button.");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }

            if ( !Globals.IsDeveloperMode && DriverBaseGlobals.IsAudit())
            {
                var doors = new[] { SignalEnum.UDoor1, SignalEnum.UDoor2, SignalEnum.UDoor3, SignalEnum.UDoor4, };
                if ( doors.Any(d => GetSignalValue(d, readFromCache: false)) )
                {
                    ShowMessageBox("Door opened.\r\nRestart CP tester after closing door(s).");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }

            if (DriverBaseGlobals.IsAudit78())
            {
                _powerSignals = new[] { SignalEnum.UPower1, SignalEnum.UPower2, SignalEnum.UPower3, SignalEnum.UPower4, SignalEnum.UPower5, };
                _breakSignals = new[] { SignalEnum.UBreakTilt, SignalEnum.UBreakZ, };
            }
            else
            {
                _powerSignals = new[] { SignalEnum.UMotorMC, SignalEnum.UTiltMC, };
                _breakSignals = Enumerable.Empty<SignalEnum>().ToArray();
            }

            RawSignalSubject.Subscribe(async s =>
            {
                SignalEnum signal = SignalEnum.Undefined;
                ParsedSignal parsedSignal = null;
                bool value = false;
                string message = "";
                string comment = "";

                var ioSignal = s as IIOSignal;
                var extraSignal = s as ExtraSignal;
                if (ioSignal != null)
                {
                    var tpl = GetSignalDetails(ioSignal, ioSignal.Address);
                    parsedSignal = tpl.Item1;
                    if (parsedSignal == null)
                        return;         // symbols which are not defined in SignalDefinition.xml

                    message = parsedSignal.Message;
                    signal = parsedSignal.SignalEnum;
                    value = tpl.Item2;
                    comment = parsedSignal.Comment;
                }
                else if (extraSignal != null)
                {
                    signal = extraSignal.Enum;
                    message = extraSignal.Message;
                }
                else
                    Debug.Assert(false);

                FilteredSignalSubject.OnNext(new FilteredSignal(signal, value));

                if (signal != SignalEnum.Undefined && signal != SignalEnum.XUDIOConnected)
                    LogInfo($"Button {signal} push status changed to {value} ({message}, {comment})");

                switch (signal)
                {
                    case SignalEnum.UEmergency:
                        isEmergency = !value;
                        OnEmergencyStatusChanged(isEmergency);
                        break;

                    case SignalEnum.UDoor1:  
                    case SignalEnum.UDoor2:
                    case SignalEnum.UDoor3:
                    case SignalEnum.UDoor4:
                        if (Globals.IsDeveloperMode)
                            break;

                        if (value)
                        {
                            if(SignalEnum.UDoor1 == signal || SignalEnum.UDoor2 == signal) 
                                ShowErrorForm(signal, "ERROR: side door open.");
                            else
                                ShowErrorForm(signal, "ERROR: front door open.");

                            FormAppSs.TheMainForm.StopTest();
                        }
                        else
                            ClearError(signal);

                        break;

                    case SignalEnum.USpare1:
                        ShowErrorFormShortly(signal, "TEST: short message1.");
                        break;
                    case SignalEnum.USpare2:
                        ShowErrorFormShortly(signal, "TEST: short message2.");
                        break;


                    case SignalEnum.XUDIODisconnected:
                        ShowErrorFormShortly(signal, "UDIO disconnected.", 0);
                        break;
                    case SignalEnum.XUDIODisconnectedUnrecoverbly:
                        ShowErrorForm(signal, "UDIO disconnected unrecovably.");
                        break;

                    case SignalEnum.XUDIOConnected:
                        ClearError(SignalEnum.XUDIODisconnected);
                        break;


                    case SignalEnum.UStart1:
                    case SignalEnum.UStart2:
                        if (FormAppSs.TheMainForm.AutoStartPLC)
                            break;  // if PLC is used, we have nothing to do with start button.

                        if (value)
                        {
                            _startSum++;
                            if (_startSum == 2)
                            {
                                var tplStartable = IsStartable();
                                var startable = tplStartable.Item1;
                                var reason = tplStartable.Item2;
                                if (startable)
                                {
                                    // start the system
                                    if (FormAppSs.TheMainForm.AutoMode)
                                    {
                                        LogInfo("Starting the system.");
                                        ExceptionWithCode.IsFatalErrorOccurred = false;
                                        await FormAppSs.TheMainForm.StartTest();
                                    }
                                    else
                                        ShowErrorFormShortly(signal, "Change Auto Mode");
                                }
                                else
                                {
                                    ShowErrorFormShortly(signal, reason);
                                    LogError(reason);
                                }
                            }
                        }
                        else
                            _startSum--;
                        break;

                    case SignalEnum.UOriginSensor:
                        PaixManagerBase.IsOriginCalibratable = value;
                        LogInfo($"Tooltip on near origin = {value}.");
                        break;

                    case SignalEnum.UColorBit1:
                        _isColorBit1 = value;
                        break;
                    case SignalEnum.UColorBit2:
                        _isColorBit2 = value;
                        break;
                    case SignalEnum.UColorBit3:
                        _isColorBit3 = value;
                        break;

                    case SignalEnum.UPart7:
                        CpSignalManager.IsPart7 = value;
                        break;
                    case SignalEnum.UPart8:
                        CpSignalManager.IsPart8 = value;
                        break;

                    default:
                        if (message.NonNullAny())
                        {
                            if (signal.ToString().StartsWith("PMessage"))
                            {
                                if (value)
                                    ShowErrorForm(signal, message);
                                else
                                    ClearError(signal);
                            }
                        }
                        break;
                }
            });
        }


        private void WriteDIO(SignalEnum e, bool value)
        {
            var dios = FormAppSs.GetDeviceManagers<CpMngDIOControl>();

            var signal = new UDIOSignal(FindKey(e), hasValue: false);
            var index = signal.Index;
            var cpuId = signal.DeviceId;
            var dio = dios.First(d => d.DeviceInfo.Device_ID == cpuId);
            if (dio == null)
                LogError($"Failed to find DIO device with ID = {cpuId}.");
            else
            {
                dio.SetDOutState(index, value);
                LogInfo($"Written DIO {e}={value}");
            }
        }

        private void PowerDevices(bool on)
        {
            var dios = CpUtil.GetManagerDevices(FormAppSs.TheSystemManager, CpDeviceType.DIGITAL_IO).Cast<CpMngDIOControl>();

            foreach (var e in _powerSignals)
            {
                var signal = new UDIOSignal(FindKey(e), hasValue: false);
                var index = signal.Index;
                var cpuId = signal.DeviceId;
                var dio = dios.First(d => d.DeviceInfo.Device_ID == cpuId);
                if (dio == null)
                    LogError($"Failed to find DIO device with ID = {cpuId}.");
                else
                {
                    dio.SetDOutState(index, on ? true : false);
                    LogInfo(String.Format("Motors power {0}'ed.", on ? "ON" : "OFF"));
                }
            }
        }
        public void PowerOnDevices() => PowerDevices(true);
        public void PowerOffDevices() => PowerDevices(false);


        private void BreakDevices(bool on)
        {
            var dios = CpUtil.GetManagerDevices(FormAppSs.TheSystemManager, CpDeviceType.DIGITAL_IO).Cast<CpMngDIOControl>();

            foreach (var e in _breakSignals)
            {
                var signal = new UDIOSignal(FindKey(e), hasValue: false);
                var index = signal.Index;
                var cpuId = signal.DeviceId;
                var dio = dios.First(d => d.DeviceInfo.Device_ID == cpuId);
                if (dio == null)
                    LogError($"Failed to find DIO device with ID = {cpuId}.");
                else
                {
                    dio.SetDOutState(index, on ? true : false);
                    LogInfo(String.Format("Break power {0}'ed.", on ? "ON" : "OFF"));
                }
            }
        }
        public void BreakDevicesOn() => BreakDevices(false);
        public void BreakDevicesOff() => BreakDevices(true);


        private static CpMngDIOControl GetDioControl(ParsedSignal parsedSignalT, IEnumerable<CpMngDIOControl> dios)
        {
            var dio = dios.FirstOrDefault(d => d.DeviceInfo.Device_ID == parsedSignalT.DeviceId);
            if (dio == null)
                throw ExceptionWithCode.Create(ErrorCodes.APP_ConfigurationError, $"Can't find DIO point {parsedSignalT.DeviceId} ({parsedSignalT.Message}).");

            return dio;
        }
        public static bool GetDio(SignalEnum sigEnum, IEnumerable<CpMngDIOControl> dios) => GetDio(SignalManager.GetParsedSignal(sigEnum), dios);
        public static bool GetDio(SignalEnum sigEnum, CpMngDIOControl dio) => GetDio(SignalManager.GetParsedSignal(sigEnum), dio);
        public static bool GetDio(ParsedSignal parsedSignalT, IEnumerable<CpMngDIOControl> dios) => GetDio(parsedSignalT, GetDioControl(parsedSignalT, dios));

        public static bool GetDio(ParsedSignal parsedSignalT, CpMngDIOControl dio)
        {
            var index = int.Parse(parsedSignalT.Address);
            switch (parsedSignalT.Type)
            {
                case "DI": return dio.GetDInState(index);
                case "DO": return dio.GetDOutState(index);
                default:
                    throw ExceptionWithCode.Create(ErrorCodes.APP_ConfigurationError, $"Unknown DIO type {parsedSignalT.Type}.");
            }
        }

        private static void SetDio(ParsedSignal parsedSignalT, bool bOn, IEnumerable<CpMngDIOControl> dios, bool bWriteAlways)
        {
            var dio = GetDioControl(parsedSignalT, dios);
            SetDio(parsedSignalT, bOn, dio, bWriteAlways);
        }
        private static void SetDio(ParsedSignal parsedSignalT, bool bOn, CpMngDIOControl dio, bool bWriteAlways)
        {
            if (bWriteAlways || GetDio(parsedSignalT, dio) != bOn)
                dio.SetDOutState(int.Parse(parsedSignalT.Address), bOn);
        }
        public static void SetDioOn(SignalEnum sigEnum, IEnumerable<CpMngDIOControl> dios, bool bWriteAlways = true) => SetDio(SignalManager.GetParsedSignal(sigEnum), true, dios, bWriteAlways);
        public static void SetDioOff(SignalEnum sigEnum, IEnumerable<CpMngDIOControl> dios, bool bWriteAlways = true) => SetDio(SignalManager.GetParsedSignal(sigEnum), false, dios, bWriteAlways);
    }
}
