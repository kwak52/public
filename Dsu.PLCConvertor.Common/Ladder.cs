using Dsu.Common.Utilities.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// PLC 접점 및 coil, 출력 등을 표현하기 위한 class
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class Point : IEquatable<Point>
    {
        public string Name { get; private set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public ILSentence ILSentence { get; set; }

        public bool Equals(Point other)
        {
            if (other == null)
                return false;

            return Object.ReferenceEquals(this, other);
        }

        public Point(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}({ILSentence})";
        }
    }


    /// <summary>
    /// PLC rung 에서 접점 간 연결을 표현하는 edge class
    /// </summary>

    [DebuggerDisplay("{Name}")]
    public class Edge
    {
        public string Name { get; private set; }
        public Edge(ILSentence sentence)           
        {
            Name = sentence.ToString();
        }
        public Edge(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }

    /// <summary>
    /// PLC 의 rung 을 graph 구조로 표현한 class
    /// </summary>
    public partial class Rung : Graph<Point, Edge>
    {
        public Rung()
        {
        }
        public Rung(IEnumerable<string> mnemonics)
        {
            Mnemonics = mnemonics.ToArray();
        }
        public static Rung CreateRung(IEnumerable<string> mnemonics)
        {
            var r4p = new Rung4Parsing(mnemonics);
            r4p.CoRoutineRungParser().ToArray();
            return r4p.ToRung();
        }
        /// <summary>
        /// Rung 을 구성하는 IL 목록
        /// </summary>
        public string[] Mnemonics { get; protected set; }
        internal bool AddEdge(Point start, Point end) => AddEdge(start, end, new Edge($"{start.Name}->{end.Name}"));
    }




    public class ILSentence
    {
        public string Command { get; private set; }
        public string[] Args { get; private set; }
        public ILSentence(string ilSentence)
        {
            var tokens = ilSentence.Split(new[] { ' ', '\t' });
            Command = tokens[0];
            Args = tokens.Skip(1).ToArray();
        }

        public ILSentence(ILSentence other)
        {
            Command = other.Command;
            Args = other.Args;
        }

        public override string ToString()
        {
            return $"{Command} {string.Join(" ", Args)}";
        }
    }
}
