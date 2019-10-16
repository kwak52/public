using System;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    internal class CxtLineInfo
    {
        int _nonWhiteSpaceStart = 0;

        public int NonWhiteSpaceStartIndex => _nonWhiteSpaceStart;
        public string Trimed { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public string Line { get; private set; }
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
