using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using CpTesterSs.UserControl;
using CpTesterPlatform.Functions;
using CpTesterPlatform.CpTester;
using Dsu.Driver.Paix;
using CpTesterPlatform.CpMngLib.Manager;
using static CpBase.CpLog4netLogging;
using Dsu.Driver.Base;
using Dsu.Common.Utilities;

namespace CpTesterPlatform.CpTesterSs
{
    /// <summary>
    /// To Build Tester-Conditions before a Test Starts.
    /// Example: Initialize Devices
    /// </summary>
    class TaskSgBackground
    {
        public static async Task<bool> taskSgBackgroundSkippable(UcMainViewSs xOwner, CancellationTokenSource token)
        {
            Stopwatch xSW = new Stopwatch();
            bool bSkipTest = false;
            xSW.Start();

            xOwner.TheMainForm.BackgroundRunning = true;

            if (FormAppSs.TheMainForm.AutoStartPLC)
            {
                if (await RunAutoModeSkippable(xOwner, xSW, token))
                {
                    xOwner.ChangeSystemStatus(CpSystemStatus.SKIP);
                    bSkipTest = true;
                }
            }

            xOwner.TheMainForm.BackgroundRunning = false;

            xSW.Stop();

            return bSkipTest;
        }

        private static async Task<bool> RunAutoModeSkippable(UcMainViewSs xOwner, Stopwatch xSW, CancellationTokenSource token)
        {

            xOwner.PlcIF.InitialInterface();

            while (!xOwner.PlcIF.PLC_PASS && !xOwner.PlcIF.PLC_START)
                await Task.Delay(50);

            xOwner.PlcIF.ResetData();

            if (xOwner.PlcIF.PLC_PASS)
            {
                xOwner.PlcIF.PC_TOTAL_JUDGE = 2;  // 2 is SKIP
                PlcUtil.ResultPLC(xOwner, true);
                xOwner.PlcIF.PC_FINISHED = true;

                while (xOwner.PlcIF.PLC_PASS)
                    await Task.Delay(50);

                return true;
            }
            else
            {
                if (xOwner.PlcIF.STATION == 0 && CpUtil.FD_MASTER_USE)
                {
                    xOwner.PlcIF_FdMasterHandler(true);
                    await Task.Delay(1000);
                }

                xOwner.PlcIF.PC_RUNNING = true;

                return false;
            }
        }
    }
}
