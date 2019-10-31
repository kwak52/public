using Dsu.Common.Utilities.ExtensionMethods;
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
        public int Arity => ILSentence == null ? -1 : ILSentence.Arity;

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

        public virtual string ToShortString()
        {
            if (ILSentence == null)
                return Name;


            if (ILSentence.Args.Length == 0)
                return ILSentence.Command.IsNullOrEmpty() ? Name : ILSentence.Command;
            
            return ILSentence.Args[0].ToString();
        }
    }
}
