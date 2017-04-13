using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public class OpEntityEq : OpEntityBase
    {
        public OpEntityBase ENTITY_LEFT
        {
            set; get;
        }

        public OpEntityBase ENTITY_RIGHT
        {
            set; get;
        }

        public DefOperator.OPERATOR_CAL OPERATOR
        {
            set; get;
        }

        public OpEntityEq()
        {
            TYPE = ENTITY_TYPE.EQUATION;
        }

        override public string GetResult()
        {
            return OpEntityComputer.GetResult(ENTITY_LEFT, ENTITY_RIGHT, OPERATOR);
        }

        override public void UpdateEquation()
        {
            string strLeft = string.Empty;
            string strRight = string.Empty;
            char cSymbol = DefOperator.GetCalOperatorSymbol(OPERATOR);

            ENTITY_LEFT.UpdateEquation();
            ENTITY_RIGHT.UpdateEquation();

            strRight = ENTITY_RIGHT.EQUATION;
            strLeft = ENTITY_LEFT.EQUATION;

            EQUATION = DefOperator.BRACKET_B + strLeft + cSymbol + strRight + DefOperator.BRACKET_E;
        }

        override public void GetVariables(Dictionary<string, List<OpEntityVar>> vDicVariables)
        {
            ENTITY_LEFT.GetVariables(vDicVariables);
            ENTITY_RIGHT.GetVariables(vDicVariables);
        }
    }
}
