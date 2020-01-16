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
        internal IEnumerable<CxtInfoRung> EnumerateValidRungs() => EnumerateType<CxtInfoRung>().Where(r => r.IsValid);

        /// <summary>
        /// 섹션에 대해서 PLC 변환
        /// </summary>
        public IEnumerable<ConvertResult> Convert(ConvertParams cvtParam, CxtInfoProgram prog, int targetStartIndex)
        {
            Global.Logger.Info($"Section {Name} 변환 중...");
#if DEBUG
            var xxx = this.EnumerateValidRungs();
            xxx
                .Where(rung => !rung.ILs[0].StartsWith("END"))
                .Iter(rung => {
                    rung.ILs.Iter(il => Global.Logger.Debug($"{il}"));
                });
#endif
            Debug.Assert(prog == Parent);

            var rungs = EnumerateType<CxtInfoRung>().ToArray();
            foreach (var rung in rungs)
            {
                if (rung.IsValid)
                {
                    var result = rung.Convert(cvtParam, prog, this, targetStartIndex);
                    if (result != null)
                    {
                        targetStartIndex += result.Messages.Count() + result.Results.Count();
                        yield return result;
                    }
                }
                else
                {
                    var msgs = new[] { "현재 위치에 누락 부분(e.g 평션블록 등)이 존재할 수 있습니다." };
                    yield return new ConvertResult(Enumerable.Empty<string>(), msgs)
                    {
                        Program = prog,
                        Section = this,
                        Rung = rung
                    };
                }
            }
        }
    }
}
