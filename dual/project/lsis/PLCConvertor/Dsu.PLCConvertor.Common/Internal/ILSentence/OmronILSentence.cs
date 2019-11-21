using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dsu.PLCConvertor.Common
{
    public class OmronILSentence : ILSentence
    {
        public enum VariationType
        {
            None,
            /// <summary>
            /// "@"  Instruction that differentiates when the execution condition turns ON
            /// </summary>
            DiffrentiationOn,
            /// <summary>
            /// "%"  Instruction that differentiates when the execution condition turns OFF
            /// </summary>
            DiffrentiationOff,
            /// <summary>
            /// "!"  Refreshes data in the I/O area specified by the operands or the Special I/O Unit words when the instruction is executed.
            /// </summary>
            ImmediateRefreshing,
        }

        PLCVendor _vendorType = PLCVendor.Omron;

        public VariationType Variation { get; private set; }
        // 옴론 -> 산전 변환시 사용되지 않음.  변환 target 이 옴론일 경우 사용될 수 있음.
        public OmronILSentence(ILSentence other)
            : base(other)
        {
            Debugger.Break();
            if (Mnemonic == Mnemonic.UNDEFINED)
                Command = other.Command;
            else
            {
                // 옴론 type 의 command 로 변환한다.
                ILCommand = IL.GetILCommand(_vendorType, other.Mnemonic);
                Command = IL.GetOperator(_vendorType, other.Mnemonic);
            }
        }


        // 옴론 -> 산전 변환시 사용되는 생성자
        private OmronILSentence()
            : base(PLCVendor.Omron, true)
        {
        }


        /// <summary>
        /// 옴론 명령어의 code 부분을 normalize 한다.
        /// "MOVD(083)" --> "MOVD(83)"
        /// </summary>
        public static string NormalizeCommandAndCode(string command)
            => Regex.Replace(command, @"(?<command>)\(0*(?<code>\d+)\)", @"${command}(${code})");


        protected override string FilterCommand(string command)
        {
            if (! command[0].IsOneOf('@', '%', '!'))
                return command;

            string cmd = command;
            var normalCmd = cmd.Substring(1, cmd.Length - 1); ;

            if (cmd.StartsWith("@"))
            {
                Variation = VariationType.DiffrentiationOn;
                return normalCmd;
            }

            if (cmd.StartsWith("%"))
            {
                Variation = VariationType.DiffrentiationOff;
                return normalCmd;
            }
            if (cmd.StartsWith("!"))
            {
                Variation = VariationType.ImmediateRefreshing;
                return normalCmd;
            }

            return command;
        }

        private void FillMe(string sentence)
        {
            if (sentence.IsNullOrEmpty())
                return;

            base.Fill(sentence);
            var vendorType = PLCVendor.Omron;
            if (! Mnemonic.IsOneOf(Mnemonic.USERDEFINED, Mnemonic.RUNG_COMMENT))
            {
                Mnemonic = IL.GetMnemonic(vendorType, Command);
                ILCommand = IL.GetILCommand(vendorType, Mnemonic, Command);
            }

            if (Mnemonic == Mnemonic.UNDEFINED)
                Global.Logger.Warn($"Command {Command} not defined.");
        }

        public static OmronILSentence Create(string sentence)
        {
            var ils = new OmronILSentence();
            ils.FillMe(sentence);
            return ils;
        }
    }

}
