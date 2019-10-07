using Dsu.Common.Utilities.Graph;
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
        public static Rung CreateRung(IEnumerable<string> mnemonics)
        {
            var r4p = new Rung4Parsing(mnemonics);
            r4p.CoRoutineRungParser().ToArray();
            return r4p.ToRung();
        }
        public static Rung CreateRung(string mnemonics) => CreateRung(MnemonicInput.MultilineString2Array(mnemonics));
        /// <summary>
        /// Rung 을 구성하는 IL 목록
        /// </summary>
        public string[] Mnemonics { get; protected set; }
        internal bool AddEdge(Point start, Point end) => AddEdge(start, end, new Wire(""));
    }
}
