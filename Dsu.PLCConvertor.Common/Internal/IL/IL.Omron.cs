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
            [Mnemonic.LOAD] = _n(1, "LD"),
            [Mnemonic.LOADP] = _n(1, "@LD"),
            [Mnemonic.LOADN] = _n(1, "%LD"),
            [Mnemonic.LOADNOT] = _n(1, "LDNOT"),
            [Mnemonic.LOADEQ] = _n(1, "LD=(300)"),
            [Mnemonic.AND] = _n(1, "AND"),
            [Mnemonic.ANDP] = _n(1, "@AND"),
            [Mnemonic.ANDN] = _n(1, "%AND"),
            [Mnemonic.ANDNOT] = _n(1, "ANDNOT"),
            [Mnemonic.ANDLD] = _n(1, "ANDLD"),
            [Mnemonic.OR] = _n(1, "OR"),
            [Mnemonic.ORP] = _n(1, "@OR"),
            [Mnemonic.ORN] = _n(1, "%OR"),
            [Mnemonic.ORNOT] = _n(1, "ORNOT"),
            [Mnemonic.ORLD] = _n(1, "ORLD"),
            [Mnemonic.OUT] = _t(1, "OUT"),
            [Mnemonic.OUTNOT] = _t(1, "OUTNOT"),

            // 옴론은 MPush/MLoad/MPop 을 이용하지 않음
            [Mnemonic.MPUSH] = _t(1, "--ERROR:MPUSH"),
            [Mnemonic.MLOAD] = _t(1, "--ERROR:MLOAD"),
            [Mnemonic.MPOP] = _t(1, "--ERROR:MPOP"),

            [Mnemonic.CTU] = _t(2, "CNT"),
            [Mnemonic.TON] = _t(1, "TIM"),
            [Mnemonic.TMR] = _t(2, "TTIM(87)"),
            [Mnemonic.KEEP] = _n(2, "KEEP(11)"),
            [Mnemonic.SFT] = _t(3, "SFT(10)"),
            [Mnemonic.CMP] = _t(1, "CMP(20)"),
            [Mnemonic.ADD] = _n(1, "+(400)"),
            [Mnemonic.SUB] = _n(1, "-(410)"),
            [Mnemonic.MUL] = _n(1, "*(420)"),
            [Mnemonic.DIV] = _n(1, "/(430)"),

            [Mnemonic.BCD] = _t(1, "BCD(24)"),
            [Mnemonic.BIN] = _t(1, "BIN(23)"),

            //[Mnemonic.CPS] = _t(1, "CPS(114)"),
            [Mnemonic.ANDEQ] = _n(1, "AND=(300)"),
            [Mnemonic.OREQ] = _n(1, "OR=(300)"),
            [Mnemonic.ANDLESSTHAN] = _n(1, "AND<(310)"),
            [Mnemonic.ANDGREATERTHAN] = _n(1, "AND>(320)"),


            [Mnemonic.MOVE] = _t(1, "MOV(21)"),
            [Mnemonic.RSTB] = _t(1, "RSTB(533)"),
            [Mnemonic.BSET] = _t(1, "BSET(71)"),

            [Mnemonic.CONCATSTRING] = _n(1, "+$(656)"),

            [Mnemonic.NOP] = _t(1, "NOP(0)"),
            [Mnemonic.END] = _t(1, "END(1)"),

            [Mnemonic.RUNG_COMMENT] = _t(0, "'"),

        };
    }
}