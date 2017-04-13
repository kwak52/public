using System;
using System.Windows.Forms;

namespace Dsu.LicenseKeyChecker
{
    internal partial class FrmLicense : Form
    {
        #region Member Variables

		private CLicenseInfo m_cInfo = null;

		private string m_sProduct = "";
		private string m_sCode = "";
        private int m_iTrialCount = 0;

        #endregion


        #region Initialize/Dispose

        internal FrmLicense(string sProduct, string sCode)
        {
            InitializeComponent();

			m_sProduct = sProduct;
			m_sCode = sCode;
        }

        #endregion


        #region Public Properties

		internal CLicenseInfo LicenseInfo
		{
			get { return m_cInfo; }
		}
       
        #endregion


        #region Public Methods


        #endregion


        #region Private Methods

        private void ShowInfo()
        {
			txtProduct.Text = m_sProduct;
            txtProduct.Refresh();

			txtActivationCode.Text = m_sCode;
			txtActivationKey.Refresh();
        }

        private void CheckKey()
        {   
            string sKeyInput = txtActivationKey.Text.Trim();
			CLicenseInfo cInfo = CLicenseActivator.AnalyseKey(sKeyInput);
			if (cInfo == null)
				return;

			if (m_sCode == cInfo.ActivationCode)
			{
				m_cInfo = cInfo;
				m_cInfo.Product = m_sProduct;
				m_cInfo.IsLicensed = true;

				if (m_cInfo.IsDemo)
				{
				//	MessageBox.Show("[" + txtProduct.Text + "] Trial License Activated!!", "Dsu License", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
					this.Close();
				}
				else
				{
				//	MessageBox.Show("[" + txtProduct.Text + "] Full License Activated!!", "Dsu License", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
					this.Close();
				}
			}

			cInfo = null;

			m_iTrialCount++;
			if (m_iTrialCount > 2)
			{
				m_iTrialCount = 0;

				MessageBox.Show("You entered the wrong license key", "Dsu License", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				this.Close();
			}
			else
			{
				//lblAlertMessage.Text = "The Activation Key is not correct!! [" + m_iTrialCount + " / 3]";
			}
        }

        #endregion


        #region Event Methods

        private void FrmLicense_Load(object sender, EventArgs e)
        {
			ShowInfo();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CheckKey();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
			m_cInfo = new CLicenseInfo();
			m_cInfo.Product = m_sProduct;
			m_cInfo.ActivationCode = m_sCode;

            this.Close();
        }

        private void txtActivationKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOK_Click(this, EventArgs.Empty);
        }

        #endregion

        
    }
}
