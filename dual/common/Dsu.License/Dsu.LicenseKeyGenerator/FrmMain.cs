using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DsuLicenseGenerator
{
    public partial class FrmMain : Form
    {

        #region Member Variables


        #endregion


        #region Initialize/Dispose

        public FrmMain()
        {
            InitializeComponent();
        }

        #endregion


        #region Private Methods


        #endregion


        #region Event Methods

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //SentinelLicLib.SentinelLicenseServer sentinelLicenseServer = new SentinelLicLib.SentinelLicenseServer();

            //if (!sentinelLicenseServer.QValidLicense_p("VMSS"))
            //{
            //    MessageBox.Show("License ERROR : Check VMSS USB Key!!");

            //    Process.GetCurrentProcess().Kill();
            //}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtActivationCodeFull.Text.Trim() != "")
            {
                string sCode = txtActivationCodeFull.Text.Trim();
                txtActivationKeyFull.Text = CLicenseGenerator.GenerateActivationKeyFull(sCode);
            }

            if (txtActivationCodeDemo.Text.Trim() != "")
            {
                string sCode = txtActivationCodeDemo.Text.Trim();

                int sDays = int.Parse(txtDays.Text.Trim());
                int sHours = int.Parse(txtHours.Text.Trim());
                txtActivationKeyDemo.Text = CLicenseGenerator.GenerateActivationKeyDemo(sCode, sDays, sHours);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }

        #endregion
    }
}
