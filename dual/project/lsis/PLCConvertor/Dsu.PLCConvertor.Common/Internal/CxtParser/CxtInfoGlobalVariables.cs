using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class CxtInfoGlobalVariables : CxtInfo
    {
        public CxtInfoVariableList VariableList { get; internal set; }
        internal override void ClearMyResult() { }
        public override IEnumerable<CxtInfo> Children { get { yield return VariableList; } }
        public CxtInfoGlobalVariables()
            : base("GlobalVariables")
        {

        }
    }


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
                let tokens = line.Trim().Split(new[] { ',' }).ToArray()
                where tokens.Length >= 5
                //let name = tokens[0]
                let device = tokens[1]
                let type = getType(tokens[2])
                let comment = tokens.Last()
                where type.HasValue
                select new PLCVariable(device, type.Value, comment, null)
            ).ToArray()
            ;

            PLCVariable.DeviceType? getType(string type)
            {
                PLCVariable.DeviceType enumResult;
                if (Enum.TryParse<PLCVariable.DeviceType>(type, true, out enumResult))
                    return enumResult;

                Global.Logger.Warn($"Unknown PLC device type {type}.");
                return null;
            }
        }
    }
}
