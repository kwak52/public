using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmString
    {
        public static string[] ParseAgainstRegex(this string s, string regex)
        {
            List<string> tokens = new List<string>();
            var match = Regex.Match(s, regex);
            for (int i = 1; i < match.Groups.Count; i++)
                tokens.Add(match.Groups[i].Value);

            return tokens.ToArray();
        }

		/// <summary>
		/// begin 과 end 로 둘러싸인 s 에서 begin 과 end 를 제거한 가운데 값만을 뽑아내어 반환
		/// </summary>
	    public static string Strip(this string s, string begin, string end)
	    {
			if (s.NonNullAny() && s.StartsWith(begin) && s.EndsWith(end))
			{
				var length = s.Length - begin.Length - end.Length;
				return new string(s.Skip(begin.Length).Take(length).ToArray());
			}

		    return null;
	    }
	}
}
