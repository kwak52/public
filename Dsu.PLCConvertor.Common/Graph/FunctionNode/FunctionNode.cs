using Dsu.PLCConvertor.Common.Internal;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// Function node : 함수의 input 다릿발을 가진다.
    /// Rung 의 coil 위치에 위치할 수도 있고(TerminalFunctionNode)
    /// Rung 의 내부 조건으로 사용될 수도 있다(InnerFunctionNode)
    /// </summary>
    internal class FunctionNode : Point, IFunctionNode
    {
        /// <summary>
        /// 함수의 다릿발.  함수 본체로 들어오는 subrung 들의 입력.
        /// e.g 옴론에서 TTIM(->timer) 함수는 timer 를 구동시키기 위한 입력 조건1과
        /// 해당 timer 를 reset 시키기 위한 입력 조건2가 동시에 입력으로 주어진다.
        /// </summary>
        public List<SubRung> Inputs { get; private set; } = new List<SubRung>();
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
            return Inputs.Select(subRung => Rung2ILConvertor.Convert(subRung, cvtParam).Results.ToList()).ToList();
        }

        public override string ToShortString() => $"{ILSentence}";

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
            switch (sentence.Mnemonic)
            {
                case Mnemonic.SFT: return new FunctionNodeSFT(sentence);
                case Mnemonic.TMR: return new FunctionNodeTMR(sentence);
                case Mnemonic.KEEP: return new FunctionNodeSetReset(sentence);
                case Mnemonic.CTU: return new FunctionNodeCTU(sentence);
                case Mnemonic.USERDEFINED:
                    if (sentence.ILCommand is UserDefinedILCommand)
                        return new UserDefinedTerminalFunctionNode(sentence);

                    return new UserDefinedInnerFunctionNode(sentence);
                default:
                    Debug.Assert(false);
                    return null;
            }
        }
    }
}
