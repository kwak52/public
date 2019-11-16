using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static Dictionary<string, PLCVariable> SourceVariableMap => ConvertParams.SourceVariableMap;
        public static Dictionary<string, PLCVariable> UsedSourceDevices => ConvertParams.UsedSourceDevices;


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
            var tokens = sentence.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            Command = tokens[0];
            Args = tokens.Skip(1).ToArray();
            Sentence = sentence;
            Mnemonic = IL.GetMnemonic(VendorType, Command);
            ILCommand = IL.GetILCommand(VendorType, Mnemonic, Command);
            if (ILCommand is UserDefinedILCommand)
                Mnemonic = Mnemonic.USERDEFINED;

            if (Mnemonic != Mnemonic.RUNG_COMMENT)
            {
                // Args : 옴론에서 변수명으로 사용된 것을 주소로 변환 
                Args =
                    Args.Select(arg =>
                    {
                        if (SourceVariableMap.ContainsKey(arg))
                        {
                            var v = SourceVariableMap[arg];
                            try
                            {
                                var d = ILSentence.AddressConvertorInstance.Convert(v.Device);
                                if (!UsedSourceDevices.ContainsKey(d))
                                    UsedSourceDevices.Add(d, new PLCVariable(d, v));
                                return v.Device;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        return arg;

                    })
                    .ToArray();
            }
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
                        case PLCVendor.LSIS: sentence = LSILSentence.Create($"{Xg5k.RungCommentCommand}\t{rc}"); break;
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

        public bool IsAndFamily()
        {
            if (Mnemonic.IsOneOf(Mnemonic.AND, Mnemonic.ANDEQ,
                    Mnemonic.ANDGREATERTHAN, Mnemonic.ANDLESSTHAN, Mnemonic.ANDNOT))
                return true;

            if (Mnemonic == Mnemonic.USERDEFINED && Command.Contains("AND"))
            {
                var match = Regex.Match(Command, @"([^\(]*)\((\d*)\)");
                var g = match.Groups.Cast<Group>().Select(gr => gr.ToString()).ToArray();
                if (g.Length == 3)
                {
                    var code = int.Parse(g[2]);
                    return code.InRange(300, 330);
                }
            }

            return false;
        }

    }

}
