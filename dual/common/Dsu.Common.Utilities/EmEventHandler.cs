using System;

namespace Dsu.Common.Utilities
{
    public static class EmEventHandler
    {
        public static void Handle(this EventHandler eventHandler, object sender, EventArgs eventArgs)
        {
            var handler = eventHandler;
            if (handler != null)
                handler(sender, eventArgs);
        }

        public static void Handle<T>(this EventHandler<T> eventHandler, object sender, T eventArgs)
        {
            var handler = eventHandler;
            if ( handler != null )
                handler(sender, eventArgs);
        }
    }
}
