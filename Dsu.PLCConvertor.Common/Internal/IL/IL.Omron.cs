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
            [Mnemonic.LOAD] = _toInner(1, "LD"),
            [Mnemonic.LOADNOT] = _toInner(1, "LDNOT"),
            [Mnemonic.AND] = _toInner(1, "AND"),
            [Mnemonic.ANDNOT] = _toInner(1, "ANDNOT"),
            [Mnemonic.ANDLD] = _toInner(1, "ANDLD"),
            [Mnemonic.OR] = _toInner(1, "OR"),
            [Mnemonic.ORNOT] = _toInner(1, "ORNOT"),
            [Mnemonic.ORLD] = _toInner(1, "ORLD"),
            [Mnemonic.OUT] = _toTerminal(1, "OUT"),

            // 옴론은 MPush/MLoad/MPop 을 이용하지 않음
            [Mnemonic.MPUSH] = _toTerminal(1, "--ERROR:MPUSH"),
            [Mnemonic.MLOAD] = _toTerminal(1, "--ERROR:MLOAD"),
            [Mnemonic.MPOP] = _toTerminal(1, "--ERROR:MPOP"),

            [Mnemonic.CTU] = _toTerminal(2, "CNT"),
            [Mnemonic.TON] = _toTerminal(1, "TIM"),
            [Mnemonic.TMR] = _toTerminal(2, "TTIM(087)"),
            [Mnemonic.KEEP] = _toInner(2, "KEEP(011)"),
            [Mnemonic.SFT] = _toTerminal(3, "SFT(010)"),
            [Mnemonic.CMP] = _toTerminal(1, "CMP(020)"),
            [Mnemonic.ADD] = _toInner(1, "+(400)"),
            [Mnemonic.SUB] = _toInner(1, "-(410)"),
            [Mnemonic.MUL] = _toInner(1, "*(420)"),
            [Mnemonic.DIV] = _toInner(1, "/(430)"),

            [Mnemonic.BCD] = _toTerminal(1, "BCD(024)"),
            [Mnemonic.BIN] = _toTerminal(1, "BIN(023)"),

            [Mnemonic.CPS] = _toInner(1, "CPS(114)"),
            [Mnemonic.ANDEQ] = _toInner(1, "AND=(300)"),
            [Mnemonic.OREQ] = _toInner(1, "OR=(300)"),
            [Mnemonic.ANDLESSTHAN] = _toInner(1, "AND<(310)"),
            [Mnemonic.ANDGREATERTHAN] = _toInner(1, "AND>(320)"),


            [Mnemonic.MOVE] = _toTerminal(1, "MOV(021)"),
            [Mnemonic.RSTB] = _toTerminal(1, "RSTB(533)"),

            [Mnemonic.CONCATSTRING] = _toInner(1, "+$(656)"),

            [Mnemonic.NOP] = _toTerminal(1, "NOP(000)"),
            [Mnemonic.END] = _toTerminal(1, "END(001)"),

            [Mnemonic.RUNG_COMMENT] = _toTerminal(0, "'"),

        };
    }
}