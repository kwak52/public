using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// 옴론 CX-One 을 LSIS XG5000 용으로 변환
    /// </summary>
    public class Cx2Xg5k
    {
        /// <summary>
        /// 옴론 CX-One .cxt file을 LSIS XG5000 용 .qtx file로 변환
        /// </summary>
        public static void Convert(ConvertParams cvtParams, string cxtFile, string xg5kFile, string configFile, string xg5kMessageFile)
        {
            var cxt = CxtInfoRoot.Parse(cxtFile);

            var programs = cxt.EnumerateType<CxtInfoProgram>().ToArray();

            programs.Iter(prog => prog.Convert(cvtParams));

            var convertedContents = programs.SelectMany(prog => prog.CollectResults(cvtParams));

            var cLines =
                new[] { GenerateHeader(), convertedContents, GenerateFooter() }
                .SelectMany(ls => ls)
                ;

            File.WriteAllLines(xg5kFile, cLines, Encoding.GetEncoding("ks_c_5601-1987"));

            // 메시지 파일 내용 생성
            using (StreamWriter msgStream = new StreamWriter(xg5kMessageFile))
            {
                var msgContents = programs.SelectMany(prog => prog.CollectMessages(cvtParams));
                var mLines = 
                    new[] { GenerateHeader(), msgContents, GenerateFooter() }
                    .SelectMany(ls => ls)
                    ;

                msgStream.WriteLine(string.Join("\r\n", mLines));
            }

            // XG5000 .qtx header 생성
            IEnumerable<string> GenerateHeader()
            {
                yield return "[PROJECT CONFIGURATION]";
                yield return "[PROGRAM LIST]";

                IEnumerable<string> xs = null;
                // 각 프로그램 (section) 이름 출력
                if (cvtParams.SplitBySection)
                {
                    var secWithResult =
                        from prg in programs
                        from sec in prg.Sections
                        where sec.Rungs.Any(r => r.ConvertResults.NonNullAny())
                        select sec
                        ;

                    xs = secWithResult.Select(sec => $"{sec.ParentProgram.Name}:{sec.Name}");
                }
                else
                {
                    var progWithResult =
                        from prg in programs
                        where prg.Sections.Any(sec => sec.Rungs.Any(r => r.ConvertResults.NonNullAny()))
                        select prg
                        ;

                    xs = progWithResult.Select(prog => prog.Name);
                }

                foreach (var x in xs)
                    yield return x;

                yield return "[PROGRAM LIST END]";
                yield return "";
            }

            // XG5000 .qtx footer 생성
            IEnumerable<string> GenerateFooter()
            {

                yield return "";
                yield return "[COMMENT FILE] COMMENT";
                yield return "[COMMENT FILE END]";
            }
        }

        public static AddressConvertor CreateDefaultAddressConvertRuleSets()
        {
            // special memory mapping pairs
            var smPairs = new[]
            {
                ("P_GT", "F123"),
                ("P_GE", "F124"),
                ("P_EQ", "F122"),
                ("P_NE", "F125"),
                ("P_LT", "F120"),
                ("P_LE", "F121"),


                ("P_On", "F00099"),     // _ON
                ("P_Off", "F0009A"),    // _OFF
            };

            // user customizable rules
            var ucRules = new[]
            {
                new AddressConvertRule(
                    "(%d).(%2d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 15) },
                    "P(%04d)(%x)", new[] { "$0 * 1000", "$1" }),

                new NamedAddressConvertRule("TIMER",
                    "(%d)", Enumerable.Empty<Tuple<int, int>>(),
                    "T(%d)", new [] {"$0" }),
            };
                

            var ruleSets =
                smPairs
                    .Select(pr => new AddressConvertRuleSpecialRelay(pr.Item1, pr.Item2))
                    .Cast<IAddressConvertRule>()
                    .Concat(ucRules)
                    ;
            
            return new AddressConvertor(ruleSets);
        }
    }
}
