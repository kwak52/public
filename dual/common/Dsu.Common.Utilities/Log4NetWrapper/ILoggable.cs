using System;
using System.Runtime.InteropServices;
using log4net.Core;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Logging 기능을 가지는 interface
    /// </summary>
    [ComVisible(false)]
    public interface ILoggable
    {
        LogEntryManager LogEntryManager { get; }
        ILogWindow LogWindow { get; set; }
        void AddLogEntry(LoggingEvent logEntry);
    }

    /// <summary>
    /// Logging 출력 창이 구현해야 할 interface
    /// </summary>
    [ComVisible(false)]
    public interface ILogWindow
    {
        [Obsolete("Use AppendLog")]
        void Display(int size);
        void AppendLog(LoggingEvent logEntry, int size);
    }
}
