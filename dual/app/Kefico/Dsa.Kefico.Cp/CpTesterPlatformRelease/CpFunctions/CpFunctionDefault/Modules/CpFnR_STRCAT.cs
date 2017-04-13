using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System.Reflection;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// From the more than 10 individual strings (strings) a total string is formed by concatenation whose length must not exceed the one maximum length of 80 characters. Otherwise, an error message is displayed, and the passed (the GAUDI interface) result string contains no characters..
    /// </summary>    
    /// <param name="ASCIISTRING1"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING2"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING3"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING4"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING5"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING6"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING7"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING8"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING9"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="ASCIISTRING10"> constant string with max. 14 string, or by a '&' - Variable received string.</param>
    /// <param name="RESULTSTRING"> as the name implies, error flag, which is the law, when an error occurs during the conversion (for example, the case of invalid conversion). The parameter has become necessary to the M_AUSWERTUNG a NOK to convey..</param>

    public class CpFnR_STRCAT : CpTsShell, IE_ARRAY_INI
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);

            PsCCSStdFnModuleR_STRCAT psModuleR_STRCAT = this.Core as PsCCSStdFnModuleR_STRCAT;
            Debug.Assert(psModuleR_STRCAT != null);
            ISTRCAT cpTsSTRCAT = cpTsParent as ISTRCAT;
            if (cpTsSTRCAT != null)
            {
                string strVars = string.Empty;
                string strVariable = psModuleR_STRCAT.RESULTSTRING;
                // get global variable.
                if (!iMngStation.MngTStep.GetMngGv().IsKeyContained(strVariable))
                    Debug.Assert(false);

                string strResult = string.Empty;
                for (int i = 0; i < 10; i++)
                {
                    strVars = psModuleR_STRCAT.LstParameterWithValue[i].Value;
                    if (string.IsNullOrEmpty(strVars))
                        continue;
                    else
                    {
                        if (iMngStation.MngTStep.GetMngGv().IsKeyContained(strVars))
                        {
                            CpGlbVarBase retValue = iMngStation.MngTStep.GetMngGv().GetValue(strVars);
                            strResult += retValue.RawValue;
                        }
                        else
                        {
                            strResult += strVars;
                        }
                    }
                }

                iMngStation.MngTStep.GetMngGv().SetValue(strVariable, strResult, CpStringFormat.NONE);
                base.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }

            this.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.NONE;
        }
    }
}
