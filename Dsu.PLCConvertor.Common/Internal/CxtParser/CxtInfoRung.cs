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
        internal string[] ConvertResults = new string[]{};

        /// <summary>
        /// Rung 변환 중 발생한 경고/알림 메시지 저장
        /// </summary>
        internal string[] ConvertWarnMessages = new string[]{};

        /// <summary>
        /// Rung 변환 중 발생한 에러 메시지 저장
        /// </summary>
        internal string[] ConvertErrorMessages = new string[]{};

        internal IEnumerable<string> GetAllConvertMessages() => ConvertErrorMessages.Concat(ConvertWarnMessages);

        /// <summary>
        /// 변환 중간 결과 clear/reset
        /// </summary>
        internal override void ClearMyResult()
        {
            ConvertResults = new string[]{};
            ConvertWarnMessages = new string[]{};
        }
    }
}
