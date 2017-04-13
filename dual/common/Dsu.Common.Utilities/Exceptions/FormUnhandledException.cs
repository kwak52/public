using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.Exceptions
{
    public partial class FormUnhandledException: Form
    {

        public FormUnhandledException()
        {
            InitializeComponent();
        }

        private void UnhandledExDlgForm_Load(object sender, EventArgs e)
        {
            buttonNotSend.Focus();
            labelExceptionDate.Text = String.Format(labelExceptionDate.Text, DateTime.Now);
            linkLabelData.Left = labelLinkTitle.Right;
        }

    }
}