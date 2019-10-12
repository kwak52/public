using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public partial class MnemonicInput
    {
        internal static MnemonicInput[] InputsComplex = new MnemonicInput[]
        {

        new MnemonicInput("Complex 1",
            @"
                LD 0.00
                AND 0.07
                AND 0.10
                AND 0.15
                LD 0.01
                LD 0.00
                AND 0.02
                ORLD
                OR 0.03
                LD 0.04
                OR 0.05
                LD 0.07
                OR 0.08
                ANDLD
                ORLD
                OR 0.06
                AND 0.08
                LD 0.11
                OR 0.12
                OR 0.13
                OR 0.14
                ANDLD
                AND 1.00
                LD 0.01
                OR 0.02
                OR 0.03
                OR 0.04
                ANDLD
                ORLD
                LD 0.09
                LD 0.05
                OR 0.06
                ANDLD
                ORLD
                OUT 99.00
                "),
        };
    }
}
