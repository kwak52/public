using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public partial class FormOKCancelOnCloseDisposable : FormOnCloseDisposable
    {
        private Button m_btnOK = null;
        private Button m_btnCancel = null;

        public FormOKCancelOnCloseDisposable()
        {
            InitializeComponent();
        }

        public void InitializeFormOKCancel(Button btnOK, Button btnCancel)
        {
            m_btnOK = btnOK;
            m_btnCancel = btnCancel;

            if (btnOK != null)
                m_btnOK.Click += new System.EventHandler(this.OnOK);

            if (btnCancel != null)
                m_btnCancel.Click += new System.EventHandler(this.OnCancel);
        }

        protected virtual void OnOK(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected virtual void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormOKCancelOnCloseDisposable_Load(object sender, EventArgs e)
        {
            if (m_btnOK != null)
                m_btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            if (m_btnCancel != null)
                m_btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }
    }
}
