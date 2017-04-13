using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Format supported Trace replacement.
    /// </summary>
    public static class FmtTrace
    {
        static private string FormatString(string format, params object[] args)
        {
            if (args.Count() == 0)
                return format;
            return String.Format(format, args);
        }

        [Conditional("TRACE")]
        static public void Write(string format, object arg0) { Trace.Write(String.Format(format, arg0)); }
        [Conditional("TRACE")]
        static public void Write(string format, params object[] args)  { Trace.Write(String.Format(format, args)); }
        [Conditional("TRACE")]
        static public void Write(IFormatProvider provider, string format, params object[] args)   { Trace.Write(String.Format(provider, format, args)); }
        [Conditional("TRACE")]
        static public void Write(string format, object arg0, object arg1)   { Trace.Write(String.Format(format, arg0, arg1)); }
        [Conditional("TRACE")]
        static public void Write(string format, object arg0, object arg1, object arg2)   { Trace.Write(String.Format(format, arg0, arg1, arg2)); }

        [Conditional("TRACE")]
        static public void WriteLine(string msg) { Trace.WriteLine(msg); }
        [Conditional("TRACE")]
        static public void WriteLine(string format, object arg0) { Trace.WriteLine(String.Format(format, arg0)); }
        [Conditional("TRACE")]
        static public void WriteLine(string format, params object[] args) { Trace.WriteLine(String.Format(format, args)); }
        [Conditional("TRACE")]
        static public void WriteLine(IFormatProvider provider, string format, params object[] args) { Trace.WriteLine(String.Format(provider, format, args)); }
        [Conditional("TRACE")]
        static public void WriteLine(string format, object arg0, object arg1) { Trace.WriteLine(String.Format(format, arg0, arg1)); }
        [Conditional("TRACE")]
        static public void WriteLine(string format, object arg0, object arg1, object arg2) { Trace.WriteLine(String.Format(format, arg0, arg1, arg2)); }

        [Conditional("TRACE")]
        static public void WriteIf(bool b, string format, params object[] args) { Trace.WriteIf(b, String.Format(format, args)); }
        [Conditional("TRACE")]
        static public void WriteLineIf(bool b, string format, params object[] args) { Trace.WriteLineIf(b, String.Format(format, args)); }

        [Conditional("TRACE")]
        static public void Write(object obj) { Trace.Write(obj); }
        [Conditional("TRACE")]
        static public void WriteIf(bool b, object obj) { Trace.WriteIf(b, obj); }
        [Conditional("TRACE")]
        static public void WriteLine(object obj) { Trace.WriteLine(obj); }
        [Conditional("TRACE")]
        static public void WriteLineIf(bool b, object obj) { Trace.WriteLineIf(b, obj); }
    }
}
