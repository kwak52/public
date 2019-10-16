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
            new MnemonicInput("TR Complex 1",
                @"
                    LD P000
                    OR P001
                    OR P002
                    OR P003
                    LD P005
                    OR P006
                    ANDLD
                    OR P004
                    LD P007
                    OR P008
                    OR P009
                    OR P00A
                    ANDLD
                    OUT TR0
                    LD P00B
                    OR P00C
                    LD P011
                    OR P012
                    OR P013
                    ANDLD
                    OR P00D
                    ANDLD
                    OUT TR1
                    AND P017
                    OUT P100
                    LD TR1
                    LD P018
                    OR P019
                    OR P01A
                    ANDLD
                    OUT P101
                    OUT P102
                    OUT P103
                    LD TR0
                    LD P00E
                    OR P00F
                    ANDLD
                    LD P014
                    OR P015
                    OR P016
                    ANDLD
                    OUT P104
                    OUT P105
                    OUT P106
                    LD TR0
                    OUT P107
                    AND P010
                    OUT P108
                    OUT P109
                    ",
                @"
                    LOAD	P000
                    OR	P001
                    OR	P002
                    OR	P003
                    LOAD	P005
                    OR	P006
                    AND LOAD	
                    OR	P004
                    LOAD	P007
                    OR	P008
                    OR	P009
                    OR	P00A
                    AND LOAD	
                    MPUSH	
                    LOAD	P00B
                    OR	P00C
                    LOAD	P011
                    OR	P012
                    OR	P013
                    AND LOAD	
                    OR	P00D
                    AND LOAD	
                    MPUSH	
                    AND	P017
                    OUT	P100
                    MPOP	
                    LOAD	P018
                    OR	P019
                    OR	P01A
                    AND LOAD	
                    OUT	P101
                    OUT	P102
                    OUT	P103
                    MLOAD	
                    LOAD	P00E
                    OR	P00F
                    AND LOAD	
                    LOAD	P014
                    OR	P015
                    OR	P016
                    AND LOAD	
                    OUT	P104
                    OUT	P105
                    OUT	P106
                    MLOAD	
                    OUT	P107
                    MPOP	
                    AND	P010
                    OUT	P108
                    OUT	P109
            "),

        new MnemonicInput("TR Complex 112",
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
