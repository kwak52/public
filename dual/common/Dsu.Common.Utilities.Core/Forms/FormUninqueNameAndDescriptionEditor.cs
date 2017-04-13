using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.Forms
{
    public partial class FormUninqueNameAndDescriptionEditor : Form
    {
        public string Title { get { return Text; } set { Text = value; } }
        public FilterableTextBox NameTextBox { get { return filterableTextBoxName; } }
        public string UniqueName { get { return filterableTextBoxName.Text; } set { filterableTextBoxName.Text = value; } }
        public string Note { get { return textBoxNote.Text; } set { textBoxNote.Text = value; } }

        public string OKButtonText { get { return btnOK.Text; } set { btnOK.Text = value; } }

        public bool EmbeddedMode
        {
            get { return _embeddedMode; }
            set
            {
                _embeddedMode = value;
                btnOK.Visible = false;
                Height += (_embeddedMode ? -1 : +1 ) * btnOK.Height;
            }
        }

        private bool _embeddedMode;

        public FormUninqueNameAndDescriptionEditor()
        {
            InitializeComponent();
            filterableTextBoxName.SetAlphaNumericFilter(addition: "_");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
