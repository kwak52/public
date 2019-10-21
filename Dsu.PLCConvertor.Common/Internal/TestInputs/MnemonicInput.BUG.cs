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
        internal static MnemonicInput[] InputsBUG = new MnemonicInput[]
        {
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
        };
    }
}
