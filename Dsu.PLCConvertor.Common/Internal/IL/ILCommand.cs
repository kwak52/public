using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class ILCommand
    {
        /// <summary>
        /// IL 명령어
        /// </summary>
        [JsonProperty]
        public string Command { get; protected set; }

        /// <summary>
        /// IL 명령어의 input 다릿발 갯수
        /// </summary>
        [JsonProperty]
        public int Arity { get; internal set; }
        public ILCommand(string command, int arity=0)
        {
            Command = command;
            Arity = arity;
        }

        [JsonConstructor]
        protected ILCommand() { }
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

        ADD,
        SUB,
        MUL,
        DIV,

        BCD,
        BIN,

        /// <summary>
        /// SIGNED BINARY COMPARE
        /// </summary>
        CPS,

        ANDEQ,
        OREQ,

        ANDLESSTHAN,
        ANDGREATERTHAN,


        /// <summary>
        /// Bit reset
        /// </summary>
        RSTB,

        CONCATSTRING,
        
        NOP,
        RUNG_COMMENT,
        END,
    }
}
