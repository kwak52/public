using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterSs.UserControl;
using CpTesterPlatform.CpTester;
using System;
using Dsu.Driver.Base;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.Functions;
using static CpBase.CpLog4netLogging;
using System.Linq;

namespace CpTesterPlatform.CpTesterSs
{
    /// <summary>
    /// To Build Tester-Conditions before a Test Starts.
    /// Example: Initialize Devices
    /// </summary>
    class TaskSgPreTask
    {
        public static async Task<CpProcessEndStatus> taskSgPreTest(UcMainViewSs xOwner, CancellationTokenSource token)
        {
            Stopwatch xSW = new Stopwatch();
            xSW.Start();

            foreach (KeyValuePair<string, CpDeviceManagerBase> hwMng in xOwner.TheSystemManager.MngHardware.DicDeviceManager)
            {
                IDevManager mgrDevice = hwMng.Value as IDevManager;
                if (!mgrDevice.InitManager())
                    return new CpProcessEndStatus(CpSystemStatus.STOP, xSW.ElapsedMilliseconds, 0);

                await Task.Delay(1);
            }
            if (FormAppSs.TheMainForm.AutoStartPLC)
            {
                string TTNR = xOwner.PlcIF.GetProductId();
                if (TTNR.Length != 10)
                {
                    FormAppSs.TheMainForm.ShowErrorFormShortly(string.Format("{0} : The model number must be 10 characters.", TTNR));
                    return new CpProcessEndStatus(CpSystemStatus.SKIP, xSW.ElapsedMilliseconds, 0);
                }

                if (xOwner.MngStation.MngTStep.GaudiReadData.TestListInfo.PartNum != TTNR)
                    xOwner.TheMainForm.UpdateMainLabelText();

                xOwner.MngStation.MngTStep.GaudiReadData.TestListInfo.PartNum = TTNR;
                xOwner.MngStation.MngTStep.GaudiReadData.ApplyTTNRInTestStep(TTNR);

                if (xOwner.TheApplication.Station.First().StationId == xOwner.StationID)
                    xOwner.PlcIF.UpdateMESPartId();
            }
            else
            {
                if (DriverBaseGlobals.IsAudit78())
                    await FormManualRobotAudit78.UnparkingAudit78();

                if (await RunAuditModeSkippable(xOwner, xSW, token))
                {
                    if (DriverBaseGlobals.IsAudit78())
                        await FormManualRobotAudit78.ParkingAudit78();
                    return new CpProcessEndStatus(CpSystemStatus.STOP, xSW.ElapsedMilliseconds, 0);
                }

                var nRepeat = Convert.ToInt32(xOwner.MngStation.CnfTestCondition.RepeatNum);
                var nCurrent = xOwner.MngStation.TsCount + xOwner.MngStation.NgCount;
                if (nRepeat <= nCurrent)
                {
                    xOwner.MngStation.TsCount = 0;
                    xOwner.MngStation.NgCount = 0;
                }
            }

            return new CpProcessEndStatus(CpSystemStatus.OK, xSW.ElapsedMilliseconds, 0);
        }


        private static async Task<bool> RunAuditModeSkippable(UcMainViewSs xOwner, Stopwatch xSW, CancellationTokenSource token)
        {
            const bool skip = true;
            const bool goOn = false;

            xOwner.ManualStop = false;
            var ps = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(xOwner.TheSystemManager, CpDeviceType.MOTION), true);
            if (ps == null)
                return skip;
            else
            {
                if (DriverBaseGlobals.IsAudit78())
                {
                    if (CpSignalManager.IsPart8)
                    {
                        // check color after moving color check position.
                        // if mismatch, return NG
                        LogInfoRobot("Moving to color check point..");
                        if ( ! await ((CpMngMotion)ps[0]).MovePath("ReadyToColorChk", token.Token) )
                        {
                            LogInfoRobot("Failed to move color check point..");
                            return skip;
                        }
                        await Task.Delay(1000);
                        LogInfoRobot("\tFinished Moving to color check point.");


                        // color match 실패하더라도 ready position 으로 다시 오게 해야 한다.
                        bool PartColorMatched = CpSignalManager.TheCpSignalManager.IsPartColorMatched(colorCheck: true);

                        LogInfoRobot("Moving to ready point..");
                        var readMoveSucceeded = await ((CpMngMotion)ps[0]).MovePath("ColorChkToReady", token.Token);
                        LogInfoRobot((readMoveSucceeded ? "\tFinished" : "\tFailed") + " Moving to ready point.");

                        return !PartColorMatched || !readMoveSucceeded;
                    }
                }
                else if (DriverBaseGlobals.IsAuditGCVT())
                {
                    //kwak, todo. needs part check
                    return goOn;
                }

                return goOn;
            }
        }

    }
}
