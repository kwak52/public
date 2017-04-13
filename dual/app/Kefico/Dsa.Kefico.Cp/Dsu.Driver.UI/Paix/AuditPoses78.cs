using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Dsu.Driver.Paix;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.Util.Emergency;
using Dsu.Driver.Base;

namespace Dsu.Driver.UI.Paix
{
    public class AuditPoses78 : AuditPoses
    {
        public override short[] AllAxes { get { return new short[] { 0, 1, 2, 3 }; } }

        public AuditPoses78() { }
        public AuditPoses78(IEnumerable<AuditPose78> poses)
            :base(poses)
        {
            if (!DriverBaseGlobals.IsAudit78())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "AuditPoses78 should be called in Audit78 tester");
        }

        public void MarkAllPosesChecked() => this.Iter(p => p.Checked = true);
    }
}
