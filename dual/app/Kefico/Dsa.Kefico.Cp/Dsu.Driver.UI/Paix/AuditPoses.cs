using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.Paix;
using Dsu.Driver.Util.Emergency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using Dsu.Driver.Base;
using static LanguageExt.Prelude;
using static LanguageExt.FSharp;

namespace Dsu.Driver.UI.Paix
{
    public abstract class AuditPoses : List<AuditPose>
    {
        public abstract short[] AllAxes { get; }

        protected AuditPoses() { }
        protected AuditPoses(IEnumerable<AuditPose> poses)
        {
            AddRange(poses);
        }

        /// Wait until previous motion fully stopped.
        protected async Task UntilAxesStoppedAsync(Manager manager, CancellationToken token)
        {
            await Task.Delay(0);
            //while (!manager.IsAxesStop(AllAxes) && !token.IsCancellationRequested)
            //{
            //    await Task.Delay(100);
            //}

            //match(fs(manager.GetStopInfo()),
            //    Some: stopInfo =>
            //    {
            //        bool stopStatusOK = AllAxes.ForAll(ax => stopInfo[ax].IsOneOf((short)0, (short)1, (short)6));    // 0=Normal stop, 1=Emergency stop, 6=Encoder Z stop
            //        if (!stopStatusOK)
            //            throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Not normally stopped.");

            //    },
            //    None: () =>
            //    {
            //        //kwak, token.Cancel();
            //        throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Paix not connected.");
            //    });
        }

        /// Checked 된 여러 pose 들을 따라서 움직임.
        public async Task PathMove(Manager manager, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                await UntilAxesStoppedAsync(manager, token);

                var poses = this.Where(p => p.Checked);
                foreach (var p in poses)
                {
                    token.ThrowIfCancellationRequested();

                    await p.Goto(manager, token);
                    await UntilAxesStoppedAsync(manager, token);
                }
            }, token);
        }


        /// (확장자 .poses 를 갖는) xml file 에서 pose sequence 를 읽어서 Audit78Poses 객체를 생성해서 반환
        public static AuditPoses LoadFromXml(string file)
        {
            XDocument doc = XDocument.Load(file);
            var poses = from poseXml in doc.Descendants("Poses").Elements("Pose")
                        let att = poseXml.Attributes().ToDictionary(at => at.Name.ToString(), at => at.Value)
                        let grp = att["Group"]
                        let comment = att["Comment"]
                        let v0 = Int64.Parse(att["V0"])
                        let v1 = Int64.Parse(att["V1"])
                        let v2 = Int64.Parse(att["V2"])
                        let v3 = Int64.Parse(att["V3"])
                        let chk = Boolean.Parse(att["Checked"])        // check status is only valid in TEACHING UI.
                        let axesSpecs =
                            from axXml in poseXml.Descendants("AxisSpecs").Elements("Axis")
                            let att = axXml.Attributes().ToDictionary(at => at.Name.ToString(), at => at.Value)
                            let speed = Int64.Parse(att["StartSpeed"])
                            let acceleraion = Int64.Parse(att["Acceleraion"])
                            let deceleraion = Int64.Parse(att["Deceleraion"])
                            let maxSpeed = Int64.Parse(att["DriveSpeed"])
                            select new AxisSpec(speed, acceleraion, deceleraion, maxSpeed)
                        let speedSpec = new SpeedSpec(axesSpecs)
                        let pose = DriverBaseGlobals.IsAudit78() ? (AuditPose)new AuditPose78(v0, v1, v2, v3, grp, comment, chk, speedSpec) : (AuditPose)new AuditPoseGCVT(v1, v2, v3, grp, comment, chk, speedSpec)
                        select pose
                        ;

            if (DriverBaseGlobals.IsAudit78())
                return new AuditPoses78(poses.Cast<AuditPose78>());
            else
                return new AuditPosesGCVT(poses.Cast<AuditPoseGCVT>());
        }


        /// (확장자 .node 를 갖는) PAIX file 에서 pose sequence 를 읽어서 Audit78Poses 객체를 생성해서 반환
        public static AuditPoses LoadFromPaix(string file)
        {

            if (!DriverBaseGlobals.IsAudit78())
                throw new NotSupportedException("Load from PAIX only supported on Audit78 tester.");

            var poses = from line in File.ReadLines(file).Skip(2)
                        let items = line.Split(new[] { ',' }).ToArray()
                        let n = items[0]       // 축번호
                        let f = items[1]       // "3직선"
                        let x = Int64.Parse(items[2])
                        let y = Int64.Parse(items[3])
                        let z = Int64.Parse(items[4])
                        let ss = Int64.Parse(items[5])     // 시작
                        let ac = Int64.Parse(items[6])     // 가속
                        let dc = Int64.Parse(items[7])     // 감속
                        let ds = Int64.Parse(items[8])     // 구동
                        let io = items[9]
                        let axisSpec = new AxisSpec(ss, ac, dc, ds)
                        let tiltingAxisSpec = new AxisSpec(1000, 1000, 1000, 1000)
                        let speedSpec = new SpeedSpec(new[] { axisSpec.Duplicate(), axisSpec.Duplicate(), axisSpec.Duplicate(), tiltingAxisSpec })
                        select new AuditPose78(x, y, z, 0, "", "", false, speedSpec)
                    ;
            return new AuditPoses78(poses);
        }

        public static XElement PosesToXml(IEnumerable<AuditPose> poses)
        {
            return new XElement("Poses",
                from p in poses
                select new XElement("Pose",
                    new XAttribute("Group", p.Group),
                    new XAttribute("Comment", p.Comment),
                    new XAttribute("V0", p.V0),
                    new XAttribute("V1", p.V1),
                    new XAttribute("V2", p.V2),
                    new XAttribute("V3", p.V3),
                    new XAttribute("Checked", p.Checked),    // check status is only valid in TEACHING UI.  it won't be serialized.
                    p.SpeedToXml()));
        }

    }
}
