using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public static class EmLinq
    {
        // https://stackoverflow.com/questions/25643382/cartesian-products-with-n-number-of-list
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            IEnumerable<IEnumerable<T>> result = emptyProduct;
            foreach (IEnumerable<T> sequence in sequences)
            {
                result =
                    from accseq in result
                    from item in sequence
                    select accseq.Concat(new[] { item });
            }
            return result;
        }

        public static string replace(this string input, string pat1, string pat2)
        {
            return Regex.Replace(input, pat1, pat2);
        }

        // https://stackoverflow.com/questions/7148768/string-split-by-index-params
        public static IEnumerable<string> SplitAt(this string source, params int[] index)
        {
            var indices = new[] { 0 }.Union(index).Union(new[] { source.Length });

            return indices
                        .Zip(indices.Skip(1), (a, b) => (a, b))
                        .Select(_ => source.Substring(_.a, _.b - _.a));

            /*
            var s = "abcd";

            s.SplitAt(); // "abcd"
            s.SplitAt(0); // "abcd"
            s.SplitAt(1); // "a", "bcd"
            s.SplitAt(2); // "ab", "cd"
            s.SplitAt(1, 2) // "a", "b", "cd"
            s.SplitAt(3); // "abc", "d" 
             */
        }

    }


	/// <summary>
	/// 메모리 주소 변환 규칙
	/// </summary>
	// TK0 - TK15 : L112620 - L11262F
	// TK16 - TK31 : L112630 - L11263F
	// ==> TK(%d), $1 = [0-31] => L1126(%x), $1 = $1 + 20
	//
	// CIO
	// 0.00 - 0.15 -> P00000 - P0000F
	// 1.00 - 0.15 -> P10000 - P1000F
	// ==> (%d).(%2d), $1 = [0, 1], $2 = [0-15] => P(%04d)(%x), $1 = $1 * 1000, $2 = $2
	internal class AddressConvertRule
	{
		/// <summary>
		/// e.g "(%d).(%2d)" for Omron CIO
		/// </summary>
		public string SourceRepr { get; private set; }
        /// <summary>
        /// SourceRepr 에서의 argument 의 최소/최대값.  inclusive range
        /// </summary>
		public Tuple<int, int> [] SourceArgsMinMax { get; private set; }
        int SourceArity => SourceArgsMinMax.Length;
		/// <summary>
		/// e.g "P(%04d)(%x)"
		/// </summary>
		public string TargetRepr { get; private set; }
        /// <summary>
        /// Target argument 명세: source argument 를 이용한 expression
        /// </summary>
		public string [] TargetArgsExpr { get; private set; }

		public AddressConvertRule(string rule)
		{

		}

        public AddressConvertRule(string sourceRepr, IEnumerable<Tuple<int, int>> sourceArgsMinMax,
            string targetRepr, IEnumerable<string> targetArgsExpr)
        {
            SourceRepr = sourceRepr;
            TargetRepr = targetRepr;
            SourceArgsMinMax = sourceArgsMinMax.ToArray();
            TargetArgsExpr = targetArgsExpr.ToArray();
        }

        /// <summary>
        /// 규칙에 맞는 모든 source 의 sample 생성
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GenerateSourceSamples()
        {
            IEnumerable<int> range(Tuple<int, int> tpl) => Enumerable.Range(tpl.Item1, tpl.Item2-tpl.Item1+1);

            return Enumerable.Range(0, SourceArity)
                .Select(n => range(SourceArgsMinMax[n]))
                .CartesianProduct()
                .Select(x => FormatAddress(SourceRepr, x.ToArray()))
                ;
        }



        /// <summary>
        /// address pattern 에 맞추어 argument 값들을 적용한 전체 address
        /// </summary>
		/// <param name="pattern">e.g "P(%4d)(%2x)"</param>
        /// <param name="args">e.g [] {2000, 15}</param>
        /// <returns>e.g "P20000F" </returns>
        public static string FormatAddress(string pattern, int [] args)
        {
            return string.Join("", generatePartial(pattern));

            // e.g pat = "P(%4d)(%2x)"   args = [2000, 15]
            // => returns [ "P", "2000", "0F" ]
            IEnumerable<string> generatePartial(string pat)
            {
                var matches =   // e.g [| "(%4d)"; "(%2x)"; |]
                    Regex.Matches(pat, @"\((.*?)\)")
                    .Cast<Match>()
                    .Select(ma => ma.ToString())
                    .ToArray();
                ;

                int n = 0;
                while(true)
                {
                    if (pat.Length <= 1 || matches.Length == 0)
                    {
                        yield return pat;
                        yield break;
                    }

                    if (pat.StartsWith(matches[n]))
                    {
                        var format = C2CSharp(matches[n]);
                        var x2 = string.Format(format, args[n]);
                        yield return x2;
                        pat = pat.Substring(matches[n].Length, pat.Length - matches[n].Length);
                        n++;
                        continue;
                    }

                    var split = pat.SplitAt(1).ToArray();
                    yield return split[0];
                    pat = split[1];
                }
            }


            // C format string 을 C# format string 으로 변환
            string C2CSharp(string c)
            {
                var match = Regex.Match(c, @"%(0?)(\d*)([dxDX])");
                var g = match.Groups.Cast<Group>().Select(gr => gr.ToString()).ToArray();
                if (g.Length == 4)
                {
                    var hex = g[3];

                    if (g[1].Length == 0 && g[2].Length == 0)   // "%d" or %x
                        return $"{{0:{hex}}}";

                    var d = g[2].Length == 0 ? 0 : int.Parse(g[2].ToString());
                    return $"{{0:{hex}{d}}}";
                }

                return "";
            }
        }


        static DataTable _dt = new DataTable();

        public string Convert(string sourceAddress)
        {
            var elements = AnalyzeAddressComponents(sourceAddress, SourceRepr);
            var sourceIsInRange =
                elements.Select((e, n) => (e, n))
                    .All(tpl => SourceArgsMinMax[tpl.n].Item1 <= tpl.e && tpl.e <= SourceArgsMinMax[tpl.n].Item2)
                ;

            if (!sourceIsInRange)
                throw new Exception($"{sourceAddress} is not in proper range.");

            var transformed =
                TargetArgsExpr
                    .Select(ex =>
                    {
                        for (int i = 0; i < elements.Length; i++)
                            ex = ex.Replace($"${i}", $"{elements[i]}");

                        return ex;
                    })
                    //.Select(ex => int.Parse(_dt.Compute(ex, "").ToString()))
                    .Select(ex =>
                    {
                        var compu = _dt.Compute(ex, "");
                        var n = int.Parse(compu.ToString());
                        return n;
                    })
                    .ToArray()
                    ;

            var target = FormatAddress(TargetRepr, transformed);
            return target;
        }

        // https://www.dotnetperls.com/regex-groups : named regex replacement
        /// <summary>
        /// 주어진 입력 주소에서 pattern 별로 분해 한 후, 분해된 요소의 정수 값을 arrary 로 반환한다.
        /// </summary>
        /// <param name="input">e.g "P10000F"</param>
        /// <param name="pattern_">e.g "P(%4d)(%2x)"</param>
        /// <returns></returns>
        static int[] AnalyzeAddressComponents(string input, string pattern_)
        {
            // C printf format 을 가진 pattern_ 을 정규식으로 변환 한 값.  e.g "P(\\d{4})([A-Fa-f0-9]{2})(\\d+)"
            var pattern =
                pattern_
                .replace(@"%d", @"\d+")                                 // %d --> \d+
                .replace(@"%x", @"[A-Fa-f0-9]+")                        // %x -> [A-Fa-f0-9]+
                .replace(@"%(?<count>\d+)d", @"\d{${count}}")           // %2d --> \d{2}
                .replace(@"%(?<count>\d+)x", @"[A-Fa-f0-9]{${count}}")  // %3x -> [A-Fa-f0-9]{3}
                ;

            // pattern_ 의 () 인자 순서대로 hex 값을 가질 수 있는지 여부의 array.  e.g [false, true, false]
            var matchDecHex =
                Regex.Matches(pattern_, @"\((.*?)\)")           // Regex.Match() 와 Regex.Matches() 는 다르다.   https://stackoverflow.com/questions/740642/c-sharp-regex-split-everything-inside-square-brackets
                .Cast<Match>()
                .Select(ma => ma.ToString().Contains("x"))
                .ToArray()
                ;



            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                Debug.Assert(input == match.Groups[0].ToString());

                var intValues =     // e.g [1000, 255, 3]
                    match.Groups.Cast<Group>().Skip(1)
                        .Zip(matchDecHex, (g, h) => (g, h))
                        .Select(tpl => System.Convert.ToInt32(tpl.g.ToString(), tpl.h ? 16 : 10))
                        .ToArray()
                    ;
                return intValues;
            }
            return null;
        }


        //JsonConvert.SerializeObject(this, MyJsonSerializer.JsonSettings);


        public static void Test()
        {
            {
                var r = AnalyzeAddressComponents("P1000FF3", @"P(%4d)(%2x)(%d)");
                Debug.Assert(r[0] == 1000);
                Debug.Assert(r[1] == 0xFF);
                Debug.Assert(r[2] == 3);
            }
            {
                var r = AnalyzeAddressComponents("P13FF3", @"P(%04d)(%02x)(%d)");
                Debug.Assert(r == null);
            }

            {
                var r = AnalyzeAddressComponents("P20003F", @"P(%d)(%2x)");
                Debug.Assert(r.Length == 2);
                Debug.Assert(r[0] == 2000);
                Debug.Assert(r[1] == 0x3F);
            }
            {
                var r = AnalyzeAddressComponents("P20003F", @"P(%04d)(%02x)");
                Debug.Assert(r.Length == 2);
                Debug.Assert(r[0] == 2000);
                Debug.Assert(r[1] == 0x3F);
            }

        }
    }

	public static class DoTest
	{
        public static void Test()
		{
            AddressConvertRule.Test();
            var rule = new AddressConvertRule(
                "(%d).(%2d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 15) },
                "P(%04d)(%x)", new[] { "$0 * 1000", "$1" });

            var samples = rule.GenerateSourceSamples().ToArray();
            var converted = samples.Select(s => $"{s} => {rule.Convert(s)}").ToArray();

            Console.WriteLine("");
		}
	}


}
