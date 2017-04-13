using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System;
using System.Reflection;
using System.Diagnostics;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// [CCS Doc]
    /// Connect the Ansteuer parameters waiting time with Unit.
    /// - 'WZ' use for the wait timer in the test list.
    /// - Since it is kind of the control block variable, it should be connect with the hardware object (timer). 
    /// </summary>
    /// <param name="WAIT_PARAMETER">Entering the Ansteuer parameter names the waiting time from the PAV.</param>
    /// <param name="FIRST_RUN_TIME">Enter the wait time in milliseconds but only in FirstRun. This time is added in addition to the normal waiting time.</param>
    /// 


    public class CpFnE_WAIT : CpTsShell, IE_WAIT
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            // main function.
            PsCCSStdFnModuleE_WAIT psModuleE_WAIT = this.Core as PsCCSStdFnModuleE_WAIT;
            Debug.Assert(psModuleE_WAIT != null);
			
            string strCtrBlockAnsteuerName = psModuleE_WAIT.WAIT_PARAMETER;
            string strWaitTime = psModuleE_WAIT.WAIT;
            string strWaitFunction = psModuleE_WAIT.WAIT_FUNKTION;
            string strFirstRunTime = psModuleE_WAIT.FIRST_RUN_TIME;
            string strVarReturn = psModuleE_WAIT.R_MESSWERT;

            System.Threading.Thread.Sleep(Convert.ToInt32(strWaitTime));
            return TsResult.NONE;
        }
    }
}
