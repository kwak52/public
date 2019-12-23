using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// CXT 에서 global 변수 선언부
    /// </summary>
    public class CxtInfoGlobalVariables : CxtInfoVariables
    {
        public CxtInfoGlobalVariables()
            : base("GlobalVariables")
        {

        }
    }


    /// <summary>
    /// CXT 에서 변수 선언들의 실제 list 표현
    /// </summary>
    public class CxtInfoVariableList : CxtInfo
    {
        public PLCVariable[] Variables { get; private set; }
        internal override void ClearMyResult() { }
        public override IEnumerable<CxtInfo> Children { get { return Enumerable.Empty<CxtInfo>(); } }
        public CxtInfoVariableList(string[] varDefLines)
            : base("VariableList")
        {
            Variables = (
                from line in varDefLines
                let tokens = line.Trim().TrimEnd(';').Split(new[] { ',' }).ToArray()
                where tokens.Length >= 5
                let name = tokens[0]
                let device = tokens[1]
                let type = getType(tokens[2])
                let comment = tokens.Last()
                where type.HasValue
                select new PLCVariable(name, device, type.Value, comment, null)
            ).ToArray()
            ;

            PLCVariable.DeviceType? getType(string type)
            {
                PLCVariable.DeviceType enumResult;
                if (type == "FUNCTION BLOCK")
                    return PLCVariable.DeviceType.Function_Block;

                if (Enum.TryParse<PLCVariable.DeviceType>(type, true, out enumResult))
                    return enumResult;

                Global.Logger.Warn($"Unknown PLC device type {type}.");
                return null;
            }
        }
    }
}
