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
        /// LSIS Xg5000 을 위한 { mnemonic -> 문자 mnemonic } 참조용 dictionary
        /// </summary>
        static Dictionary<Mnemonic, List<ILCommand>> _dicLSIS = new Dictionary<Mnemonic, List<ILCommand>>()
        {
            [Mnemonic.LOAD] = _n(1, "LOAD"),
            [Mnemonic.LOADP] = _n(1, "LOADP"),
            [Mnemonic.LOADN] = _n(1, "LOADN"),
            [Mnemonic.LOADNOT] = _n(1, "LOAD NOT"),
            [Mnemonic.LOADEQ] = _n(1, "LOAD="),
            [Mnemonic.AND] = _n(1, "AND"),
            [Mnemonic.ANDP] = _n(1, "ANDP"),
            [Mnemonic.ANDN] = _n(1, "ANDN"),
            [Mnemonic.ANDNOT] = _n(1, "AND NOT"),
            [Mnemonic.ANDLD] = _n(1, "AND LOAD"),
            [Mnemonic.OR] = _n(1, "OR"),
            [Mnemonic.ORP] = _n(1, "ORP"),
            [Mnemonic.ORN] = _n(1, "ORN"),
            [Mnemonic.ORNOT] = _n(1, "OR NOT"),
            [Mnemonic.ORLD] = _n(1, "OR LOAD"),
            [Mnemonic.OUT] = _t(1, "OUT"),
            [Mnemonic.OUTNOT] = _t(1, "OUT NOT"),

            [Mnemonic.MPUSH] = _t(0, "MPUSH"),
            [Mnemonic.MLOAD] = _t(0, "MLOAD"),
            [Mnemonic.MPOP] = _t(0, "MPOP"),

            [Mnemonic.CTU] = _t(2, "CTU"),
            [Mnemonic.TON] = _t(1, "TON"),
            [Mnemonic.TMR] = _t(2, "TMR"),
            [Mnemonic.KEEP] = _n(2, "KEEP(011)"),
            [Mnemonic.SFT] = _t(3, "SFT"),
            [Mnemonic.CMP] = _t(1, "CMP"),
            [Mnemonic.ADD] = _n(1, "ADD"),
            [Mnemonic.SUB] = _n(1, "SUB"),
            [Mnemonic.MUL] = _n(1, "MUL"),
            [Mnemonic.DIV] = _n(1, "DIV"),

            [Mnemonic.BCD] = _t(1, "BCD"),
            [Mnemonic.BIN] = _t(1, "BIN"),

            [Mnemonic.CPS] = _n(1, "CMP"),
            [Mnemonic.ANDEQ] = _n(1, "AND="),
            [Mnemonic.OREQ] = _n(1, "OR="),
            [Mnemonic.ANDLESSTHAN] = _n(1, "AND<"),
            [Mnemonic.ANDGREATERTHAN] = _n(1, "AND>"),


            [Mnemonic.MOVE] = _t(1, "MOV"),
            [Mnemonic.RSTB] = _t(1, "BRST"),

            [Mnemonic.CONCATSTRING] = _n(1, "+$(656)"),

            [Mnemonic.NOP] = _t(1, "NOP"),
            [Mnemonic.END] = _t(1, "END"),

            [Mnemonic.RUNG_COMMENT] = _t(0, Xg5k.RungCommentCommand),
        };

    }
}