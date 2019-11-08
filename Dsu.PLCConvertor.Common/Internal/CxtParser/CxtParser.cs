using Dsu.Common.Utilities.ExtensionMethods;
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
        /// <summary>
        /// .cxt file 을 읽어 들여서 line 단위로 저장
        /// </summary>
        string[] _lines;

        /// <summary>
        /// _lines 의 현재 읽고 있는 라인 번호
        /// </summary>
        int _index;
        public CxtParser(string cxtfile)
        {
            _index = 0;
            _lines = File.ReadLines(cxtfile
                , System.Text.Encoding.GetEncoding("ks_c_5601-1987")
                ).ToArray();
        }


        /// <summary>
        /// _lines 의 현재 위치에서 target 으로 주어진 문자열 이후의 line 으로 cursor (index) 위치 이동
        /// </summary>
        IEnumerable<string> ReadPassLineStart(string target)
        {
            while(_index < _lines.Length && ! _lines[_index].Contains(target))
            {
                yield return _lines[_index++];
            }

            var l = _lines[_index];
            var p = l.IndexOf(target);
            if (p > 0)
                yield return l.Substring(0, p);

            _index++;
        }

        /// <summary>
        /// _lines 의 현재 위치에서 text block 들을 생성해서 반환.
        /// 현재 위치와 동일한 레벨의 text block 이 여럿인 경우 모두 반환
        /// </summary>
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

                if (key.IsOneOf("Programs", "Sections", "RC"))
                    System.Console.WriteLine("");


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

                    if (next == null)
                    {
                        var subs = BuildStructures().ToArray();

                        yield return new CxtTextBlock(key, null) { SubStructures = subs.ToList() };
                    }
                    else
                    {
                        if (next.StartsWith("$?St$Bk?"))
                        {
                            _index++;
                            var lineData = ReadPassLineStart(next).ToArray();
                            yield return new CxtTextBlock(key, value, lineData);
                        }
                        else if (next == "BEGIN" || next.StartsWith("BEGIN_LIST_"))
                        {
                            var subs = BuildStructures().ToArray();

                            yield return new CxtTextBlock(key, null) { SubStructures = subs.ToList() };
                        }
                        else
                            Trace.WriteLine($"Warn: Unprocessed {next}");
                    }
                }
                else
                    yield return new CxtTextBlock(key, value);
            }


            yield break;
        }
    }
}
