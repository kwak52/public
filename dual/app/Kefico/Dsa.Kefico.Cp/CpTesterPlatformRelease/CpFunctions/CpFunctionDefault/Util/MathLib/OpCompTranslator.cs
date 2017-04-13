using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public class OpCompTranslator
    {
        public static OpEntityBase Translate(string strEq)
        {
            string strPreProc = PreprocessCompString(strEq);
            OpEntityBase entBase = AnalyzeCompConditionSyntax(strPreProc);

            entBase.EQUATION = strPreProc;

            return (entBase);
        }

        #region PREPROCESS
        static string PreprocessCompString(string strEqOrigin)
        {
            string strPreprocessed = RemoveWhiteSpace(strEqOrigin);

            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_EQUAL, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_DIFFERENT, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_BIGGEREQUAL, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_SMALLEREQUAL, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_BIGGER, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_SMALLER, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_AND, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.COMP_OR, 0);

            return strPreprocessed;
        }

        static string RemoveWhiteSpace(string strEqOrigin)
        {
            string strRemoved = strEqOrigin;

            while (true)
            {
                int nIdx = strRemoved.IndexOf(DefOperator.SPACE);

                if (nIdx == strRemoved.Length || nIdx == -1)
                    break;

                strRemoved = strRemoved.Remove(nIdx, 1);
            }

            return strRemoved;
        }

        static string SetPrimaryOpBracket(string strEqOrigin, string strCompOperator, int nStartIdx)
        {
            string strBracketed = strEqOrigin;
            bool bFound = false;
            int nIdx = nStartIdx + 1;
            char cCurVal = strBracketed.ToCharArray()[0];

            while (true)
            {
                if (nIdx == strBracketed.Length - 1 || nIdx == -1)
                    break;

                char cNextVal = strEqOrigin.ElementAt(nIdx);
                string strCmpStr = cCurVal.ToString() + cNextVal.ToString();

                if (strCmpStr == strCompOperator)
                {
                    bFound = true;

                    int nOpFIdx = SearchOperatorIdx(strBracketed, strCompOperator, nIdx - 1, true);
                    int nOpRIdx = SearchOperatorIdx(strBracketed, strCompOperator, nIdx + 1, false);

                    if (nOpFIdx > -1)
                        nOpFIdx++;
                    if (nOpRIdx >= strBracketed.Length)
                        nOpRIdx++;

                    if (SetBracketCover(ref strBracketed, nOpFIdx, nOpRIdx) == true)
                        nIdx++;

                    break;
                }
                else if (DefOperator.GetCompOperatorIdx(strCmpStr) == DefOperator.OPERATOR_COMP.ERROR && cNextVal.ToString() == strCompOperator)
                {
                    bFound = true;

                    int nOpFIdx = SearchOperatorIdx(strBracketed, strCompOperator, nIdx - 1, true);
                    int nOpRIdx = SearchOperatorIdx(strBracketed, strCompOperator, nIdx + 1, false);

                    if (nOpFIdx != -1)
                        nOpFIdx++;
                    if (nOpRIdx >= strBracketed.Length)
                        nOpRIdx++;

                    if (SetBracketCover(ref strBracketed, nOpFIdx, nOpRIdx) == true)
                        nIdx++;

                    break;
                }

                cCurVal = cNextVal;

                nIdx++;
            }

            if (bFound == true)
                strBracketed = SetPrimaryOpBracket(strBracketed, strCompOperator, nIdx + 1);

            return strBracketed;
        }

        static int SearchOperatorIdx(string strEq, string strTargetOp, int nCentreIdx, bool bFormer)
        {
            int nCurIdx = nCentreIdx;
            int nBracketCnt = 0;
            bool bFirstOpDet = false;
            char cExChar = ' ';

            while (true)
            {
                if (nCurIdx < 1)
                    break;
                else if (nCurIdx >= strEq.Length - 1)
                    break;

                char cCurVal = strEq.ElementAt(nCurIdx);
                string strCmpStr = (bFormer) ? cCurVal.ToString() + cExChar.ToString() : cExChar.ToString() + cCurVal.ToString();

                if (CompareCompOperatorValidity(strCmpStr, strTargetOp) == true)
                {
                    char cFormer = ' ';

                    if (nCurIdx != 0)
                        cFormer = strEq.ElementAt(nCurIdx - 1);

                    if (bFormer == true && nBracketCnt >= 0 && cFormer != DefOperator.BRACKET_B)
                        return nCurIdx;
                    else if (bFormer == false && nBracketCnt <= 0 && cFormer != DefOperator.BRACKET_B)
                        return nCurIdx;

                    bFirstOpDet = true;
                }

                cExChar = cCurVal;

                if (cCurVal == DefOperator.BRACKET_B)
                    nBracketCnt++;
                else if (cCurVal == DefOperator.BRACKET_E)
                    nBracketCnt--;

                if (bFormer == true)
                    nCurIdx--;
                else
                    nCurIdx++;
            }

            if (bFirstOpDet == false)
                return (bFormer == true) ? -1 : strEq.Length;
            else
                return (bFormer == true) ? -2 : strEq.Length;
        }

        static bool SetBracketCover(ref string strEq, int nBS, int nBE)
        {
            if (nBS < -1)
                return false;
            else
                strEq = strEq.Insert(nBS + 1, DefOperator.BRACKET_B.ToString());

            strEq = strEq.Insert(nBE, DefOperator.BRACKET_E.ToString());

            return true;
        }

        static bool CompareCompOperatorValidity(string strCurVal, string strTargetOp)
        {
            bool bResult = false;

            switch (strCurVal)
            {
                case DefOperator.COMP_AND:
                    bResult = true;
                    break;
                case DefOperator.COMP_OR:
                    bResult = true;
                    break;
                case DefOperator.COMP_EQUAL:
                    bResult = (strTargetOp == DefOperator.COMP_AND) || (strTargetOp == DefOperator.COMP_OR) ? true : false;
                    break;
                case DefOperator.COMP_DIFFERENT:
                    bResult = (strTargetOp == DefOperator.COMP_AND) || (strTargetOp == DefOperator.COMP_OR) ? true : false;
                    break;
                case DefOperator.COMP_BIGGEREQUAL:
                    bResult = (strTargetOp == DefOperator.COMP_AND) || (strTargetOp == DefOperator.COMP_OR) ? true : false;
                    break;
                case DefOperator.COMP_SMALLEREQUAL:
                    bResult = (strTargetOp == DefOperator.COMP_AND) || (strTargetOp == DefOperator.COMP_OR) ? true : false;
                    break;
                case DefOperator.COMP_SMALLER:
                    bResult = (strTargetOp == DefOperator.COMP_AND) || (strTargetOp == DefOperator.COMP_OR) ? true : false;
                    break;
                case DefOperator.COMP_BIGGER:
                    bResult = (strTargetOp == DefOperator.COMP_AND) || (strTargetOp == DefOperator.COMP_OR) ? true : false;
                    break;

                default:
                    bResult = false;
                    break;
            }

            return bResult;
        }
        #endregion

        #region CONSTRUCTOR
        public static OpEntityBase AnalyzeCompConditionSyntax(string strEq)
        {
            OpEntityBase entMain = null;
            string strResultEq = strEq;
            int nMidOpIdx = -1;
            string strCompOp = string.Empty;
            bool bIsCondition = DefOperator.IsCondition(strEq);

            try
            {
                FindMidOpIdx(strResultEq, ref nMidOpIdx, ref strCompOp);

                if (strResultEq == string.Empty)
                    return null;

                if (bIsCondition == true)
                    CheckBracketIntegrity(ref strResultEq, ref nMidOpIdx, ref strCompOp);

                if (bIsCondition == false)
                {
                    entMain = new OpEntityVar();
                    entMain.EQUATION = strResultEq;

                    MakeVarEntity((OpEntityVar)entMain);
                }
                else
                {
                    entMain = new OpEntityComp();
                    entMain.EQUATION = strResultEq;

                    MakeCompEntity(nMidOpIdx, strCompOp, (OpEntityComp)entMain);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CAL : " + ex.Message);
            }



            return entMain;
        }

        static void MakeCompEntity(int nMidOpIdx, string strCompOp, OpEntityComp entEq)
        {
            string strResultEq = entEq.EQUATION;
            string strFormerEq = strResultEq.Substring(0, nMidOpIdx - strCompOp.Length + 1);
            string strLatterEq = strResultEq.Substring(nMidOpIdx + 1, strResultEq.Length - nMidOpIdx - 1);
            char cOpMid = strResultEq.ElementAt(nMidOpIdx);

            OpEntityBase entLeft = (DefOperator.IsCondition(strFormerEq) == true) ? AnalyzeCompConditionSyntax(strFormerEq) : OpCalTranslator.Translate(strFormerEq);
            OpEntityBase entRight = (DefOperator.IsCondition(strLatterEq) == true) ? AnalyzeCompConditionSyntax(strLatterEq) : OpCalTranslator.Translate(strLatterEq);

            entLeft.PARENT = entEq;
            entRight.PARENT = entEq;
            ((OpEntityComp)entEq).OPERATOR = DefOperator.GetCompOperatorIdx(strCompOp);
            ((OpEntityComp)entEq).ENTITY_LEFT = entLeft;
            ((OpEntityComp)entEq).ENTITY_RIGHT = entRight;
        }

        static void MakeVarEntity(OpEntityVar entVar)
        {
            string strEqOrg = EliminateOuterBracket(entVar.EQUATION);

            if (DefOperator.IsConstant(strEqOrg) == true)
            {
                entVar.VARIABLE = DefOperator.CONSTANT;
                entVar.VALUE = strEqOrg;
            }
            else
            {
                if (strEqOrg == DefOperator.MATH_CONST_SYMBOL)
                {
                    entVar.VARIABLE = DefOperator.MATH_CONST_SYMBOL;
                    entVar.VALUE = DefOperator.MATH_CONST.ToString();
                }
                else
                {
                    entVar.VARIABLE = strEqOrg;
                    entVar.VALUE = string.Empty;
                }
            }
        }

        static void FindMidOpIdx(string strEq, ref int nMidOpIdx, ref string strCompOp)
        {
            int nCurIdx = 1;
            int nBracketCnt = 0;
            char cFormer = strEq.ToCharArray()[nCurIdx - 1];
            char cLatter = strEq.ToCharArray()[nCurIdx + 1];

            if (cFormer == DefOperator.BRACKET_B)
                nBracketCnt++;

            while (true)
            {
                if (nCurIdx < 0 || nCurIdx >= strEq.Length - 1)
                {
                    nMidOpIdx = -1;
                    strCompOp = string.Empty;
                    return;
                }

                char cCurVal = strEq.ElementAt(nCurIdx);
                cFormer = strEq.ToCharArray()[nCurIdx - 1];
                cLatter = strEq.ToCharArray()[nCurIdx + 1];

                string strCmpStr = cFormer.ToString() + cCurVal.ToString();
                string strCmpStrNxt = cCurVal.ToString() + cLatter.ToString();

                if (CompareCompOperatorValidity(strCmpStr, DefOperator.COMP_AND) == true && nBracketCnt == 0)
                {
                    nMidOpIdx = nCurIdx;
                    strCompOp = strCmpStr;

                    return;
                }

                else if (DefOperator.GetCompOperatorIdx(strCmpStr) == DefOperator.OPERATOR_COMP.ERROR &&
                        DefOperator.GetCompOperatorIdx(strCmpStrNxt) == DefOperator.OPERATOR_COMP.ERROR &&
                        DefOperator.GetCompOperatorIdx(cCurVal.ToString()) != DefOperator.OPERATOR_COMP.ERROR && nBracketCnt == 0)
                {
                    nMidOpIdx = nCurIdx;
                    strCompOp = cCurVal.ToString();

                    return;
                }

                if (cCurVal == DefOperator.BRACKET_B)
                    nBracketCnt++;
                else if (cCurVal == DefOperator.BRACKET_E)
                    nBracketCnt--;

                nCurIdx++;
            }
        }

        static int CheckBracketIntegrity(ref string strResultEq, ref int nMidOpIdxResult, ref string strTargetOpResult)
        {
            int nMidOpIdx = 0;
            string strTargetOp = string.Empty;

            try
            {
                while (true)
                {
                    FindMidOpIdx(strResultEq, ref nMidOpIdx, ref strTargetOp);

                    if (nMidOpIdx >= 0)
                        break;
                    else
                        strResultEq = RemoveOuterBracket(strResultEq);
                }

                nMidOpIdxResult = nMidOpIdx;
                strTargetOpResult = strTargetOp;
            }
            catch (Exception)
            {
                throw;
            }

            return nMidOpIdx;
        }

        static string EliminateOuterBracket(string strEq)
        {
            string strEqResult = strEq;

            if (strEq == string.Empty)
                return strEq;

            while (IsBracketRemovable(strEqResult) == true)
            {
                int nMidOpIdx = -1;
                string strCompOp = string.Empty;

                FindMidOpIdx(strEq, ref nMidOpIdx, ref strCompOp);

                if (nMidOpIdx == -1)
                    strEqResult = RemoveOuterBracket(strEqResult);
                else
                    break;
            }


            return strEqResult;
        }

        static string RemoveOuterBracket(string strEq)
        {
            string strEqResult = strEq;

            try
            {
                if (strEq == string.Empty)
                    return strEq;

                if (IsBracketRemovable(strEqResult) == true)
                {
                    strEqResult = strEqResult.Remove(strEqResult.Length - 1);
                    strEqResult = strEqResult.Remove(0, 1);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return strEqResult;
        }

        static bool IsBracketRemovable(string strEq)
        {
            if (strEq.ElementAt(strEq.Length - 1) == DefOperator.BRACKET_E && strEq.ElementAt(0) == DefOperator.BRACKET_B)
                return true;

            return false;
        }
        #endregion

    }
}
