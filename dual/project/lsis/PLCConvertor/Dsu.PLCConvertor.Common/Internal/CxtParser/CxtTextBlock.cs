using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// CXT file 구조를 담기 위한 구조
    /// </summary>
    [DebuggerDisplay("{Key}")]
    internal class CxtTextBlock
    {
        public string Key { get; internal set; }
        public object Value { get; internal set; }

        public CxtTextBlock Parent { get; private set; }

        public List<CxtTextBlock> _subStructures = new List<CxtTextBlock>();
        public List<CxtTextBlock> SubStructures {
            get => _subStructures;
            set {
                value.Iter(s => s.Parent = this);
                _subStructures.AddRange(value);
            } }

        public CxtTextBlock this[string childKey]
        {
            get => SubStructures.FirstOrDefault(s => s.Key == childKey);
        }
        public List<string> Lines { get; set; }

        public CxtTextBlock(string key, object val)
        {
            Key = key;
            Value = val;

            if (/*key == "END" || key == "END;" ||*/ key == "POUInstances" || key == "Programs")
                Console.WriteLine("");
        }

        public CxtTextBlock(string key, object value, IEnumerable<string> lineData)
            : this(key, value)
        {
            if (lineData.NonNullAny())
                Lines = new List<string>(lineData);
        }

        public void PrintAll()
        {
            Stack<CxtTextBlock> stack = new Stack<CxtTextBlock>();
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
            Stack<CxtTextBlock> stack = new Stack<CxtTextBlock>();
            stack.Push(this);

            foreach (var l in _print())
                Console.WriteLine(l);

            return _print();

            IEnumerable<string> _print()
            {
                var start = stack.Peek();
                switch(start.Key)
                {
                    case "Programs":
                        break;
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
}
