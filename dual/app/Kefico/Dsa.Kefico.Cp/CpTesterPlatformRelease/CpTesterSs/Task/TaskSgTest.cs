using CpTesterPlatform.CpApplication;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncLib.Manager;
using CpTesterPlatform.DxUtility;
using CpTesterSs.UserControl;
using Dsu.Common.Utilities.ExtensionMethods;
using PsCommon;
using PsKGaudi.Parser;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using static CpCommon.ExceptionHandler;
using CpTesterPlatform.CpLogUtil;
using System.Collections.Generic;
using CpTesterPlatform.CpTester;
using CpTesterPlatform.Functions;
using CpTesterPlatform.CpMngLib.Manager;
using Dsu.Driver.Base;
using Dsu.Driver.Util.Emergency;

namespace CpTesterPlatform.CpTesterSs
{
    /// <summary>
    /// Main Test Engine
    /// </summary>
    class TaskSgTest
    {
        private static CpMngTriggerIO mngTempHumi;
        public static async Task<CpProcessEndStatus> taskSgTest(UcMainViewSs xOwner, CancellationTokenSource token)
        {
            await Task.Delay(1);

            mngTempHumi = CpUtil.GetManagerDevice(xOwner.TheSystemManager, CpDeviceType.TRIGGER_IO) as CpMngTriggerIO;
            Stopwatch xSW = new Stopwatch();

            xSW.Start();

            var stepManager = (CpTsManager)xOwner.MngStation.MngTStep;

            /// 00. Initialize test-list to be executed.
            int nTsStartIndex = stepManager.GetTStepIndexByNum(stepManager.TsTargetStartNum);
            int nTsRangeByIndex = stepManager.GetTStepIndexByNum(stepManager.TsTargetEndNum) - nTsStartIndex;
            stepManager.InitTestResult();
            stepManager.clearTStepActionLog(nTsRangeByIndex);

            var oResult = await TryFuncAsync(async () =>
            {
                using (xOwner.GetOnOperationMarker())
                {
                    CpTsShell cpTStep = null;
                    stepManager.TsCurrentNumIndex = stepManager.GetTStepIndexByNum(stepManager.TsTargetStartNum);
                    xOwner.MngStation.StationStatus = CpSystemStatus.RUN;
                    xOwner.frmSgTStepStatus.ChangeStatus(xOwner.MngStation.StationStatus);
                    xSW.Stop();

                    xSW.Start();
                    while (true)
                    {
                        /// 01.Stop Condition (Stop Condition / Test Finished / Debug Point)
                        if (token.IsCancellationRequested || ExceptionWithCode.IsFatalErrorOccurred)
                        {
                            //Task.Run(() =>
                            //{
                            //    CptApplication.TheApplication.ApplicationSubject.OnNext(new MyEvent());
                            //});

                            xSW.Stop();
                            return new CpProcessEndStatus(CpSystemStatus.STOP, xSW.ElapsedMilliseconds, stepManager.TsCurrentNumIndex);
                        }

                        /// 02.Pick up the Current Step (Test Spec) && Update UI.
                        cpTStep = stepManager.getTStepByIndex(stepManager.TsCurrentNumIndex);
                        if (cpTStep.Activated != TsActivate.ACTIVATED)
                        {
                            stepManager.TsCurrentNumIndex++;
                            continue;
                        }

                        CpTsMacroShell cpTStelMacroMeasuring = cpTStep as CpTsMacroShell;
                        var stepIndex = stepManager.GetTStepIndexByNum(cpTStep.Core.StepNum);

                        /// 03.Pause Condition (Break Point) / Jump Step.
                        ///			
                        if (!ExePauseCondition(xOwner, stepManager, cpTStep, token, xSW))
                            return new CpProcessEndStatus(CpSystemStatus.STOP, xSW.ElapsedMilliseconds, stepManager.TsCurrentNumIndex);

                        /// 04. Display Status Update
                        UpdateProgressStatus(xOwner, cpTStep, stepIndex, nTsStartIndex, nTsRangeByIndex);
                        await UpdateDisplayStatus(xOwner, cpTStep, stepIndex);

                        if ((cpTStep.GetPsStdFuncName() != PsCCSDefineEnumSTDFunction.CONTROL)
                            && CpTsControlManager.controlActionStatus.IsOneOf(CpSpecControlAction.GO_ELSE, CpSpecControlAction.GO_ENDIF))
                        {
                            //! - If TsResult == Skip: Skip TestStep
                            cpTStep.ResultLog = new ClsRlBase(TsResult.SKIP);
                        }
                        else
                        {
                            await Task.Delay(0);

                            /*
                             * Time limited version
                             */
                            /// 05.Execute the test Step.
                            TsResult tsResult = TsResult.ERROR;
                            var robotStep = CpUtil.UsingRobotStep(xOwner.TheSystemManager, xOwner.MngStation, cpTStep.Core);
                            int timeoutMilli = (robotStep ? 30 : 10) * 1000;
                            CancellationTokenSource timeOutCts = new CancellationTokenSource();
                            timeOutCts.CancelAfter(timeoutMilli);
                            var localLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeOutCts.Token, token.Token);

                            try
                            {
                                var task = cpTStep.ExecuteAsync(xOwner.TheSystemManager, xOwner.MngStation);
                                var finished = task.Wait(timeoutMilli, localLinkedCts.Token);
                                if (localLinkedCts.IsCancellationRequested)
                                {
                                    if (timeOutCts.IsCancellationRequested)
                                        Trace.WriteLine("Operation Timeout.");
                                }
                                else if (finished && task.IsCompleted)
                                {
                                    tsResult = task.Result;
                                    if (!stepManager.DicTsResult.ContainsKey(cpTStep.Core.StepNum))
                                        stepManager.DicTsResult.Add(cpTStep.Core.StepNum, tsResult);

                                    //Thread.Sleep(10);
                                    if (cpTStep.Core.Traceability == PsCCSDefineEnumTraceability.On)
                                        await xOwner.UpdateResultViewTable();
                                }
                            }
                            catch (Exception ex)
                            {
                                CpSignalManager.ShowErrorFormShortly(SignalEnum.XException, ex.Message);
                                return new CpProcessEndStatus(CpSystemStatus.STOP, xSW.ElapsedMilliseconds, stepManager.TsCurrentNumIndex);
                            }


                            ///// 05.Execute the test Step.
                            ////TsResult tsResult = cpTStep.Execute(xOwner.TheSystemManager, xOwner.MngStation);
                            //TsResult tsResult = await cpTStep.ExecuteAsync(xOwner.TheSystemManager, xOwner.MngStation);
                            //if (!stepManager.DicTsResult.ContainsKey(cpTStep.Core.StepNum))
                            //    stepManager.DicTsResult.Add(cpTStep.Core.StepNum, tsResult);

                            ////Thread.Sleep(10);
                            //if (cpTStep.Core.Traceability == PsCCSDefineEnumTraceability.On)
                            //    xOwner.UpdateResultViewTable();
                        }

                        /// 06.Update current main thread status for children (threads controlled by this thread).

                        if (stepManager.isFinished(stepManager.TsCurrentNumIndex))
                        {
                            xOwner.frmSgTStepStatus.UpdateProgressFinished();
                            await xOwner.frmSgTStepInformation.ChangeStepFunction(cpTStep.Core.STDBoschName);
                            break;
                        }

                        /// 99.Go to the Next Step.
                        UpdateNextStep(xOwner.MngStation, stepManager, cpTStep);
                    }

                    UtilTextMessageEdits.UtilTextMsgToConsole("Single Test Time : " + xSW.ElapsedMilliseconds);
                    stepManager.TsPause = false;
                    xSW.Stop();

                    if (DriverBaseGlobals.IsLine() && xOwner.PlcIF.STATION == 0 && CpUtil.FD_MASTER_USE)
                    {
                        xOwner.frmSgTStepStatus.UpdateProgressFinished();
                        await xOwner.frmSgTStepInformation.ChangeStepFunction(cpTStep.Core.STDBoschName);
                        return new CpProcessEndStatus(CpSystemStatus.SKIP, xSW.ElapsedMilliseconds, stepManager.TsCurrentNumIndex);
                    }
                    else
                        /// - After Test Finished: Save Measuring Log.
                        SaveMeasuringLog(xOwner, xSW.ElapsedMilliseconds);

                    UtilTextMessageEdits.UtilTextMsgToConsole("Escaped main thread loop Time : " + xSW.ElapsedMilliseconds);
                    return new CpProcessEndStatus(GetEndStatus(stepManager), xSW.ElapsedMilliseconds, stepManager.TsCurrentNumIndex);
                }
            });

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("[Thread Error] in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                xSW.Stop();
                return new CpProcessEndStatus(CpSystemStatus.NG, xSW.ElapsedMilliseconds, stepManager.TsCurrentNumIndex);
            }

