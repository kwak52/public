using DevExpress.XtraSplashForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Dsa.Kefico.MWS
{
    public partial class frmSplash : SplashFormBase
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            marqueeProgressBarControl1.Text = "MWS Connecting to database...";
        }
    }
}
