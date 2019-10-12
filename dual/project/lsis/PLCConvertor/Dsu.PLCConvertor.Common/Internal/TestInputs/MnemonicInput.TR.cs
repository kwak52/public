using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public partial class MnemonicInput
    {
        internal static MnemonicInput[] InputsTR = new MnemonicInput[]
        {
            new MnemonicInput("TR Simple",
                @"
                    LD 0.01
                    OR 0.06
                    OUT TR0
                    AND 0.07
                    OUT 2.00
                    LD TR0
                    OUT 2.01
                ",
                @"
                    LOAD 0.01
                    OR 0.06
                    MPUSH
                    AND 0.07
                    OUT 2.00
                    MPOP
                    OUT 2.01
                "),
            new MnemonicInput("TR Basic2",
                @"
                    LD 0.00
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
                @"
                    LOAD 0.00
                    MPUSH
                    AND 0.01
                    OUT 110.00
                    MLOAD
                    AND 110.00
                    OUT 103.00
                    MPOP//2
                    AND 111.00
                    AND 112.00
                    OUT 102.10
                "),
            new MnemonicInput("TR Basic4",
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
                    LOAD 0.01
                    LOAD 0.02
                    AND 0.03
                    OR LOAD
                    LOAD 0.04
                    OR 0.05
                    AND LOAD
                    OR 0.06
                    MPUSH
                    AND 0.07
                    OUT 2.00
                    MPOP
                    OUT 2.01
                    "
                ),
            new MnemonicInput("TR Buggy123",
                @"
                    LD 0.01
                    LD 0.04
                    AND 0.03
                    ORLD
                    OUT TR0
                    AND 0.02
                    OUT 1.00
                    LD TR0
                    OUT 1.01
                    AND 0.06
                    OUT 1.02
                    AND 0.08
                    OUT 1.03
                ",
                @"
                    LOAD 0.01
                    LOAD 0.04
                    AND 0.03
                    OR LOAD
                    MPUSH
                    AND 0.02
                    OUT 1.00
                    MLOAD
                    OUT 1.01
                    MPOP
                    AND 0.06
                    OUT 1.02
                    AND 0.08
                    OUT 1.03
                "),
        };
    }
}
