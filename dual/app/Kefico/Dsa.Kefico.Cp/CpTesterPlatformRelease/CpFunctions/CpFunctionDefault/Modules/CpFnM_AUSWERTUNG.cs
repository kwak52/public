using System;
using System.Diagnostics;
using System.Reflection;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using CpUtility.TextString;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// Task of the module it is, the measuring results by variables &werdenberg just to berprfen whether they lie in the tolerance.
    /// Thereafter, the measuring results , and The remaining parameters to the Process Measurement data collector passed.    
    /// </summary>    
    /// <param name="MESSWERT"> (ex. &MEWE) Variable from recording module.</param>
    /// <param name="Modulname"> Text for printer string. It is over a variable $ MO mountain just the Meobjekt.</param>    
    /// <param name="Messungsnummer"> (ex. 100) integer number.</param>
    /// <param name="Abweichungsart"> Answer the interface whether OK or error.</param>    
    /// <param name="Statistik"> .</param>    
    /// <param name="Klass_typ"> .</param>    
    /// <param name="FehlerArt"> .</param>    
    /// <param name="Dimension"> .</param>    
    /// <param name="TUG"> MIN.</param>    
    /// <param name="TOG"> MAX.</param>    
    /// <param name="Abgleichstelle"> .</param>    
    /// <param name="DimWiderstand"> .</param>    
    /// <param name="DimSteigung"> .</param>    
    /// <param name="Vorwiderstand"> .</param>    
    /// <param name="Vorsteigung"> .</param>    
    /// <param name="AktWiderstand"> .</param>    
    /// <param name="AktSteigung"> .</param>    
    /// <param name="AnzIterationen"> .</param>    
    /// <param name="ExxWiderstand"> .</param>    
    /// <param name="MG_FEHLER"> .</param>    
    /// <param name="NIO_AUSDRUCK"> .</param>    
    /// <param name="RETURN_MODE"> .</param>    
    /// <param name="RETURN_WERT"> .</param>    
    /// <param name="INPUT_FORMAT"> Indicates the interpretation of the input value reading.</param> 
    /// <param name="ERGEBNIS"> .</param>    
    /// </summary>    

    public class CpFnM_AUSWERTUNG : CpTsShell, IM_AUSWERTUNG
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            CpUtil.ConsoleWrite(iMngStation, MethodBase.GetCurrentMethod().ReflectedType.Name);
            PsCCSStdFnModuleM_AUSWERTUNG psModuleM_AUSWERTUNG = this.Core as PsCCSStdFnModuleM_AUSWERTUNG;
            Debug.Assert(psModuleM_AUSWERTUNG != null);

            CpSpecDimension cpDim = CpSpecDimension.NONE;

            if (!Enum.TryParse(psModuleM_AUSWERTUNG.DIMENSION, out cpDim))
                return TsResult.ERROR;
            if (cpDim == CpSpecDimension.STR || cpDim == CpSpecDimension.STRCMP || cpDim == CpSpecDimension.STRING)
                return TsResult.NONE;
            if (psModuleM_AUSWERTUNG.PairedParmsMinMax.Max == "" && psModuleM_AUSWERTUNG.PairedParmsMinMax.Min == "")
                return TsResult.NONE;

            TsResult judgeResult = TsResult.NONE;
            var inputValue = string.Empty;

            TryResultT<TsResult> oResult = TryFunc(() =>
            {
                if (iMngStation.MngTStep.GetMngGv().IsKeyContained(psModuleM_AUSWERTUNG.R_MESSWERT))
                    inputValue = iMngStation.MngTStep.GetMngGv().GetValue(psModuleM_AUSWERTUNG.R_MESSWERT).RawValue.ToString();
                else
                    inputValue = psModuleM_AUSWERTUNG.ReturnValue;

                if (inputValue == "")
                    return TsResult.ERROR;

                string strData;
                judgeResult = EvaluateData(cpDim, inputValue, psModuleM_AUSWERTUNG.PairedParmsMinMax.Max, psModuleM_AUSWERTUNG.PairedParmsMinMax.Min, out strData);
                (cpTsParent.ResultLog as ClsRlMeasuring).setLog(TsResult.OK, judgeResult, strData);

                return judgeResult;
            });

            if (oResult.HasException)
                return TsResult.ERROR;
            return judgeResult;
        }

        TsResult ComparesStringValues(string strValue, string strULim, string strLLim)
        {
            if (strValue == strULim || strValue == strLLim)
                return TsResult.OK;
            else
                return TsResult.NG;
        }

        TsResult ComparesNumericValues(double dVal, double dULim, double dLLim, bool bUpperMax = false, bool bLowerMin = false)
        {
            if (bUpperMax || bLowerMin)
            {
                if (bUpperMax)
                {
                    if (dVal >= dLLim) return TsResult.OK;
                    else return TsResult.NG;
                }
                else
                {
                    if (dVal <= dULim) return TsResult.OK;
                    else return TsResult.NG;
                }
            }
            else
            {
                if (dVal <= dULim && dVal >= dLLim)
                    return TsResult.OK;
                else
                    return TsResult.NG;
            }
        }

        /// <summary>
        /// Compares an input value with upper/lower limits in a defined dimension
        /// </summary>
        /// <param name="dimInfo"></param>
        /// <param name="strValue"></param>
        /// <param name="strULim"></param>
        /// <param name="strLLim"></param>
        /// <returns></returns>
        TsResult EvaluateData(CpSpecDimension dimInfo, string strValue, string strULim, string strLLim, out string strData)
        {
            TsResult exeResult = TsResult.NONE;
            var MultiVal = 1.0;
            strData = string.Empty;
            if (string.IsNullOrEmpty(strValue) || (string.IsNullOrEmpty(strULim) && string.IsNullOrEmpty(strLLim)))
                return TsResult.ERROR;

            bool bUpperMax = strULim == "" ? true : false;
            bool bLowerMin = strLLim == "" ? true : false;
            strULim = bUpperMax ? "0" : strULim;
            strLLim = bLowerMin ? "0" : strLLim;

            switch (dimInfo)
            {
                case CpSpecDimension.INT:
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.A:
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.MA:
                    MultiVal = Math.Pow(10, -3);
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.UA:
                    MultiVal = Math.Pow(10, -6);
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.V:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.MV:
                    MultiVal = Math.Pow(10, -3);
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.DEZ:
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.S:
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.MS:
                    MultiVal = Math.Pow(10, -3);
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.US:
                    MultiVal = Math.Pow(10, -6);
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.NUMERIC:
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.HEX:
                    break;
                case CpSpecDimension.STR:
                    exeResult = ComparesStringValues(strValue, strULim, strLLim);
                    break;
                case CpSpecDimension.STRING:
                    exeResult = ComparesStringValues(strValue, strULim, strLLim);
                    break;
                case CpSpecDimension.STRCMP:
                    exeResult = ComparesStringValues(strValue, strULim, strLLim);
                    break;
                case CpSpecDimension.EH:
                    exeResult = TsResult.ERROR;
                    break;
                case CpSpecDimension.EMPTY:
                    exeResult = TsResult.ERROR;
                    break;
                case CpSpecDimension.NONE:
                    exeResult = TsResult.ERROR;
                    break;
                case CpSpecDimension.RPM:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.ANGLE:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.OHM:
                    if (strULim == "-") strULim = Int64.MaxValue.ToString();
                    if (strLLim == "-") strLLim = Int64.MinValue.ToString();
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.UF:
                    MultiVal = Math.Pow(10, -6);
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.NF:
                    MultiVal = Math.Pow(10, -9);
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.HZ:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.SA:
                    break;
                case CpSpecDimension.BIN:
                    break;
                case CpSpecDimension.PCT:
                    exeResult = ComparesNumericValues(Convert.ToInt32(strValue), Convert.ToInt32(strULim) * MultiVal, Convert.ToInt32(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.SW:
                    break;
                case CpSpecDimension.FLOAT:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.MEGAOHM:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.GOHM:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                case CpSpecDimension.mOHM:
                    exeResult = ComparesNumericValues(Convert.ToDouble(strValue), Convert.ToDouble(strULim) * MultiVal, Convert.ToDouble(strLLim) * MultiVal, bUpperMax, bLowerMin);
                    break;
                default:
                    exeResult = TsResult.ERROR;
                    break;
            }
            strData = (Convert.ToDouble(strValue) / MultiVal).ToString();
            return exeResult;
        }
    }
}
