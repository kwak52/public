using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.Forms
{
    public partial class FormSimpleEditor : Form
    {
        public string Title { get { return Text; } set { Text = value; } }
        public string Contents { get { return textBox1.Text; } set { textBox1.Text = value; } }

        public string OKButtonText { get { return btnOK.Text; } set { btnOK.Text = value; } }

        public TextBox TextBox { get { return textBox1; } }

        public bool ReadOnly { get { return textBox1.ReadOnly; } set { textBox1.ReadOnly = value; } }

        public bool Multiline
        {
            get { return textBox1.Multiline; }
            set
            {
                if (textBox1.Multiline != value)
                {
                    var backup = textBox1.Height;
                    textBox1.Multiline = value;
                    textBox1.Height = value ? _multilineHeight : 20;
                    Height -= backup - textBox1.Height;
                }
            }
        }

        private int _multilineHeight;

        public FormSimpleEditor()
        {
            InitializeComponent();
            _multilineHeight = textBox1.Height;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
