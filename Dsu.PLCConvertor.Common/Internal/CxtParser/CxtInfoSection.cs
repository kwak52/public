using Dsu.Common.Utilities.ExtensionMethods;
using System.Collections.Generic;
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
        /// 섹션에 대해서 PLC 변환
        /// </summary>
        public void Convert(ConvertParams cvtParam)
        {
#if DEBUG
            Global.Logger.Info($"SecName={Name}");
            this.EnumerateType<CxtInfoRung>()
                .Where(rung => rung.ILs.NonNullAny())
                .Where(rung => !rung.ILs[0].StartsWith("END"))
                .Iter(rung => {
                    rung.ILs.Iter(il => Global.Logger.Debug($"{il}"));
                });
#endif
            if (cvtParam.SplitBySection)
                cvtParam.ResetStartStep();

            this.EnumerateType<CxtInfoRung>()
                .Where(rung => rung.ILs.NonNullAny())
                .Where(rung => !rung.ILs[0].StartsWith("END"))
                .Iter(rung =>
                {
                    var ils = rung.ILs.Where(il => !il.StartsWith("'") && !il.StartsWith("//"));

                    if (ils.Any())
                    {
                        var s = cvtParam.SourceStartStep;
                        var t = cvtParam.TargetStartStep;

                        try
                        {
                            var convertResult = Rung2ILConvertor.ConvertFromMnemonics(ils, rung.Comment, cvtParam);
                            rung.ConvertResults = convertResult.Results;
                            rung.ConvertMessages = convertResult.Messages;
                        }
                        catch (System.Exception ex)
                        {
                            rung.ConvertMessages = new[] { $"[{s}]\t[{t}]\t{ex.Message}" };
                        }

                        cvtParam.SourceStartStep += rung.ILs.Length;

                        if (rung.ConvertResults != null)
                            cvtParam.TargetStartStep += rung.ConvertResults.Length;
                    }
                });
        }

        /// <summary>
        /// 섹션을 변환한 결과를 모아서 반환.
        /// Convert() 에 의해서 변환이 수행되고 그 결과를 따로 저장해 두었다가 CollectResults() 호출에서 모아서 반환한다.
        /// </summary>
        public IEnumerable<string> CollectResults(ConvertParams cvtParam)
        {
            var secConversion =
                this.EnumerateType<CxtInfoRung>()
                    .Where(rung => rung.ConvertResults.NonNullAny())
                    .SelectMany(rung => rung.ConvertResults)
                    ;

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
                this.EnumerateType<CxtInfoRung>()
                    .Where(rung => rung.ConvertMessages.NonNullAny())
                    .SelectMany(rung => rung.ConvertMessages)
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
