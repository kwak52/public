using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using CpMathCalLib;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.Mathmatics;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Task of computing modules is to make calculations based on the input line.
	/// The result is to retransmit in the result string.
	/// The entire expression must be stapled.
	/// BERECHNUNG = CALCULATION
	/// </summary>    
	/// <param name="BERECHNUNG"> (ex. (&G_MEWE)/ (&G_HILF)) Description of arithmetical expression range the string of 60 characters.</param>
	/// <param name="ERGEBNIS"> (ex. &G_MEWEOUT) Outputting the result in a variable.</param>
	/// <param name="FEHLER"> (ex. --) Error description in a variable.</param>
	/// <param name="RUNDUNGSART"> (ex. 0) Run dart in integer operations (I don't know 2016.04.22).</param>

	public class CpFnR_RECHNE : CpTsShell, IR_RECHNE
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);

            PsCCSStdFnModuleR_RECHNE psModuleR_RECHNE = this.Core as PsCCSStdFnModuleR_RECHNE;
            Debug.Assert(psModuleR_RECHNE != null);

            #region PsCCSDefineEnumSTDFunction.
            // examples used in the SIM2K-260
            // (&E1_GETDATASTR+&E2_GETDATASTR+&E3_GETDATASTR+&E4_GETDATASTR)
            // (&G_MEWE)/ (&G_HILF)
            // (&G_FEHL1+&G_FEHL2+&G_FEHL3)
            // (&G_MEWE)/(&G_HILF_DCITRG)
            // (&G_MEWE_DCI)/(&G_HILF_DCITRG)
            // (&G_MEWE)/ (&G_HILF)
            // (20*LOG10((&V_MAX1TO2+&V_MIN1TO2)*5/32767/4))
            #endregion

			string strMathExpressionWithVars = psModuleR_RECHNE.BERECHNUNG;
			string strRoundVar = psModuleR_RECHNE.RUNDUNGSART;
			CpDefineRound eRoundVars = CpDefineRound.NO_ROUNDING;               
            string strReturn = psModuleR_RECHNE.ERGEBNIS;
			int nRoundVars = 0;
			Double dResult = -9999999;
			string strMathExpressionWithValues = string.Empty;

			 if (strRoundVar != string.Empty || strRoundVar != "-" || Int32.TryParse(strRoundVar, out nRoundVars))
				eRoundVars = (CpDefineRound) nRoundVars;

			//WEqData eqResult = new WEqData(strMathExpressionWithVars);
			
            #region BERECHNEN.
            // assign the value calculated from here to the parent function.
            IBERECHNEN cpTsBerechnen = cpTsParent as IBERECHNEN;
            if (cpTsBerechnen != null)
            {                        				
                if (strMathExpressionWithVars.Contains(ClsGlobalStringForTStep.CP_TS_PARM_MATH_LOG_10))
				{
					strMathExpressionWithVars = (strMathExpressionWithVars.Replace(ClsGlobalStringForTStep.CP_TS_PARM_MATH_LOG_10, ClsGlobalStringForTStep.CP_TS_PARM_MATH_LOG));

					// 01. find variables from the expression. ex.&E1_GETDATASTR, &E2_GETDATASTR, &E3_GETDATASTR, &E4_GETDATASTR
					List<string> lstVariables = CpComMathLib.getVariableinMathExpression(strMathExpressionWithVars);
					foreach (string strVarInMath in lstVariables)
					{
						CpGlbVarBase gvGetVariable = iMngStation.MngTStep.GetMngGv().GetValueWithGlobalLocalAll(
							iMngStation.MngTStep.GetMngGv(), cpTsBerechnen.MacroGlobalVar, strVarInMath);

						// 02. convert HEX string to the integer value.
						string strVarValue = (string) gvGetVariable.RawValue;
						if (gvGetVariable.Format == CpStringFormat.HEX)
							//if (System.Text.RegularExpressions.Regex.IsMatch(strVarValue, @"\A\b[0-9a-fA-F]+\b\Z"))
							strVarValue = uint.Parse((string) gvGetVariable.RawValue, NumberStyles.AllowHexSpecifier).ToString();

						// 03. set the value to the expression.                
						//strMathExpressionWithVars = strMathExpressionWithVars.Replace(strVarInMath, string.Format("({0})",strVarValue));
						int nIndexVal = strMathExpressionWithVars.IndexOf(strVarInMath);
						int nLengthVal = strVarInMath.Length;

						if (nIndexVal < 0)
						{
							UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Find String : " + strVarInMath + " in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
							UtilTextMessageEdits.UtilTextMsgToConsole("strMathExpressionWithVars : " + strMathExpressionWithVars, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
						}
						else
						{
							strMathExpressionWithVars = strMathExpressionWithVars.Remove(nIndexVal, nLengthVal).Insert(nIndexVal, string.Format("({0})", strVarValue));
						}
						
						strMathExpressionWithValues = strMathExpressionWithVars;

						// 04. solve the equation.
						dResult = CpComMathLib.getValueWithMathExpression(strMathExpressionWithValues);
					}
                }    
				else
				{
					CpMathEqCal cpMathEq = new CpMathEqCal(strMathExpressionWithVars);//

					List<string> vstrGbRst = cpMathEq.GetVariableList();

                    if (vstrGbRst != null)
                    {
                        foreach (string strGb in vstrGbRst)
                        {
                            double dVal = Convert.ToDouble(iMngStation.MngTStep.GetMngGv().GetValue(strGb).RawValue);

                            cpMathEq.SetVariableValue(strGb, dVal.ToString());
                        }
                    }

					string strResult = cpMathEq.GetResult();

					if(string.IsNullOrEmpty(cpMathEq.ErrorMessge))
						dResult = Convert.ToDouble(strResult);
					else
						return TsResult.ERROR;
				}            

                // 05. Round Off the value.
                /*
                Specifies whether and how the result is to be rounded.
                 0= No rounding; the result is a floating-point number
                 1= Round to nearest whole number (2.4 becomes 2 and 2.6 becomes 3)
                 2= Round down to next whole number
                 3= Round up to next whole number
                */
                switch (eRoundVars)
                {
                    case CpDefineRound.NO_ROUNDING:
                        dResult = Math.Round(dResult, ClsDefineGlobalConstMath.MATH_ROUND_DIGIT);
                        break;
                    case CpDefineRound.ROUND:
                        dResult = Math.Round(dResult);
                        break;
                    case CpDefineRound.ROUND_DOWN:
                        dResult = Math.Truncate(dResult);
                        break;
                    case CpDefineRound.ROUND_UP:
                        dResult = Math.Ceiling(dResult);
                        break;
                }

                // [DEBUG]
                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Math Expression : {0} = {1}", strMathExpressionWithValues, dResult)
                    , ConsoleColor.Cyan);

                //Debug.Assert(dResult != -1);
                cpTsBerechnen.setEquationResult(dResult);

                if (strReturn != string.Empty)
                {
                    iMngStation.MngTStep.GetMngGv().SetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsBerechnen.MacroGlobalVar, strReturn, dResult.ToString(), CpStringFormat.FLOAT);
                }

                base.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }
            #endregion           

            this.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.NONE;
        }
    }
}
