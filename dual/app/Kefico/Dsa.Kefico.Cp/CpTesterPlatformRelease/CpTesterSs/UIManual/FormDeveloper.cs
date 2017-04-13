using Dsu.Common.Utilities;
using System;
using System.Windows.Forms;

namespace CpTesterPlatform.CpTester
{
    public partial class FormDeveloper : Form
    {
        public FormDeveloper()
        {
            InitializeComponent();
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            Globals.IsDeveloperMode = textEdit_Value.Text == "1";
            if (Globals.IsDeveloperMode)
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

            return new FormDeveloper().ShowDialog() == DialogResult.OK;
        }

        private void simpleButton_LayoutInitial_Click(object sender, EventArgs e)
        {
            Globals._isSkipLoadLayout = true;
        }
    }
}
