using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// PLC 접점 및 coil, 출력 등을 표현하기 위한 class
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class Point : IEquatable<Point>
    {
        public string Name { get; private set; }

        //public Guid Guid { get; set; } = Guid.NewGuid();
        static int _guid = 0;
        public int Guid { get; } = ++_guid;
        public ILSentence ILSentence { get; set; }

        ///// <summary>
        ///// User tag object
        ///// </summary>
        //internal NodeInfo4ILConvert NodeInfo { get; set; }

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
        public Point(string name, ILSentence sentence)
            : this(name)
        {
            ILSentence = sentence;
        }

        public override string ToString()
        {
            var il = ILSentence == null ? "" : ILSentence.ToString();
            return il.Contains(Name) ? il : $"{Name}({ILSentence})";
        }
    }
}
