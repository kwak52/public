using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CpMathCalLib;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.Mathmatics;
using PsCommon;
using PsCommon.Enum;
using PsKGaudi.Parser.PsCCSSTDFn;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// Calling the process VXI_DMM.
    /// The object of the function is a branch symbol with a Bedingsymbol zussammen to lead, and Vector interpreter possibly a to leave by lead branch. carried out occupation of the variable often by the function M_ZUWEIS. In order to be able to performing any comparison,
    /// mu of variable type (INT / FLOAT / STRING) can be entered.
    /// Variables that will come from standard macros automatically with the correct type provided.
    /// </summary>  

    public class CpFnM_CONTROL : CpTsShell, IM_CONTROL
    {
        static public CpSpecControlAction controlActionStatus = CpSpecControlAction.NONE;

       protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
       {           
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            ResultLog.TsActionResult = TsResult.NONE;

           try
           {
               // Measuring Creating Scanner

               PsCCSStdFnModuleM_CONTROL psModuleM_CONTROL = this.Core as PsCCSStdFnModuleM_CONTROL;
               Debug.Assert(psModuleM_CONTROL != null);

               // assign the value calculated from here to the parent function.
               ICONTROL cpTsControl = cpTsParent as ICONTROL;
               if (cpTsControl != null)
               {
                   if (iMngStation.MngTStep.TsWhileLoopCounter > ClsDefineControlLoopConstants.WHILE_LOOP_LIMIT)
                   {
                       iMngStation.MngTStep.TsWhileLoopCounter = 0;
                       
                       return TsResult.NONE;
                   }

                   eVERZWEIGUNG eBranch = psModuleM_CONTROL.VERZWEIGUNG;
                   string strMathExpressionWithVars = psModuleM_CONTROL.BEDINGUNG;
                   string strVarType = psModuleM_CONTROL.PARA_7;

                   switch (eBranch)
                   {
                       case eVERZWEIGUNG.IF:
                           {        
                                string strMathExpressionWithValues = strMathExpressionWithVars;
                                CpMathCondComp mathComp = new CpMathCondComp(strMathExpressionWithVars);
                                List<string> vstrGbRst = mathComp.GetVariableList();

                                if(vstrGbRst != null)
                                {
                                    foreach (string strGb in vstrGbRst)
                                    {
                                        double dVal = Convert.ToDouble(iMngStation.MngTStep.GetMngGv().GetValue(strGb).RawValue);

                                        mathComp.SetVariableValue(strGb, dVal.ToString());
                                    }
                                }

                                string strResult = mathComp.GetResult();
                                if (string.IsNullOrEmpty(strResult))
                                    break;

                                bool bCondtionResult = Convert.ToBoolean(strResult);                                

                                try
                                {
                                    if (bCondtionResult)
                                    {
                                        controlActionStatus = CpSpecControlAction.DO_IF;
                                    }
                                    else
                                    {
                                        controlActionStatus = CpSpecControlAction.GO_ELSE;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to convert given value to the boolean value in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                                    UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
                                }

                                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Condition = {0}, Result = {1}", strMathExpressionWithVars, bCondtionResult.ToString()), ConsoleColor.Yellow);
                           }
                           break;
                       case eVERZWEIGUNG.ELSE:
                           {
                               switch (controlActionStatus)
                               {
                                   case CpSpecControlAction.DO_IF:
                                       controlActionStatus = CpSpecControlAction.GO_ENDIF;
                                       break;
                                   case CpSpecControlAction.GO_ELSE:
                                       controlActionStatus = CpSpecControlAction.DO_ELSE;
                                       break;
                                   default:
                                       Debug.Assert(false);
                                       break;
                               }
                           }
                           break;
                       case eVERZWEIGUNG.ENDIF:
                           {
                               controlActionStatus = CpSpecControlAction.NONE;
                           }
                           break;
                       case eVERZWEIGUNG.WHILE:
                           {
                               // 01. find variables from the expression. ex.&E1_GETDATASTR, &E2_GETDATASTR, &E3_GETDATASTR, &E4_GETDATASTR  
							   CpMathCondComp mathComp = new CpMathCondComp(strMathExpressionWithVars);

							   List<string> vstrGbRst = mathComp.GetVariableList();

								if (vstrGbRst != null)
								{
									foreach (string strGb in vstrGbRst)
									{
										double dVal = Convert.ToDouble(iMngStation.MngTStep.GetMngGv().GetValue(strGb).RawValue);

										mathComp.SetVariableValue(strGb, dVal.ToString());
									}
								}

                               bool bCondtionResult = Convert.ToBoolean(mathComp.GetResult());

                               try
                               {
                                   //bCondtionResult = Convert.ToBoolean(dResult);
                               }
                               catch (System.Exception ex)
                               {
                                   UtilTextMessageEdits.UtilTextMsgToConsole("Failed to convert given value to the boolean value in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                                   UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
                               }

                               UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Condition = {0}, Result = {1}", strMathExpressionWithVars, bCondtionResult.ToString()), ConsoleColor.Yellow);

                               if (!bCondtionResult)
                               {
                                   // escape the loop
                                   // find a step name of 'ENDWHILE' below this step number.

                                   int nTStepIndex = iMngStation.MngTStep.TsCurrentNumIndex;
                                   int nTargetNum = cpTsControl.PairControlFnStepNum;
                                   int nTargetIndex = iMngStation.MngTStep.GetTStepIndexByNum(nTargetNum);
                                   iMngStation.MngTStep.TsCurrentNumIndex = nTargetIndex;
                                   iMngStation.MngTStep.TsWhileLoopCounter = 0;
                               }
                               else
                                   iMngStation.MngTStep.TsWhileLoopCounter++;
                           }
                           break;
                       case eVERZWEIGUNG.ENDWHILE:
                           {                              
                               // go to while before this step.
                               int nTargetNum = cpTsControl.PairControlFnStepNum;
                               int nTargetIndex = iMngStation.MngTStep.GetTStepIndexByNum(nTargetNum);
                               iMngStation.MngTStep.TsCurrentNumIndex = nTargetIndex - 1; // before while
                           }
                           break;
                   }
               }

               return TsResult.OK;
           }
           catch (System.Exception ex)
           {
               UtilTextMessageEdits.UtilTextMsgToConsole("Failed to execute test step in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
               UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
           }
            return TsResult.ERROR;
       }

       static bool checkCondition(string strLeftCmp, string strCondition, string strRightCmp)
       {
           try
           {
               if (strCondition == CPDefineControlCondition.CP_FT_CTR_CONDITION_EQUAL)
               {
                   return string.Compare(strLeftCmp, strRightCmp) == 0 ? true : false;
               }
               else if (strCondition == CPDefineControlCondition.CP_FT_CTR_CONDITION_NOT_EQUAL)
               {
                   return string.Compare(strLeftCmp, strRightCmp) != 0 ? true : false;
               }
               else if (strCondition == CPDefineControlCondition.CP_FT_CTR_CONDITION_BIGGER)
               {
                   Debug.Assert(false);
               }
               else if (strCondition == CPDefineControlCondition.CP_FT_CTR_CONDITION_SMALLER)
               {
                   Debug.Assert(false);
               }
               else if (strCondition == CPDefineControlCondition.CP_FT_CTR_CONDITION_OR)
               {
                   Debug.Assert(false);
               }
               else
               {
                   Debug.Assert(false);
               }

               return false;
           }
           catch (System.Exception ex)
           {
               UtilTextMessageEdits.UtilTextMsgToConsole("Failed to check the condition of the 'CONTROL' function test step in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
               UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
           }
           return false;
       }
    }
}
