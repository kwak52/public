using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System.Diagnostics;

namespace Dsu.PLCConvertor.Common
{
    public class OmronILSentence : ILSentence
    {
        public enum VariationType
        {
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
            : base(PLCVendor.Omron)
        {
        }


            
        private void Fill(string sentence)
        {
            if (sentence.IsNullOrEmpty())
                return;

            base.Fill(sentence);
            var vendorType = PLCVendor.Omron;
            Command = getCommand();
            if (! Mnemonic.IsOneOf(Mnemonic.USERDEFINED, Mnemonic.RUNG_COMMENT))
            {
                Mnemonic = IL.GetMnemonic(vendorType, Command);
                ILCommand = IL.GetILCommand(vendorType, Mnemonic, Command);
            }

            if (Mnemonic == Mnemonic.UNDEFINED)
                Global.Logger.Warn($"Command {Command} not defined.");

            string getCommand()
            {
                string cmd = Command;
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

                return Command;
            }
        }

        public static OmronILSentence Create(string sentence)
        {
            var ils = new OmronILSentence();
            ils.Fill(sentence);
            return ils;
        }
    }

}
