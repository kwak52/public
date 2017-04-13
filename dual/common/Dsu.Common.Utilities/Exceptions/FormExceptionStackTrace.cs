using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.Forms
{
    /// <summary>
    /// Exceptin 발생시, stack trace 의 내용을 listview로 보여 주기 위한 form
    /// <para/> - 발생한 exception 의 type 및 message 정보 표시
    /// <para/> - listview double click 시 source file browsing. ListViewExceptionStackTrace 에서 구현됨.
    /// <para/> - listview single click 시, 해당 frame 내용 자세히 표시
    /// </summary>
    public partial class FormExceptionStackTrace : Form
    {
        private Exception _exception;
        public FormExceptionStackTrace(Exception exception)
        {
            InitializeComponent();
            _exception = exception;
        }

        private void FormExceptionStackTrace_Load(object sender, EventArgs e)
        {
            listViewExceptionStackTrace1.OpenFileOnDoubleClick = true;
            listViewExceptionStackTrace1.UseVisualStudioIDEIfApplicable = true;
            listViewExceptionStackTrace1.Initialize(_exception);
            Text = String.Format("Exception stack trace: {0}", _exception.GetType().Name);

            textBoxExceptionSummary.ForeColor = Color.Blue;
            textBoxExceptionSummary.Text = "Exception type: " + _exception.GetType().Name + "\r\n";
            textBoxExceptionSummary.Text += " - Exception message: " + _exception.Message + "\r\n";
            var aggregateException = _exception as AggregateException;
            if (aggregateException != null)
                textBoxExceptionSummary.Text += " - Aggregate inner exception: " + aggregateException.Flatten().InnerExceptions + "\r\n";
            if (_exception.InnerException != null)
                textBoxExceptionSummary.Text += " - InnerException: " + _exception.InnerException + "\r\n";
            if (_exception.Source != null)
                textBoxExceptionSummary.Text += " - Source: " + _exception.Source + "\r\n";
            if (_exception.TargetSite != null)
                textBoxExceptionSummary.Text += " - TargetSite: " + _exception.TargetSite + "\r\n";
        }

        private void listViewExceptionStackTrace1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewExceptionStackTrace1.SelectedItems.Count != 1)
            {
                textBoxFrameInfo.Text = String.Empty;
                return;
            }

            var frame = (StackFrame)listViewExceptionStackTrace1.SelectedItems[0].Tag;
            var method = frame.GetMethod();

            textBoxFrameInfo.Text = String.Empty;
            if (!String.IsNullOrEmpty(frame.GetFileName()))
                textBoxFrameInfo.Text += ("Filename: " + frame.GetFileName() + "\r\n");

            var type = method.ReflectedType ?? method.GetType();

            if (type != null)
            {
                textBoxFrameInfo.Text += ("Fullname: " + type.FullName + "\r\n");
                textBoxFrameInfo.Text += ("Namespace: " + type.Namespace + "\r\n");
                textBoxFrameInfo.Text += ("DeclaringType: " + type.DeclaringType + "\r\n");
                textBoxFrameInfo.Text += ("Assembly: " + type.Assembly + "\r\n");                
            }
        }
    }
}
