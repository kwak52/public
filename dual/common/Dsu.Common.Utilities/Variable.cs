using System;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities
{
    public class Variable : IVariable
    {
        public string Name { get; set; }
        public string UniqueName { get { return Name; } set { Name = value; } }
        public string Note { get; set; }
        public object Value { get; set; }
        public DataValueType DataType
        {
            get { return Value == null ? DataValueType.Unknown : Value.GetType().GetDataValueType(); }
            set
            {
                if (Value == null || Value.GetType().GetDataValueType() != value)
                    Value = value.CreateObject();
            }
        }


        /// <summary> Symbol 에 대한 write request 전송.  PLC symbol 이 아니면 직접 value 에 write </summary>
        public bool PostWriteRequest(object value, IReason reason) { Value = value; return true; }

        [Obsolete("Use PostWriteRequest() instead.")]
        public object WriteRequestValue { set { PostWriteRequest(value, null); } }

        public bool Readable { get { return true; } }
        public bool Writable { get { return true; } }

        public object UserTag { get; set; }
    }
}
