using System.Collections.Generic;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// CXT 에서 변수 선언부 (global / local 의 공통 parent class)
    /// </summary>
    public class CxtInfoVariables : CxtInfo
    {
        public CxtInfoVariableList VariableList { get; internal set; }
        internal override void ClearMyResult() { }
        public override IEnumerable<CxtInfo> Children { get { yield return VariableList; } }
        protected CxtInfoVariables(string key)
            : base(key)
        {
        }
    }

    /// <summary>
    /// CXT 에서 local 변수 선언부
    /// </summary>
    public class CxtInfoLocalVariables : CxtInfoVariables
    {
        public CxtInfoLocalVariables()
            : base("LocalVariables")
        {

        }
    }
}
