using System;
using System.Diagnostics;
using System.Reflection;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;
using PsCommon.Enum;

namespace CpTesterPlatform.Functions
{
	public class CpFnE_SCANNER : CpTsShellWithWiring, IE_SCANNER
	{
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
		{
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
			if((cpTsParent is IGETDATA_LCR) || (cpTsParent is IGETDATA_ARRAY) || (cpTsParent is IDCV_CP))
				return TsResult.OK;
				
			createWiring(cpMngSystem, iMngStation, cpTsParent);
			
			return TsResult.OK;
		}

		protected override TsResult setWiring(CpSystemManager cpMngSystem, IStnManager iMngStation)
		{
            var tResult = TryFunc(() =>
            {
                

				return TsResult.OK;
			});

            return tResult.Succeeded ? tResult.Result : TsResult.ERROR;
        }

        //protected override TsResult connectWiring(CpSystemManager cpMngSystem)
        //{
        //    try
        //    {
        //        // 1. Processing MK Switch
        //        if(!cpMngSystem.MngHardware.MngSwitch.SetConnection(CpInstSwitchType.MK_SWITCH, ClsCtrBlockWiringAssist.GetConnectWiringInfo(m_lstWiringInfo, CpInstSwitchType.MK_SWITCH)))
        //        {
        //            return TsResult.NG;
        //        }

        //        return TsResult.OK;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        UtilTextMessageEdits.UtilTextMsgToConsole("Failed to execute test step for wiring in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
        //        UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
        //    }
        //    return TsResult.ERROR;
        //}

        //protected override TsResult disconnectWiring(CpSystemManager cpMngSystem)
        //{
        //    try
        //    {
        //        // 1. Processing MK Switch
        //        if(!cpMngSystem.MngHardware.MngSwitch.SetConnection(CpInstSwitchType.MK_SWITCH, ClsCtrBlockWiringAssist.GetDisconnectWiringInfo(m_lstWiringInfo, CpInstSwitchType.MK_SWITCH)))
        //        {
        //            return TsResult.NG;
        //        }

        //        return TsResult.OK;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        UtilTextMessageEdits.UtilTextMsgToConsole("Failed to execute test step for wiring in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
        //        UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
        //    }
        //    return TsResult.ERROR;
        //}

        private bool getScanUnitFromParam(string paramUnit, ref string strUnit, CpTsShell cpTsParent)
		{
			if (paramUnit.Contains(ClsGlobalStringForTStep.CP_TS_PARM_VARIABLE))
			{
				CpTsMacroMsDCTrgShell cpMacroShellParent = cpTsParent as CpTsMacroMsDCTrgShell;

				if (cpMacroShellParent == null)
					return false;

				strUnit = NIDevTriggerInfo.getScannerUnitFromTrgInfo(cpMacroShellParent.DicTriggerInfo, paramUnit);
			}
			else
				strUnit = paramUnit;

			return true;
		}

		protected override TsResult setLoadData(CpSystemManager cpMngSystem, IStnManager cpMngStn, CpTsShell cpTsParent)
		{
            var tResult = TryFunc(() =>
            {
                IDCIS dcis = cpTsParent as IDCIS;
                

				return TsResult.NG;
			});

            return tResult.Succeeded ? tResult.Result : TsResult.ERROR;
        }

        public override bool createLoadList(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent)
        {
            var tResult = TryFunc(() =>
            {
                CpInstDCType dctype = CpInstDCType.NONE;
                if (!checkIsDCIFamily(cpTsParent, ref dctype))
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("checkIsDCIFamily is failed", ConsoleColor.White);
                    return false;
                }

                if (dctype != CpInstDCType.DCI_FAMILY)
                    return true;

                int nStepNum = cpTsParent.Core.StepNum;
                ICbManager tsManager = iMngStation.MngTStep.MngControlBlock;
                PsCCSStdFnModuleE_SCANNER psModuleE_SCANNER = this.Core as PsCCSStdFnModuleE_SCANNER;
                Debug.Assert(psModuleE_SCANNER != null);

                string strUnit = string.Empty;
                if (!getScanUnitFromParam(psModuleE_SCANNER.SCAN_UNIT, ref strUnit, cpTsParent))
                    Debug.Assert(false);

                // It needs Channel number and enable info that load connection on e_scanner.

                string strPinHigh = psModuleE_SCANNER.PIN_HIGH;
                string strPinLow = psModuleE_SCANNER.PIN_LOW;
                string strUnitTrg = psModuleE_SCANNER.SCANN_UNIT_TRG;
                bool bConnect = psModuleE_SCANNER.SCHALTER_FUNKT == eSCHALTER_FUNKT.ON ? true : false;


                ClsTsCtrBlockWithAdapter ctrblk = tsManager.DicAnsteuerWithCtrBlock[strPinHigh] as ClsTsCtrBlockWithAdapter;

                if (ctrblk.AdtCnf.CtrBlockProperty != ControBlockProperty.LOAD)
                    Debug.Assert(false);

                LstLoaddataInfo = iMngStation.MngTStep.MngControlBlock.CreateLoadMKDataListControlBlock(ctrblk, bConnect, iMngStation.MngTStep.MngControlBlock);

                return true;
            });

