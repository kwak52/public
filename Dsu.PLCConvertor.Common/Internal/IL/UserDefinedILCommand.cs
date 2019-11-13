using Newtonsoft.Json;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 사용자 정의 명령어.  see FunctionNodeUserDefined
    /// 실제 serialize 지원 class
    /// </summary>
    public class UserDefinedILCommand : ILCommand
    {
        /// <summary>
        /// Function input 의 각 다릿발에 붙일 명령어들의 prototype.
        /// e.g TTIM 의 경우,  [| "CTU C$0 $1"; "RST C$0 0" |]
        /// </summary>
        [JsonProperty(Order = 2)]
        public string[] PerInputProc { get; private set; }

        /// <summary>
        /// Coil 에 해당하는 terminal 인지 여부
        /// </summary>
        [JsonProperty(Order = 3)]
        public bool IsTerminal { get; private set; } = true;

        [JsonProperty(Order = 4)]
        public string Message { get; internal set; }

        public UserDefinedILCommand(string json)
            : base("UserDefinedCommand", 1)
        {
        }

        // e.g TMR 명령이라면, perInputProc = [| "TMR $0 $1"; "RST T0" |]
        public UserDefinedILCommand(string command, string[] perInputProc)
            : base(command, perInputProc.Length)
        {
            PerInputProc = perInputProc;
        }

        [JsonConstructor]
        protected UserDefinedILCommand() { }
    }
}
