using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public class OpEntityVar : OpEntityBase
    {
        public OpEntityVar()
        {
            TYPE = ENTITY_TYPE.VARIABLE;
        }

        public string VARIABLE
        {
            set; get;
        }

        public string VALUE
        {
            set; get;
        }

        public override string GetResult()
        {
            return VALUE;
        }

        override public void UpdateEquation()
        {
            if (VARIABLE == DefOperator.CONSTANT)
                EQUATION = VALUE;
            else
                EQUATION = VARIABLE;
        }

        override public void GetVariables(Dictionary<string, List<OpEntityVar>> vDicVariables)
        {
            if (VARIABLE != DefOperator.CONSTANT)
            {
                if (!vDicVariables.ContainsKey(VARIABLE))
                    vDicVariables.Add(VARIABLE, new List<OpEntityVar>());

                vDicVariables[VARIABLE].Add(this);
            }
        }
    }
}
