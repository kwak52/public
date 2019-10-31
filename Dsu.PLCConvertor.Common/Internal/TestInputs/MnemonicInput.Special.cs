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
        internal static MnemonicInput[] InputsSpecial = new MnemonicInput[]
        {
            new MnemonicInput("Special output case : Arity=1",
                @"
                    LD 0.00
                    TIM 0000 0
                ",
                @"
                    LOAD 0.00
                    TON	T0 0
                "),

            new MnemonicInput("Special output case : Arity=1",
                @"
                    LD 0.00
                    LD 0.01
                    OR 0.02
                    ANDLD
                    TIM 0000 0
                "),

            new MnemonicInput("Special output case : Arity=2",
                @"
                    LD 0.05
                    LD 0.06
                    TTIM(087) 0001 10
                ",
                @"
                    LOAD P00005
                    TMR	T1 10
                    LOAD P00006
                    RST T1
                "),

            new MnemonicInput("Special output case : Arity=2",
                @"
                    LD 0.01
                    AND 0.01
                    LD 0.02
                    LD 0.04
                    OR 0.03
                    ANDLD
                    TTIM(087) 0001 1
                "),

            new MnemonicInput("Special output case : Arity=2",
                @"
                    LD 0.01
                    AND 0.03
                    LD 0.02
                    OR 0.05
                    TTIM(087) 0001 1
                ",
                @"
                    LOAD P00001
                    AND	P00003
                    TMR	T0 1
                    LOAD	P00002
                    OR	P00005
                    RST	T0000
                "),

            new MnemonicInput("Special output case : Arity=3",
                @"
                    LD 0.04
                    LD 0.01
                    AND 0.01
                    LD 0.02
                    LD 0.04
                    OR 0.03
                    ANDLD
                    SFT(010) 10 20
                "),

            new MnemonicInput("Special output case(Keep) : Arity=2",
                @"
                    LD 0.01
                    AND 0.01
                    LD 0.02
                    LD 0.04
                    OR 0.03
                    ANDLD
                    KEEP(011) C
                "),

        };
    }
}
