using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public partial class MnemonicInput
    {
        internal static MnemonicInput[] InputsOK = new MnemonicInput[]
        {
            new MnemonicInput("Basic",
                @"LD A // Load
AND B
OR C
OUT D
",
                new string[] {
                    @"LD A
AND B
LD C
ORLD
OUT D",
                    @"LD C
LD A
AND B
ORLD
OUT D",
                }),

            new MnemonicInput("Basic2",
                @"LD A
AND B
LD C
ORLD
OUT D
",
                new string[] {
                    @"LD A
AND B
LD C
ORLD
OUT D",
                    @"LD C
LD A
AND B
ORLD
OUT D",
                }),

            new MnemonicInput("Basic3",
                @"LD A
LD B
ORLD
AND C
OUT D
",
                @"LD B
LD A
ORLD
AND C
OUT D
"                ),

            new MnemonicInput("TR Basic1",
                @"LD A
OUT TR0
AND B
OUT O1
LD TR0
AND C
OUT O2
",
                @"LD A
MPUSH
AND B
OUT O1
MPOP
AND C
OUT O2
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
",
@"LD 0.02
AND 0.03
LD 0.01
ORLD
LD 0.04
LD 0.05
ORLD
ANDLD
LD 0.06
ORLD
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
OUT 102.10
",
            @"LD 0.00
    MPUSH
    AND 0.01
    OUT 110.00
    MPOP//2
    AND 110.00
    OUT 102.10
"),
        };       
    }
}
