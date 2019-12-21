using Dsu.PLCConvertor.Common.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{

    /// <summary>
    /// Rung 의 출력 부분이 함수로 구성된 node 를 표현하기 위한 class.
    /// e.g Timer, Shift, ..
    /// </summary>
    internal class TerminalFunctionNode : FunctionNode, ITerminalNode
    {
        protected TerminalFunctionNode(string name, ILSentence sentence)
            : base(name, sentence)
        {
        }

        protected TerminalFunctionNode(ILSentence sentence)
            : this(sentence.Command, sentence)
        {
        }

    }

    internal class UserDefinedInnerFunctionNode : UserDefinedFunctionNode
    {
        public UserDefinedInnerFunctionNode(ILSentence sentence)
            : base(sentence)
        {
        }
    }

    internal class UserDefinedTerminalFunctionNode : UserDefinedFunctionNode, ITerminalNode
    {
        public UserDefinedTerminalFunctionNode(ILSentence sentence)
            : base(sentence)
        {
        }

    }


    /// <summary>
    /// Timer 설정 관련 function node
    /// </summary>
    internal class FunctionNodeTMR : TerminalFunctionNode
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

            var tVar = ILSentence.Args[0];
            var tArg = ILSentence.Args[1];
            perInputs[0].Add($"TMR\tT{tVar} {tArg}");
            perInputs[1].Add($"RST\tT{tVar}");

            return perInputs.SelectMany(paragarph => paragarph);
        }
    }


    /// <summary>
    /// Shift 설정 관련 function node
    /// </summary>
    internal class FunctionNodeSFT : TerminalFunctionNode
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

    

    internal class FunctionNodeCTU : TerminalFunctionNode
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
    internal class FunctionNodeSetReset : TerminalFunctionNode
    {
        public FunctionNodeSetReset(ILSentence sentence)
            : base(sentence)
        {

        }

        public override IEnumerable<string> Convert(ConvertParams cvtParam)
        {
            var perInputs = ConvertPerInputs(cvtParam);

            var tVar = ILSentence.Args[0];
            perInputs[0].Add($"SET\t{tVar}");
            perInputs[1].Add($"RST\t{tVar}");

            return perInputs.SelectMany(paragarph => paragarph);
        }
    }

    /// <summary>
    /// BSET function node
    /// </summary>
    internal class FunctionNodeBSET : TerminalFunctionNode
    {
        public FunctionNodeBSET(ILSentence sentence) : base(sentence) {}
    }

}
