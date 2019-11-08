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
        public int Arity { get; internal set; }
        public ILCommand(string command, int arity=0)
        {
            Command = command;
            Arity = arity;
        }
    }

    /// <summary>
    /// 사전 정의되지 않은 명령어
    /// </summary>
    public class UndefinedILCommand : ILCommand
    {
        public UndefinedILCommand(string command)
            : base(command, 0)
        {
        }
    }

    /// <summary>
    /// coil 에 해당하는 명령어.  OUT, TMR, ...
    /// </summary>
    public class TerminalILCommand : ILCommand
    {
        public TerminalILCommand(string command, int arity = 0)
            : base(command, arity)
        {
        }
    }

    /// <summary>
    /// 사용자 정의 명령어.  see FunctionNodeUserDefined
    /// </summary>
    public class UserDefinedILCommand : ILCommand
    {
        public string[] PerInputProc { get; private set; }
        public string TargetCommand { get; private set; }
        public UserDefinedILCommand(string json)
            : base("UserDefinedCommand", 1)
        {
        }

        // e.g TMR 명령이라면, perInputProc = [| "TMR $0 $1"; "RST T0" |]
        public UserDefinedILCommand(string command, string targetCommand, string[] perInputProc)
            : base(command, perInputProc.Length)
        {
            PerInputProc = perInputProc;
            TargetCommand = targetCommand;
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
            [Mnemonic.CMP] = _toList(1, "CMP"),
            [Mnemonic.MOVE] = _toList(1, "MOV"),
            [Mnemonic.END] = _toList(1, "END"),

            [Mnemonic.RUNG_COMMENT] = _toList(0, "CMT"),
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
            [Mnemonic.CMP] = _toList(1, "CMP(020)"),
            [Mnemonic.MOVE] = _toList(1, "MOV(021)"),
            [Mnemonic.END] = _toList(1, "END(001)"),

            [Mnemonic.RUNG_COMMENT] = _toList(0, "'"),

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
        public static ILCommand GetILCommand(PLCVendor targetType, Mnemonic op, string command=null)
        {
            var dic = GetDictionary(targetType);
            if (dic.ContainsKey(op))
                return dic[op].FirstOrDefault();

            var udc = _userDefinedCommands.FirstOrDefault(c => c.Command == command);
            if (udc != null)
                return udc;
           
            return new UndefinedILCommand(command) { Arity = 1 };   // unknown command 의 deafult arity 는 1로 가정한다.
        }

        /// <summary>
        /// targetType 에 맞는 문자열의 mnemoinc 값을 반환
        /// </summary>
        public static Mnemonic GetMnemonic(PLCVendor targetType, string op)
        {
            var dic = GetReversedDictionary(targetType);
            if (dic.ContainsKey(op))
                return dic[op].FirstOrDefault();

            return Mnemonic.UNDEFINED;
        }


        /// <summary>
        /// 사용자 정의 명령어 set
        /// </summary>
        static List<UserDefinedILCommand> _userDefinedCommands =
            new List<UserDefinedILCommand>(
                new [] {
                    new UserDefinedILCommand("STUP(237)", "OUT", new [] { "MOV $0 $1", }),
                    new UserDefinedILCommand("CNTX(546)", "CTU", new [] { "CTU C$0 $1", "RST C$0 0" }),
                    new UserDefinedILCommand("MOVD(083)", "MOV", new [] { "MOV $0 $1 $2" }),
                    new UserDefinedILCommand("MOV(021)", "MOV", new [] { "MOV $0 $1" }),
                    new UserDefinedILCommand("+(400)", "ADD", new [] { "ADD $0 $1" }),
                }
            );
    }

    /// <summary>
    /// IL mnemonic 열거
    /// </summary>
    public enum Mnemonic
    {
        /// <summary>
        /// 사전 등록된 명령어 set 에 포함되지 않고, 사용자 정의 명령어에도 포함되지 않은 경우
        /// </summary>
        UNDEFINED = 0,
        /// <summary>
        /// 사용자 정의 명령어에 포함된 경우
        /// </summary>
        USERDEFINED,
        LOAD, LOADNOT,      
        AND, ANDNOT, ANDLD,
        OR, ORNOT, ORLD,
        OUT,
        MOVE,
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
        /// <summary>
        /// Compare
        /// </summary>
        CMP,
        RUNG_COMMENT,
        END,
    }

}
