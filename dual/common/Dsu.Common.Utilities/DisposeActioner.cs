// http://stackoverflow.com/questions/917551/func-delegate-with-no-return-type

using System;

namespace Dsu.Common.Utilities
{
    public class DisposeActioner : IDisposable
    {
        public Action EpilogueAction { get; set; }        // Func<> 의 경우, return type 과 argument 가 필수 이므로 Action 을 사용

        public DisposeActioner(Action f)
        {
            EpilogueAction = f;
        }

        public void Dispose()
        {
            EpilogueAction();
        }
    }
}
