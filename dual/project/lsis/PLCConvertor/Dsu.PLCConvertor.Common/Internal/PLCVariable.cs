using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class PLCVariable
    {
        /// <summary>
        /// PLC device 의 data type enumeration
        /// </summary>
        public enum DeviceType
        {
            Bool = 1,
            Bit = Bool,
            Word,
        }

        /// <summary>
        /// PLC device 의 data type
        /// </summary>
        public DeviceType Type { get; set; }
        public string Device { get; private set; }
        public string Comment { get; private set; }
        public string Variable { get; private set; }
        public PLCVariable(string device, DeviceType type, string comment, string variable)
        {
            Device = device;
            Type = type;
            Comment = comment;
            Variable = variable;
        }

        public PLCVariable(string device, PLCVariable src)
            : this(device, src.Type, src.Comment, src.Variable)
        { }
    }
}
