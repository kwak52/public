using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
    public class CpFnP_AUFTRAG : CpTsShell, IP_AUFTRAG
    {
        /// <summary>
        /// This module is used for procurement of the PSS.
        /// This can any Strings to the PSS and to the SG be sent.
        /// The evaluation of the Reply is not in this module.
        /// The answer is in the shared memory stored and can, for example, there p_zerlege_string fetched from the module and are processed.
        /// </summary>    
        /// <param name="BEFEHL"> COMBLOCK (E : command)</param>
        /// <param name="DATUM"> Here the string to the PSS and the SG is to be entered.(ex. 99(H)).</param>    
        /// <param name="PARA1"> See command SEXY PAV.</param>
        /// <param name="PARA2"> See command SEXY PAV. (ex. 0.05).</param>
        /// <param name="PARA3"> 
        /// C : The strings in data are as long concatenated until here L, S or I shall be entered.</param>
        /// L : The string in data is sent to the PSS / SKM and waiting for response.
        /// <param name="PARA4"> Control Parameters for Future Use.</param>
        /// <param name="FEHLERFLAG"> (ex. &FEHLERFLAG) Set for errors that occurred.</param>
        /// <param name="ERGEBNIS"> 0(ex. &MEWE) Pointer to the response string of PSS / SKM.</param>
        /// <param name="ERGEBNIS_FELD"> .</param>
        /// </summary>    
		/// 
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            var tResult = TryFunc(() =>
            {
                PsCCSStdFnModuleP_AUFTRAG psModuleP_AUFTRAG = this.Core as PsCCSStdFnModuleP_AUFTRAG;
                Debug.Assert(psModuleP_AUFTRAG != null);

                // CAN2000INI does not use this function.
                ICAN2000INI cpTsCAN2000INI = cpTsParent as ICAN2000INI;
                if (cpTsCAN2000INI != null)
                {
                    this.ResultLog.TsActionResult = TsResult.OK;
                    return TsResult.OK;
                }

                string strCmd = psModuleP_AUFTRAG.BEFEHL;
                string strData = psModuleP_AUFTRAG.DATUM;
                string strParam1 = psModuleP_AUFTRAG.PARA1;
                string strParam2 = psModuleP_AUFTRAG.PARA2;
                string strParamCL = psModuleP_AUFTRAG.PARA3; // C = Continue, L = Last (get response message)
                string strParam4 = psModuleP_AUFTRAG.PARA4;

                ICOMBLOCK cpTsCOMBLOCK = cpTsParent as ICOMBLOCK;
                if (cpTsCOMBLOCK != null)
                {

                }


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
