using Newtonsoft.Json;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 사용자 정의 명령어.  see FunctionNodeUserDefined
    /// </summary>
    public class UserDefinedILCommand : ILCommand
    {
        [JsonProperty]
        public string[] PerInputProc { get; private set; }
        [JsonProperty]
        public string TargetCommand { get; private set; }

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
