using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using LanguageExt;
using System.Windows.Forms;
using Dsu.Driver.Util.Emergency;
using Dsu.Driver.Base;
using static LanguageExt.FSharp;
using static Dsu.Driver.PaixMotionControler.NMC2;
using static CpBase.CpLog4netLogging;
using Dsu.Common.Utilities.Core.ExtensionMethods;
using EmLinq = Dsu.Common.Utilities.ExtensionMethods.EmLinq;

namespace CpTesterPlatform.CpDevices
{
    partial class CpDevPaixRobot
    {
        private async Task AwaitMotionFinish(CancellationToken cancelToken, params short[] axes)
        {
            while (!_paix.IsAxesStop(axes))
            {
                cancelToken.ThrowIfCancellationRequested();
                await Task.Delay(100);
            }

            // stop info check
            var succeeded =
                Prelude.match(fs(_paix.GetStopInfo()),
                    Some: v => axes.ForAll(ax => v[ax] != 1), // 1 : stop by emergency, pp.189
                    None: () => false);
            if (!succeeded)
                throw ExceptionWithCode.Create(ErrorCodes.USR_OperationCancel,
                    "Waiting motion finish failed by user operatino cancel request.");
        }

        /// Home move 수행을 종료한 시점에 cmd 와 enc 의 오차가 1000 이내일 때에 성공으로 간주
        /// return : every failed axis list.  if all finished without large error, return empty
        private async Task<bool> AwaitHomeMoveFinish(CancellationToken cancelToken, params short[] axes)
        {
            // awaits until completed.
            LogInfoRobot("Waiting home move finish.");
            var linkedToken = cancelToken.CreateLinkedTimeoutTokenSource(TimeSpan.FromMinutes(3)).Token;  // 3 minutes time budget
            while (true)
            {
                // pp. 144
                // nSrchFlag : 0=원점이동완료, 1=원점이동중
                // nStatusFlag : 0 = 원점이동정상완료, 1=원점이동중, 2=사용자에의한중지, 3=원점이동미실행, 4=비상정지, 5=알람정지, 6=LimitOn(2차), 7=LimitOn(3차), 8=+-limit동시ON
                var finished =
                    Prelude.match(fs(_paix.GetHomeStatus()),
                        Some: v => axes.ForAll(ax => v.nSrchFlag[ax] == 0 && v.nStatusFlag[ax] == 0),
                        None: () => false);


                linkedToken.ThrowIfCancellationRequested();

                if (finished)
                {
                    // check whether normally stopped after successful home move.  if any case of alarm, error, ... it fails
                    var succeeded =
                        Prelude.match(fs(_paix.GetStopInfo()),
                            Some: v => axes.ForAll(ax => v[ax] == 6),       // 6 : stop by Z enc, pp.189
                            None: () => false);
                    if (!succeeded)
                        throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Failed to move home.");

                    int n = 0;
                    // waits until InPosition value on.
                    while (!Prelude.match(fs(_paix.GetAxesExpress()),
                                Some: v => axes.ForAll(ax => v.nInpo[ax] == 1),
                                None: () => false))
                    {
                        await Task.Delay(100);
                        if (n++ > 200)     // 20 seconds max
                            throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Failed to move home : InPosition not confirmed.");
                    }

                    LogInfoRobot("Finished robot home move.");
                    return true;        // success
                }


                await Task.Delay(100);
            }
        }


        public static bool HomeMoved { get; set; } = false;
        public static bool KissOriginMoved { get; set; } = false;


        // BitOR 0x80 means : After move home, extracts Z (ZOC, Zone of control)
        // 2 : +Near
        // 3 : -Near
        private short GetHomeMode(bool cw) => (short)(0x80 | (cw ? 0x03 : 0x02));

        private Dictionary<short, double> _accelerationBackcup;
        private Dictionary<short, double> _decelerationBackcup;
        private void HomeMovePrologue(IEnumerable<short> axes)
        {
            HomeMoved = false;
            var realAxes = axes ?? AxisList;

            // Home move 수행 시, alarm clear 는 하지 않는 것으로 한다.   강제 alarm clear 시 문제 발생.
            // alarm 존재 시, 기기 재부팅 유도
            if (_paix.AlaramedAxes(realAxes).Any())
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, $"Alaram not cleared.  Requires tester reboot.");

            TryClearErrorBeforeHomeMove(realAxes);

            var errorMessage = CheckErrorStatusBeforeHomeMove(realAxes);
            if (errorMessage.Any())
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, $"ERROR on robot: {errorMessage.Aggregate((agg, next) => next + agg)}");

