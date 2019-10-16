using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
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
        public static void Convert(string cxtFile, string xg5kFile, string configFile, string xg5kMessageFile)
        {
            var cxt = CxtInfoRoot.Parse(cxtFile);
            var sections = cxt.EnumerateType<CxtInfoSection>();
            var secsConverted = sections.SelectMany(sec => sec.Convert(PLCVendor.LSIS));
            var lines =
                new[] { GenerateHeader(), secsConverted, GenerateFooter() }
                .SelectMany(ls => ls)
                ;

            File.WriteAllLines(xg5kFile, lines, Encoding.GetEncoding("ks_c_5601-1987"));


            // XG5000 .qtx header 생성
            IEnumerable<string> GenerateHeader()
            {
                yield return "[PROJECT CONFIGURATION]";
                yield return "[PROGRAM LIST]";

                // 각 프로그램 (section) 이름 출력
                var xs = sections.Select(sec => sec.Name);
                foreach (var x in xs)
                    yield return x;
                
                yield return "[PROGRAM LIST END]";
                yield return "";
            }

            // XG5000 .qtx footer 생성
            IEnumerable<string> GenerateFooter()
            {

                yield return "[PROGRAM FILE END]";
                yield return "";
                yield return "[COMMENT FILE] COMMENT";
                yield return "[COMMENT FILE END]";
            }
        }
    }
}
