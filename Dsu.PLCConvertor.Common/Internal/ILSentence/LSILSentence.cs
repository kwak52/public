using Dsu.PLCConvertor.Common.Internal;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    public class LSILSentence : ILSentence
    {
        PLCVendor _vendorType = PLCVendor.LSIS;
        public LSILSentence(ILSentence other)
            : base(other)
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

            var omron = other as OmronILSentence;
            if (omron != null)
            {
                switch (omron.Variation)
                {
                    case OmronILSentence.VariationType.DiffrentiationOn:
                        Command = Command + "P";
                        break;
                    case OmronILSentence.VariationType.DiffrentiationOff:
                        Command = Command + "N";
                        break;
                }
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
                var args = Args.Select(arg =>
                {
                    var targetDevice = arg;
                    if (rs.IsMatch(arg))
                    {
                        targetDevice = rs.Convert(arg);
                    }

                    return targetDevice;
                });
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

}
