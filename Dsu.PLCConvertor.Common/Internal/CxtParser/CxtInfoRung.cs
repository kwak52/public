﻿using System.Collections.Generic;
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
        /// Rung 에 대한 변환 메시지를 저장
        /// </summary>
        internal string[] NumberedConvertMessages = new string[]{};

        internal string[] ConvertMessages = new string[]{};

        internal IEnumerable<string> GetAllConvertMessages() => ConvertMessages.Concat(NumberedConvertMessages);

        /// <summary>
        /// 변환 중간 결과 clear/reset
        /// </summary>
        internal override void ClearMyResult()
        {
            ConvertResults = new string[]{};
            NumberedConvertMessages = new string[]{};
        }
    }
}
