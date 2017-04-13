using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Dsu.Common.Utilities.Exceptions;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// http://www.codeproject.com/Articles/286265/Reduce-debugging-time-using-the-NET-StackTrace-cla
    /// Exception 의 stack trace 내용을 list view 로 표현하기 위한 class.
    /// - double click 시 source file browsing
    ///  * Visual studio 이용하거나, (이때, 해당 라인으로 tracing)
    ///  * 일반 editor 이용
    /// </summary>
    public class ListViewExceptionStackTrace : ListView
    {
        private Dictionary<string, int> _columnWidthMap = new Dictionary<string, int>()
            {
                {"Location", 200},
                {"File", 200},
                {"Line", 60},
                {"Column", 60},
            };

        private Exception _exception;
        public bool OpenFileOnDoubleClick { get; set; }        
        public bool UseVisualStudioIDEIfApplicable { get; set; }

        public void Initialize(Exception exception)
        {
            View = View.Details;
            FullRowSelect = true;
            GridLines = true;
            ForeColor = Color.Blue;

            _exception = exception;
            foreach (var pr in _columnWidthMap)
                Columns.Add(pr.Key, pr.Value);

            var trace = new System.Diagnostics.StackTrace(exception, true);
            foreach (var frame in trace.GetFrames())
            {
                var method = frame.GetMethod();
                var reflected = method.ReflectedType;
                var file = Path.GetFileName(frame.GetFileName());
                var line = frame.GetFileLineNumber();
                var col = frame.GetFileColumnNumber();
                ListViewItem lvi;
                if ( String.IsNullOrEmpty(file) )
                    lvi = new ListViewItem(new string[]{method.Name, "-", "-", "-"});
                else
                    lvi = new ListViewItem(new string[] { method.Name, file, line.ToString(), col.ToString() });
                lvi.Tag = frame;
                Items.Add(lvi);
            }

            DoubleClick += OnDoubleClick;
        }

        ~ListViewExceptionStackTrace()
        {
            DoubleClick -= OnDoubleClick;
        }


        /// <summary> Stack frame double click 시, 해당 source 로 이동 </summary>
        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (OpenFileOnDoubleClick && SelectedItems.Count == 1)
            {
                var frame = (StackFrame)SelectedItems[0].Tag;
                var file = frame.GetFileName();
                var line = frame.GetFileLineNumber();
                var column = frame.GetFileColumnNumber();

                if (String.IsNullOrEmpty(file))
                    return;

                if (!File.Exists(file))
                {
                    //new PopupToolTip("Error"){ToolTipIcon = ToolTipIcon.Error}.Show(String.Format("file {0} not found.", file), this, 3000);
                    AutoClosingMessageBox.Show(String.Format("file {0} not found.", file), 3000);
                    return;                    
                }

                bool bSucceeded = false;
                try
                {
                    if (UseVisualStudioIDEIfApplicable)
                    {
                        foreach (var vsVersion in new[] { "VisualStudio.DTE.12.0", "VisualStudio.DTE.11.0",})
                        {
                            ExceptionHider.DoSilently(() =>
                            {
                                // Get an instance of the currently running Visual Studio IDE.
                                // https://msdn.microsoft.com/en-us/library/vstudio/68shb4dw%28v=vs.110%29.aspx
                                var dte2 =
                                    (EnvDTE80.DTE2) System.Runtime.InteropServices.Marshal.GetActiveObject(vsVersion);
                                if (dte2 != null)
                                {
                                    EnvDTE.ProjectItem projItem = dte2.Solution.FindProjectItem(file);
                                    if (null != projItem)
                                    {
                                        projItem.Open(EnvDTE.Constants.vsViewKindCode).Activate();
                                        var textSelection = (EnvDTE.TextSelection) dte2.ActiveDocument.Selection;
                                        textSelection.MoveToLineAndOffset(line, column, true);
                                        textSelection.SelectLine();
                                        bSucceeded = true;
                                    }
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHider.SwallowException(ex);
                }

                /*
                 * VisualStudio 를 이용해서 file open 에 실패한 경우 : NotePad 나 기타 사용자 정의 editor 로 file 을 open 한다.
                 */
                if (!bSucceeded)
                    Process.Start(file);
            }
        }

    }
}
