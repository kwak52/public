using Dsu.PLCConvertor.Common.Internal;
using System;
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
        protected ILSentence(string sentence, PLCVendor vendorType)
        {
            var tokens = sentence.Split(new[] { ' ', '\t' });
            Command = tokens[0];
            Args = tokens.Skip(1).ToArray();
            Sentence = sentence;
            VendorType = vendorType;
            Mnemonic = IL.GetMnemonic(vendorType, Command);
            ILCommand = IL.GetILCommand(vendorType, Mnemonic, Command);
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
                case PLCVendor.LSIS: return new LSILSentence(sentence);
                case PLCVendor.Omron: return new OmronILSentence(sentence);
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{vendorType}");
            }
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
        public LSILSentence(string sentence)
            : base(sentence, PLCVendor.LSIS)
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
                return base.ToString();
        }
    }


    public class OmronILSentence : ILSentence
    {
        PLCVendor _vendorType = PLCVendor.Omron;
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
        public OmronILSentence(string sentence)
            : base(sentence, PLCVendor.Omron)
        {
        }
    }

}
