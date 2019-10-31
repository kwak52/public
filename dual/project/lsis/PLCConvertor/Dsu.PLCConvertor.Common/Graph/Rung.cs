using Dsu.Common.Utilities.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// PLC 의 rung 을 graph 구조로 표현한 class
    /// </summary>
    public class Rung : Graph<Point, Wire>
    {
        public Rung()
            : base(true, false)
        {
        }
        public Rung(IEnumerable<string> mnemonics)
            : this()
        {
            Mnemonics = mnemonics.ToArray();
        }
        public Rung(Rung src)
            : base(src)
        {
        }

        public static Rung CreateRung(IEnumerable<string> mnemonics, PLCVendor sourceType, PLCVendor targetType)
        {
            var r4p = new Rung4Parsing(mnemonics, sourceType, targetType);
            r4p.CoRoutineRungParser().ToArray();
            return r4p.ToRung();
        }
        public static Rung CreateRung(string mnemonics, PLCVendor sourceType, PLCVendor targetType) => CreateRung(MnemonicInput.MultilineString2Array(mnemonics), sourceType, targetType);
        /// <summary>
        /// Rung 을 구성하는 IL 목록
        /// </summary>
        public string[] Mnemonics { get; protected set; }
        internal Nullable<Edge<Point, Wire>> AddEdge(Point start, Point end) => AddEdge(start, end, new Wire(""));
    }
}
