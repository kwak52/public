using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public class OpEntityComp : OpEntityBase
    {
        public OpEntityBase ENTITY_LEFT
        {
            set; get;
        }

        public OpEntityBase ENTITY_RIGHT
        {
            set; get;
        }

        public DefOperator.OPERATOR_COMP OPERATOR
        {
            set; get;
        }

        public OpEntityComp()
        {
            TYPE = ENTITY_TYPE.COMPARISON;
        }

        override public string GetResult()
        {
            return OpEntityComparer.GetResult(ENTITY_LEFT, ENTITY_RIGHT, OPERATOR);
        }

        override public void UpdateEquation()
        {
            string strLeft = string.Empty;
            string strRight = string.Empty;
            string strSymbol = DefOperator.GetCompOperatorSymbol(OPERATOR);

            ENTITY_LEFT.UpdateEquation();
            ENTITY_RIGHT.UpdateEquation();

            strRight = ENTITY_RIGHT.EQUATION;
            strLeft = ENTITY_LEFT.EQUATION;

            EQUATION = DefOperator.BRACKET_B + strLeft + strRight + strRight + DefOperator.BRACKET_E;
        }

        override public void GetVariables(Dictionary<string, List<OpEntityVar>> vDicVariables)
        {
            ENTITY_LEFT.GetVariables(vDicVariables);
            ENTITY_RIGHT.GetVariables(vDicVariables);
        }
    }
}
