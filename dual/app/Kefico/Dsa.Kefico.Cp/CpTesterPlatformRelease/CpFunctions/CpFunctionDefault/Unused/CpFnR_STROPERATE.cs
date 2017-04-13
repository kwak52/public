using System;
using System.Diagnostics;
using System.Reflection;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Description of module parameters:
	/// </summary>    
	/// <param name="IN_FORMAT"> Specifies which display format the passed.</param> 
	/// <param name="INPUT_STRING1"> constant string with max. 14 characters in length, which represents a number in one of the previously EXPORTED representations or by a '&' - Variable received string with the same content (consisting of Possibly more than 14 characters, currently up to 80 characters);.</param>
	/// <param name="OPERATOR"> One of the following binary operations.</param>
	/// <param name="INPUT_STRING2"> constant string with max. 14 characters in length, which represents a number in (almost) any representation or by a '&' - Variable received string with the same content (consisting of Possibly more than 14 characters, currently up to 80 characters);</param>
	/// <param name="OUTPUT_STRING"> Output string, of the outcome of the Operation includes; Further communication to other modules is carried out by a '&'-Variable; The length of the output is equal to the length of the longest 'Input_string';.</param>    
	/// <param name="FEHLERFLAG"> as the name implies, error flag, which is the law, when an error occurs during the conversion (for example, the case of invalid conversion). The parameter has become necessary to the M_AUSWERTUNG a NOK to convey..</param>

	public class CpFnR_STROPERATE : CpTsShell, IR_STROPERATE
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleR_STROPERATE psModuleR_STROPERATE = this.Core as PsCCSStdFnModuleR_STROPERATE;
            Debug.Assert(psModuleR_STROPERATE != null);

            IBITOP cpTsBITOP = cpTsParent as IBITOP;
            if (cpTsBITOP != null)
            {
                string strInFormat = psModuleR_STROPERATE.IN_FORMAT;
                CpStringFormat eInFormat = (CpStringFormat)Enum.Parse(typeof(CpStringFormat), strInFormat);

                string strInStringVarInputFst = psModuleR_STROPERATE.INPUT_STRING1;
                // convert given string to the output string by the format.
                if (iMngStation.MngTStep.GetMngGv().IsKindOfVariable(strInStringVarInputFst))
                {
                    CpGlbVarBase gvGetVariable = iMngStation.MngTStep.GetMngGv().GetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsBITOP.MacroGlobalVar, strInStringVarInputFst);
                    strInStringVarInputFst = (string) gvGetVariable.RawValue;
                }

                string strInStringOperator = psModuleR_STROPERATE.OPERATOR;
                CpBitOperator eBitOperator = (CpBitOperator)Enum.Parse(typeof(CpBitOperator), strInStringOperator);

                string strInStringVarInputScd = psModuleR_STROPERATE.INPUT_STRING2;
                // convert given string to the output string by the format.
                if (iMngStation.MngTStep.GetMngGv().IsKindOfVariable(strInStringVarInputScd))
                {
                    CpGlbVarBase gvGetVariable = iMngStation.MngTStep.GetMngGv().GetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsBITOP.MacroGlobalVar, strInStringVarInputScd);
                    strInStringVarInputScd = (string) gvGetVariable.RawValue;
                }

                string strOutStringVar = psModuleR_STROPERATE.OUTPUT_STRING;
                string strResult = convertStringByGivenFormat(eInFormat, strInStringVarInputFst, eBitOperator, strInStringVarInputScd);

                iMngStation.MngTStep.GetMngGv().SetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsBITOP.MacroGlobalVar, strOutStringVar, strResult, eInFormat);
                base.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }
            else
                Debug.Assert(false);

            this.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.NONE;
        }

        public string convertStringByGivenFormat(CpStringFormat eInFormat, string strInStringVarInputFst, CpBitOperator eBitOperator, string strInStringVarInputScd)
        {
            var tResult = TryFunc(() =>
            {
                if (eInFormat == CpStringFormat.HEX)
                {
                    // decimal number format: Evaluation is carried out as an unsigned number; one set in the integer representation Hoechst significant bit has not the importance that the number is to be assessed as a negative number;

                    if (
                        !System.Text.RegularExpressions.Regex.IsMatch(strInStringVarInputFst, @"\A\b[0-9a-fA-F]+\b\Z") ||
                        !System.Text.RegularExpressions.Regex.IsMatch(strInStringVarInputScd, @"\A\b[0-9a-fA-F]+\b\Z"))
                    {
                        return TsResult.ERROR.ToString();
                    }

                    uint nInputFst = Convert.ToUInt32(strInStringVarInputFst, 16);
                    uint nInputScd = Convert.ToUInt32(strInStringVarInputScd, 16);
                    uint nResult = 0;
                    switch (eBitOperator)
                    {
                        case CpBitOperator.AND:
                        {
                            nResult = nInputFst & nInputScd;
                        }
                            break;
                        case CpBitOperator.OR:
                        {
                            nResult = nInputFst | nInputScd;
                        }
                            break;
                        case CpBitOperator.XOR:
                        {
                            nResult = nInputFst ^ nInputScd;
                        }
                            break;
                        case CpBitOperator.NAND:
                        {
                            nResult = ~(nInputFst & nInputScd);
                        }
                            break;
                        case CpBitOperator.NOR:
                        {
                            nResult = ~(nInputFst | nInputScd);
                        }
                            break;
                        case CpBitOperator.XNOR:
                        {
                            nResult = ~(nInputFst ^ nInputScd);
                        }
                            break;
                        default:
                        {
                            Debug.Assert(false);
                        }
                            break;
                    }

                    string strHexResult = nResult.ToString("X");
                    return strHexResult;
                }
                else
                {
                    Debug.Assert(false);
                    throw new Exception("Failure on convertStringByGivenFormat()");
                }
            });

            return tResult.Succeeded ? tResult.Result : TsResult.ERROR.ToString();
        }
    }
}
