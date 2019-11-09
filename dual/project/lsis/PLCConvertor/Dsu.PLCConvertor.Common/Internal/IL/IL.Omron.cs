using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    public partial class IL
    {
        /// <summary>
        /// 옴론 CX-One 을 위한 { mnemonic -> 문자 mnemonic } 참조용 dictionary
        /// </summary>
        static Dictionary<Mnemonic, List<ILCommand>> _dicOmron = new Dictionary<Mnemonic, List<ILCommand>>()
        {
            [Mnemonic.LOAD] = _toList(1, "LD"),
            [Mnemonic.LOADNOT] = _toList(1, "LDNOT"),
            [Mnemonic.AND] = _toList(1, "AND"),
            [Mnemonic.ANDNOT] = _toList(1, "ANDNOT"),
            [Mnemonic.ANDLD] = _toList(1, "ANDLD"),
            [Mnemonic.OR] = _toList(1, "OR"),
            [Mnemonic.ORNOT] = _toList(1, "ORNOT"),
            [Mnemonic.ORLD] = _toList(1, "ORLD"),
            [Mnemonic.OUT] = _toList(1, "OUT"),

            // 옴론은 MPush/MLoad/MPop 을 이용하지 않음
            [Mnemonic.MPUSH] = _toList(1, "--ERROR:MPUSH"),
            [Mnemonic.MLOAD] = _toList(1, "--ERROR:MLOAD"),
            [Mnemonic.MPOP] = _toList(1, "--ERROR:MPOP"),

            [Mnemonic.CTU] = _toList(2, "CNT"),
            [Mnemonic.TON] = _toList(1, "TIM"),
            [Mnemonic.TMR] = _toList(2, "TTIM(087)"),
            [Mnemonic.KEEP] = _toList(2, "KEEP(011)"),
            [Mnemonic.SFT] = _toList(3, "SFT(010)"),
            [Mnemonic.CMP] = _toList(1, "CMP(020)"),
            [Mnemonic.ADD] = _toList(1, "+(400)"),
            [Mnemonic.SUB] = _toList(1, "-(410)"),
            [Mnemonic.MUL] = _toList(1, "*(420)"),
            [Mnemonic.DIV] = _toList(1, "/(430)"),

            [Mnemonic.BCD] = _toList(1, "BCD(024)"),
            [Mnemonic.BIN] = _toList(1, "BIN(023)"),

            [Mnemonic.CPS] = _toList(1, "CPS(114)"),
            [Mnemonic.ANDEQ] = _toList(1, "AND=(300)"),
            [Mnemonic.OREQ] = _toList(1, "OR=(300)"),
            [Mnemonic.ANDLESSTHAN] = _toList(1, "AND<(310)"),
            [Mnemonic.ANDGREATERTHAN] = _toList(1, "AND>(320)"),


            [Mnemonic.MOVE] = _toList(1, "MOV(021)"),
            [Mnemonic.RSTB] = _toList(1, "RSTB(533)"),

            [Mnemonic.CONCATSTRING] = _toList(1, "+$(656)"),

            [Mnemonic.NOP] = _toList(1, "NOP(000)"),
            [Mnemonic.END] = _toList(1, "END(001)"),

            [Mnemonic.RUNG_COMMENT] = _toList(0, "'"),

        };
    }
}