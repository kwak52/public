using Newtonsoft.Json;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 사용자 정의 명령어.  see FunctionNodeUserDefined
    /// </summary>
    public class UserDefinedILCommand : ILCommand
    {
        /// <summary>
        /// Function input 의 각 다릿발에 붙일 명령어들의 prototype.
        /// e.g TTIM 의 경우,  [| "CTU C$0 $1"; "RST C$0 0" |]
        /// </summary>
        [JsonProperty]
        public string[] PerInputProc { get; private set; }

        /// <summary>
        /// 변환 명령어
        /// </summary>
        [JsonProperty]
        public string TargetCommand { get; private set; }

        /// <summary>
        /// Coil 에 해당하는 terminal 인지 여부
        /// </summary>
        [JsonProperty]
        public bool IsTerminal { get; private set; } = true;
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

        [JsonConstructor]
        protected UserDefinedILCommand() { }
    }
}
