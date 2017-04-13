using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dsu.Common.Utilities.Core.ExtensionMethods
{
    public static class EmCancellationToken
    {
        /// 이미 존재하는 cancellation token 에 지정된 시간 이후에 cancel 하는 time out 기능을 부가한 token source 를 생성해서 반환한다.
        public static CancellationTokenSource CreateLinkedTimeoutTokenSource(this CancellationToken token, int delayMilli)
        {
            CancellationTokenSource ctsTimeOut = new CancellationTokenSource();
            ctsTimeOut.CancelAfter(delayMilli);
            return CancellationTokenSource.CreateLinkedTokenSource(token, ctsTimeOut.Token);
        }
        /// 이미 존재하는 cancellation token 에 지정된 시간 이후에 cancel 하는 time out 기능을 부가한 token source 를 생성해서 반환한다.
        public static CancellationTokenSource CreateLinkedTimeoutTokenSource(this CancellationToken token, TimeSpan ts) => CreateLinkedTimeoutTokenSource(token, (int)ts.TotalMilliseconds);
    }
}
