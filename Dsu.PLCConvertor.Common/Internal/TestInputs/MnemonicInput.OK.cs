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
            new MnemonicInput("Simplest:1x1",
                @"
                    LD 0.00
                    OUT 1.00
                "),

            new MnemonicInput("Basic",
                @"
                    LD A // Load
                    AND B
                    OR C
                    OUT D
                "),

            new MnemonicInput("Basic2",
                @"
                    LD A
                    AND B
                    LD C
                    ORLD
                    OUT D
                "),

            new MnemonicInput("Basic3",
                @"
                    LD A
                    OR B
                    AND C
                    OUT D
                "),

            new MnemonicInput("TR Basic1",
                @"
                    LD A
                    OUT TR0
                    AND B
                    OUT O1
                    LD TR0
                    AND C
                    OUT O2
                ",
                @"
                    LD A
                    MPUSH
                    AND B
                    OUT O1
                    MPOP
                    AND C
                    OUT O2
                    "),

            new MnemonicInput("Simplest:2x1",
                @"
                    LD 0.00
                    OR 0.01
                    OUT 1.00
                "),
            new MnemonicInput("Simplest:1x2",
                @"
                    LD 0.00
                    OUT 1.00
                    OUT 1.05
                "),
            new MnemonicInput("Simplest2: 2x1",
                @"
                    LD 0.00
                    LD 0.01
                    AND 0.02
                    ORLD
                    OUT 1.00
                "),

            new MnemonicInput("1 x 2 x 1",
                @"
                    LD 0.00
                    LD 0.01
                    OR 0.02
                    ANDLD
                    OUT 1.02
                "),

            new MnemonicInput("(2 x 2 + 1) x 1",
                @"
                    LD 0.01
                    OR 0.02
                    LD 0.03
                    OR 0.04
                    ANDLD
                    OR 0.05
                    OUT 1.01
                "),

            new MnemonicInput("(2 x 2 + 2) x 1",
                @"
                    LD 0.01
                    OR 0.02
                    LD 0.03
                    OR 0.04
                    ANDLD
                    LD 0.05
                    AND 0.06
                    ORLD
                    OUT 1.01
                "),
            new MnemonicInput("2 x 2 x 1 x 2",
                @"
                    LD 0.01
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

            new MnemonicInput("Branching Outputs",
                @"
                    LD 0.00
                    OUT 1.01
                    AND 0.02
                    OUT 1.02
                    LD 0.03
                    OR 0.04
                    ANDLD
                    OUT 1.03
                "),


            new MnemonicInput("Medium",
                @"
                    LD 0.00
                    LD 0.01
                    LD 0.02
                    OR 0.03
                    AND 0.04
                    ORLD
                    ANDLD
                    LD 0.05
                    LD 0.06
                    AND 0.07
                    AND 0.08
                    LD 0.09
                    AND 0.10
                    ORLD
                    ANDLD
                    ORLD
                    AND 0.11
                    AND 0.12
                    OUT 1.00
                "),
            new MnemonicInput("Solved: FixMe, First!!",
                @"
                    LD 0.01
                    OR 0.06
                    LD 0.10
                    LD 0.11
                    OR 0.13
                    ANDLD
                    ORLD
                    OUT 1.00
                "),


            new MnemonicInput("Solved: FixMe",
                @"
                    LD 0.01
                    LD 0.02
                    AND 0.03
                    LD 0.04
                    AND 0.05
                    ORLD
                    ANDLD
                    LD 0.06
                    LD 0.07
                    AND 0.08
                    LD 0.09
                    AND 0.15
                    ORLD
                    ANDLD
                    ORLD
                    LD 0.10
                    LD 0.11
                    AND 0.12
                    LD 0.13
                    AND 0.14
                    ORLD
                    ANDLD
                    ORLD
                    OUT 1.00
                "),
            new MnemonicInput("Medium:1x1",
                @"
                    LD 0.01
                    LD 0.02
                    OR 0.03
                    LD 0.05
                    OR 0.06
                    ANDLD
                    AND 0.07
                    LD 0.04
                    LD 0.08
                    AND 0.10
                    OR 0.09
                    ANDLD
                    ORLD
                    ANDLD
                    LD 0.11
                    LD 0.12
                    OR 0.14
                    LD 0.13
                    AND 1.02
                    OR 1.00
                    ANDLD
                    LD 0.15
                    OR 1.04
                    AND 1.01
                    AND 1.03
                    ORLD
                    ANDLD
                    ORLD
                    OUT 2.01
                "),
        };
    }
}
