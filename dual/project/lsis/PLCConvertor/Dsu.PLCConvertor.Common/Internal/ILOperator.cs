using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// IL 변환을 위한 class
    /// </summary>
    public class IL
    {
        /// <summary>
        /// LSIS Xg5000 을 위한 { mnemonic -> 문자 mnemonic } 참조용 dictionary
        /// </summary>
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
            [Mnemonic.END] = "END",
        };


        /// <summary>
        /// 옴론 CX-One 을 위한 { mnemonic -> 문자 mnemonic } 참조용 dictionary
        /// </summary>
        static Dictionary<Mnemonic, string> _dicOmron = new Dictionary<Mnemonic, string>()
        {
            [Mnemonic.LOAD] = "LD",
            [Mnemonic.AND] = "AND",
            [Mnemonic.ANDLD] = "ANDLD",
            [Mnemonic.OR] = "OR",
            [Mnemonic.ORLD] = "ORLD",
            [Mnemonic.OUT] = "OUT",

            // 옴론은 MPush/MLoad/MPop 을 이용하지 않음
            [Mnemonic.MPUSH] = "--ERROR:MPUSH",
            [Mnemonic.MLOAD] = "--ERROR:MLOAD",
            [Mnemonic.MPOP] = "--ERROR:MPOP",
            [Mnemonic.END] = "END(001)",
        };

        /// <summary>
        /// _dicLSIS 의 반대방향 참조용 dictionary.  mnemonic 문자열로 mnemonic 을 검색
        /// </summary>
        static Dictionary<string, Mnemonic> _reverseDicLSIS;

        /// <summary>
        /// _dicOmron 의 반대방향 참조용 dictionary.  mnemonic 문자열로 mnemonic 을 검색
        /// </summary>
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

        /// <summary>
        /// targetType 에 맞는 mnemoinc 의 문자열을 반환
        /// </summary>
        public static string GetOperator(PLCVendor targetType, Mnemonic op) => GetDictionary(targetType)[op];
        /// <summary>
        /// targetType 에 맞는 문자열의 mnemoinc 값을 반환
        /// </summary>
        public static Mnemonic GetMnemonic(PLCVendor targetType, string op) => GetReversedDictionary(targetType)[op];
    }

    /// <summary>
    /// IL mnemonic 열거
    /// </summary>
    public enum Mnemonic
    {
        LOAD,
        AND, ANDLD,
        OR, ORLD,
        OUT,
        MPUSH,
        MLOAD,
        MPOP,
        END,
    }

}
