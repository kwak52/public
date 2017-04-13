using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public class OpEntityComparer
    {
        public static string GetResult(OpEntityBase enLeft, OpEntityBase enRight, DefOperator.OPERATOR_COMP eOp)
        {
            return CompareData(enLeft.GetResult(), enRight.GetResult(), eOp);
        }

        static string CompareData(string strValLeft, string strValRight, DefOperator.OPERATOR_COMP eOp)
        {
            try
            {
                switch (eOp)
                {
                    case DefOperator.OPERATOR_COMP.BIGGER:
                        return (Convert.ToDouble(strValLeft) > Convert.ToDouble(strValRight)) ? true.ToString() : false.ToString();
                    case DefOperator.OPERATOR_COMP.SMALLER:
                        return (Convert.ToDouble(strValLeft) < Convert.ToDouble(strValRight)) ? true.ToString() : false.ToString();
                    case DefOperator.OPERATOR_COMP.BIGGEREQUAL:
                        return (Convert.ToDouble(strValLeft) >= Convert.ToDouble(strValRight)) ? true.ToString() : false.ToString();
                    case DefOperator.OPERATOR_COMP.SMALLEREQUAL:
                        return (Convert.ToDouble(strValLeft) <= Convert.ToDouble(strValRight)) ? true.ToString() : false.ToString();
                    case DefOperator.OPERATOR_COMP.DIFFERENT:
                        return strValLeft != strValRight ? true.ToString() : false.ToString();
                    case DefOperator.OPERATOR_COMP.EQUAL:
                        return strValLeft == strValRight ? true.ToString() : false.ToString();
                    case DefOperator.OPERATOR_COMP.AND:
                        return (Convert.ToBoolean(strValLeft) == true && Convert.ToBoolean(strValRight) == true) ? true.ToString() : false.ToString();
                    case DefOperator.OPERATOR_COMP.OR:
                        return (Convert.ToBoolean(strValLeft) == true || Convert.ToBoolean(strValRight) == true) ? true.ToString() : false.ToString();
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
