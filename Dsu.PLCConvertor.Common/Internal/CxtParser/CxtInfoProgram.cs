using Dsu.Common.Utilities.ExtensionMethods;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 옴론 CXT 파일의 Program 정보
    /// </summary>
    public class CxtInfoProgram : CxtInfo
    {
        public string Name { get; private set; }
        CxtInfoLocalVariables _localVariables;

        /// <summary>
        /// 프로그램 local 변수 선언부
        /// </summary>
        public CxtInfoLocalVariables LocalVariables {
            get => _localVariables;
            set {
                Debug.Assert(_localVariables == null);
                _localVariables = value;
                _localVariables.Parent = this;
            }
        }


        List<CxtInfoSection> _sections = new List<CxtInfoSection>();
        public IEnumerable<CxtInfoSection> Sections => _sections;
        internal CxtInfoProgram(string name)
            : base("Program")
        {
            Name = name;
        }

        public void AddSection(CxtInfoSection section)
        {
            section.Parent = this;
            _sections.Add(section);
        }

        public override IEnumerable<CxtInfo> Children
        {
            get
            {
                if (_localVariables == null)
                    return _sections;
                return _sections.Cast<CxtInfo>().Concat(new[] { _localVariables });
            }
        }
        internal override void ClearMyResult() { }


        /// <summary>
        /// 프로그램에 대해서 PLC 변환
        /// </summary>
        public void Convert(ConvertParams cvtParam)
        {
            // 변환 이전에 초기화 수행
            cvtParam.ResetStartStep();
            ClearResult();

            // program 을 구성하는 각 section 들을 모두 변환.  변환 결과는 중간 결과로 저장하고 있음
            Sections.Iter(sec => sec.Convert(cvtParam));
        }

        /// <summary>
        /// 프로그램을 변환한 결과를 모아서 반환.
        /// Convert() 에 의해서 변환이 수행되고 그 결과를 따로 저장해 두었다가 CollectResults() 호출에서 모아서 반환한다.
        /// </summary>
        public IEnumerable<string> CollectResults(ConvertParams cvtParam)
        {
            cvtParam.ResetStartStep();

            var progConversion =
                this.EnumerateType<CxtInfoSection>()
                    .SelectMany(sec =>
                    {
                        if (cvtParam.SplitBySection)
                            cvtParam.ResetStartStep();
                        return sec.CollectResults(cvtParam);
                    })
                    .ToArray();

            if (cvtParam.SplitBySection)
                return progConversion;

            var annotated = CxtInfo.WrapWithProgram(Name, progConversion);

            return annotated;
        }

        /// <summary>
        /// 프로그램을 변환시 발생한 메시지를 모아서 반환.
        /// Convert() 에 의해서 변환 수행시 발생되는 메시지를 따로 저장해 두었다가 CollectMessages() 호출에서 모아서 반환한다.
        /// </summary>
        public IEnumerable<string> CollectMessages(ConvertParams cvtParam)
        {
            cvtParam.ResetStartStep();

            var progMessages =
                this.EnumerateType<CxtInfoSection>()
                    .SelectMany(sec =>
                    {
                        if (cvtParam.SplitBySection)
                            cvtParam.ResetStartStep();
                        return sec.CollectMessages(cvtParam);
                    })
                    .ToArray();

            if (cvtParam.SplitBySection)
                return progMessages;

            var annotated = CxtInfo.WrapWithProgram(Name, progMessages);

            return annotated;
        }

    }
}
