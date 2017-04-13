using CpTesterPlatform.CpCommon;
using CpTesterPlatform.DxUtility;
using CpTesterSs.UserControl;
using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using CpTesterSs;
using CpTesterPlatform.CpApplication.Manager;
using System.Collections.Generic;
using static CpCommon.ExceptionHandler;
using static CpBase.CpLog4netLogging;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.Functions;
using Dsu.Common.Utilities.ExtensionMethods;
using CpTesterSs.Event;
using System.Linq;
using DevExpress.XtraBars.Docking;
using CpTesterPlatform.CpTesterSs;
using CpTesterPlatform.CpMngLib.Interface;
using Dsu.Driver.Base;
using System.Threading;
using CpTesterSs.UIManual;
using Dsu.Driver.Paix;
using Dsu.Common.Utilities;
using Dsu.Driver.Util.Emergency;
using Dsu.Common.Utilities.DX;

namespace CpTesterPlatform.CpTester
{
    public partial class FormAppSs
    {
        public DevExpress.XtraBars.Docking.DockManager DockManager => dockManager;

        public class ClsMainUIControl
        {
            public bool SaveMeasuringLog { get; set; }
            public bool SaveConsoleLogToFile { get; set; }
            public bool SaveMeasuringData { get; set; }

            public ClsMainUIControl(bool bSaveMeasuringLog, bool bSaveConsoleLogToFile, bool bSaveMeasuringData)
            {
                SaveMeasuringLog = bSaveMeasuringLog;
                SaveConsoleLogToFile = bSaveConsoleLogToFile;
                SaveMeasuringData = bSaveMeasuringData;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try // ref parameter.  TryAction/TryFunc 적용 불가
            {
                if (!base.ProcessCmdKey(ref msg, keyData))
                {
                    if (keyData.Equals(Keys.F1))
                    {
                        //barButtonItemMnCtrStart.PerformClick();
                        return true;
                    }
                    else if (keyData.Equals(Keys.F4))
                    {
                        //barButtonItemMnCtrStop.PerformClick();
                        return true;
                    }
                    else if (keyData.Equals(Keys.F6))
                    {
                        // barButtonItemMnCtrTsConditon.PerformClick();
                        return true;
                    }
                    else if (keyData.Equals(Keys.F9))
                    {

                        return true;
                    }
                    else if (keyData.Equals(Keys.F11))
                    {
                        //barButtonItemMnCtrJumpToNextBreak.PerformClick();
                        return true;
                    }
                    else if (keyData.Equals(Keys.F12))
                    {
                        //barButtonItemMnCtrJumpToNextStep.PerformClick();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed process command key in the main frame.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return false;
        }


        private bool SetRepeatTestStatus(int nTestNumber, int nTotalTestNumber)
        {
            return TryAction(() =>
            {
                barStaticItemRepeatTest.Caption = string.Format("RepeatTest : {0}/{1}", nTestNumber, nTotalTestNumber);
            }).Succeeded;
        }


        public void ShowDisplayMessage(string message)
        {
            this.DoAsync(() => labelControl_Message.Text = message);
        }

        public IEnumerable<frmSgDocFrame> GetDocFrames() => MdiChildren.OfType<frmSgDocFrame>();
        public IEnumerable<UcMainViewSs> GetViews() => GetDocFrames().Select(frm => frm.GetMyViewFrm());
        public IEnumerable<Tuple<string, DockManager>> GetDockManagers()
        {
            yield return Tuple.Create("main", dockManager);
            foreach (var v in GetViews())
                yield return Tuple.Create($"station{v.MngStation.StationId}", v.DockManager);
        }

        private async void barButtonItemMnCtrStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            await StartTest();
        }

        /// Emergency / stop
        private void barButtonItemMnCtrStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            StopTest();
        }

        public async Task StartTest()
        {
            _cts = new CancellationTokenSource();

            ProcessIdle = false;

            List<Task> vPreTask = new List<Task>();
            GetViews().ForEach(v =>
            {
                v.ResetCancellationTokenSources();
                vPreTask.Add(new Task(() => v.StartSgPreTest()));
            });
            Parallel.ForEach(vPreTask, task => task.Start());

            await CheckAllStationFinState();
        }

        public void AddMonitiorUnloadingStation(CpMngPlc MngPlc)
        {
            if (DicPlcIF.ContainsKey(CpUtil.MonitoringPLC))
                return;

            DicPlcIF.Add(CpUtil.MonitoringPLC, new CpPlcIF(CpUtil.MonitoringPLC)); //monitoring Station
            DicPlcIF[CpUtil.MonitoringPLC].MngPlcRead = MngPlc;
            DicPlcIF[CpUtil.MonitoringPLC].CreatEventPLC();
            foreach (CpAdtCnf cpAdtCnf in TheApplication.Station[0].MngTStep.MngControlBlock.LstLoadedAdapterCnf)
            {
                CpDeviceManagerBase DeviceManagerBase = CpUtil.GetManagerDevice(TheSystemManager, cpAdtCnf.AdtMslUnit.RawString);
                if (DeviceManagerBase == null)
                    continue;

                if (DeviceManagerBase.DeviceInfo.DeviceType == CpDeviceType.PLC)
                {
                    DicPlcIF[CpUtil.MonitoringPLC].AddItem(cpAdtCnf.AdtPinName.RawString, cpAdtCnf.AdtBpAddress.RawString);
                    DicPlcIF[CpUtil.MonitoringPLC].MngPlcRead.AddDevices(cpAdtCnf.AdtBpAddress.RawString);
                }
            }

            DicPlcIF[CpUtil.MonitoringPLC].UnloadingResultHandler += FormAppSs_UnloadingResultHandler;
            DicPlcIF[CpUtil.MonitoringPLC].UnloadingCloseHandler += FormAppSs_UnloadingCloseHandler;
            DicPlcIF[CpUtil.MonitoringPLC].UnloadingPassHandler += FormAppSs_UnloadingPassHandler;
            ucTestMonitor1.UnloadingDialogHandler += UcTestMonitor1_UnloadingDialogHandler;
        }

        public void OnDeveloperModeChanged()
        {
            repositoryItemMarqueeProgressBar1.MarqueeWidth = 30;
            repositoryItemMarqueeProgressBar1.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.Cycle;
        }

        private async void FormAppSs_UnloadingPassHandler(bool bPassMode)
        {
            await TheMainForm.DoAsync(() =>
            {
                labelControl_Message.Visible = bPassMode;
                if (bPassMode)
                    labelControl_Message.Text = "Cg TEST MODE";
                else
                    labelControl_Message.Text = "";
            });
        }

        private async void FormAppSs_UnloadingCloseHandler(bool bClose)
        {
            await TheMainForm.DoAsync(() =>
            {
                TotalResultDialog?.CloseForm();
                TotalResultDialog7DCT?.CloseForm();
            });
        }

        private async void FormAppSs_UnloadingResultHandler(List<PLCResult> result, bool nNg, string Message, int NgStation, int StationIndex)
        {
            await TheMainForm.DoAsync(() =>
            {
                CpPartID patid = new CpPartID(DateTime.Now, "", "");
                if (CpUtil.Station_PartID.ContainsKey(StationIndex.ToString()))
                    patid = CpUtil.Station_PartID[StationIndex.ToString()];

                ucTestMonitor1.UdpateNgStation(NgStation, Message, patid.MesID);
                TotalResultDialog?.CloseForm();
                TotalResultDialog7DCT?.CloseForm();
         
                 if (DriverBaseGlobals.IsLine7DCT())
                    TotalResultDialog7DCT = new FormTotalResult7DCT(result, nNg, Message);
                else if (DriverBaseGlobals.IsLine8FF())
                    TotalResultDialog = new FormTotalResult(result, nNg, Message);

               // TotalResultDialog?.Show();
               // TotalResultDialog7DCT?.Show();
            });
        }

        private async void UcTestMonitor1_UnloadingDialogHandler(bool bShowDialog)
        {
            await TheMainForm.DoAsync(() =>
            {
                if (TotalResultDialog != null && !TotalResultDialog.IsDisposed)
                    TotalResultDialog.Show();
                if (TotalResultDialog7DCT != null && !TotalResultDialog7DCT.IsDisposed)
                    TotalResultDialog7DCT.Show();
            });
        }

        public double TemperatureMin => double.Parse(barEditItem_TemperatureLower.EditValue.ToString());
        public double TemperatureMax => double.Parse(barEditItem_TemperatureUpper.EditValue.ToString());
        public double HumidityMin => double.Parse(barEditItem_HumidityLower.EditValue.ToString());
        public double HumidityMax => double.Parse(barEditItem_HumidityUpper.EditValue.ToString());
        public double CurrentTemerature { get; set; }
        public double CurrentHumidity { get; set; }

        public void StopTest()
        {
            _cts.Cancel();

            var ps = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), true);
            if (ps != null) ps.ForEach(mng => ((CpMngMotion)mng).StopMotionEmergency());
            var psMotor = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), false);
            if (psMotor != null) psMotor.ForEach(mng => ((CpMngMotion)mng).StopMotionEmergency());

