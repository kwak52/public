using DevExpress.XtraSplashForm;
using System;

namespace Dsa.Kefico.PDV
{
    public partial class frmSplash : SplashFormBase
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            marqueeProgressBarControl1.Text = "PDV Connecting to database...";
        }
    }
}
