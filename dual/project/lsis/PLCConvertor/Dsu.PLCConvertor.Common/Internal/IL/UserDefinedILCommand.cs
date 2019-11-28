using Dsu.Common.Utilities.ExtensionMethods;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 사용자 정의 명령어.  see FunctionNodeUserDefined, UserDefinedCommandMapper
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

        /// <summary>
        /// LOAD 에 해당하는 명령 인지 여부
        /// </summary>
        [JsonProperty(Order = 4)]
        public bool IsLoad { get; private set; } = false;

        /// <summary>
        /// 변환 결과에 반영할 메시지
        /// </summary>
        [JsonProperty(Order = 5)]
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

        /// <summary>
        /// 재정의 금지 명령어
        /// </summary>
        static string[] _forbiddenOverrides = new[]
        {
            "LD", "AND", "OR", "OUT",
            "@LDNOT", "%LDNOT",
            "@ANDNOT", "%ANDNOT",
            "TIM",
            "BSET(071)",
        };

        public bool Validate(PLCVendor targetType, bool throwOnFail=true)
        {
            if (IsTerminal && IsLoad)
            {
                throwOnDemand(new Exception($"[{Command}] Both IsTeminal and IsLoad can't be true."));
                return false;
            }

            if (PerInputProc.IsNullOrEmpty())
            {
                throwOnDemand(new Exception($"[{Command}] PerInputProc is empty."));
                return false;
            }

            if (_forbiddenOverrides.Contains(Command))
            {
                throwOnDemand(new Exception($"[{Command}] is not allowed to be overridden."));
                return false;
            }

            if (targetType == PLCVendor.Omron)
                Command = OmronILSentence.NormalizeCommandAndCode(Command);
            

            return true;

            void throwOnDemand(Exception ex)
            {
                if (throwOnFail)
                    throw ex;
            }
        }
    }
}
