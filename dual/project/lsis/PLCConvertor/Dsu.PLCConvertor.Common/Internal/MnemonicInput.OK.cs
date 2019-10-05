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
