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

            new MnemonicInput("TR 재사용 case",
                @"
                    LD P00000
                    OUT TR0
                    AND P00001
                    OUT P00100
                    LD TR0
                    AND P0000B
                    OUT TR0
                    AND P0000C
                    OUT P00105
                    LD TR0
                    AND P0000D
                    OUT P00106
                ",
                @"
                    LOAD P00000
                    MPUSH
                    AND P00001
                    OUT P00100
                    MPOP
                    AND P0000B
                    MPUSH
                    AND P0000C
                    OUT P00105
                    MPOP
                    AND P0000D
                    OUT P00106
                    "),
            new MnemonicInput("TR 재사용불가 case",
                @"
                    LD P00000
                    OUT TR0
                    AND P00001
                    OUT P00100
                    LD TR0
                    AND P0000B
                    OUT TR1
                    AND P0000C
                    OUT P00105
                    LD TR1
                    AND P0000D
                    OUT P00106
                    LD TR0
                    AND P0000E
                    OUT P00107
                ",
                @"
                    LOAD	P00000
                    MPUSH
                    AND	P00001
                    OUT	P00100
                    MLOAD
                    AND	P0000B
                    MPUSH
                    AND	P0000C
                    OUT	P00105
                    MPOP
                    AND	P0000D
                    OUT	P00106
                    MPOP
                    AND	P0000E
                    OUT	P00107
                    "),
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
        };
    }
}