            return oResult.Result;
        }

        #region ENGINE_FUNCTION
        static void UpdateNextStep(CpStnManager stnManager, CpTsManager stepManager, CpTsShell cpTStep)
        {
            if (stnManager.CnfTestCondition.FailStop == true)
            {
                if (IsNGStep(cpTStep))
                    stepManager.TsCurrentNumIndex = stepManager.GetTStepIndexByNum(stepManager.TsTargetEndNum);
                else
                    stepManager.TsCurrentNumIndex++;
            }
            else
                stepManager.TsCurrentNumIndex++;
        }

        static CpSystemStatus GetEndStatus(CpTsManager stepManager)
        {
            foreach (CpTsShell cpTStep in stepManager.LstTestSteps)
            {
                if (IsNGStep(cpTStep))
                    return CpSystemStatus.NG;
            }

            return CpSystemStatus.OK;
        }

        static bool IsNGStep(CpTsShell cpTStep)
        {
            if (cpTStep.ResultLog.TsActionResult == TsResult.NG || cpTStep.ResultLog.TsActionResult == TsResult.ERROR || (cpTStep.ResultLog as ClsRlMeasuring)?.TsMeasuringResult == TsResult.NG)
                return true;

            return false;
        }
        #endregion

        #region DISPLAY_REL_FUNCTION
        /// 04-1. Progress bar and focused row update action
        static void UpdateProgressStatus(UcMainViewSs xOwner, CpTsShell cpTStep, int nStepIndex, int nTsStartIndex, int nTsRangeByIndex)
        {
            var updateProgressBar = act(() =>
            {
                xOwner.frmSgTStepStatus.UpdateProgress(nStepIndex - nTsStartIndex, nTsRangeByIndex, cpTStep.Core.StepNum);
                xOwner.frmSgTStepList.UpdateFocusForSelectedStep(cpTStep.StepNum);
            });

            //if (cpTStelMacroMeasuring != null)
            updateProgressBar();
        }

