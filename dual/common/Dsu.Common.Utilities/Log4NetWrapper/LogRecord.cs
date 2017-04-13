using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Core;

namespace Dsu.Common.Utilities
{
    public class LogRecord
    {
        [Browsable(false)]
        public LoggingEvent LogEntry { get; private set; }
        [DisplayName("Time")]
        public string TimeStamp { get { return LogEntry.TimeStamp.ToString("hh:mm:ss.ff"); } }
        public Level Level { get { return LogEntry.Level; } }
        [DisplayName("Sender")]
        public string LoggerName { get { return LogEntry.LoggerName; } }
        [DisplayName("Message")]
        public object MessageObject { get { return LogEntry.MessageObject; } }

        [DisplayName("t-Id")]
        public string ThreadName { get { return LogEntry.ThreadName; } }

        public LogRecord(LoggingEvent log)
        {
            LogEntry = log;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(TimeStamp);
            sb.Append("\t");
            sb.Append(Level);
            sb.Append("\t");
            sb.Append(LoggerName);
            sb.Append("\t");
            sb.Append(MessageObject);

            return sb.ToString();
        }
    }


    [ComVisible(false)]
    public class LogRecordFiledWriter<T> : IDisposable
    {
        private bool _globalContext;
        private string _field;
        private T _valueBackup;

        public LogRecordFiledWriter(string field, T value, bool globalContext=false)
        {
            _field = field;
            _globalContext = globalContext;

            if (globalContext)
            {
                var backup = GlobalContext.Properties[field];
                _valueBackup = backup == null ? default(T) : (T)backup;
                GlobalContext.Properties[field] = value;
            }
            else
            {
                var backup = ThreadContext.Properties[field];
                _valueBackup = backup == null ? default(T) : (T)backup;
                ThreadContext.Properties[field] = value;
            }
        }
        public void Dispose()
        {
            if (_globalContext)
                GlobalContext.Properties[_field] = _valueBackup;
            else
                ThreadContext.Properties[_field] = _valueBackup;
        }
    }


}
