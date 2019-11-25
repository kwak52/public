using log4net;
using System.Reactive.Subjects;

namespace Dsu.PLCConvertor.Common
{
    public class Global
    {
        public static ILog Logger { get; set; }

        public static Subject<string> UIMessageSubject = new Subject<string>();
    }

    public enum LogLevel
    {
        NONE = 0,
        FATAL,
        WARN,
        INFO,
        DEBUG,
    }
}
