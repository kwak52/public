using System;
using System.Diagnostics;
using System.Reflection;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.TextString;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
	public class CpFnP_ZERLEGE_STR : CpTsShell, IP_ZERLEGE_STR
    {
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            var tResult = TryFunc(() =>
            {

                PsCCSStdFnModuleP_ZERLEGE_STR psModuleP_ZERLEGE_STR = this.Core as PsCCSStdFnModuleP_ZERLEGE_STR;
                Debug.Assert(psModuleP_ZERLEGE_STR != null);

                string strInputString = psModuleP_ZERLEGE_STR.INPUT_STRING;
                string strLowBytePos = psModuleP_ZERLEGE_STR.ZEIGER_ANF;
                string strHighBytePos = psModuleP_ZERLEGE_STR.ZEIGER_END;
                string strGvStoreValue = psModuleP_ZERLEGE_STR.ERGEBNIS;
                string strDimension = psModuleP_ZERLEGE_STR.DIMENSION;
                CpSpecDimension eDimension = CpSpecDimension.EMPTY;
                if (strDimension != ClsGlobalStringForTStep.CP_TS_PARM_EMPTY)
                    eDimension = (CpSpecDimension)Enum.Parse(typeof(CpSpecDimension), strDimension);
                CpStringFormat eFormat = UtilTextConvertOrParse.convertDimmensionToStringFormat(eDimension);

				#region BLOCKINT
				IBLOCKINT cpTsBLOCKINT = cpTsParent as IBLOCKINT;
                if (cpTsBLOCKINT != null)
                {
                    if (!cpMngSystem.CnfSystem.HardwareActivation.ActiveHwFncCAN)
                        return TsResult.NONE;

                    Stopwatch swLoopCtr = new Stopwatch();
                    swLoopCtr.Start();

                    while (true)
                    {
                        if (swLoopCtr.ElapsedMilliseconds > CPDefineTestTimeOut.CP_FT_TS_LOOP_TIME_OUT)
                        {
                            UtilTextMessageEdits.UtilTextMsgToConsole("ERROR : Time out in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red);                           
                            return TsResult.NG;
                        }
                       
                        // this is bottleneck for this step.
                        //if (cpMngSystem.MngHardware.MngCan.CANRxStatus == ThreadCpCANStatus.eTsCANRxStatus.READY_RX)
                        //    break;

                        // UtilTextMessageEdits.UtilTextMsgToConsole("[P_ZERLEGE] RX thread status = " + cpMngSystem.MngHardware.MngCAN.CANRxStatus.ToString());

                        System.Threading.Thread.Sleep(CPDefineSleepTime.CP_FT_SHORT_THREADING_SLEEP_TIME);
                    }

                    //Debug.Assert(cpMngSystem.MngHardware.MngCAN.getCrtActivatedCANPort().RxMsgByteBuffer.Count != 0);
					if(!iMngStation.MngTStep.GetMngGv().IsKeyContained(strGvStoreValue))
                        Debug.Assert(false);

                    // 'BLOCKINT' array index start at the "1".
                    int nLowBytePos = Convert.ToInt32(strLowBytePos);
                    int nHighBytePos = Convert.ToInt32(strHighBytePos);

                    Byte[] arBytes = new Byte[nHighBytePos - nLowBytePos + 1];
                    Byte[] arcrtRxBuffer = null;//cpMngSystem.MngHardware.MngCan.getCrtActivatedCANPort().getRxBuffer();

                    if (arcrtRxBuffer == null)
                    {
                        base.ResultLog.TsActionResult = TsResult.NG;
                        string strRet = BitConverter.ToString(arBytes);
                        iMngStation.MngTStep.GetMngGv().SetValue(strGvStoreValue, strRet, eFormat);
                    }

                    // positive response
                    if (arcrtRxBuffer.Length >= ((nLowBytePos - 1/*start*/) + (nHighBytePos - nLowBytePos + 1) /*length*/))
                    {
                        try
                        {
                            Debug.Assert(arcrtRxBuffer.Length >= nHighBytePos - nLowBytePos + 1);
                            Array.Copy(arcrtRxBuffer /*source*/,
                                   nLowBytePos - 1, arBytes, 0, nHighBytePos - nLowBytePos + 1);
                        }
                        catch (System.Exception ex)
                        {
                            UtilTextMessageEdits.UtilTextMsgToConsole("Failed to copy an array in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                            UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
                        }  
                    }                    

                    string strHex = BitConverter.ToString(arBytes);
                    strHex = strHex.Replace("-", "");
                    iMngStation.MngTStep.GetMngGv().SetValue(strGvStoreValue, strHex, eFormat);
                    
                    return TsResult.OK;
                }
				#endregion

				#region BLOCKINT_CP
				IBLOCKINT_CP cpTsBLOCKINT_CP = cpTsParent as IBLOCKINT_CP;
				if (cpTsBLOCKINT_CP != null)
                {
					if(iMngStation.MngTStep.GetMngGv().IsKindOfVariable(strInputString))
						strInputString = (string) iMngStation.MngTStep.GetMngGv().GetValue(strInputString).RawValue;

					int nDataStart = Convert.ToInt32(strLowBytePos);
					int nDataEnd = Convert.ToInt32(strHighBytePos);
								
					if(cpTsParent is IBLOCKINT_CP)
					{
						string strRxData = strInputString;
					
						if(nDataEnd < 0)
							nDataEnd = strRxData.Length + nDataEnd;

						string strValueResult = strRxData.Substring(nDataStart, nDataEnd-nDataStart);

						Console.WriteLine("**RESOURCE MUTUAL EXCLUSION CRASH TEST CODE** Station ID: " + iMngStation.StationId.ToString() + ", Value: " + strValueResult);
					
						iMngStation.MngTStep.GetMngGv().SetValue(strGvStoreValue, strValueResult, CpStringFormat.ASCII);
					}
                
					return TsResult.OK;
				}
				#endregion

                base.ResultLog.TsActionResult = TsResult.NONE;

                return TsResult.OK;
            });

            if (tResult.HasException)
            {
                this.ResultLog.TsActionResult = TsResult.ERROR;
                return TsResult.ERROR;
            }

            return tResult.Result;
        }
    }
}
