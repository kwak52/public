using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    internal interface INode { }

    [DebuggerDisplay("{_name}")]
    public class Node : INode, IEquatable<Node>
    {
        string _name;
        public bool Equals(Node other)
        {
            if (other == null)
                return false;

            return Object.ReferenceEquals(this, other);
        }

        public Node(string name)
        {
            _name = name;
        }

        internal bool IsAuxNode { get; set; }
    }



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
    }

    public partial class Rung : Graph<Node, Edge>
    {
        public static Rung CreateRung(IEnumerable<string> mnemonics) => new Rung4Parsing(mnemonics).ToRung();
        /// <summary>
        /// Rung 을 구성하는 IL 목록
        /// </summary>
        public string[] Mnemonics { get; protected set; }
        internal bool AddEdge(Node start, Node end) => AddEdge(start, end, new Edge($"{start}->{end}"));
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
