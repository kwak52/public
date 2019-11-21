using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static Dictionary<string, PLCVariable> UsedSourceDevices => ConvertParams.UsedSourceDevices;


        public string Command { get; protected set; }
        public string[] Args { get; protected set; }
        public string Sentence { get; private set; }

        public Mnemonic Mnemonic { get; protected set; }

        public ILCommand ILCommand { get; protected set; }
        public int Arity => ILCommand.Arity;

        PLCVendor VendorType;
        protected bool IsSource { get; set; }

        protected ILSentence(PLCVendor vendorType, bool isSource)
        {
            VendorType = vendorType;
            IsSource = isSource;
        }

        protected virtual string FilterCommand(string command) => command;

        protected virtual string ModifiyArgument(string arg, int nth) { return arg; }
        public void ModifyArguments() { Args = Args.Select((arg, n) => ModifiyArgument(arg, n)).ToArray(); }
        protected void Fill(string sentence)
        {
            var tokens = sentence.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            Command = FilterCommand(tokens[0]);
            if (VendorType == PLCVendor.Omron)
                Command = OmronILSentence.NormalizeCommandAndCode(Command);

            Args = tokens.Skip(1).ToArray();
            Sentence = sentence;
            Mnemonic = IL.GetMnemonic(VendorType, Command);
            ILCommand = IL.GetILCommand(VendorType, Mnemonic, Command);
            if (ILCommand is UserDefinedILCommand)
                Mnemonic = Mnemonic.USERDEFINED;

            if (Mnemonic != Mnemonic.RUNG_COMMENT)
            {
                if (IsSource)
                {
                    // Args : 옴론에서 변수명으로 사용된 것을 주소로 변환 
                    Args =
                        Args.Select(arg =>
                        {

                            var v = ConvertParams.SearchVariable(arg);
                            if (v != null)
                            {
                                try
                                {
                                    var d = ILSentence.AddressConvertorInstance.Convert(v.Device);
                                    if (!UsedSourceDevices.ContainsKey(d))
                                        UsedSourceDevices.Add(d, new PLCVariable(d, v));
                                    return v.Device;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            return arg;

                        })
                        .ToArray();
                }
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

            IsSource = false;

            if (Mnemonic == Mnemonic.UNDEFINED || Mnemonic == Mnemonic.USERDEFINED)
                Console.WriteLine("");
        }

        public override string ToString()
        {
            return $"{Command} {string.Join(" ", Args)}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
        }

        public static ILSentence Create(PLCVendor vendorType, string sentence)
        {
            ILSentence ils = null;
            switch (vendorType)
            {
                case PLCVendor.LSIS: Debugger.Break(); ils = LSILSentence.Create(sentence); break;
                case PLCVendor.Omron: ils = OmronILSentence.Create(sentence); break;
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{vendorType}");
            }

            return ils;
        }

        public static IEnumerable<ILSentence> CreateRungComments(PLCVendor vendorType, string rungComments)
        {
            return
                CxtParser.SplitBlock(rungComments, StringSplitOptions.RemoveEmptyEntries)
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
            ILSentence ils = null;
            switch (vendorType)
            {
                case PLCVendor.LSIS: ils = new LSILSentence(sentence); break;
                case PLCVendor.Omron: ils = new OmronILSentence(sentence); break;
                default:
                    throw new NotImplementedException($"Unknown target PLC type:{vendorType}");
            }

            ils.ModifyArguments();
            return ils;
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


        public bool IsOrFamily()
        {
            if (Mnemonic.IsOneOf(Mnemonic.OR, Mnemonic.OREQ,
                    /*Mnemonic.ORGREATERTHAN, Mnemonic.ORLESSTHAN,*/ Mnemonic.ORNOT))
                return true;

            if (Mnemonic == Mnemonic.USERDEFINED && Command.Contains("OR"))
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
