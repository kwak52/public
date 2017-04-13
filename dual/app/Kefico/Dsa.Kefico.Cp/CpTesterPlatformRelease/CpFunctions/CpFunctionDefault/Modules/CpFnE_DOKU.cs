using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
    public class CpFnE_DOKU : CpTsShell, IE_DOKUcs
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            // main function.
            // load test list from the configuration file.
            PsCCSStdFnModuleBase psModuleE_DOCU = this.Core as PsCCSStdFnModuleBase;
            Debug.Assert(psModuleE_DOCU != null);
            string strCnfFileName = psModuleE_DOCU.GetValueWithParm(ClsGlobalStringForTStep.CP_TS_PARM_ADAPTER_FILE);
            var sFilePath = Directory.GetCurrentDirectory() + "\\Configure\\HwConfig\\" + strCnfFileName + ".cnf";
            if (!File.Exists(sFilePath))
            {
                MessageBox.Show("HwConfig file loading Error.\r\n" + sFilePath, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResultLog.TsActionResult = TsResult.ERROR;
            }
            else
            {
                ResultLog.TsActionResult = loadingAdapterFile(iMngStation.MngTStep.MngControlBlock, sFilePath) ? TsResult.OK : TsResult.NG;
            }

            return ResultLog.TsActionResult;
        }

        public static bool loadingAdapterFile(ICbManager cpMngControlBlock, string strSelFileName)
        {
            var oResult = TryFunc(() =>
            {
                List<CpAdtCnf> lstLoadedCpCnf = (new CpCbManager()).readCpConfirationFile(strSelFileName, ControBlockProperty.MUX);
                if (cpMngControlBlock.LstLoadedAdapterCnf != null)
                {
                    cpMngControlBlock.LstLoadedAdapterCnf.AddRange(lstLoadedCpCnf);
                    return true;
                }
                else
                    return false;
            });

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("[Thread Error] in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }

            return oResult.Result;
        }
    }
}
