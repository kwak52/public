using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// IL sentence.  실제 사용된 IL command 및 argument 를 표현하기 위한 class
    /// </summary>
    public class ILSentence
    {
        /// <summary>
        /// IL 문장 변환에 사용될 address convertor instance.
        /// </summary>
        public static AddressConvertor AddressConvertorInstance { get; set; }

        public string Command { get; protected set; }
        public string[] Args { get; protected set; }
        public string Sentence { get; private set; }

        public Mnemonic Mnemonic { get; protected set; }

        public ILCommand ILCommand { get; protected set; }
        public int Arity => ILCommand.Arity;

        PLCVendor VendorType;

        protected ILSentence(PLCVendor vendorType)
        {
            VendorType = vendorType;
        }

        protected void Fill(string sentence)
        {
            var tokens = sentence.Split(new[] { ' ', '\t' });
            Command = tokens[0];
            Args = tokens.Skip(1).ToArray();
            Sentence = sentence;
            Mnemonic = IL.GetMnemonic(VendorType, Command);
            ILCommand = IL.GetILCommand(VendorType, Mnemonic, Command);
            if (ILCommand is UserDefinedILCommand)
                Mnemonic = Mnemonic.USERDEFINED;

        }

        protected ILSentence(ILSentence other)
        {
            Command = other.Command;
            Args = other.Args;
            Sentence = other.Sentence;
            VendorType = other.VendorType;
            Mnemonic = other.Mnemonic;
            ILCommand = other.ILCommand;

            if (Mnemonic == Mnemonic.UNDEFINED || Mnemonic == Mnemonic.USERDEFINED)
                Console.WriteLine("");
        }

        public override string ToString()
        {
            return $"{Command} {string.Join(" ", Args)}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
        }

        public static ILSentence Create(PLCVendor vendorType, string sentence)
        {
            switch(vendorType)
            {
                case PLCVendor.LSIS: return LSILSentence.Create(sentence);
                case PLCVendor.Omron: return OmronILSentence.Create(sentence);
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{vendorType}");
            }
        }

        public static IEnumerable<ILSentence> CreateRungComments(PLCVendor vendorType, string rungComments)
        {
            return
                rungComments
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(rc =>
                {
                    ILSentence sentence = null;
                    switch (vendorType)
                    {
                        case PLCVendor.LSIS: sentence = LSILSentence.Create($"CMT\t{rc}"); break;
                        case PLCVendor.Omron: sentence = OmronILSentence.Create($"'\t{rc}"); break;
                        default:
                            throw new NotImplementedException($"Unknown target PLC type:{vendorType}");
                    }

                    sentence.Mnemonic = Mnemonic.RUNG_COMMENT;
                    return sentence;
                });
        }

        public static ILSentence Create(PLCVendor vendorType, ILSentence sentence)
        {
            switch (vendorType)
            {
                case PLCVendor.LSIS: return new LSILSentence(sentence);
                case PLCVendor.Omron: return new OmronILSentence(sentence);
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{vendorType}");
            }
        }
    }
    
    public class LSILSentence : ILSentence
    {
        PLCVendor _vendorType = PLCVendor.LSIS;
        public LSILSentence(ILSentence other)
            : base(other)
        {
            switch(Mnemonic)
            {
                case Mnemonic.UNDEFINED:
                    Command = other.Command;
                    break;
                case Mnemonic.USERDEFINED:
                    var udc = other.ILCommand as UserDefinedILCommand;
                    Command = udc.TargetCommand;
                    break;
                default:
                    // 산전 format 의 Command 로 변환한다.
                    ILCommand = IL.GetILCommand(_vendorType, other.Mnemonic);
                    Command = IL.GetOperator(_vendorType, other.Mnemonic);
                    break;
            }
        }

        // 옴론 -> 산전 변환시 사용되지 않음.
        private LSILSentence()
            : base(PLCVendor.LSIS)
        {
            Debugger.Break();
        }


        /// <summary>
        /// Address mapping 사용 여부.  default 는 true.  Unit Test 등에서 임시로 disable
        /// </summary>
        public static bool UseAddressMapping = true;
        public override string ToString()
        {
            if (UseAddressMapping)
            {
                var rs = AddressConvertorInstance;
                var args = Args.Select(arg => rs.IsMatch(arg) ? rs.Convert(arg) : arg);
                var operands = string.Join(" ", args);
                return $"{Command}\t{operands}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
            }
            else
                return $"{Command}\t{string.Join(" ", Args)}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
        }

        public static LSILSentence Create(string sentence)
        {
            var ils = new LSILSentence();
            ils.Fill(sentence);
            return ils;
        }

    }


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
