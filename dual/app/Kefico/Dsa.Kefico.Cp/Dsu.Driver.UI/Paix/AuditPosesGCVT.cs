using Dsu.Driver.Base;
using Dsu.Driver.Util.Emergency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.Driver.UI.Paix
{
    public class AuditPosesGCVT : AuditPoses
    {
        public override short[] AllAxes { get { return new short[] { 1, 2, 3 }; } }     // No zero axis!!!

        public AuditPosesGCVT() { }
        public AuditPosesGCVT(IEnumerable<AuditPoseGCVT> poses)
            : base(poses)
        {
            if (!DriverBaseGlobals.IsAuditGCVT())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "AuditPosesGCVT should be called in Audit GCVT tester");
        }
    }
}
