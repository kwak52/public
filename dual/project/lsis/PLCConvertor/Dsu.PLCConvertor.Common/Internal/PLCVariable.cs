using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// PLC 에 사용된 변수 정보.  Device type, comment, variable 이름 등이 담겨 있다.
    /// </summary>
    public class PLCVariable
    {
        /// <summary>
        /// PLC device 의 data type enumeration
        /// </summary>
        public enum DeviceType
        {
            Bool = 1,
            Bit = Bool,
            Int,
            UInt,
            UINT_BCD,
            DInt,
            UDInt,
            Word,
            Channel,
            Function_Block,
        }
        public string Name { get; internal set; }

        /// <summary>
        /// PLC device 의 data type
        /// </summary>
        public DeviceType Type { get; set; }
        /// <summary>
        /// PLC device (즉 PLC 주소)
        /// </summary>
        public string Device { get; private set; }
        /// <summary>
        /// 사용된 device 의 comment
        /// </summary>
        public string Comment { get; internal set; }
        /// <summary>
        /// Device 의 variable (alias?)
        /// </summary>
        public string Variable { get; internal set; }
        public PLCVariable(string name, string device, DeviceType type, string comment, string variable)
        {
            Name = name;
            Device = device;
            Type = type;
            Comment = comment;
            Variable = variable;
        }

        public PLCVariable(string device, PLCVariable src)
            : this(src.Name, device, src.Type, src.Comment, src.Variable)
        { }
    }
}
