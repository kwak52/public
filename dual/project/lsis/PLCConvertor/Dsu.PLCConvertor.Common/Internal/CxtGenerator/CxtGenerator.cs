using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// CXT text project 파일 생성자
    /// 변환에 실패한 rung 만을 골라내어서 review 용으로 새로운 project 생성하기 위한 용도
    /// </summary>
    public class CxtGenerator
    {
        static string _cxtProjectHeader;
        static string _cxtProjectFooter;
        static Encoding _encoding => CxtParser._encoding;
        static CxtGenerator()
        {
            var emptyProjectLines = File.ReadAllLines("Config/EmptyProject.cxt", _encoding).ToArray();
            if (_cxtProjectHeader == null)
                _cxtProjectHeader =
                    emptyProjectLines
                    .TakeWhile(l => !l.StartsWith("<End Of Header>"))
                    .JoinString("\r\n")
                    ;
            if (_cxtProjectFooter == null)
                _cxtProjectFooter =
                    emptyProjectLines
                    .SkipWhile(l => !l.StartsWith("<Start Of Footer>"))
                    .Skip(1)
                    .JoinString("\r\n")
                    ;
        }


        //private ConvertParams _convertParams;
        public CxtGenerator(ConvertParams cvtParams)
        {
            //_convertParams = cvtParams;
        }

        List<string[]> _rungs = new List<string[]>();

        /// <summary>
        /// 변환에 실패한 rung 을 project 생성해서 출력하기 위해서 따로 모은다.
        /// </summary>
        /// <param name="prog">실패한 rung 이 포함된 Program</param>
        /// <param name="mnemonics">실패한 rung</param>
        public void AddRungs(CxtInfoProgram prog, string[] mnemonics, Exception ex)
        {
            var dic = ConvertParams.SourceVariableMap;
            var commented = new[] {
                $"' {ex.Message}",
                };

            var mnReverted =
                mnemonics.Select(m =>
                {
                    var tokens = m.Split(new[] { ' ' }).ToArray();

                    return generateToken().JoinString(" ");
                    IEnumerable<string> generateToken()
                    {
                        yield return tokens[0];
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            var tok = tokens[i];
                            var variable = ConvertParams.SearchVariable(prog, tok);
                            if (variable != null)
                                yield return variable.Device;
                            else
                                yield return tok;
                        }
                    }
                })
                .ToArray();
                ;

            var failedRung = new[] { commented, mnReverted }.SelectMany(m => m).ToArray();
            _rungs.Add(failedRung);
        }

        public IEnumerable<string> GenerateProject()
        {
            var q = '"';
            int startNumber = 8;

            yield return _cxtProjectHeader;
            var numRungs = _rungs.Count + 1;
            yield return $"      RC:={numRungs};";

            int n = 0;

            foreach (var r in _rungs)
            {
                yield return generateRung(n, _rungs[n]);
                n++;
            }
            yield return generateRung(n, new string[] { });
            n++;

            yield return _cxtProjectFooter;


            string generateRung(int rIndex, string[] mnemonics)
            {
                /*
      R[0]:=
      BEGIN
       Com:="";
       Flags:="1,0";
       FBversion:="";
       SL:="";
       AtchCmts:=
       BEGIN
        CC:=0;
       END;
      END;
      */
                var m = generateSL();
                return $@"      R[{rIndex}]:=
      BEGIN
       Com:={q}{q};
       Flags:={q}1,0{q};
       FBversion:={q}{q};
       SL:={m}
       AtchCmts:=
       BEGIN
        CC:=0;
       END;
      END;";


                string generateSL()
                {
                    if (mnemonics.IsNullOrEmpty())
                        return "\"\"";

                    var xs = generateWrappedBlock();
                    return xs.JoinString("\r\n");

                    IEnumerable<string> generateWrappedBlock()
                    {
                        yield return "";
                        yield return $"{Cxp.BlockSeparator}_#[{startNumber}]";
                        foreach (var x in mnemonics)
                            yield return x;
                        yield return $"{Cxp.BlockSeparator}_#[{startNumber++}]";
                    }
                }
            }

        }
    }


}
