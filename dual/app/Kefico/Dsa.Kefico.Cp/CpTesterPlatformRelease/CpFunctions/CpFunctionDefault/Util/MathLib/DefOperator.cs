using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public static class DefOperator
    {
        public const char PLUS = '+';
        public const char MINUS = '-';
        public const char MULTIPLIER = '*';
        public const char DIVIDER = '/';
        public const char SQUARE = '^';
        public const char BRACKET_B = '(';
        public const char BRACKET_E = ')';
        public const char SPACE = ' ';

        public const string COMP_BIGGER = ">";
        public const string COMP_BIGGEREQUAL = ">=";
        public const string COMP_SMALLER = "<";
        public const string COMP_SMALLEREQUAL = "<=";
        public const string COMP_EQUAL = "==";
        public const string COMP_DIFFERENT = "!=";
        public const string COMP_AND = "&&";
        public const string COMP_OR = "||";


        public const string CONSTANT = "Const.";
        public const string SUB_EQ = "Sub_Eq.";

        public const int ASCII_NUM_BEGIN = 48;
        public const int ASCII_NUM_END = 57;
        public const int ASCII_NUM_DOT = 46;

        public const string MATH_CONST_SYMBOL = "Exp.";
        public const double MATH_CONST = 2.71828182845904523536028747135266249;

        public enum OPERATOR_COMP
        {
            BIGGER = 0,
            BIGGEREQUAL,
            SMALLER,
            SMALLEREQUAL,
            EQUAL,
            DIFFERENT,
            AND,
            OR,
            ERROR
        }

        public enum OPERATOR_CAL
        {
            VAR = 0,
            PLUS,
            MINUS,
            MULTIPLIER,
            DIVIDER,
            SQUARE,
            BRACKET_B,
            BRACKET_E
        }

        public static OPERATOR_CAL GetCalOperatorIdx(char cTargetOp)
        {
            switch (cTargetOp)
            {
                case DefOperator.DIVIDER: return DefOperator.OPERATOR_CAL.DIVIDER;
                case DefOperator.MULTIPLIER: return DefOperator.OPERATOR_CAL.MULTIPLIER;
                case DefOperator.PLUS: return DefOperator.OPERATOR_CAL.PLUS;
                case DefOperator.MINUS: return DefOperator.OPERATOR_CAL.MINUS;
                case DefOperator.SQUARE: return DefOperator.OPERATOR_CAL.SQUARE;
                case DefOperator.BRACKET_B: return DefOperator.OPERATOR_CAL.BRACKET_B;
                case DefOperator.BRACKET_E: return DefOperator.OPERATOR_CAL.BRACKET_E;
                default: return DefOperator.OPERATOR_CAL.VAR;
            }
        }

        public static char GetCalOperatorSymbol(OPERATOR_CAL eOp)
        {
            switch (eOp)
            {
                case DefOperator.OPERATOR_CAL.DIVIDER: return DefOperator.DIVIDER;
                case DefOperator.OPERATOR_CAL.MULTIPLIER: return DefOperator.MULTIPLIER;
                case DefOperator.OPERATOR_CAL.PLUS: return DefOperator.PLUS;
                case DefOperator.OPERATOR_CAL.MINUS: return DefOperator.MINUS;
                case DefOperator.OPERATOR_CAL.SQUARE: return DefOperator.SQUARE;
                case DefOperator.OPERATOR_CAL.BRACKET_B: return DefOperator.BRACKET_B;
                case DefOperator.OPERATOR_CAL.BRACKET_E: return DefOperator.BRACKET_E;
                default: return DefOperator.SPACE;
            }
        }

        public static OPERATOR_COMP GetCompOperatorIdx(string strTargetOp)
        {
            switch (strTargetOp)
            {
                case DefOperator.COMP_BIGGER: return DefOperator.OPERATOR_COMP.BIGGER;
                case DefOperator.COMP_BIGGEREQUAL: return DefOperator.OPERATOR_COMP.BIGGEREQUAL;
                case DefOperator.COMP_SMALLER: return DefOperator.OPERATOR_COMP.SMALLER;
                case DefOperator.COMP_SMALLEREQUAL: return DefOperator.OPERATOR_COMP.SMALLEREQUAL;
                case DefOperator.COMP_EQUAL: return DefOperator.OPERATOR_COMP.EQUAL;
                case DefOperator.COMP_DIFFERENT: return DefOperator.OPERATOR_COMP.DIFFERENT;
                case DefOperator.COMP_AND: return DefOperator.OPERATOR_COMP.AND;
                case DefOperator.COMP_OR: return DefOperator.OPERATOR_COMP.OR;
                default: Debug.Assert(true); return OPERATOR_COMP.ERROR;
            }
        }

        public static string GetCompOperatorSymbol(OPERATOR_COMP eOp)
        {
            switch (eOp)
            {
                case DefOperator.OPERATOR_COMP.BIGGER: return DefOperator.COMP_BIGGER;
                case DefOperator.OPERATOR_COMP.BIGGEREQUAL: return DefOperator.COMP_BIGGEREQUAL;
                case DefOperator.OPERATOR_COMP.SMALLER: return DefOperator.COMP_SMALLER;
                case DefOperator.OPERATOR_COMP.SMALLEREQUAL: return DefOperator.COMP_SMALLEREQUAL;
                case DefOperator.OPERATOR_COMP.EQUAL: return DefOperator.COMP_EQUAL;
                case DefOperator.OPERATOR_COMP.DIFFERENT: return DefOperator.COMP_DIFFERENT;
                case DefOperator.OPERATOR_COMP.AND: return DefOperator.COMP_AND;
                case DefOperator.OPERATOR_COMP.OR: return DefOperator.COMP_OR;
                case DefOperator.OPERATOR_COMP.ERROR: return string.Empty;
                default: return string.Empty;
            }
        }

        public static bool IsCondition(string strEq)
        {
            return GetCompOpCount(strEq) > 0 ? true : false;
        }

        public static bool IsEquation(string strEq)
        {
            return GetCalOpCount(strEq) > 0 ? true : false;
        }

        public static bool IsMinusValue(string strEq)
        {
            if (strEq.ElementAt(0) == DefOperator.MINUS)
                return true;

            return false;
        }

        public static bool IsConstant(string strEq)
        {
            bool bConst = true;
            int nCurIdx = 0;

            if (IsMinusValue(strEq) == true)
                strEq = strEq.Remove(0, 1);

            while (true)
            {
                if (nCurIdx < 0 || nCurIdx >= strEq.Length)
                    break;

                char cVal = strEq.ElementAt(nCurIdx);
                int nVal = Convert.ToInt32(cVal);

                nCurIdx++;

                if (nVal >= DefOperator.ASCII_NUM_BEGIN && nVal <= DefOperator.ASCII_NUM_END || nVal == DefOperator.ASCII_NUM_DOT)
                    continue;
                else
                {
                    bConst = false;

                    break;
                }
            }

            return bConst;
        }

        public static int GetCalOpCount(string strEq)
        {
            int nCurIdx = 0;
            int nOpCnt = 0;
            int nValueDetected = 0;

            while (true)
            {
                if (nCurIdx < 0 || nCurIdx >= strEq.Length)
                    break;

                char cCurVal = strEq.ElementAt(nCurIdx);

                if (DefOperator.GetCalOperatorIdx(cCurVal) != DefOperator.OPERATOR_CAL.VAR
                    && DefOperator.GetCalOperatorIdx(cCurVal) != DefOperator.OPERATOR_CAL.BRACKET_B
                    && DefOperator.GetCalOperatorIdx(cCurVal) != DefOperator.OPERATOR_CAL.BRACKET_E
                    && nValueDetected > 0)
                    nOpCnt++;
                else if (DefOperator.GetCalOperatorIdx(cCurVal) == DefOperator.OPERATOR_CAL.VAR)
                    nValueDetected++;


                nCurIdx++;
            }

            if (nOpCnt == 1)
                if (IsMinusValue(strEq) == true)
                    return 0;

            return nOpCnt;
        }

        public static int GetCompOpCount(string strEq)
        {
            int nCurIdx = 0;
            int nOpCnt = 0;
            char cFormer = ' ';

            while (true)
            {
                if (nCurIdx < 0 || nCurIdx >= strEq.Length - 1)
                    break;

                char cCurVal = strEq.ElementAt(nCurIdx);
                string strOpCheck = cFormer.ToString() + cCurVal.ToString();

                if (GetCompOperatorIdx(strOpCheck) != OPERATOR_COMP.ERROR)
                    nOpCnt++;
                else if (GetCompOperatorIdx(cCurVal.ToString()) != OPERATOR_COMP.ERROR)
                    nOpCnt++;

                cFormer = cCurVal;
                nCurIdx++;
            }

            return nOpCnt;
        }

        public static int GetCharCount(string strEq, char cVal)
        {
            return strEq.Count(x => x == cVal);
        }

        public static bool CheckExpressionIntegrity(string strEq)
        {
            int nBracketOpenCount = DefOperator.GetCharCount(strEq, DefOperator.BRACKET_B);
            int nBracketCloseCount = DefOperator.GetCharCount(strEq, DefOperator.BRACKET_E);

            if (nBracketCloseCount != nBracketOpenCount)
                return false;
            return true;
        }
    }

    public enum ENTITY_TYPE
    {
        BASE,
        COMPARISON,
        EQUATION,
        VARIABLE
    }
}
