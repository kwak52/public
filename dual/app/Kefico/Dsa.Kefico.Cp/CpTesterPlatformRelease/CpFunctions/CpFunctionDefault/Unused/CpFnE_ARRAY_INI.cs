using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using PsCommon;
using PsCommon.Enum;

namespace CpTesterPlatform.Functions
{
	public class CpFnE_ARRAY_INI : CpTsShell, IE_ARRAY_INI
    {
        /// <summary>
        /// E = ???
        /// ARRAY = ARRAY
        /// INI = INITIALIZATION
        /// E_ARRAY_INI = initialization an array of the internal variable to use in the test list.
        /// [CCS Doc]
        /// Initialization of array variables.
        /// </summary>    

        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem,  IStnManager iMngStation, CpTsShell cpTsParent = null)
        {

            PsCCSStdFnModuleE_ARRAY_INI psModuleE_ARRAY_INI = this.Core as PsCCSStdFnModuleE_ARRAY_INI;
            Debug.Assert(psModuleE_ARRAY_INI != null);
            string strArrayName = psModuleE_ARRAY_INI.ARRAY;
            //if (!cpMngSystem.MngTStep.GaudiReadData.GlobalVariable.ArstrGlobalVariable.Contains(strArrayName))
            //{
            //    UtilTextMessageEdits.UtilTextMsgToConsole("There is no variable name of " + strArrayName + " in the test list.", ConsoleColor.Red);
            //    this.ResultLog.TsActionResult = TsResult.ERROR;
            //    return base.ExecuteEpilog();
            //}

            string strArrayType = psModuleE_ARRAY_INI.ARRAY_TYP;
            CpStdFuncArrayType eArrayType = (CpStdFuncArrayType)Enum.Parse(typeof(CpStdFuncArrayType), strArrayType);

            string strArraySize = psModuleE_ARRAY_INI.ARRAY_DIM1;
            int nArraySize = Convert.ToInt32(strArraySize);
            eARRAY_CLEAR eArrayClear = psModuleE_ARRAY_INI.ARRAY_CLEAR;
            bool bArrayClear = Convert.ToBoolean(eArrayClear);
            CpGlbVarArray gv = new CpGlbVarArray(eArrayType, nArraySize, bArrayClear);

            //If there is a global variable in the list already, we have to replace it with variable array type which was created from here.
            if (iMngStation.MngTStep.GetMngGv().IsKeyContained(strArrayName))
                iMngStation.MngTStep.GetMngGv().ModifyData(strArrayName, gv);
            else
                iMngStation.MngTStep.GetMngGv().AddKeyData(strArrayName, gv);

            // log
            this.ResultLog.TsActionResult = TsResult.OK;
            return TsResult.OK;
        }
    }
}
