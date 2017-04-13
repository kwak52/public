using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon.Enum;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System.Reflection;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// Connect with driving parameters Unit.
    /// </summary>    
    /// <param name="ANSTEUER_UNIT"> .</param>
    /// <param name="ANSTEUER_NAME"> This parameter is the name of the PAV as driving parameters is declared.</param>
    /// <param name="ANSTEUER_WERT"> Control parameter value from the PAV.</param>
    /// <param name="ANSTEUER_DIM"> Control parameter dimension of the PAV.</param>
    /// <param name="ANSTEUER_PIN"> SG pin of the PAV.</param>
    /// <param name="ANSTEUER_BEZUG"> Reference Pin from the PAV.</param>
    /// <param name="ANSTEUER_ART"> This parameter can be specified whether the hardware to be controlled or control parameters of the module parameters.</param>
    /// <param name="ANSTEUER_FAKTO"> .</param>

    public class CpFnF_ANSTEUER : CpTsShell, IF_ANSTEUER
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            // main function.
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);

            // load test list from the configuration file.
            PsCCSStdFnModuleF_ANSTEUER psModuleF_ANSTEUER = this.Core as PsCCSStdFnModuleF_ANSTEUER;
            Debug.Assert(psModuleF_ANSTEUER != null);

            // /*K41_SV*/
            string strCtrBlockDevUnit = psModuleF_ANSTEUER.ANSTEUER_UNIT;
            string strCtrBlockAnsteuerName = psModuleF_ANSTEUER.ANSTEUER_NAME;
            string strCtrBlockValue = psModuleF_ANSTEUER.ANSTEUER_WERT;
            eANSTEUER_DIM eCtrBlockDim = psModuleF_ANSTEUER.ANSTEUER_DIM;
            string strSgPinNum = psModuleF_ANSTEUER.ANSTEUER_PIN;
            string strCtrBlockPinRefNum = psModuleF_ANSTEUER.ANSTEUER_BEZUG;
            string strCtrBlockMode = psModuleF_ANSTEUER.ANSTEUER_ART;
            string strCtrBlockFactor = psModuleF_ANSTEUER.ANSTEUER_FAKTO;

            // U_KON_UA4;XLE04.d18;K41;
            CpAdtCnf adtCnf = iMngStation.MngTStep.MngControlBlock.getAdapterCnfByMslUnitWithPinNum(strCtrBlockDevUnit/*ex. U_KON_UA4*/);//, strSgPinNum /*ex. K41*/);

            if (adtCnf != null) // it was created from the adapter configuration file.
            {
                ClsTsCtrBlockWithAdapter tsCtrBlockAnsteuer = new ClsTsCtrBlockWithAdapter(strCtrBlockDevUnit, strCtrBlockAnsteuerName, strCtrBlockMode, adtCnf, strSgPinNum);

                if (!iMngStation.MngTStep.MngControlBlock.DicAnsteuerWithCtrBlock.ContainsKey(strCtrBlockAnsteuerName))
                    iMngStation.MngTStep.MngControlBlock.DicAnsteuerWithCtrBlock.Add(strCtrBlockAnsteuerName, tsCtrBlockAnsteuer);
                else
                {
                    ClsTsCtrBlockBase existTsCtrBlockAnsteuer = null;
                    iMngStation.MngTStep.MngControlBlock.DicAnsteuerWithCtrBlock.TryGetValue(strCtrBlockAnsteuerName, out existTsCtrBlockAnsteuer);
                    if (tsCtrBlockAnsteuer.AdtCnf.getPinName().RawString != ((ClsTsCtrBlockWithAdapter)(existTsCtrBlockAnsteuer)).AdtCnf.getPinName().RawString)
                    {
                        UtilTextMessageEdits.UtilTextMsgToConsole("Fetal ERROR : Key Redundant && Different Pin Name." + strCtrBlockDevUnit, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                        Debug.Assert(false);
                    }
                }
            }
            else
            {
                ControBlockProperty eCtrBlockProperty = strCtrBlockDevUnit == ControBlockProperty.SCHALTER.ToString() ? ControBlockProperty.SCHALTER : ControBlockProperty.VARIABLE;

                // Changed code for checking the FGN parameter in E_OUT.
                string strCtrBlockMslUnit = strCtrBlockDevUnit;// strMslUnit.Contains("FGN") ? strMslUnit : string.Format("{0}_{1}", eCtrBlockProperty.ToString(), strSgPinNum);

                CpAdtCnfBase adtCnfScharlter = new CpAdtCnfBase(new CpAdtMslUnit(strCtrBlockMslUnit), eCtrBlockProperty);
                ClsTsCtrBlockWithAdapter tsCtrBlockAnsteuer = new ClsTsCtrBlockWithAdapter(strCtrBlockDevUnit, strCtrBlockAnsteuerName, strCtrBlockMode, adtCnfScharlter, strSgPinNum);
                iMngStation.MngTStep.MngControlBlock.DicAnsteuerWithCtrBlock.Add(strCtrBlockAnsteuerName, tsCtrBlockAnsteuer);

                if (eCtrBlockProperty == ControBlockProperty.VARIABLE)
                    UtilTextMessageEdits.UtilTextMsgToConsole(strCtrBlockAnsteuerName + "\t" + strCtrBlockDevUnit + "\t" + strSgPinNum + "\t", ConsoleColor.White);
            }

            // log
            this.ResultLog.TsActionResult = TsResult.OK;
            return TsResult.OK;
        }
    }
}
