using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities.Forms
{
    /// <summary>
    /// IDocument 의 속성을 편집하기 위한 form
    /// </summary>
    public partial class FormDocumentProperty : Form
    {
        public FormDocumentProperty()
        {
            InitializeComponent();
        }

        public IDocument Document
        {
            get
            {
                return _document;
            }
            set
            {
                Contract.Requires(value != null);
                if (_document != value)
                {
                    _document = value;
                    textBoxName.Text = _document.Name;
                    textBoxNote.Text = _document.Note;
                    textBoxProgramNote.Text = _document.ProgramNote;
                    textBoxGuid.Text = _document.GuidString;
                }
            }
        }

        private IDocument _document;

        private void FormDocumentProperty_Load(object sender, System.EventArgs e)
        {
            textBoxName.ReadOnly = true;
            textBoxProgramNote.ReadOnly = true;
            textBoxGuid.ReadOnly = true;
        }

        private void FormDocumentProperty_FormClosing(object sender, FormClosingEventArgs e)
        {
            _document.Name = textBoxName.Text;
            _document.GuidString = textBoxGuid.Text;
            _document.ProgramNote = textBoxProgramNote.Text;
            _document.Note = textBoxNote.Text;
        }

        private void btnNewGuid_Click(object sender, System.EventArgs e)
        {
            textBoxGuid.Text = Guid.NewGuid().ToString();
        }

        public void AddPage(TabPage page)
        {
            tabControl1.TabPages.Add(page);
        }
    }
}
