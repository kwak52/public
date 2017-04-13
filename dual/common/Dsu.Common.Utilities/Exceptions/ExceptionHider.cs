using System;

namespace Dsu.Common.Utilities.Exceptions
{
    /// <summary>
    /// Swallow exception class
    /// - exception 발생하였으나, 조용히 무시하고 싶은 경우에 사용.
    /// </summary>
    public static class ExceptionHider
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static void SwallowException(Exception ex, string hint=null)
        {
            logger.FatalFormat("{0}-{1}{2}", ex.GetType().Name, ex.Message, hint == null ? "" : ": " + hint);
        }

        public static void DoSilently(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                SwallowException(ex);
            }
        }
    }
}

