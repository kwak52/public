using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
	public class CpFnR_STRCOPY : CpTsShell, IR_STRCOPY
    {
        /// <summary>
        /// With this module, the user can extract the substring (strings) from a string passed by or from a file and via '&' - pass variable.
        /// </summary>
        /// <param name="MODE"> 
        ///     INIT : The initial call to R_STRCOPY in a checklist has a (ignoring other parameters) to be with INIT.
        ///     MAKE :
        ///     READFILE :
        ///     EXTRACT :
        ///     * The default entry is '-' (minus) with the same meaning as INIT.
        /// </param>
        /// <param name="EXECUTION"> 
        ///     the user can be controlled with this, when the module is to be active
        ///     LOAD :
        ///     FIRST :
        ///     TEST :
        ///     * The default entry is '-' (minus) with the same meaning as LOAD.
        /// </param>
        /// <param name="INPUTSTRING"> 
        ///     the character string from which a substring is to be extracted.
        ///     this can also be a '&' - variable be.
        ///     the maximum length of a string is 80 characters (Corresponding to 80 bytes)
        /// </param>
        /// <param name="INPUTFILE"> </param>
        /// <param name="STARTINDEX"> the first byte (the string passed by, the file or the line of a file) to be extracted.
        ///      is counted from 1 to
        /// </param>
        /// <param name="STOPINDEX"> </param>
        /// <param name="FIELDNUMBER"> </param>
        /// <param name="SEPARATOR"> </param>
        /// <param name="OUTPUTSTRING"> </param>
        /// <param name="ERRORFLAG"> </param>

        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleR_STRCOPY psModuleR_STRCOPY = this.Core as PsCCSStdFnModuleR_STRCOPY;
            Debug.Assert(psModuleR_STRCOPY != null);

            ISTRCOPY cpTsSTRCOPY = cpTsParent as ISTRCOPY;
            if (cpTsSTRCOPY != null)
            {
                string strVars = string.Empty;
                string strVariable = psModuleR_STRCOPY.OUTPUTSTRING;
                string strInputString = psModuleR_STRCOPY.INPUTSTRING;
                int nStartIndex = Convert.ToInt16(psModuleR_STRCOPY.STARTINDEX);
                int nStopIndex = Convert.ToInt16(psModuleR_STRCOPY.STOPINDEX);

                // get global variable.
                if (!iMngStation.MngTStep.GetMngGv().IsKeyContained(strVariable))
                    Debug.Assert(false);

                if (nStartIndex > nStopIndex)
                    Debug.Assert(false);

                if (iMngStation.MngTStep.GetMngGv().IsKeyContained(strInputString))
                {
                    CpGlbVarBase retValue = iMngStation.MngTStep.GetMngGv().GetValue(strInputString);
                    strInputString = (string) retValue.RawValue;
                }

                if (strInputString.Length < nStopIndex)
                {
                    base.ResultLog.TsActionResult = TsResult.NG;
                    return TsResult.NG;
                }

                string strResult = strInputString.Substring(nStartIndex - 1, nStopIndex - nStartIndex + 1);

                iMngStation.MngTStep.GetMngGv().SetValue(strVariable, strResult, CpStringFormat.HEX);
                base.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }

            this.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.NONE;
        }
    }
}
