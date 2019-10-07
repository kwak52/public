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
            new MnemonicInput("Simplest:2x1",
@"LD 0.00
OR 0.01
OUT 1.00
"),
            new MnemonicInput("Simplest:1x2",
@"LD 0.00
OUT 1.00
OUT 1.05
"),
            new MnemonicInput("Simplest2: 2x1",
@"LD 0.00
LD 0.01
AND 0.02
ORLD
OUT 1.00
"),
        new MnemonicInput("TR Basic3",
            @"LD 0.00
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

            new MnemonicInput("1 x 2 x 1",
@"LD 0.00
LD 0.01
OR 0.02
ANDLD
OUT 1.02
",
@"LD 0.00
LD 0.01
LD 0.02
ORLD
ANDLD
OUT 1.02"),

            new MnemonicInput("(2 x 2 + 1) x 1",
@"LD 0.01
OR 0.02
LD 0.03
OR 0.04
ANDLD
OR 0.05
OUT 1.01
",
@"LD 0.01
LD 0.02
ORLD
LD 0.03
LD 0.04
ORLD
ANDLD
LD 0.05
ORLD
OUT 1.01
"),

            new MnemonicInput("2 x 2 x 1 x 2",
@"LD 0.01
LD 0.02
AND 0.03
ORLD
LD 0.04
OR 0.05
ANDLD
OR 0.06
OUT 2.00
OUT 2.01
"),

        new MnemonicInput("TR Basic2",
            @"LD 0.00
OUT TR0
AND 0.01
OUT 110.00
LD TR0
AND 110.00
OUT 103.00
LD TR0
AND 111.00
AND 112.00
OUT 102.10
",
            @"LD 0.00
    MPUSH
    AND 0.01
    OUT 110.00
    MREAD
    AND 110.00
    OUT 103.00
    MPOP//2
    AND 111.00
    And 112.0
    OUT 102.10
"),

        new MnemonicInput("TR Simple",
            @"LD 0.01
OR 0.06
OUT TR0
AND 0.07
OUT 2.00
LD TR0
OUT 2.01",
            @"LD 0.01
OR 0.06
MPUSH
AND 0.07
OUT 2.00
MPOP
OUT 2.01"),
        new MnemonicInput("산전 변환 불가 case: TR Basic4",
            @"LD 0.00
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

        new MnemonicInput("버그 OR 0.06 case: TR Basic4",
            @"
LD 0.01
LD 0.02
AND 0.03
ORLD
LD 0.04
OR 0.05
ANDLD
OR 0.06
OUT TR0
AND 0.07
OUT 2.00
LD TR0
OUT 2.01
",
            @"
LD 0.01
LD 0.02
AND 0.03
ORLD
LD 0.04
OR 0.05
ANDLD
OR 0.06
MPUSH
AND 0.07
OUT 2.00
MPOP
OUT 2.01
"

            ),
        };
    }
}
