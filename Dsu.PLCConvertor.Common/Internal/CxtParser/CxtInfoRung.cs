using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 옴론 project(.cxt) 파일에서 rung 을 표현하기 위한 정보
    /// Rung 은 다수의 il 문장을 포함한다.
    /// </summary>
    public class CxtInfoRung : CxtInfo
    {
        public string Name { get; private set; }
        public string Comment { get; internal set; }
        public string[] ILs { get; internal set; } = new string[] {};

        /// <summary>
        /// Program 으로부터의 누적 rung index
        /// </summary>
        public int AccumulatedRungIndex { get; set; }
        public int AccumulatedStartILIndex { get; set; }

        public IEnumerable<string> EffectiveILs => ILs.SkipWhile(il => il.StartsWith("'"));
        internal CxtInfoRung(string name)
            : base("Rung")
        {
            Name = name;
        }
        public override IEnumerable<CxtInfo> Children { get { return Enumerable.Empty<CxtInfo>(); } }

        /// <summary>
        /// Rung 에 대한 변환 결과를 저장
        /// </summary>
        internal ConvertResult ConvertResult;
        internal IEnumerable<string> ConvertResults => ConvertResult?.Results;

        internal IEnumerable<string> GetAllConvertMessages() => ConvertResult?.Messages;

        /// <summary>
        /// 변환 중간 결과 clear/reset
        /// </summary>
        internal override void ClearMyResult()
        {
            ConvertResult = null;
        }

        public ConvertResult Convert(ConvertParams cvtParam, CxtInfoProgram prog, CxtInfoSection sec, int targetStartIndex)
        {
            ConvertResult convertResult = null;
            var rung = this;
            // "A  F0" 가 맨 끝에 오는 경우는 rung 주석이 포함된 경우이다.
            var ils = rung.ILs.Where(il => !il.StartsWith("'") && !il.StartsWith("//") && il != "\"\"" && il != "A  F0");

            if (ils.Any())
            {
                var s = rung.AccumulatedStartILIndex;
                var t = targetStartIndex;

                try
                {
                    convertResult = Rung2ILConvertor.ConvertFromMnemonics(ils, rung.Comment, cvtParam);
                }
                catch (ConvertorException ex)
                {
                    Global.Logger.Error($"Convertor exception {ex.Message}");
                    var exceptions = ex.Message.Replace("\r\n", "\n");
                    //var message = $"[{s + 1}] [{t + 1}] [{Cx2Xg5kOption.LabelHeader} {exceptions}]";
                    var message = exceptions;
                    convertResult = new ConvertResult(Enumerable.Empty<string>(), new[] { message });   // kkk

                    // 생성 실패한 rung 따로 project 로 기록
                    cvtParam.ReviewProjectGenerator.AddRungs(prog, ils.ToArray(), ex);
                }
                catch (Exception ex)
                {
                    Global.Logger.Error($"Unknown exception {ex}");
                    throw;
                }
            }
            else if (rung.Comment.NonNullAny())
            {
                var results =
                    rung.Comment.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .SelectMany(cmt => CxtParser.SplitBlock(cmt))
                        .Select(cmt => $"{Xg5k.RungCommentCommand}\t{cmt}")
                        .ToArray();
                convertResult = new ConvertResult(results);
            }

            if (convertResult == null)
                return null;

            convertResult.Program = prog;
            convertResult.Section = sec;
            convertResult.Rung = rung;
            rung.ConvertResult = convertResult;

            return rung.ConvertResult;
        }
    }
}
