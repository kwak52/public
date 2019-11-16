using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// IL 변환을 위한 class
    /// </summary>
    public partial class IL
    {
        /// <summary>
        /// Terminal command 생성
        /// </summary>
        static List<ILCommand> _t(int arity, params string[] commands)
            => commands.Select(cmd => new TerminalILCommand(cmd, arity) as ILCommand).ToList();

        /// <summary>
        /// Non terminal command 생성
        /// </summary>
        static List<ILCommand> _n(int arity, params string[] commands)
            => commands.Select(cmd => new ILCommand(cmd, arity)).ToList();

        /// <summary>
        /// _dicLSIS 의 반대방향 참조용 dictionary.  mnemonic 문자열로 mnemonic 을 검색
        /// </summary>
        static Multimap<string, Mnemonic> _reverseDicLSIS;

        /// <summary>
        /// _dicOmron 의 반대방향 참조용 dictionary.  mnemonic 문자열로 mnemonic 을 검색
        /// </summary>
        static Multimap<string, Mnemonic> _reverseDicOmron;

        /// <summary>
        /// 사용자 정의 명령어 set
        /// </summary>
        public static UserDefinedCommandMapper UserDefinedCommandMapper { get; internal set; }

        static IL()
        {
            _reverseDicLSIS = _toReversedMultimap(_dicLSIS);
            _reverseDicOmron = _toReversedMultimap(_dicOmron);

            //UserDefinedCommands = 


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


        internal static Dictionary<Mnemonic, List<ILCommand>> GetDictionary(PLCVendor targetType)
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

            // 사용자 정의 우선
            if (command != null && UserDefinedCommandMapper != null)
            {
                var udc = UserDefinedCommandMapper?[command];
                if (udc != null)
                    return udc;
            }

            // 시스템 정의 사용
            var dic = GetDictionary(targetType);
            if (dic.ContainsKey(op))
                return dic[op].FirstOrDefault();

            // 사용자 정의에도 없고, system 정의에도 없는 경우, undefined command 로 반환
            return new UndefinedILCommand(command) { Arity = 1 };   // kkk: unknown command 의 deafult arity 는 1로 가정한다.
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
    }


#if DEBUG
    public static class ILTester
    {
        public static void Test()
        {
            /// <summary>
            /// 사용자 정의 명령어 set
            /// </summary>
            var userDefinedCommands =
                new[] {
                    new UserDefinedILCommand("STUP(237)", new [] { "MOV $0 $1", }) {Message = "Hello" },
                    new UserDefinedILCommand("CNTX(546)", new [] { "CTU C$0 $1", "RST C$0 0" }),
                    new UserDefinedILCommand("MOVD(083)", new [] { "MOV $0 $1 $2" }),
                    new UserDefinedILCommand("MOV(021)", new [] { "MOV $0 $1" }),
                    new UserDefinedILCommand("+(400)", new [] { "ADD $0 $1" }),
                };
            var userDefinedCommandMapper = new UserDefinedCommandMapper(userDefinedCommands);

            var jsonFile = "defaultCommandMapping.json";
            userDefinedCommandMapper.SaveToJsonFile(jsonFile);

            var dup = UserDefinedCommandMapper.LoadFromJsonFile(jsonFile, PLCVendor.Omron);
            Console.WriteLine("");
        }
    }
#endif
}
