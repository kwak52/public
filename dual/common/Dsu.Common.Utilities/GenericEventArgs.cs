using System;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Generic event argument decorator class
    /// </summary>
    public class GenericEventArgs : EventArgs
    {
        private EventArgs _innerEventArgs;

        public T Cast<T>() where T : EventArgs
        {
            if (_innerEventArgs is T)
                return (T)_innerEventArgs;

            return null;
        }

        public GenericEventArgs(EventArgs args)
        {
            _innerEventArgs = args;
        }
    }
}