            LogInfoRobot("Backup Acc/Dec speed.");
            // 가감속 backup
            _accelerationBackcup = new Dictionary<short, double>();
            _decelerationBackcup = new Dictionary<short, double>();
            foreach (var ax in realAxes)
            {
                var res = _paix.GetParaSpeed(ax).Value;
                _accelerationBackcup[ax] = res.dAcc;
                _decelerationBackcup[ax] = res.dDec;
                _paix.SetAccSpeed(ax, 10000);
                _paix.SetDecSpeed(ax, 10000);
            }
        }

        /// 정상/비정상 모든 경우에 호출됨.
        private void HomeMoveEpilogue(IEnumerable<short> axes)
        {
            var realAxes = axes ?? AxisList;
            // 비정상 상황에서 호출 될 수 있으므로 HomeMoved 값을 true 로 설정하는 것은 외부에서 정상 종료 상황일 때 해야 한다.
            LogInfoRobot("Recovering Acc/Dec speed.");
            foreach (var ax in realAxes)
            {
                _paix.SetAccSpeed(ax, _accelerationBackcup[ax]);
                _paix.SetDecSpeed(ax, _decelerationBackcup[ax]);
            }
        }

        public override Task<bool> HomeMove(CancellationToken cancelToken, bool resetCmdEncPosition)
        {
            if (DriverBaseGlobals.IsAudit78())
                return HomeMoveAudit78(cancelToken, resetCmdEncPosition);
            else
                return HomeMoveAuditGCVT(cancelToken, resetCmdEncPosition);
        }

        /// <summary>
        /// Tilt 축만 HomeMove 수행한다.
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        private async Task<bool> HomeMoveAuditGCVT(CancellationToken cancelToken, bool resetCmdEncPosition)
        {
            var theAxis = (short)AxisEnumAuditGCVT.Tilt;
            var theAxes = new[] { theAxis };
            HomeMovePrologue(theAxes);

            try
            {
                // pp. 124, NMC2-Library-Manual.pdf
                const short plusNear = 0x2;
                const short extractZ = 0x80;    // BitOR 0x80 means : After move home, extracts Z (ZOC, Zone of control)

                SetHomeDelay(theAxis, 0);
                // Default speed should be exists (비록 0일지라도).
                _paix.SetSpeed(theAxis, 0, 0, 0, 0);

               _paix.SetHomeSpeedAccDec(theAxis,
                    1500, 1, 30000, 30000,     // homespeed0, start, acc, dec
                    750, 1, 30000, 30000,      // homespeed1, start, acc, dec
                    500, 1, 30000, 30000,      // homespeed2, start, acc, dec
                    0, 0, 0, 0);               // offsetspeed, start, acc, dec

                const short homeEndMode = 0x0;      // do nothing
                //const short homeEndMode = 0xC;    // 0xC => Reset CMD ENC after Home, and before offset movement


                if (cancelToken.IsCancellationRequested) return false;

                // 1. Tilt axis home move
                _paix.HomeMove(theAxis, extractZ | plusNear, homeEndMode, offset: 0, reserve: 0);
                bool succeeded = await AwaitHomeMoveFinish(cancelToken, theAxis);
                cancelToken.ThrowIfCancellationRequested();

                if (!succeeded)
                {
                    LogErrorRobot($"Failed to move home.");
                    throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Home set error");
                }

                LogInfoRobot("Tilt axis home move finished.");

                // check if stopped by Z enc
                var stopInfo = _paix.GetStopInfo().Value;
                bool stopStatusOK = stopInfo[theAxis] == 6;    // 0 : Normal stop, 6 : Encoder Z stop
                if (!stopStatusOK)
                    throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Not Encorder Z stopped.");

                await Task.Delay(3000);
                LogInfoRobot("All axes move home finished.");
                HomeMoved = true;

                if (resetCmdEncPosition)
                {
                    // Reset Cmd/Enc value to zero at home position 
                    SetCommandEncorderZeroSetting(theAxes);

                    VerifiyCommandEncoderZero(theAxes, 200);     // allow upto 200 pulse error
                }

                return true;
            }
            catch (Exception ex)
            {
                _paix.EmergencyStop();
                foreach (var ax in theAxes)
                    _paix.HomeDoneCancel(ax);

                var message = $"Exception occurred while SetHome().  Stop servo requested.\r\n{ex}";
                LogErrorRobot(message);
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, message);
            }
            finally
            {
                HomeMoveEpilogue(theAxes);
            }
        }




        /// <summary>
        /// 1. Z axis up
        /// 2. X, Y, Z move to near limit
        /// 3. Reset cmd/enc into all zero, there.
        /// 4. 
        /// </summary>
        private async Task<bool> HomeMoveAudit78(CancellationToken cancelToken, bool resetCmdEncPosition)
        {
            HomeMovePrologue(null);

            try
            {
                // pp. 124, NMC2-Library-Manual.pdf
                const short plusNear = 0x2;
                //const short minusNear = 0x3;
                const short extractZ = 0x80;    // BitOR 0x80 means : After move home, extracts Z (ZOC, Zone of control)

                const double homeSpeed0 = 7500;    // 1st speed
                //const double homeSpeed1 = 1000;    // 2nd speed
                //const double homeSpeed2 = 100;     // 3rd speed         // tilt = 50

                foreach (var ax in AxisList)
                    SetHomeDelay(ax, 0);

                // Default speed should be exists (비록 0일지라도).
                _paix.SetSpeed((short)AxisEnumAudit78.X, 0, 0, 0, 0);
                _paix.SetSpeed((short)AxisEnumAudit78.Y, 0, 0, 0, 0);
                _paix.SetSpeed((short)AxisEnumAudit78.Z, 0, 0, 0, 0);
                _paix.SetSpeed((short)AxisEnumAudit78.Tilt, 0, 0, 0, 0);

                foreach (var axis in new[] { (short)AxisEnumAudit78.X, (short)AxisEnumAudit78.Y, (short)AxisEnumAudit78.Z })
                {
                    var realHomeSpeed0 = axis == (short)AxisEnumAudit78.Tilt ? 5000 : homeSpeed0;
                    _paix.SetLimitStopMode(axis, 0);         // MUST: enable emergency stop mode
                                                             //_paix.SetHomeSpeed(axis, realHomeSpeed0, homeSpeed1, homeSpeed2);
                    _paix.SetHomeSpeedAccDec(axis,
                        15000, 1, 30000, 30000,     // homespeed0, start, acc, dec
                        10000, 1, 30000, 30000,      // homespeed1, start, acc, dec
                        2000, 1, 100000, 100000,          // homespeed2, start, acc, dec
                        0, 0, 0, 0);                // offsetspeed, start, acc, dec
                }
                _paix.SetHomeSpeedAccDec((short)AxisEnumAudit78.Tilt,
                    1500, 1, 30000, 30000,     // homespeed0, start, acc, dec
                    750, 1, 30000, 30000,      // homespeed1, start, acc, dec
                    500, 1, 30000, 30000,          // homespeed2, start, acc, dec
                    0, 0, 0, 0);                // offsetspeed, start, acc, dec

                const short homeEndMode = 0x0;      // do nothing
                //const short homeEndMode = 0xC;    // 0xC => Reset CMD ENC after Home, and before offset movement


                if (cancelToken.IsCancellationRequested) return false;

                // 1. Z axis move upper limit
                _paix.HomeMove((short)AxisEnumAudit78.Z, extractZ | plusNear, homeEndMode, offset: 0, reserve: 0);
                bool succeeded = await AwaitHomeMoveFinish(cancelToken, (short)AxisEnumAudit78.Z);
                cancelToken.ThrowIfCancellationRequested();

                if (!succeeded)
                {
                    LogErrorRobot($"Failed to move home.");
                    throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Home set error");
                }

                LogInfoRobot("Z move up finished.");
                // 2. X, Y move to h/w near sensor and set that position as origin
                foreach (var tpl in _axisWithDirList.Where(tpl => tpl.Item1 == (short)AxisEnumAudit78.X || tpl.Item1 == (short)AxisEnumAudit78.Y || tpl.Item1 == (short)AxisEnumAudit78.Tilt))
                {
                    if (cancelToken.IsCancellationRequested) return false;

                    var axis = tpl.Item1;
                    LogInfoRobot($"Axis{axis} move home initiated.");
                    _paix.HomeMove(axis, extractZ | plusNear, homeEndMode, offset: 0, reserve: 0);  //homeEndMode 0xC => Reset CMD ENC after Home, and before offset movement
                }

                cancelToken.ThrowIfCancellationRequested();

                // awaits until completed.
                await AwaitHomeMoveFinish(cancelToken, AxisList.ToArray());

                // check if stopped by Z enc
                var stopInfo = _paix.GetStopInfo().Value;
                bool stopStatusOK = AxisList.ForAll(ax => stopInfo[ax] == 6);    // 0 : Normal stop, 6 : Encoder Z stop
                if (!stopStatusOK)
                    throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Not Encorder Z stopped.");

                await Task.Delay(3000);
                LogInfoRobot("All axes move home finished.");
                HomeMoved = true;

                if (resetCmdEncPosition)
                {
                    // Reset Cmd/Enc value to zero at home position 
                    SetCommandEncorderZeroSetting(null);

                    VerifiyCommandEncoderZero(null, 200);     // allow upto 200 pulse error
                }

                return true;
            }
            catch (Exception ex)
            {
                _paix.EmergencyStop();
                foreach (var ax in AxisList)
                    _paix.HomeDoneCancel(ax);

                var message = ex is OperationCanceledException ? "Homemove Operation canceled." : $"Exception occurred while SetHome().  Stop servo requested.\r\n{ex}";
                LogErrorRobot(message);
                throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, message);
            }
            finally
            {
                HomeMoveEpilogue(null);
            }
        }
    }
}
