using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class IL
    {
        static Dictionary<Mnemonic, string> _dicLSIS = new Dictionary<Mnemonic, string>()
        {
            [Mnemonic.LOAD] = "LOAD",
            [Mnemonic.AND] = "AND",
            [Mnemonic.ANDLD] = "AND LOAD",
            [Mnemonic.OR] = "OR",
            [Mnemonic.ORLD] = "OR LOAD",
            [Mnemonic.OUT] = "OUT",

            [Mnemonic.MPUSH] = "MPUSH",
            [Mnemonic.MLOAD] = "MLOAD",
            [Mnemonic.MPOP] = "MPOP",
        };


        static Dictionary<Mnemonic, string> _dicOmron = new Dictionary<Mnemonic, string>()
        {
            [Mnemonic.LOAD] = "LD",
            [Mnemonic.AND] = "AND",
            [Mnemonic.ANDLD] = "ANDLD",
            [Mnemonic.OR] = "OR",
            [Mnemonic.ORLD] = "ORLD",
            [Mnemonic.OUT] = "OUT",

            [Mnemonic.MPUSH] = "--ERROR:MPUSH",
            [Mnemonic.MLOAD] = "--ERROR:MLOAD",
            [Mnemonic.MPOP] = "--ERROR:MPOP",
        };

        static Dictionary<string, Mnemonic> _reverseDicLSIS;
        static Dictionary<string, Mnemonic> _reverseDicOmron;

        static IL()
        {
            _reverseDicLSIS = _dicLSIS.ToDictionary(tpl => tpl.Value, tpl => tpl.Key);
            _reverseDicOmron = _dicOmron.ToDictionary(tpl => tpl.Value, tpl => tpl.Key);
        }

        static Dictionary<Mnemonic, string> GetDictionary(PLCVendor targetType)
        {
            switch (targetType)
            {
                case PLCVendor.LSIS: return _dicLSIS;
                case PLCVendor.Omron: return _dicOmron;
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{targetType}");
            };
        }
        static Dictionary<string, Mnemonic> GetReversedDictionary(PLCVendor targetType)
        {
            switch (targetType)
            {
                case PLCVendor.LSIS: return _reverseDicLSIS;
                case PLCVendor.Omron: return _reverseDicOmron;
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{targetType}");
            };
        }

        public static string GetOperator(PLCVendor targetType, Mnemonic op) => GetDictionary(targetType)[op];
        public static Mnemonic GetMnemonic(PLCVendor targetType, string op) => GetReversedDictionary(targetType)[op];
    }

    public enum Mnemonic
    {
        LOAD,
        AND, ANDLD,
        OR, ORLD,
        OUT,
        MPUSH,
        MLOAD,
        MPOP,
    }

}
