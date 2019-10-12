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
            new MnemonicInput("TR Complex2",
                @"
                    LD 0.1
                    OR 0.2
                    OR 0.3
                    OR 0.4
                    LD 0.6
                    OR 0.7
                    ANDLD
                    OR 0.5
                    LD 0.8
                    OR 0.9
                    OR 0.10
                    OR 0.11
                    ANDLD
                    OUT TR0
                    LD 0.12
                    OR 0.13
                    LD 0.18
                    OR 0.19
                    OR 0.20
                    ANDLD
                    OR 0.14
                    ANDLD
                    OUT TR1
                    AND 0.24
                    OUT 1.1
                    LD TR1
                    LD 0.25
                    OR 0.25
                    OR 0.27
                    ANDLD
                    OUT 1.2
                    OUT 1.3
                    OUT 1.4
                    LD TR0
                    LD 0.15
                    OR 0.16
                    ANDLD
                    LD 0.21
                    OR 0.22
                    OR 0.23
                    ANDLD
                    OUT 1.5
                    OUT 1.6
                    OUT 1.7
                    LD TR0
                    OUT 1.8
                    AND 0.17
                    OUT 1.9
                    OUT 1.10

                "),
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
        };
    }
}
