using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// ILog 가 runtime 에 debug log 를 enable/disable 하는 기능을 지원하지 않으므로, 
    /// proxy 를 통해서 enable/disable 한다.
    /// https://social.msdn.microsoft.com/forums/vstudio/en-US/0624de68-fe7f-45c0-9cd8-468d9dac844b/how-to-disable-log4net-debug-logging-during-runtime
    /// </summary>
    public class LogProxy
    {
        public ILog Logger { get; set; }
        public string Name { get { return Logger.Logger.Name; } }

        public Type Type { get; set; }

        /// <summary>
        /// runtime 에 debug 로그에 대한 enable/disable 여부
        /// </summary>
        public bool IsEnableDebug
        {
            get { return _isEnableDebug && Logger.IsDebugEnabled; }
            set
            {
                _isEnableDebug = value;
                DEBUG.WriteLine("{0}: IsEnableDebug={1}", Name, IsEnableDebug);
            }
        }
        private bool _isEnableDebug = false;

        public bool IsEnableInfo { get { return _isEnableInfo && Logger.IsInfoEnabled; } set { _isEnableInfo = value; } }
        private bool _isEnableInfo = false;

        public bool IsEnableWarn { get { return _isEnableWarn && Logger.IsWarnEnabled; } set { _isEnableWarn = value; } }
        private bool _isEnableWarn = false;

        /// <summary>
        /// 현재 등록된 logger 들 : var loggers = log4net.LogManager.GetCurrentLoggers(); 와 동일
        /// </summary>
        public static IEnumerable<LogProxy> CurrentLoggers { get { return _currentLoggers; } }
        private static List<LogProxy> _currentLoggers = new List<LogProxy>();

        private LogProxy(Type type)
        {
            Type = type;
            Logger = log4net.LogManager.GetLogger(type);
        }

        public static LogProxy GetLogProxy(Type t)
        {
            return CurrentLoggers.FirstOrDefault(l => l.Name == t.FullName);
        }

        /// <summary>
        /// proxy 된 logger 를 생성한다.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="enableDebug"></param>
        /// <returns></returns>
        public static LogProxy CreateLoggerProxy(Type t, bool? enableDebug = null)
        {
            var proxy = GetLogProxy(t);
            if (proxy == null)
            {
                proxy = new LogProxy(t);
                _currentLoggers.Add(proxy);
            }

            if (enableDebug.HasValue)
                proxy.IsEnableDebug = enableDebug.Value;

            return proxy;
        }


        [Conditional("DEBUG")]
        public void Debug(object message, Exception exception)
        {
            if (IsEnableDebug) Logger.Debug(message, exception);
        }

        [Conditional("DEBUG")]
        public void Debug(object message)
        {
            if (IsEnableDebug) Logger.Debug(message);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, params object[] args)
        {
            if (IsEnableDebug) Logger.DebugFormat(format, args);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0)
        {
            if (IsEnableDebug) Logger.DebugFormat(format, arg0);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (IsEnableDebug) Logger.DebugFormat(format, arg0, arg1);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsEnableDebug) Logger.DebugFormat(format, arg0, arg1, arg2);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (IsEnableDebug) Logger.DebugFormat(provider, format, args);
        }



        public void Error(object message) { Logger.Error(message); }
        public void Error(object message, Exception exception) { Logger.Error(message, exception); }
        public void ErrorFormat(string format, object arg0) { Logger.ErrorFormat(format, arg0); }
        public void ErrorFormat(string format, params object[] args) { Logger.ErrorFormat(format, args); }
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) { Logger.ErrorFormat(provider, format, args); }
        public void ErrorFormat(string format, object arg0, object arg1) { Logger.ErrorFormat(format, arg0, arg1); }
        public void ErrorFormat(string format, object arg0, object arg1, object arg2) { Logger.ErrorFormat(format, arg0, arg1, arg2); }
        public void Fatal(object message) { Logger.Fatal(message); }
        public void Fatal(object message, Exception exception) { Logger.Fatal(message, exception); }
        public void FatalFormat(string format, object arg0) { Logger.FatalFormat(format, arg0); }
        public void FatalFormat(string format, params object[] args) { Logger.FatalFormat(format, args); }
        public void FatalFormat(IFormatProvider provider, string format, params object[] args) { Logger.FatalFormat(provider, format, args); }
        public void FatalFormat(string format, object arg0, object arg1) { Logger.FatalFormat(format, arg0, arg1); }
        public void FatalFormat(string format, object arg0, object arg1, object arg2) { Logger.FatalFormat(format, arg0, arg1, arg2); }
        public void Info(object message) { if (IsEnableInfo) Logger.Info(message); }
        public void Info(object message, Exception exception) { if (IsEnableInfo) Logger.Info(message, exception); }
        public void InfoFormat(string format, object arg0) { if (IsEnableInfo) Logger.InfoFormat(format, arg0); }
        public void InfoFormat(string format, params object[] args) { if (IsEnableInfo) Logger.InfoFormat(format, args); }
        public void InfoFormat(IFormatProvider provider, string format, params object[] args) { if (IsEnableInfo) Logger.InfoFormat(provider, format, args); }
        public void InfoFormat(string format, object arg0, object arg1) { if (IsEnableInfo) Logger.InfoFormat(format, arg0, arg1); }
        public void InfoFormat(string format, object arg0, object arg1, object arg2) { if (IsEnableInfo) Logger.InfoFormat(format, arg0, arg1, arg2); }
        public void Warn(object message) { if (IsEnableWarn) Logger.Warn(message); }
        public void Warn(object message, Exception exception) { if (IsEnableWarn) Logger.Warn(message, exception); }
        public void WarnFormat(string format, object arg0) { if (IsEnableWarn) Logger.WarnFormat(format, arg0); }
        public void WarnFormat(string format, params object[] args) { if (IsEnableWarn) Logger.WarnFormat(format, args); }
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) { if (IsEnableWarn) Logger.WarnFormat(provider, format, args); }
        public void WarnFormat(string format, object arg0, object arg1) { if (IsEnableWarn) Logger.WarnFormat(format, arg0, arg1); }
        public void WarnFormat(string format, object arg0, object arg1, object arg2) { if (IsEnableWarn) Logger.WarnFormat(format, arg0, arg1, arg2); }
    }
}
