using System;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmRx
    {
        /// System.Reactive.Subjects.Subject.Subscribe 대신 System.ObservableExtensions.Subscribe 를 강제로 호출하기 위한 exntension method
        public static IDisposable SubscribeRxEx<T>(this IObservable<T> source, Action<T> onNext)
        {
            // Assembly System.Reactive.Core.dll 에 정의된 System.ObservableExtensions
            return ObservableExtensions.Subscribe(source, onNext);
        }
    }
}
