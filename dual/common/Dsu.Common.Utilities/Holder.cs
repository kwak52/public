using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// http://stackoverflow.com/questions/2760087/storing-a-reference-to-an-object-in-c-sharp
    /// ref can't be stored.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ComVisible(false)]
    public class ValueReferer<T>
    {
        private Func<T> getter;
        private Action<T> setter;
        public ValueReferer(Func<T> getter, Action<T> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }
        public T Value
        {
            get { return getter(); }
            set { setter(value); }
        }
    }

    /// <summary>
    /// see CBooleanEnabler also
    /// </summary>
    [ComVisible(false)]
    public class BooleanHolder : ValueReferer<bool>
    {
        public BooleanHolder(Func<bool> getter, Action<bool> setter) : base(getter, setter) { }
    }


    [ComVisible(false)]
    public class BooleanSetter : IDisposable
    {
        private BooleanHolder _valueHolder;
        private bool _valueBackup;
        public BooleanSetter(BooleanHolder holder, bool val)
        {
            _valueBackup = holder.Value;
            _valueHolder = holder;
            holder.Value = val;
        }

        public void Dispose()
        {
            _valueHolder.Value = _valueBackup;
        }
    }
}
