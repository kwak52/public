using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public partial class MnemonicInput
    {
        /// <summary>
        /// 현재 bug 로 남아 있는 상태
        /// </summary>
        internal static MnemonicInput[] InputsConstants = new MnemonicInput[]
        {
            new MnemonicInput("Checking Constants : Decimal",
                @"
                    LD P_First_Cycle
                    MOV(021) &1000 A668
                ",
                @"
                    LOAD P_First_Cycle
                    MOV 1000 A668
                "),

            new MnemonicInput("Checking Constants : HexaDecimal",
                @"
                    LD P_First_Cycle
                    MOV(021) #F000 A668
                ",
                @"
                    LOAD P_First_Cycle
                    MOV HF000 A668
                "),

        };
    }
}
