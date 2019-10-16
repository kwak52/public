using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    internal class CxtParser
    {
        string[] _lines;
        int _index;
        public CxtParser(string cxtfile)
        {
            _index = 0;
            _lines = File.ReadLines(cxtfile
                , System.Text.Encoding.GetEncoding("ks_c_5601-1987")
                ).ToArray();
        }


        IEnumerable<string> ReadPassLineStart(string target)
        {
            while(_index < _lines.Length && ! _lines[_index].TrimStart().StartsWith(target))
            {
                yield return _lines[_index++];
            }

            _index++;
        }

        public IEnumerable<CxtTextBlock> BuildStructures()
        {
            // 마지막 line 도달 check
            if (_index == _lines.Length)
                yield break;


            while (_index < _lines.Length)
            {
                var li = new CxtLineInfo(_lines[_index++]);
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
                        yield return new CxtTextBlock(key, value, lineData);
                    }
                    else
                    {
                        var subs = BuildStructures().ToArray();

                        yield return new CxtTextBlock(key, null) { SubStructures = subs.ToList() };
                    }

                }
                else
                    yield return new CxtTextBlock(key, value);
            }


            yield break;
        }
    }
}
