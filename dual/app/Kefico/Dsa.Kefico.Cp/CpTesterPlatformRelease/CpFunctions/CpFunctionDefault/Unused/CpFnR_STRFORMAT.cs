using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
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
	/// Description of the module parameters.
	/// </summary>    
	/// <param name="INPUT_STRING"> constant string with max. 14 characters in length, which is a number in (almost) any representation Represents (in decimal form also signed), or by a '&' - Variable received string with the same content (consisting of Possibly more than 14 characters); Furthermore, can any strings are converted into their hexadecimal representation zugehoerende..</param>
	/// <param name="INPUT_FORMAT"> Specifies which display format the passed.</param>
	/// <param name="OUT_LEN"> The length of the converted into the desired format output strings (. number from 1 to 32 max, 0 creates an empty 'OUTPUT' string); are a number of zeros are preceded by the length - corr the CCS internal format - limited to 80 characters.</param>
	/// <param name="OUT_NACHKOMMA"> </param>
	/// <param name="OUT_NULLFUELL"> </param>
	/// <param name="OUT_FORMAT"> Specifies display format in which to be transferred 'INPUT STRING' converted:.</param>
	/// <param name="OUTPUT_STRING"> Output string, in which was 'INPUT STRING' converted; Further communication to other modules is carried out by a '&' - variable;.</param>
	/// <param name="FEHLERFLAG"> as the name implies, error flag, which is the law, when an error occurs during the conversion (for example, the case of invalid conversion). The parameter has become necessary to the M_AUSWERTUNG a NOK to convey..</param>

	public class CpFnR_STRFORMAT : CpTsShell, IR_STRFORMAT
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleR_STRFORMAT psModuleR_STRFORMAT = this.Core as PsCCSStdFnModuleR_STRFORMAT;
            Debug.Assert(psModuleR_STRFORMAT != null);

            string strInFormat = psModuleR_STRFORMAT.IN_FORMAT;
            CpStringFormat eInFormat = (CpStringFormat)Enum.Parse(typeof(CpStringFormat), strInFormat);

            string strInStringVar = psModuleR_STRFORMAT.INPUT_STRING;

            string strOutLenth = psModuleR_STRFORMAT.OUT_LEN;
            int nOutLength = Convert.ToInt32(strOutLenth);

            string strOutFormat = psModuleR_STRFORMAT.OUT_FORMAT;
            CpStringFormat eOutFormat = (CpStringFormat)Enum.Parse(typeof(CpStringFormat), strOutFormat);

            string strOutStringVar = psModuleR_STRFORMAT.OUTPUT_STRING;

            IBITOP cpTsBITOP = cpTsParent as IBITOP;
            IHEXSTRINGTOSTR cpTsHEXSTRINGTOSTR = cpTsParent as IHEXSTRINGTOSTR;
            ISTRTOINT cpTsSTRTOINT = cpTsParent as ISTRTOINT;
            IPRINTINT cpTsPRINTINT = cpTsParent as IPRINTINT;

            string strOutString = string.Empty;

            if (cpTsBITOP != null || cpTsHEXSTRINGTOSTR != null || cpTsSTRTOINT != null || cpTsPRINTINT != null)
            {
                IMacro cpTsMacroParent = cpTsParent as IMacro;
                CpStringFormat eInputFormat = eInFormat;
                // convert given string to the output string by the format.
                if (iMngStation.MngTStep.GetMngGv().IsKindOfVariable(strInStringVar))
                {
                    CpGlbVarBase gvGetVariable = iMngStation.MngTStep.GetMngGv().GetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsMacroParent.MacroGlobalVar, strInStringVar);
                    strInStringVar = (string) gvGetVariable.RawValue;
                    if (gvGetVariable.Format != CpStringFormat.NONE)
                        eInputFormat = gvGetVariable.Format;
                }

                /* Input data change format to INT */
                double dTempData = 0;

                switch (eInputFormat)
                {
                    case CpStringFormat.HEX:
                        {
                            dTempData = uint.Parse(strInStringVar, NumberStyles.AllowHexSpecifier);
                        }
                        break;
                    case CpStringFormat.OCT:
                        {
                            dTempData = Convert.ToInt64(strInStringVar, ClsDefineGlobalConstantcs.OCTA);
                        }
                        break;
                    case CpStringFormat.ASCII:
                        {
                            foreach (char c in strInStringVar)
                            {
                                dTempData = dTempData * ClsDefineGlobalConstantcs.BYTE_MAX + (int)c;
                            }
                        }
                        break;
                    case CpStringFormat.INT:
                    case CpStringFormat.FLOAT:
                        {
                            dTempData = Convert.ToDouble(strInStringVar);
                        }
                        break;
                    case CpStringFormat.BIN:
                        {
                            dTempData = Convert.ToInt64(strInStringVar, ClsDefineGlobalConstantcs.BINA);
                        }
                        break;
                    default:
                        {
                            Debug.Assert(false);
                        }
                        break;
                }

                switch (eOutFormat)
                {
                    case CpStringFormat.HEX:
                        {
                            ulong nInput = (ulong)dTempData;
                            string strHexFormat = "X" + nOutLength.ToString();
                            strOutString = nInput.ToString(strHexFormat);
                        }
                        break;
                    case CpStringFormat.ASCII:
                        {
                            StringBuilder sb = new StringBuilder();
                            while (dTempData > 0)
                            {
                                uint nMod = (uint)dTempData % ClsDefineGlobalConstantcs.BYTE_MAX;
                                sb.Insert(0, (char)nMod);
                                dTempData = (dTempData - nMod) / ClsDefineGlobalConstantcs.BYTE_MAX;
                            }

                            strOutString = sb.ToString();
                        }
                        break;
                    case CpStringFormat.INT:
                        {
                            strOutString = ((long)dTempData).ToString();
                            strOutString = strOutString.PadLeft(nOutLength, '0');
                        }
                        break;
                    case CpStringFormat.BIN:
                        {
                            strOutString = Convert.ToString(((long)dTempData), 2);
                            strOutString = strOutString.PadLeft(nOutLength, '0');
                        }
                        break;
                    case CpStringFormat.OCT:
                        {
                            strOutString = Convert.ToString((long)dTempData, ClsDefineGlobalConstantcs.OCTA);
                            strOutString = strOutString.PadLeft(nOutLength, '0');
                        }
                        break;
                    default:
                        {
                            Debug.Assert(false);
                        }
                        break;
                }

                if (strOutString.Length > nOutLength)
                    strOutString = strOutString.Substring(strOutString.Length - nOutLength);

                iMngStation.MngTStep.GetMngGv().SetValueWithGlobalLocalAll(iMngStation.MngTStep.GetMngGv(), cpTsMacroParent.MacroGlobalVar, strOutStringVar, strOutString, eOutFormat);
                base.ResultLog.TsActionResult = TsResult.OK;
            }

            return TsResult.OK;
        }

        public string convertStringByGivenFormat(CpStringFormat eInFormat, string strInString, CpStringFormat eOutFormat, int nOutLength)
        {
            var tResult = TryFunc(() =>
            {
                // INT > HEX
                if (eInFormat == CpStringFormat.INT && eOutFormat == CpStringFormat.HEX)
                {
                    uint nInput = 0;
                    if (!UInt32.TryParse(strInString, out nInput))
                    {
                        return double.NaN.ToString();
                    }
                    string strHexFormat = "X" + nOutLength.ToString();
                    return nInput.ToString(strHexFormat);
                }
                // BIN > INT
                if (eInFormat == CpStringFormat.BIN && eOutFormat == CpStringFormat.INT)
                {
                    return "0";
                }
                // HEX > ANCII
                else if (eInFormat == CpStringFormat.HEX && eOutFormat == CpStringFormat.ASCII)
                {
                    return ConvertHex(strInString);
                }
                else
                {
                    Debug.Assert(false);
                    throw new Exception($"Failed to convert string with given format: {strInString}, {eOutFormat}");
                }
            });

            return tResult.Succeeded ? tResult.Result : TsResult.ERROR.ToString();
        }

        public static string ConvertHex(String hexString)
        {
            var tResult = TryFunc(() =>
            {
                if (hexString.Length < 2)
                    return "NaN";

                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;
                }

                return ascii;
            });
            return tResult.Succeeded ? tResult.Result : string.Empty;
        }
    }
}
