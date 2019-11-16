using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public partial class MnemonicInput
    {
        internal static MnemonicInput[] InputsNG = new MnemonicInput[]
        {
            new MnemonicInput("산전 변환 불가 case: TR Basic3",
                @"
                    LD 0.00
                    LD 0.01
                    OUT TR0
                    AND 0.02
                    ORLD
                    AND 0.03
                    OUT 102.11
                    LD TR0
                    AND 0.04
                    OUT 102.12
                "),
            new MnemonicInput("산전 변환 불가 case: TR Basic4",
                @"
                    LD 0.00
                    LD 0.01
                    OUT TR0
                    AND 0.02
                    ORLD
                    AND 0.03
                    OUT 102.11
                    LD TR0
                    AND 0.04
                    OUT 102.12
                "),

            new MnemonicInput("산전 변환 특수 case: TR with ORLD",
                @"
                    LD 0.01
                    OUT TR0
                    AND 0.02
                    OUT 0.01
                    LD TR0
                    AND 0.03
                    LD TR0
                    UP(521)
                    ORLD
                    OUT 0.02
                "),


        };
    }
}