            return tResult.Succeeded ? tResult.Result : false;
        }

		private bool createWiringDCI(CpSystemManager mgrSystem, IStnManager iMngStation, CpTsShell cpTsParent, int nStepnum)
		{
            var tResult = TryFunc(() =>
            {
                ICbManager tsManager = iMngStation.MngTStep.MngControlBlock;
				PsCCSStdFnModuleE_SCANNER psModuleE_SCANNER = this.Core as PsCCSStdFnModuleE_SCANNER;
				Debug.Assert(psModuleE_SCANNER != null);

				string strUnit = string.Empty;
				if (!getScanUnitFromParam(psModuleE_SCANNER.SCAN_UNIT, ref strUnit, cpTsParent))
					Debug.Assert(false);

                // It needs Channel number and enable info that load connection on e_scanner.

                string strPinHigh = psModuleE_SCANNER.PIN_HIGH;
                string strPinLow = psModuleE_SCANNER.PIN_LOW;
                string strUnitTrg = psModuleE_SCANNER.SCANN_UNIT_TRG;
                bool bConnect = psModuleE_SCANNER.SCHALTER_FUNKT == eSCHALTER_FUNKT.ON ? true : false;

                string strPinHighTrg = psModuleE_SCANNER.PIN_HIGH_TRG;
                string strPinLowTrg = psModuleE_SCANNER.PIN_LOW_TRG;

				if (!strPinHigh.Equals(strPinLow))
					Debug.Assert(false);

				ClsTsCtrBlockWithAdapter ctrblk = tsManager.DicAnsteuerWithCtrBlock[strPinHigh] as ClsTsCtrBlockWithAdapter;


				if (ctrblk.AdtCnf.CtrBlockProperty != ControBlockProperty.LOAD)
					Debug.Assert(false);

				LstWiringInfo = iMngStation.MngTStep.MngControlBlock.CreateMKLoadWiringMeasure(strUnit, ctrblk.SgPinNum, bConnect, iMngStation, (ISystemManager) mgrSystem, nStepnum);

				if (!strPinHighTrg.Equals(ClsDefineTriggerTypeConstans.INTERNAL_TRIGGER) && !strPinHighTrg.Equals(ClsDefineTriggerTypeConstans.EMPTY_TRIGGER))
					if (tsManager.DicAnsteuerWithCtrBlock.ContainsKey(strPinHighTrg) || tsManager.DicAnsteuerWithCtrBlock.ContainsKey(strPinLowTrg))
					{
						UtilTextMessageEdits.UtilTextMsgToConsole("Load pin could not connect to MK for Trigger[" + strPinHighTrg + "," + strPinLowTrg + "], " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
						// Do not connect trigger to load pin.
						Debug.Assert(false);

						return false;
					}
					else
					{
						LstWiringInfo.AddRange(tsManager.CreateMKWiringMeasure(MESS_TYPE_ID.DVM_02_MESS.ToString(), strPinHighTrg, strPinLowTrg, bConnect, tsManager));
						((CpTsMacroMsDCTrgShell)cpTsParent).TriggerType = CpDefineTriggerType.EXT;
					}

				return true;
			});
            return tResult.Succeeded ? tResult.Result : false;
        }

        private bool createWiringDCV(ICbManager cbManager, IStnManager iMngStation, CpTsShell cpTsParent)
		{
            var tResult = TryFunc(() =>
            {
                PsCCSStdFnModuleE_SCANNER psModuleE_SCANNER = this.Core as PsCCSStdFnModuleE_SCANNER;
				Debug.Assert(psModuleE_SCANNER != null);

				string strUnit = string.Empty;
				if (!getScanUnitFromParam(psModuleE_SCANNER.SCAN_UNIT, ref strUnit, cpTsParent))
					Debug.Assert(false);

                //string strDeviceID = iMngStation.GetDevMapInfo().GetDeviceID(psModuleE_SCANNER.LstParameterWithValue[(int)STDFuncParmsE_SCANNER.SCAN_UNIT].Value);
                string strPinHigh = psModuleE_SCANNER.PIN_HIGH;
                string strPinLow = psModuleE_SCANNER.PIN_LOW;
                string strPinHighTrg = psModuleE_SCANNER.PIN_HIGH_TRG;
                string strPinLowTrg = psModuleE_SCANNER.PIN_LOW_TRG;

                string strUnitTrg = psModuleE_SCANNER.SCANN_UNIT_TRG;
                bool bConnect = psModuleE_SCANNER.SCHALTER_FUNKT == eSCHALTER_FUNKT.ON ? true : false;

                if (cbManager.DicAnsteuerWithCtrBlock.ContainsKey(strPinHigh) || cbManager.DicAnsteuerWithCtrBlock.ContainsKey(strPinLow))
				{
					UtilTextMessageEdits.UtilTextMsgToConsole("Load pin could not connect to MK for DCV [" + strPinHigh + "," + strPinLow + "], " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
					// dcv is not supported load pin.
					Debug.Assert(false);

					return false;
				}
				else
					LstWiringInfo = cbManager.CreateMKWiringMeasure(strUnit, strPinHigh, strPinLow, bConnect, cbManager);

				if (!strPinHighTrg.Equals(ClsDefineTriggerTypeConstans.INTERNAL_TRIGGER) && !strPinHighTrg.Equals(ClsDefineTriggerTypeConstans.EMPTY_TRIGGER))
					if (cbManager.DicAnsteuerWithCtrBlock.ContainsKey(strPinHighTrg) || cbManager.DicAnsteuerWithCtrBlock.ContainsKey(strPinLowTrg))
					{
						UtilTextMessageEdits.UtilTextMsgToConsole("Load pin could not connect to MK for Trigger [" + strPinHighTrg + "," + strPinHighTrg + "], " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
						// Do not connect trigger to load pin
						//Debug.Assert(false);

						return true;
					}
					else
					{
						LstWiringInfo.AddRange(cbManager.CreateMKWiringMeasure(MESS_TYPE_ID.DVM_02_MESS.ToString(), strPinHighTrg, strPinLowTrg, bConnect, cbManager));
						((CpTsMacroMsDCTrgShell)cpTsParent).TriggerType = CpDefineTriggerType.EXT;
					}

				return true;
			});
            return tResult.Succeeded ? tResult.Result : false;
		}

		private bool checkIsDCIFamily(CpTsShell cpTsParent, ref CpInstDCType dctype)
		{
			IDCV dcv = cpTsParent as IDCV;
			if (dcv != null)
			{
				dctype = CpInstDCType.DCV_FAMILY;
				return true;
			}

			IDCVTRG dcvtrg = cpTsParent as IDCVTRG;
			if (dcvtrg != null)
			{
				dctype = CpInstDCType.DCV_FAMILY;
				return true;
			}

			IRDCVTRG dcvrtrg = cpTsParent as IRDCVTRG;
			if (dcvrtrg != null)
			{
				dctype = CpInstDCType.DCV_FAMILY;
				return true;
			}

			ITIMEA timea = cpTsParent as ITIMEA;
			if (timea != null)
			{
				dctype = CpInstDCType.DCV_FAMILY;
				return true;
			}

			IRTIMEA rtimea = cpTsParent as IRTIMEA;
			if (rtimea != null) /* No action on RTIMEA, the E_SCANNER of RTIMEA is only for removed trigger */
			{
				dctype = CpInstDCType.NONE;
				return true;
			}

			IDCI dci = cpTsParent as IDCI;
			if (dci != null)
			{
				dctype = CpInstDCType.DCI_FAMILY;
				return true;
			}

			IDCIS dcis = cpTsParent as IDCIS;
			if (dcis != null)
			{
				dctype = CpInstDCType.DCI_FAMILY;
				return true;
			}

			IDCITRG dcitrg = cpTsParent as IDCITRG;
			if (dcitrg != null)
			{
				dctype = CpInstDCType.DCI_FAMILY;
				return true;
			}

			IRDCITRG rdcitrg = cpTsParent as IRDCITRG;
			if (rdcitrg != null)
			{
				dctype = CpInstDCType.NONE;
				return true;
			}

			return false;
		}

		public override bool createWiring(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent)
		{
            // Create connection info of Matrix
		    var tResult = TryFunc(() =>
		    {
		        int nStepnum = cpTsParent.Core.StepNum;
		        ICbManager mgrCtrblk = iMngStation.MngTStep.MngControlBlock;

		        CpInstDCType dctype = CpInstDCType.NONE;
		        if (!checkIsDCIFamily(cpTsParent, ref dctype))
		        {
		            UtilTextMessageEdits.UtilTextMsgToConsole("checkIsDCIFamily is failed", ConsoleColor.White);
		            return false;
		        }

		        switch (dctype)
		        {
		            case CpInstDCType.DCI_FAMILY:
		                if (!createWiringDCI(cpMngSystem, iMngStation, cpTsParent, nStepnum))
		                    return false;
		                break;
		            case CpInstDCType.DCV_FAMILY:
		                if (!createWiringDCV(mgrCtrblk, iMngStation, cpTsParent))
		                    return false;
		                break;
		            case CpInstDCType.NONE:
		                break;
		        }

		        return true;
		    });
            return tResult.Succeeded ? tResult.Result : false;
		}
	}
}
