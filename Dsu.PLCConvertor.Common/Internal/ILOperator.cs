using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class ILCommand
    {
        public string Command { get; protected set; }
        public int Arity { get; protected set; }
        public ILCommand(string command, int arity=0)
        {
            Command = command;
            Arity = arity;
        }
    }

    /// <summary>
    /// coil 에 해당하는 명령어.  OUT, TMR, ...
    /// </summary>
    public class ILTerminalCommand : ILCommand
    {
        public ILTerminalCommand(string command, int arity = 0)
            : base(command, arity)
        {
        }
    }


    /// <summary>
    /// IL 변환을 위한 class
    /// </summary>
    public class IL
    {
        static List<ILCommand> _toList(int arity, params string[] operators)
            => operators.Select(o => new ILCommand(o, arity)).ToList();


        /// <summary>
        /// LSIS Xg5000 을 위한 { mnemonic -> 문자 mnemonic } 참조용 dictionary
        /// </summary>
        static Dictionary<Mnemonic, List<ILCommand>> _dicLSIS = new Dictionary<Mnemonic, List<ILCommand>>()
        {
            [Mnemonic.LOAD] = _toList(1, "LOAD"),
            [Mnemonic.LOAD] = _toList(1, "LOAD"),
            [Mnemonic.LOADNOT] = _toList(1, "LOAD NOT"),
            [Mnemonic.AND] = _toList(1, "AND"),
            [Mnemonic.ANDNOT] = _toList(1, "AND NOT"),
            [Mnemonic.ANDLD] = _toList(1, "AND LOAD"),
            [Mnemonic.OR] = _toList(1, "OR"),
            [Mnemonic.ORNOT] = _toList(1, "OR NOT"),
            [Mnemonic.ORLD] = _toList(1, "OR LOAD"),
            [Mnemonic.OUT] = _toList(1, "OUT"),

            [Mnemonic.MPUSH] = _toList(0, "MPUSH"),
            [Mnemonic.MLOAD] = _toList(0, "MLOAD"),
            [Mnemonic.MPOP] = _toList(0, "MPOP"),

            [Mnemonic.CTU] = _toList(2, "CTU"),
            [Mnemonic.TON] = _toList(1, "TON"),
            [Mnemonic.TMR] = _toList(2, "TMR"),
            [Mnemonic.KEEP] = _toList(2, "XXXKEEP(011)"),
            [Mnemonic.SFT] = _toList(3, "SFT"),
            [Mnemonic.END] = _toList(1, "END"),
        };


        /// <summary>
        /// 옴론 CX-One 을 위한 { mnemonic -> 문자 mnemonic } 참조용 dictionary
        /// </summary>
        static Dictionary<Mnemonic, List<ILCommand>> _dicOmron = new Dictionary<Mnemonic, List<ILCommand>>()
        {
            [Mnemonic.LOAD] = _toList(1, "LD"),
            [Mnemonic.LOADNOT] = _toList(1, "LDNOT"),
            [Mnemonic.AND] = _toList(1, "AND"),
            [Mnemonic.ANDNOT] = _toList(1, "ANDNOT"),
            [Mnemonic.ANDLD] = _toList(1, "ANDLD"),
            [Mnemonic.OR] = _toList(1, "OR"),
            [Mnemonic.ORNOT] = _toList(1, "ORNOT"),
            [Mnemonic.ORLD] = _toList(1, "ORLD"),
            [Mnemonic.OUT] = _toList(1, "OUT"),

            // 옴론은 MPush/MLoad/MPop 을 이용하지 않음
            [Mnemonic.MPUSH] = _toList(1, "--ERROR:MPUSH"),
            [Mnemonic.MLOAD] = _toList(1, "--ERROR:MLOAD"),
            [Mnemonic.MPOP] = _toList(1, "--ERROR:MPOP"),

            [Mnemonic.CTU] = _toList(2, "CNT"),
            [Mnemonic.TON] = _toList(1, "TIM"),
            [Mnemonic.TMR] = _toList(2, "TTIM(087)"),
            [Mnemonic.KEEP] = _toList(2, "KEEP(011)"),
            [Mnemonic.SFT] = _toList(3, "SFT(010)"),
            [Mnemonic.END] = _toList(1, "END(001)"),
        };

        /// <summary>
        /// _dicLSIS 의 반대방향 참조용 dictionary.  mnemonic 문자열로 mnemonic 을 검색
        /// </summary>
        static Multimap<string, Mnemonic> _reverseDicLSIS;

        /// <summary>
        /// _dicOmron 의 반대방향 참조용 dictionary.  mnemonic 문자열로 mnemonic 을 검색
        /// </summary>
        static Multimap<string, Mnemonic> _reverseDicOmron;

        static IL()
        {
            _reverseDicLSIS = _toReversedMultimap(_dicLSIS);
            _reverseDicOmron = _toReversedMultimap(_dicOmron);


            Multimap<string, Mnemonic> _toReversedMultimap(Dictionary<Mnemonic, List<ILCommand>> dict)
            {
                var reverseDic = new Multimap<string, Mnemonic>();

                dict.Iter(kv =>
                {
                    var mnemonic = kv.Key;
                    var operators = kv.Value;   // list of string values
                    operators.Iter(o => reverseDic.Add(o.Command, mnemonic));
                });

                return reverseDic;
            }
        }

        static Dictionary<Mnemonic, List<ILCommand>> GetDictionary(PLCVendor targetType)
        {
            switch (targetType)
            {
                case PLCVendor.LSIS: return _dicLSIS;
                case PLCVendor.Omron: return _dicOmron;
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{targetType}");
            };
        }
        static Multimap<string, Mnemonic> GetReversedDictionary(PLCVendor targetType)
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
        public static string GetOperator(PLCVendor targetType, Mnemonic op) => GetILCommand(targetType, op)?.Command;
        public static ILCommand GetILCommand(PLCVendor targetType, Mnemonic op) => GetDictionary(targetType)[op].First();
        /// <summary>
        /// targetType 에 맞는 문자열의 mnemoinc 값을 반환
        /// </summary>
        public static Mnemonic GetMnemonic(PLCVendor targetType, string op) => GetReversedDictionary(targetType)[op].FirstOrDefault();
    }

    /// <summary>
    /// IL mnemonic 열거
    /// </summary>
    public enum Mnemonic
    {
        LOAD, LOADNOT,      
        AND, ANDNOT, ANDLD,
        OR, ORNOT, ORLD,
        OUT,
        MPUSH,
        MLOAD,
        MPOP,
        /// <summary>
        /// Counter Up
        /// </summary>
        CTU,
        TON, TMR,
        KEEP,
        /// <summary>
        /// Data shift
        /// </summary>
        SFT,
        END,
    }

}
