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
