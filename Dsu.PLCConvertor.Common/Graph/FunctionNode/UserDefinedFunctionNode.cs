﻿using Dsu.PLCConvertor.Common.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// 사용자 정의 function node.  see UserDefinedILCommand
    /// </summary>
    internal class UserDefinedFunctionNode : FunctionNode, IUserDefinedFunctionNode
    {
        UserDefinedILCommand _userDefinedCommand => ILSentence.ILCommand as UserDefinedILCommand;
        internal UserDefinedFunctionNode(ILSentence sentence)
            : base(sentence)
        {
        }

        public IEnumerable<string> EnumeratePerInputs()
        {
            var args = ILSentence.Args; // CNTX 에 주어진 argument 목록
            return
                _userDefinedCommand.PerInputProc        // function input 의 각 다릿발에 붙일 명령어들 prototype.  e.g [| "CTU C$0 $1"; "RST C$0 0" |]
                .Select(pip =>
                {
                    for (int i = 0; i < args.Length; i++)
                        pip = pip.Replace($"${i}", args[i]);    // kkk: prototype 의 positional argument (-> $??) 를 실제 argument 로 치환
                    pip = pip.Replace(" ", "\t")
                            .Replace("\tNOT", " NOT");    // 잘못 치환된 AND<TAB>NOT 을 AND NOT 으로 다시 변환
                    return pip;
                })
                ;

        }

        /// <summary>
        /// 사용자 정의 function 에 대해서
        /// input arity 에 맞게 sub rung 들의 변환 결과를 붙여서 완성한다.
        /// </summary>
        public override IEnumerable<string> Convert(ConvertParams cvtParam)
        {
            /*
             * e.g 사용자 정의 : new UserDefinedILCommand("CNTX(546)", "CTU", new [] { "CTU C$0 $1", "RST C$0 0" }),
             */
            
            // function 의 input 다릿발에 연결될 sub rung 의 IL 문장 들
            var perInputs = ConvertPerInputs(cvtParam);

            var perInputProcs =
                EnumeratePerInputs()
                .ToArray()
                ;

            for (int i = 0; i < Arity; i++)
                perInputs[i].Add(perInputProcs[i]);

            return perInputs.SelectMany(paragarph => paragarph);
        }
    }
}
