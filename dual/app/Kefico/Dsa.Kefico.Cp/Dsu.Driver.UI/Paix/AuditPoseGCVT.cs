using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dsu.Driver.Paix;
using Dsu.Driver.Base;
using Dsu.Driver.Util.Emergency;
using System.Diagnostics;

namespace Dsu.Driver.UI.Paix
{
    public class AuditPoseGCVT : AuditPose
    {
        public override short[] AllAxes { get { return StaticAllAxes; } }
        public static short[] StaticAllAxes = { 1, 2, 3 };

        public AuditPoseGCVT(long v1, long v2, long v3, string group, string comment, bool checked_, SpeedSpec speedSpec)
            : base(0, v1, v2, v3, group, comment, checked_, speedSpec)
        {
            if (!DriverBaseGlobals.IsAuditGCVT())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "AuditPoseGCVT should be called in Audit GCVT tester");
        }

        public override AuditPose Duplicate() => new AuditPoseGCVT(V1, V2, V3, Group, Comment, Checked, SpeedSpec.Duplicate());


        public override async Task Goto(Manager manager, CancellationToken token)
        {
            // 원점 센서 확인 여부 점검
            //Debug.Assert(false);
            //if (!PaixManagerBase.IsOriginCalibrated)
            //    throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot origin not calibrated.");

            SetAxesSpeed(manager);
            await Task.Run(async () =>
            {
                var spec = new List<Tuple<short, long>>
                    {
                        //Tuple.Create((short)0, V0),       // NO zero axis!!!
                        Tuple.Create((short)1, V1),
                        Tuple.Create((short)2, V2),
                        Tuple.Create((short)3, V3),
                    };

                VerifyMotionController(manager);
                _logger.Info($"\tMoving to: {V1}, {V2}, {V3}");
                manager.MultiAxesMoveAbs(spec);
                await UntilAxesStoppedAsync(manager, token);

                System.Diagnostics.Trace.WriteLine($"Moved to {V1}, {V2}, {V3}");
            }, token);
        }
    }
}
