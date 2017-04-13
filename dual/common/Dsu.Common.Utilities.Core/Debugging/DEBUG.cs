using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public static partial class DEBUG
    {
        static private string FormatString(string format, params object[] args)
        {
            if (args.Count() == 0)
                return format;
            return String.Format(format, args);
        }

        [Conditional("DEBUG")]
        static public void Write(string format, object arg0) { Debug.Write(String.Format(format, arg0)); }
        [Conditional("DEBUG")]
        static public void Write(string format, params object[] args)  { Debug.Write(String.Format(format, args)); }
        [Conditional("DEBUG")]
        static public void Write(IFormatProvider provider, string format, params object[] args)   { Debug.Write(String.Format(provider, format, args)); }
        [Conditional("DEBUG")]
        static public void Write(string format, object arg0, object arg1)   { Debug.Write(String.Format(format, arg0, arg1)); }
        [Conditional("DEBUG")]
        static public void Write(string format, object arg0, object arg1, object arg2)   { Debug.Write(String.Format(format, arg0, arg1, arg2)); }

        [Conditional("DEBUG")]
        static public void WriteLine(string msg) { Debug.WriteLine(msg); }
        [Conditional("DEBUG")]
        static public void WriteLine(string format, object arg0) { Debug.WriteLine(String.Format(format, arg0)); }
        [Conditional("DEBUG")]
        static public void WriteLine(string format, params object[] args) { Debug.WriteLine(String.Format(format, args)); }
        [Conditional("DEBUG")]
        static public void WriteLine(IFormatProvider provider, string format, params object[] args) { Debug.WriteLine(String.Format(provider, format, args)); }
        [Conditional("DEBUG")]
        static public void WriteLine(string format, object arg0, object arg1) { Debug.WriteLine(String.Format(format, arg0, arg1)); }
        [Conditional("DEBUG")]
        static public void WriteLine(string format, object arg0, object arg1, object arg2) { Debug.WriteLine(String.Format(format, arg0, arg1, arg2)); }

        [Conditional("DEBUG")]
        static public void WriteIf(bool b, string format, params object[] args) { Debug.WriteIf(b, String.Format(format, args)); }
        [Conditional("DEBUG")]
        static public void WriteLineIf(bool b, string format, params object[] args) { Debug.WriteLineIf(b, String.Format(format, args)); }

        [Conditional("DEBUG")]
        static public void Write(object obj) { Debug.Write(obj); }
        [Conditional("DEBUG")]
        static public void WriteIf(bool b, object obj) { Debug.WriteIf(b, obj); }
        [Conditional("DEBUG")]
        static public void WriteLine(object obj) { Debug.WriteLine(obj); }
        [Conditional("DEBUG")]
        static public void WriteLineIf(bool b, object obj) { Debug.WriteLineIf(b, obj); }

        static public string CallStackGetNthName(int nFromTop)
        {
            StackFrame sf = CallStackGetNth(nFromTop + 1);  // +1 : to exclude 'CallStackGetNthName' itself
            if (sf == null)
                return null;
            return sf.GetMethod().Name;
        }

        static public StackFrame CallStackGetNth(int nFromTop)
        {
            StackTrace st = new StackTrace(true);
            StackFrame[] sfs = st.GetFrames();
            if (sfs.Length - 1 <= nFromTop)         // -1 and returning +1 : to exclude 'CallStackGetNth' itself
                return null;
            return sfs[nFromTop + 1];
        }

        static public List<StackFrame> CallStackGetAllFrames() { return CallStackGetAllFrames(1, true); }
        static public List<StackFrame> CallStackGetAllFrames(int skipFrames/*=1*/, bool fNeedFileInfo/*=true*/)
        {
            StackTrace st = new StackTrace(skipFrames, fNeedFileInfo);
            List<StackFrame> lstsf = st.GetFrames().ToList();
            return lstsf;
        }


        static public string GetStackFrameInfo(StackFrame sf)
        {
            return String.Format("{0}@{1}:{2}", sf.GetMethod().Name, sf.GetFileName(), sf.GetFileLineNumber());
        }

        static public List<string> CallStackGetAllFramesInfo() { return CallStackGetAllFramesInfo(1, true); }
        static public List<string> CallStackGetAllFramesInfo(int skipFrames/*=1*/, bool fNeedFileInfo/*=true*/)
        {
            List<string> lststr = new List<string>();

            foreach (StackFrame sf in CallStackGetAllFrames(skipFrames + 1, fNeedFileInfo))
                lststr.Add(GetStackFrameInfo(sf));

            return lststr;
        }


        [Conditional("ARCH_INTEL")]
        static public void StackTrace(string strMessage)
        {
            using(FormStackTrace frm = new FormStackTrace(strMessage, DEBUG.CallStackGetAllFramesInfo(2, true).ToArray()))
                frm.ShowDialog();
        }

        [Conditional("ARCH_INTEL")]
        static public void HandleException(System.Exception ex)
        {
            StackTrace st = new StackTrace(true);
            string key = GetStackFrameInfo(st.GetFrame(1));
            if (FormStackTrace.Contains(key))
                return;
                
            using(FormStackTrace frm = new FormStackTrace(ex, DEBUG.CallStackGetAllFramesInfo(2, true).ToArray()))
            {
                if (DialogResult.OK == frm.ShowDialog() && frm.cbSkipSimiliar.Checked)
                    FormStackTrace.AddHistoryOnDemand(key);
            }
        }

        /// <summary>
        /// CheckRecursive() 를 호출한 함수가 재귀 호출되었는지의 여부를 판단한다.
        /// </summary>
        /// <param name="nRangeDown"></param>
        /// <returns></returns>
        static public bool CheckRecursive(int nRangeDown/*=-1*/)
        {
            List<string> lstSf = DEBUG.CallStackGetAllFramesInfo(0, false).ToList();
            string strFunctionName = lstSf[1];      // lstSf[0] 는 CheckRecursive 함수 자신임.
            if ( nRangeDown < 0 )
                nRangeDown = lstSf.Count-2;
            string sub = lstSf.GetRange(2, nRangeDown).Find(s => { return s.Contains(strFunctionName); });
            bool bRecursive = !string.IsNullOrEmpty(sub);

            return bRecursive;
        }
    }
}
