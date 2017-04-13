using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Dsu.Driver.Util.Emergency
{
    public enum ErrorCodes
    {
        UnknownError = 0,
        APP_InternalError,      // internal implementation error
        APP_ConfigurationError,
        APP_OperationCanceled,
        APP_NotAuthentificated,

        NET_OpenError,
        NET_NotRespondingError,

        USR_OperationCancel,

        DEV_PaixError,
        DEV_UdioError,
        DEV_SorensenError,
        DEV_DaqError,
        DEV_UpsError,
        DEV_HiokiError,
    }

    public class ExceptionWithCode : Exception
    {
        public static Subject<ExceptionWithCode> ExceptionWithCodeSubject = new Subject<ExceptionWithCode>();
        public static bool IsFatalErrorOccurred { get; set; }

        public ErrorCodes ErrorCode { get; set; }
        public Exception OriginalException { get; private set; }
        protected ExceptionWithCode(ErrorCodes errorCode, string message, Exception originalException)
            : base(message)
        {
            ErrorCode = errorCode;
            OriginalException = originalException;
            Log4NetWrapper.gLogger.Error(message);
        }
        public static ExceptionWithCode Create(ErrorCodes errorCode, string message, Exception originalException=null)
        {
            var ex = new ExceptionWithCode(errorCode, message, originalException);
            ExceptionWithCodeSubject.OnNext(ex);
            return ex;
        }
        public static ExceptionWithCode CreateFatal(ErrorCodes errorCode, string message, Exception originalException = null)
        {
            var ex = Create(errorCode, message, originalException);
            IsFatalErrorOccurred = true;
            return ex;
        }
    }
}
