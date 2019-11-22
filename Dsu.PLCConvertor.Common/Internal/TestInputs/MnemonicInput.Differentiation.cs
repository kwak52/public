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
        internal static MnemonicInput[] InputsDifferentiation = new MnemonicInput[]
        {
            new MnemonicInput("Checking Differentiation : ON",
                @"
                    LD P_First_Cycle
                    @OUT ARG0
                ",
                @"
                    LOAD P_First_Cycle
                    OUTP ARG0
                "),

            new MnemonicInput("Checking Differentiation : OFF",
                @"
                    LD P_First_Cycle
                    %OUT ARG0
                ",
                @"
                    LOAD P_First_Cycle
                    OUTN ARG0
                "),

            new MnemonicInput("Checking Differentiation : ON",
                @"
                    @LD P_First_Cycle
                    @OUT ARG0
                ",
                @"
                    LOADP P_First_Cycle
                    OUTP ARG0
                "),

            new MnemonicInput("Checking Differentiation : OFF",
                @"
                    %LD P_First_Cycle
                    %OUT ARG0
                ",
                @"
                    LOADN P_First_Cycle
                    OUTN ARG0
                "),


            new MnemonicInput("Checking Differentiation : ON",
                @"
                    @LD P_First_Cycle
                    @AND ARG2
                    @OUT ARG0
                ",
                @"
                    LOADP P_First_Cycle
                    ANDP ARG2
                    OUTP ARG0
                "),

            new MnemonicInput("Checking Differentiation : OFF",
                @"
                    %LD P_First_Cycle
                    %AND ARG2
                    %OUT ARG0
                ",
                @"
                    LOADN P_First_Cycle
                    ANDN ARG2
                    OUTN ARG0
                "),


            // Full blown 에서만 테스트 가능함.  UnitTest 에서는 TemporaryAddressAllocator 가 할당되지 않아서 결과가 나오지 않음.
            /*
            new MnemonicInput("Checking @ANDNOT",
                @"
                    @LD ARG0
                    @ANDNOT ARG2
                    OUT ARG2
                ",
                @"
                    LOADP ARG0
                    OUT TEMP
                    LOAD NOT TEMP
                    OUT ARG2
                "),
            */
        };
    }
}
