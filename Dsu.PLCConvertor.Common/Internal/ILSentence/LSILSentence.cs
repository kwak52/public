using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dsu.PLCConvertor.Common
{
    public class LSILSentence : ILSentence
    {
        PLCVendor _vendorType = PLCVendor.LSIS;

        
        internal LSILSentence(Rung2ILConvertor r2iConverter, ILSentence other)
            : base(r2iConverter, other)
        {
            switch (Mnemonic)
            {
                case Mnemonic.UNDEFINED:
                    Command = other.Command;
                    break;

                case Mnemonic.USERDEFINED:
                    var udc = other.ILCommand as UserDefinedILCommand;
                    Command = udc.PerInputProc[0].Split(new[] { ' ', '\t' })[0];
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

        public static LSILSentence Create(string sentence)
        {
            Debugger.Break();

            var ils = new LSILSentence();
            ils.Fill(sentence);
            return ils;
        }


        /// <summary>
        /// 변환시 필요하면 Argument 변환
        /// e.g : 옴론의 '#' 으로 시작하는 hex constant 를 산전의 'H' 로 시작하도록 변경
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="nth">argument 의 position.  첫번째 argument 는 0 의 값을 가짐</param>
        /// <returns></returns>
        protected override string ModifiyArgument(string arg, int nth)
        {
            switch(Mnemonic)
            {
                case Mnemonic.TON when nth == 0:
                    return $"T{arg}";
                case Mnemonic.TON when nth == 1 && arg.StartsWith("#"):
                    return arg.SkipNChar(1);
            }

            // '#': hexadecimal
            var match = Regex.Match(arg, "^#([A-Fa-f0-9]+)");
            if (match.Success)
                return Regex.Replace(arg, "^#", "H");

            // '&': decimal
            match = Regex.Match(arg, @"^&(\d+)");
            if (match.Success)
                return Regex.Replace(arg, "^&", "");

            // '+': 
            match = Regex.Match(arg, @"^\+(\d+)");
            if (match.Success)
                return Regex.Replace(arg, @"^\+", "");

            return arg;
        }

        // Args : 산전으로 변환되기 이전의 상태를 갖고 있다.  Args 에도 옴론의 argument 가 들어 있는 상태
        // Command : 변환 이후의 상태
        public override string[] ModifyArguments()
        {
            var args = base.ModifyArguments();
            if (Mnemonic == Mnemonic.BSET)
            {
                // BSET(071) #4 D20002 D20009 ---> FMOV #4 D20002 8 의 결과가 나오도록 D20009 를 8 (9-2+1) 로 수정
                var leadingLength =
                    Args[1].Zip(Args[2], (f, s) => (f, s))
                    .TakeWhile(tpl => tpl.f == tpl.s)
                    .Count()
                    ;
                var n1 = int.Parse(Args[1].SkipNChar(leadingLength));
                var n2 = int.Parse(Args[2].SkipNChar(leadingLength));
                args[2] = $"{n2 - n1 + 1}";
            }

            return args;
        }


        protected override string FilterCommand(string command)
        {
            var omron = _sourceILSentence as OmronILSentence;
            if (omron != null)
            {
                // diffrentiation 과 NOT 이 함께 쓰인 것은 산전으로 직접 변환이 불가능.
                // NOT 없이 사용되면 P 나 N 을 붙여서 처리
                // ONS
                var containsNot = Command.Contains("NOT");
                switch (omron.Variation)
                {
                    case OmronILSentence.VariationType.DiffrentiationOn:
                        if (!containsNot)
                            Command = Command + "P";
                        break;
                    case OmronILSentence.VariationType.DiffrentiationOff:
                        if (!containsNot)
                            Command = Command + "N";
                        break;
                }
            }
            return Command;
        }


        /// <summary>
        /// Address mapping 사용 여부.  default 는 true.  Unit Test 등에서 임시로 disable
        /// </summary>
        public static bool UseAddressMapping = true;

        public override string ToIL()
        {
            if (UseAddressMapping && Mnemonic != Mnemonic.RUNG_COMMENT)
            {
                var omron = _sourceILSentence as OmronILSentence;
                bool isOneShot =
                    omron != null 
                    && omron.Variation.IsOneOf(OmronILSentence.VariationType.DiffrentiationOn, OmronILSentence.VariationType.DiffrentiationOff)
                    && omron.Command.Contains("NOT")
                    ;


                var tempAddressAllocator = _rung2ILConvertor.TempAddressAllocator.Value;
                var rs = AddressConvertorInstance;

                Args = Args.Select((a, n) =>
                {
                    var arg = rs.IsMatch(a) ? rs.Convert(a) : a;

                    if (isOneShot) // ONS:
                    {
                        var diffrentiation = omron.Variation == OmronILSentence.VariationType.DiffrentiationOn ? '@' : '%';
                        var searchResult = tempAddressAllocator.Allocate("BIT", this, $"{diffrentiation}{arg}");
                        _rung2ILConvertor.ProglogRungs.AddRange(searchResult.PrologRungILs);
                        return searchResult.Temporary;
                    }

                    if (n == 1 && Mnemonic == Mnemonic.TON && omron != null)
                    {
                        var sa = omron.Args;
                        if (!sa[1].StartsWith("#"))
                        {
                            var searchResult = tempAddressAllocator.Allocate("TIMER_BUFFER", this, $"{sa[0]}");
                            _rung2ILConvertor.ProglogRungs.Add($"CMT\t{Cx2Xg5kOption.LabelHeader} 변환용 임시 버퍼");
                            _rung2ILConvertor.ProglogRungs.Add($"LOAD\t_ON");
                            _rung2ILConvertor.ProglogRungs.Add($"BIN\t{omron.Args[1]} {searchResult.Temporary}");

                            return searchResult.Temporary;
                        }
                    }
                    return arg;
                }).ToArray();

                var args = ModifyArguments();
                var operands = string.Join(" ", args);
                return $"{Command}\t{operands}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
            }
            else
            {
                var args = ModifyArguments();
                return $"{Command}\t{string.Join(" ", args)}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
            }
        }
    }

}
