using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CpMathCalLib.OpCtrl;

namespace CpMathCalLib
{
    public class CpMathBase
    {
        public string ErrorMessge { set; get; }
        protected OpEntityBase ResultEntity { set; get; } = null;
        protected Dictionary<string, List<OpEntityVar>> DicVariables { set; get; } = new Dictionary<string, List<OpEntityVar>>();

        protected void UpdateVariableList()
        {
            DicVariables = new Dictionary<string, List<OpEntityVar>>();

            if (ResultEntity == null)
                return;

            ResultEntity.GetVariables(DicVariables);
        }

        public List<string> GetVariableList()
        {
            if (ResultEntity == null)
                return null;

            UpdateVariableList();

            return DicVariables.Keys.ToList();
        }

        public void SetVariableValue(string strVariable, string strValue)
        {
            if (!DicVariables.ContainsKey(strVariable))
                return;

            List<OpEntityVar> vVarEnts = DicVariables[strVariable];

            foreach (OpEntityVar entvar in vVarEnts)
                entvar.VALUE = strValue;
        }

        public string GetResult()
        {
            return ResultEntity?.GetResult();
        }
    }

    public class CpMathEqCal : CpMathBase
    {
        string MathEquationExpression { set; get; } = string.Empty;

        public CpMathEqCal(string strEquation)
        {

            if (!DefOperator.CheckExpressionIntegrity(strEquation))
            {
                ErrorMessge = DefErrorMessage.ERROR_001;

                return;
            }

            MathEquationExpression = strEquation;
            ResultEntity = OpCalTranslator.Translate(MathEquationExpression);

            UpdateVariableList();
        }
    }

    public class CpMathCondComp : CpMathBase
    {
        public string ComparisionExpression { set; get; } = string.Empty;

        public CpMathCondComp(string strEquation)
        {
            if (strEquation == string.Empty)
                return;

            if (!DefOperator.CheckExpressionIntegrity(strEquation))
            {
                ErrorMessge = DefErrorMessage.ERROR_001;

                return;
            }

            ComparisionExpression = strEquation;
            ResultEntity = OpCompTranslator.Translate(ComparisionExpression);

            UpdateVariableList();
        }
    }
}
