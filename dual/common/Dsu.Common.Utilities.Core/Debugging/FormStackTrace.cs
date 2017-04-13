using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dsu.Common.Utilities
{
    public partial class FormStackTrace : FormOKCancel
    {
        private object m_oException = null;         // string or Exception object
        private object[] m_oStackFrames = null;

        static private HashSet<string> m_setHistory = new HashSet<string>();
        static public bool Contains(string msg) { return m_setHistory.Contains(msg);}
        static public bool AddHistoryOnDemand(string msg)
        {
            if (Contains(msg))
                return false;

            m_setHistory.Add(msg);
            return true;
        }

        public FormStackTrace(object ex, object[] aStackFrame)
        {
            Debug.Assert(ex is Exception || ex is string);
            if (aStackFrame == null)
                m_oStackFrames = DEBUG.CallStackGetAllFramesInfo(2, true).ToArray();
            else
                m_oStackFrames = aStackFrame;

            m_oException = ex;

            InitializeComponent();
            InitializeFormOKCancel(btnOK, null);
        }

        private void FormStackTrace_Load(object sender, EventArgs e)
        {
            Exception ex = m_oException as Exception;
            if (ex == null)
            {
                editMessage.Text = m_oException as string;
                Text = "Stack Trace";
            }
            else
            {
                editMessage.Text = ex.Message;
                Text = "Exception Stack Trace";
            }

            editMessage.Enabled = false;

            lbStackTrace.Items.AddRange(m_oStackFrames);
        }
    }
}
