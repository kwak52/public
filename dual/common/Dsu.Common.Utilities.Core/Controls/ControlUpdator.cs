using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities
{
    [ComVisible(false)]
    public class ControlUpdator<T> : IDisposable
    {
        private string m_strBeginUpdate = "BeginUpdate";
        private string m_strEndUpdate = "EndUpdate";
        protected bool m_bDisposed = false;
        protected T m_Control;

        public ControlUpdator(T control) : this(control, "BeginUpdate", "EndUpdate") { }
        public ControlUpdator(T control, string begin, string end)
        {
            if (!RTTI.HasMethod_p(control.GetType(), begin) || !RTTI.HasMethod_p(control.GetType(), end))
                throw new ArgumentException(String.Format("Update method name error on control [{0}] : check {1} or {2}", control, begin, end));

            m_Control = control;
            m_strBeginUpdate = begin;
            m_strEndUpdate = end;
            m_Control.GetType().InvokeMember(m_strBeginUpdate, BindingFlags.InvokeMethod, null, m_Control, null);
        }

        public void Dispose()
        {
            if (!m_bDisposed)
            {
                m_Control.GetType().InvokeMember(m_strEndUpdate, BindingFlags.InvokeMethod, null, m_Control, null);
                m_bDisposed = true;
            }
        }
    }
}