        /// 04-2. Update UI.
        static async Task UpdateDisplayStatus(UcMainViewSs xOwner, CpTsShell cpTStep, int nStepIndex)
        {
            if (cpTStep.GetPsStdFuncName() == PsCCSDefineEnumSTDFunction.PRINTOUT)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole($"Step = {cpTStep.Core.StepNum}", ConsoleColor.White);

                await xOwner.UpdateForPrintOutFn(nStepIndex, cpTStep, CpSystemStatus.RUN);
                await Task.Delay(CPDefineSleepTime.CP_FT_SHORT_THREADING_SLEEP_TIME);
            }
        }
        #endregion

        #region LOG_REL_FUNCTIONS
        private static void SaveMeasuringLog(UcMainViewSs xOwner, long nTime)
        {
            if (xOwner.SgStnUiCtr.SaveMeasuringLog)
            {
                CpLogHeader rlHeader = CpLogHeader.CreateRlHeader(xOwner.TheApplication.CnfApp);

                rlHeader.PART_ID = GetPartID(xOwner);
                if (DriverBaseGlobals.IsLine())
                    rlHeader.ST_TIME = CpUtil.GetMesTime(xOwner.StationID).ToString("yyyyMMdd_HHmmss");
                else
                    rlHeader.ST_TIME = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                rlHeader.DURATION = nTime.ToString();  //DURATION: >> 시험 수행 시간
                rlHeader.RESULT = GetEndStatus(xOwner.MngStation.MngTStep as CpTsManager).ToString();
                rlHeader.ERROR_CODE = GetEndStatus(xOwner.MngStation.MngTStep as CpTsManager).ToString();
                rlHeader.HW_NUMBER = xOwner.MngStation.MngTStep.GaudiReadData.TestListInfo.PartNum;
                rlHeader.SW_NUMBER = xOwner.MngStation.MngTStep.GaudiReadData.TestListInfo.PartNum;
                rlHeader.PATH = Directory.GetCurrentDirectory() + xOwner.TheApplication.CnfApp.AppConfigure.TestListPath + xOwner.MngStation.FileName;
                rlHeader.CP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                rlHeader.PHASE = "P#1";
                rlHeader.STATION = xOwner.MngStation.StationId.ToString();   // Process Number
                rlHeader.TESTER = "010";                               // Tester Number +Index Number => 01 + 0 = 010
                rlHeader.TYPE = "Speed Sensor";
                rlHeader.ENGINE = xOwner.TheApplication.CnfApp.SystemInfoConfigure.Title;
                rlHeader.TEMP = mngTempHumi?.GetTemperature().ToString();
                rlHeader.HUM = mngTempHumi?.GetHumidity().ToString();
                rlHeader.CONTROL = FormAppSs.TheMainForm.AutoMode ? "AUTO" : "MANUAL";
                rlHeader.COMMENT = "CG_CGK_SAMPLE_TEST";

                CpUtilRl.SaveMeasuringLog(xOwner.TheApplication, xOwner.MngStation, rlHeader, GetEndStatus(xOwner.MngStation.MngTStep as CpTsManager), xOwner.SgStnUiCtr.SaveMeasuringData);
            }
        }

        private static string GetPartID(UcMainViewSs xOwner)
        {
            string PartID = "";
            if (CpUtil.PartID.Contains("+"))
                PartID = CpUtil.GetPartID(xOwner.StationID);
            else if (CpUtil.PartID.Contains("*"))
                PartID = CpUtil.PartID.TrimEnd('*');
            else if(DriverBaseGlobals.IsLine())
                PartID = CpUtil.GetMesID(xOwner.StationID);
            else
                PartID = CpUtil.PartID + CpUtil.GetStationindex(xOwner.StationID);

            return PartID;
        }
        #endregion

        #region PAUSE_REL_FUNCTIONS
        static bool ExePauseCondition(UcMainViewSs xOwner, CpTsManager stepManager, CpTsShell cpTStep, CancellationTokenSource token, Stopwatch xSW)
        {
            if ((xOwner.SgStnUiCtr.UseBreak && xOwner.IsThisBreakStep(cpTStep.Core.StepNum)))
            {
                xOwner.SetBreakStep(cpTStep.Core.StepNum, xSW.ElapsedMilliseconds);

                if (!WaitChangingBreakStatus(xOwner, stepManager, cpTStep, token))
                    return false;
            }

            if (!CheckPauseState(xOwner, stepManager, cpTStep, token, xSW))
                return false;

            return true;
        }

        /// <summary>
        /// Pause Condition
        /// <return>Paused Time</return>
        /// </summary>
        static bool CheckPauseState(UcMainViewSs xOwner, CpTsManager stepManager, CpTsShell cpTStep, CancellationTokenSource token, Stopwatch xSW)
        {
            while (true)
            {
                if (xOwner.MngStation.StationStatus != CpSystemStatus.PAUSE)
                    break;

                if (xOwner.SgStnUiCtr.JumpToNextBreak)
                {
                    xOwner.SgStnUiCtr.JumpToNextBreak = false;
                    stepManager.TsPause = false;
                    break;
                }

                if (xOwner.SgStnUiCtr.JumpToNextStep)
                {
                    xOwner.SetMoveNextStep(cpTStep.Core.StepNum, xSW.ElapsedMilliseconds);
                    xOwner.SgStnUiCtr.JumpToNextStep = false;
                    stepManager.TsPause = true;
                    break;
                }

                if (token.IsCancellationRequested)
                {
                    Task.Run(() =>
                    {
                        CptApplication.TheApplication.ApplicationSubject.OnNext(new MyEvent());
                    });

                    return false;
                }
            }

            return true;
        }

        static bool WaitChangingBreakStatus(UcMainViewSs xOwner, CpTsManager stepManager, CpTsShell cpTStep, CancellationTokenSource token)
        {
            while (true)
            {
                if (xOwner.MngStation.StationStatus == CpSystemStatus.PAUSE)
                    break;

                if (token.IsCancellationRequested)
                {
                    Task.Run(() =>
                    {
                        CptApplication.TheApplication.ApplicationSubject.OnNext(new MyEvent());
                    });

                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
