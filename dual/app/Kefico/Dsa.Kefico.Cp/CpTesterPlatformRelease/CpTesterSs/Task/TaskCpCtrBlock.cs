using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CpTesterPlatform.CpApplication;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpTesterPlatform.CpTStepFncLib.Manager;
using PsKGaudi.Parser;

namespace CpTesterPlatform.CpTesterSs
{
	/// <summary>
	/// To Construct a Manager for Control Variables and Global Variables
	/// </summary>
	class TaskCpCtrBlock
	{
		public static async Task<bool> WaitCpConstructComblockSteps(CpSystemManager MngSystem, CpApplicationManager MngApp, CpStnManager MngStn, CancellationTokenSource token)
		{
			try  //! Info: await block : TryFunc 적용 불가
			{
				CpTsManager MngTs = (CpTsManager)MngStn.MngTStep;

				MngTs.TsCurrentNumIndex = MngTs.GetTStepIndexByNum(MngTs.TsTargetStartNum);
				int nTsRangeByIndex = MngTs.GetTStepIndexByNum(MngTs.TsTargetEndNum) - MngTs.GetTStepIndexByNum(MngTs.TsTargetStartNum);

				Stopwatch xSW = new Stopwatch();
				xSW.Start();

				//! - Create a set of managers.
				MngTs.MngGlobalVariable = new CpGvManager(MngTs.GaudiReadData.GlobalVariable.ArstrGlobalVariable.ToArray());

				//! - Loading CP-Tester configuration.
				string strFileName = Directory.GetCurrentDirectory() + MngApp.CnfApp.AppConfigure.CnfPath + ClsGlobalStringForCpConfiguration.CP_CNF_FILE_NAME;
				List<CpAdtCnf> lstLoadedCpCnf = (new CpCbManager()).readCpConfirationFile(strFileName, ControBlockProperty.CP_FU);

                if (lstLoadedCpCnf == null)
                {
                    UtilTextMessageBox.UIMessageBoxForWarning("AppConfigure Error", $"[AppConfigure Error] in {strFileName}");
                    return false;
                }

                MngStn.MngTStep.MngControlBlock = new CpCbManager(lstLoadedCpCnf, MngSystem.MngHardware.CnfDevices.DevConfigue.LstDeviceInfo);
				MngTs.DicTsResult.Clear();

				/// 00.Start Construction

				Stopwatch swLoopCtr = new Stopwatch();
				swLoopCtr.Start();

				while (true)
				{
					/// 01.Stop Condition (Stop Condition / Test Finished / Debug Point)
					if (token.IsCancellationRequested)
					{
						xSW.Stop();
						return false;
					}

					if (MngTs.isFinished(MngTs.TsCurrentNumIndex))
						break;

					/// 02.Pick up the Current Step (Test Spec) && Update UI.
					CpTsShell cpTStep = MngTs.getTStepByIndex(MngTs.TsCurrentNumIndex);

					/// 03.Execute the test Step.
					TsResult tsResult = cpTStep.Execute(MngSystem, MngStn);
					MngTs.DicTsResult.Add(cpTStep.Core.StepNum, tsResult);

					/// 99.Go to the Next Step.
					MngTs.TsCurrentNumIndex++;
					await Task.Delay(CPDefineSleepTime.CP_FT_SHORT_THREADING_SLEEP_TIME);
				}

				if (!CpTsControlManager.linkBetweenWhileEndWhile(MngTs.LstTestSteps))
					UtilTextMessageEdits.UtilTextMsgToConsole("Failed to link a pair of control functions.", ConsoleColor.Red);

				xSW.Stop();
				UtilTextMessageEdits.UtilTextMsgToConsole("Time : " + xSW.ElapsedMilliseconds.ToString(), ConsoleColor.White);

				return true;
			}
			catch (System.Exception ex)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("[Thread Error] in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}
		}


		static bool processdControlBlockWiring(CpSystemManager MngSystem, CpStnManager MngStn, string strCrtTestListGate)
		{
			CpTsManager MngTs = (CpTsManager)MngStn.MngTStep;
			/// create wiring, stimuli data
			foreach (CpTsShell ts in MngTs.LstTestSteps)
			{
				try
				{
					CpTsMacroShell checkts = ts as CpTsMacroShell;

					if (checkts == null)
						continue;

					if (ts.Core.Activate != PsCCSDefineEnumActivate.ACTIVATE)
						continue;

					if (ts.Core.VariantActivate != PsCCSDefineEnumVariantAcrivate.ACTIVATE)
						continue;

					string strGate = ts.Core.Gate;
					if (ts.Core.Gate != "" && !(ts.Core.Gate.Contains(strCrtTestListGate)) && ts.Core.Gate != "ALL")
					{
						UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("step = {0}, gate = {1}", ts.Core.StepNum, ts.Core.Gate));
						continue;
					}

					IBERECHNEN bere = checkts as IBERECHNEN;

					foreach (CpTsShell subshell in checkts.LstTsSubModule)
					{
						CpTsShellWithWiring wiringshell = subshell as CpTsShellWithWiring;
						if (wiringshell != null)
						{
							if (!wiringshell.createWiring(MngSystem, MngStn, checkts))
								return false;

							if (!wiringshell.createLoadList(MngSystem, MngStn, checkts))
								return false;
						}

						CpTsShellWithCotrolBlock ctlblkshell = subshell as CpTsShellWithCotrolBlock;
						if (ctlblkshell != null)
						{
							ctlblkshell.createStimuliList(MngTs.MngControlBlock, checkts.Core.StepNum);
						}

						IM_DMM clsdmmshell = subshell as IM_DMM;
						if (clsdmmshell != null)
							clsdmmshell.createDMMProperies(MngTs.MngControlBlock, checkts);

						IM_COUNTER clscounter = subshell as IM_COUNTER;
						if (clscounter != null)
							clscounter.createDMMProperies(MngTs.MngControlBlock, checkts);

						IE_TRIGGER clstrgshell = subshell as IE_TRIGGER;
						if (clstrgshell != null)
							clstrgshell.createTriggerInfo(MngTs.MngControlBlock, checkts);

						IM_ZUWEIS clszuweis = subshell as IM_ZUWEIS;
						if (clszuweis != null)
							clszuweis.setShuntValue(MngTs.MngControlBlock, checkts);
					}
				}
				catch
				{
					Debug.Assert(false);
					return false;
				}
			}

			return true;
		}
	}
}
