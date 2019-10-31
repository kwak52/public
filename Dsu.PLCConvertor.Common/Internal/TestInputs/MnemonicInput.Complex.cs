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
            new MnemonicInput("TR Complex2",
                @"
                    LD	P00000
                    OR	P00001
                    OR	P00002
                    OR	P00003
                    OR	P00004
                    OR	P00005
                    OR	P00006
                    OUT	TR0
                    LD	P00007
                    OR	P00008
                    LD	P0000B
                    OR	P0000C
                    OR	P0000D
                    OR	P0000E
                    OR	P0000F
                    ANDLD	
                    OR	P00009
                    ANDLD	
                    OUT	TR1
                    LD	P00010
                    OR	P00011
                    OR	P00012
                    ANDLD	
                    OUT	TR2
                    LD	P00015
                    OR	P00016
                    OR	P00017
                    OR	P00018
                    OR	P00019
                    ANDLD	
                    OUT	TR3
                    AND	P0001A
                    OUT	TR4
                    AND	P0001C
                    LD	P0001E
                    OR	P0001F
                    OR	P00020
                    OR	P00021
                    ANDLD	
                    OUT	TR5
                    OUT	P00100
                    AND	P00022
                    OUT	P00101
                    LD	TR5
                    AND	P00023
                    OUT	P00102
                    LD	TR5
                    OUT	P00103
                    AND	P00024
                    OUT	P00104
                    LD	TR4
                    AND	P0001D
                    OUT	P00105
                    LD	TR3
                    AND	P0001B
                    OUT	P00106
                    LD	TR2
                    OUT	P00107
                    LD	TR1
                    AND	P00013
                    OUT	P00108
                    LD	TR1
                    AND	P00014
                    OUT	P00109
                    LD	TR0
                    AND	P0000A
                    OUT	P00110
                ",
                @"
                    LOAD	P00000
                    OR	P00001
                    OR	P00002
                    OR	P00003
                    OR	P00004
                    OR	P00005
                    OR	P00006
                    MPUSH	
                    LOAD	P00007
                    OR	P00008
                    LOAD	P0000B
                    OR	P0000C
                    OR	P0000D
                    OR	P0000E
                    OR	P0000F
                    AND LOAD	
                    OR	P00009
                    AND LOAD	
                    MPUSH	
                    LOAD	P00010
                    OR	P00011
                    OR	P00012
                    AND LOAD	
                    MPUSH	
                    LOAD	P00015
                    OR	P00016
                    OR	P00017
                    OR	P00018
                    OR	P00019
                    AND LOAD	
                    MPUSH	
                    AND	P0001A
                    MPUSH	
                    AND	P0001C
                    LOAD	P0001E
                    OR	P0001F
                    OR	P00020
                    OR	P00021
                    AND LOAD	
                    MPUSH	
                    OUT	P00100
                    AND	P00022
                    OUT	P00101
                    MLOAD	
                    AND	P00023
                    OUT	P00102
                    MLOAD	
                    OUT	P00103
                    MPOP	
                    AND	P00024
                    OUT	P00104
                    MPOP	
                    AND	P0001D
                    OUT	P00105
                    MPOP	
                    AND	P0001B
                    OUT	P00106
                    MPOP	
                    OUT	P00107
                    MLOAD	
                    AND	P00013
                    OUT	P00108
                    MPOP	
                    AND	P00014
                    OUT	P00109
                    MPOP	
                    AND	P0000A
                    OUT	P00110
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
