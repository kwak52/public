using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 옴론 project(.cxt) 파일에서 section 을 표현하기 위한 정보
    /// section 은 다수의 rung 을 포함한다.
    /// </summary>
    public class CxtInfoSection : CxtInfo
    {
        public string Name { get; internal set; }
        public List<CxtInfoRung> _rungs = new List<CxtInfoRung>();
        public IEnumerable<CxtInfoRung> Rungs => _rungs;
        public void AddRung(CxtInfoRung rung)
        {
            rung.Parent = this;
            _rungs.Add(rung);
        }

        internal CxtInfoSection(string name)
            : base("Section")
        {
            Name = name;
        }
        public override IEnumerable<CxtInfo> Children { get { return Rungs; } }
        public CxtInfoProgram ParentProgram => Parent as CxtInfoProgram;
        internal override void ClearMyResult() {}

        /// <summary>
        /// Section 내에서 valid 한 rung 만을 반환.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<CxtInfoRung> EnumerateValidRungs()
        {
            return this.EnumerateType<CxtInfoRung>()
                .Where(rung => rung.ILs.NonNullAny())
                .Where(rung => rung.ILs.Any(il => ! il.StartsWith("\"\"")))
                ;
        }

        /// <summary>
        /// 섹션에 대해서 PLC 변환
        /// </summary>
        public IEnumerable<ConvertResult> Convert(ConvertParams cvtParam, CxtInfoProgram prog, int targetStartIndex)
        {
            Global.Logger.Info($"Section {Name} 변환 중...");
#if DEBUG
            this.EnumerateValidRungs()
                .Where(rung => !rung.ILs[0].StartsWith("END"))
                .Iter(rung => {
                    rung.ILs.Iter(il => Global.Logger.Debug($"{il}"));
                });
#endif
            if (cvtParam.SplitBySection)
            {
                targetStartIndex = 1;
                cvtParam.ResetStartStep();
            }

            Debug.Assert(prog == Parent);

            return this.EnumerateValidRungs()
                .Select(rung =>
                {
                    var result = rung.Convert(cvtParam, prog, this, targetStartIndex);
                    targetStartIndex += result.Messages.Count() + result.Results.Count();
                    return result;
                });
        }

        /// <summary>
        /// 섹션을 변환한 결과를 모아서 반환.
        /// Convert() 에 의해서 변환이 수행되고 그 결과를 따로 저장해 두었다가 CollectResults() 호출에서 모아서 반환한다.
        /// </summary>
        public IEnumerable<string> CollectResults(ConvertParams cvtParam)
        {
            if (cvtParam.TargetType != PLCVendor.LSIS)
                throw new ConvertorException($"Not supported PLC vendor type : {cvtParam.TargetType}");

            var cmtcmd = Xg5k.RungCommentCommand;

            var secConversion =
                this.EnumerateValidRungs()
                    .SelectMany(rung =>
                    {
                        if (rung.ConvertResults.IsNullOrEmpty() && rung.ConvertResult.Messages.IsNullOrEmpty())
                        {
                            var sampling = string.Join("\t", rung.ILs.Take(5));
                            if (rung.EffectiveILs.NonNullAny()) // comment 를 제외한 유효 IL 문장이 있는 경우
                            {
                                rung.ConvertResult.Messages.AddRange(new[] {
                                    $"{cmtcmd}\t::::::::::::::::RUNG 변환에 실패하였습니다::::::::::::::::::",
                                    $"{cmtcmd}\t{sampling}\t...",
                                });
                            }
                            else // comment 만 존재하는 section
                            {
                                Debug.Assert(rung.ILs.All(il => il.StartsWith("'")));
                                rung.ConvertResult.Results =
                                    rung.ILs
                                    .SelectMany(il => CxtParser.SplitBlock(il))
                                    .Select(il => il.TrimStart(new[] { '\'' }))
                                    .Select(il => $"{cmtcmd}\t{il}")
                                    .ToList()
                                    ;
                            }

                            return rung.ConvertResults;
                        }

                        if (! Cx2Xg5kOption.AddMessagesToLabel)
                            return rung.ConvertResults;

                        // 변환 중 발생한 message 를 설명문에 추가해서 반환.
                        var comments = rung.GetAllConvertMessages().Select(msg => $"{cmtcmd}\t{msg}");
                        if (comments.Any())
                            Console.WriteLine("");
                        return comments.Concat(rung.ConvertResults);
                    });

            IEnumerable<string> annotated = null;
            var nStart = cvtParam.TargetStartStep;
            if (cvtParam.SplitBySection)
                annotated = CxtInfo.WrapWithProgram($"{ParentProgram.Name}:{Name}", secConversion, nStart);
            else
                annotated = CxtInfo.AnnotateLineNumber(secConversion, nStart);

            cvtParam.TargetStartStep += annotated.Count();
            return annotated;
        }

        /// <summary>
        /// 섹션을 변환시 발생한 메시지를 모아서 반환.
        /// Convert() 에 의해서 변환 수행시 발생되는 메시지를 따로 저장해 두었다가 CollectMessages() 호출에서 모아서 반환한다.
        /// </summary>
        public IEnumerable<string> CollectMessages(ConvertParams cvtParam)
        {
            var secMessages =
                this.EnumerateValidRungs()
                    .Where(rung => rung.ConvertResult.Messages.NonNullAny())
                    .SelectMany(rung => rung.ConvertResult.Messages)
                    ;

            IEnumerable<string> annotated = null;
            var nStart = cvtParam.TargetStartStep;
            if (cvtParam.SplitBySection)
                annotated = CxtInfo.WrapWithProgram($"{ParentProgram.Name}:{Name}", secMessages);
            else
                annotated = secMessages;    // CxtInfo.AnnotateLineNumber(secMessages, nStart);

            cvtParam.TargetStartStep += annotated.Count();
            return annotated;
        }
    }
}
