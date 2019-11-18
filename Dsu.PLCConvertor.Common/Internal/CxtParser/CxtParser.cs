using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        static Encoding _encoding = Encoding.GetEncoding("ks_c_5601-1987");

        /// <summary>
        /// 다중 라인을 한 라인으로 encoding 하기 위한 line 구분자 특수문자
        /// </summary>
        static string _lineSeparator = "\f";
        /// <summary>
        /// 다중 라인을 한 라인으로 encoding 하기 위한 block 구분자 특수문자
        /// </summary>
        static string _blockSeparator = "\v";
        static string _blockRegexPattern = @"\$\?St\$Bk\?_#\[\d+\]";        // e.g. "$?St$Bk?_#[5894]"

        /// <summary>
        /// 특수문자로 Encoding 된 다중라인 block 을 해체해서 다중 라인으로 반환한다.
        /// </summary>
        /// <param name="block">encoding 된 다중 라인 문자열</param>
        /// <param name="splitOption"></param>
        /// <returns></returns>
        public static string[] SplitBlock(string block, StringSplitOptions splitOption=StringSplitOptions.None)
        {
            return block.Replace(_blockSeparator, "").Split(new[] { _lineSeparator }, splitOption);
        }
        public CxtParser(string cxtfile)
        {
            _index = 0;
            _lines =
                joinMultiLineInputs()
                .ToArray();

#if DEBUG
            var conv = joinMultiLineInputs().ToArray();
            File.WriteAllLines("test.conv.txt", conv);
#endif
            // CXT project file 을 읽어서 다중 라인에 표현된 항목을 하나의 line 합친다.
            // CXT file 에서 "$?St$Bk?" 로 구분된 다중 라인은 parsing 을 힘들게 하므로
            // "$?St$Bk?" 를 포함한 다중 라인을 특수 문자를 포함한 하나의 라인으로 변환한다.
            IEnumerable<string> joinMultiLineInputs()
            {
                var lines =
                    File.ReadLines(cxtfile, _encoding)
                    .ToArray();

                int i = 1;
                for (; i < lines.Length;)
                {
                    if (lines[i].StartsWith(Cxp.BlockSeparator))
                    {
                        var prevLine = lines[i - 1];
                        var text = Regex.Replace(lines[i], _blockRegexPattern, _blockSeparator);
                        var key = lines[i];
                        i++;
                        var pass = readPass(key);
                        var res = prevLine + text + pass;
                        yield return res;
                    }
                    else
                    {
                        yield return lines[i - 1];
                        i++;
                    }
                }
                yield return lines[lines.Length - 1];

                // "$?St$Bk?" 의 끝부분까지 읽으면서, 결과를 하나의 라인으로 합쳐서 반환한다.
                string readPass(string key)
                {
                    var multi = new List<string>();
                    for (; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(key))
                        {
                            try
                            {
                                return
                                    multi
                                    .Concat(new[] { Regex.Replace(lines[i], _blockRegexPattern, _blockSeparator) })
                                    .JoinString(_lineSeparator);
                            }
                            finally
                            {
                                i += 2;
                            }
                        }
                        else
                            multi.Add(lines[i]);
                    }

                    return null;
                }
            }
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
            Stack<string> keyStack = new Stack<string>(new[] { "ROOT" });
            return buildStructures();


            IEnumerable<CxtTextBlock> buildStructures()
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

                    if (key.IsOneOf("Programs", "Sections", "RC", "VariableList", "GlobalVariables"))
                        System.Console.WriteLine("");


                    if (key == "BEGIN")
                        continue;
                    else if (key == "END")
                        yield break;
                    else if (key.StartsWith("BEGIN_LIST_"))
                    {
                        var end = key.Replace("BEGIN_LIST_", "END_LIST_");
                        var lineData = ReadPassLineStart(end).ToArray();
                        yield return new CxtTextBlock(keyStack.Peek(), value, lineData);
                        yield break;
                    }


                    if (value == null)
                    {
                        string next = null;
                        if (_index < _lines.Length)
                            next = _lines[_index].TrimStart();

                        if (next == null)
                        {
                            keyStack.Push(key);
                            var subs = buildStructures().ToArray();
                            keyStack.Pop();

                            yield return new CxtTextBlock(key, null) { SubStructures = subs.ToList() };
                        }
                        else
                        {
                            if (next.StartsWith(Cxp.BlockSeparator))
                            {
                                _index++;
                                var lineData = ReadPassLineStart(next).ToArray();
                                yield return new CxtTextBlock(key, value, lineData);
                            }
                            else if (next == "BEGIN" || next.StartsWith("BEGIN_LIST_"))
                            {
                                keyStack.Push(key);
                                var subs = buildStructures().ToArray();
                                keyStack.Pop();

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
}
