using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.Exceptions
{
    public partial class FormException: Form
    {
        public FormException()
        {
            InitializeComponent();
        }

        private void buttonException_Click(object sender, EventArgs e)
        {
            throw new ApplicationException("\nUI Thread Exception raised!");
        }        
    }
}