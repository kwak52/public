using System;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 옴론 .cxt file 의 한 라인에 담긴 key, value pari 의 정보를 추출하기 위한 구조
    /// </summary>
    internal class CxtLineInfo
    {
        int _nonWhiteSpaceStart = 0;

        /// <summary>
        /// 실제 읽어 들인 line 의 raw string
        /// </summary>
        public string Line { get; private set; }
        public int NonWhiteSpaceStartIndex => _nonWhiteSpaceStart;
        /// <summary>
        /// 읽어 들인 라인에서 불필요한 공백 및 마지막 ';' 등을 제외한 결과
        /// </summary>
        public string Trimed { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public CxtLineInfo(string line)
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
}
