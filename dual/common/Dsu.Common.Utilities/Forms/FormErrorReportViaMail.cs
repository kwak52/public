﻿using System;
using System.Net.Mail;
using System.Windows.Forms;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.Core;
namespace Dsu.Common.Utilities.Forms
{
    /// <summary>
    /// 오류 발생시, 메일을 통해 report 하기 위한 form
    /// </summary>
    public partial class FormErrorReportViaMail : Form
    {
        public string Title { get { return textBoxTitle.Text; } set { textBoxTitle.Text = value; } }
        public string To { get { return textBoxTo.Text; } set { textBoxTo.Text = value; } }
        public string Cc { get { return textBoxCc.Text; } set { textBoxCc.Text = value; } }
        public string ReplyTo { get { return textBoxReplyTo.Text; } set { textBoxReplyTo.Text = value; } }
        public string Contents { get { return textBoxContents.Text; } set { textBoxContents.Text = value; } }
        public string AutoGeneratedContents { get { return textBoxAutoGeneratedContents.Text; } set { textBoxAutoGeneratedContents.Text = value; } }

        public SmtpClient SmtpClient { get; set; }

        public FormErrorReportViaMail()
        {
            InitializeComponent();
        }

        private void FormErrorReportViaMail_Load(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                // UseWaitCursor 나 Application.UseWaitCursor 는 click event 내에서 변경이 안된다.
                // Cursor.Current 를 변경
                using (new TextCursor("Sending email.."))
                {
                    var message = new MailMessage(ReplyTo, To, Title, Contents + AutoGeneratedContents);
                    SmtpClient.Send(message);
                    //new PopupToolTip("Joint sensor").Show("Report Sent!!!", btnSend);
                    AutoClosingMessageBox.Show("Report Sent!!!", 3000, "DONE");
                    Close(); 
                }
            }
            catch (Exception ex)
            {
                var msg = "Failed to send error report.\r\n" + ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show(msg, "Failed");
                ExceptionHider.SwallowException(ex);
            }
        }
    }
}
