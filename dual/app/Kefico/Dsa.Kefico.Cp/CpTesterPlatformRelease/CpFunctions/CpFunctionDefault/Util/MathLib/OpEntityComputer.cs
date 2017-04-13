using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public static class OpEntityComputer
    {
        public static string GetResult(OpEntityBase enLeft, OpEntityBase enRight, DefOperator.OPERATOR_CAL eOp)
        {
            return ComputeData(Convert.ToDouble(enLeft.GetResult()), Convert.ToDouble(enRight.GetResult()), eOp);
        }

        static string ComputeData(double dValLeft, double dValRight, DefOperator.OPERATOR_CAL eOp)
        {
            try
            {
                switch (eOp)
                {
                    case DefOperator.OPERATOR_CAL.PLUS:
                        return (dValLeft + dValRight).ToString();
                    case DefOperator.OPERATOR_CAL.MINUS:
                        return (dValLeft - dValRight).ToString();
                    case DefOperator.OPERATOR_CAL.MULTIPLIER:
                        return (dValLeft * dValRight).ToString();
                    case DefOperator.OPERATOR_CAL.DIVIDER:
                        return (dValLeft / dValRight).ToString();
                    case DefOperator.OPERATOR_CAL.SQUARE:
                        return Math.Pow(dValLeft, dValRight).ToString();
                    default:
                        return string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("WEqCalLib.OpCtrl.OpEntityComputer.ComputeData Error: " + ex.Message);

                return string.Empty;
            }
        }

    }
}
