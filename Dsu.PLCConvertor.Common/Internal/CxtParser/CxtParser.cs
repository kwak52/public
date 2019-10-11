using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    [DebuggerDisplay("{Key}")]
    internal class Structure
    {
        public string Key { get; internal set; }
        public object Value { get; internal set; }

        public Structure Parent { get; private set; }

        public List<Structure> _subStructures = new List<Structure>();
        public List<Structure> SubStructures {
            get => _subStructures;
            set {
                value.Iter(s => s.Parent = this);
                _subStructures.AddRange(value);
            } }
        public List<string> Lines { get; set; }

        public Structure(string key, object val)
        {
            Key = key;
            Value = val;

            if (/*key == "END" || key == "END;" ||*/ key == "POUInstances")
                Console.WriteLine("");
        }

        public Structure(string key, object value, IEnumerable<string> lineData)
            : this(key, value)
        {
            if (lineData.NonNullAny())
                Lines = new List<string>(lineData);
        }

        public void PrintAll()
        {
            Stack<Structure> stack = new Stack<Structure>();
            stack.Push(this);
            _print();

            void _print()
            {
                var start = stack.Peek();
                Trace.WriteLine($"{GetPath()}={start.Value}");
                //printStack.Pop();

                if (start.SubStructures.Any())
                {
                    foreach (var s in start.SubStructures)
                    {
                        stack.Push(s);
                        _print();
                        //printStack.Pop();
                    }
                }
                else
                    stack.Pop();


                string GetPath() => string.Join("/", 
                    stack
                        .Reverse()
                        .Select(s => s.Key));
            }
        }


        public IEnumerable<string> PrintCodeStuffs()
        {
            Stack<Structure> stack = new Stack<Structure>();
            stack.Push(this);

            foreach (var l in _print())
                Console.WriteLine(l);

            return _print();

            IEnumerable<string> _print()
            {
                var start = stack.Peek();
                switch(start.Key)
                {
                    case "Com":
                    case "SecName":
                        yield return $"{start.Key} = {start.Value}";
                        break;

                    case "SL" when start.Lines != null:
                        foreach (var c in start.Lines)
                            yield return c;

                        break;

                }

                if (start.SubStructures.Any())
                {
                    foreach (var s in start.SubStructures)
                    {
                        stack.Push(s);
                        var xs = _print();
                        foreach (var x in xs)
                            yield return x;
                    }
                }
                else
                    stack.Pop();
            }
        }

    }

    class LineInfo
    {
        int _nonWhiteSpaceStart = 0;

        public int NonWhiteSpaceStartIndex => _nonWhiteSpaceStart;
        public string Trimed { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public string Line { get; private set; }
        public LineInfo(string line)
        {
            Line = line;
            while (char.IsWhiteSpace(line[_nonWhiteSpaceStart]))
                _nonWhiteSpaceStart++;
            Trimed = string.Concat(line.Skip(_nonWhiteSpaceStart)).TrimEnd(new[] { ';' });

            var splitted = Trimed.Split(new[] { ":=" }, StringSplitOptions.RemoveEmptyEntries);
            Key = splitted[0];
            if (splitted.Length > 1)
                Value = splitted[1];
        }
    }

    public class CxtInfo
    {
        public string CXProgVer { get; private set; }
        public string WrittenByCXPVersion { get; private set; }
        public string FileType { get; private set; }
        public string Created { get; private set; }
        public string Modified { get; private set; }

        /// <summary>
        /// Project name.  CX-One 의 project tab 이름
        /// </summary>
        public string Name { get; private set; }
    }

    public class CxtParser
    {
        string[] _lines;
        int _index;
        public CxtParser(string cxtfile)
        {
            _index = 0;
            _lines = File.ReadLines(cxtfile
                , System.Text.Encoding.GetEncoding("ks_c_5601-1987")
                ).ToArray();

            var structure = new Structure("ROOT", "ROOT") { SubStructures = BuildStructures().ToList() };
            structure.PrintAll();

            Trace.WriteLine($"Num. Structures:{structure.SubStructures.Count}");

            structure.PrintCodeStuffs().ToArray();

        }


        IEnumerable<string> ReadPassLineStart(string target)
        {
            while(_index < _lines.Length && ! _lines[_index].TrimStart().StartsWith(target))
            {
                yield return _lines[_index++];
            }

            _index++;
        }

        IEnumerable<Structure> BuildStructures()
        {
            // 마지막 line 도달 check
            if (_index == _lines.Length)
                yield break;


            while (_index < _lines.Length)
            {
                var li = new LineInfo(_lines[_index++]);
                Trace.WriteLine($"{li.NonWhiteSpaceStartIndex}:{li.Key} = {li.Value}");

                var key = li.Key;
                var value = li.Value;


                if (key == "BEGIN")
                    continue;
                else if (key == "END")
                    yield break;
                else if (key.StartsWith("BEGIN_LIST_"))
                {
                    ReadPassLineStart("END_LIST_").ToArray();
                    yield break;
                }


                if (value == null)
                {
                    string next = null;
                    if (_index < _lines.Length)
                        next = _lines[_index].TrimStart();

                    if (next != null && next.StartsWith("$?St$Bk?"))
                    {
                        _index++;
                        var lineData = ReadPassLineStart("$?St$Bk?").ToArray();
                        yield return new Structure(key, value, lineData);
                    }
                    else
                    {
                        var subs = BuildStructures().ToArray();

                        yield return new Structure(key, null) { SubStructures = subs.ToList() };
                    }

                }
                else
                    yield return new Structure(key, value);
            }


            yield break;
        }
    }
}
