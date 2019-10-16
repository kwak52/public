using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    public class ILSentence
    {
        public string Command { get; protected set; }
        public string[] Args { get; protected set; }
        public string Sentence { get; private set; }

        public Mnemonic Mnemonic { get; protected set; }

        PLCVendor VendorType;
        protected ILSentence(string sentence, PLCVendor vendorType)
        {
            var tokens = sentence.Split(new[] { ' ', '\t' });
            Command = tokens[0];
            Args = tokens.Skip(1).ToArray();
            Sentence = sentence;
            VendorType = vendorType;
            Mnemonic = IL.GetMnemonic(vendorType, Command);
        }

        protected ILSentence(ILSentence other)
        {
            Command = other.Command;
            Args = other.Args;
            Sentence = other.Sentence;
            VendorType = other.VendorType;
            Mnemonic = other.Mnemonic;
        }

        public override string ToString()
        {
            return $"{Command}\t{string.Join(" ", Args)}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
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
            Command = IL.GetOperator(_vendorType, other.Mnemonic);
        }

        public LSILSentence(string sentence)
            : base(sentence, PLCVendor.LSIS)
        {
            Command = IL.GetOperator(_vendorType, Mnemonic);
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
