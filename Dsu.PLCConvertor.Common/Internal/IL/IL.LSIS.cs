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
            [Mnemonic.LOAD] = _toInner(1, "LOAD"),
            [Mnemonic.LOAD] = _toInner(1, "LOAD"),
            [Mnemonic.LOADNOT] = _toInner(1, "LOAD NOT"),
            [Mnemonic.AND] = _toInner(1, "AND"),
            [Mnemonic.ANDNOT] = _toInner(1, "AND NOT"),
            [Mnemonic.ANDLD] = _toInner(1, "AND LOAD"),
            [Mnemonic.OR] = _toInner(1, "OR"),
            [Mnemonic.ORNOT] = _toInner(1, "OR NOT"),
            [Mnemonic.ORLD] = _toInner(1, "OR LOAD"),
            [Mnemonic.OUT] = _toTerminal(1, "OUT"),

            [Mnemonic.MPUSH] = _toTerminal(0, "MPUSH"),
            [Mnemonic.MLOAD] = _toTerminal(0, "MLOAD"),
            [Mnemonic.MPOP] = _toTerminal(0, "MPOP"),

            [Mnemonic.CTU] = _toTerminal(2, "CTU"),
            [Mnemonic.TON] = _toTerminal(1, "TON"),
            [Mnemonic.TMR] = _toTerminal(2, "TMR"),
            [Mnemonic.KEEP] = _toInner(2, "KEEP(011)"),
            [Mnemonic.SFT] = _toTerminal(3, "SFT"),
            [Mnemonic.CMP] = _toTerminal(1, "CMP"),
            [Mnemonic.ADD] = _toInner(1, "ADD"),
            [Mnemonic.SUB] = _toInner(1, "SUB"),
            [Mnemonic.MUL] = _toInner(1, "MUL"),
            [Mnemonic.DIV] = _toInner(1, "DIV"),

            [Mnemonic.BCD] = _toTerminal(1, "BCD"),
            [Mnemonic.BIN] = _toTerminal(1, "BIN"),

            [Mnemonic.CPS] = _toInner(1, "CMP"),
            [Mnemonic.ANDEQ] = _toInner(1, "AND="),
            [Mnemonic.OREQ] = _toInner(1, "OR="),
            [Mnemonic.ANDLESSTHAN] = _toInner(1, "AND<"),
            [Mnemonic.ANDGREATERTHAN] = _toInner(1, "AND>"),


            [Mnemonic.MOVE] = _toTerminal(1, "MOV"),
            [Mnemonic.RSTB] = _toTerminal(1, "BRST"),

            [Mnemonic.CONCATSTRING] = _toInner(1, "+$(656)"),

            [Mnemonic.NOP] = _toTerminal(1, "NOP"),
            [Mnemonic.END] = _toTerminal(1, "END"),

            [Mnemonic.RUNG_COMMENT] = _toTerminal(0, Xg5k.RungCommentCommand),
        };

    }
}