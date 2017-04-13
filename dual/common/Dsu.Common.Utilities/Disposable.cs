using System;
using System.IO;

namespace Dsu.Common.Utilities
{    
    public static class Disposable
    {
        private class DummyDisposable : IDisposable
        {
            public void Dispose() { }
        }
        public static IDisposable EmptyInstance { get {  return new DummyDisposable(); } }

        public static void DisposeSafely(this IDisposable disposable)
        {
            if ( disposable != null )
                disposable.Dispose();
        }
    }




    #region =====> Code Snippets
    /* Snippets for base class
     * 
private bool _disposed;

~DisposablePatternSample()
{
    Dispose(false);     // false : 암시적 dispose 호출
}
public void Dispose()
{
    Dispose(true);      // true : 명시적 dispose 호출
    GC.SuppressFinalize(this);  // 사용자에 의해서 명시적으로 dispose 되었으므로, GC 는 이 객체에 대해서 손대지 말것을 알림.
}

protected virtual void Dispose(bool disposing)
{
    if (_disposed)
        return;

    if (disposing)
    {
    }

    _disposed = true;
}
     */

    /* Snippets for derived class
     * 
private bool _disposed;

protected override void Dispose(bool disposing)
{
    if (!_disposed)
    {
        if (disposing)
        {
	        // free other managed objects that implement IDisposable only
        }

        _disposed = true;
    }

    base.Dispose(disposing);
}
     */





    /// <summary>
    /// IDisposable 을 이용한 dispose 사용 패턴
    /// <para/> - 닷넷의 Finalizer 와 IDisposable 인터페이스를 함께 사용하여 Dispose 메서드 호출을 통한 명시적인 자원 해제와 Finalizer 를 이용한 암시적인 자원 해제를 동시에 지원하는 것이 바로 Dispose 패턴이다.
    /// <para/> - http://www.simpleisbest.net/post/2011/08/22/Dispose-Pattern-Basic.aspx
    /// <para/> - https://lostechies.com/chrispatterson/2012/11/29/idisposable-done-right/
    /// <para/> - http://www.codeproject.com/Articles/29534/IDisposable-What-Your-Mother-Never-Told-You-About
    /// </summary>
    /// Control 에 대한 dispose 처리를 override 하려면 다음과 같이 수행한다.
    /// <c>
    /// Disposed += (sender, args) =>
    /// {
    ///     FmtTrace.WriteLine("I am disposing");
    /// };
    /// </c>    
    #endregion
    public class DisposablePatternSample : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Finalizer 에 의한 호출.  reference counter 값이 0 된 후에, GC 에 의해서 호출됨
        /// </summary>
        ~DisposablePatternSample()
        {
            Dispose(false);     // false : 암시적 dispose 호출
        }
        public void Dispose()
        {
            Dispose(true);      // true : 명시적 dispose 호출
            GC.SuppressFinalize(this);  // 사용자에 의해서 명시적으로 dispose 되었으므로, GC 는 이 객체에 대해서 손대지 말것을 알림.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing">명시적(using() 구문 사용 포함) Dispose() 호출시 true, Finalizer 에서 암시적으로 호출시 false</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                /*
                 * free other managed objects that implement IDisposable only
                 */
                //_stream.Dispose();
            }

            /*
             * release any unmanaged objects
             * set the object references to null
             */

            _disposed = true;
        }
    }


    public class SubDisposableClass : DisposablePatternSample
    {
        private bool _disposed;

        // a finalizer is not necessary, as it is inherited from
        // the base class

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    /*
                     * free other managed objects that implement IDisposable only
                     */
                }

                /*
                 * release any unmanaged objects
                 * set the object references to null
                 */

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
