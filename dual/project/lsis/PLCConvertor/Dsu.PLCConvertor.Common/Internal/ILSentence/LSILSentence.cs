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
        public override string ToString()
        {
            if (UseAddressMapping && Mnemonic != Mnemonic.RUNG_COMMENT)
            {
                var omron = _sourceILSentence as OmronILSentence;
                bool isOneShot =
                    omron != null 
                    && omron.Variation.IsOneOf(OmronILSentence.VariationType.DiffrentiationOn, OmronILSentence.VariationType.DiffrentiationOff);


                var rs = AddressConvertorInstance;
                var args = Args.Select(arg =>
                {
                    if (isOneShot) // ONS:
                    {
                        var diffrentiation = omron.Variation == OmronILSentence.VariationType.DiffrentiationOn ? '@' : '%';
                        var searchResult = _rung2ILConvertor.TempAddressAllocator.Allocate(this, $"{diffrentiation}{arg}");
                        _rung2ILConvertor.ProglogRungs.AddRange(searchResult.PrologRungILs);
                        return searchResult.Temporary;
                    }
                    else
                    {
                        if (rs.IsMatch(arg))
                            return rs.Convert(arg);

                        return arg;
                    }
                });
                var operands = string.Join(" ", args);
                return $"{Command}\t{operands}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
            }
            else
                return $"{Command}\t{string.Join(" ", Args)}".TrimEnd(new[] { ' ', '\t', '\r', '\n' });
        }

        public static LSILSentence Create(string sentence)
        {
            Debugger.Break();

            var ils = new LSILSentence();
            ils.Fill(sentence);
            return ils;
        }

    }

}
