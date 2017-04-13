using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterSs.UserControl;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpApplication.Configure;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using static CpCommon.ExceptionHandler;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTester;
using CpTesterPlatform.Functions;
using Dsu.Driver.Base;

namespace CpTesterPlatform.CpTesterSs
{
    /// <summary>
    /// To Finalize Tester-Conidition after a Test Finished.
    /// Example: Close Devices
    /// </summary>
    class TaskSgPostTask
    {
        public static async Task<CpProcessEndStatus> taskSgPostTest(UcMainViewSs xOwner, CancellationTokenSource token)
        {
            Stopwatch xSW = new Stopwatch();
            xSW.Start();

            UtilTextMessageEdits.UtilTextMsgToConsole("Finalized, Station." + xOwner.StationID.ToString(), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.INFO);

            if (FormAppSs.TheMainForm.AutoStartPLC)
            {
                if (!xOwner.PlcIF.PC_FINISHED)
                {
                    if (PlcUtil.ResultPLC(xOwner).Where(R => R == true).Count() > 0)
                        xOwner.PlcIF.PC_TOTAL_JUDGE = 1;
                    else
                        xOwner.PlcIF.PC_TOTAL_JUDGE = 0;
                }

                await Task.Delay(500);   // PLC Data Write Time

                xOwner.PlcIF.PC_FINISHED = true;

                while (xOwner.PlcIF.PLC_START || xOwner.PlcIF.PLC_PASS)
                    await Task.Delay(50);


                xOwner.PlcIF.PC_RUNNING = false;
                xOwner.PlcIF.PC_FINISHED = false;

                if (xOwner.TheApplication.Station.Last().StationId == xOwner.StationID)
                    xOwner.PlcIF.SendMarkingData();

                ResetDaqBuffer(xOwner, false);
            }
            else
            {
                ResetDaqBuffer(xOwner, true);
                if (DriverBaseGlobals.IsAudit78())
                    await FormManualRobotAudit78.ParkingAudit78();
            }

            return new CpProcessEndStatus(CpSystemStatus.OK, xSW.ElapsedMilliseconds, 0);
        }


        private static void ResetDaqBuffer(UcMainViewSs xOwner, bool forceReset)
        {
            var device = xOwner.TheSystemManager.MngHardware.DicCommDeviceManager.Where(w => w.Value.DeviceInfo.DeviceType == CpDeviceType.DAQ_CONTROLLER).FirstOrDefault().Value;
            if (device == null)
                return;
            CpMngDAQControl DAQ = device as CpMngDAQControl;
            if (forceReset)
                DAQ.ResetBuffer(-1);
            else
            {
                int index = -1;
                foreach (var AI in xOwner.MngStation.CnfStation.DevList)
                {
                    if (AI.ID == "AI0")
                        index = 0;
                    else if (AI.ID == "AI1")
                        index = 1;
                    else if (AI.ID == "AI2")
                        index = 2;
                }

                DAQ.ResetBuffer(index);
            }
        }
    }
}
