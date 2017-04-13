using System.Linq;
using System.Xml.Linq;
using Dsu.Driver.Paix;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Dsu.Driver.Util.Emergency;
using LanguageExt;
using static LanguageExt.FSharp;
using Dsu.Driver.Base;

namespace Dsu.Driver.UI.Paix
{
    //internal class Audit78
    //{
    //    public static short[] AllAxesStatic = { 0, 1, 2, 3 };
    //}

    public class AuditPose78 : AuditPose
    {
        public override short[] AllAxes { get { return StaticAllAxes; } }
        public static short[] StaticAllAxes = { 0, 1, 2, 3 };

        public AuditPose78(long v0, long v1, long v2, long v3, string group, string comment, bool checked_, SpeedSpec speedSpec)
            : base(v0, v1, v2, v3, group, comment, checked_, speedSpec)
        {
            if (!DriverBaseGlobals.IsAudit78())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "AuditPose78 should be called in Audit78 tester");
        }


        public override AuditPose Duplicate() => new AuditPose78(V0, V1, V2, V3, Group, Comment, Checked, SpeedSpec.Duplicate());

        public async override Task Goto(Manager manager, CancellationToken token)
        {
            // 원점 센서 확인 여부 점검
            if (!PaixManagerBase.IsOriginCalibrated)
            {
                if (DriverBaseGlobals.IsAudit78() && Group.ToUpper().StartsWith("TOORG"))
                {
                    // allow going to kiss-home
                }
                else
                    throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot origin not calibrated.");
            }

            SetAxesSpeed(manager);
            await Task.Run(async () =>
            {
                var spec = new List<Tuple<short, long>>
                    {
                        Tuple.Create((short)0, V0),
                        Tuple.Create((short)1, V1),
                        Tuple.Create((short)2, V2),
                        Tuple.Create((short)3, V3),
                    };


                var curZ = manager.GetEncPos(2).Value;
                var targetZ = spec[2].Item2;

                var specXYT = new List<Tuple<short, long>>
                    {
                        Tuple.Create((short)0, V0),
                        Tuple.Create((short)1, V1),
                        Tuple.Create((short)2, (long)curZ),
                        Tuple.Create((short)3, V3),
                    };

                _logger.Info($"Going to position {V0}, {V1}, {V2}, {V3} with curZ={curZ}, targetZ={targetZ}");
                if (targetZ >= curZ)     // 위에 있는 상황 => 수평 먼저
                {
                    VerifyMotionController(manager);
                    _logger.Info($"\tMoving horizontally to: {V0}, {V1}, {curZ}, {V3}");
                    manager.MultiAxesMoveAbs(specXYT);
                    await UntilAxesStoppedAsync(manager, token);

                    token.ThrowIfCancellationRequested();
                    VerifyMotionController(manager);
                    _logger.Info($"\tMoving vertically down to : {V0}, {V1}, {targetZ}, {V3}");
                    manager.AbsMove(2, targetZ);
                    await UntilAxesStoppedAsync(manager, token);
                }
                else
                {
                    // 아래 있는 상황 : 위로 먼저 올라감
                    VerifyMotionController(manager);
                    _logger.Info($"\tMoving vertically up to: {V0}, {V1}, {targetZ}, {V3}");
                    manager.AbsMove(2, targetZ);
                    await UntilAxesStoppedAsync(manager, token);

                    token.ThrowIfCancellationRequested();
                    VerifyMotionController(manager);
                    _logger.Info($"\tMoving up horizontally to : {V0}, {V1}, {V2}, {V3}");
                    manager.MultiAxesMoveAbs(spec);
                    await UntilAxesStoppedAsync(manager, token);
                }

                Trace.WriteLine($"Moved to {V0}, {V1}, {V2}, {V3}");
            }, token);
        }
    }


    public static class EmAudit78
    {
        public static bool Check(this SpeedSpec spec, bool isRedZone)
        {
            var factor = isRedZone ? 10 : 1;
            return spec.AxesSpec.ForAll((sp, i) =>
            {
                var max = (i.IsOneOf(0, 1, 2) ? 30000 : 3000) / factor;
                return sp.StartSpeed <= max && sp.Acceleraion <= max && sp.Deceleraion <= max && sp.DriveSpeed <= max;
            });
        }
    }
}
