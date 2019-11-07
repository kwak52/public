using Dsu.PLCConvertor.Common.Internal;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// Rung 의 출력 부분이 함수로 구성된 node 를 표현하기 위한 class.
    /// e.g Timer, Shift, ..
    /// </summary>
    internal class FunctionNode : TerminalNode
    {
        /// <summary>
        /// 함수의 다릿발.  함수 본체로 들어오는 subrung 들의 입력.
        /// e.g 옴론에서 TTIM(->timer) 함수는 timer 를 구동시키기 위한 입력 조건1과
        /// 해당 timer 를 reset 시키기 위한 입력 조건2가 동시에 입력으로 주어진다.
        /// </summary>
        public List<SubRung> Inputs { get; private set; } = new List<SubRung>();
        protected FunctionNode(string name, ILSentence sentence)
            : base(name, sentence)
        {
        }

        protected FunctionNode(ILSentence sentence)
            : this(sentence.Command, sentence)
        {
        }

        public static FunctionNode Create(ILSentence sentence)
        {
            switch(sentence.Mnemonic)
            {
                case Mnemonic.SFT: return new FunctionNodeSFT(sentence);
                case Mnemonic.TMR: return new FunctionNodeTMR(sentence);
                case Mnemonic.KEEP: return new FunctionNodeSetReset(sentence);
                case Mnemonic.CTU: return new FunctionNodeCTU(sentence);
                case Mnemonic.USERDEFINED: return new FunctionNodeUserDefined(sentence);
                default:
                    Debug.Assert(false);
                    return null;
            }
        }
        public override string ToShortString() => $"{ILSentence}";

        public virtual IEnumerable<string> Convert(ConvertParams cvtParam) { Debug.Assert(false); yield break; }

        /// <summary>
        /// {함수의 개별 다릿발에 대해서 변환한 결과} 의 list 를 반환.
        /// 추후 여기에 함수 원형 변환결과와 결합한다.
        /// e.g FunctionNodeTMR.Convert() 함수 참고.  옴론 TTIM 의 조건1 과 조건2 에 대해서 
        /// 조건 1에는 TMR 호출을 붙이고, 조건2에는 RST 조건을 붙여서 변환을 완성한다.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected virtual List<List<string>> ConvertPerInputs(ConvertParams cvtParam)
        {
            return Inputs.Select(subRung => Rung2ILConvertor.Convert(subRung, cvtParam).ToList()).ToList();
        }
    }


    /// <summary>
    /// 사용자 정의 function node.  see UserDefinedILCommand
    /// </summary>
    internal class FunctionNodeUserDefined : FunctionNode
    {
        UserDefinedILCommand _userDefinedCommand => ILSentence.ILCommand as UserDefinedILCommand;
        public FunctionNodeUserDefined(ILSentence sentence)
            : base(sentence)
        {
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

            var args = ILSentence.Args; // CNTX 에 주어진 argument 목록
            
            // function 의 input 다릿발에 연결될 sub rung 의 IL 문장 들
            var perInputs = ConvertPerInputs(cvtParam);

            var perInputProcs =
                _userDefinedCommand.PerInputProc        // function input 의 각 다릿발에 붙일 명령어들 prototype.  e.g [| "CTU C$0 $1"; "RST C$0 0" |]
                .Select(pip =>
                {
                    for (int i = 0; i < Arity; i++)
                        pip = pip.Replace($"${i}", args[i]);    // prototype 의 positional argument (-> $??) 를 실제 argument 로 치환
                    pip = pip.Replace(" ", "\t");
                    return pip;
                })
                .ToArray()
                ;

            for (int i = 0; i < Arity; i++)
                perInputs[i].Add(perInputProcs[i]);

            return perInputs.SelectMany(paragarph => paragarph);
        }
    }



    /// <summary>
    /// Timer 설정 관련 function node
    /// </summary>
    internal class FunctionNodeTMR : FunctionNode
    {
        public FunctionNodeTMR(ILSentence sentence)
            : base(sentence)
        {
        }

        /// <summary>
        /// 옴론 TTIM 의 조건1 과 조건2 에 대해서 
        /// 조건 1에는 TMR 호출을 붙이고, 조건2에는 RST 조건을 붙여서 변환을 완성한다.
        /// </summary>
        public override IEnumerable<string> Convert(ConvertParams cvtParam)
        {
            var perInputs = ConvertPerInputs(cvtParam);

            // TODO: argument 에 대해서는 구체화하고, 생성하는 routine 은 일반화한다.
            var tVar = ILSentence.Args[0];
            var tArg = ILSentence.Args[1];
            perInputs[0].Add($"TMR T{tVar} {tArg}");
            perInputs[1].Add($"RST T{tVar}");

            return perInputs.SelectMany(paragarph => paragarph);
        }
    }


    /// <summary>
    /// Shift 설정 관련 function node
    /// </summary>
    internal class FunctionNodeSFT : FunctionNode
    {
        public FunctionNodeSFT(ILSentence sentence)
            : base(sentence)
        {

        }

        public override IEnumerable<string> Convert(ConvertParams cvtParam)
        {
            Global.Logger.Warn($"FunctionNodeSFT.Convert() not implemented, yet!");
            yield break;
        }
    }

    

    internal class FunctionNodeCTU : FunctionNode
    {
        public FunctionNodeCTU(ILSentence sentence)
            : base(sentence)
        {
        }

        public override IEnumerable<string> Convert(ConvertParams cvtParam)
        {
            Global.Logger.Warn($"FunctionNodeCTU.Convert() not implemented, yet!");
            yield break;
        }
    }

    /// <summary>
    /// Keep(Set/Reset) function node
    /// </summary>
    internal class FunctionNodeSetReset : FunctionNode
    {
        public FunctionNodeSetReset(ILSentence sentence)
            : base(sentence)
        {

        }

        public override IEnumerable<string> Convert(ConvertParams cvtParam)
        {
            var perInputs = ConvertPerInputs(cvtParam);

            var tVar = ILSentence.Args[0];
            perInputs[0].Add($"SET {tVar}");
            perInputs[1].Add($"RST {tVar}");

            return perInputs.SelectMany(paragarph => paragarph);
        }
    }
}
