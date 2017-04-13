using System;
using System.Reactive.Subjects;

namespace Dsu.Driver.Util
{
    public static class DriverLog4netLogging
    {
        public enum DriverLogLevel
        {
            Debug = 0,
            Info,
            Warning,
            Error, 
        }
        public static Subject<Tuple<DriverLogLevel, string>> RobotMessageSubject = new Subject<Tuple<DriverLogLevel, string>>();
    }
}
