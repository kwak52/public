using Dsu.Common.Utilities.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 옴론 project(.cxt) 파일을 읽어 들이기 위한 추상화된 정보
    /// cxt 파일의 계층 구조를 text block 으로 해석하기 위한 구조
    /// </summary>
    public abstract class CxtInfo
    {
        public string Key { get; private set; }
        public CxtInfo Parent { get; internal set; }
        protected CxtInfo(string key)
        {
            Key = key;
        }

        public abstract IEnumerable<CxtInfo> Children { get; }

        public IEnumerable<T> EnumerateType<T>()
        {
            return _enumerate(this).Cast<T>();

            IEnumerable<CxtInfo> _enumerate(CxtInfo start)
            {
                if (start.GetType() == typeof(T))
                    yield return start;

                var xs = start.Children.SelectMany(ch => _enumerate(ch)).ToArray();
                foreach (var x in xs)
                    yield return x;
            }
        }

#if DEBUG
        public IEnumerable<CxtInfo> EnumerateWithKey(string key)
        {
            return _enumerate(this);

            IEnumerable<CxtInfo> _enumerate(CxtInfo start)
            {
                if (start.Key == key)
                    yield return start;

                var xs = start.Children.SelectMany(ch => _enumerate(ch)).ToArray();
                foreach (var x in xs)
                    yield return x;
            }
        }
#endif


        internal abstract void ClearMyResult();

        /// <summary>
        /// PLC 프로그램 변환 도중에 발생한 메시지를 모두 clear
        /// </summary>
        public void ClearResult()
        {
            ClearMyResult();
            Children.Iter(ch => ch.ClearResult());
        }


        internal static IEnumerable<string> WrapWithProgram(string progName, IEnumerable<string> converted, int nStart)
            => wrapWithProgramHelper(progName, converted, nStart);
        internal static IEnumerable<string> WrapWithProgram(string progName, IEnumerable<string> converted)
            => wrapWithProgramHelper(progName, converted, null);
        static IEnumerable<string> wrapWithProgramHelper(string progName, IEnumerable<string> converted, int? nStart)
        {
            yield return $"[PROGRAM FILE] {progName}";

            var xs = nStart.HasValue ? AnnotateLineNumber(converted, nStart.Value) : converted;
            foreach (var x in xs)
                yield return x;

            yield return "[PROGRAM FILE END]\r\n";
        }

        internal static IEnumerable<string> AnnotateLineNumber(IEnumerable<string> converted, int nStart)
            => converted.Select((line, n) => $"{nStart + n}\t{line}");
    }
}
