using Dsu.Common.Utilities;
using System;
using System.Windows.Forms;

namespace CpTesterPlatform.CpTester
{
    public partial class FormAdmin : Form
    {
        public bool AdminLogin { get; set; } = false;
        public FormAdmin()
        {
            InitializeComponent();
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            if (textEdit_Value.Text == "1")
                DialogResult = DialogResult.OK;
            else
                textEdit_Value.Text = "Error";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        public static bool DoModal()
        {
            if (Globals.IsDeveloperMode)
                return true;

            return new FormAdmin().ShowDialog() == DialogResult.OK;
        }
    }
}
