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
            [Mnemonic.LOAD] = _toList(1, "LOAD"),
            [Mnemonic.LOAD] = _toList(1, "LOAD"),
            [Mnemonic.LOADNOT] = _toList(1, "LOAD NOT"),
            [Mnemonic.AND] = _toList(1, "AND"),
            [Mnemonic.ANDNOT] = _toList(1, "AND NOT"),
            [Mnemonic.ANDLD] = _toList(1, "AND LOAD"),
            [Mnemonic.OR] = _toList(1, "OR"),
            [Mnemonic.ORNOT] = _toList(1, "OR NOT"),
            [Mnemonic.ORLD] = _toList(1, "OR LOAD"),
            [Mnemonic.OUT] = _toList(1, "OUT"),

            [Mnemonic.MPUSH] = _toList(0, "MPUSH"),
            [Mnemonic.MLOAD] = _toList(0, "MLOAD"),
            [Mnemonic.MPOP] = _toList(0, "MPOP"),

            [Mnemonic.CTU] = _toList(2, "CTU"),
            [Mnemonic.TON] = _toList(1, "TON"),
            [Mnemonic.TMR] = _toList(2, "TMR"),
            [Mnemonic.KEEP] = _toList(2, "XXXKEEP(011)"),
            [Mnemonic.SFT] = _toList(3, "SFT"),
            [Mnemonic.CMP] = _toList(1, "CMP"),
            [Mnemonic.ADD] = _toList(1, "ADD"),
            [Mnemonic.SUB] = _toList(1, "SUB"),
            [Mnemonic.MUL] = _toList(1, "MUL"),
            [Mnemonic.DIV] = _toList(1, "DIV"),

            [Mnemonic.BCD] = _toList(1, "BCD"),
            [Mnemonic.BIN] = _toList(1, "BIN"),

            [Mnemonic.CPS] = _toList(1, "CMP"),
            [Mnemonic.ANDEQ] = _toList(1, "AND="),
            [Mnemonic.OREQ] = _toList(1, "OR="),
            [Mnemonic.ANDLESSTHAN] = _toList(1, "AND<"),
            [Mnemonic.ANDGREATERTHAN] = _toList(1, "AND>"),


            [Mnemonic.MOVE] = _toList(1, "MOV"),
            [Mnemonic.RSTB] = _toList(1, "BRST"),

            [Mnemonic.CONCATSTRING] = _toList(1, "+$(656)"),

            [Mnemonic.NOP] = _toList(1, "NOP"),
            [Mnemonic.END] = _toList(1, "END"),

            [Mnemonic.RUNG_COMMENT] = _toList(0, "CMT"),
        };

    }
}