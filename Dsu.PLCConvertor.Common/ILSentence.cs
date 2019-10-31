using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// IL sentence.  실제 사용된 IL command 및 argument 를 표현하기 위한 class
    /// </summary>
    public class ILSentence
    {
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
            ILCommand = IL.GetILCommand(vendorType, Mnemonic);
        }

        protected ILSentence(ILSentence other)
        {
            Command = other.Command;
            Args = other.Args;
            Sentence = other.Sentence;
            VendorType = other.VendorType;
            Mnemonic = other.Mnemonic;
            ILCommand = other.ILCommand;
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
            ILCommand = IL.GetILCommand(_vendorType, other.Mnemonic);
            Command = IL.GetOperator(_vendorType, other.Mnemonic);
        }

        public LSILSentence(string sentence)
            : base(sentence, PLCVendor.LSIS)
        {
            ILCommand = IL.GetILCommand(_vendorType, Mnemonic);
            Command = IL.GetOperator(_vendorType, Mnemonic);
        }


        public static bool UseDirtyOperandReplaceImplementation = true;
        public override string ToString()
        {
            if (UseDirtyOperandReplaceImplementation)
            {
                var args =
                    Args.Select(arg => arg.Replace("0.", "P0").Replace("1.", "P1").Replace("2.", "P2"));
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
        public OmronILSentence(ILSentence other)
            : base(other)
        {
            Command = IL.GetOperator(_vendorType, other.Mnemonic);
        }

        public OmronILSentence(string sentence)
            : base(sentence, PLCVendor.Omron)
        {
            Command = IL.GetOperator(_vendorType, Mnemonic);
        }
    }

}
