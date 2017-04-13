using Dsu.Driver.Paix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Xml.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.Util.Emergency;
using System.Threading;
using LanguageExt;
using static LanguageExt.FSharp;

namespace Dsu.Driver.UI.Paix
{
    public abstract class AuditPose : Dsu.Driver.Paix.Pose4
    {
        protected ILog _logger = Dsu.Driver.UI.Logging.Logger;
        /// Generated column for button edit cell
        public string Go { get; } = "";

        public abstract short[] AllAxes { get; }


        protected AuditPose(long v0, long v1, long v2, long v3, string group, string comment, bool checked_, SpeedSpec speedSpec)
            : base(v0, v1, v2, v3, group, comment, checked_, speedSpec)
        {
        }

        public abstract AuditPose Duplicate();

        public abstract Task Goto(Manager manager, CancellationToken token);


        public XElement SpeedToXml()
        {
            return new XElement("AxisSpecs",
            SpeedSpec.AxesSpec.Select((a, i) =>
               new XElement("Axis",
                   new XAttribute("Number", i),
                   new XAttribute("StartSpeed", a.StartSpeed),
                   new XAttribute("Acceleraion", a.Acceleraion),
                   new XAttribute("Deceleraion", a.Deceleraion),
                   new XAttribute("DriveSpeed", a.DriveSpeed))));
        }

        protected void SetAxesSpeed(Manager manager)
        {
            //if (!SpeedSpec.Check(isRedZone: false))     // TODO: 일단은 red zone 아닌 걸로 적용... 윗단에서 isRedZone 판정해서 넘겨주어야..
            //{
            //    MessageBox.Show($"You're over speeding: {SpeedSpec}.");
            //    return;
            //}

            AllAxes.ForEach(ax =>
            {
                var sp = SpeedSpec.AxesSpec[ax];
                manager.SetSpeed(ax, sp.StartSpeed, sp.Acceleraion, sp.Deceleraion, sp.DriveSpeed);
            });
        }

        protected async Task UntilAxesStoppedAsync(Manager manager, CancellationToken token)
        {
            while (!manager.IsAxesStop(AllAxes) && !token.IsCancellationRequested)
                await Task.Delay(30);
        }

        protected void VerifyMotionController(Manager manager)
        {
            //// Emergency stop 에 의한 error 를 강제로 clear 하기 위해서 조금 움직임
            //Prelude.match(fs(manager.GetAxesExpress()),
            //    Some: v =>
            //    {
            //        var erroneousAxes = AllAxes.Where(ax => v.nError[ax] == 1).ToArray();
            //        if ( erroneousAxes.Any() )
            //        {
            //            AllAxes.Where(ax => v.nError[ax] == 1).ForEach(ax =>
            //            {
            //                manager.RelMove(ax, 1);
            //                Thread.Sleep(50);
            //                manager.RelMove(ax, -1);
            //            });

            //            // waits until not busy status.
            //            Thread.Sleep(500);
            //        }
            //    },
            //    None: () => { });

            Prelude.match(fs(manager.GetAxesExpress()),
                Some: v =>
                {
                    if (AllAxes.Any(ax => v.nBusy[ax] == 1))   // BUSY : Pulse 출력 상태(0:Idle, 1:Busy)
                        throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "Not all axes stopped.");

                    /* restart 시 오류는 무시한다 => limit, emergency stop,  */
                    //if (AllAxes.Any(ax => v.nError[ax] == 1))  // Error 발생 여부(0:None error, 1:Error)
                    //    throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "There is an error on axes.");

                    if (AllAxes.Any(ax => v.nAlarm[ax] == 1))  // Alarm Sensor 입력 상태(0:OFF, 1:ON)
                        throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "There is an error on axes (require alarm reset).");
                    if (AllAxes.Any(ax => v.nSReady[ax] == 1))  // Servo Ready 입력 상태(0:OFF, 1:ON)
                        throw ExceptionWithCode.Create(ErrorCodes.DEV_PaixError, "There is an error on axes (require servo ready).");
                },
                None: () => { });
        }
    }
}
