using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

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
