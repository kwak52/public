using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpMathCalLib.OpCtrl
{
    public class OpEntityBase
    {
        public OpEntityBase()
        {
            TYPE = ENTITY_TYPE.BASE;
        }

        public ENTITY_TYPE TYPE
        {
            set; get;
        }

        public string EQUATION
        {
            set; get;
        }

        public OpEntityBase PARENT
        {
            set; get;
        }

        virtual public string GetResult()
        {
            return string.Empty;
        }

        virtual public string GetEquation()
        {
            return EQUATION;
        }

        virtual public void UpdateEquation()
        {

        }

        virtual public void GetVariables(Dictionary<string, List<OpEntityVar>> vDicVariables)
        {

        }
    }
}
