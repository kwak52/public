using Dsu.Common.Utilities.Core.ExtensionMethods;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static Encoding _encoding = Encoding.GetEncoding("ks_c_5601-1987");
        /// <summary>
        /// 옴론 CX-One .cxt file을 LSIS XG5000 용 .qtx file로 변환
        /// </summary>
        public static CxtInfoRoot Convert(ConvertParams cvtParams, string cxtFile, string xg5kFile, string configFile, string cxtReviewFile, string xg5kMessageFile)
        {
            Global.UIMessageSubject.OnNext($"CXT file 분석 중..");
            // text project 를 파싱함
            var cxt = CxtInfoRoot.Parse(cxtFile);

            // global/local 변수 선언부 처리
            cvtParams.BuildSymbolTables(cxt);            

            // PLC programs 부 : 각 program 은 다중 section 으로 구성되어 있다.
            var programs = cxt.Programs.ToArray();

            var xxxResults = programs.SelectMany(prog => prog.Convert(cvtParams)).ToArray();
            var ccc = CollectResult();

            //var convertedContents = programs.SelectMany(prog => prog.CollectResults(cvtParams)).ToArray();
            var convertedContents = ccc.Item1;

            // 산전 PLC 로 변환된 lines
            var cLines =
                new[] { GenerateHeader(), convertedContents, GenerateFooter() }
                .SelectMany(ls => ls)
                ;

            File.WriteAllLines(xg5kFile, cLines, _encoding);

            // 메시지 파일 내용 생성
            using (StreamWriter msgStream = new StreamWriter(xg5kMessageFile, false, _encoding))
            {
                //var msgContents = programs.SelectMany(prog => prog.CollectMessages(cvtParams));
                var msgContents = ccc.Item2;
                var mLines = 
                    new[] { GenerateHeader(), msgContents }
                    .SelectMany(ls => ls)
                    ;

                msgStream.WriteLine(string.Join("\r\n", mLines));
            }

            // 생성 실패한 rung 따로 project 로 기록
            var fails = cvtParams.ReviewProjectGenerator.GenerateProject();
            File.WriteAllLines(cxtReviewFile, fails, _encoding);

            return cxt;

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

                foreach ( var v in ConvertParams.UsedSourceDevices.Values )
                {
                    var name = v.Variable.NonNullEmptySelector(v.Name);
                    yield return $"{v.Device}:{v.Comment}:{name}:{v.Type.ToString().ToUpper()}";

                }

                yield return "[COMMENT FILE END]";
            }

            (string[], string[]) CollectResult()
            {
                (string[], string[]) CollectResultForUnit(CxtInfo unit, IEnumerable<ConvertResult> results)
                {
                    int lineNum = 0;
                    var qtxs = new List<string>();
                    var msgs = new List<string>();
                    foreach ( var r in results)
                    {
                        var srcStart = r.Rung.AccumulatedStartILIndex;
                        foreach (var m in r.Messages)
                        {
                            if (Cx2Xg5kOption.AddMessagesToLabel)
                            {
                                qtxs.Add($"{lineNum}\tCMT\t{Cx2Xg5kOption.LabelHeader}\t{m}");
                                msgs.Add($"[{lineNum++}] [{srcStart}] [{m}]");
                            }
                            else
                                msgs.Add($"[{lineNum}] [{srcStart}] [{m}]");
                        }
                        var annotatedQtxs = r.Results.Select(m => $"{lineNum++}\t{m}");
                        qtxs.AddRange(annotatedQtxs);
                    }

                    var prog = unit as CxtInfoProgram;
                    var sec = unit as CxtInfoSection;
                    Debug.Assert(prog != null || sec != null);
                    var hdr = prog == null ? $"{sec.ParentProgram.Name}:{sec.Name}" : prog.Name;
                    var Qtx = CxtInfo.WrapWithProgram(hdr, qtxs).ToArray();
                    var Msg = CxtInfo.WrapWithProgram(hdr, msgs).ToArray();


                    return (Qtx, Msg);
                }
                var bySection = cvtParams.SplitBySection;
                //var sections = programs.SelectMany(prog => prog.Sections).ToHashSet();
                var genUnits =
                    from r in xxxResults
                    group r by (bySection ? (CxtInfo)r.Section : r.Program) into g
                    select new { Unit = g.Key, Rungs = g.ToList() }
                    ;

                var rQtx = new List<string>();
                var rMsg = new List<string>();
                genUnits
                    .Select(gu => CollectResultForUnit(gu.Unit, gu.Rungs))
                    .Iter(pr =>
                    {
                        rQtx.AddRange(pr.Item1);
                        rMsg.AddRange(pr.Item2);
                    });

                //return (rQtx.JoinString("\r\n"), rMsg.JoinString("\r\n"));
                return (rQtx.ToArray(), rMsg.ToArray());
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

                ("P_1s", ""),         // 1-s clock pulse

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
                    //"(%d)", Enumerable.Empty<Tuple<int, int>>(),
                    "(%d)", new [] { Tuple.Create(0, 100), },
                    "T(%d)", new [] {"$0" }),

                //new AddressConvertRule(
                //    "E(%x)_(%5d)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 32767) },
                //    "L(%05d)", new[] { "$0 * 32768 + $1" }),

                //new AddressConvertRule(
                //    "E(%x)_(%5d).(%x)", new[] { Tuple.Create(0, 1), Tuple.Create(0, 32767), Tuple.Create(0, 15) },
                //    "L(%05d).(%x)", new[] { "$0 * 32768 + $1", "$2" }),
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



    public interface IConvertOption { }
    public static class Cx2Xg5kOption /*: IConvertOption*/
    {
        /// <summary>
        /// 강제로 section 에 의해서 구분할 지의 여부.
        /// </summary>
        public static bool SplitBySection { get; set; } = true;

        /// <summary>
        /// XG5000 에 rung 단위 구분자를 삽입할지 여부.
        /// </summary>
        public static bool ForceRungSplit { get; set; } = false;

        /// <summary>
        /// 변환 과정에 발생한 문제점들을 설명문에 삽입할지 여부
        /// </summary>
        public static bool AddMessagesToLabel { get; set; } = true;

        public static bool CopySourceComment { get; set; } = true;

        public static string LabelHeader { get; set; } = "**(변환)**";


        static LogLevel _logLevel = LogLevel.WARN;
        public static LogLevel LogLevel
        {
            get => _logLevel;
            set {
                _logLevel = value;
                var log4netLevel = Level.Off;
                switch(_logLevel)
                {
                    case LogLevel.NONE: log4netLevel = Level.Off; break;
                    case LogLevel.FATAL: log4netLevel = Level.Fatal; break;
                    case LogLevel.WARN: log4netLevel = Level.Warn; break;
                    case LogLevel.INFO: log4netLevel = Level.Info; break;
                    case LogLevel.DEBUG: log4netLevel = Level.Debug; break;
                }
                ((log4net.Repository.Hierarchy.Logger)Global.Logger.Logger).Level = log4netLevel;
            }
        }
    }

}
