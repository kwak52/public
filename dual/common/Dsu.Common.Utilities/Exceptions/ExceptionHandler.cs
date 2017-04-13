using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Dsu.Common.Utilities.Exceptions
{
    /// <summary>
    /// Exception handler methods.
    /// </summary>
    /*
using static Dsu.Common.Utilities.Exceptions.ExceptionHandler;
     * 
     */
    public static class ExceptionHandler
    {
        /// <summary>
        /// TryAction() 의 return type.
        /// try block 내의 코드가 return 값을 갖지 않는 경우의 return 값
        /// </summary>
        [ComVisible(false)]
        public class TryResult
        {
            public Exception Exception { get; }
            public bool HasException => Exception != null;
            public bool Succeeded => Exception == null;

            public TryResult(Exception ex)
            {
                Exception = ex;
            }

            public static TryResult Success = new TryResult(null);
        }


        /// <summary>
        /// TryFunc() 의 return type.
        /// try block 내의 코드가 T type 의 return 값을 갖는 경우의 return 값
        /// </summary>
        [ComVisible(false)]
        public class TryResultT<T>
            : TryResult
        {
            public T Result { get; }
            public TryResultT(T result, Exception ex)
                : base(ex)
            {
                Result = result;
            }
        }



        private static void LogException(Exception ex)
        {
            Console.WriteLine($"Got exception {ex}");
        }


        /// <summary>
        /// try block 내의 코드가 return 값을 갖는 경우에 호출
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TryResultT<T> TryFunc<T>(Func<T> func)
        {
            try
            {
                return new TryResultT<T>(func(), null);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return new TryResultT<T>(default(T), ex);
            }
        }


        /// <summary>
        /// try block 내의 코드가 return 값을 갖지 않는 경우에 호출
        /// </summary>
        /// <param name="action">try block 내에서 수행할 코드(람다 함수로 적용)</param>
        /// <returns></returns>
        public static TryResult TryAction(Action action)
        {
            try
            {                
                action();
                return TryResult.Success;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return new TryResult(ex);
            }
        }


        /// <summary>
        /// try block 내의 코드가 async 이면서 return 값을 갖는 경우에 호출
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        // http://stackoverflow.com/questions/28814267/how-do-i-pass-async-method-as-action-or-func
        public async static Task<TryResultT<T>> TryFuncAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return new TryResultT<T>(await func(), null);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return new TryResultT<T>(default(T), ex);
            }
        }

        /// <summary>
        /// try block 내의 코드가 async 이면서 return 값을 갖지 않는 경우에 호출
        /// </summary>
        /// <param name="action">try block 내에서 수행할 코드(람다 함수로 적용)</param>
        /// <returns></returns>
        public async static Task<TryResult> TryActionAsync(Func<Task> action)
        {
            try
            {
                await action();
                return TryResult.Success;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return new TryResult(ex);
            }
        }

    }
}
