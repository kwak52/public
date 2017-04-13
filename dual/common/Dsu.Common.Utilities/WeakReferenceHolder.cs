using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Usage Example
    /// <code>
    ///     private WeakReferenceHolder&lt;IActuator&gt; _container = new WeakReferenceHolder&lt;IActuator&gt;(null);
    ///     public IActuator Container { get { return _container.Target; } set { _container.Target = value; } }
    /// </code>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ComVisible(false)]
    public class WeakReferenceHolder<T> where T : class
    {
        private WeakReference<T> _weakReference;
        public WeakReferenceHolder(T target, bool trackResurrection=false) { _weakReference = new WeakReference<T>(target, trackResurrection); }

        public T Target
        {
            get
            {
                if (_weakReference == null)
                    return null;
                T target = null;
                return _weakReference.TryGetTarget(out target) ? target : null;
            }
            set { _weakReference.SetTarget(value); }
        }
    }
}
