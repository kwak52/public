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
        IEnumerable<CxtInfoRung> EnumerateValidRungs()
        {
            return this.EnumerateType<CxtInfoRung>()
                .Where(rung => rung.ILs.NonNullAny())
                .Where(rung => rung.ILs.Any(il => ! il.StartsWith("\"\"")))
                ;
        }

        /// <summary>
        /// 섹션에 대해서 PLC 변환
        /// </summary>
        public void Convert(ConvertParams cvtParam)
        {
#if DEBUG
            Global.Logger.Info($"SecName={Name}");
            this.EnumerateValidRungs()
                .Where(rung => !rung.ILs[0].StartsWith("END"))
                .Iter(rung => {
                    rung.ILs.Iter(il => Global.Logger.Debug($"{il}"));
                });
#endif
            if (cvtParam.SplitBySection)
                cvtParam.ResetStartStep();

            var prog = Parent as CxtInfoProgram;

            this.EnumerateValidRungs()
                .Iter(rung =>
                {
                    // "A  F0" 가 맨 끝에 오는 경우는 rung 주석이 포함된 경우이다.
                    var ils = rung.ILs.Where(il => !il.StartsWith("'") && !il.StartsWith("//") && il != "\"\"" && il != "A  F0");

                    if (ils.Any())
                    {
                        var s = cvtParam.SourceStartStep;
                        var t = cvtParam.TargetStartStep;

                        try
                        {
                            var convertResult = Rung2ILConvertor.ConvertFromMnemonics(ils, rung.Comment, cvtParam);
                            rung.ConvertResults = convertResult.Results;
                            rung.NumberedConvertMessages = convertResult.NumberedMessages.Clone() as string[];
                            rung.ConvertMessages = convertResult.Messages.Clone() as string[];
                        }
                        catch (System.Exception ex)
                        {
                            rung.NumberedConvertMessages = new[] { $"[{s+1}] [{t+1}] [{Cx2Xg5kOption.LabelHeader} {ex.Message}]" };   // kkk
                            rung.ConvertMessages = rung.ConvertMessages.Concat(new[] { $"{Cx2Xg5kOption.LabelHeader} {ex.Message}" }).ToArray();

                            // 생성 실패한 rung 따로 project 로 기록
                            cvtParam.ReviewProjectGenerator.AddRungs(prog, ils.ToArray());
                        }

                        cvtParam.SourceStartStep += rung.ILs.Length;

                        if (rung.ConvertResults != null)
                            cvtParam.TargetStartStep += rung.ConvertResults.Length;
                    }
                    else if (rung.Comment.NonNullAny() )
                    {
                        rung.ConvertResults =
                         
                        rung.Comment.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                            .SelectMany(cmt => CxtParser.SplitBlock(cmt))
                            .Select(cmt => $"{Xg5k.RungCommentCommand}\t{cmt}")
                            .ToArray();
                    }
                });
        }

        /// <summary>
        /// 섹션을 변환한 결과를 모아서 반환.
        /// Convert() 에 의해서 변환이 수행되고 그 결과를 따로 저장해 두었다가 CollectResults() 호출에서 모아서 반환한다.
        /// </summary>
        public IEnumerable<string> CollectResults(ConvertParams cvtParam)
        {
            if (cvtParam.TargetType != PLCVendor.LSIS)
                throw new Exception($"Not supported PLC vendor type : {cvtParam.TargetType}");

            var cmtcmd = Xg5k.RungCommentCommand;

            var secConversion =
                this.EnumerateValidRungs()
                    .SelectMany(rung =>
                    {
                        if (rung.ConvertResults.IsNullOrEmpty() && rung.NumberedConvertMessages.IsNullOrEmpty() && rung.ConvertMessages.IsNullOrEmpty())
                        {
                            var sampling = string.Join("\t", rung.ILs.Take(5));
                            if (rung.EffectiveILs.NonNullAny()) // comment 를 제외한 유효 IL 문장이 있는 경우
                            {
                                rung.ConvertResults = new[] {
                                    $"{cmtcmd}\t::::::::::::::::RUNG 변환에 실패하였습니다::::::::::::::::::",
                                    $"{cmtcmd}\t{sampling}\t...",
                                };
                            }
                            else // comment 만 존재하는 section
                            {
                                Debug.Assert(rung.ILs.All(il => il.StartsWith("'")));
                                rung.ConvertResults =
                                    rung.ILs
                                    .SelectMany(il => CxtParser.SplitBlock(il))
                                    .Select(il => il.TrimStart(new[] { '\'' }))
                                    .Select(il => $"{cmtcmd}\t{il}")
                                    .ToArray()
                                    ;
                            }

                            return rung.ConvertResults;
                        }

                        if (! Cx2Xg5kOption.AddMessagesToLabel)
                            return rung.ConvertResults;

                        // 변환 중 발생한 message 를 설명문에 추가해서 반환.
                        var comments = rung.GetAllConvertMessages().Select(msg => $"{cmtcmd}\t{msg}");
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
                    .Where(rung => rung.NumberedConvertMessages.NonNullAny())
                    .SelectMany(rung => rung.NumberedConvertMessages)
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
