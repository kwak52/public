using System;

namespace Dsu.Common.Utilities.Core.ExtensionMethods
{
    public static partial class EmLinq
    {
        /// <summary>
        /// 두개의 function 을 합성
        /// Real world functional progarmming.pdf, pp.162
        /// </summary>
        public static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g)
        {
            return (x) => g(f(x));
        }
    }
}
