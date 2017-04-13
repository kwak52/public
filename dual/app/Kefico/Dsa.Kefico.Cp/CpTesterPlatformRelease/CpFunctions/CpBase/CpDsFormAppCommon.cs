using CpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CpBase
{
    public class CpDsFormAppCommon : CpDxRibbonFormAppCommon
    {
        protected static CancellationTokenSource _cts = new CancellationTokenSource();
        public static void ResetCancellationTokenSource() { _cts = new CancellationTokenSource(); }

        public static CancellationToken CancellationToken
        {
            get
            {
                if (_cts == null)
                    throw new Exception("No cancellation token source in CpDsFormAppCommon");
                return _cts.Token;
            }
        }
    }
}
