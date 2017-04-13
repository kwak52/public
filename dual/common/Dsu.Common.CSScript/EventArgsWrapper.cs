using System;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Script 함수에서 받을 event argument 를 정의한다.
    /// <br/> - 실제 script 에서는 object 로 받아서 ScriptEventArgs 로 casting 해서 보아야 한다.
    /// <br/> - GenericEventArgs, ITaskArgs 에서 상속
    /// </summary>
    public class ScriptEventArgs : GenericEventArgs, ITaskArgs
    {
        public object Sender { get; private set; }
        public ScriptEventArgs(object sender, EventArgs args)
            : base(args)
        {
            Sender = sender;
        }
    }
}
