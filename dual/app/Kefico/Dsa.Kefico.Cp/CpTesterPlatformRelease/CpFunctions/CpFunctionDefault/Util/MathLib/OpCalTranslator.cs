using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public static class OpCalTranslator
    {
        public static OpEntityBase Translate(string strEq)
        {
            string strPreProc = PreprocessEqString(strEq);
            OpEntityBase entBase = AnalyzeMathSyntax(strPreProc);

            entBase.UpdateEquation();

            string strEq1 = entBase.EQUATION;

            return (entBase);
        }

        #region PREPROCESS
        static string PreprocessEqString(string strEqOrigin)
        {
            string strPreprocessed = RemoveWhiteSpace(strEqOrigin);

            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.SQUARE, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.DIVIDER, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.MULTIPLIER, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.PLUS, 0);
            strPreprocessed = SetPrimaryOpBracket(strPreprocessed, DefOperator.MINUS, 0);

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

        static string SetPrimaryOpBracket(string strEqOrigin, char cOperator, int nStartIdx)
        {
            string strBracketed = strEqOrigin;
            bool bFound = false;
            int nIdx = nStartIdx;

            while (true)
            {
                if (nIdx == strBracketed.Length || nIdx == -1)
                    break;

                char cOpVal = strEqOrigin.ElementAt(nIdx);

                if (cOpVal == cOperator)
                {
                    bFound = true;

                    int nOpFIdx = SearchOperatorIdx(strBracketed, cOperator, nIdx - 1, true);
                    int nOpRIdx = SearchOperatorIdx(strBracketed, cOperator, nIdx + 1, false);

                    if (SetBracketCover(ref strBracketed, nOpFIdx, nOpRIdx) == true)
                        nIdx++;

                    break;
                }

                nIdx++;
            }

            if (bFound == true)
                strBracketed = SetPrimaryOpBracket(strBracketed, cOperator, nIdx + 1);

            return strBracketed;
        }

        static int SearchOperatorIdx(string strEq, char cTargetOp, int nCentreIdx, bool bFormer)
        {
            int nCurIdx = nCentreIdx;
            int nBracketCnt = 0;
            bool bFirstOpDet = false;

            while (true)
            {
                if (nCurIdx < 0)
                    break;
                else if (nCurIdx >= strEq.Length)
                    break;

                char cCurVal = strEq.ElementAt(nCurIdx);

                if (CompareOperatorValidity(cCurVal, cTargetOp) == true)
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

            strEq = strEq.Insert(nBE + 1, DefOperator.BRACKET_E.ToString());

            return true;
        }

        static bool CompareOperatorValidity(char cCurVal, char cTargetOp)
        {
            bool bResult = false;

            switch (cCurVal)
            {
                case DefOperator.DIVIDER:
                    bResult = (cTargetOp != DefOperator.PLUS && cTargetOp != DefOperator.MINUS) ? true : false;
                    break;
                case DefOperator.MULTIPLIER:
                    bResult = (cTargetOp != DefOperator.PLUS && cTargetOp != DefOperator.MINUS) ? true : false;
                    break;
                case DefOperator.PLUS:
                    bResult = true;
                    break;
                case DefOperator.MINUS:
                    bResult = true;
                    break;
                case DefOperator.SQUARE:
                    bResult = (cTargetOp == DefOperator.SQUARE) ? true : false;
                    break;
                default:
                    bResult = false;
                    break;
            }

            return bResult;
        }
        #endregion

        #region CONSTRUCTOR
        public static OpEntityBase AnalyzeMathSyntax(string strEq)
        {
            OpEntityBase entMain = null;
            string strResultEq = strEq;
            int nMidOpIdx = FindMidOpIdx(strResultEq);
            bool bIsEquation = DefOperator.IsEquation(strEq);

            if (strResultEq == string.Empty)
                return null;

            if (bIsEquation == true)
                nMidOpIdx = CheckBracketIntegrity(ref strResultEq);

            if (bIsEquation == false)
            {
                entMain = new OpEntityVar();
                entMain.EQUATION = strResultEq;

                MakeVarEntity((OpEntityVar)entMain);
            }
            else
            {
                entMain = new OpEntityEq();
                entMain.EQUATION = strResultEq;

                MakeEqEntity(nMidOpIdx, (OpEntityEq)entMain);
            }

            return entMain;
        }

        static void MakeEqEntity(int nMidOpIdx, OpEntityEq entEq)
        {
            string strResultEq = entEq.EQUATION;
            string strFormerEq = strResultEq.Substring(0, nMidOpIdx);
            string strLatterEq = strResultEq.Substring(nMidOpIdx + 1, strResultEq.Length - nMidOpIdx - 1);
            char cOpMid = strResultEq.ElementAt(nMidOpIdx);

            OpEntityBase entLeft = AnalyzeMathSyntax(strFormerEq);
            OpEntityBase entRight = AnalyzeMathSyntax(strLatterEq);

            entLeft.PARENT = entEq;
            entRight.PARENT = entEq;
            ((OpEntityEq)entEq).OPERATOR = DefOperator.GetCalOperatorIdx(cOpMid);
            ((OpEntityEq)entEq).ENTITY_LEFT = entLeft;
            ((OpEntityEq)entEq).ENTITY_RIGHT = entRight;
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

        static int FindMidOpIdx(string strEq)
        {
            int nCurIdx = 0;
            int nBracketCnt = 0;

            while (true)
            {
                if (nCurIdx < 0 || nCurIdx >= strEq.Length)
                    return -1;

                char cCurVal = strEq.ElementAt(nCurIdx);

                if (CompareOperatorValidity(cCurVal, DefOperator.SQUARE) == true && nBracketCnt == 0)
                    return nCurIdx;

                if (cCurVal == DefOperator.BRACKET_B)
                    nBracketCnt++;
                else if (cCurVal == DefOperator.BRACKET_E)
                    nBracketCnt--;

                nCurIdx++;
            }
        }

        static int CheckBracketIntegrity(ref string strResultEq)
        {
            int nMidOpIdx = 0;

            while (true)
            {
                nMidOpIdx = FindMidOpIdx(strResultEq);

                if (nMidOpIdx >= 0)
                    break;
                else
                    strResultEq = RemoveOuterBracket(strResultEq);
            }

            return nMidOpIdx;
        }

        static string EliminateOuterBracket(string strEq)
        {
            string strEqResult = strEq;

            if (strEq == string.Empty)
                return strEq;

            while (IsBracketRemovable(strEqResult) == true)
                strEqResult = RemoveOuterBracket(strEqResult);

            return strEqResult;
        }

        static string RemoveOuterBracket(string strEq)
        {
            string strEqResult = strEq;

            if (strEq == string.Empty)
                return strEq;

            if (IsBracketRemovable(strEqResult) == true)
            {
                strEqResult = strEqResult.Remove(strEqResult.Length - 1);
                strEqResult = strEqResult.Remove(0, 1);
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
