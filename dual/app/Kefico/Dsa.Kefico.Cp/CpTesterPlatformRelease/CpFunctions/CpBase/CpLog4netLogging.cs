using DevExpress.XtraEditors;
using log4net;
using System;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Windows.Forms;
using static Dsu.Driver.Util.DriverLog4netLogging;

namespace CpBase
{
    /// <summary>
    /// using static CpFunctionDefault.Util.CpLog4netLogging
    /// </summary>
    public static class CpLog4netLogging
    {
        public static ILog Logger { get; set; }

        public static Subject<Tuple<DriverLogLevel, string>> MessageSubject = new Subject<Tuple<DriverLogLevel, string>>();

        public static void LogDebug(string message)
        {
            Logger.Debug(message);
            Trace.WriteLine(message);
            MessageSubject.OnNext(Tuple.Create(DriverLogLevel.Debug, message));
        }

        public static void LogInfo(string message)
        {
            Logger.Info(message);
            Trace.WriteLine(message);
            MessageSubject.OnNext(Tuple.Create(DriverLogLevel.Info, message));
        }

        public static void LogError(string message)
        {
            Logger.Error(message);
            Trace.WriteLine(message);
            MessageSubject.OnNext(Tuple.Create(DriverLogLevel.Error, message));
        }

        public static void LogErrorWithMessageBox(string message)
        {
            Logger.Error(message);
            Trace.WriteLine(message);
            MessageSubject.OnNext(Tuple.Create(DriverLogLevel.Error, message));
            ShowMessageBox(message);
        }

        public static void LogErrorWithMessageBox(string message, Exception ex)
        {
            var fullMessage = $"{message}\r\nException={ex}";
            Logger.Error(fullMessage);
            Trace.WriteLine(fullMessage);
            MessageSubject.OnNext(Tuple.Create(DriverLogLevel.Error, fullMessage));
            ShowMessageBox($"{message}\r\n{ex.Message}");
        }


        public static void ShowMessageBox(string message, string caption = "ERROR", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            XtraMessageBox.Show(message, caption, buttons, icon);
        }

        public static DialogResult AskMessageBox(string message, string caption = "SELECT", MessageBoxButtons buttons = MessageBoxButtons.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return XtraMessageBox.Show(message, caption, buttons, icon);
        }

        public static void LogDebugRobot(string message)
        {
            LogDebug(message);
            RobotMessageSubject.OnNext(Tuple.Create(DriverLogLevel.Debug, message));
        }

        public static void LogInfoRobot(string message)
        {
            LogInfo(message);
            RobotMessageSubject.OnNext(Tuple.Create(DriverLogLevel.Info, message));
        }

        public static void LogErrorRobot(string message)
        {
            LogError(message);
            RobotMessageSubject.OnNext(Tuple.Create(DriverLogLevel.Error, message));
        }
    }
}