            foreach (CpProcess cpproc in TheProcessManager.CurrentTasks)
                if (cpproc != null) cpproc.CancelToken.Cancel();

            if (CpUtil.CheckEnableDevice(TheSystemManager, CpDeviceType.PLC))
                foreach (var plc in DicPlcIF.Values)
                    plc.StopInterface();

            // 강제로 stop 상태로 변경.
            Stations.Where(w => w.CnfStation.Enable).ForEach(s => s.StationStatus = CpSystemStatus.STOP);
            // 강제로 IDLE 상태로 변경.
            GetViews().ForEach(s => s.SgStnStatus = CpStationStatus.IDLE);

            TheMainForm.BackgroundRunning = false;
        }

        async Task CheckAllStationFinState()
        {
            while (true)
            {
                if (CheckTaskIdleState() == true)
                    break;

                await Task.Delay(2);
            }

            ProcessIdle = true;

            UtilTextMessageEdits.UtilTextMsgToConsole("All Processes have been finished.", ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.INFO);
        }

        async Task CheckAllStationsReady()
        {
            while (true)
            {
                bool allStationReady =
                    GetViews()
                        .ForAll(v => v.frmSgTStepStatus.Status == CpSystemStatus.READY);

                if (allStationReady)
                    break;

                await Task.Delay(100);
            }

            UtilTextMessageEdits.UtilTextMsgToConsole("All Processes have been ready.", ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.INFO);
        }

        bool CheckTaskIdleState() => GetViews().ForAll(v => v.SgStnStatus == CpStationStatus.IDLE);

        #region Test-list Option Commands
        private void barCheckItemShowDeactivateStep_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
				{
                    if (!stnMng.CnfStation.Enable) continue;
					UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
					UiCpAssistForDxApp.updateGridViewStepsByShowOption(childMainView.GridCtrTestSteps, stnMng.MngTStep, barCheckItemShowDeactivateStep.Checked);
					childMainView.GridViewTestSteps.BestFitColumns(true);
				}
				
			});
		}

		private void barCheckItemShowColPosition_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
				{
                    if (!stnMng.CnfStation.Enable) continue;
					UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
					UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.POSITION.ToString(), barCheckItemShowColPosition.Checked);
					childMainView.GridViewTestSteps.BestFitColumns(true);
				}
				
			});
		}
		private void barCheckItemShowColVariant_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
				{
                    if (!stnMng.CnfStation.Enable) continue;
					UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
					UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.VARIANT.ToString(), barCheckItemShowColVariant.Checked);
					childMainView.GridViewTestSteps.BestFitColumns(true);
				}
			});
		}
		private void barCheckItemShowColGate_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
				{
                    if (!stnMng.CnfStation.Enable) continue;
					UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
					UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.GATES.ToString(), barCheckItemShowColGate.Checked);
					childMainView.GridViewTestSteps.BestFitColumns(true);
				}
			});
		}

		private void barCheckItemShowColReturn_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
				{
                    if (!stnMng.CnfStation.Enable) continue;
					UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
					UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.RETURN.ToString(), barCheckItemShowColReturn.Checked);
					childMainView.GridViewTestSteps.BestFitColumns(true);
				}
			});
		}

		private void barCheckItemShowColParameter_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
				{
                    if (!stnMng.CnfStation.Enable) continue;
					UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
					UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.PARAMETER.ToString(), barCheckItemShowColParameter.Checked);
					childMainView.GridViewTestSteps.BestFitColumns(true);
				}				
			});
		}

		private void barCheckItemShowColComment_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
				{
                    if (!stnMng.CnfStation.Enable) continue;
					UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
					UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.COMMENTS.ToString(), barCheckItemShowColComment.Checked);
					childMainView.GridViewTestSteps.BestFitColumns(true);
				}				
			});
		}

        private void barCheckItemShowColMP_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
                {
                    if (!stnMng.CnfStation.Enable) continue;
                    UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
                    UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.MP.ToString(), barCheckItemShowColMP.Checked);
                    childMainView.GridViewTestSteps.BestFitColumns(true);
                }
            });
        }

        private void barCheckItemInf_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TryAction(() =>
            {
                foreach (CpStnManager stnMng in TheApplication.Station)
                {
                    if (!stnMng.CnfStation.Enable) continue;
                    UcTStepList childMainView = GetStationForm(stnMng.StationId).frmSgTStepList;
                    UiCpAssistForDxApp.setVisibleColumns(childMainView.GridViewTestSteps, UiCpDefineEnumTsColumns.INF.ToString(), barCheckItemShowColInf.Checked);
                    childMainView.GridViewTestSteps.BestFitColumns(true);
                }
            });
        }
        #endregion

        #region LOG Configuration
        private void barEditItemShowLogLevel_EditValueChanged(object sender, EventArgs e)
		{			
			CpDefineEnumDebugPrintLogLevel eCurLogLevel;
			Enum.TryParse(barEditItemShowLogLevel.EditValue.ToString(), out eCurLogLevel);
			
            UtilTextMessageEdits.PrintLogLevel = eCurLogLevel;
        }

        private void barCheckItemLogSaveMeasuringLog_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                this.MainUiCtr.SaveMeasuringLog = barCheckItemLogSaveMeasuringLog.Checked;

				foreach(CpStnManager stnMng in TheApplication.Station)
					GetStationForm(stnMng.StationId).CfgLogState(MainUiCtr.SaveMeasuringLog, MainUiCtr.SaveMeasuringData,MainUiCtr.SaveConsoleLogToFile);
			});
		}
				
		private void barCheckItemMeasuringArray_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                this.MainUiCtr.SaveMeasuringData = barCheckItemMeasuringArray.Checked;
				
				foreach(CpStnManager stnMng in TheApplication.Station)
                    if(stnMng.CnfStation.Enable)
					GetStationForm(stnMng.StationId).CfgLogState(MainUiCtr.SaveMeasuringLog, MainUiCtr.SaveMeasuringData,MainUiCtr.SaveConsoleLogToFile);
			});			
		}
		private void barCheckItemSaveConsoleLog_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{

            TryAction(() =>
            {
                string strCrtTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                FileWriteLog();
#if DEBUG

                this.MainUiCtr.SaveConsoleLogToFile = barCheckItemSaveConsoleLog.Checked;

				foreach(CpStnManager stnMng in TheApplication.Station)
					GetStationForm(stnMng.StationId)?.CfgLogState(MainUiCtr.SaveMeasuringLog, MainUiCtr.SaveMeasuringData,MainUiCtr.SaveConsoleLogToFile);

				if (this.MainUiCtr.SaveConsoleLogToFile)
				{
					FileStream filestream = new FileStream(strCrtTime + "_ConsoleOutMessage.txt", FileMode.Create);
					StreamWriter streamwriter = new StreamWriter(filestream);
					streamwriter.AutoFlush = true;
					Console.SetOut(streamwriter);

					FileStream filestreamError = new FileStream(strCrtTime + "_ConsoleOutError.txt", FileMode.Create);
					StreamWriter streamwriterError = new StreamWriter(filestreamError);
					streamwriterError.AutoFlush = true;
					Console.SetError(streamwriterError);
				}
				else
				{
					TextWriter crtOut = new StreamWriter(Console.OpenStandardOutput());
					Console.SetOut(crtOut);
					Console.SetError(crtOut);
				}
#endif
            });
		}
		        			   
		private void barButtonItemSaveLogPath_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            TryAction(() =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
				fbd.ShowDialog();
				if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					TheApplication.CnfApp.AppConfigure.SaveLogPath = fbd.SelectedPath;

					UpdateLog("Change Selected Log File Path : " + fbd.SelectedPath, CpDefineEnumDebugPrintLogLevel.INFO);
				}
			});
		}

		private void barEditItemLogTail_EditValueChanged(object sender, EventArgs e)
		{
            //MngSystem.CnfSystem.AppConfigue.LogTail = barEditItemLogTail.EditValue.ToString();
        }
        #endregion

        #region Thread delegates for the main form.

        private delegate void DelegateUpdateProgressBarForCurrentTStepIndex(int nCrtStepIndex, int nTsRangeByIndex, int nUpdateStatusHeaderTsNum = -1);
		public void threadInvokeUpdateProgressBarForCurrentTStepIndex(int nCrtStepIndex, int nTsRangeByIndex, int nUpdateStatusHeaderTsNum = -1)
		{
			if (this.InvokeRequired)
			{
				DelegateUpdateProgressBarForCurrentTStepIndex delTxtCrtStep = new DelegateUpdateProgressBarForCurrentTStepIndex(threadInvokeUpdateProgressBarForCurrentTStepIndex);
				this.Invoke(delTxtCrtStep, nCrtStepIndex, nTsRangeByIndex, nUpdateStatusHeaderTsNum);
			}
			else
			{                
			}
		}        
		
		//barStaticItemRepeatTest
		private delegate void DelegateSetRepeatTestStatus(int nTestNumber, int nTotalTestNumber);
		public void threadInvokeSetRepeatTestStatus(int nTestNumber, int nTotalTestNumber)
		{
			if (this.InvokeRequired)
			{
				DelegateSetRepeatTestStatus delSetStatus = new DelegateSetRepeatTestStatus(threadInvokeSetRepeatTestStatus);
				this.Invoke(delSetStatus, nTestNumber, nTotalTestNumber);
			}
			else
			{
				SetRepeatTestStatus(nTestNumber, nTotalTestNumber);
			}
		}
				
		private delegate void DelegateSetMainFrameUsabilityButtons(bool bBtnStart, bool bBtnStop, bool bBtnBreak, bool bBtnJumpTo, bool bBtnNextTo);
		public void threadInvokeSetMainFrameUsabilityButtons(bool bBtnStart, bool bBtnStop, bool bBtnBreak, bool bBtnJumpTo, bool bBtnNextTo)
		{
            TryAction(() =>
            {
                if (this.InvokeRequired)
				{
					DelegateSetMainFrameUsabilityButtons delSetButtons = new DelegateSetMainFrameUsabilityButtons(threadInvokeSetMainFrameUsabilityButtons);
					this.Invoke(delSetButtons, bBtnStart, bBtnStop, bBtnBreak, bBtnJumpTo, bBtnNextTo);
				}
				else
				{
				}
			});
		}

		#endregion

		private void timerGlobalTask_Tick(object sender, EventArgs e)
		{
            TryAction(() =>
            {
                barStaticItemCurrentTime.Caption = DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
			});
		}       
		private void startRepeatTestByMainThread()
		{
            // prepare to start repeat tests.               
        }
        void timerRepeatTest_Tick(object sender, EventArgs e)
		{
		}


        private void OnApplicationIdle(object sender, EventArgs e)
        {
            barEditItem_CurrentTemp.EditValue = CurrentTemerature.ToString();
            barEditItem_CurrentHumidity.EditValue = CurrentHumidity.ToString();
        }


        private void barButtonItem_LCR_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var ps = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.LCRMETER);
            new FormManualLCR(ps).Show();
        }

        private void barButtonItem_DigtalIO_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var ps = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.DIGITAL_IO);

            Dictionary<string, List<CpAdtCnf>> dicPin = new Dictionary<string, List<CpAdtCnf>>();
            foreach (CpStnManager stn in TheApplication.Station)
            {
                if (!stn.CnfStation.Enable) continue;

                if (dicPin.ContainsKey(stn.Name))
                    continue;
                dicPin.Add(stn.Name, new List<CpAdtCnf>());
                foreach (CpAdtCnf AdapterCnf in stn.MngTStep.MngControlBlock.LstLoadedAdapterCnf)
                {
                    dicPin.Last().Value.Add(AdapterCnf);
                }
            }

            new FormManualDigitIO(ps, dicPin).Show();
        }

        private void barButtonItem_AnalogIO_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var ps = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.ANALOG_INPUT);
            new FormManualDAQ(ps).Show();
        }

        private void barButtonItem_Motor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<IDevManager> lstMng = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), false);
               if(lstMng.Count > 0)
                new FormManualMotion(lstMng[0]).Show();
        }

        private void barButtonItem_Motor2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<IDevManager> lstMng = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), false);
            if (lstMng.Count > 1)
                new FormManualMotion(lstMng[1]).Show();
        }

        private void barButtonItem_Motor3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<IDevManager> lstMng = CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.MOTION), false);
            if (lstMng.Count > 2)
                new FormManualMotion(lstMng[2]).Show();
        }


        private void barButtonItem_PowerSupply_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<IDevManager> lstMng = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.POWER_SUPPLY);
            if(lstMng.Count > 0)
                new FormManualDCPower(lstMng[0]).Show();
        }

        private void barButtonItem_PowerSupply2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<IDevManager> lstMng = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.POWER_SUPPLY);
            if (lstMng.Count > 1)
                new FormManualDCPower(lstMng[1]).Show();
        }

        private void barButtonItem_PowerSupply3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<IDevManager> lstMng = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.POWER_SUPPLY);
            if (lstMng.Count > 2)
                new FormManualDCPower(lstMng[2]).Show();
        }
        private void barButtonItem_LVDT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var ps = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.LVDT);
            new FormManualLVDT(ps).Show();
        }

        private void barButtonItem_Robot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var robots = GetDeviceManagers<CpMngMotion>().Where(d => d.PaixRobot != null);
            var mngDio = GetDeviceManagers<CpMngDIOControl>();
            FormManualRobotAudit.SetDeviceManagers(robots, mngDio);

            if (DriverBaseGlobals.IsAudit78())
                new FormManualRobotAudit78().Show();
            else if (DriverBaseGlobals.IsAuditGCVT())
                new FormManualRobotAuditGCVT().Show();
        }

        private void barButtonItem_TempHumi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CpMngTriggerIO mngTempHumi = CpUtil.GetManagerDevice(TheSystemManager, CpDeviceType.TRIGGER_IO) as CpMngTriggerIO;
            new FormManualTempHumi(mngTempHumi).Show();
        }

        private void barButtonItem_PLC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var ps = CpUtil.GetManagerDevices(TheSystemManager, CpDeviceType.PLC);
            new FormManualPLC(ps).Show();
        }

        private void barButtonItem_manualAirGap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormAdmin.DoModal()) return;
            new FormManualAirGap().Show();
        }

        private async void barButtonItem_AdminDialogClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FormAdmin.DoModal())
            {
                await FormError.ClearAllErrors();
            }
        }

        private void barButtonItemDoorControl_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FormDoorControl(GetDeviceManagers<CpMngDIOControl>()).Show();
        }

        private bool _robotReadyMoving;
        private async void barButtonItem_RobotReadyMove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!DriverBaseGlobals.IsAudit78())
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Only allowed in Audit78.");

            if ( ! PaixManagerBase.IsOriginCalibrated)
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Robot origin not set.");

            ResetCancellationTokenSource();
            await FormManualRobotAudit78.UnparkingAudit78();
            barButtonItem_RobotReadyMove.Enabled = false;
            _robotReadyMoving = true;
            try
            {
                using (var waitor = new SplashScreenWaitor(this))
                {
                    waitor.ProgressDescription = $"Moving to ready..";
                    await RobotManager.MovePath("ToReady", _cts.Token);
                }
            }
            catch(Exception ex)
            {
                LogInfoRobot($"Ready move failed: {ex.Message}");
            }
            finally
            {
                await FormManualRobotAudit78.ParkingAudit78();
                barButtonItem_RobotReadyMove.Enabled = true;
                _robotReadyMoving = false;
            }
        }
    }
}
